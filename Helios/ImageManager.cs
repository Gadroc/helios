//  Copyright 2014 Craig Courtney
//    
//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Helios is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace GadrocsWorkshop.Helios
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// ImageManager gives access to loading and resizing images from the appropriate locations.
    /// </summary>
    public class ImageManager : IImageManager
    {
        private string _documentImagePath;

        internal ImageManager(string userImagePath)
        {
            ConfigManager.LogManager.Log("ImageManager Intialisation: " + userImagePath);
            _documentImagePath = userImagePath;
        }

        /// <summary>
        /// Loads an image file iterating through the profile subdirectories.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ImageSource LoadImage(string path)
        {
            Uri imageUri = GetImageUri(path);

            if (imageUri != null)
            {
                if (path.EndsWith(".xaml"))
                {
                    Stream xamlStream;

                    if (imageUri.Scheme.Equals("pack"))
                    {
                        xamlStream = Application.GetResourceStream(imageUri).Stream;
                    }
                    else
                    {
                        xamlStream = WebRequest.Create(imageUri).GetResponse().GetResponseStream();
                    }

                    if (xamlStream != null)
                    {
                        using (xamlStream)
                        {
                            Canvas canvas = (Canvas)XamlReader.Load(xamlStream);
                            RenderTargetBitmap render = new RenderTargetBitmap((int)canvas.Width, (int)canvas.Height, 96d, 96d, PixelFormats.Pbgra32);
                            canvas.Measure(new Size(canvas.Width, canvas.Height));
                            canvas.Arrange(new Rect(new Size(canvas.Width, canvas.Height)));
                            render.Render(canvas);
                            return render;
                        }
                    }
                }
                else
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnDemand;
                    image.CreateOptions = BitmapCreateOptions.DelayCreation;
                    image.UriSource = imageUri;
                    image.EndInit();
                    return image;
                }
            }

            return null;
        }

        /// <summary>
        /// Loads an image file iterating through the profile subdirectories.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ImageSource LoadImage(string path, int width, int height)
        {
            Uri imageUri = GetImageUri(path);

            if (imageUri != null)
            {
                if (path.EndsWith(".xaml"))
                {
                    Stream xamlStream;

                    if (imageUri.Scheme.Equals("pack"))
                    {
                        xamlStream = Application.GetResourceStream(imageUri).Stream;
                    }
                    else
                    {
                        xamlStream = WebRequest.Create(imageUri).GetResponse().GetResponseStream();
                    }

                    if (xamlStream != null)
                    {
                        using (xamlStream)
                        {
                            RenderTargetBitmap render = new RenderTargetBitmap(Math.Max(1, width), Math.Max(1, height), 96d, 96d, PixelFormats.Pbgra32);
                            Canvas canvas = (Canvas)XamlReader.Load(xamlStream);
                            canvas.RenderTransform = new ScaleTransform(width / canvas.Width, height / canvas.Height);
                            canvas.Measure(new Size(canvas.Width, canvas.Height));
                            canvas.Arrange(new Rect(new Size(canvas.Width, canvas.Height)));
                            render.Render(canvas);
                            return render;
                        }
                    }
                }
                else
                {
                    BitmapImage image = null;

                    image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnDemand;
                    image.CreateOptions = BitmapCreateOptions.DelayCreation;
                    image.UriSource = imageUri;
                    image.DecodePixelWidth = Math.Max(1, width);
                    image.DecodePixelHeight = Math.Max(1, height);
                    image.EndInit();

                    return image;
                }
            }

            return null;
        }

        private Uri GetImageUri(string path)
        {
            if (path != null)
            {
                if (path.StartsWith("pack:") || path.StartsWith("{"))
                {
                    string packPath = MakeImagePathAbsolute(path);
                    Uri imageUri = new Uri(packPath, UriKind.Absolute);

                    if (CanOpenPackUri(imageUri))
                    {
                        return imageUri;
                    }
                }
                else if (path.StartsWith("http:", true, CultureInfo.CurrentCulture) ||
                    path.StartsWith("https:", true, CultureInfo.CurrentCulture) ||
                    path.StartsWith("ftp:", true, CultureInfo.CurrentCulture) ||
                    path.StartsWith("file:", true, CultureInfo.CurrentCulture))
                {
                    return new Uri(path);
                }
                else
                {
                    string filePath = MakeImagePathAbsolute(path);
                    if (filePath != null && filePath.Length > 0)
                    {
                        return new Uri("file://" + filePath);
                    }
                }
            }
            return null;
        }

        public string MakeImagePathRelative(string filename)
        {
            string newFilename = filename;

            if (filename.Length > 0)
            {
                if (filename.StartsWith("pack://application:,,,/"))
                {
                    int closingIndex = filename.IndexOf(";component");
                    if (closingIndex > -1)
                    {
                        string assembly = filename.Substring(23, closingIndex - 23);
                        newFilename = "{" + assembly + "}" + filename.Substring(closingIndex + 10);
                    }
                }
                else
                {
                    string fullFilename = Path.GetFullPath(filename);
                    if (fullFilename.StartsWith(ConfigManager.ImagePath, StringComparison.CurrentCulture))
                    {
                        newFilename = fullFilename.Substring(ConfigManager.ImagePath.Length + 1);
                    }
                }
            }

            return newFilename;
        }

        public string MakeImagePathAbsolute(string fileName)
        {
            string loadName;

            if (fileName == null || fileName.Length == 0)
            {
                return null;
            }

            if (fileName.StartsWith("pack:"))
            {
                return fileName;
            }

            // If the file is an absolute position.
            if (Path.IsPathRooted(fileName))
            {
                if (File.Exists(fileName))
                {
                    return fileName;
                }
            }
            else
            {
                if (fileName.StartsWith("{"))
                {
                    int closingIndex = fileName.IndexOf('}');
                    if (closingIndex > -1)
                    {
                        string assembly = fileName.Substring(1, closingIndex - 1);
                        return "pack://application:,,,/" + assembly + ";component" + fileName.Substring(closingIndex + 1);
                    }
                }

                // First check the users images directory
                loadName = Path.Combine(_documentImagePath, fileName);
                if (File.Exists(loadName))
                {
                    return loadName;
                }
            }

            return "";
        }

        public static bool CanOpenPackUri(Uri uri)
        {
            try
            {
                Stream resourceStream = Application.GetResourceStream(uri).Stream;
                resourceStream.Close();
                return true;
            }
            catch (Exception e)
            {
                ConfigManager.LogManager.LogError("Error loading image. (Filename=\"" + uri.ToString() + "\")", e);
                return false;
            }
        }

    }
}

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

namespace GadrocsWorkshop.Helios.Windows.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Win32;
 
    /// <summary>
    /// Interaction logic for ImagePicker.xaml
    /// </summary>
    public partial class ImagePicker : UserControl
    {
        public ImagePicker()
        {
            InitializeComponent();
        }

        #region Properties

        public string ImageFilename
        {
            get { return (string)GetValue(ImageFilenameProperty); }
            set { SetValue(ImageFilenameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageFilename.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageFilenameProperty =
            DependencyProperty.Register("ImageFilename", typeof(string), typeof(ImagePicker), new UIPropertyMetadata(""));

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DereferenceLinks = true;
            ofd.Multiselect = false;
            ofd.ValidateNames = true;
            ofd.Filter = "Images Files (*.gif, *.jpg, *.jpe, *.png, *.bmp, *.dib, *.tif, *.wmf, *.pcx, *.tga)|*.gif;*.jpg;*.jpe;*.png;*.bmp;*.dib;*.tif;*.wmf;*.pcx;*.tga";
            ofd.Title = "Select Image";

            ofd.CustomPlaces.Add(new FileDialogCustomPlace(ConfigManager.ImagePath));
            
            string path = ConfigManager.ImageManager.MakeImagePathAbsolute(ImageFilename);

            if (path == null || path.Length == 0 || !path.StartsWith(ConfigManager.ImagePath))
            {
                ofd.InitialDirectory = ConfigManager.ImagePath;
            }
            else
            {
                ofd.InitialDirectory = System.IO.Path.GetDirectoryName(path);
                ofd.FileName = path;
            }

            Nullable<bool> result = ofd.ShowDialog(Window.GetWindow(this));

            if (result == true)
            {
                ImageFilename = ConfigManager.ImageManager.MakeImagePathRelative(ofd.FileName);
            }
        }

    }
}

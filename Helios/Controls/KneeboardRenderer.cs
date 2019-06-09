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

namespace GadrocsWorkshop.Helios.Controls
{
	using System.Collections.Generic;
	using System.Windows;
    using System.Windows.Media;

    public class KneeBoardRenderer : HeliosVisualRenderer
    {
        private ImageSource _image;
        private Rect _imageRect;
        private ImageBrush _imageBrush;
        private Pen _borderPen;

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
           if (_image != null)
            {
				KneeBoard profileImage = Visual as KneeBoard;
                OnRefresh();
                drawingContext.DrawRoundedRectangle(_imageBrush, _borderPen, _imageRect, 0, 0);
            }
        }

        protected override void OnRender(DrawingContext drawingContext, double scaleX, double scaleY)
        {
            if (_image != null)
            {
				
				KneeBoard profileImage = Visual as KneeBoard;
                OnRefresh();

                Rect scaledRect = new Rect(_imageRect.X, _imageRect.Y, _imageRect.Width * scaleX, _imageRect.Height * scaleY);
                drawingContext.DrawRoundedRectangle(_imageBrush, _borderPen, scaledRect, 0, 0);
            }            
        }

        protected override void OnRefresh()
        {
            KneeBoard profileImage = Visual as KneeBoard;
            if (profileImage != null)
            {
                _image = ConfigManager.ImageManager.LoadImage(profileImage.Positions[profileImage._currentPosition].Name);
                _imageRect.Width = profileImage.Width;
                _imageRect.Height = profileImage.Height;
                

                    _borderPen = null;
                

                if (_image == null)
                {
                    _image = ConfigManager.ImageManager.LoadImage("{Helios}/Images/KneeBoards/default_kneeboard_image.png");
                    _imageBrush = new ImageBrush(_image);
                    _imageBrush.Stretch = Stretch.Uniform;
                    _imageBrush.TileMode = TileMode.None;
                    _imageBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
                    _imageBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                }
                else
                {
                    _imageBrush = new ImageBrush(_image);
                    switch (profileImage.Alignment)
                    {
                        case ImageAlignmentPlus.Centered:
                            _imageBrush.Stretch = Stretch.None;
                            _imageBrush.TileMode = TileMode.None;
                            _imageBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
                            _imageBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                            break;

                        case ImageAlignmentPlus.Stretched:
                            _imageBrush.Stretch = Stretch.Fill;
                            _imageBrush.TileMode = TileMode.None;
                            _imageBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
                            _imageBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                            break;

                        case ImageAlignmentPlus.Uniform:
                            _imageBrush.Stretch = Stretch.Uniform;
                            _imageBrush.TileMode = TileMode.None;
                            _imageBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
                            _imageBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                            break;

                        case ImageAlignmentPlus.Tiled:
                            _imageBrush.Stretch = Stretch.None;
                            _imageBrush.TileMode = TileMode.Tile;
                            _imageBrush.Viewport = new Rect(0d, 0d, _image.Width, _image.Height);
                            _imageBrush.ViewportUnits = BrushMappingMode.Absolute;
                            break;
                    }
                }
            }
            else
            {
                _image = null;
            }
        }
    }
}

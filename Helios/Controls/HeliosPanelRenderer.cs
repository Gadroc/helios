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
    using System;
    using System.Windows.Media;
    using System.Windows;

    public class HeliosPanelRenderer : HeliosVisualRenderer
    {
        private ImageSource _backgroundImage;
        private ImageBrush _backgroundBrush;

        private ImageSource _topBorderImage;
        private ImageSource _rightBorderImage;
        private ImageSource _bottomBorderImage;
        private ImageSource _leftBorderImage;

        private ImageSource _topLeftCornerImage;
        private ImageSource _topRigthCornerImage;
        private ImageSource _bottomLeftCornerImage;
        private ImageSource _bottomRightCornerImage;

        protected override void OnRender(DrawingContext drawingContext)
        {
            HeliosPanel panel = Visual as HeliosPanel;

            if (panel != null)
            {
                double width = panel.Width;
                double height = panel.Height;

                // TODO Subtract top/left/bottom/right border sizes from background draw (this will allow for rounded transparent corners).

                if (panel.FillBackground)
                {
                    drawingContext.DrawRectangle(new SolidColorBrush(panel.BackgroundColor), null, new Rect(0, 0, width, height));
                }

                if (_backgroundBrush != null)
                {
                    drawingContext.DrawRectangle(_backgroundBrush, null, new Rect(0, 0, width, height));
                }

                if (panel.DrawBorder)
                {

                    if (_topBorderImage != null)
                    {
                        drawingContext.DrawImage(_topBorderImage, new Rect(0, 0, width, _topBorderImage.Height));
                    }

                    if (_rightBorderImage != null)
                    {
                        drawingContext.DrawImage(_rightBorderImage, new Rect(width - _rightBorderImage.Width, 0, _rightBorderImage.Width, height));
                    }

                    if (_bottomBorderImage != null)
                    {
                        drawingContext.DrawImage(_bottomBorderImage, new Rect(0, height - _bottomBorderImage.Height, width, _bottomBorderImage.Height));
                    }

                    if (_leftBorderImage != null)
                    {
                        drawingContext.DrawImage(_leftBorderImage, new Rect(0, 0, _leftBorderImage.Width, height));
                    }

                    if (_topLeftCornerImage != null)
                    {
                        drawingContext.DrawImage(_topLeftCornerImage, new Rect(0, 0, _topLeftCornerImage.Width, _topLeftCornerImage.Height));
                    }

                    if (_topRigthCornerImage != null)
                    {
                        drawingContext.DrawImage(_topRigthCornerImage, new Rect(width - _topRigthCornerImage.Width, 0, _topRigthCornerImage.Width, _topRigthCornerImage.Height));
                    }

                    if (_bottomRightCornerImage != null)
                    {
                        drawingContext.DrawImage(_bottomRightCornerImage, new Rect(width - _bottomRightCornerImage.Width, height - _bottomRightCornerImage.Height, _bottomRightCornerImage.Width, _bottomRightCornerImage.Height));
                    }

                    if (_bottomLeftCornerImage != null)
                    {
                        drawingContext.DrawImage(_bottomLeftCornerImage, new Rect(0, height - _bottomLeftCornerImage.Height, _bottomLeftCornerImage.Width, _bottomLeftCornerImage.Height));
                    }
                }
            }
        }

        protected override void OnRefresh()
        {
            HeliosPanel panel = Visual as HeliosPanel;

            if (panel != null)
            {
                _backgroundImage = ConfigManager.ImageManager.LoadImage(panel.BackgroundImage);
                _backgroundBrush = null;
                if (_backgroundImage != null)
                {
                    _backgroundBrush = new ImageBrush(_backgroundImage);
                    switch (panel.BackgroundAlignment)
                    {
                        case ImageAlignment.Centered:
                            _backgroundBrush.Stretch = Stretch.None;
                            _backgroundBrush.TileMode = TileMode.None;
                            _backgroundBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
                            _backgroundBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                            break;

                        case ImageAlignment.Stretched:
                            _backgroundBrush.Stretch = Stretch.Fill;
                            _backgroundBrush.TileMode = TileMode.None;
                            _backgroundBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
                            _backgroundBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                            break;

                        case ImageAlignment.Tiled:
                            _backgroundBrush.Stretch = Stretch.None;
                            _backgroundBrush.TileMode = TileMode.Tile;
                            _backgroundBrush.Viewport = new Rect(0d, 0d, _backgroundImage.Width, _backgroundImage.Height);
                            _backgroundBrush.ViewportUnits = BrushMappingMode.Absolute;
                            break;
                    }
                }

                _topBorderImage = ConfigManager.ImageManager.LoadImage(panel.TopBorderImage);
                _rightBorderImage = ConfigManager.ImageManager.LoadImage(panel.RightBorderImage);
                _bottomBorderImage = ConfigManager.ImageManager.LoadImage(panel.BottomBorderImage);
                _leftBorderImage = ConfigManager.ImageManager.LoadImage(panel.LeftBorderImage);

                _topLeftCornerImage = ConfigManager.ImageManager.LoadImage(panel.TopLeftCornerImage);
                _topRigthCornerImage = ConfigManager.ImageManager.LoadImage(panel.TopRightCornerImage);
                _bottomLeftCornerImage = ConfigManager.ImageManager.LoadImage(panel.BottomLeftCornerImage);
                _bottomRightCornerImage = ConfigManager.ImageManager.LoadImage(panel.BottomRightCornerImage);
            }
        }
    }
}
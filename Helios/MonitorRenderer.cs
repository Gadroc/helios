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
    using System.Windows;
    using System.Windows.Media;

    public class MonitorRenderer : HeliosVisualRenderer
    {
        private Rect _monitorRect;
        private Brush _backgroundFillBrush;
        private ImageBrush _backgroundImageBrush;

        private bool _skipFill = false;

        public bool SkipFill
        {
            get { return _skipFill; }
            set { _skipFill = value; }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            if (!SkipFill && _backgroundFillBrush != null)
            {
                drawingContext.DrawRectangle(_backgroundFillBrush, null, _monitorRect);
            }

            if (_backgroundImageBrush != null)
            {
                drawingContext.DrawRectangle(_backgroundImageBrush, null, _monitorRect);
            }
        }

        protected override void OnRefresh()
        {
            Monitor display = Visual as Monitor;

            _monitorRect = new Rect(0d, 0d, Visual.Width, Visual.Height);
            _backgroundFillBrush = null;
            _backgroundImageBrush = null;

            if (display != null)
            {
                if (display.FillBackground)
                {
                    _backgroundFillBrush = new SolidColorBrush(display.BackgroundColor);
                }

                ImageSource backgroundImage = ConfigManager.ImageManager.LoadImage(display.BackgroundImage);
                if (backgroundImage != null)
                {
                    _backgroundImageBrush = new ImageBrush(backgroundImage);
                    switch (display.BackgroundAlignment)
                    {
                        case ImageAlignment.Centered:
                            _backgroundImageBrush.Stretch = Stretch.None;
                            _backgroundImageBrush.TileMode = TileMode.None;
                            _backgroundImageBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
                            _backgroundImageBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                            break;

                        case ImageAlignment.Stretched:
                            _backgroundImageBrush.Stretch = Stretch.Fill;
                            _backgroundImageBrush.TileMode = TileMode.None;
                            _backgroundImageBrush.Viewport = new Rect(0d, 0d, 1d, 1d);
                            _backgroundImageBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                            break;

                        case ImageAlignment.Tiled:
                            _backgroundImageBrush.Stretch = Stretch.None;
                            _backgroundImageBrush.TileMode = TileMode.Tile;
                            _backgroundImageBrush.Viewport = new Rect(0d, 0d, backgroundImage.Width, backgroundImage.Height);
                            _backgroundImageBrush.ViewportUnits = BrushMappingMode.Absolute;
                            break;
                    }
                }
            }
        }
    }
}

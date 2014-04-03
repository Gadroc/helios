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
    using System.Windows;
    using System.Windows.Media;

    public class IndicatorRenderer : HeliosVisualRenderer
    {
        private Indicator _indicator;

        private ImageSource _onImage;
        private ImageSource _offImage;

        private Brush _onBrush;
        private Brush _offBrush;

        private Rect _imageRect;

        protected override void OnPropertyChanged(Helios.ComponentModel.PropertyNotificationEventArgs args)
        {
            if (args.PropertyName.Equals("Visual"))
            {
                _indicator = args.NewValue as Indicator;
            }
            base.OnPropertyChanged(args);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            ImageSource image = _indicator.On ? _onImage : _offImage;
            if (image == null)
            {
                image = _indicator.On ? _offImage : _onImage;
            }

            drawingContext.DrawImage(image, _imageRect);
            _indicator.TextFormat.RenderText(drawingContext, _indicator.On ? _onBrush : _offBrush, _indicator.Text, _imageRect);
        }

        protected override void OnRefresh()
        {
            if (_indicator != null)
            {
                _imageRect.Width = _indicator.Width;
                _imageRect.Height = _indicator.Height;
                _onImage = ConfigManager.ImageManager.LoadImage(_indicator.OnImage);
                _offImage = ConfigManager.ImageManager.LoadImage(_indicator.OffImage);
                _onBrush = new SolidColorBrush(_indicator.OnTextColor);
                _offBrush = new SolidColorBrush(_indicator.OffTextColor);
            }
            else
            {
                _onImage = null;
                _offImage = null;
                _onBrush = null;
                _offBrush = null;
            }
        }
    }
}

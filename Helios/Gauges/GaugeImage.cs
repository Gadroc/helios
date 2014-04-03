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

namespace GadrocsWorkshop.Helios.Gauges
{
    using System.Windows;
    using System.Windows.Media;

    public class GaugeImage : GaugeComponent
    {
        private string _imageFile;
        private ImageSource _image;
        private Rect _rectangle;
        private Rect _imageRetangle;

        public GaugeImage(string imageFile, Rect location)
        {
            _imageFile = imageFile;
            _rectangle = location;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawImage(_image, _imageRetangle);
        }

        protected override void OnRefresh(double xScale, double yScale)
        {
            _imageRetangle = new Rect(_rectangle.X * xScale, _rectangle.Y * yScale, _rectangle.Width * xScale, _rectangle.Height * yScale);
            _image = ConfigManager.ImageManager.LoadImage(_imageFile, (int)_imageRetangle.Width, (int)_imageRetangle.Height);
        }
    }
}

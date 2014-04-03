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

    public class GaugeRectangle : GaugeComponent
    {
        private Color _fillColor;
        private Rect _nativeRect;
        private Brush _brush;
        private Rect _renderRect;

        public GaugeRectangle(Color fillColor, Rect rectangle)
        {
            _fillColor = fillColor;
            _nativeRect = rectangle;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(_brush, null, _renderRect);
        }

        protected override void OnRefresh(double xScale, double yScale)
        {
            _brush = new SolidColorBrush(_fillColor);
            _renderRect = _nativeRect;
            _renderRect.Scale(xScale, yScale);
        }
    }
}

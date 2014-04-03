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

namespace GadrocsWorkshop.Helios.ProfileEditor
{
    using System;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    public class DragSelectionAdorner : Adorner
    {
        private readonly Pen SelectionRectPen = new Pen(Brushes.Teal, 1.5d);
        private readonly Brush SelectionBrush = new SolidColorBrush(Colors.Aquamarine);

        private Point _startLocation;
        private Point _endLocation;
        private Vector _dragVector;
        private Rect _rectangle = new Rect();

        public DragSelectionAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            SelectionBrush.Opacity = 0.5d;
        }

        public Point StartLocation
        {
            get { return _startLocation; }
            set { _startLocation = value; UpdateRect(); }
        }

        public Point EndLocation
        {
            get { return _endLocation; }
        }


        public Vector DragVector
        {
            get { return _dragVector; }
            set { _dragVector = value; UpdateRect(); }
        }

        public Rect Rectangle
        {
            get { return _rectangle; }
        }

        private void UpdateRect()
        {
            _endLocation = StartLocation + DragVector;

            _rectangle.X = Math.Max(0d, (_startLocation.X < _endLocation.X) ? _startLocation.X : _endLocation.X);
            _rectangle.Y = Math.Max(0d, (_startLocation.Y < _endLocation.Y) ? _startLocation.Y : _endLocation.Y);
            _rectangle.Width = Math.Max(1d, Math.Abs(_startLocation.X - _endLocation.X));
            _rectangle.Height = Math.Max(1d, Math.Abs(_startLocation.Y - _endLocation.Y));

            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(SelectionBrush, SelectionRectPen, Rectangle);      
        }
    }
}

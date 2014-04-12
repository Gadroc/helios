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
    using System.Windows.Media;

    public class PointOfViewIndicator : Control
    {
        private Brush _activePositionBrush;
        private Brush _lineBrush;
        private Pen _linePen;
        private PathGeometry _arrowPath;

        public PointOfViewIndicator()
        {
            _activePositionBrush = Brushes.Red;
            _lineBrush = Brushes.DarkGray;
            _linePen = new Pen(_lineBrush, 1d);

            PathSegmentCollection segments = new PathSegmentCollection(4);
            segments.Add(new LineSegment(new Point(0, 10), false));
            segments.Add(new LineSegment(new Point(5, 0), false));
            segments.Add(new LineSegment(new Point(10, 10), false));
            segments.Add(new LineSegment(new Point(5, 5), false));

            PathFigureCollection figures = new PathFigureCollection();
            figures.Add(new PathFigure(new Point(5, 5), segments, true));

            _arrowPath = new PathGeometry(figures);
        }

        #region Properties

        public POVDirection Direction
        {
            get { return (POVDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(POVDirection), typeof(PointOfViewIndicator), new FrameworkPropertyMetadata(POVDirection.Center, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        protected override void OnRender(DrawingContext drawingContext)
        {
            Point povCenter = new Point(ActualHeight / 2d, ActualWidth / 2d);

            drawingContext.DrawEllipse(null, _linePen, povCenter, povCenter.X - 5d, povCenter.Y - 5d);
            drawingContext.DrawEllipse(_lineBrush, null, povCenter, 5, 5);

            if (Direction == POVDirection.Center)
            {
                drawingContext.DrawEllipse(_activePositionBrush, null, povCenter, 4, 4);
            }
            else
            {
                drawingContext.PushTransform(new RotateTransform((double)Direction, povCenter.X, povCenter.Y));
                drawingContext.PushTransform(new TranslateTransform(povCenter.X - 5, 0));
                drawingContext.DrawGeometry(_activePositionBrush, null, _arrowPath);
                drawingContext.Pop();
                drawingContext.Pop();
            }
        }
    }
}

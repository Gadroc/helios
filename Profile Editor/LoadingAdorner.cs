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
    using System.Windows.Shapes;

    public class LoadingAdorner : Adorner
    {
        private VisualCollection _children;

        private Rectangle _dimRectangle;
        private CircularProgressBar _progress;

        public LoadingAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            _children = new VisualCollection(this);

            Brush dimBrush = new SolidColorBrush(Colors.DarkGray);
            dimBrush.Opacity = 0.4d;

            _dimRectangle = new Rectangle();
            _dimRectangle.Fill = dimBrush;

            _progress = new CircularProgressBar();
            _progress.Width = 200;
            _progress.Height = 200;

            _children.Add(_dimRectangle);
            _children.Add(_progress);
        }

        #region Visual Methods

        protected override int VisualChildrenCount
        {
            get
            {
                return _children.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size resultSize = AdornedElement.DesiredSize;

            resultSize.Width = double.IsPositiveInfinity(constraint.Width) ? resultSize.Width : constraint.Width;
            resultSize.Height = double.IsPositiveInfinity(constraint.Height) ? resultSize.Height : constraint.Height;

            return resultSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {

            _dimRectangle.Arrange(new Rect(finalSize));
            _progress.Arrange(new Rect((finalSize.Width / 2d) - 100d, (finalSize.Height / 2d) - 100d, 200d, 200d));

            return finalSize;
        }

        #endregion
    }
}

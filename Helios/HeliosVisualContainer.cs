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
    using System;
    using System.Windows;

    public abstract class HeliosVisualContainer : HeliosVisual
    {
        // private HeliosVisual _mouseDownVisual;

        public HeliosVisualContainer(string name, Size nativeSize)
            : base(name, nativeSize)
        {
        }

        public override void Reset()
        {
            base.Reset();
            foreach(HeliosVisual child in Children)
            {
                child.Reset();
            }
        }

        public override void ScaleChildren(double scaleX, double scaleY)
        {
            foreach (HeliosVisual visual in Children)
            {
                if (visual.Left > 0)
                {
                    double locXDif = visual.Left;
                    visual.Left += (locXDif * scaleX) - locXDif;
                }

                if (visual.Top > 0)
                {
                    double locYDif = visual.Top;
                    visual.Top += (locYDif * scaleY) - locYDif;
                }

                visual.Width = Math.Max(visual.Width * scaleX, 1d);
                visual.Height = Math.Max(visual.Height * scaleY, 1d);

                visual.ScaleChildren(scaleX, scaleY);
            }
        }

        //public override bool MouseDown(Point location)
        //{
        //    Point transformedPoint = TransformLocation(location);
        //    foreach (HeliosVisual visual in Children.Reverse())
        //    {
        //        if (!visual.IsHidden && visual.DisplayRectangle.Contains(transformedPoint))
        //        {
        //            Point visualPoint = transformedPoint;
        //            visualPoint.Offset(-visual.Left, -visual.Top);
        //            if (visual.MouseDown(visualPoint))
        //            {
        //                _mouseDownVisual = visual;
        //                break;
        //            }
        //        }
        //    }
        //    return true;
        //}

        //public override void MouseDrag(Point location)
        //{
        //    if (_mouseDownVisual != null)
        //    {
        //        Point visualPoint = TransformLocation(location);
        //        visualPoint.Offset(-_mouseDownVisual.Left, -_mouseDownVisual.Top);
        //        _mouseDownVisual.MouseDrag(visualPoint);
        //    }
        //}

        //public override void MouseUp(Point location)
        //{
        //    if (_mouseDownVisual != null)
        //    {
        //        Point visualPoint = TransformLocation(location);
        //        visualPoint.Offset(-_mouseDownVisual.Left, -_mouseDownVisual.Top);
        //        _mouseDownVisual.MouseUp(visualPoint);
        //    }
        //    _mouseDownVisual = null;
        //}
    }
}

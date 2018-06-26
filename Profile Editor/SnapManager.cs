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
    using System.Collections.Generic;
    using System.Windows;

    public class SnapManager : NotificationObject
    {
        private Rect _bounds;

        private double _snapThreshold = 10d;
        private Vector _locationOffset;

        private SnapAction _snapMode;
        private List<SnapTarget> _targets = new List<SnapTarget>();

        private Vector _dragVector;

        private bool _ignoreTargets;

        private Rect _originalRectangle;
        private Rect _newRectangle;

        private bool _forceProportions = false;

        private bool _validDrag = false;

        #region Properties

        public bool ForceProportions
        {
            get
            {
                return _forceProportions;
            }
            set
            {
                if (!_forceProportions.Equals(value))
                {
                    bool oldValue = _forceProportions;
                    _forceProportions = value;
                    OnPropertyChanged("ForceProportions", oldValue, value, true);
                }
            }
        }

        public bool IsValidDrag
        {
            get
            {
                return _validDrag;
            }
            private set
            {
                if (!_validDrag.Equals(value))
                {
                    bool oldValue = _validDrag;
                    _validDrag = value;
                    OnPropertyChanged("IsValidDrag", oldValue, value, true);
                }
            }
        }


        public double SnapThreshold
        {
            get
            {
                return _snapThreshold;
            }
            set
            {
                if (!_snapThreshold.Equals(value))
                {
                    double oldValue = _snapThreshold;
                    _snapThreshold = value;
                    OnPropertyChanged("SnapThreshold", oldValue, value, true);
                }
            }
        }

        /// <summary>
        /// Bounds which the snapobject must remain with in.
        /// </summary>
        public Rect Bounds
        {
            get
            {
                return _bounds;
            }
            set
            {
                if (!_bounds.Equals(value))
                {
                    Rect oldValue = _bounds;
                    _bounds = value;
                    OnPropertyChanged("Bounds", oldValue, value, false);
                    Calculate();
                }
            }
        }

        /// <summary>
        /// If set to true snap will only take into account grid and bounds.
        /// </summary>
        public bool IgnoreTargets
        {
            get
            {
                return _ignoreTargets;
            }
            set
            {
                if (!_ignoreTargets.Equals(value))
                {
                    bool  oldValue = _ignoreTargets;
                    _ignoreTargets = value;
                    OnPropertyChanged("IgnoreTargets", oldValue, value, false);
                }
            }
        }

        /// <summary>
        /// Collection of targets to snap to.
        /// </summary>
        public List<SnapTarget> Targets
        {
            get { return _targets; }
        }

        /// <summary>
        /// Current action being snaped.
        /// </summary>
        public SnapAction Action
        {
            get
            {
                return _snapMode;
            }
            set
            {
                if (!_snapMode.Equals(value))
                {
                    SnapAction oldValue = _snapMode;
                    _snapMode = value;
                    OnPropertyChanged("Action", oldValue, value, false);
                    Calculate();
                }
            }
        }

        /// <summary>
        /// Location offset for the current action.
        /// </summary>
        public Vector LocationOffset
        {
            get { return _locationOffset; }
        }


        /// <summary>
        /// Original location for the SnapObject
        /// </summary>
        public Point Location
        {
            get
            {
                return _originalRectangle.Location;
            }
            set
            {
                if (!_originalRectangle.Location.Equals(value))
                {
                    Point oldValue = _originalRectangle.Location;
                    _originalRectangle.Location = value;
                    OnPropertyChanged("Location", oldValue, value, false);
                    Calculate();
                }
            }
        }

        /// <summary>
        /// Original Size for the SnapObject
        /// </summary>
        public Size Size
        {
            get
            {
                return _originalRectangle.Size;
            }
            set
            {
                if (!_originalRectangle.Size.Equals(value))
                {
                    Size oldValue = _originalRectangle.Size;
                    _originalRectangle.Size = value;
                    OnPropertyChanged("Size", oldValue, value, false);
                    Calculate();
                }
            }
        }

        /// <summary>
        /// Vector representing the current mouse drag.
        /// </summary>
        public Vector DragVector
        {
            get
            {
                return _dragVector;
            }
            set
            {
                if (!_dragVector.Equals(value))
                {
                    Vector oldValue = _dragVector;
                    _dragVector = value;
                    OnPropertyChanged("DragVector", oldValue, value, false);
                    Calculate();
                }
            }
        }

        public Point NewLocation
        {
            get
            {
                return _newRectangle.Location;
            }
            set
            {
                if (!_newRectangle.Location.Equals(value))
                {
                    Point oldValue = _newRectangle.Location;
                    _newRectangle.Location = value;
                    _locationOffset = value - Location;
                    OnPropertyChanged("NewLocation", oldValue, value, false);
                    OnPropertyChanged("NewRectangle", null, null, false);
                    OnPropertyChanged("LocationOffset", null, null, false);
                }
            }
        }


        public Size NewSize
        {
            get
            {
                return _newRectangle.Size;
            }
            set
            {
                if (!_newRectangle.Size.Equals(value))
                {
                    Size oldValue = _newRectangle.Size;
                    _newRectangle.Size = value;
                    OnPropertyChanged("NewSize", oldValue, value, false);
                    OnPropertyChanged("NewRectangle", null, null, false);
                }
            }
        }


        public Rect NewRectangle
        {
            get
            {
                return _newRectangle;
            }
        }

        #endregion

        private void Calculate()
        {
            IsValidDrag = (Math.Abs(DragVector.X) > SystemParameters.MinimumHorizontalDragDistance ||
                           Math.Abs(DragVector.Y) > SystemParameters.MinimumVerticalDragDistance);

            double horizontalOffset = SnapThreshold;
            double verticalOffset = SnapThreshold;

            double widthOffset = SnapThreshold;
            double heightOffset = SnapThreshold;

            bool foundHorizontalSnap = false;
            bool foundVerticalSnap = false;

            Rect dragRectangle = CalculateDragRectangle();

            if (!IgnoreTargets)
            {
                foreach (SnapTarget target in Targets)
                {
                    if (Action == SnapAction.Move || Action == SnapAction.Drop || Action == SnapAction.LineEnd || Action == SnapAction.LineStart)
                    {
                        CheckLeftSnap(dragRectangle, target, ref horizontalOffset, ref foundHorizontalSnap);
                        CheckRightSnap(dragRectangle, target, ref horizontalOffset, ref foundHorizontalSnap);
                        CheckTopSnap(dragRectangle, target, ref verticalOffset, ref foundVerticalSnap);
                        CheckBottomSnap(dragRectangle, target, ref verticalOffset, ref foundVerticalSnap);

                        widthOffset = 0d;
                        heightOffset = 0d;
                    }
                    else
                    {                          
                        if (Action == SnapAction.ResizeW || Action == SnapAction.ResizeSW || Action == SnapAction.ResizeNW)
                        {
                            CheckLeftSnap(dragRectangle, target, ref horizontalOffset, ref foundHorizontalSnap);
                            if (foundHorizontalSnap) widthOffset = -horizontalOffset;
                        }
                        else if (Action == SnapAction.ResizeE || Action == SnapAction.ResizeSE || Action == SnapAction.ResizeNE)
                        {
                            CheckRightSnap(dragRectangle, target, ref widthOffset, ref foundHorizontalSnap);
                            horizontalOffset = 0d;
                        }

                        if (Action == SnapAction.ResizeN || Action == SnapAction.ResizeNE || Action == SnapAction.ResizeNW)
                        {
                            CheckTopSnap(dragRectangle, target, ref verticalOffset, ref foundVerticalSnap);
                            if (foundVerticalSnap) heightOffset = -verticalOffset;
                        }
                        else if (Action == SnapAction.ResizeS || Action == SnapAction.ResizeSE || Action == SnapAction.ResizeSW)
                        {
                            CheckBottomSnap(dragRectangle, target, ref heightOffset, ref foundVerticalSnap);
                            verticalOffset = 0d;
                        }
                    }
                }
            }

            if (ForceProportions && (Action == SnapAction.ResizeNE || Action == SnapAction.ResizeNW || Action == SnapAction.ResizeSE || Action == SnapAction.ResizeSW))
            {
                if (foundHorizontalSnap && foundVerticalSnap)
                {
                    if (widthOffset < heightOffset)
                    {
                        // TODO Calculate width based off height change
                        verticalOffset = horizontalOffset;
                        heightOffset = widthOffset;
                    }
                    else if (widthOffset > heightOffset)
                    {
                        // TOOD Caclulate height based off width change
                        horizontalOffset = verticalOffset;
                        widthOffset = heightOffset;
                    }
                }
                else if (foundVerticalSnap)
                {
                    // TODO Calculate width based off height change
                    foundHorizontalSnap = true;
                    horizontalOffset = verticalOffset;
                    widthOffset = heightOffset;
                }
                else if (foundHorizontalSnap)
                {
                    // TOOD Caclulate height based off width change
                    foundVerticalSnap = true;
                    verticalOffset = horizontalOffset;
                    heightOffset = widthOffset;
                }
            }

            NewLocation = new Point(dragRectangle.Location.X + (foundHorizontalSnap ? horizontalOffset : 0d), dragRectangle.Location.Y + (foundVerticalSnap ? verticalOffset : 0d));
            NewSize = new Size(Math.Max(1d, dragRectangle.Size.Width + (foundHorizontalSnap ? widthOffset : 0d)), Math.Max(1d, dragRectangle.Size.Height + (foundVerticalSnap ? heightOffset : 0d)));
        }

        private void CheckTopSnap(Rect dragRectangle, SnapTarget target, ref double currentVerticalOffset, ref bool isSnap)
        {
            if ((Math.Abs(dragRectangle.Top - target.Rectangle.Bottom) <= Math.Abs(currentVerticalOffset)))
            {
                currentVerticalOffset = target.Rectangle.Bottom - dragRectangle.Top;
                isSnap = true;
            }

            if ((Math.Abs(dragRectangle.Top - target.Rectangle.Top) <= Math.Abs(currentVerticalOffset)))
            {
                currentVerticalOffset = target.Rectangle.Top - dragRectangle.Top;
                isSnap = true;
            }
        }

        private void CheckBottomSnap(Rect dragRectangle, SnapTarget target, ref double currentVerticalOffset, ref bool isSnap)
        {
            if ((Math.Abs(target.Rectangle.Top - dragRectangle.Bottom) <= Math.Abs(currentVerticalOffset)))
            {
                currentVerticalOffset = target.Rectangle.Top - dragRectangle.Bottom;
                isSnap = true;
            }

            if ((Math.Abs(target.Rectangle.Bottom - dragRectangle.Bottom) <= Math.Abs(currentVerticalOffset)))
            {
                currentVerticalOffset = target.Rectangle.Bottom - dragRectangle.Bottom;
                isSnap = true;
            }
        }

        private void CheckLeftSnap(Rect dragRectangle, SnapTarget target, ref double currentHorizontalOffset, ref bool isSnap)
        {
            if ((Math.Abs(dragRectangle.Left - target.Rectangle.Right) <= Math.Abs(currentHorizontalOffset)))
            {
                currentHorizontalOffset = target.Rectangle.Right - dragRectangle.Left;
                isSnap = true;
            }

            if ((Math.Abs(dragRectangle.Left - target.Rectangle.Left) <= Math.Abs(currentHorizontalOffset)))
            {
                currentHorizontalOffset = target.Rectangle.Left - dragRectangle.Left;
                isSnap = true;
            }
        }

        private void CheckRightSnap(Rect dragRectangle, SnapTarget target, ref double currentHorizontalOffset, ref bool isSnap)
        {
            if ((Math.Abs(target.Rectangle.Left - dragRectangle.Right) <= Math.Abs(currentHorizontalOffset)))
            {
                currentHorizontalOffset = target.Rectangle.Left - dragRectangle.Right;
                isSnap = true;
            }

            if ((Math.Abs(target.Rectangle.Right - dragRectangle.Right) <= Math.Abs(currentHorizontalOffset)))
            {
                currentHorizontalOffset = target.Rectangle.Right - dragRectangle.Right;
                isSnap = true;
            }
        }

        private Rect CalculateDragRectangle()
        {
            Rect currentRectangle = new Rect(Location, Size);
            Rect dragRectangle = new Rect(Location, Size);

            if (Action == SnapAction.Drop || Action == SnapAction.Move || Action == SnapAction.LineEnd || Action == SnapAction.LineStart)
            {
                dragRectangle.X += Clamp(DragVector.X, Bounds.Left - currentRectangle.Left, Bounds.Right - currentRectangle.Right);
                dragRectangle.Y += Clamp(DragVector.Y, Bounds.Top - currentRectangle.Top, Bounds.Bottom - currentRectangle.Bottom);
            }
            else
            {
                Vector adjustedVector = DragVector;

                if (Action == SnapAction.ResizeN || Action == SnapAction.ResizeNE || Action == SnapAction.ResizeNW)
                {
                    adjustedVector.Y = Clamp(DragVector.Y, Bounds.Top - currentRectangle.Top, currentRectangle.Height - 1d);
                }
                else if (Action == SnapAction.ResizeS || Action == SnapAction.ResizeSE || Action == SnapAction.ResizeSW)
                {
                    adjustedVector.Y = Clamp(DragVector.Y, 1d - currentRectangle.Height, Bounds.Bottom - currentRectangle.Bottom);
                }

                if (Action == SnapAction.ResizeW || Action == SnapAction.ResizeSW || Action == SnapAction.ResizeNW)
                {
                    adjustedVector.X = Clamp(DragVector.X, Bounds.Left - currentRectangle.Left, currentRectangle.Width - 1d);
                }
                else if (Action == SnapAction.ResizeE || Action == SnapAction.ResizeNE || Action == SnapAction.ResizeSE)
                {
                    adjustedVector.X = Clamp(DragVector.X, 1d - currentRectangle.Width, Bounds.Right - currentRectangle.Right);
                }

                if (ForceProportions)
                {
                    double aspectRation = Size.Width / Size.Height;
                    double multiplier = 1d;
                    if (Action == SnapAction.ResizeNE || Action == SnapAction.ResizeSW)
                    {
                        multiplier = -1d;
                    }

                    if (Math.Abs(adjustedVector.X) < Math.Abs(adjustedVector.Y))
                    {
                        adjustedVector.Y = (adjustedVector.X / aspectRation) * multiplier;
                    }
                    else
                    {
                        adjustedVector.X = (adjustedVector.Y * aspectRation) * multiplier ;
                    }
                }

                if (Action == SnapAction.ResizeN || Action == SnapAction.ResizeNE || Action == SnapAction.ResizeNW)
                {
                    dragRectangle.Y += adjustedVector.Y;
                    dragRectangle.Height = Math.Max(1d, dragRectangle.Height - adjustedVector.Y);
                }
                else if (Action == SnapAction.ResizeS || Action == SnapAction.ResizeSE || Action == SnapAction.ResizeSW)
                {
                    dragRectangle.Height = Math.Max(1d, dragRectangle.Height + adjustedVector.Y);
                }

                if (Action == SnapAction.ResizeW || Action == SnapAction.ResizeSW || Action == SnapAction.ResizeNW)
                {
                    dragRectangle.X += adjustedVector.X;
                    dragRectangle.Width = Math.Max(1d, dragRectangle.Width - adjustedVector.X);
                }
                else if (Action == SnapAction.ResizeE || Action == SnapAction.ResizeNE || Action == SnapAction.ResizeSE)
                {
                    dragRectangle.Width = Math.Max(1d, dragRectangle.Width + adjustedVector.X);
                }
            }

            return dragRectangle;
        }

        private double Clamp(double value, double min, double max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }
    }
}
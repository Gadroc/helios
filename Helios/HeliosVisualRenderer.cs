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
    using System.Windows.Media;
    using System.Windows.Threading;

    /// <summary>
    /// Base class for visual renderers.
    /// </summary>
    public abstract class HeliosVisualRenderer : NotificationObject
    {
        private WeakReference _visual = new WeakReference(null);
        private bool _needsRefresh = true;
        private Dispatcher _dispatcher;
        private TransformGroup _transform;

        public HeliosVisualRenderer()
        {
        }

        #region Properties

        /// <summary>
        /// Visual which this renderer will render.
        /// </summary>
        public HeliosVisual Visual
        {
            get
            {
                return _visual.Target as HeliosVisual;
            }
            set
            {
                if ((_visual == null && value != null)
                    || (_visual != null && !_visual.Equals(value)))
                {
                    HeliosVisual oldControl = _visual.Target as HeliosVisual;
                    _visual = new WeakReference(value);
                    OnPropertyChanged("Visual", oldControl, value, false);
                }
            }
        }

        public Dispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
            }
            set
            {
                if ((_dispatcher == null && value != null)
                    || (_dispatcher != null && !_dispatcher.Equals(value)))
                {
                    Dispatcher oldValue = _dispatcher;
                    _dispatcher = value;
                    OnPropertyChanged("Dispatcher", oldValue, value, false);
                }
            }
        }

        public Transform Transform
        {
            get { return _transform; }
        }

        #endregion

        /// <summary>
        /// Renders the visual at it's configured size.
        /// </summary>
        /// <param name="drawingContext">Context on which to draw this control.</param>
        public void Render(DrawingContext drawingContext)
        {
            CheckRefresh();
            OnRender(drawingContext);
        }

        /// <summary>
        /// Renders the visual scaled.
        /// </summary>
        /// <param name="drawingContext">Context on which to draw this control.</param>
        public void Render(DrawingContext drawingContext, double scaleX, double scaleY)
        {
            CheckRefresh();
            OnRender(drawingContext, scaleX, scaleY);
        }

        /// <summary>
        /// Render the visual at a specified size.
        /// </summary>
        /// <param name="drawingContext">Context on which to draw this control.</param>
        /// 
        public void Render(DrawingContext drawingContext, Size size)
        {
            CheckRefresh();
            OnRender(drawingContext, size.Width / Visual.Width, size.Height / Visual.Height);
        }

        private void CheckRefresh()
        {
            if (_needsRefresh)
            {
               OnRefresh();
               UpdateTransform();
                _needsRefresh = false;
            }
        }

        /// <summary>
        /// Renders this control in the given drawing context.
        /// </summary>
        /// <param name="drawingContext">Context on which to draw this control.</param>
        protected abstract void OnRender(DrawingContext drawingContext);

        /// <summary>
        /// Renders this control using scale in the given drawing context.
        /// </summary>
        /// <param name="drawingContext">Context on which to draw this control.</param>
        protected virtual void OnRender(DrawingContext drawingContext, double scaleX, double scaleY)
        {
            drawingContext.PushTransform(new ScaleTransform(scaleX, scaleY));
            OnRender(drawingContext);
            drawingContext.Pop();
        }

        /// <summary>
        /// Refreshes and reloads all resources needed to display this visual.
        /// </summary>
        public void Refresh()
        {
            _needsRefresh = true;
            if (Dispatcher != null)
            {
                if (Dispatcher.CheckAccess())
                {
                    CheckRefresh();
                }
                else
                {
                    Dispatcher.Invoke(new Action(CheckRefresh), null);
                }
            }
        }

        /// <summary>
        /// Recreate all cached images and text based on current control state
        /// </summary>
        protected abstract void OnRefresh();

        private void UpdateTransform()
        {
            switch (Visual.Rotation)
            {
                case HeliosVisualRotation.CW:
                    _transform = new TransformGroup();
                    _transform.Children.Add(new RotateTransform(90));
                    _transform.Children.Add(new TranslateTransform(Visual.Height, 0));
                    break;
                case HeliosVisualRotation.CCW:
                    _transform = new TransformGroup();
                    _transform.Children.Add(new RotateTransform(-90));
                    _transform.Children.Add(new TranslateTransform(0, Visual.Width));
                    break;
				case HeliosVisualRotation.ROT180:
					_transform = new TransformGroup();
					_transform.Children.Add(new RotateTransform(180));
					_transform.Children.Add(new TranslateTransform(0,0));
					break;
				default:
                    _transform = null;
                    break;
            }
        }
    }
}

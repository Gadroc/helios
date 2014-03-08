//  Copyright 2013 Craig Courtney
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

namespace GadrocsWorkshop.Helios.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    using GadrocsWorkshop.Helios.Visuals;

    /// <summary>
    /// Handler which monitors controls instances for display updates.
    /// </summary>
    /// <param name="sender">Control Instance which has been updated.</param>
    /// <param name="args">Arguments for the control update.</param>
    public delegate void ControlUpdateHandler(object sender, EventArgs args);

    /// <summary>
    /// Contains state data for an invididual instance of a control.
    /// </summary>
    public class ControlInstance
    {
        private IControl _control;
        private List<VisualState> _visualStates;
        private ObservableCollection<ControlInstance> _children;

        public ControlInstance(IControl control)
        {
            _control = control;
            _visualStates = new List<VisualState>();
            foreach (Visual visual in control.Visuals)
            {
                _visualStates.Add(new VisualState(visual));
            }
            _children = new ObservableCollection<ControlInstance>();
            _children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Force redisplay of control if our children have changed.
            OnControlUpdated();
        }

        /// <summary>
        /// Event which is triggered when a controls state has changed and it's display needs to be refreshed.
        /// </summary>
        public event ControlUpdateHandler ControlUpdated;

        /// <summary>
        /// Control that this state is for.
        /// </summary>
        public IControl Control
        {
            get
            {
                return _control;
            }
        }

        /// <summary>
        /// State of each visual for this instance.
        /// </summary>
        public IEnumerable<VisualState> VisualStates
        {
            get
            {
                return _visualStates;
            }
        }

        /// <summary>
        /// Child controls which will be rendered on this control.
        /// </summary>
        public ObservableCollection<ControlInstance> Children
        {
            get
            {
                return _children;
            }
        }

        /// <summary>
        /// Top of the Control.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Left of the Control.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Width of this Control.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of this Control.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Helper method which must be called anytime this control must be re-rendered.
        /// </summary>
        protected void OnControlUpdated()
        {
            ControlUpdateHandler handler = ControlUpdated;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        private VisualState GetVisualState(string name)
        {
            foreach (VisualState state in _visualStates)
            {
                if (state.Visual.Id.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return state;
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the translation for a visual to an offset value from it's default state.
        /// </summary>
        /// <param name="visualName">ID of the visual which will be moved.</param>
        /// <param name="x">Offset to be applied to the X axis.</param>
        /// <param name="y">Offset to be applied to the Y axis.</param>
        public void SetVisualOffset(string visualName, float x, float y)
        {
            VisualState state = GetVisualState(visualName);
            if (state != null)
            {
                lock (this)
                {
                    state.XOffset = x;
                    state.YOffset = y;
                }
                OnControlUpdated();
            }
        }

        /// <summary>
        /// Sets the translation for a visual to an offset value from it's default state.
        /// </summary>
        /// <param name="visualName">ID of the visual which will be moved.</param>
        /// <param name="x">Offset to be applied to the X axis.</param>
        public void SetVisualXOffset(string visualName, float x)
        {
            VisualState state = GetVisualState(visualName);
            if (state != null)
            {
                lock (this)
                {
                    state.XOffset = x;
                }
                OnControlUpdated();
            }
        }

        /// <summary>
        /// Sets the translation for a visual to an offset value from it's default state.
        /// </summary>
        /// <param name="visualName">ID of the visual which will be moved.</param>
        /// <param name="y">Offset to be applied to the Y axis.</param>
        public void SetVisualYOffset(string visualName, float y)
        {
            VisualState state = GetVisualState(visualName);
            if (state != null)
            {
                lock (this)
                {
                    state.YOffset = y;
                }
                OnControlUpdated();
            }
        }

        /// <summary>
        /// Sets the rotation for a visual.
        /// </summary>
        /// <param name="visualName">ID of the visual which will be moved.</param>
        /// <param name="degrees">Degrees to rotate the visual.</param>
        public void SetVisualRotation(string visualName, float degrees)
        {
            VisualState state = GetVisualState(visualName);
            if (state != null)
            {
                lock (this)
                {
                    state.Rotation = degrees;
                }
                OnControlUpdated();
            }
        }

        /// <summary>
        /// Sets the opacity that a visual will be displayed with.
        /// </summary>
        /// <param name="visualName">ID of the visual which will be moved.</param>
        /// <param name="opacity"></param>
        public void SetVisualOpacity(string visualName, float opacity)
        {
            VisualState state = GetVisualState(visualName);
            if (state != null)
            {
                lock (this)
                {
                    state.Opacity = opacity;
                }
                OnControlUpdated();
            }
        }

        /// <summary>
        /// Sets the visibility of of a visual.
        /// </summary>
        /// <param name="visualName">ID of the visual which will be moved.</param>
        /// <param name="visible"></param>
        public void SetVisualVisibility(string visualName, bool visible)
        {
            VisualState state = GetVisualState(visualName);
            if (state != null)
            {
                lock (this)
                {
                    state.Visible = visible;
                }
                OnControlUpdated();
            }
        }

        /// <summary>
        /// Sets the color which a visual will be displayed with.
        /// </summary>
        /// <param name="visualName">ID of the visual which will be moved.</param>
        /// <param name="red">Red value of the color to be displayed.</param>
        /// <param name="green">Green value of the color to be displayed.</param>
        /// <param name="blue">Blue value of the color to be displayed.</param>
        public void SetVisualColor(string visualName, byte red, byte green, byte blue)
        {
            VisualState state = GetVisualState(visualName);
            if (state != null)
            {
                lock (this)
                {
                    state.Color = new Color(red, green, blue, (byte)255);
                }
                OnControlUpdated();
            }
        }

        /// <summary>
        /// Sets the text which will be displayed on this visual.
        /// </summary>
        /// <param name="visualName">ID of the visual which will be moved.</param>
        /// <param name="text">Text which will be displayed on the visual.</param>
        public void SetVisualText(string visualName, string text)
        {
            VisualState state = GetVisualState(visualName);
            if (state != null)
            {
                lock (this)
                {
                    state.Text = text;
                }
                OnControlUpdated();
            }
        }
    }
}

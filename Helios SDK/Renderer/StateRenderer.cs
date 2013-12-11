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

namespace GadrocsWorkshop.Helios.Renderer
{
    using System;
    using System.Collections.ObjectModel;

    using SharpDX;
    using SharpDX.Direct2D1;

    using GadrocsWorkshop.Helios.Runtime;
    using GadrocsWorkshop.Helios.Visuals;

    /// <summary>
    /// Base class for all elements which will be rendered in the scnene.
    /// </summary>
    public abstract class StateRenderer<T> : IDisposable where T : Visual
    {
        private WeakReference _parent;
        private WeakReference _state;
        private Matrix3x2 _matrix;
        private ObservableCollection<StateRenderer<Visual>> _children;
        private RectangleF _rectangle;

        /// <summary>
        /// Base constructure for RenderNodes.
        /// </summary>
        /// <param name="parent">Parent node in the scene</param>
        public StateRenderer(VisualState state, StateRenderer<Visual> parent)
        {
            _parent = new WeakReference(parent);
            _matrix = Matrix3x2.Identity;
            _children = new ObservableCollection<StateRenderer<Visual>>();
        }

        #region Properties

        /// <summary>
        /// Parent render node that this node is rendered under.
        /// </summary>
        public StateRenderer<Visual> Parent
        {
            get
            {
                return (StateRenderer<Visual>)_parent.Target;
            }
        }

        /// <summary>
        /// VisualState this renderer is for.
        /// </summary>
        protected VisualState State
        {
            get
            {
                return (VisualState)_state.Target;
            }
        }

        /// <summary>
        /// Returns the visual for this renderer.
        /// </summary>
        protected T Visual
        {
            get
            {
                return (T)State.Visual;
            }
        }

        /// <summary>
        /// Children nodes which are rendered using this nodes transform matrix
        /// </summary>
        public Collection<StateRenderer<Visual>> Children
        {
            get
            {
                return _children;
            }
        }

        /// <summary>
        /// ModelViewProjection matrix for this node.
        /// </summary>
        protected Matrix3x2 Matrix
        {
            get
            {
                return _matrix;
            }
            set
            {
                _matrix = value;
            }
        }

        /// <summary>
        /// Bounding rectangle for the visual state
        /// </summary>
        protected RectangleF BoundingRectangle
        {
            get
            {
                return _rectangle;
            }
        }

        #endregion

        /// <summary>
        /// Renders this node.
        /// </summary>
        public void Render(RenderTarget target)
        {
            Matrix3x2 oldMatrix = target.Transform;
            target.Transform = oldMatrix * Matrix;
            OnRender(target);
            foreach(StateRenderer<Visual> child in Children)
            {
                child.Render(target);
            }
            target.Transform = oldMatrix;
        }

        /// <summary>
        /// Override function which does all rendering for this node.
        /// </summary>
        protected abstract void OnRender(RenderTarget target);

        /// <summary>
        /// Disposes of all non-managed resources for this object.
        /// </summary>
        public abstract void Dispose();
    }
}

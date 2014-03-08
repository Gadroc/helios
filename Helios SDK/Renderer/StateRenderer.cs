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
    /// Base renderer for a visutal state.
    /// </summary>
    internal abstract class StateRenderer : RenderNode
    {
        private WeakReference _state;
        private Matrix3x2 _matrix;
        private RectangleF _rectangle;

        /// <summary>
        /// Base constructure for RenderNodes.
        /// </summary>
        /// <param name="parent">Parent node in the scene</param>
        public StateRenderer(VisualState state)
        {
            _state = new WeakReference(state);
            _rectangle = new RectangleF(0, 0, state.Width, state.Height);
        }

        #region Properties

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
        /// ModelViewProjection matrix for this node.
        /// </summary>
        protected override Matrix3x2 Matrix
        {
            get
            {
                if (State.IsMatrixDirty)
                {
                    _matrix = Matrix3x2.Translation(State.Left, State.Top);
                    if (State.Rotation != 0)
                    {
                        _matrix *= Matrix3x2.Rotation(State.Rotation, new Vector2(State.RotationCenterHorizontal, State.RotationCenterVertical));
                    }
                    State.IsMatrixDirty = false;
                }
                return _matrix;
            }
        }

        /// <summary>
        /// Bounding rectangle for the visual state
        /// </summary>
        protected override RectangleF BoundingRectangle
        {
            get
            {
                return _rectangle;
            }
        }

        #endregion

    }

    /// <summary>
    /// Base renderer for a visutal state.
    /// </summary>
    internal abstract class StateRenderer<T> : StateRenderer where T : Visual
    {

        public StateRenderer(VisualState state)
            : base(state)
        {
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
    }
}

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
    using System.Collections.Generic;

    using SharpDX;
    using SharpDX.Direct2D1;

    /// <summary>
    /// Base for all rendering nodes in a render tree.
    /// </summary>
    internal abstract class RenderNode : IDisposable
    {
        private bool _disposed;
        private List<RenderNode> _children;

        public RenderNode()
        {
            _children = new List<RenderNode>();
            _disposed = false;
        }

        #region Properties

        /// <summary>
        /// Transform matrix used for this visual state.
        /// </summary>
        protected abstract Matrix3x2 Matrix { get; }

        /// <summary>
        /// Bounding rectangle for the visual state
        /// </summary>
        protected abstract RectangleF BoundingRectangle { get; }

        /// <summary>
        /// Collection of children nodes which will be rendered using this nodes transform matrix.
        /// </summary>
        public IList<RenderNode> Children
        {
            get
            {
                return _children;
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
            OnRenderBeforeChildren(target);
            foreach (RenderNode child in Children)
            {
                child.Render(target);
            }
            OnRenderAfterChildren(target);
            target.Transform = oldMatrix;
        }

        /// <summary>
        /// Override function which renders items drawn before it's children
        /// </summary>
        protected abstract void OnRenderBeforeChildren(RenderTarget target);

        /// <summary>
        /// Override function which renders items drawn after it's children
        /// </summary>
        protected abstract void OnRenderAfterChildren(RenderTarget target);

        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Disposes of all non-managed resources for this object.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Resource was already disposed.");
            }
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}

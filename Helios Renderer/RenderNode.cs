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

using System;
using System.Collections.ObjectModel;

using SharpGL.SceneGraph;

namespace GadrocsWorkshop.Helios.Renderer
{
    /// <summary>
    /// Base class for all elements which will be rendered in the scnene.
    /// </summary>
    internal abstract class RenderNode
    {
        private WeakReference _parent;
        private WeakReference _renderer;
        private Matrix _matrix;
        private ObservableCollection<RenderNode> _children;

        /// <summary>
        /// Base constructure for RenderNodes.
        /// </summary>
        /// <param name="renderer">Renderer that will render this node.</param>
        /// <param name="parent">Parent node in the scene</param>
        public RenderNode(SceneRenderer renderer, RenderNode parent)
        {
            _renderer = new WeakReference(renderer);
            _parent = new WeakReference(parent);
            _matrix = new Matrix(4, 4);
            _matrix.SetIdentity();
            _children = new ObservableCollection<RenderNode>();
            _children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region Properties

        /// <summary>
        /// Parent render node that this node is rendered under.
        /// </summary>
        public RenderNode Parent
        {
            get
            {
                return (RenderNode)_parent.Target;
            }
        }

        public Collection<RenderNode> Children
        {
            get
            {
                return _children;
            }
        }

        /// <summary>
        /// Scenerender that this rendernode belongs to.
        /// </summary>
        public SceneRenderer Renderer
        {
            get
            {
                return (SceneRenderer)_renderer.Target;
            }
        }

        /// <summary>
        /// ModelViewProjection matrix for this node.
        /// </summary>
        public Matrix Matrix
        {
            get
            {
                return _matrix;
            }
        }
        // TODO Need to detect all matrix changes (aka matrix will probably need to become immutable) so we know when to recalc children matrix

        #endregion

        /// <summary>
        /// Override function which does all rendering for this node.
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Renders all children in order.
        /// </summary>
        protected void RenderChildern()
        {
            foreach (RenderNode child in Children)
            {
                child.Render();
            }
        }
    }
}

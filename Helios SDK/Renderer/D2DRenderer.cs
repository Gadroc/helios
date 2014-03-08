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
    using System.Collections.ObjectModel;
    using System.Threading;

    using SharpDX;
    using SharpDX.Direct2D1;

    using GadrocsWorkshop.Helios.Runtime;
    using GadrocsWorkshop.Helios.Util;
    using GadrocsWorkshop.Helios.Visuals;

    /// <summary>
    /// Renders a set of control instances to a Direct2D render target.  D2DRender does not manage thread saftey.
    /// Updates to the Scene, ControlInstances and VisualStates must be done between calls to the renderers lock / unlock methods.
    /// </summary>
    public class D2DRenderer : IDisposable
    {
        private object _renderLock = new object();
        private RenderTarget _target;
        private ObservableCollection<ControlInstance> _scene;

        private List<RenderNode> Nodes;

        /// <summary>
        /// Creates a Direct2D renderer with out a render target.
        /// </summary>
        public D2DRenderer()
        {
            Nodes = new List<RenderNode>();
            Scene = new ObservableCollection<ControlInstance>();
        }

        /// <summary>
        /// Creates a Direct2D renderer with a render target.
        /// </summary>
        /// <param name="target"></param>
        public D2DRenderer(RenderTarget target) : this()
        {
            Target = target;
        }

        /// <summary>
        /// Dispose of any non-managed resources
        /// </summary>
        public void Dispose()
        {
            // Loop through all state renderers and dispose!
            DestroyRenderTree();
        }

        /// <summary>
        /// Gets or sets the rendertarget for this renderer.
        /// </summary>
        public RenderTarget Target
        {
            get
            {
                return _target;
            }
            set
            {
                if (_target != value)
                {
                    // TODO Release all existing resources.
                    _target = value;
                }
            }
        }

        /// <summary>
        /// Collection of control instances to display.
        /// </summary>
        public ObservableCollection<ControlInstance> Scene
        {
            get 
            { 
                return _scene; 
            }
            set
            {
                if (_scene != null)                
                {
                    _scene.CollectionChanged -= OnSceneContentsChanged;
                    foreach (ControlInstance control in _scene)
                    {
                        UnWatchChildren(control);
                    }
                }

                _scene = value;

                _scene.CollectionChanged += OnSceneContentsChanged;
                foreach (ControlInstance control in value)
                {
                    WatchChildren(control);
                }

                DestroyRenderTree();
            }
        }

        /// <summary>
        /// Adds scene graph watchers for a control instance.
        /// </summary>
        private void WatchChildren(ControlInstance control)
        {
            control.Children.CollectionChanged += OnSceneContentsChanged;
            foreach (ControlInstance child in control.Children)
            {
                WatchChildren(child);
            }
        }

        /// <summary>
        /// Remove scene graph watchers for a control instance
        /// </summary>
        private void UnWatchChildren(ControlInstance control)
        {
            control.Children.CollectionChanged -= OnSceneContentsChanged;
            foreach (ControlInstance child in control.Children)
            {
                UnWatchChildren(child);
            }
        }

        /// <summary>
        /// Manages adds and removes to all children collections in the scenegraph
        /// </summary>
        private void OnSceneContentsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Stop watching all for changes in removed branches of the graph.
            if (e.OldItems != null)
            {
                foreach (ControlInstance child in e.OldItems)
                {
                    UnWatchChildren(child);
                }
            }

            // Watch for changes in the new branches of the graph.
            if (e.NewItems != null)
            {
                foreach (ControlInstance child in e.NewItems)
                {
                    WatchChildren(child);
                }
            }

            // Graph has changed we need to recreate teh render tree
            DestroyRenderTree();
            CreateRenderTree();
        }

        /// <summary>
        /// Disposes and removes all nodes from the current rendertree.
        /// </summary>
        private void DestroyRenderTree()
        {
            foreach (RenderNode node in Nodes)
            {
                DestroyRenderNodeBranch(node);
            }
            Nodes.Clear();
        }

        private void DestroyRenderNodeBranch(RenderNode node)
        {
            foreach (RenderNode child in node.Children)
            {
                DestroyRenderNodeBranch(child);
            }
            node.Dispose();
        }

        /// <summary>
        /// Creates the render node graph based on the current scene contents.
        /// </summary>
        private void CreateRenderTree()
        {
            foreach (ControlInstance control in Scene)
            {
                Nodes.Add(CreateRenderNode(control));
            }
        }

        private RenderNode CreateRenderNode(ControlInstance control)
        {
            ControlRenderNode controlNode = new ControlRenderNode(control);

            foreach (VisualState state in control.VisualStates)
            {
                controlNode.Children.Add(CreateVisualNode(state));
            }

            return controlNode;
        }

        private RenderNode CreateVisualNode(VisualState state)
        {
            RenderNode childNode = null;

            switch (state.Visual.VisualType)
            {
                case 0:
                    childNode = new RectangleRenderer(state);
                    break;
            }

            if (childNode != null)
            {
                foreach (VisualState childState in state.Children)
                {
                    childNode.Children.Add(CreateVisualNode(childState));
                }
            }
            return childNode;
        }

        /// <summary>
        /// Creates a lock to prevent rendering from happening.  Must be called before modifying any scene data.
        /// </summary>
        /// <returns>True if lock was sucessfuly obtained, false if not.</returns>
        public bool Lock()
        {
            bool retValue = false;
            Monitor.Enter(_renderLock, ref retValue);
            return retValue;
        }

        /// <summary>
        /// Unlocks the renderer.  Must be called after modifying scene data to re-enable renderer.
        /// </summary>
        public void Unlock()
        {
            Monitor.Exit(_renderLock);
        }

        /// <summary>
        /// Triggers a redraw of the control instances.
        /// </summary>
        public void Render()
        {
            lock (_renderLock)
            {
                Target.BeginDraw();
                foreach (RenderNode node in Nodes)
                {
                    node.Render(Target);
                }
                Target.EndDraw();
            }
        }
    }
}
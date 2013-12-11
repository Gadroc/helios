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
using System.Collections.Generic;
using System.Threading;

using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph.Shaders;

namespace GadrocsWorkshop.Helios.Renderer
{
    /// <summary>
    /// Common rendering base for all D2D based renderers.
    /// </summary>
    public class SceneRenderer
    {
        private Scene _renderList;
        private float[] _projectionMatrix;
        private List<RenderNode> _renderTree;

        public SceneRenderer()
        {
            _renderTree = new List<RenderNode>();
        }

        #region Manage Scene Changes

        void ControlRemoved(object sender, ControlState control)
        {
            throw new NotImplementedException();
        }

        private void ControlAdded(object sender, ControlState control)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Shader program for solid color objects
        /// </summary>
        private ShaderProgram Program { get; set; }

        /// <summary>
        /// OpenGL context for this renderer.
        /// </summary>
        internal OpenGL gl { get; private set; }

        /// <summary>
        /// Returns the index to the vertex varriant in the vertex program.
        /// </summary>
        internal int VertexIndex { get; private set; }

        /// <summary>
        /// Returns the index to the color varriant in the fragment program.
        /// </summary>
        internal int ColorIndex { get; private set; }

        /// <summary>
        /// Returns index to the Projection Matrix
        /// </summary>
        public int ProjectionIndex { get; private set; }

        #endregion

        /// <summary>
        /// Initializes this renderer.
        /// </summary>
        /// <param name="gl">OpenGL render context for this renderer.</param>
        public void Initialize(OpenGL gl)
        {
            this.gl = gl;

            gl.ShadeModel(ShadeModel.Smooth);

            VertexShader vertexShader = new VertexShader();
            vertexShader.CreateInContext(gl);
            vertexShader.LoadSource("NormalVertex.glsl");
            vertexShader.Compile();

            FragmentShader fragmentShader = new FragmentShader();
            fragmentShader.CreateInContext(gl);
            fragmentShader.LoadSource("NormalFragment.glsl");
            fragmentShader.Compile();

            Program = new ShaderProgram();
            Program.CreateInContext(gl);
            Program.AttachShader(vertexShader);
            Program.AttachShader(fragmentShader);
            Program.Link();

            gl.ClearColor(1f, 0f, 0f, 1f);

            ProjectionIndex = gl.GetUniformLocation(Program.ProgramObject, "uProjection");
            VertexIndex = gl.GetAttribLocation(Program.ProgramObject, "vPosition");
            ColorIndex = gl.GetUniformLocation(Program.ProgramObject, "vColor");

            // Loop through all controls and initialize them
        }

        /// <summary>
        /// Renders this scene
        /// </summary>
        public void Render()
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT);	// Clear The Frame
            gl.Flush();
        }

        /// <summary>
        /// Resize the renderer. This must be called at least once with the initial size of the renderer.
        /// </summary>
        /// <param name="width">Width of the scene render</param>
        /// <param name="height">Height of the scene render</param>
        public void Resize( int width, int height)
        {
            _projectionMatrix = new float[16] {
                2 / width,  0,            0, -1,
                0,          2 / -height,  0,  1,
                0,          0,           -2, -1,
                0,          0,            0,  1
            };

            // TODO: Trigger cascade recalc of matrixes
            // TODO: Trigger scene change
        }
    }
}

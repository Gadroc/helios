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

    using SharpDX;

    using GadrocsWorkshop.Helios.Runtime;

    internal class ControlRenderNode : RenderNode
    {
        private Matrix3x2 _matrixCache;
        private RectangleF _rectangle;

        public ControlRenderNode(ControlInstance control)
        {
            _matrixCache = Matrix3x2.Translation(control.Left, control.Top);
            _rectangle = new RectangleF(0f, 0f, control.Width, control.Height);
        }

        protected override Matrix3x2 Matrix
        {
            get { return _matrixCache; }
        }

        protected override RectangleF BoundingRectangle
        {
            get { return _rectangle; }
        }

        protected override void OnRenderBeforeChildren(SharpDX.Direct2D1.RenderTarget target)
        {
            target.PushAxisAlignedClip(_rectangle, SharpDX.Direct2D1.AntialiasMode.Aliased);
        }

        protected override void OnRenderAfterChildren(SharpDX.Direct2D1.RenderTarget target)
        {
            target.PopAxisAlignedClip();
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}

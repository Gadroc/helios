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

    using SharpDX.DXGI;
    using SharpDX.Direct2D1;
    using Factory = SharpDX.Direct2D1.Factory;
    using Format = SharpDX.DXGI.Format;
    using Surface = SharpDX.DXGI.Surface;

    using GadrocsWorkshop.Helios.Runtime;

    public class D2DRenderer : IDisposable
    {
        private Device DxgiDevice;
        private Surface BackBuffer;
        private RenderTarget Target;
        private ObservableCollection<ControlInstance> _scene;

        public D2DRenderer(Device device, Surface backBuffer)
        {
            StartD2D();
            this.DxgiDevice = device;
            this.BackBuffer = backBuffer;
            Target = new RenderTarget(D2DFactory, backBuffer, new RenderTargetProperties()
                {
                    DpiX = 96,
                    DpiY = 96,
                    MinLevel = SharpDX.Direct2D1.FeatureLevel.Level_DEFAULT,
                    PixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Ignore),
                    Type = RenderTargetType.Default,
                    Usage = RenderTargetUsage.None
                });
        }

        public void Dispose()
        {
            D2DRenderer.RemoveAndDispose(ref this.Target);
            StopD2D();
        }

        public IEnumerable<ControlInstance> Scene
        {
            get { return _scene; }
        }

        public void Render()
        {

        }

        #region Manage Shared Resources

        private static int ActiveClients = 0;
        private static Factory D2DFactory;

        private void StartD2D()
        {
            D2DRenderer.ActiveClients++;

            if (D2DRenderer.ActiveClients > 1)
                return;

            D2DRenderer.D2DFactory = new Factory();
        }

        private void StopD2D()
        {
            D2DRenderer.ActiveClients--;

            if (D2DRenderer.ActiveClients != 0)
                return;

            D2DRenderer.RemoveAndDispose(ref D2DRenderer.D2DFactory);
        }

        #endregion

        private static void RemoveAndDispose<TypeName>(ref TypeName resource) where TypeName : class
        {
            if (resource == null)
                return;

            IDisposable disposer = resource as IDisposable;
            if (disposer != null)
            {
                try
                {
                    disposer.Dispose();
                }
                catch
                {
                }
            }

            resource = null;
        }


    }
}

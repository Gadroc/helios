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

namespace GadrocsWorkshop.Helios.Editor.UI
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using SharpDX;
    using SharpDX.Direct2D1;
    using SharpDX.Direct3D10;
    using SharpDX.DXGI;
    using Device = SharpDX.Direct3D10.Device1;
    using DxgiDevice = SharpDX.DXGI.Device1;

    using GadrocsWorkshop.Helios.Renderer;
    using GadrocsWorkshop.Helios.Runtime;
    using GadrocsWorkshop.Helios.Util;

    /// <summary>
    /// WPF Control to display a since control instance.
    /// </summary>
    public class ControlHost : Image
    {
        private bool _isRendering;
        private Device _device;
        private D2DRenderer _renderer;
        private DX10ImageSource _d3dSurface;
        private Texture2D _backBuffer;
        private RenderTarget _renderTarget;
        private SharpDX.Direct2D1.Factory _d2dFactory;

        public ControlHost()
        {
            this.Loaded += this.Control_Loaded;
            this.Unloaded += this.Control_Closing;
        }

        #region DirectX Management

        /// <summary>
        /// Initialize resoruces when this control is first loaded.
        /// </summary>
        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            if (ControlHost.IsInDesignMode)
                return;

            InitializeD3D();

            // Once we are initialized we need to close on shutdown even if we do not get unloaded.
            this.Dispatcher.ShutdownFinished += Control_Closing;

            SetupBackBuffer();
            _renderer.Scene.Clear();
            _renderer.Scene.Add(Control);
            StartRendering();
        }

        /// <summary>
        /// Destroy unmanaged resources when the control is unloaded
        /// </summary>
        private void Control_Closing(object sender, EventArgs e)
        {
            if (ControlHost.IsInDesignMode)
                return;

            // If we are destroyed via unloaded event we do not want to destroy again from the shutdownfinished message
            this.Dispatcher.ShutdownFinished -= Control_Closing;

            StopRendering();
            DestroyD3D();
        }

        /// <summary>
        /// Create all necessary unmanaged resources for direct2d.
        /// </summary>
        private void InitializeD3D()
        {
#if DEBUG
            this._device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport | DeviceCreationFlags.Debug, SharpDX.Direct3D10.FeatureLevel.Level_10_1);
            this._d2dFactory = new SharpDX.Direct2D1.Factory(FactoryType.MultiThreaded, DebugLevel.Information);
#else
            this.Device = new Device(DriverType.Hardware, DeviceCreationFlags.BgraSupport, FeatureLevel.Level_10_1);
            this.D2DFactory = new SharpDX.Direct2D1.Factory(FactoryType.MultiThreaded);
#endif

            this._d3dSurface = new DX10ImageSource();
            this._d3dSurface.IsFrontBufferAvailableChanged += IsFrontBufferAvailableChanged;

            this._renderer = new D2DRenderer();

            this.Source = this._d3dSurface;
        }

        /// <summary>
        /// Destroy all unmanaged direct2d resources.
        /// </summary>
        private void DestroyD3D()
        {
            this._d3dSurface.IsFrontBufferAvailableChanged -= IsFrontBufferAvailableChanged;
            this.Source = null;

            Disposer.RemoveAndDispose(ref this._d3dSurface);
            Disposer.RemoveAndDispose(ref this._renderer);
            Disposer.RemoveAndDispose(ref this._renderTarget);
            Disposer.RemoveAndDispose(ref this._backBuffer);
            Disposer.RemoveAndDispose(ref this._d2dFactory);
            Disposer.RemoveAndDispose(ref this._device);
        }

        /// <summary>
        /// Recreates the backbuffer based on current control instance and binds it to the view.
        /// </summary>
        private void SetupBackBuffer()
        {
            // Unbind current backbuffer from the image source.
            this._d3dSurface.SetRenderTargetDX10(null);

            // Destroy any existing resources
            Disposer.RemoveAndDispose(ref this._renderTarget);
            Disposer.RemoveAndDispose(ref this._backBuffer);

            // Create our new backbuffer
            int width = Control != null ? Math.Max(Control.Width, 1) : 10;
            int height = Control != null ? Math.Max(Control.Height, 1) : 10;
            Texture2DDescription colordesc = new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.Shared,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1
            };
            this._backBuffer = new Texture2D(this._device, colordesc);

            // Create a new rendertarget for this backbuffer and assign it to the renderer
            Surface surface = this._backBuffer.QueryInterface<Surface>();
            RenderTargetProperties props = new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, AlphaMode.Premultiplied));
            this._renderTarget = new RenderTarget(this._d2dFactory, surface, props);
            this._renderer.Target = this._renderTarget;

            // Bind the backbuffer into our image sources
            this._d3dSurface.SetRenderTargetDX10(this._backBuffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this._d3dSurface.IsFrontBufferAvailable)
            {
                this.SetupBackBuffer();
                this.StartRendering();
            }
            else
            {
                this.StopRendering();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Control which will be rendered.
        /// </summary>
        public ControlInstance Control
        {
            get { return (ControlInstance)GetValue(ControlProperty); }
            set { SetValue(ControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Control.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlProperty =
            DependencyProperty.Register("Control", typeof(ControlInstance), typeof(ControlHost), new PropertyMetadata(null, OnControlInstanceChanged));

        private static void OnControlInstanceChanged(DependencyObject o, DependencyPropertyChangedEventArgs a)
        {
            ControlHost host = o as ControlHost;
            if (host._d3dSurface != null)
            {
                host.SetupBackBuffer();

                host._renderer.Scene.Clear();
                host._renderer.Scene.Add(host.Control);

                host.StartRendering();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is in design mode
        /// (running in Blend or Visual Studio).
        /// </summary>
        public static bool IsInDesignMode
        {
            get
            {
                DependencyProperty prop = DesignerProperties.IsInDesignModeProperty;
                bool isDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
                return isDesignMode;
            }
        }

        #endregion

        #region Rendering

        private void StartRendering()
        {
            if (_isRendering)
                return;

            CompositionTarget.Rendering += OnRendering;
            _isRendering = true;
        }

        private void StopRendering()
        {
            if (!_isRendering)
                return;

            CompositionTarget.Rendering -= OnRendering;
            _isRendering = false;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (!_isRendering)
                return;

            this.Render();
            this._d3dSurface.InvalidateD3DImage();
        }

        /// <summary>
        /// Renders the current scene.
        /// </summary>
        private void Render()
        {
            int targetWidth = this._backBuffer.Description.Width;
            int targetHeight = this._backBuffer.Description.Height;

            this._renderer.Render();            
            this._device.Flush();
        }

        #endregion

    }
}

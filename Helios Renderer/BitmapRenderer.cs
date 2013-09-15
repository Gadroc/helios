using System;
using System.Threading;

using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.WIC;

using Factory = SharpDX.Direct2D1.Factory;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.WIC.Bitmap;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;

namespace GadrocsWorkshop.Helios.Renderer
{
    // TODO:: Add visual tree locking so rendering is done in a different thread.
    //http://msdn.microsoft.com/en-us/library/system.threading.readerwriterlockslim.tryenterreadlock.aspx

    /// <summary>
    /// Renders a Helios visual to an offscreen bitmap using
    /// software rendering.
    /// </summary>
    public class BitmapRenderer : DisposableObject
    {
        private Factory _d2dFactory = null;
        private ImagingFactory _wicFactory = null;

        private Bitmap _bitmap = null;
        private RenderTarget _renderTarget = null;
        private BitmapLock _lock = null;

        /// <summary>
        /// Constructs an offscreen bitmap renderer for Helios visuals.
        /// </summary>
        /// <param name="width">Width of the bitmap</param>
        /// <param name="height">Height of the bitmap</param>
        /// <param name="format">What type of bitmap should be created</param>
        public void BitMapRenderer(int width, int height, BitmapFormat format)
        {
            _wicFactory = new ImagingFactory();
            _d2dFactory = new Factory();
            _bitmap = new Bitmap(_wicFactory, width, height, GetPixelFormat(format) , BitmapCreateCacheOption.NoCache);
            RenderTargetProperties targetProperties = new RenderTargetProperties(RenderTargetType.Default, new PixelFormat(Format.Unknown, AlphaMode.Unknown), 0, 0, RenderTargetUsage.None, FeatureLevel.Level_DEFAULT);
            _renderTarget = new WicRenderTarget(_d2dFactory, _bitmap, targetProperties);
        }

        /// <summary>
        /// Returns the WIC pixel format GUID for the desired bitmap format.
        /// </summary>
        /// <param name="format">Desired bitmap format.</param>
        /// <returns></returns>
        private Guid GetPixelFormat(BitmapFormat format)
        {
            switch (format)
            {
                case BitmapFormat.DIB:
                    return SharpDX.WIC.PixelFormat.Format32bppBGRA;

                case BitmapFormat.NoAlpha:
                    return SharpDX.WIC.PixelFormat.Format24bppRGB;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Locks the bitmap so it's pixel data may be read.
        /// </summary>
        public void Lock()
        {
            if (_lock == null)
            {
                _lock = _bitmap.Lock(null, BitmapLockFlags.Read);
            }
        }

        /// <summary>
        /// Returns true if the bitmap is currently locked.
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return _lock != null;
            }
        }

        /// <summary>
        /// Returns a pointer to the pixel data.  If called with out a lock
        /// returns a zero pointer.
        /// </summary>
        public IntPtr PixelData
        {
            get
            {
                return IsLocked ? _lock.Data.DataPointer : IntPtr.Zero;
            }
        }

        /// <summary>
        /// Returns the stride between rows of pixel data.  If called with out a lock
        /// returns 0.
        /// </summary>
        public int Stride
        {
            get
            {
                return IsLocked ? _lock.Stride : 0;
            }
        }

        /// <summary>
        /// Unlocks the bitmap data so it is available for writing again
        /// </summary>
        public void Unlock()
        {
            if (IsLocked)
            {
                _lock.Dispose();
                _lock = null;
            }
        }

        ~BitmapRenderer()
        {
            // Close the resources used here.
            Dispose();
        }

        protected override void OnDispose()
        {
            if (IsLocked)
            {
                Unlock();
            }
            _renderTarget.Dispose();
            _bitmap.Dispose();
            _wicFactory.Dispose();
            _d2dFactory.Dispose();
            _renderTarget = null;
            _bitmap = null;
            _wicFactory = null;
            _d2dFactory = null;
        }
    }
}

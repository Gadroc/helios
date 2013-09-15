using System;

namespace GadrocsWorkshop.Helios.Renderer
{
    /// <summary>
    /// Base class for objects which handle 
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        private bool _isDisposed = false;

        internal DisposableObject()
        {
        }

        public bool IsDisposed
        {
            get
            {
                return _isDisposed;
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                OnDispose();
            }
            _isDisposed = true;
        }

        protected abstract void OnDispose();
    }
}

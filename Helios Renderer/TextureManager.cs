using System;

using SharpDX.WIC;

namespace GadrocsWorkshop.Helios.Renderer
{
    /// <summary>
    /// Class for combining managing textures into larger atlas textures.
    /// </summary>
    internal class TextureManager
    {
        private ImagingFactory _wicFactory;

        // TODO Implement http://www.codeproject.com/Articles/210979/Fast-optimizing-rectangle-packing-algorithm-for-bu

        public TextureManager(ImagingFactory wicFactory)
        {
            _wicFactory = wicFactory;
        }


    }
}

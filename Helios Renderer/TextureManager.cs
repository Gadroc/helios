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

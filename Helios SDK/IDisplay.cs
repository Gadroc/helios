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

namespace GadrocsWorkshop.Helios
{
    using System.Collections.Generic;

    using GadrocsWorkshop.Helios.Renderer;

    /// <summary>
    /// Interface for exposing displays so they can be used to render Controls.
    /// </summary>
    public interface IDisplay : IProfileObject
    {
        /// <summary>
        /// Returns the id for this display.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Width of the display in pixels.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of the display in pixels.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Initialize this display so it's ready for use.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Tells the display to identify itself.
        /// </summary>
        /// <param name="label">Label to put on the display.</param>
        void Identify(string label);

        /// <summary>
        /// Disposes any resources for this display since it's no longer used.
        /// </summary>
        void Dispose();

    }
}

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
using System.IO;

namespace GadrocsWorkshop.Helios
{
    /// <summary>
    /// Interface for controls can be displayed and possibly interacted with on screen.  A single instance of this
    /// object will be shared across all displayed versions of this control.
    /// </summary>
    public interface IControl
    {
        /// <summary>
        /// Identifier for the control type of this control.
        /// </summary>
        IControlType ControlType { get; }

        /// <summary>
        /// Native width of this visual.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Native height of this visual.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Flag indicating whether this control allows children to be specified.
        /// </summary>
        bool AllowChildren { get; }

        /// <summary>
        /// Collection of visuals which will be displayed on this control.
        /// </summary>
        IEnumerable<IVisual> Visuals { get; }

        /// <summary>
        /// Serializes configuration of this control.
        /// </summary>
        /// <param name="stream">Stream to serialize this control to.</param>
        void Serialize(Stream stream);

        /// <summary>
        /// Desearilizes a configuration of this control.
        /// </summary>
        /// <param name="stream">Stream to deserialize this control from.</param>
        void Deserialize(Stream stream);
    }
}

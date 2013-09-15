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

namespace GadrocsWorkshop.Helios
{
    /// <summary>
    /// Interface for describing meta-data about available control types.
    /// </summary>
    public interface IControlType
    {
        /// <summary>
        /// Returns unique id for this control type.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns the display name of this control type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns a description of this control type.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Returns true if this control can be displayed on a remote display.
        /// </summary>
        bool IsRemoteable { get; }
    }
}

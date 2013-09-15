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

namespace GadrocsWorkshop.Helios
{
    /// <summary>
    /// Interface for exposing devices which supplies inputs and outputs to the Helios Runtime.
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Returns unique id for this device.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Returns the display name of this device type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns a description of this device type.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// If true there should be a limit of only one of this device type per profile.
        /// </summary>
        bool IsUnique { get; }
    }
}

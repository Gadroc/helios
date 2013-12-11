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
    /// <summary>
    /// Common interface for all objects which are configured in a profile.
    /// </summary>
    public interface IProfileObject
    {
        /// <summary>
        /// ID of the plug-in which owns this object.
        /// </summary>
        string PlugInId { get; }

        /// <summary>
        /// Returns unique id for this object type.
        /// </summary>
        string TypeId { get; }

        /// <summary>
        /// Returns the display name of this object type.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Returns a description of this object type.
        /// </summary>
        string TypeDescription { get; }

        // TODO Define serialization entry points?
    }
}

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

    /// <summary>
    /// Base class for all plugins
    /// </summary>
    public interface IPlugIn
    {
        /// <summary>
        /// Enumerate through all displays supplied by this plug in.
        /// </summary>
        IEnumerable<IDisplay> GetDisplays();

        /// <summary>
        /// Enumerate through all displays supplied by this plug in.
        /// </summary>
        IEnumerable<IDevice> GetDevices();

        /// <summary>
        /// Enumerate through all available control types supplied by this plug in.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IControl> GetControls();

        /// <summary>
        /// Creates a new instance of a control type.
        /// </summary>
        /// <param name="type">Type ID of the control to create.</param>
        /// <returns>New instance of that control type.</returns>
        IControl CreateControl(string typeId);

        /// <summary>
        /// Flag indicating whether this plugin is automatially added to new profiles.  This should
        /// only be done for plugins which only have optional configuration parameters.
        /// </summary>
        /// <returns>True if this plugin should be automatically added to new profiles.</returns>
        bool IsAutoActive { get; }

        /// <summary>
        /// Flag indicating whether this plug in can only be added once to a profile.
        /// </summary>
        /// <returns>True if this plugin can only be instantiated once.</returns>
        bool IsUnique { get; }

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called when plug-in is not longer needed.
        /// </summary>
        void Destroy();
    }
}

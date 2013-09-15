using System;
using System.Collections.Generic;

namespace GadrocsWorkshop.Helios
{
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
        IEnumerable<IControlType> GetControlTypes();

        /// <summary>
        /// Creates a new instance of a control type.
        /// </summary>
        /// <param name="type">Type ID of the control to create.</param>
        /// <returns>New instance of that control type.</returns>
        IControl CreateControl(string typeId);

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

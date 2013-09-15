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

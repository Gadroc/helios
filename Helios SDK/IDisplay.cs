using System;
using System.Collections.Generic;

using GadrocsWorkshop.Helios.Profile;

namespace GadrocsWorkshop.Helios
{
    /// <summary>
    /// Interface for exposing displays so they can be used to render Controls.
    /// </summary>
    public interface IDisplay
    {
        /// <summary>
        /// ID of the plug-in which controls this display.
        /// </summary>
        string PlugInId { get; }

        /// <summary>
        /// Unique id for this display.  Value will is unique per plug-in but
        /// may be duplicated across plug-ins.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Name of the display as set by the user.
        /// </summary>
        string Name { get; set; }

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

        /// <summary>
        /// Controls displayed on this display.
        /// </summary>
        IEnumerable<ControlInstance> Controls { get; set; }
    }
}

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

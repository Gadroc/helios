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

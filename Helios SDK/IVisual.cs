using System;

namespace GadrocsWorkshop.Helios
{
    /// <summary>
    /// Interface for all visual elements displayed by Helios.
    /// </summary>
    public interface IVisual
    {
        /// <summary>
        /// Returns the type id for this visual
        /// </summary>
        byte VisualType { get; }

        /// <summary>
        /// Returns the type id for this visual
        /// </summary>
        string VisualTypeName { get; }

        /// <summary>
        /// Id used for this visual state manipulation.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Flag indicating wheter this visual is static. Static visuals do not
        /// change during runtime.
        /// </summary>
        bool IsStatic { get; set; }

        /// <summary>
        /// Top of the visual.
        /// </summary>
        int Top { get; set; }

        /// <summary>
        /// Left of the visual.
        /// </summary>
        int Left { get; set; }

        /// <summary>
        /// Width of this visual.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Height of this visual.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Horizontal point which this visual will be rotated around.
        /// </summary>
        int RotationCenterHorizontal { get; set; }

        /// <summary>
        /// Veritcal point which this visual will be rotated around.
        /// </summary>
        int RotationCenterVertical { get; set; }
    }
}

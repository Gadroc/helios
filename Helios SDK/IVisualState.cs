using System;

namespace GadrocsWorkshop.Helios
{
    /// <summary>
    /// State data for display visuals.
    /// </summary>
    public interface IVisualState
    {
        /// <summary>
        /// Visual that this state is for
        /// </summary>
        IVisual Visual { get; }

        /// <summary>
        /// Top of the visual.
        /// </summary>
        int Top { get; }

        /// <summary>
        /// Left of the visual.
        /// </summary>
        int Left { get; }

        /// <summary>
        /// Width of this visual.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of this visual.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Horizontal point which this visual will be rotated around.
        /// </summary>
        int RotationCenterHorizontal { get; }

        /// <summary>
        /// Veritcal point which this visual will be rotated around.
        /// </summary>
        int RotationCenterVertical { get; }

        /// <summary>
        /// Indicates whether this control is visible or not.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// The amount of offset from default X position for this visual
        /// </summary>
        int XOffset { get; set; }

        /// <summary>
        /// The amount of offset from default Y position for this visual
        /// </summary>
        int YOffset { get; set; }

        /// <summary>
        /// The amount of rotation for this visual.
        /// </summary>
        double Rotation { get; set; }

        /// <summary>
        /// Opacity to display this visual with
        /// </summary>
        double Opacity { get; set; }
    }
}

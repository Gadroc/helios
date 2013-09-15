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

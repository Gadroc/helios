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

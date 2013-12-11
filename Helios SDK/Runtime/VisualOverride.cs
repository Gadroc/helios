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

namespace GadrocsWorkshop.Helios.Runtime
{
    using System;

    using GadrocsWorkshop.Helios.Visuals;

    /// <summary>
    /// Values that have been override on a visual for an instance of a control.
    /// </summary>
    public class VisualOverride
    {
        /// <summary>
        /// ID of the visual this override is for.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Color this visual will display glyphs with.
        /// </summary>
        public Color? Color { get; set; }

        /// <summary>
        /// Images this visual will display.
        /// </summary>
        public string ImagePath { get; set; }
    }
}

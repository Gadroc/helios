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

namespace GadrocsWorkshop.Helios.Binding
{
    using System;

    public class BindingUomType
    {
        /// <summary>
        /// Unique ID used in proeprty and configuration files used to specify units of measure
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Text which will be displayed in the UI for this unit of measure
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Flag indicating this Uom is a compound uom.
        /// </summary>
        public Boolean Compound { get; set; }

        /// <summary>
        /// For compound types this is the type of the base unit (Ex: for speed this would be distance)
        /// </summary>
        public BindingUomType BaseType { get; set; }


        /// <summary>
        /// For compunt types this is set to the per unit type (Ex: for speed this would be time)
        /// </summary>
        public BindingUomType PerType { get; set; }

        /// <summary>
        /// Base unit which is used to calculate conversion factors for this type of unit.  Note
        /// this is not applicable for compound unit types.
        /// </summary>
        public string BaseUnit { get; set; }
    }
}

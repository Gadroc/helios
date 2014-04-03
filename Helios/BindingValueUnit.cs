//  Copyright 2014 Craig Courtney
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

namespace GadrocsWorkshop.Helios
{
    /// <summary>
    /// Class representing a unit which values can be expressed in.  For units which may
    /// be non unique per type (like Ranges) you must override the Equals method properly.
    /// </summary>
    public abstract class BindingValueUnit
    {
        /// <summary>
        /// Returns the type of value this unit meassures.
        /// </summary>
        public abstract BindingValueUnitType Type
        {
            get;
        }

        /// <summary>
        /// Short display name for this unit type.
        /// </summary>
        public abstract string ShortName
        {
            get;
        }

        /// <summary>
        /// Long description for this unit type.
        /// </summary>
        public abstract string LongName
        {
            get;
        }

        public override int GetHashCode()
        {
            return 7 + 31 * GetType().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return obj.GetType() == GetType();
        }
    }
}
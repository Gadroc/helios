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
    public abstract class BindingValueUnitConverter
    {
        /// <summary>
        /// Returns a list of units which this converter can convert between.  Converter must be able to convert betwen any of the
        /// given return units.
        /// </summary>
        public abstract bool CanConvert(BindingValueUnit from, BindingValueUnit to);

        /// <summary>
        /// Converts value from one unit to another.
        /// </summary>
        /// <param name="value">Value which will be converted.</param>
        /// <param name="from">Unit which this value is currently in.</param>
        /// <param name="to">Unit to use for the return value.</param>
        /// <returns></returns>
        public abstract BindingValue Convert(BindingValue value, BindingValueUnit from, BindingValueUnit to);
    }
}

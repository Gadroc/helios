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

namespace GadrocsWorkshop.Helios.Units
{
    public abstract class TimeUnit : BindingValueUnit
    {
        public override BindingValueUnitType Type
        {
            get { return BindingValueUnitType.Time; }
        }

        /// <summary>
        /// Returns the factor which represents the number of seconds in one of this unit.
        /// Ex: Hour = 3600 seconds per hour so ConverstionFactor is 3600.
        /// </summary>
        public abstract double ConversionFactor { get; }
    }
}

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

namespace GadrocsWorkshop.Helios.Base.UnitConverters
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Units;

    [HeliosUnitConverter]
    public class TimeConverter : BindingValueUnitConverter
    {
        public override bool CanConvert(BindingValueUnit from, BindingValueUnit to)
        {
            return (from is TimeUnit && to is TimeUnit);
        }

        public override BindingValue Convert(BindingValue value, BindingValueUnit from, BindingValueUnit to)
        {
            TimeUnit fromUnit = from as TimeUnit;
            TimeUnit toUnit = to as TimeUnit;

            if (fromUnit != null && toUnit != null)
            {
                double newValue = (value.DoubleValue * fromUnit.ConversionFactor) / toUnit.ConversionFactor;
                return new BindingValue(newValue);
            }
            else
            {
                return BindingValue.Empty;
            }
        }
    }
}

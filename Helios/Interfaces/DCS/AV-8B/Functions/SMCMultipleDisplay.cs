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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.AV8B.Functions
{
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using System;
    using System.Globalization;

    public class SMCMultipleDisplay : NetworkFunction
    {
        private static DCSDataElement[] _dataElements = new DCSDataElement[] { new DCSDataElement("2021", null, false) };

        private HeliosValue _one_digit_display;

        public SMCMultipleDisplay(BaseUDPInterface sourceInterface)
            : base(sourceInterface)
        {
            _one_digit_display = new HeliosValue(sourceInterface, BindingValue.Empty, "Stores Management", "Stores multiple display", "Multiple value", "", BindingValueUnits.Numeric);
            Values.Add(_one_digit_display);
            Triggers.Add(_one_digit_display);
        }

        public override ExportDataElement[] GetDataElements()
        {
            return _dataElements;
        }

        public override void ProcessNetworkData(string id, string value)
        {

                    double displayvalue = Convert.ToDouble(value);
                    _one_digit_display.SetValue(new BindingValue(displayvalue), false);


        }

        private double ClampedParse(string value, double scale)
        {
            double scaledValue = 0d;
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out scaledValue))
            {
                if (scaledValue < 1.0d)
                {
                    scaledValue = Math.Truncate(scaledValue * 10d) * scale;
                }
                else
                {
                    scaledValue = 0d;
                }
            }
            return scaledValue;
        }


        public override void Reset()
        {
            _one_digit_display.SetValue(BindingValue.Empty, true);
        }
    }
}

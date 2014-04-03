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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.A10C.Functions
{
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using System;
    using System.Globalization;

    public class TotalFuel : NetworkFunction
    {
        private static DCSDataElement[] _dataElements = new DCSDataElement[] { new DCSDataElement("2090", null, true) };

        private HeliosValue _fuel;

        public TotalFuel(BaseUDPInterface sourceInterface)
            : base(sourceInterface)
        {
            _fuel = new HeliosValue(sourceInterface, BindingValue.Empty, "Fuel Gauge", "Total Fuel", "Fuel amount shown on the totalizer.", "", BindingValueUnits.Pounds);
            Values.Add(_fuel);
            Triggers.Add(_fuel);
        }

        public override ExportDataElement[] GetDataElements()
        {
            return _dataElements;
        }

        public override void ProcessNetworkData(string id, string value)
        {
            string[] parts = value.Split(';');

            double tenThousands = ClampedParse(parts[0], 10000d);
            double thousands = ClampedParse(parts[1], 1000d);
            double hundreds = Parse(parts[2], 100d);

            double fuel = tenThousands + thousands + hundreds;
            _fuel.SetValue(new BindingValue(fuel), false);
        }

        private double Parse(string value, double scale)
        {
            double scaledValue = 0d;
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out scaledValue))
            {
                if (scaledValue < 1.0d)
                {
                    scaledValue *= scale * 10d;
                }
                else
                {
                    scaledValue = 0d;
                }
            }
            return scaledValue;
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
            _fuel.SetValue(BindingValue.Empty, true);
        }
    }
}

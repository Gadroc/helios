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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.BlackShark.Functions
{
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using System;
    using System.Globalization;

    public class MagVariation : NetworkFunction
    {
        private static DCSDataElement[] _dataElements = new DCSDataElement[] { new DCSDataElement("337", "%0.4f", true), new DCSDataElement("596", "%0.4f", true) };

        private double _tens = 0;
        private double _units = 0;

        private HeliosValue _variation;

        public MagVariation(BaseUDPInterface sourceInterface)
            : base(sourceInterface)
        {
            _variation = new HeliosValue(sourceInterface, BindingValue.Empty, "ZMS-3 Magnetic Variation", "Variation Display", "", "", BindingValueUnits.Degrees);
            Values.Add(_variation);
            Triggers.Add(_variation);
        }

        public override ExportDataElement[] GetDataElements()
        {
            return _dataElements;
        }

        public override void ProcessNetworkData(string id, string value)
        {
            switch (id)
            {
                case "337":
                    _tens = ClampedParse(value, 18d);
                    if (_tens < 0) _tens += 360d;
                    break;
                case "596":
                    _units = Parse(value, 1d);
                    break;
            }
           // double distance = _tens;
            double distance = _tens + _units;
            _variation.SetValue(new BindingValue(distance), false);
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
                    if (scaledValue < 0.0d)
                    {
                        scaledValue = Math.Truncate((scaledValue * 18) - 0.1) * 10; // round negative values
                    }
                    else
                    {
                        scaledValue = Math.Truncate((scaledValue * 18) + 0.1)*10; // round positive values
                    }
 
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
            _variation.SetValue(BindingValue.Empty, true);
        }
    }
}

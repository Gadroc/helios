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

    class VHF2Rotary23 : NetworkFunction
    {
        private ExportDataElement[] _elements;
        private HeliosValue _value;

        public VHF2Rotary23(BaseUDPInterface sourceInterface, string id, string device, string name)
            : base(sourceInterface)
        {
            _elements = new ExportDataElement[] { new DCSDataElement(id, "%.2f", true) };
            _value = new HeliosValue(sourceInterface, BindingValue.Empty, device, name, "", "", BindingValueUnits.Numeric);
            Values.Add(_value);
            Triggers.Add(_value);
        }

        public override void ProcessNetworkData(string id, string value)
        {
            double parseValue;
            if (double.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out parseValue))
            {
                double newValue = Math.Truncate(parseValue * 10d);
                _value.SetValue(new BindingValue(newValue), false);
            }
        }

        public override ExportDataElement[] GetDataElements()
        {
            return _elements;
        }

        public override void Reset()
        {
            _value.SetValue(BindingValue.Empty, true);
        }
    }
}
    
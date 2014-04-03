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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.Common
{
    using GadrocsWorkshop.Helios.UDPInterface;
    using System.Globalization;

    public class FlagValue : NetworkFunction
    {
        private string _id;
        private string _format;

        private HeliosValue _value;

        public FlagValue(BaseUDPInterface sourceInterface, string id, string device, string name, string description)
            : this(sourceInterface, id, device, name, description, "%0.1f")
        {
        }

        public FlagValue(BaseUDPInterface sourceInterface, string id, string device, string name, string description, string exportFormat)
            : base(sourceInterface)
        {
            _id = id;
            _format = exportFormat;

            _value = new HeliosValue(sourceInterface, BindingValue.Empty, device, name, description, "", BindingValueUnits.Boolean);
            Values.Add(_value);
            Triggers.Add(_value);
        }

        public override void ProcessNetworkData(string id, string value)
        {
            double parsedValue;
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out parsedValue))
            {
                _value.SetValue(new BindingValue(parsedValue != 0d), false);
            }
        }

        public override ExportDataElement[] GetDataElements()
        {
            return new ExportDataElement[] { new DCSDataElement(_id, _format, true) };
        }

        public override void Reset()
        {
            _value.SetValue(BindingValue.Empty, true);
        }
    }
}

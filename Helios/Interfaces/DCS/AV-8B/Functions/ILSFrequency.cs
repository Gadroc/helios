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
    using System.Collections.ObjectModel;
    using System.Globalization;

    class ILSFrequency : NetworkFunction
    {
        private static DCSDataElement[] _dataElements = new DCSDataElement[] { new DCSDataElement("251", "%0.1f", false), new DCSDataElement("252", "%0.1f", false) };

        private double _mhz = 108;
        private double _khz = .10;

        private HeliosValue _frequency;

        public ILSFrequency(BaseUDPInterface sourceInterface)
            : base(sourceInterface)
        {
            _frequency = new HeliosValue(sourceInterface, BindingValue.Empty, "ILS", "Frequency", "Currently tuned ILS frequency.", "", BindingValueUnits.Numeric);
            Values.Add(_frequency);
            Triggers.Add(_frequency);
        }

        public override ExportDataElement[] GetDataElements()
        {
            return _dataElements;
        }

        public override void ProcessNetworkData(string id, string value)
        {
            switch (id)
            {
                case "251":
                    _mhz = 108 + (double.Parse(value, CultureInfo.InvariantCulture) * 10);
                    break;
                case "252":
                    switch (value)
                    {
                        case "0.0":
                            _khz = .10;
                            break;
                        case "0.1":
                            _khz = .15;
                            break;
                        case "0.2":
                            _khz = .30;
                            break;
                        case "0.3":
                            _khz = .35;
                            break;
                        case "0.4":
                            _khz = .50;
                            break;
                        case "0.5":
                            _khz = .55;
                            break;
                        case "0.6":
                            _khz = .70;
                            break;
                        case "0.7":
                            _khz = .75;
                            break;
                        case "0.8":
                            _khz = .90;
                            break;
                        case "0.9":
                            _khz = .95;
                            break;
                    }
                    break;
            }

            _frequency.SetValue(new BindingValue(_mhz + _khz), false);
        }

        public override void Reset()
        {
            _frequency.SetValue(BindingValue.Empty, true);
        }
    }
}

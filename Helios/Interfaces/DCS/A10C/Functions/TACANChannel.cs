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

    public class TACANChannel : NetworkFunction
    {
        private static DCSDataElement[] _dataElements = new DCSDataElement[] { new DCSDataElement("2263", null, false), new DCSDataElement("266", "0.1f", false) };

        private static BindingValue _xValue = new BindingValue(1);
        private static BindingValue _yValue = new BindingValue(2);

        private double _hundreds;
        private double _tens;
        private double _ones;

        private HeliosValue _channel;
        private HeliosValue _mode;

        public TACANChannel(BaseUDPInterface sourceInterface)
            : base(sourceInterface)
        {
            _channel = new HeliosValue(sourceInterface, BindingValue.Empty, "TACAN", "Channel", "Currently tuned TACAN channel.", "", BindingValueUnits.Numeric);
            Values.Add(_channel);
            Triggers.Add(_channel);

            _mode = new HeliosValue(sourceInterface, BindingValue.Empty, "TACAN", "Channel Mode", "Current TACAN channel mode.", "1=X, 2=Y", BindingValueUnits.Numeric);
            Values.Add(_mode);
            Triggers.Add(_mode);
        }

        public override ExportDataElement[] GetDataElements()
        {
            return _dataElements;
        }

        public override void ProcessNetworkData(string id, string value)
        {
            switch (id)
            {
                case "2263":
                    string[] parts = value.Split(';');
                    _hundreds = ClampedParse(parts[0], 100d);
                    _tens = ClampedParse(parts[1], 10d);
                    _ones = ClampedParse(parts[2], 1d);

                    double channel = _hundreds + _tens + _ones;
                    _channel.SetValue(new BindingValue(channel), false);
                    break;
                case "266":
                    switch (value)
                    {
                        case "0.0":
                            _mode.SetValue(_xValue, false);
                            break;
                        case "0.1":
                            _mode.SetValue(_yValue, false);
                            break;
                    }
                    break;
            }
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
            _channel.SetValue(BindingValue.Empty, true);
            _mode.SetValue(BindingValue.Empty, true);
        }
    }
}

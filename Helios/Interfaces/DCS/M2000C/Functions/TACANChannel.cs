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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.M2000C.Functions
{
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using System;
    using System.Globalization;

    public class TACANChannel : NetworkFunction
    {
        private static DCSDataElement[] _dataElements = new DCSDataElement[] { new DCSDataElement("623", "%.3f", false), new DCSDataElement("625", "%.3f", false) };

//        private static BindingValue _xValue = new BindingValue(1);
//        private static BindingValue _yValue = new BindingValue(2);

        private double _tens;
        private double _ones;

        private HeliosValue _channel;

        public TACANChannel(BaseUDPInterface sourceInterface)
            : base(sourceInterface)
        {
            _channel = new HeliosValue(sourceInterface, BindingValue.Empty, "TACAN", "Channel", "Currently tuned TACAN channel.", "", BindingValueUnits.Numeric);
            Values.Add(_channel);
            Triggers.Add(_channel);
        }

        public override ExportDataElement[] GetDataElements()
        {
            return _dataElements;
        }

        public override void ProcessNetworkData(string id, string value)
        {
            switch (id)
            {
                case "623":
                    switch (value)
                    {
                        case "0.00":
                            _tens = 0.0;
                            break;
                        case "0.08":
                            _tens = 10.0;
                            break;
                        case "0.15":
                            _tens = 20.0;
                            break;
                        case "0.23":
                            _tens = 30.0;
                            break;
                        case "0.31":
                            _tens = 40.0;
                            break;
                        case "0.38":
                            _tens = 50.0;
                            break;
                        case "0.46":
                            _tens = 60.0;
                            break;
                        case "0.54":
                            _tens = 70.0;
                            break;
                        case "0.62":
                            _tens = 80.0;
                            break;
                        case "0.69":
                            _tens = 90.0;
                            break;
                        case "0.77":
                            _tens = 100.0;
                            break;
                        case "0.85":
                            _tens = 110.0;
                            break;
                        case "0.92":
                            _tens = 120.0;
                            break;
                    }
                    break;
                case "625":
                    switch (value)
                    {
                        case "0.0":
                            _ones = 0;
                            break;
                        case "0.1":
                            _ones = 1;
                            break;
                        case "0.2":
                            _ones = 2;
                            break;
                        case "0.3":
                            _ones = 3;
                            break;
                        case "0.4":
                            _ones = 4;
                            break;
                        case "0.5":
                            _ones = 5;
                            break;
                        case "0.6":
                            _ones = 6;
                            break;
                        case "0.7":
                            _ones = 7;
                            break;
                        case "0.8":
                            _ones = 8;
                            break;
                        case "0.9":
                            _ones = 9;
                            break;
                    }
                    break;
            }
            _channel.SetValue(new BindingValue(_tens + _ones), false);
        }
/*
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

    */
        public override void Reset()
        {
            _channel.SetValue(BindingValue.Empty, true);
        }
    }
}

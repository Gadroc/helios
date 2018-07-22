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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.FA18C.Functions
{
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;

    class Altimeter : NetworkFunction
    {
        private static DCSDataElement[] _dataElements;
        private HeliosValue _altitude;
        private HeliosValue _pressure;
        private string _altID;
        private string _pressID;

        public Altimeter(BaseUDPInterface sourceInterface, string instrumentClass, string altitudeDeviceId, string altitudeName, string altitudeDescription, string altitudeComments, string pressureDeviceId, string pressureName, string pressureDescription, string pressureComments)
            : base(sourceInterface)
        {
            _dataElements = new DCSDataElement[] { new DCSDataElement(altitudeDeviceId, null, true), new DCSDataElement(pressureDeviceId, null, true) };
            _altID = altitudeDeviceId;
            _pressID = pressureDeviceId;
            _altitude = new HeliosValue(sourceInterface, BindingValue.Empty, "Standby Baro Altimeter AAU-52/A", "Altitude", "Barometric altitude above sea level of the aircraft.", "Value is adjusted per altimeter pressure setting.", BindingValueUnits.Feet);
            Values.Add(_altitude);
            Triggers.Add(_altitude);

            _pressure = new HeliosValue(sourceInterface, BindingValue.Empty, "Standby Baro Altimeter AAU-52/A", "Pressure", "Manually set barometric altitude.", "", BindingValueUnits.InchesOfMercury);
            Values.Add(_pressure);
            Triggers.Add(_pressure);
        }

        public override ExportDataElement[] GetDataElements()
        {
            return _dataElements;
        }

        public override void ProcessNetworkData(string id, string value)
        {
            string[] parts = value.Split(';');

            if (id == _altID)
            {
                double altitude = ClampedParse(parts[0], 10000d) + ClampedParse(parts[1], 1000d) + Parse(parts[2], 100d);
                //ConfigManager.LogManager.LogDebug("F/A-18C Interface Argument " + id.ToString() + " value = " + altitude.ToString());
                _altitude.SetValue(new BindingValue(altitude), false);
            }
            else if (id == _pressID)
            {
                double pressure = ClampedParse(parts[0], 1d, 26d, 5d) + ClampedParse(parts[1], .1d) + ClampedParse(parts[2], .01d);
                //ConfigManager.LogManager.LogDebug("F/A-18C Interface Argument " + id.ToString() + " value = " + pressure.ToString());
                _pressure.SetValue(new BindingValue(pressure), false);
            } else
            {
            }
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
        private double ClampedParse(string value, double scale, double offset, double mult)
        {
            double scaledValue = 0d;
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out scaledValue))
            {
                if (scaledValue < 1.0d)
                {
                    scaledValue = (Math.Truncate(scaledValue * mult)+ offset) * scale;
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
            _altitude.SetValue(BindingValue.Empty, true);
            _pressure.SetValue(BindingValue.Empty, true);
        }

    }
}

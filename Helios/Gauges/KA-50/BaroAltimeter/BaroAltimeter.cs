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

namespace GadrocsWorkshop.Helios.Gauges.KA_50.BaroAltimeter
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.KA50.BarometricAltimeter", "Barometric Altimeter", "KA-50 Gauges", typeof(GaugeRenderer))]
    public class BarometricAltimeter : BaseGauge
    {
        private GaugeNeedle _qfeCard;
        private GaugeNeedle _shortNeedle;
        private GaugeNeedle _longNeedle;
        private GaugeNeedle _altitudeBug;

        private HeliosValue _altitude;
        private HeliosValue _commandedAltitude;
        private HeliosValue _qfePressure;

        private CalibrationPointCollectionDouble _qfeCalibration;
        private CalibrationPointCollectionDouble _needleCalibration;

        public BarometricAltimeter()
            : base("Barometric Altimeter", new Size(340, 340))
        {
            Point center = new Point(170, 170);

            _qfeCalibration = new CalibrationPointCollectionDouble(600d, 0d, 800d, 300d);
            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 10d, 360d);

            _qfeCard = new GaugeNeedle("{Helios}/Gauges/KA-50/BaroAltimeter/baro_alt_qfe_card.xaml", center, new Size(264, 264), new Point(132, 132), 300d);
            Components.Add(_qfeCard);

            Components.Add(new GaugeImage("{Helios}/Gauges/KA-50/BaroAltimeter/baro_alt_faceplate.xaml", new Rect(0, 0, 340, 340)));

            _shortNeedle = new GaugeNeedle("{Helios}/Gauges/KA-50/BaroAltimeter/baro_alt_short_needle.xaml", center, new Size(35, 144), new Point(17.5, 104));
            Components.Add(_shortNeedle);

            _longNeedle = new GaugeNeedle("{Helios}/Gauges/KA-50/BaroAltimeter/baro_alt_long_needle.xaml", center, new Size(40, 209), new Point(20, 139));
            Components.Add(_longNeedle);

            _altitudeBug = new GaugeNeedle("{Helios}/Gauges/KA-50/BaroAltimeter/baro_alt_bug.xaml", center, new Size(14, 15), new Point(7, 150));
            Components.Add(_altitudeBug);

            Components.Add(new GaugeImage("{Helios}/Gauges/KA-50/BaroAltimeter/baro_alt_bezel.xaml", new Rect(0, 0, 340, 340)));

            _altitude = new HeliosValue(this, BindingValue.Empty, "", "Altitude", "Current barometric altitude", "", BindingValueUnits.Meters);
            _altitude.Execute += Altitude_Execute;
            Actions.Add(_altitude);

            _commandedAltitude = new HeliosValue(this, BindingValue.Empty, "", "Commanded Altitude", "Current commanded altitude", "", BindingValueUnits.Meters);
            _commandedAltitude.Execute += CommandedAltitude_Execute;
            Actions.Add(_commandedAltitude);

            _qfePressure = new HeliosValue(this, BindingValue.Empty, "", "QFE Pressure", "Current calibrated qfe pressure", "", BindingValueUnits.MilimetersOfMercury);
            _qfePressure.Execute += Pressure_Execute;
            Actions.Add(_qfePressure);
        }

        private void Pressure_Execute(object action, HeliosActionEventArgs e)
        {
            _qfePressure.SetValue(e.Value, e.BypassCascadingTriggers);
            _qfeCard.Rotation = -_qfeCalibration.Interpolate(e.Value.DoubleValue);
        }

        private void Altitude_Execute(object action, HeliosActionEventArgs e)
        {
            _altitude.SetValue(e.Value, e.BypassCascadingTriggers);
            _shortNeedle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue / 1000d);
            _longNeedle.Rotation = _needleCalibration.Interpolate((e.Value.DoubleValue % 1000d) / 100d);
        }

        private void CommandedAltitude_Execute(object action, HeliosActionEventArgs e)
        {
            _commandedAltitude.SetValue(e.Value, e.BypassCascadingTriggers);
            _altitudeBug.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue / 1000d);
        }
    }
}

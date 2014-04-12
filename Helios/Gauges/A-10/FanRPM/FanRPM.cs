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

namespace GadrocsWorkshop.Helios.Gauges.A_10.FanRPM
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.A10.FanRPM", "Fan RPM", "A-10 Gauges", typeof(GaugeRenderer))]
    public class FanRPM : BaseGauge
    {
        private HeliosValue _rpm;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public FanRPM()
            : base("Fan RPM", new Size(360, 360))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/FanRPM/eng_fan_rpm_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 100d, 330d);
            _needleCalibration.Add(new CalibrationPointDouble(75d, 150d));
            _needle = new GaugeNeedle("{Helios}/Gauges/A-10/Common/eng_needle.xaml", new Point(180d, 180d), new Size(69d, 161d), new Point(34.5d, 126.5d), 30d);
            Components.Add(_needle);

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/engine_bezel.png", new Rect(0d, 0d, 360d, 360d)));

            _rpm = new HeliosValue(this, new BindingValue(0d), "", "rpm", "Current RPM of the aircraft enigne fan.", "(0 - 100)", BindingValueUnits.RPMPercent);
            _rpm.Execute += new HeliosActionHandler(RPM_Execute);
            Actions.Add(_rpm);
        }

        void RPM_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

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

namespace GadrocsWorkshop.Helios.Gauges.F_16.RPM
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.F16.RPM", "RPM", "F-16", typeof(GaugeRenderer))]
    public class RPM : BaseGauge
    {
        private HeliosValue _rpm;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public RPM()
            : base("RPM", new Size(360, 360))
        {
            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 110d, 337.5d);
            _needleCalibration.Add(new CalibrationPointDouble(60d, 108d));

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/RPM/rpm_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));

            _needle = new GaugeNeedle("{Helios}/Gauges/F-16/RPM/rpm_needle.xaml", new Point(180d, 180d), new Size(60d, 144d), new Point(30d, 114d));
            Components.Add(_needle);

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/Common/f16_engine_bezel.png", new Rect(0d, 0d, 360d, 360d)));

            _rpm = new HeliosValue(this, new BindingValue(0d), "", "rpm", "Current RPM of the engine.", "", BindingValueUnits.RPMPercent);
            _rpm.SetValue(new BindingValue(29.92), true);
            _rpm.Execute += new HeliosActionHandler(RPM_Execute);
            Actions.Add(_rpm);
        }

        void RPM_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

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

namespace GadrocsWorkshop.Helios.Gauges.A_10.EngineRPM
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.A10.EngineRPM", "Engine RPM", "A-10 Gauges", typeof(GaugeRenderer))]
    public class EngineRPM : BaseGauge
    {
        private HeliosValue _rpm;
        private GaugeNeedle _needle;
        private GaugeNeedle _smallNeedle;
        private CalibrationPointCollectionDouble _needleCalibration;
        private CalibrationPointCollectionDouble _smallNeedleCalibration;

        public EngineRPM()
            : base("Engine RPM", new Size(360, 360))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/EngineRPM/eng_rpm_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/eng_inner_faceplate.xaml", new Rect(70d, 70d, 95d, 95d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 100d, 270d);
            _needle = new GaugeNeedle("{Helios}/Gauges/A-10/Common/eng_needle.xaml", new Point(180d, 180d), new Size(69d, 161d), new Point(34.5d, 126.5d));
            Components.Add(_needle);

            _smallNeedleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 10d, 360d);
            _smallNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/Common/eng_small_needle.xaml", new Point(117.5d, 117.5d), new Size(17d, 45d), new Point(8.5d, 35d));
            Components.Add(_smallNeedle);

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/engine_bezel.png", new Rect(0d, 0d, 360d, 360d)));

            _rpm = new HeliosValue(this, new BindingValue(0d), "", "rpm", "Current RPM of the aircraft enigne.", "(0 - 100)", BindingValueUnits.RPMPercent);
            _rpm.Execute += new HeliosActionHandler(RPM_Execute);
            Actions.Add(_rpm);
        }

        void RPM_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
            _smallNeedle.Rotation = _smallNeedleCalibration.Interpolate(e.Value.DoubleValue % 10d);
        }
    }
}

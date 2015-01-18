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

    [HeliosControl("Helios.A10.EngineTemp", "Engine Temp", "A-10 Gauges", typeof(GaugeRenderer))]
    public class EngineTemp : BaseGauge
    {
        private HeliosValue _temperature;
        private GaugeNeedle _needle;
        private GaugeNeedle _smallNeedle;
        private CalibrationPointCollectionDouble _needleCalibration;
        private CalibrationPointCollectionDouble _smallNeedleCalibration;

        public EngineTemp()
            : base("Engine Temp", new Size(360, 360))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/EngineTemp/eng_temp_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));
            //Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/eng_inner_faceplate.xaml", new Rect(70d, 70d, 95d, 95d)));
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/eng_inner_faceplate.xaml", new Rect(223d, 133d, 95d, 95d)));

            _needleCalibration = new CalibrationPointCollectionDouble(100d, 0d, 1200d, 275d);
            _needle = new GaugeNeedle("{Helios}/Gauges/A-10/Common/eng_needle.xaml", new Point(180d, 180d), new Size(69d, 161d), new Point(34.5d, 126.5d), 130d);
            Components.Add(_needle);

            _smallNeedleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 100d, 360d);
            _smallNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/Common/eng_small_needle.xaml", new Point(270, 180), new Size(17, 45), new Point(8.5, 35));
            Components.Add(_smallNeedle);

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/engine_bezel.png", new Rect(0d, 0d, 360d, 360d)));

            _temperature = new HeliosValue(this, new BindingValue(0d), "", "temperature", "Current temperature of the aircraft enigne.", "(0 - 1200)", BindingValueUnits.Celsius);
            _temperature.Execute += new HeliosActionHandler(Temperature_Execute);
            Actions.Add(_temperature);
        }

        void Temperature_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
            _smallNeedle.Rotation = _smallNeedleCalibration.Interpolate(e.Value.DoubleValue % 100d);
        }
    }
}

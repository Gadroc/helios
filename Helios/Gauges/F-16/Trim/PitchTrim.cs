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

namespace GadrocsWorkshop.Helios.Gauges.F_16.Trim
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.F16.PitchTrim", "Pitch Trim", "F-16", typeof(GaugeRenderer))]
    public class PitchTrim : BaseGauge
    {
        private HeliosValue _trim;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public PitchTrim()
            : base("Trim", new Size(200, 100))
        {
            _needleCalibration = new CalibrationPointCollectionDouble(-0.5d, -90d, 0.5d, 90d);
            _needleCalibration.Add(new CalibrationPointDouble(60d, 108d));

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/Trim/pitchtrim_faceplate.xaml", new Rect(0d, 0d, 200d, 100d)));

            _needle = new GaugeNeedle("{Helios}/Gauges/F-16/Trim/pitchtrim_needle.xaml", new Point(100d, 7d), new Size(14d, 90d), new Point(7d, 7d));
            //_needle.Clip = new RectangleGeometry(new Rect(0d, 0d, 199d, 99d));
            Components.Add(_needle);

            _trim = new HeliosValue(this, new BindingValue(0d), "", "pitch trim", "Current amount of pitch trim currently set.", "(-0.5 to 0.5)", BindingValueUnits.Numeric);
            _trim.SetValue(new BindingValue(29.92), true);
            _trim.Execute += new HeliosActionHandler(Trim_Execute);
            Actions.Add(_trim);
        }

        void Trim_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

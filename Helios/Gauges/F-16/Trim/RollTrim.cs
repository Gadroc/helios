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

    [HeliosControl("Helios.F16.RollTrim", "Roll Trim", "F-16", typeof(GaugeRenderer))]
    public class RollTrim : BaseGauge
    {
        private HeliosValue _trim;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public RollTrim()
            : base("Trim", new Size(100, 200))
        {
            _needleCalibration = new CalibrationPointCollectionDouble(-0.5d, -90d, 0.5d, 90d);
            _needleCalibration.Add(new CalibrationPointDouble(60d, 108d));

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/Trim/rolltrim_faceplate.xaml", new Rect(0d, 0d, 100d, 200d)));

            _needle = new GaugeNeedle("{Helios}/Gauges/F-16/Trim/rolltrim_needle.xaml", new Point(93d, 100d), new Size(90d, 14d), new Point(83d, 7d));
            //_needle.Clip = new RectangleGeometry(new Rect(0d, 0d, 100d, 200d));
            Components.Add(_needle);

            _trim = new HeliosValue(this, new BindingValue(0d), "", "roll trim", "Current amount of roll trim currently set.", "(-0.5 to 0.5)", BindingValueUnits.Numeric);
            _trim.SetValue(new BindingValue(29.92), true);
            _trim.Execute += new HeliosActionHandler(Trim_Execute);
            Actions.Add(_trim);
        }

        void Trim_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = -_needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

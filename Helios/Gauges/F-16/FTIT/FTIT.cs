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

namespace GadrocsWorkshop.Helios.Gauges.F_16.FTIT
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.F16.FTIT", "FTIT", "F-16", typeof(GaugeRenderer))]
    public class FTIT : BaseGauge
    {
        private HeliosValue _ftit;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public FTIT()
            : base("FTIT", new Size(360, 360))
        {
            _needleCalibration = new CalibrationPointCollectionDouble(200d, 18d, 1200d, 342d);
            _needleCalibration.Add(new CalibrationPointDouble(700d, 108d));
            _needleCalibration.Add(new CalibrationPointDouble(1000d, 306d));

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/FTIT/ftit_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));

            _needle = new GaugeNeedle("{Helios}/Gauges/F-16/FTIT/ftit_needle.xaml", new Point(180d, 180d), new Size(60d, 144d), new Point(30d, 114d), 90d);
            _needle.Rotation = _needleCalibration.Interpolate(0);
            Components.Add(_needle);

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/Common/f16_engine_bezel.png", new Rect(0d, 0d, 360d, 360d)));

            _ftit = new HeliosValue(this, new BindingValue(0d), "", "ftit", "Current fan turbine inlet temperature of the engine.", "", BindingValueUnits.Celsius);
            _ftit.SetValue(new BindingValue(29.92), true);
            _ftit.Execute += new HeliosActionHandler(FTIT_Execute);
            Actions.Add(_ftit);
        }

        void FTIT_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

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

namespace GadrocsWorkshop.Helios.Gauges.A_10.Flaps
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.A10.Flaps", "Flaps", "A-10 Gauges", typeof(GaugeRenderer))]
    public class Flaps : BaseGauge
    {
        private HeliosValue _position;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public Flaps()
            : base("Flaps", new Size(360, 360))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Flaps/flaps_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 30d, 90d);
            _needle = new GaugeNeedle("{Helios}/Gauges/A-10/Common/eng_needle.xaml", new Point(180d, 180d), new Size(69d, 161d), new Point(34.5d, 126.5d), 90d);
            Components.Add(_needle);

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Flaps/flaps_cover.xaml", new Rect(30d, 30d, 300d, 300d)));

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/engine_bezel.png", new Rect(0d, 0d, 360d, 360d)));

            _position = new HeliosValue(this, new BindingValue(0d), "", "position", "Current position of the flaps on the aircraft.", "(0 - 30)", BindingValueUnits.Degrees);
            _position.Execute += new HeliosActionHandler(Position_Execute);
            Actions.Add(_position);
        }

        void Position_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

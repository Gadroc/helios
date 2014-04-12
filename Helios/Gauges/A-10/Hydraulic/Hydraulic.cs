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

namespace GadrocsWorkshop.Helios.Gauges.A_10.Hydraulic
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.A10.Hydraulic", "Hydraulic Pressure", "A-10 Gauges", typeof(GaugeRenderer))]
    public class Hydraulic : BaseGauge
    {
        private HeliosValue _pressure;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public Hydraulic()
            : base("Hydraulic Pressure", new Size(360, 360))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Hydraulic/hydraulic_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 4000d, 296d);
            _needle = new GaugeNeedle("{Helios}/Gauges/A-10/Hydraulic/hydraulic_needle.xaml", new Point(180d, 180d), new Size(58d, 164d), new Point(29d, 135d), 103.5d);
            Components.Add(_needle);

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/engine_bezel.png", new Rect(0d, 0d, 360d, 360d)));

            _pressure = new HeliosValue(this, new BindingValue(0d), "", "pressure", "Current pressure for the hydraulic system.", "", BindingValueUnits.PoundsPerSquareInch);
            _pressure.Execute += new HeliosActionHandler(Pressure_Execute);
            Actions.Add(_pressure);
        }

        void Pressure_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

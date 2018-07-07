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

namespace GadrocsWorkshop.Helios.Gauges.FA18C.Hydraulic
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.FA18C.Hydraulic", "Hydraulic Pressure", "F/A-18C Gauges", typeof(GaugeRenderer))]
    public class Hydraulic : BaseGauge
    {
        private HeliosValue _pressure;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public Hydraulic()
            : base("Hydraulic Pressure", new Size(300, 300))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/FA-18C/HydraulicPressure/Hyd_Pressure_Faceplate.png", new Rect(0d, 0d, 300d, 300d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 80d, 5000d, 40d);
            _needle = new GaugeNeedle("{Helios}/Gauges/AV-8B/Common/needle_a.xaml", new Point(150d, 150d), new Size(30, 128), new Point(15, 113), 80d);
            Components.Add(_needle);
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

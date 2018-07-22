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

namespace GadrocsWorkshop.Helios.Gauges.FA18C.Instruments
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.FA18C.Instruments", "RADAR Altimeter", "F/A-18C Gauges", typeof(GaugeRenderer))]
    public class RAltimeter : BaseGauge
    {
        private HeliosValue _altitdue;
        private GaugeNeedle _needle;
        private HeliosValue _min_altitdue;
        private GaugeNeedle _minimum_needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public RAltimeter()
            : base("RADAR Altimeter", new Size(420, 420))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/FA-18C/Altimeter/RADAR_Altimeter_Faceplate.png", new Rect(0d, 0d, 420d, 420d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 5000d, 330d);
            _needle = new GaugeNeedle("{Helios}/Gauges/FA-18C/Altimeter/altimeter_needle.xaml", new Point(210d, 210d), new Size(16d, 250d), new Point(8d, 200d),0d);
            Components.Add(_needle);
            _minimum_needle = new GaugeNeedle("{Helios}/Gauges/FA-18C/Altimeter/RADAR_Altimeter_Min_Needle.xaml", new Point(210d, 210d), new Size(46d, 205d), new Point(23d, 205d),-10d);
            Components.Add(_minimum_needle);

            Components.Add(new GaugeImage("{Helios}/Gauges/FA-18C/Altimeter/RADAR_Altimeter_Cover.png", new Rect(94d, 11d, 89d, 88d)));  // this is the needle cover

            //Components.Add(new GaugeImage("{Helios}/Gauges/FA-18C/Common/engine_bezel.png", new Rect(0d, 0d, 400d, 400d)));

            _altitdue = new HeliosValue(this, new BindingValue(0d), "", "RADAR altitude", "Current RADAR altitude of the aircraft.", "", BindingValueUnits.Feet);
            _altitdue.Execute += new HeliosActionHandler(Altitude_Execute);
            Actions.Add(_altitdue);
            _min_altitdue = new HeliosValue(this, new BindingValue(0d), "", "RADAR Altimeter Minimum", "Minimum Altitude setting for the aricraft in feet.", "", BindingValueUnits.Feet);
            _min_altitdue.Execute += new HeliosActionHandler(Min_Altitude_Execute);
            Actions.Add(_min_altitdue);

        }

        void Altitude_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
        void Min_Altitude_Execute(object action, HeliosActionEventArgs e)
        {
            _minimum_needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

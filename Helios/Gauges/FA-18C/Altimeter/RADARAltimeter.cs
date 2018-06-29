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

    [HeliosControl("Helios.FA18C.Instruments.RAltimeter", "RADAR Altimeter", "F/A-18C Gauges", typeof(GaugeRenderer))]
    public class RAltimeter : BaseGauge
    {
        private HeliosValue _altitdue;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;
        private GaugeDrumCounter _drum;

        public RAltimeter()
            : base("RADAR Altimeter", new Size(400, 400))
        {

            _drum = new GaugeDrumCounter("{Helios}/Gauges/FA-18C/Common/drum_tape.xaml", new Point(165d, 276d), "###%00", new Size(65d, 26d), new Size(65d, 26d));
            _drum.Clip = new RectangleGeometry(new Rect(165d, 276d, 65d, 26d));
            Components.Add(_drum);

            Components.Add(new GaugeImage("{Helios}/Gauges/FA-18C/Altimeter/RADAR_Altimeter_Faceplate.png", new Rect(0d, 0d, 400d, 400d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 1000d, 360d);
            _needle = new GaugeNeedle("{Helios}/Gauges/FA-18C/Altimeter/altimeter_needle.xaml", new Point(200d, 200d), new Size(16d, 250d), new Point(8d, 200d));
            Components.Add(_needle);

            //Components.Add(new GaugeImage("{Helios}/Gauges/FA-18C/Common/engine_bezel.png", new Rect(0d, 0d, 400d, 400d)));

            _altitdue = new HeliosValue(this, new BindingValue(0d), "", "RADAR altitude", "Current RADAR altitude of the aricraft.", "", BindingValueUnits.Feet);
            _altitdue.Execute += new HeliosActionHandler(Altitude_Execute);
            Actions.Add(_altitdue);
        }

        void Altitude_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue % 1000d);
            _drum.Value = e.Value.DoubleValue;
        }
     }
}

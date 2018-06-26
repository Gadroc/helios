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

namespace GadrocsWorkshop.Helios.Gauges.AV8B
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

  public class trimPosition: BaseGauge
    {
        private HeliosValue _angle;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public trimPosition(GaugeImage GI,String gaugeName,Size gaugeSize)
            : base(gaugeName, gaugeSize)
        {
            Components.Add(GI);

            _needleCalibration = new CalibrationPointCollectionDouble(-1.0d, -45d, 1.0d, 45d);
            //_needleCalibration.Add(new CalibrationPointDouble(-0.50d, -35d));
            //_needleCalibration.Add(new CalibrationPointDouble(-0.10d, -22d));
            //_needleCalibration.Add(new CalibrationPointDouble(0d, 0d));
            //_needleCalibration.Add(new CalibrationPointDouble(0.10d, 22d));
            //_needleCalibration.Add(new CalibrationPointDouble(0.50d, 35d));

            _needle = new GaugeNeedle("{Helios}/Gauges/AV-8B/Engine Panel/nozzle_needle.xaml", new Point(150d, 150d), new Size(36d, 140d), new Point(18d, 122d), 0d);
            Components.Add(_needle);

            _angle = new HeliosValue(this, new BindingValue(0d), "", "angle", "Current position of trim.", "", BindingValueUnits.Degrees);
            _angle.Execute += new HeliosActionHandler(Angle_Execute);
            Actions.Add(_angle);
        }

        void Angle_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

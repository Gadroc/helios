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

    [HeliosControl("Helios.AV8B.edpNoz", "EDP Nozzle", "AV-8B", typeof(GaugeRenderer))]
    public class edpNoz: BaseGauge
    {
        private HeliosValue _angle;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public edpNoz()
            : base("EDP Nozzle", new Size(12, 48))
        {
            //Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/Engine Panel/edp_faceplate.xaml", new Rect(0d, 0d, 528d, 302d)));
            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 0.94d, 130d);
            _needle = new GaugeNeedle("{Helios}/Gauges/AV-8B/Engine Panel/nozzle_needle.xaml", new Point(0d, 0d), new Size(12d, 48d), new Point(6d, 42d), 90d);
            Components.Add(_needle);

            _angle = new HeliosValue(this, new BindingValue(0d), "", "nozzle angle", "Current position of Nozzles.", "(0 - 120)", BindingValueUnits.Degrees);
            _angle.Execute += new HeliosActionHandler(Angle_Execute);
            Actions.Add(_angle);
        }

        void Angle_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

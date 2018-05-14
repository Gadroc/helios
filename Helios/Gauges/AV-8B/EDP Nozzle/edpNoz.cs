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

namespace GadrocsWorkshop.Helios.Gauges.AV8B.edpNoz
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.AV8B.edpNoz", "EDP Nozzle", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class edpNoz: BaseGauge
    {
        private HeliosValue _angle;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public edpNoz()
            : base("EDP Nozzle", new Size(80, 80))
        {
            //Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/EDP Nozzle/edp_noz_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 0.94d, 150d);
            _needle = new GaugeNeedle("{Helios}/Gauges/AV-8B/Common/nozzle_needle.xaml", new Point(40d, 40d), new Size(15d, 36d), new Point(7.6d, 28.1d), 90d);
            Components.Add(_needle);

            _angle = new HeliosValue(this, new BindingValue(0d), "", "angle", "Current position of Nozzles.", "(0 - 120)", BindingValueUnits.Degrees);
            _angle.Execute += new HeliosActionHandler(Angle_Execute);
            Actions.Add(_angle);
        }

        void Angle_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

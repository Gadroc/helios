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

    [HeliosControl("Helios.AV8B.Battery", "Battery", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class Battery : BaseGauge
    {
        private HeliosValue _voltage;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _calibrationPoints;

        public Battery()
            : base("voltage", new Size(300, 300))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/Battery/battery_faceplate.xaml", new Rect(0d, 0d, 300d, 300d)));
            _needle = new GaugeNeedle("{Helios}/Gauges/AV-8B/Common/needle_a.xaml", new Point(150d, 150d), new Size(36, 154), new Point(18, 136), -90d);
            Components.Add(_needle);

            //Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/gauge_bezel.png", new Rect(0d, 0d, 364d, 376d)));

            _voltage = new HeliosValue(this, new BindingValue(0d), "", "Battery Voltage", "This is the voltage of the battery in volts", "(15 to 30)", BindingValueUnits.Volts);
            _voltage.Execute += new HeliosActionHandler(voltage_Execute);
            Actions.Add(_voltage);

            _calibrationPoints = new CalibrationPointCollectionDouble(15d, 0d, 30d, 180d);
            _calibrationPoints.Add(new CalibrationPointDouble(0d, 0d));  // used to set an end stop at 15v

        }

        void voltage_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _calibrationPoints.Interpolate(e.Value.DoubleValue);
        }
    }
}

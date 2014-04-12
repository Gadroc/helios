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

namespace GadrocsWorkshop.Helios.Gauges.A_10.LiquidOxygen
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.A10.LiquidOxygen", "Liquid Oxygen", "A-10 Gauges", typeof(GaugeRenderer))]
    public class LiquidOxygen : BaseGauge
    {
        private HeliosValue _liquidOxygen;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public LiquidOxygen()
            : base("Liquid Oxygen", new Size(360, 360))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/LiquidOxygen/liquid_oxygen_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 5d, 180d);
            _needle = new GaugeNeedle("{Helios}/Gauges/A-10/LiquidOxygen/liquid_oxygen_needle.xaml", new Point(180d, 180d), new Size(175d, 74d), new Point(138d, 37d), 0d);
            Components.Add(_needle);

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/engine_bezel.png", new Rect(0d, 0d, 360d, 360d)));

            _liquidOxygen = new HeliosValue(this, new BindingValue(0d), "", "liquid oxygen", "Remaining liquid oxygen.", "(1-5)", BindingValueUnits.Liters);
            _liquidOxygen.Execute += new HeliosActionHandler(LiquidOxygen_Execute);
            Actions.Add(_liquidOxygen);
        }

        void LiquidOxygen_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
    }
}

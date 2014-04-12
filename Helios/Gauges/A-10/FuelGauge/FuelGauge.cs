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

namespace GadrocsWorkshop.Helios.Gauges.A_10.FuelGauge
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.A10.FuelGauge", "Fuel Gauge", "A-10 Gauges", typeof(GaugeRenderer))]
    public class FuelGauge : BaseGauge
    {
        private HeliosValue _totalQuantity;
        private HeliosValue _leftQuantity;
        private HeliosValue _rightQuantity;
        private GaugeNeedle _leftNeedle;
        private GaugeNeedle _rightNeedle;
        private GaugeDrumCounter _totalDrum;
        private CalibrationPointCollectionDouble _needleCalibration;

        public FuelGauge()
            : base("Fuel Gauge", new Size(360, 360))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/FuelGauge/fuel_gauge_faceplate.xaml", new Rect(30d, 30d, 300d, 300d)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 6000d, 168d);

            _totalDrum = new GaugeDrumCounter("{Helios}/Gauges/A-10/Common/drum_tape.xaml", new Point(135d, 95d), "##%00", new Size(10d, 15d), new Size(18d, 28d));
            _totalDrum.Clip = new RectangleGeometry(new Rect(135d, 95d, 90d, 28d));
            _totalDrum.Value = 0d;
            Components.Add(_totalDrum);

            _leftNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/FuelGauge/fuel_gauge_needle.xaml", new Point(180d, 180d), new Size(90d, 177d), new Point(45d, 132d), 186d);
            Components.Add(_leftNeedle);

            _rightNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/FuelGauge/fuel_gauge_needle.xaml", new Point(180d, 180d), new Size(90d, 177d), new Point(45d, 132d), 174d);
            Components.Add(_rightNeedle);

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/engine_bezel.png", new Rect(0d, 0d, 360d, 360d)));

            _leftQuantity = new HeliosValue(this, new BindingValue(0d), "", "left quantity", "Quantity of fuel to display on left needle.", "(0 - 6000)", BindingValueUnits.Pounds);
            _leftQuantity.Execute += new HeliosActionHandler(LeftQuantity_Execute);
            Actions.Add(_leftQuantity);

            _rightQuantity = new HeliosValue(this, new BindingValue(0d), "", "right quantity", "Quantity of fuel to display on right needle.", "(0 - 6000)", BindingValueUnits.Pounds);
            _rightQuantity.Execute += new HeliosActionHandler(RightQuantity_Execute);
            Actions.Add(_rightQuantity);

            _totalQuantity = new HeliosValue(this, new BindingValue(0d), "", "total quantity", "Quantity of fuel to display on the totalizer.", "(0 - 99,999)", BindingValueUnits.Pounds);
            _totalQuantity.Execute += new HeliosActionHandler(TotalQuantity_Execute);
            Actions.Add(_totalQuantity);
        }

        void LeftQuantity_Execute(object action, HeliosActionEventArgs e)
        {
            _leftNeedle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }

        void RightQuantity_Execute(object action, HeliosActionEventArgs e)
        {
            _rightNeedle.Rotation = -_needleCalibration.Interpolate(e.Value.DoubleValue);
        }

        void TotalQuantity_Execute(object action, HeliosActionEventArgs e)
        {
            _totalDrum.Value = e.Value.DoubleValue;
        }
    }
}

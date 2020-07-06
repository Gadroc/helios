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
    using GadrocsWorkshop.Helios.Gauges.A_10.VVI;
    using System;
    using System.Windows;

    [HeliosControl("Helios.AV8B.VVI (AutoBinding)", "VVI", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class VVI1 : BaseGauge
    {
        private HeliosValue _verticalVelocity;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _calibrationPoints;

        public VVI1()
            : base("Flight Instruments", new Size(364, 376))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/VVI/vvi_faceplate.xaml", new Rect(32d, 38d, 300d, 300d)));

            _needle = new GaugeNeedle("{Helios}/Gauges/A-10/Common/needle_a.xaml", new Point(182d, 188d), new Size(22, 165), new Point(11, 130), -90d);
            Components.Add(_needle);

            //Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/gauge_bezel.png", new Rect(0d, 0d, 364d, 376d)));
            //Components.Add(new GaugeImage("{AV-8B}/Images/WQHD/Panel/crystal_reflection_round.png", new Rect(32d, 38d, 300d, 300d)));
            GaugeImage _gauge = new GaugeImage("{AV-8B}/Images/WQHD/Panel/crystal_reflection_round.png", new Rect(32d, 38d, 300d, 300d));
            _gauge.Opacity = 0.4;
            Components.Add(_gauge);

            _verticalVelocity = new HeliosValue(this, new BindingValue(0d), "Flight Instruments", "vertical velocity", "Veritcal velocity of the aircraft", "(-6,000 to 6,000)", BindingValueUnits.FeetPerMinute);
            _verticalVelocity.Execute += new HeliosActionHandler(VerticalVelocity_Execute);
            Actions.Add(_verticalVelocity);

            _calibrationPoints = new CalibrationPointCollectionDouble(-6000d, -169d, 6000d, 169d);
            _calibrationPoints.Add(new CalibrationPointDouble(-2000d, -81d));
            _calibrationPoints.Add(new CalibrationPointDouble(-1000d, -45d));
            _calibrationPoints.Add(new CalibrationPointDouble(0d, 0d));
            _calibrationPoints.Add(new CalibrationPointDouble(1000d, 45d));
            _calibrationPoints.Add(new CalibrationPointDouble(2000d, 81d));
        }

        void VerticalVelocity_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _calibrationPoints.Interpolate(e.Value.DoubleValue);
        }
    }
}

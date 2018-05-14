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

namespace GadrocsWorkshop.Helios.Gauges.AV8B.VVI
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.AV8B.VVI", "VVI", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class VVI : BaseGauge
    {
        private HeliosValue _verticalVelocity;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _calibrationPoints;

        public VVI()
            : base("VVI", new Size(182, 188))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/VVI/vvi_faceplate.xaml", new Rect(32d, 38d, 300d, 300d)));
            _needle = new GaugeNeedle("{Helios}/Gauges/AV-8B/Common/needle_a.xaml", new Point(91d, 94d), new Size(11, 82), new Point(5, 65), -90d);
            Components.Add(_needle);

            //Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/gauge_bezel.png", new Rect(0d, 0d, 364d, 376d)));

            _verticalVelocity = new HeliosValue(this, new BindingValue(0d), "", "vertical velocity", "Veritcal velocity of the aircraft", "(-6,000 to 6,000)", BindingValueUnits.FeetPerMinute);
            _verticalVelocity.Execute += new HeliosActionHandler(VerticalVelocity_Execute);
            Actions.Add(_verticalVelocity);

            _calibrationPoints = new CalibrationPointCollectionDouble(-0.6d, -170d, 0.6d, 170d);
            _calibrationPoints.Add(new CalibrationPointDouble(-0.29d, -119d));
            _calibrationPoints.Add(new CalibrationPointDouble(-0.19d, -92d));
            _calibrationPoints.Add(new CalibrationPointDouble(0d, 0d));
            _calibrationPoints.Add(new CalibrationPointDouble(0.19d, 92d));
            _calibrationPoints.Add(new CalibrationPointDouble(0.29d, 119d));
        }

        void VerticalVelocity_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _calibrationPoints.Interpolate(e.Value.DoubleValue);
        }
    }
}

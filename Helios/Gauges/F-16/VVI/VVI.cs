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

namespace GadrocsWorkshop.Helios.Gauges.F_16.VVI
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.F16.VVI", "VVI", "F-16", typeof(GaugeRenderer))]
    public class VVI : BaseGauge
    {
        private HeliosValue _offFlag;
        private HeliosValue _verticalVelocity;
        private GaugeImage _offFlagImage;
        private GaugeNeedle _vviTape;
        private CalibrationPointCollectionDouble _tapeCalibration = new CalibrationPointCollectionDouble(-6000d, -600d, 6000d, 600d);

        public VVI()
            : base("VVI", new Size(220, 452))
        {
            _vviTape = new GaugeNeedle("{Helios}/Gauges/F-16/VVI/vvi_tape.xaml", new Point(110, 226), new Size(130, 1960), new Point(65, 980));
            _vviTape.Clip = new RectangleGeometry(new Rect(55d, 86d, 130d, 280d));
            Components.Add(_vviTape);

            _offFlagImage = new GaugeImage("{Helios}/Gauges/F-16/VVI/vvi_off_flag.xaml", new Rect(55d, 84d, 111d, 282d));
            _offFlagImage.IsHidden = true;
            Components.Add(_offFlagImage);

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/VVI/vvi_faceplate.xaml", new Rect(0, 0, 220, 452)));

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/VVI/vvi_bezel.png", new Rect(0, 0, 220, 452)));

            _offFlag = new HeliosValue(this, new BindingValue(false), "", "off flag", "Indicates whether the off flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _offFlag.Execute += new HeliosActionHandler(OffFlag_Execute);
            Actions.Add(_offFlag);

            _verticalVelocity = new HeliosValue(this, new BindingValue(0d), "", "vertical velocity", "Current", "", BindingValueUnits.FeetPerMinute);
            _verticalVelocity.Execute += new HeliosActionHandler(VerticalVelocity_Execute);
            Actions.Add(_verticalVelocity);
            Values.Add(_verticalVelocity);
        }

        void OffFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _offFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _offFlagImage.IsHidden = !e.Value.BoolValue;
        }

        void VerticalVelocity_Execute(object action, HeliosActionEventArgs e)
        {
            _verticalVelocity.SetValue(e.Value, e.BypassCascadingTriggers);
            _vviTape.VerticalOffset = -_tapeCalibration.Interpolate(_verticalVelocity.Value.DoubleValue);
        }
    }
}

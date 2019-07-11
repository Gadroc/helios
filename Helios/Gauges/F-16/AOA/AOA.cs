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

namespace GadrocsWorkshop.Helios.Gauges.F_16.AOA
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.F16.AOA", "AOA", "F-16", typeof(GaugeRenderer))]
    public class AOA : BaseGauge
    {
        private HeliosValue _angleOfAttack;
        private HeliosValue _offFlag;
        private GaugeImage _offFlagImage;
        private GaugeNeedle _aoaNeedle;
        private CalibrationPointCollectionDouble _tapeCalibration = new CalibrationPointCollectionDouble(-30d, -600d, 30d, 600d);

        public AOA()
            : base("AOA", new Size(220, 452))
        {
            _aoaNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/AOA/aoa_tape.xaml", new Point(110, 226), new Size(130, 1482), new Point(65, 741));
            _aoaNeedle.Clip = new RectangleGeometry(new Rect(55d, 86d, 130d, 280d));
            Components.Add(_aoaNeedle);

            _offFlagImage = new GaugeImage("{Helios}/Gauges/F-16/AOA/aoa_off_flag.xaml", new Rect(55d, 84d, 110d, 282d));
            _offFlagImage.IsHidden = true;
            Components.Add(_offFlagImage);

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/AOA/aoa_faceplate.xaml", new Rect(0, 0, 220, 452)));

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/AOA/aoa_bezel.png", new Rect(0, 0, 220, 452)));

            _offFlag = new HeliosValue(this, new BindingValue(false), "", "off flag", "Indicates whether the off flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _offFlag.Execute += new HeliosActionHandler(OffFlag_Execute);
            Actions.Add(_offFlag);

            _angleOfAttack = new HeliosValue(this, new BindingValue(0d), "", "angle of attack", "Current angle of attack", "", BindingValueUnits.Degrees);
            _angleOfAttack.Execute += new HeliosActionHandler(AngleOfAttack_Execute);
            Actions.Add(_angleOfAttack);
            Values.Add(_angleOfAttack);
        }

        void OffFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _offFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _offFlagImage.IsHidden = !e.Value.BoolValue;
        }

        void AngleOfAttack_Execute(object action, HeliosActionEventArgs e)
        {
            _angleOfAttack.SetValue(e.Value, e.BypassCascadingTriggers);
            _aoaNeedle.VerticalOffset = _tapeCalibration.Interpolate(_angleOfAttack.Value.DoubleValue);
        }
    }
}

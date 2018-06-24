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
    using System.Windows.Media;

    [HeliosControl("Helios.AV8B.fuzeDisplay", "SMC Fuze Display", "AV-8B", typeof(GaugeRenderer))]
    public class fuzeDisplay : BaseGauge
    {
        private HeliosValue _fuze_display;
        private GaugeDrumCounter _leftDrum;
        private GaugeDrumCounter _rightDrum;

        public fuzeDisplay()
            : base("Fuze Mode Display", new Size(140, 50))
        {
            //Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/SMC/fuze_digit_faceplate.xaml", new Rect(0d, 0d, 140d, 50d)));

            _leftDrum = new GaugeDrumCounter("{Helios}/Gauges/AV-8B/SMC/fuze_mode_drum_tape_left.xaml", new Point(0d, 0d), "#", new Size(100d,100d), new Size(50d,50d));
            _leftDrum.Clip = new RectangleGeometry(new Rect(0d, 0d, 50d, 50d));
            Components.Add(_leftDrum);
            _rightDrum = new GaugeDrumCounter("{Helios}/Gauges/AV-8B/SMC/fuze_mode_drum_tape_right.xaml", new Point(60d, 0d), "#", new Size(100d, 100d), new Size(50d, 50d));
            _rightDrum.Clip = new RectangleGeometry(new Rect(60d, 0d, 50d, 50d));
            Components.Add(_rightDrum);

            _fuze_display = new HeliosValue(this, new BindingValue(0d), "", "value", "Stores Management", "display for SMC fuze mode", BindingValueUnits.Numeric);
            _fuze_display.Execute += new HeliosActionHandler(DigitDisplay_Execute);
            Actions.Add(_fuze_display);
        }

        void DigitDisplay_Execute(object action, HeliosActionEventArgs e)
        {
            double _triggerVal = e.Value.DoubleValue;
            if (_triggerVal == 110) _triggerVal = 77;  // 10 in both drums should be SAFE so we adjust for this.
            _rightDrum.Value = _triggerVal;
            _leftDrum.Value = _rightDrum.Value / 10d;
        }
    }
}

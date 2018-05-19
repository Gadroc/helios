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

namespace GadrocsWorkshop.Helios.Gauges.AV8B.fuzeDisplay
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.AV8B.fuzeDisplay", "SMC Fuze Display", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class fuzeDisplay : BaseGauge
    {
        private HeliosValue _one_digit_display;
        private GaugeDrumCounter _onesDrum;

        public fuzeDisplay()
            : base("Fuze Mode Display", new Size(140, 50))
        {
            //Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/fuze Display/digit_faceplate.xaml", new Rect(0d, 0d, 280d, 100d)));

            _onesDrum = new GaugeDrumCounter("{Helios}/Gauges/AV-8B/fuze display/fuze_mode_drum_tape.xaml", new Point(0d, 0d), "#", new Size(28d,10d), new Size(140d,50d));
            _onesDrum.Clip = new RectangleGeometry(new Rect(0d, 0d, 140d, 50d));
            Components.Add(_onesDrum);

            _one_digit_display = new HeliosValue(this, new BindingValue(0d), "", "value", "Stores Management", "display for SMC fuze mode", BindingValueUnits.Numeric);
            _one_digit_display.Execute += new HeliosActionHandler(DigitDisplay_Execute);
            Actions.Add(_one_digit_display);

        }

        void DigitDisplay_Execute(object action, HeliosActionEventArgs e)
        {
            if (e.Value.DoubleValue == 1)
                _onesDrum.Value = 5;
            else
                _onesDrum.Value = e.Value.DoubleValue * 10;  // 386 returns values 0.1, 0.2 etc to multiply by 10 to get unit values
        }
    }
}

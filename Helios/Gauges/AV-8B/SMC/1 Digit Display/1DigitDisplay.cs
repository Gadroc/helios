﻿//  Copyright 2014 Craig Courtney
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

namespace GadrocsWorkshop.Helios.Gauges.AV8B.SMC
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.AV8B.SMC.OneDigitDisplay", "One Digit Display", "_Hidden Parts", typeof(GaugeRenderer))]
    public class OneDigitDisplay : BaseGauge
    {
        private HeliosValue _one_digit_display;
        private GaugeDrumCounter _onesDrum;

        public OneDigitDisplay()
            : base("One Digit Display", new Size(82, 96))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/SMC/1 Digit Display/digit_faceplate.xaml", new Rect(0d, 0d, 82d, 96d)));

            _onesDrum = new GaugeDrumCounter("{Helios}/Gauges/AV-8B/Common/drum_tape.xaml", new Point(9d, 2d), "#", new Size(10d, 15d), new Size(64d, 96d));
            _onesDrum.Clip = new RectangleGeometry(new Rect(9d, 2d, 64d, 96d));
            Components.Add(_onesDrum);

            _one_digit_display = new HeliosValue(this, new BindingValue(0d), "", "value", "One digit display", "Simple one digit drum display", BindingValueUnits.Numeric);
            _one_digit_display.Execute += new HeliosActionHandler(DigitDisplay_Execute);
            Actions.Add(_one_digit_display);

        }

        void DigitDisplay_Execute(object action, HeliosActionEventArgs e)
        {
            _onesDrum.Value = e.Value.DoubleValue;
        }
    }
}

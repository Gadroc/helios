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

namespace GadrocsWorkshop.Helios.Gauges.AV8B.stabilizerDisplay
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.AV8B.stabilizerDisplay", "Stabilizer Direction Display", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class stabilizerDisplay : BaseGauge
    {
        private HeliosValue _one_digit_display;
        private GaugeDrumCounter _onesDrum;

        public stabilizerDisplay()
            : base("Stabilizer Direction", new Size(40, 50))
        {
            //Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/stabilizer display/digit_faceplate.xaml", new Rect(0d, 0d, 80d, 100d)));

            _onesDrum = new GaugeDrumCounter("{Helios}/Gauges/AV-8B/stabiliser display/stabilizer_drum_tape.xaml", new Point(0d, 0d), "#", new Size(8d, 10d), new Size(40d, 50d));
            _onesDrum.Clip = new RectangleGeometry(new Rect(0d, 0d, 40d, 50d));
            Components.Add(_onesDrum);

            _one_digit_display = new HeliosValue(this, new BindingValue(0d), "", "value", "EDP", "Stabilizer direction display", BindingValueUnits.Numeric);
            _one_digit_display.Execute += new HeliosActionHandler(DigitDisplay_Execute);
            Actions.Add(_one_digit_display);

        }

        void DigitDisplay_Execute(object action, HeliosActionEventArgs e)
        {
            _onesDrum.Value = e.Value.DoubleValue + 2;  // The adddition of the +2 is to cater for the fact that the value is -1 to 1
        }
    }
}

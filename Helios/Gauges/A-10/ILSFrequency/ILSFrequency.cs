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

namespace GadrocsWorkshop.Helios.Gauges.A_10.ILSFrequencey
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.A10.ILSFrequency", "ILS Frequency", "A-10 Gauges", typeof(GaugeRenderer))]
    public class ILSFrequency : BaseGauge
    {
        private HeliosValue _frequency;
        private GaugeDrumCounter _megaHzDrum;
        private GaugeDrumCounter _kiloHzDrum;

        public ILSFrequency()
            : base("ILS Frequency", new Size(300, 100))
        {
            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/ILSFrequency/ils_frequency_faceplate.xaml", new Rect(0d, 0d, 300d, 100d)));

            _megaHzDrum = new GaugeDrumCounter("{Helios}/Gauges/A-10/Common/drum_tape.xaml", new Point(13.5d, 11.5d), "###", new Size(10d, 15d), new Size(50d, 75d));
            _megaHzDrum.Clip = new RectangleGeometry(new Rect(13.5d, 11.5d, 150d, 75d));
            Components.Add(_megaHzDrum);

            _kiloHzDrum = new GaugeDrumCounter("{Helios}/Gauges/A-10/Common/drum_tape.xaml", new Point(183.5, 11.5d), "#%", new Size(10d, 15d), new Size(50d, 75d));
            _kiloHzDrum.Clip = new RectangleGeometry(new Rect(183.5d, 11.5d, 100d, 75d));
            Components.Add(_kiloHzDrum);

            _frequency = new HeliosValue(this, new BindingValue(0d), "", "frequency", "Frequency to display.", "", BindingValueUnits.Numeric);
            _frequency.Execute += new HeliosActionHandler(Frequency_Execute);
            Actions.Add(_frequency);
        }

        void Frequency_Execute(object action, HeliosActionEventArgs e)
        {
            _megaHzDrum.Value = Math.Floor(e.Value.DoubleValue);
            string s = e.Value.DoubleValue.ToString("#.000", System.Globalization.CultureInfo.InvariantCulture);
            _kiloHzDrum.Value = double.Parse(s.Substring(s.IndexOf(".") + 1)) / 10d;
        }
    }
}

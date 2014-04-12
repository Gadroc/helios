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

namespace GadrocsWorkshop.Helios.Gauges.A_10.TISLCodeWheel
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.A10.TISLCodeWheel", "TISL Code Wheel Window", "A-10 Gauges", typeof(GaugeRenderer))]
    public class TISLCodeWheel : BaseGauge
    {
        private GaugeDrumCounter _drum;
        private HeliosValue _wheelValue;

        public TISLCodeWheel()
            : base("TISL Code Wheel Window", new Size(16, 21))
        {
            Components.Add(new GaugeRectangle(Colors.Black, new Rect(0, 0, 16, 21)));

            _drum = new GaugeDrumCounter("{Helios}/Gauges/A-10/Common/drum_tape.xaml", new Point(3, 3), "%", new Size(10d, 15d));
            _drum.Clip = new RectangleGeometry(new Rect(1, 1, 14, 19));
            Components.Add(_drum);

            _wheelValue = new HeliosValue(this, new BindingValue(0d), "", "wheel value", "Current value displayed on the wheel.", "0-9.9", BindingValueUnits.Numeric);
            _wheelValue.Execute += new HeliosActionHandler(WheelValue_Execute);
            Actions.Add(_wheelValue);
        }

        void WheelValue_Execute(object action, HeliosActionEventArgs e)
        {
            _drum.Value = e.Value.DoubleValue;
        }
    }
}

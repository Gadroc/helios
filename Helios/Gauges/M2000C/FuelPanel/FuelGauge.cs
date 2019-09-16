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

namespace GadrocsWorkshop.Helios.Gauges.M2000C.FuelGauge
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.M2000C.FuelGauge", "Fuel Gauge", "M2000C Gauges", typeof(GaugeRenderer))]
    public class FuelGauge : BaseGauge
    {
        private HeliosValue _tensQt;
        private HeliosValue _hundredsQt;
        private HeliosValue _thousandsQt;
        private GaugeDrumCounter _tens;
        private GaugeDrumCounter _hundreds;
        private GaugeDrumCounter _thousands;

        public FuelGauge()
            : this("Fuel Gauge", new Size(100, 28), 0)
        {
        }

        public FuelGauge(string name, Size size, int offsetX)
            : base(name, size)
        {
            int row0 = 67;
            int column0 = offsetX, column1 = offsetX + 24, column2 = offsetX + 49;

            _tens = new GaugeDrumCounter("{Helios}/Gauges/M2000C/Common/drum_tape.xaml", new Point(column2, row0), "#", new Size(10d, 15d), new Size(12d, 19d));
            _tens.Clip = new RectangleGeometry(new Rect(column2, row0, 12d, 19d));
            Components.Add(_tens);

            _hundreds = new GaugeDrumCounter("{Helios}/Gauges/M2000C/Common/drum_tape.xaml", new Point(column1, row0), "#", new Size(10d, 15d), new Size(12d, 19d));
            _hundreds.Clip = new RectangleGeometry(new Rect(column1, row0, 12d, 19d));
            Components.Add(_hundreds);

            _thousands = new GaugeDrumCounter("{Helios}/Gauges/M2000C/Common/drum_tape.xaml", new Point(column0, row0), "#", new Size(10d, 15d), new Size(12d, 19d));
            _thousands.Clip = new RectangleGeometry(new Rect(column0, row0, 12d, 19d));
            Components.Add(_thousands);

            _tensQt = new HeliosValue(this, new BindingValue(0d), "", "tens quantity", "Tens Quantity of fuel to display.", "(0 - 9)", BindingValueUnits.Numeric);
            _tensQt.Execute += new HeliosActionHandler(TensQt_Execute);
            Actions.Add(_tensQt);

            _hundredsQt = new HeliosValue(this, new BindingValue(0d), "", "hundred quantity", "Hundred Quantity of fuel to display.", "(0 - 9)", BindingValueUnits.Numeric);
            _hundredsQt.Execute += new HeliosActionHandler(HundredsQt_Execute);
            Actions.Add(_hundredsQt);

            _thousandsQt = new HeliosValue(this, new BindingValue(0d), "", "thousand quantity", "Thousand Quantity of fuel to display.", "(0 - 9)", BindingValueUnits.Numeric);
            _thousandsQt.Execute += new HeliosActionHandler(ThousandsQt_Execute);
            Actions.Add(_thousandsQt);
        }

        void TensQt_Execute(object action, HeliosActionEventArgs e)
        {
            _tens.Value = e.Value.DoubleValue * 10d;
        }

        void HundredsQt_Execute(object action, HeliosActionEventArgs e)
        {
            _hundreds.Value = e.Value.DoubleValue * 10d;
        }

        void ThousandsQt_Execute(object action, HeliosActionEventArgs e)
        {
            _thousands.Value = e.Value.DoubleValue * 10d;
        }
    }
}

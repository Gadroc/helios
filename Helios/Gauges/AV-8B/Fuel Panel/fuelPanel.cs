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

namespace GadrocsWorkshop.Helios.Gauges.AV8B.FuelPanel
{
    using GadrocsWorkshop.Helios.Gauges.AV8B;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Windows;

    [HeliosControl("Helios.AV8B.FuelPanel", "Fuel Panel", "AV-8B Gauges", typeof(AV8BDeviceRenderer))]
    class FQIS: AV8BDevice
    {
        private string _interfaceDeviceName = "Fuel Quantity";
        private string _font = "MS 33558";

        public FQIS()
            : base("Fuel Panel", new Size(1839,729))
        {
            AddDisplay("Total Quantity", new FiveDigitDisplay(), new Point(580, 84), new Size(448, 108), "Total display");
            AddDisplay("Left Quantity", new FourDigitDisplay(), new Point(212, 263), new Size(355, 103), "Left Tank display");
            AddDisplay("Right Quantity", new FourDigitDisplay(), new Point(919, 263), new Size(355, 103), "Right Tank display");
            AddDisplay("Bingo Quantity", new FourDigitDisplay(), new Point(587, 435), new Size(288, 100), "Bingo value display");
            AddKnob("Fuel Selector", new Point(1458, 270), new Size(208, 208), "Fuel Totaliser Selector");
            AddEncoder("Bingo Knob", new Point(960,540), new Size(124,124), "Bingo Fuel Set Knob");
            AddIndicator("Off Flag", 322, 89, new Size(148, 96), "Off Flag");


        }
        private void AddDisplay(string name, BaseGauge _gauge, Point posn, Size displaySize, string interfaceElementName)
        {
            AddDisplay(
                name: name,
                gauge: _gauge,
                posn: posn,
                size: displaySize,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName
                );
            _gauge.Name = "Fuel Panel_" + name;
        }
        private void AddKnob(string name, Point posn, Size size, string interfaceElementName)
        {
            RotarySwitch _knob = new RotarySwitch();
            _knob.Name = "Fuel Panel_" + name;
            _knob.KnobImage = "{AV-8B}/Images/Fuel Selector Knob.png";
            _knob.DrawLabels = false;
            _knob.DrawLines = false;
            _knob.Positions.Clear();
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 0, "Outboard", 265d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 1, "Inboard", 295d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 2, "Wing", 325d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 3, "Internal", 355d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 4, "Total", 25d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 5, "Feed", 55d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 6, "BIT", 85d));
            _knob.CurrentPosition = 4;
            _knob.DefaultPosition = 4;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            AddRotarySwitchBindings(name, posn, size, _knob, _interfaceDeviceName, interfaceElementName);
        }

        private void AddEncoder(string name, Point posn, Size size, string interfaceElementName)
        {
            RotaryEncoder _enc = AddEncoder(
                name: name,
                size: size,
                posn: posn,
                knobImage: "{AV-8B}/Images/Fuel Bingo Knob.png",
                stepValue: 0.1,
                rotationStep: 5,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
            _enc.Name = "Fuel Panel_" + name;
        }
        private void AddIndicator(string name, double x, double y, Size size, string interfaceElementName) { AddIndicator(name, x, y, size, false, interfaceElementName); }
        private void AddIndicator(string name, double x, double y, Size size, bool _vertical, string interfaceElementName)
        {
            Indicator indicator = AddIndicator(
                name: name,
                posn: new Point(x, y),
                size: size,
                onImage: "{AV-8B}/Images/Fuel Panel Off Flag.png",
                offImage: "{AV-8B}/Images/_transparent.png",
                onTextColor: System.Windows.Media.Color.FromArgb(0x00, 0xff, 0xff, 0xff),
                offTextColor: System.Windows.Media.Color.FromArgb(0x00, 0x00, 0x00, 0x00),
                font: _font,
                vertical: _vertical,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
            indicator.Text = "";
            indicator.Name = "Fuel Panel_" + name;
        }
        public override bool HitTest(Point location)
        {

            return true;  // nothing to press on the fuel so return false.
        }
        public override void MouseDown(Point location)
        {
            // No-Op
        }

        public override void MouseDrag(Point location)
        {
            // No-Op
        }

        public override void MouseUp(Point location)
        {
            // No-Op
        }
        public override string BezelImage
        {
            get { return "{AV-8B}/Images/AV8B Fuel Panel.png"; }
        }
    }
}

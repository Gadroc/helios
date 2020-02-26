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
    using GadrocsWorkshop.Helios.Gauges.AV8B;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Windows;

    [HeliosControl("Helios.AV8B.RWR", "RWR Panel", "AV-8B Gauges", typeof(AV8BDeviceRenderer))]
    class RWRPanel: AV8BDevice
    {
        private string _interfaceDeviceName = "RWR / ECM";

        public RWRPanel()
            : base("RWR Panel", new Size(204, 755))
        {

            AddKnob1("Decoy Dispenser", new Point(41, 297), new Size(100, 100), "Decoy Dispenser Control");
            AddKnob2("Jammer", new Point(54, 487), new Size(100, 100), "Jammer Control");
            AddPot("RWR Volume", new Point(46, 89), new Size(70, 70), "Off/Volume");

        }

        public override string BezelImage
        {
            get { return "{AV-8B}/Images/WQHD/Panel/Right RWR.png"; }
        }
            
        private void AddKnob1(string name, Point posn, Size size, string interfaceElementName)
        {
            Helios.Controls.RotarySwitch _knob = new Helios.Controls.RotarySwitch();
            _knob.Name = "RWR Panel_" + name;
            _knob.KnobImage = "{AV-8B}/Images/Common Knob.png";
            _knob.DrawLabels = false;
            _knob.DrawLines = false;
            _knob.Positions.Clear();
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 0, "Off", 225d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 1, "Auto", 300d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 2, "Up", 0d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 3, "Dn", 60d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 4, "RWR", 135d));
            _knob.CurrentPosition = 0;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            AddRotarySwitchBindings(name, posn, size, _knob, _interfaceDeviceName, interfaceElementName);
        }
        private void AddKnob2(string name, Point posn, Size size, string interfaceElementName)
        {
            Helios.Controls.RotarySwitch _knob = new Helios.Controls.RotarySwitch();
            _knob.Name = "RWR Panel_" + name;
            _knob.KnobImage = "{AV-8B}/Images/Common Knob.png";
            _knob.DrawLabels = false;
            _knob.DrawLines = false;
            _knob.Positions.Clear();
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 0, "OFF", 225d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 1, "STBY", 305d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 2, "BIT", 0d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 3, "RCV", 48d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 4, "RPT", 125d));
            _knob.CurrentPosition = 0;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            AddRotarySwitchBindings(name, posn, size, _knob, _interfaceDeviceName, interfaceElementName);
        }


        private void AddPot(string name, Point posn, Size size, string interfaceElementName)
        {
            AddPot(name: name,
                posn: posn,
                size: size,
                knobImage: "{AV-8B}/Images/Common Knob.png",
                initialRotation: 219,
                rotationTravel: 291,
                minValue: 0,
                maxValue: 1,
                initialValue: 0,
                stepValue: 0.1,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false);
        }



        public override bool HitTest(Point location)
        {
            //if (_scaledScreenRect.Contains(location))
            //{
            //    return false;
            //}

            return true;
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
    }
}

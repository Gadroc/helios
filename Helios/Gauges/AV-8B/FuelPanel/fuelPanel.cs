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

    [HeliosControl("Helios.AV8B.FuelPanel", "AV-8B Fuel Panel", "AV-8B", typeof(AV8BDeviceRenderer))]
    class fuel: AV8BDevice
    {
        public fuel()
            : base("Fuel Panel", new Size(1839,729))
        {
            AddDisplay("Total Quantity", new Helios.Gauges.AV8B.FuelPanel.FiveDigitDisplay(), new Point(580, 84), new Size(448, 108));
            AddDisplay("Left Quantity", new Helios.Gauges.AV8B.FuelPanel.FourDigitDisplay(), new Point(212, 263), new Size(355, 103));
            AddDisplay("Right Quantity", new Helios.Gauges.AV8B.FuelPanel.FourDigitDisplay(), new Point(919, 263), new Size(355, 103));
            AddDisplay("Bingo Quantity", new Helios.Gauges.AV8B.FuelPanel.FourDigitDisplay(), new Point(587, 435), new Size(288, 100));
            AddKnob("Fuel Selector", new Point(1458, 270), new Size(208, 208));
            AddEncoder("Bingo Knob", new Point(960,540), new Size(124,124));

        }
        private void AddDisplay(string name, BaseGauge _gauge, Point posn, Size displaySize)
        {
            _gauge.Name = name;
            _gauge.Width = displaySize.Width;
            _gauge.Height = displaySize.Height;
            _gauge.Left = posn.X;
            _gauge.Top = posn.Y;
            Children.Add(_gauge);
            foreach (IBindingTrigger trigger in _gauge.Triggers)
            {
                AddTrigger(trigger, name);
            }
            AddAction(_gauge.Actions["set.value"], name);

        }
        private void AddKnob(string name, Point posn, Size size)
        {
            Helios.Controls.RotarySwitch _knob = new Helios.Controls.RotarySwitch();
            _knob.Name = name;
            _knob.KnobImage = "{Helios}/Images/AV-8B/Fuel Selector Knob.png";
            _knob.DrawLabels = false;
            _knob.DrawLines = false;
            _knob.Positions.Clear();
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 0, "Outboard", 190d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 1, "Inboard", 215d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 2, "Wing", 240d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 3, "Internal", 265d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 4, "Total", 290d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 5, "Feed", 315d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 6, "BIT", 340d));
            _knob.CurrentPosition = 4;
            _knob.DefaultPosition = 4;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            Children.Add(_knob);
            AddTrigger(_knob.Triggers["position.changed"], name);
            AddAction(_knob.Actions["set.position"], name);
        }
        private void AddEncoder(string name, Point posn, Size size)
        {
            //Helios.Controls.RotaryEncoderPushable _knob = new Helios.Controls.RotaryEncoderPushable();
            Helios.Controls.RotaryEncoder _knob = new Helios.Controls.RotaryEncoder();
            _knob.Name = name;
            _knob.KnobImage = "{Helios}/Images/AV-8B/Fuel Bingo Knob.png";
            _knob.StepValue = 0.01;
            _knob.RotationStep = 5;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            Children.Add(_knob);
            foreach (IBindingTrigger trigger in _knob.Triggers)
            {
                AddTrigger(trigger, name);
            }
        }

        private new void AddTrigger(IBindingTrigger trigger, string device)
        {
            trigger.Device = device;
            Triggers.Add(trigger);
        }

        private new void AddAction(IBindingAction action, string device)
        {
            action.Device = device;
            Actions.Add(action);
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
            get { return "{Helios}/Images/AV-8B/AV8B Fuel Panel.png"; }
        }
    }
}

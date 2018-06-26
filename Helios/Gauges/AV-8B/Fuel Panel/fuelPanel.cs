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

    [HeliosControl("Helios.AV8B.Fuel", "AV-8B Fuel Panel", "AV-8B", typeof(AV8BDeviceRenderer))]
    class fuel: AV8BDevice
    {
        public fuel()
            : base("Fuel Panel", new Size(334,125))
        {
            AddDisplay("Total Quantity", new Helios.Gauges.AV8B.FiveDigitDisplay(), new Point(98, 14), new Size(77, 20));
            AddDisplay("Left Quantity", new Helios.Gauges.AV8B.FourDigitDisplay(), new Point(34, 42), new Size(64, 20));
            AddDisplay("Right Quantity", new Helios.Gauges.AV8B.FourDigitDisplay(), new Point(157, 42), new Size(64, 20));
            AddDisplay("Bingo Quantity", new Helios.Gauges.AV8B.FourDigitDisplay(), new Point(95, 68), new Size(56, 20));
            AddKnob("Fuel Selector", new Point(246, 24), new Size(80, 80));
            AddEncoder("Bingo Knob", new Point(165,80), new Size(30,30));

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
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 0, "Outboard", 200d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 1, "Inboard", 225d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 2, "Wing", 250d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 3, "Internal", 275d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 4, "Total", 300d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 5, "Feed", 325d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 6, "BIT", 350d));
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
            _knob.KnobImage = "{Helios}/Images/AV-8B/Rotary.png";
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

        private void AddTrigger(IBindingTrigger trigger, string device)
        {
            trigger.Device = device;
            Triggers.Add(trigger);
        }

        private void AddAction(IBindingAction action, string device)
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
            get { return "{Helios}/Gauges/AV-8B/Fuel Panel/fuel_faceplate.xaml"; }
        }
    }
}

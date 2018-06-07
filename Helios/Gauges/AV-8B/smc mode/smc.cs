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

    [HeliosControl("Helios.AV8B.SMC", "Stores Management Panel", "AV-8B", typeof(AV8BDeviceRenderer))]
    class SMC_AV8B: AV8BDevice
    {
 
        public SMC_AV8B()
            : base("Stores Management Panel", new Size(703, 254))
        {

            AddDisplay("Stores Mode", new Helios.Gauges.AV8B.smcModeDisplay(), new Point(48, 34), new Size(32, 32));
            AddDisplay("Fuze Mode", new Helios.Gauges.AV8B.fuzeDisplay(), new Point(120, 34), new Size(60, 32));
            AddDisplay("Quantity", new Helios.Gauges.AV8B.TwoDigitDisplay(), new Point(226, 34), new Size(64, 32));
            AddDisplay("Multiple", new Helios.Gauges.AV8B.OneDigitDisplay(), new Point(340, 34), new Size(32, 32));
            AddDisplay("Interval", new Helios.Gauges.AV8B.ThreeDigitDisplay(), new Point(428, 34), new Size(96, 32));

            AddButton("Station 1", 218, 211, new Size(30, 30));
            AddButton("Station 2", 276, 211, new Size(30, 30));
            AddButton("Station 3", 333, 211, new Size(30, 30));
            AddButton("Station 4", 390, 211, new Size(30, 30));
            AddButton("Station 5", 447, 211, new Size(30, 30));
            AddButton("Station 6", 504, 211, new Size(30, 30));
            AddButton("Station 7", 562, 211, new Size(30, 30));

            //AddIndicator("Station 1", 215, 175, new Size(38, 25));
            //AddIndicator("Station 2", 273, 175, new Size(30, 25));
            //AddIndicator("Station 3", 330, 175, new Size(30, 25));
            //AddIndicator("Station 4", 387, 175, new Size(30, 25));
            //AddIndicator("Station 5", 444, 175, new Size(30, 25));
            //AddIndicator("Station 6", 501, 175, new Size(30, 25));
            //AddIndicator("Station 7", 559, 175, new Size(30, 25));

            //AddThreeWayToggle("Aiming Mode Switch", 27, 61, new Size(50, 100));
            //AddThreeWayToggle("Fuze Toggle Switch", 139, 61, new Size(50, 100));
            //AddThreeWayToggle("Quantity 10's", 201, 61, new Size(50, 100));
            //AddThreeWayToggle("Quantity 1's", 263, 61, new Size(50, 100));
            //AddThreeWayToggle("Multiple Switch", 325, 61, new Size(50, 100));
            //AddThreeWayToggle("Interval 100's", 390, 65, new Size(50, 100));
            //AddThreeWayToggle("Interval 10's", 452, 65, new Size(50, 100));
            //AddThreeWayToggle("Interval 1's", 514, 65, new Size(50, 100));
            //AddTwoWayToggle("IR Cool Switch", 620, 147, new Size(50, 100));

            AddKnobSMC1("Fuzing Options", new Point(582, 39), new Size(75, 75));
            AddKnobSMC2("Stores Jettison Switch", new Point(58, 150), new Size(100, 100));
            AddButton("Jettison Button", 58, 150, new Size(100, 100), true, true);


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
            foreach (IBindingAction action in _gauge.Actions)
            {
                AddAction(action, name);
            }
        }

        public override string BezelImage
        {
            get { return "{ Helios}/Images/AV-8B/AV-8B SMC faceplate.png"; }
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
        private void AddKnobSMC1(string name, Point posn, Size size)
        {
            Helios.Controls.RotarySwitch _knob = new Helios.Controls.RotarySwitch();
            _knob.Name = name;
            _knob.KnobImage = "{Helios}/Images/AV-8B/Common Knob.png";
            _knob.DrawLabels = false;
            _knob.DrawLines = false;
            _knob.Positions.Clear();
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 0, "0", 225d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 1, "1", 0d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 2, "2", 135d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 3, "3", 180d));
            _knob.CurrentPosition = 0;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            Children.Add(_knob);
            foreach (IBindingTrigger trigger in _knob.Triggers)
            {
                AddTrigger(trigger, name);
            }
            AddAction(_knob.Actions["set.position"], name);
        }
        private void AddKnobSMC2(string name, Point posn, Size size)
        {
            Helios.Controls.RotarySwitch _knob = new Helios.Controls.RotarySwitch();
            _knob.Name = name;
            _knob.KnobImage = "{Helios}/Gauges/AV-8B/smc mode/Jettison Knob.xaml";
            _knob.DrawLabels = false;
            _knob.DrawLines = false;
            _knob.Positions.Clear();
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 0, "0", 225d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 1, "1", 0d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 2, "2", 135d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 3, "3", 180d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 4, "4", 90d));
            _knob.CurrentPosition = 0;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            Children.Add(_knob);
            foreach (IBindingTrigger trigger in _knob.Triggers)
            {
                AddTrigger(trigger, name);
            }
            AddAction(_knob.Actions["set.position"], name);
        }
        private void AddButton(string name, double x, double y) { AddButton(name, x, y, false); }
        private void AddButton(string name, double x, double y, Size size) { AddButton(name, x, y, size, false); }
        private void AddButton(string name, double x, double y, bool horizontal) { AddButton(name, x, y, new Size(40,40),false); }
        private void AddButton(string name, double x, double y, Size size, bool horizontal) { AddButton(name, x, y, size, horizontal, false); }
        private void AddButton(string name, double x, double y, Size size, bool horizontal, bool altImage)
        {
            Helios.Controls.PushButton button = new Helios.Controls.PushButton();
            button.Top = y;
            button.Left = x;
            button.Width = size.Width;
            button.Height = size.Height;
            if (!altImage)
            {
                button.Image = "{Helios}/Images/Buttons/tactile-dark-round.png";
                button.PushedImage = "{Helios}/Images/Buttons/tactile-dark-round-in.png";
                if (horizontal) button.Text = "|";
                else
                {
                    button.TextFormat.FontSize = 32;
                    button.TextFormat.FontWeight = FontWeights.Bold;
                    button.Text = "--";
                }

            }
            else
            {
                button.Image = "{Helios}/Gauges/AV-8B/smc mode/Jettison Button.xaml";
                button.PushedImage = "{Helios}/Gauges/AV-8B/smc mode/Jettison Button.xaml";
            }


            button.Name = name;

            Children.Add(button);
            foreach (IBindingTrigger trigger in button.Triggers)
            {
                AddTrigger(trigger, name);
            }
            foreach (IBindingAction action in button.Actions)
            {
                AddAction(action, name);
            }

            //AddTrigger(button.Triggers["pushed"], name);
            //AddTrigger(button.Triggers["released"], name);

            //AddAction(button.Actions["push"], name);
            //AddAction(button.Actions["release"], name);
            //AddAction(button.Actions["set.physical state"], name);
        }
        private void AddIndicator(string name, double x, double y, Size size) { AddButton(name, x, y, size, false); }
        private void AddIndicator(string name, double x, double y, Size size, bool horizontal)
        {
            Helios.Controls.PushButton button = new Helios.Controls.PushButton();
            button.Top = y;
            button.Left = x;
            button.Width = size.Width;
            button.Height = size.Height;
            button.Image = "{Helios}/Images/AV-8B/AV8BNA_SEL_On.png";
            button.PushedImage = "{Helios}/Images/AV-8B/AV8BNA_SEL_Off.png";
            button.Name = name;

            Children.Add(button);
            foreach (IBindingTrigger trigger in button.Triggers)
            {
                AddTrigger(trigger, name);
            }
            foreach (IBindingAction action in button.Actions)
            {
                AddAction(action, name);
            }

            //AddTrigger(button.Triggers["pushed"], name);
            //AddTrigger(button.Triggers["released"], name);

            //AddAction(button.Actions["push"], name);
            //AddAction(button.Actions["release"], name);
            //AddAction(button.Actions["set.physical state"], name);
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

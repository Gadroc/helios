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
            : base("Stores Management Panel", new Size(1231, 470))
        {

            AddDisplay("Stores Mode", new Helios.Gauges.AV8B.smcModeDisplay(), new Point(81, 64), new Size(68, 41));
            AddDisplay("Fuze Mode", new Helios.Gauges.AV8B.fuzeDisplay(), new Point(218, 59), new Size(112, 50));
            AddDisplay("Quantity", new Helios.Gauges.AV8B.TwoDigitDisplay(), new Point(407, 66), new Size(98, 48));
            AddDisplay("Multiple", new Helios.Gauges.AV8B.OneDigitDisplay(), new Point(607,66), new Size(41, 48));
            AddDisplay("Interval", new Helios.Gauges.AV8B.ThreeDigitDisplay(), new Point(746, 66), new Size(168, 48));

            AddButton("Station 1", 383, 392, new Size(50, 50));
            AddButton("Station 2", 486, 392, new Size(50, 50));
            AddButton("Station 3", 586, 392, new Size(50, 50));
            AddButton("Station 4", 688, 392, new Size(50, 50));
            AddButton("Station 5", 788, 392, new Size(50, 50));
            AddButton("Station 6", 886, 392, new Size(50, 50));
            AddButton("Station 7", 988, 392, new Size(50, 50));

            AddIndicator("Station 1 Selected", 385, 330, new Size(47, 41));
            AddIndicator("Station 2 Selected", 488, 330, new Size(47, 41)); 
            AddIndicator("Station 3 Selected", 588, 330, new Size(47, 41));
            AddIndicator("Station 4 Selected", 687, 330, new Size(47, 41));
            AddIndicator("Station 5 Selected", 784, 330, new Size(47, 41));
            AddIndicator("Station 6 Selected", 886, 330, new Size(47, 41));
            AddIndicator("Station 7 Selected", 988, 330, new Size(47, 41));

            AddThreeWayToggle("Aiming Mode Switch", 56, 130, new Size(65, 153));
            AddThreeWayToggle("Fuze Toggle Switch", 256, 130, new Size(65, 153));
            AddThreeWayToggle("Quantity 10's", 366, 134, new Size(65, 153));
            AddThreeWayToggle("Quantity 1's", 472, 134, new Size(65, 153));
            AddThreeWayToggle("Multiple Switch", 580, 132, new Size(65, 153));
            AddThreeWayToggle("Interval 100's", 696, 140, new Size(65, 153));
            AddThreeWayToggle("Interval 10's", 804, 140, new Size(65, 153));
            AddThreeWayToggle("Interval 1's", 911, 140, new Size(65, 153));
            AddTwoWayToggle("IR Cool Switch", 1094, 294, new Size(67, 154));

            AddKnobSMC1("Fuzing Options", new Point(1025,76), new Size(110,110));
            AddKnobSMC2("Stores Jettison Switch", new Point(96, 278), new Size(186, 186));
            AddButton("Jettison Button", 131, 312, new Size(118, 118), true, true);
        }
        private void AddDisplay(string name, BaseGauge _gauge, Point posn, Size displaySize)
        {
            _gauge.Name = name;
            _gauge.Width = displaySize.Width;
            _gauge.Height = displaySize.Height;
            _gauge.Left = posn.X;
            _gauge.Top = posn.Y;
            Children.Add(_gauge);
            AddAction(_gauge.Actions["set.value"], name);
        }

        public override string BezelImage
        {
            get { return "{Helios}/Images/AV-8B/AV-8B SMC Faceplate.png"; }
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
        private void AddKnobSMC1(string name, Point posn, Size size)
        {
            Helios.Controls.RotarySwitch _knob = new Helios.Controls.RotarySwitch();
            _knob.Name = name;
            _knob.KnobImage = "{Helios}/Images/AV-8B/SMC Selector Knob.png";
            _knob.DrawLabels = false;
            _knob.DrawLines = false;
            _knob.Positions.Clear();
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 0, "Norm", 315d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 1, "N/T", 0d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 2, "N", 45d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 3, "T", 90d));
            _knob.CurrentPosition = 1;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            Children.Add(_knob);
            AddTrigger(_knob.Triggers["position.changed"], name);
            AddAction(_knob.Actions["set.position"], name);
        }
        private void AddKnobSMC2(string name, Point posn, Size size)
        {
            Helios.Controls.RotarySwitch _knob = new Helios.Controls.RotarySwitch();
            _knob.Name = name;
            _knob.KnobImage = "{Helios}/Images/AV-8B/SMC Jettison Knob.png";
            _knob.DrawLabels = false;
            _knob.DrawLines = false;
            _knob.Positions.Clear();
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 0, "Station", 270d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 1, "Stores", 315d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 2, "Safe", 0d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 3, "Combat", 45d));
            _knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(_knob, 4, "Fuel", 90d));
            _knob.CurrentPosition = 3;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            Children.Add(_knob);
            AddTrigger(_knob.Triggers["position.changed"], name);
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
                button.Image = "{Helios}/Images/AV-8B/SMC Jettison Button.png";
                button.PushedImage = "{Helios}/Images/AV-8B/SMC Jettison Button.png";
            }


            button.Name = name;

            Children.Add(button);

            AddTrigger(button.Triggers["pushed"], name);
            AddTrigger(button.Triggers["released"], name);

            AddAction(button.Actions["push"], name);
            AddAction(button.Actions["release"], name);
            AddAction(button.Actions["set.physical state"], name);
        }
       private void AddTwoWayToggle(string name, double x, double y, Size size)
        {
            Helios.Controls.ToggleSwitch toggle = new Helios.Controls.ToggleSwitch();
            toggle.Top = y;
            toggle.Left = x;
            toggle.Width = size.Width;
            toggle.Height = size.Height;
            toggle.DefaultPosition = ToggleSwitchPosition.Two;
            toggle.PositionOneImage = "{Helios}/Images/AV-8B/SMC Cool Switch Up.png";
            toggle.PositionTwoImage = "{Helios}/Images/AV-8B/SMC Cool Switch Dn.png";
            toggle.Name = name;

            Children.Add(toggle);
            AddTrigger(toggle.Triggers["position.changed"], name);
            AddAction(toggle.Actions["set.position"], name);
        }
        private void AddThreeWayToggle(string name, double x, double y, Size size)
        {
            Helios.Controls.ThreeWayToggleSwitch toggle = new Helios.Controls.ThreeWayToggleSwitch();
            toggle.Top = y;
            toggle.Left = x;
            toggle.Width = size.Width;
            toggle.Height = size.Height;
            toggle.DefaultPosition = ThreeWayToggleSwitchPosition.Two;
            toggle.PositionOneImage = "{Helios}/Images/AV-8B/3 Way Toggle Square Up 1.png";
            toggle.PositionTwoImage = "{Helios}/Images/AV-8B/3 Way Toggle Square Mid 1.png";
            toggle.PositionThreeImage = "{Helios}/Images/AV-8B/3 Way Toggle Square Down 1.png";
            toggle.SwitchType = ThreeWayToggleSwitchType.MomOnMom;
            toggle.Name = name;

            Children.Add(toggle);
            AddTrigger(toggle.Triggers["position.changed"], name);
            AddAction(toggle.Actions["set.position"], name);
        }
        private void AddIndicator(string name, double x, double y, Size size) { AddIndicator(name, x, y, size, false); }
        private void AddIndicator(string name, double x, double y, Size size, bool horizontal)
        {
            Helios.Controls.Indicator indicator = new Helios.Controls.Indicator();
            indicator.Top = y;
            indicator.Left = x;
            indicator.Width = size.Width;
            indicator.Height = size.Height;
            indicator.OnImage = "AV-8B/AV8BNA_SEL_On.png";
            indicator.OffImage = "AV-8B/AV8BNA_SEL_Off.png";
            indicator.Text = "";
            indicator.Name = name;

            Children.Add(indicator);
            AddAction(indicator.Actions["set.indicator"], name);
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

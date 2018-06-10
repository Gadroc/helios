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

namespace GadrocsWorkshop.Helios.Controls
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Globalization;
    using System.Windows;

    [HeliosControl("FA18C.AMPCD", "AMPCD", "F/A-18C", typeof(MFDRenderer))]
    class AMPCD_FA18C : MFD
    {
        private static readonly Rect SCREEN_RECT = new Rect(45, 86, 343, 312);
        private Rect _scaledScreenRect = SCREEN_RECT;

        public AMPCD_FA18C()
            : base("AMPCD", new Size(727, 746))
        {
            AddButton("OSB1", 39, 567, false);
            AddButton("OSB2", 39, 487, false);
            AddButton("OSB3", 39, 409, false);
            AddButton("OSB4", 39, 329, false);
            AddButton("OSB5", 39, 247, false);

            AddButton("OSB6", 186, 112, true);
            AddButton("OSB7", 266, 112, true);
            AddButton("OSB8", 346, 112, true);
            AddButton("OSB9", 424, 112, true);
            AddButton("OSB10", 506, 112, true);

            AddButton("OSB11", 645, 247, false);
            AddButton("OSB12", 645, 329, false);
            AddButton("OSB13", 645, 409, false);
            AddButton("OSB14", 645, 487, false);
            AddButton("OSB15", 645, 567, false);

            AddButton("OSB16", 506, 696, true);
            AddButton("OSB17", 424, 696, true);
            AddButton("OSB18", 346, 696, true);
            AddButton("OSB19", 266, 696, true);
            AddButton("OSB20", 186, 696, true);

            AddRocker("Day / Night", "triangles-light", 106, 58, false);
            AddRocker("Symbols", "triangles-light", 588, 58, false);
            AddRocker("Gain", "triangles-light", 39, 650, false);
            AddRocker("Contrast", "triangles-light", 645, 650, false);

            AddThreeWayToggle("Heading", 28, 25, new Size(50, 100));
            AddThreeWayToggle("Course", 651, 25, new Size(50, 100));

            Helios.Controls.RotarySwitch knob = new Helios.Controls.RotarySwitch();
            knob.Name = "Mode Knob";
            knob.KnobImage = "{Helios}/Images/AV-8B/Common Knob.png";
            knob.DrawLabels = false;
            knob.DrawLines = false;
            knob.Positions.Clear();
            knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(knob, 0, "Off", 225d));
            knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(knob, 1, "Middle", 0d));
            knob.Positions.Add(new Helios.Controls.RotarySwitchPosition(knob, 2, "Bright", 135d));
            knob.CurrentPosition = 0;
            knob.Top = 37;
            knob.Left = 336;
            knob.Width = 60;
            knob.Height = 60;

            Children.Add(knob);
            foreach (IBindingTrigger trigger in knob.Triggers)
            {
                AddTrigger(trigger, "Mode Knob");
            }
            AddAction(knob.Actions["set.position"], "Mode Knob");
        }

        #region Properties

        public override string BezelImage
        {
            get { return "{Helios}/Images/FA-18C/AMPCD frame.png"; }
        }

        #endregion

        protected override void OnPropertyChanged(PropertyNotificationEventArgs args)
        {
            if (args.PropertyName.Equals("Width") || args.PropertyName.Equals("Height"))
            {
                double scaleX = Width / NativeSize.Width;
                double scaleY = Height / NativeSize.Height;
                _scaledScreenRect.Scale(scaleX, scaleY);
            }
            base.OnPropertyChanged(args);
        }

        private void AddButton(string name, double x, double y, bool horizontal)
        {
            Helios.Controls.PushButton button = new Helios.Controls.PushButton();
            button.Top = y;
            button.Left = x;
            button.Width = 42;
            button.Height = 42;
            //button.TextPushOffset = new System.Windows.Media.TranslateTransform(1,1);
            button.Image = "{Helios}/Images/Buttons/tactile-light-square.png";
            button.PushedImage = "{Helios}/Images/Buttons/tactile-light-square.png";
            if (horizontal) button.Text = "|";
            else
            {
                button.TextFormat.FontSize = 32;
                button.TextFormat.FontWeight = FontWeights.Bold;
                button.Text = "--";
            }


            button.Name = name;

            Children.Add(button);

            AddTrigger(button.Triggers["pushed"], name);
            AddTrigger(button.Triggers["released"], name);

            AddAction(button.Actions["push"], name);
            AddAction(button.Actions["release"], name);
            AddAction(button.Actions["set.physical state"], name);
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

        private void AddRocker(string name, string imagePrefix, double x, double y, bool horizontal)
        {
            Helios.Controls.ThreeWayToggleSwitch rocker = new Helios.Controls.ThreeWayToggleSwitch();
            rocker.Name = name;
            rocker.SwitchType = Helios.Controls.ThreeWayToggleSwitchType.MomOnMom;
            rocker.ClickType = Helios.Controls.ClickType.Touch;
            rocker.PositionTwoImage = "{Helios}/Images/Rockers/" + imagePrefix + "-norm.png";

            rocker.Top = y;
            rocker.Left = x;
            if (horizontal)
            {
                rocker.Orientation = Helios.Controls.ToggleSwitchOrientation.Horizontal;
                rocker.PositionOneImage = "{Helios}/Images/Rockers/" + imagePrefix + "-left.png";
                rocker.PositionThreeImage = "{Helios}/Images/Rockers/" + imagePrefix + "-right.png";
                rocker.Width = 80;
                rocker.Height = 40;
            }
            else
            {
                rocker.Orientation = Helios.Controls.ToggleSwitchOrientation.Vertical;
                rocker.PositionOneImage = "{Helios}/Images/Rockers/" + imagePrefix + "-up.png";
                rocker.PositionThreeImage = "{Helios}/Images/Rockers/" + imagePrefix + "-down.png";
                rocker.Width = 40;
                rocker.Height = 80;
            }

            Children.Add(rocker);

            foreach (IBindingTrigger trigger in rocker.Triggers)
            {
                AddTrigger(trigger, name);
            }

            AddAction(rocker.Actions["set.position"], name);
        }
        private void AddThreeWayToggle(string name, double x, double y, Size size)
        {
            Helios.Controls.ThreeWayToggleSwitch toggle = new Helios.Controls.ThreeWayToggleSwitch();
            toggle.Top = y;
            toggle.Left = x;
            toggle.Width = size.Width;
            toggle.Height = size.Height;
            toggle.DefaultPosition = ThreeWayToggleSwitchPosition.Two;
            toggle.PositionOneImage = "{Helios}/Images/Toggles/orange-round-up.png";
            toggle.PositionTwoImage = "{Helios}/Images/Toggles/orange-round-norm.png";
            toggle.PositionThreeImage = "{Helios}/Images/Toggles/orange-round-down.png";
            toggle.SwitchType = ThreeWayToggleSwitchType.MomOnMom;
            toggle.Name = name;

            Children.Add(toggle);
            foreach (IBindingTrigger trigger in toggle.Triggers)
            {
                AddTrigger(trigger, name);
            }
            foreach (IBindingAction action in toggle.Actions)
            {
                AddAction(action, name);
            }

            //AddTrigger(toggle.Triggers["pushed"], name);
            //AddTrigger(toggle.Triggers["released"], name);

            //AddAction(toggle.Actions["push"], name);
            //AddAction(toggle.Actions["release"], name);
            //AddAction(toggle.Actions["set.physical state"], name);
        }

        public override bool HitTest(Point location)
        {
            if (_scaledScreenRect.Contains(location))
            {
                return false;
            }

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

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

    [HeliosControl("AV8B.MPCD", "MPCD", "AV-8B", typeof(MFDRenderer))]
    class MPCD_AV8B : MFD
    {
        private static readonly Rect SCREEN_RECT = new Rect(45, 86, 343, 312);
        private Rect _scaledScreenRect = SCREEN_RECT;

        public MPCD_AV8B()
            : base("MPCD", new Size(435, 475))
        {
            AddButton("OSB1", 0, 332, false);
            AddButton("OSB2", 0, 282, false);
            AddButton("OSB3", 0, 234, false);
            AddButton("OSB4", 0, 188, false);
            AddButton("OSB5", 0, 140, false);

            AddButton("OSB6", 96, 50, true);
            AddButton("OSB7", 150, 50, true);
            AddButton("OSB8", 204, 50, true);
            AddButton("OSB9", 256, 50, true);
            AddButton("OSB10", 310, 50, true);

            AddButton("OSB11", 404, 140, false);
            AddButton("OSB12", 404, 188, false);
            AddButton("OSB13", 404, 234, false);
            AddButton("OSB14", 404, 282, false);
            AddButton("OSB15", 404, 332, false);

            AddButton("OSB16", 310, 418, true);
            AddButton("OSB17", 256, 418, true);
            AddButton("OSB18", 204, 418, true);
            AddButton("OSB19", 150, 418, true);
            AddButton("OSB20", 96, 418,true);

            AddRocker("Day / Night", "triangles-light", 59, 26, false);
            AddRocker("Symbols", "triangles-light", 347, 26, false);
            AddRocker("Gain", "triangles-light", 3, 390, false);
            AddRocker("Contrast", "triangles-light", 405, 390, false);

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
            knob.Top = 4;
            knob.Left = 200;
            knob.Width = 40;
            knob.Height = 40;

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
            get { return "{Helios}/Images/AV-8B/MPCD bezel.png"; }
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
            button.Width = 32;
            button.Height = 32;
            //button.TextPushOffset = new System.Windows.Media.TranslateTransform(1,1);
            button.Image = "{Helios}/Images/Buttons/tactile-light-square.png";
            button.PushedImage = "{Helios}/Images/Buttons/tactile-light-square.png";
            if (horizontal) button.Text = "|";
            else
            {
                button.TextFormat.FontSize = 28;
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
                rocker.Width = 56;
                rocker.Height = 30;
            }
            else
            {
                rocker.Orientation = Helios.Controls.ToggleSwitchOrientation.Vertical;
                rocker.PositionOneImage = "{Helios}/Images/Rockers/" + imagePrefix + "-up.png";
                rocker.PositionThreeImage = "{Helios}/Images/Rockers/" + imagePrefix + "-down.png";
                rocker.Width = 30;
                rocker.Height = 56;
            }

            Children.Add(rocker);

            foreach (IBindingTrigger trigger in rocker.Triggers)
            {
                AddTrigger(trigger, name);
            }

            AddAction(rocker.Actions["set.position"], name);
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

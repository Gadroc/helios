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

    [HeliosControl("FA18C.MPCD", "MPCD", "F/A-18C", typeof(MFDRenderer))]
    class MPCD_FA18C : MFD
    {
        private static readonly Rect SCREEN_RECT = new Rect(72, 137, 497, 493);
        private Rect _scaledScreenRect = SCREEN_RECT;

        public MPCD_FA18C()
            : base("MPCD", new Size(656, 706))
        {
            AddButton("OSB1", 14, 540, true);
            AddButton("OSB2", 14, 455, true);
            AddButton("OSB3", 14, 370, true);
            AddButton("OSB4", 14, 285, true);
            AddButton("OSB5", 14, 200, true);

            AddButton("OSB6", 150, 74, false);
            AddButton("OSB7", 230, 74, false);
            AddButton("OSB8", 310, 74, false);
            AddButton("OSB9", 390, 74, false);
            AddButton("OSB10", 470, 74, false);

            AddButton("OSB11", 604, 200, true);
            AddButton("OSB12", 604, 285, true);
            AddButton("OSB13", 604, 370, true);
            AddButton("OSB14", 604, 455, true);
            AddButton("OSB15", 604, 540, true);

            AddButton("OSB16", 470, 652, false);
            AddButton("OSB17", 390, 652, false);
            AddButton("OSB18", 310, 652, false);
            AddButton("OSB19", 230, 652, false);
            AddButton("OSB20", 150, 652, false);

            AddKnob("Mode Knob", new Point(298, 14), new Size(50, 50));
            AddKnob("Brightness Knob",  new Point(14, 632), new Size(50, 50));
            AddKnob("Contrast Knob",   new Point(592, 632), new Size(50, 50));
        }

        #region Properties

        public override string BezelImage
        {
            get { return "{Helios}/Images/FA-18C/MPCD frame.png"; }
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

        private void AddKnob(string name, Point posn, Size size)
        {
            Helios.Controls.Potentiometer _knob = new Helios.Controls.Potentiometer();
            _knob.Name = name;
            _knob.KnobImage = "{Helios}/Images/AV-8B/Common Knob.png";
            _knob.InitialRotation = 219;
            _knob.RotationTravel = 291;
            _knob.MinValue = 0;
            _knob.MaxValue = 1;
            _knob.InitialValue = 0;
            _knob.StepValue = 0.1;
            _knob.Top = posn.Y;
            _knob.Left = posn.X;
            _knob.Width = size.Width;
            _knob.Height = size.Height;

            Children.Add(_knob);
            foreach (IBindingTrigger trigger in _knob.Triggers)
            {
                AddTrigger(trigger, name);
            }
            AddAction(_knob.Actions["set.value"], name);
        }

        private void AddButton(string name, double x, double y, bool horizontal)
        {
            Helios.Controls.PushButton button = new Helios.Controls.PushButton();
            button.Top = y;
            button.Left = x;
            button.Width = 40;
            button.Height = 40;
            //button.TextPushOffset = new System.Windows.Media.TranslateTransform(1,1);
            //button.Image = "{Helios}/Images/FA-18C/MFD Button Up.png";
            //button.PushedImage = "{Helios}/Images/FA-18C/MFD Button Dn.png";
            if (!horizontal)
            {
                button.Image = "{Helios}/Images/FA-18C/MFD Button UpV.png";
                button.PushedImage = "{Helios}/Images/FA-18C/MFD Button DnV.png";
                //button.Rotation = HeliosVisualRotation.CCW;
            }
            else
            {
                button.Image = "{Helios}/Images/FA-18C/MFD Button UpH.png";
                button.PushedImage = "{Helios}/Images/FA-18C/MFD Button DnH.png";
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

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

namespace GadrocsWorkshop.Helios.Gauges.FA18C
{
    using GadrocsWorkshop.Helios.Gauges.FA18C;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Windows.Media;
    using System.Windows;

    [HeliosControl("Helios.FA18C.UFC", "Up Front Controller", "F/A-18C", typeof(FA18CDeviceRenderer))]
    class UFC_FA18C: FA18CDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 1, 1);
        private Rect _scaledScreenRect = SCREEN_RECT;

        private string _aircraft;
        private String _font = "MS 33558";
        //private String _font = "Franklin Gothic";
        public UFC_FA18C()
            : base("Up Front Controller", new Size(552, 431))
        {
            AddButton("EMCON", 483, 115, new Size(48, 48));
            AddButton("1", 95, 105, new Size(48, 48));
            AddButton("2", 152, 105, new Size(48, 48));
            AddButton("3", 208, 105, new Size(48, 48));
            AddButton("4", 95, 161, new Size(48, 48));
            AddButton("5", 152, 161, new Size(48, 48));
            AddButton("6", 208, 161, new Size(48, 48));
            AddButton("7", 95, 218, new Size(48, 48));
            AddButton("8", 152, 218, new Size(48, 48));
            AddButton("9", 208, 218, new Size(48, 48));
            AddButton("CLR", 95, 275, new Size(48, 48));
            AddButton("0", 152, 275, new Size(48, 48),true);
            AddButton("ENT", 208, 275, new Size(48, 48));
            AddButton("AP", 107, 363, new Size(48, 48),true);
            AddButton("IFF", 157, 363, new Size(48, 48));
            AddButton("TCN", 207, 363, new Size(48, 48));
            AddButton("ILS", 257, 363, new Size(48, 48),true);
            AddButton("DL", 307, 363, new Size(48, 48), true);
            AddButton("BCN", 357, 363, new Size(48, 48));
            AddButton("ONOFF", 407, 363, new Size(48, 48));
            AddButtonIP("IP", 25, 52, new Size(40, 40));
            AddButtonIP("ODU 1", 276, 36, new Size(40, 40));
            AddButtonIP("ODU 2", 276, 96, new Size(40, 40));
            AddButtonIP("ODU 3", 276, 159, new Size(40, 40));
            AddButtonIP("ODU 4", 276, 219, new Size(40, 40));
            AddButtonIP("ODU 5", 276, 281, new Size(40, 40));
            AddThreeWayToggle("ADF", 30, 108, new Size(30, 60));

            AddPot("UFC Display Brightness", new Point(480, 58), new Size(48, 48));
            AddPot("Radio Volume 1", new Point(20, 193), new Size(48, 48));
            AddPot("Radio Volume 2", new Point(481, 193), new Size(48, 48));
            AddEncoder("Radio 1", new Point(17, 359), new Size(75, 75));
            AddButtonIP("Radio 1 Pull", 41, 383, new Size(28, 28),false);
            AddEncoder("Radio 2", new Point(462, 359), new Size(75, 75));
            AddButtonIP("Radio 2 Pull", 485, 383, new Size(28, 28),false);
        }

        public override string BezelImage
        {
            get { return "{Helios}/Gauges/FA-18C/UFC/UFC Faceplate.png"; }
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
        private void AddPot(string name, Point posn, Size size)
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
        private void AddEncoder(string name, Point posn, Size size)
        {
            Helios.Controls.RotaryEncoder _knob = new Helios.Controls.RotaryEncoder();
            _knob.Name = name;
            //_knob.KnobImage = "{Helios}/Images/AV-8B/AV8BNA_Rotary5.png";
            _knob.KnobImage = "{Helios}/Images/FA-18C/UFC Rotator_U.png";
            _knob.StepValue = 0.1;
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
            foreach (IBindingAction action in _knob.Actions)
            {
                AddAction(action, name);
            }

            //AddAction(_knob.Actions["set.value"], name);
            //AddAction(_knob.Actions["push"], name);
            //AddAction(_knob.Actions["release"], name);
        }
        private void AddButton(string name, double x, double y, Size size) { AddButton(name, x, y, size, false); }
         private void AddButton(string name, double x, double y, Size size, bool altImage)
        {
        Helios.Controls.PushButton button = new Helios.Controls.PushButton();
            if (altImage)
            {
                _aircraft = "FA-18C";
            } else
            {
                _aircraft = "AV-8B";
            }
            button.Top = y;
            button.Left = x;
            button.Width = size.Width;
            button.Height = size.Height;
            button.Image = "{Helios}/Images/" + _aircraft + "/UFC Button Up " + name + ".png";
            button.PushedImage = "{Helios}/Images/" + _aircraft + "/UFC Button Dn " + name + ".png";
            button.Text = "";
            button.Name = "UFC Key " + name;

            Children.Add(button);

            AddTrigger(button.Triggers["pushed"], "UFC Key " + name);
            AddTrigger(button.Triggers["released"], "UFC Key " + name);

            AddAction(button.Actions["push"], "UFC Key " + name);
            AddAction(button.Actions["release"], "UFC Key " + name);
            AddAction(button.Actions["set.physical state"], "UFC Key " + name);
        }
        private void AddButtonIP(string name, double x, double y, Size size)
        { AddButtonIP(name, x, y, size, true); }
        private void AddButtonIP(string name, double x, double y, Size size,Boolean glyph)
        {
            Helios.Controls.PushButton button = new Helios.Controls.PushButton();
            button.Top = y;
            button.Left = x;
            button.Width = size.Width;
            button.Height = size.Height;
            button.Image = "{Helios}/Images/Buttons/tactile-dark-round.png";
            button.PushedImage = "{Helios}/Images/Buttons/tactile-dark-round-in.png";
            button.Text = "";
            button.Name = "UFC Key " + name;
            if (glyph)
            {
                button.Glyph = PushButtonGlyph.Circle;
                button.GlyphThickness = 3;
                button.GlyphColor = Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0);
            }
            Children.Add(button);

            AddTrigger(button.Triggers["pushed"], "UFC Key " + name);
            AddTrigger(button.Triggers["released"], "UFC Key " + name);

            AddAction(button.Actions["push"], "UFC Key " + name);
            AddAction(button.Actions["release"], "UFC Key " + name);
            AddAction(button.Actions["set.physical state"], "UFC Key " + name);
        }
        private void AddIndicator(string name, double x, double y, Size size) { AddIndicator(name, x, y, size, false); }
        private void AddIndicator(string name, double x, double y, Size size, bool _vertical)
        {
            Helios.Controls.Indicator indicator = new Helios.Controls.Indicator();
            indicator.Top = y;
            indicator.Left = x;
            indicator.Width = size.Width;
            indicator.Height = size.Height;
            indicator.OnImage = "{Helios}/Images/Indicators/anunciator.png";
            indicator.OffImage = "{Helios}/Images/Indicators/anunciator.png";
            if(name == "Unknown 1")
            {
                indicator.Text = ". . .";
            }
            else
            {
                indicator.Text = name;
            }
            indicator.Name = "Annunciator " + name;
            indicator.OnTextColor = Color.FromArgb(0xff, 0x24, 0x8D, 0x22);
            indicator.OffTextColor = Color.FromArgb(0xff, 0x1C, 0x1C, 0x1C);
            indicator.TextFormat.FontStyle = FontStyles.Normal;
            indicator.TextFormat.FontWeight = FontWeights.Normal;
            if (_vertical)
            {
                indicator.TextFormat.FontSize = 8;
            }
            else
            {
                indicator.TextFormat.FontSize = 12;
            }
            indicator.TextFormat.FontFamily = new FontFamily(_font);
            indicator.TextFormat.PaddingLeft = 0;
            indicator.TextFormat.PaddingRight = 0;
            indicator.TextFormat.PaddingTop = 0;
            indicator.TextFormat.PaddingBottom = 0;
            indicator.TextFormat.VerticalAlignment = TextVerticalAlignment.Center;
            indicator.TextFormat.HorizontalAlignment = TextHorizontalAlignment.Center;

            Children.Add(indicator);
            foreach (IBindingTrigger trigger in indicator.Triggers)
            {
                AddTrigger(trigger, name);
            }
            AddAction(indicator.Actions["set.indicator"], name);
        }
        private void AddIndicatorPushButton(string name, double x, double y, Size size)
        {
            Helios.Controls.IndicatorPushButton indicator = new Helios.Controls.IndicatorPushButton();
            indicator.Top = y;
            indicator.Left = x;
            indicator.Width = size.Width;
            indicator.Height = size.Height;
            indicator.Image = "{Helios}/Images/Indicators/indicator.png";
            indicator.PushedImage = "{Helios}/Images/Indicators/indicator-push.png";
            indicator.Text = name;
            indicator.Name = name;
            if (name == "MASTER WARNING")
            {
                indicator.OnTextColor = Color.FromArgb(0xff, 0xc7, 0x1e, 0x1e);
            }
            else
            {
                indicator.OnTextColor = Color.FromArgb(0xff, 0xb3, 0xa2, 0x29);
            }
            indicator.TextColor = Color.FromArgb(0xff, 0x1C, 0x1C, 0x1C);
            indicator.TextFormat.FontStyle = FontStyles.Normal;
            indicator.TextFormat.FontWeight = FontWeights.Normal;
            indicator.TextFormat.FontSize = 18;
            indicator.TextFormat.FontFamily = new FontFamily(_font);
            indicator.TextFormat.PaddingLeft = 0;
            indicator.TextFormat.PaddingRight = 0;
            indicator.TextFormat.PaddingTop = 0;
            indicator.TextFormat.PaddingBottom = 0;
            indicator.TextFormat.VerticalAlignment = TextVerticalAlignment.Center;
            indicator.TextFormat.HorizontalAlignment = TextHorizontalAlignment.Center;

            Children.Add(indicator);
            AddTrigger(indicator.Triggers["pushed"], name);
            AddTrigger(indicator.Triggers["released"], name);

            AddAction(indicator.Actions["push"], name);
            AddAction(indicator.Actions["release"], name);
            AddAction(indicator.Actions["set.indicator"], name);
         }
        private void AddThreeWayToggle(string name, double x, double y, Size size)
        {
            Helios.Controls.ThreeWayToggleSwitch toggle = new Helios.Controls.ThreeWayToggleSwitch();
            toggle.Top = y;
            toggle.Left = x;
            toggle.Width = size.Width;
            toggle.Height = size.Height;
            toggle.DefaultPosition = ThreeWayToggleSwitchPosition.Two;
            toggle.PositionOneImage = "{Helios}/Images/Toggles/round-up.png";
            toggle.PositionTwoImage = "{Helios}/Images/Toggles/round-norm.png";
            toggle.PositionThreeImage = "{Helios}/Images/Toggles/round-down.png";
            toggle.SwitchType = ThreeWayToggleSwitchType.OnOnOn;
            toggle.Name = name;

            Children.Add(toggle);
            foreach (IBindingTrigger trigger in toggle.Triggers)
            {
                AddTrigger(trigger, name);
            }
            AddAction(toggle.Actions["set.position"], name);
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

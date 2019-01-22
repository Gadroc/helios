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
    using GadrocsWorkshop.Helios.Gauges;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Windows.Media;
    using System.Windows;

    [HeliosControl("Helios.FA18C.UFCv2", "Up Front Controller-v2", "F/A-18C", typeof(FA18CDeviceRenderer))]
    class UFC_FA18C_V2: FA18CDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 1, 1);
        private Rect _scaledScreenRect = SCREEN_RECT;

        private string _aircraft;
        private String _font = "MS 33558";
        //private String _font = "Franklin Gothic";
        public UFC_FA18C_V2()
            : base("Up Front Controller", new Size(602, 470))
        {
            AddButton("EMCON", 527, 129, new Size(48, 48));
            AddButton("1", 105, 116, new Size(48, 48));
            AddButton("2", 167, 116, new Size(48, 48));
            AddButton("3", 229, 116, new Size(48, 48));
            AddButton("4", 105, 179, new Size(48, 48));
            AddButton("5", 167, 179, new Size(48, 48));
            AddButton("6", 229, 179, new Size(48, 48));
            AddButton("7", 105, 240, new Size(48, 48));
            AddButton("8", 167, 240, new Size(48, 48));
            AddButton("9", 229, 240, new Size(48, 48));
            AddButton("CLR", 105, 303, new Size(48, 48));
            AddButton("0", 167, 303, new Size(48, 48),true);
            AddButton("ENT", 229, 303, new Size(48, 48));
            AddButton("AP", 125, 400, new Size(40, 40),true);
            AddButton("IFF", 176, 400, new Size(40, 40));
            AddButton("TCN", 229, 400, new Size(40, 40));
            AddButton("ILS", 284, 400, new Size(40, 40),true);
            AddButton("DL", 337, 400, new Size(40, 40), true);
            AddButton("BCN", 393, 400, new Size(40, 40));
            AddButton("ONOFF", 447, 400, new Size(40, 40));
            AddButtonIP("IP", 28, 60, new Size(40, 40));
            AddButtonIP("ODU 1", 302, 42, new Size(40, 40));
            AddButtonIP("ODU 2", 302, 107, new Size(40, 40));
            AddButtonIP("ODU 3", 302, 175, new Size(40, 40));
            AddButtonIP("ODU 4", 302, 241, new Size(40, 40));
            AddButtonIP("ODU 5", 302, 310, new Size(40, 40));
            AddThreeWayToggle("ADF", 33, 122, new Size(30, 60));

            AddPot("UFC Display Brightness", new Point(528, 66), new Size(48, 48));
            AddPot("Radio Volume 1", new Point(25, 213), new Size(48, 48));
            AddPot("Radio Volume 2", new Point(528, 213), new Size(48, 48));
            AddEncoder("Radio 1", new Point(29, 383), new Size(75, 75));
            AddButtonIP("Radio 1 Pull", 52, 408, new Size(28, 28),false);
            AddEncoder("Radio 2", new Point(500, 383), new Size(75, 75));
            AddButtonIP("Radio 2 Pull", 523, 408, new Size(28, 28),false);

            /// adding the diplays
            int heightDisp = 35;
            int SPWidth = 30;
            int SPHeight = 50;
            int SPStart = 90;
            AddTextDisplay16Segment("ScratchPad1", SPStart, heightDisp, new Size(SPWidth + 1, SPHeight), "+");
            AddTextDisplay16Segment("ScratchPad2", SPStart + SPWidth, heightDisp, new Size(SPWidth + 1, SPHeight), "-");
            AddTextDisplay7Segment("Scratchpad Number", SPStart + 2 * SPWidth, heightDisp, new Size(150, SPHeight), "305.000");

            AddTextDisplay16Segment("Radio Channel 1", 25, 314, new Size(42, 42), "+");
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
            Helios.Controls.Potentiometer _knob = new Helios.Controls.Potentiometer
            {
                Name = name,
                KnobImage = "{Helios}/Images/AV-8B/Common Knob.png",
                InitialRotation = 219,
                RotationTravel = 291,
                MinValue = 0,
                MaxValue = 1,
                InitialValue = 0,
                StepValue = 0.1,
                Top = posn.Y,
                Left = posn.X,
                Width = size.Width,
                Height = size.Height
            };

            Children.Add(_knob);
            foreach (IBindingTrigger trigger in _knob.Triggers)
            {
                AddTrigger(trigger, name);
            }
            AddAction(_knob.Actions["set.value"], name);
        }
        private void AddEncoder(string name, Point posn, Size size)
        {
            Helios.Controls.RotaryEncoder _knob = new Helios.Controls.RotaryEncoder
            {
                Name = name,
                //_knob.KnobImage = "{Helios}/Images/AV-8B/AV8BNA_Rotary5.png";
                KnobImage = "{Helios}/Images/FA-18C/UFC Rotator_U.png",
                StepValue = 0.1,
                RotationStep = 5,
                Top = posn.Y,
                Left = posn.X,
                Width = size.Width,
                Height = size.Height
            };

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

        private void AddTextDisplay16Segment(string name, double x, double y, Size size, string testDisp) {
            Helios.Controls.TextDisplay display = new Helios.Controls.TextDisplay
            {
                Top = y,
                Left = x,
                Width = size.Width,
                Height = size.Height,
                Name = name
            };
            // display.FontSize = 20;
            TextFormat textFormat = new TextFormat
            {
                FontFamily = new FontFamily("Hornet_UFC"),
                HorizontalAlignment = TextHorizontalAlignment.Center,
                VerticalAlignment = TextVerticalAlignment.Center,
                FontSize = 25
            };
            // textFormat.FontFamily.Baseline = 0.01;
            // textFormat.PaddingRight = 3;
            display.TextFormat = textFormat;
            display.OnTextColor = Color.FromRgb(10, 200, 10);
            display.TextTestValue = testDisp;
            // display.OnImage = "{Helios}/Images/Indicators/indicator.png";
            Children.Add(display);
            AddAction(display.Actions["set.TextDisplay"], "UFC Display " + name);
        }

        private void AddTextDisplay7Segment(string name, double x, double y, Size size, string testText)
        {
            Helios.Controls.TextDisplay display = new Helios.Controls.TextDisplay
            {
                Top = y,
                Left = x,
                Width = size.Width,
                Height = size.Height,
                Name = name
            };
            // display.FontSize = 20;
            TextFormat textFormat = new TextFormat
            {
                FontFamily = new FontFamily("Hornet_UFC Numerals"),
                HorizontalAlignment = TextHorizontalAlignment.Left,
                VerticalAlignment = TextVerticalAlignment.Center,
                FontSize = 25
            };
            textFormat.PaddingRight = 0.02;
            display.TextFormat = textFormat;
            display.OnTextColor = Color.FromRgb(10, 200, 10);
            display.TextTestValue = testText;
            // display.OnImage = "{Helios}/Images/Indicators/indicator.png";
            Children.Add(display);
            AddAction(display.Actions["set.TextDisplay"], "UFC Display " + name);
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
            Helios.Controls.PushButton button = new Helios.Controls.PushButton
            {
                Top = y,
                Left = x,
                Width = size.Width,
                Height = size.Height,
                Image = "{Helios}/Images/Buttons/tactile-dark-round.png",
                PushedImage = "{Helios}/Images/Buttons/tactile-dark-round-in.png",
                Text = "",
                Name = "UFC Key " + name
            };
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
            Helios.Controls.Indicator indicator = new Helios.Controls.Indicator
            {
                Top = y,
                Left = x,
                Width = size.Width,
                Height = size.Height,
                OnImage = "{Helios}/Images/Indicators/anunciator.png",
                OffImage = "{Helios}/Images/Indicators/anunciator.png"
            };
            if (name == "Unknown 1")
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
            Helios.Controls.IndicatorPushButton indicator = new Helios.Controls.IndicatorPushButton
            {
                Top = y,
                Left = x,
                Width = size.Width,
                Height = size.Height,
                Image = "{Helios}/Images/Indicators/indicator.png",
                PushedImage = "{Helios}/Images/Indicators/indicator-push.png",
                Text = name,
                Name = name
            };
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
            Helios.Controls.ThreeWayToggleSwitch toggle = new Helios.Controls.ThreeWayToggleSwitch
            {
                Top = y,
                Left = x,
                Width = size.Width,
                Height = size.Height,
                DefaultPosition = ThreeWayToggleSwitchPosition.Two,
                PositionOneImage = "{Helios}/Images/Toggles/round-up.png",
                PositionTwoImage = "{Helios}/Images/Toggles/round-norm.png",
                PositionThreeImage = "{Helios}/Images/Toggles/round-down.png",
                SwitchType = ThreeWayToggleSwitchType.OnOnOn,
                Name = name
            };

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

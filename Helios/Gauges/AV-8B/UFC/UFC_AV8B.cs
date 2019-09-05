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
    using System.Windows.Media;
    using System.Windows;
    using System.Windows.Threading;


    [HeliosControl("Helios.AV8B.UFC", "Up Front Controller", "AV-8B", typeof(AV8BDeviceRenderer))]
    class UFC_AV8B : AV8BDevice
    {
        // these three sections are the dead space in the UFC image.
        private static readonly Rect SCREEN_RECT_L = new Rect(0, 135, 38, 415);
        private Rect _scaledScreenRectL = SCREEN_RECT_L;
        private static readonly Rect SCREEN_RECT_LB = new Rect(38, 476, 103, 74);
        private Rect _scaledScreenRectLB = SCREEN_RECT_LB;
        private static readonly Rect SCREEN_RECT_R = new Rect(743, 102, 65, 448);
        private Rect _scaledScreenRectR = SCREEN_RECT_R;
        private String _font = "MS 33558";
        private string _interfaceDeviceName = "UFC";
        private string _ufcNumbers16 = "`0=«;`1=¬;`2=­;`3=®;`4=¯;`5=°;`6=±;`7=²;`8=³;`9=´;~0=µ;0=¡;1=¢;2=£;3=¤;4=¥;5=¦;6=§;7=¨;8=©;9=ª;_=É"; //Numeric mapping into characters in the UFC font
        private string _ufcCueing = "!=È";

        public UFC_AV8B()
            : base("UFC", new Size(808, 550))
        {
            AddButton("TMR", 190, 54, new Size(60, 60), "UFC Function Selector Pushbutton TMR");
            AddButton("TOO", 190, 126, new Size(60, 60), "UFC Function Selector Pushbutton TOO");
            AddButton("1", 266, 126, new Size(60, 60), "UFC Keypad Pushbutton 1");
            AddButton("2", 337, 126, new Size(60, 60), "UFC Keypad Pushbutton 2");
            AddButton("3", 410, 126, new Size(60, 60), "UFC Keypad Pushbutton 3");
            AddButton("CLR", 482, 126, new Size(60, 60), "UFC Keypad Pushbutton CLR");
            AddButton("4", 266, 196, new Size(60, 60), "UFC Keypad Pushbutton 4");
            AddButton("5", 337, 196, new Size(60, 60), "UFC Keypad Pushbutton 5");
            AddButton("6", 410, 196, new Size(60, 60), "UFC Keypad Pushbutton 6");
            AddButton("SVE", 482, 196, new Size(60, 60), "UFC Keypad Pushbutton SVE");
            AddButton("7", 266, 267, new Size(60, 60), "UFC Keypad Pushbutton 7");
            AddButton("8", 337, 267, new Size(60, 60), "UFC Keypad Pushbutton 8");
            AddButton("9", 410, 267, new Size(60, 60), "UFC Keypad Pushbutton 9");
            AddButton("Dash", 482, 267, new Size(60, 60), "UFC Keypad Pushbutton -");
            AddButton("ENT", 266, 337, new Size(60, 60), "UFC Keypad Pushbutton ENT");
            AddButton("0", 337, 337, new Size(60, 60), "UFC Keypad Pushbutton 0");
            AddButton("Dot", 410, 337, new Size(60, 60), "UFC Keypad Pushbutton .");
            AddButton("ONOFF", 482, 337, new Size(60, 60), "UFC Function Selector Pushbutton ON/OFF");
            AddButton("IFF", 266, 407, new Size(60, 60), "UFC Function Selector Pushbutton IFF");
            AddButton("TCN", 337, 407, new Size(60, 60), "UFC Function Selector Pushbutton TCN");
            AddButton("AWL", 410, 407, new Size(60, 60), "UFC Function Selector Pushbutton AWL");
            AddButton("WPN", 482, 407, new Size(60, 60), "UFC Function Selector Pushbutton WPN");
            AddButton("WOF", 266, 477, new Size(60, 60), "UFC Function Selector Pushbutton WOF");
            AddButton("BCN", 337, 477, new Size(60, 60), "UFC Function Selector Pushbutton BCN");
            AddButton("ALT", 410, 477, new Size(60, 60), "UFC Function Selector Pushbutton ALT");
            AddButton("EMCON", 482, 477, new Size(60, 60), "UFC Emission Control Pushbutton");
            AddButtonIP("IP", 566, 129, new Size(50, 50), "UFC I/P Pushbutton");

            _interfaceDeviceName = "Caution Indicators";
            AddIndicator("15 SEC", 50, 231, new Size(60, 31), "Caution 15 Sec Indicator");
            AddIndicator("MFS", 50, 272, new Size(60, 31), "Caution MFS");
            AddIndicator("BINGO", 50, 313, new Size(60, 31), "Caution Bingo");
            AddIndicator("H2O", 50, 354, new Size(60, 31), "Caution H2O");
            AddIndicator("Unknown 1", 50, 394, new Size(60, 31), "Caution Unknown 1");
            AddIndicator("Unknown 2", 50, 433, new Size(60, 31), "Caution Unknown 2");
            AddIndicator("L FUEL", 48, 145, new Size(32, 73), "Caution L Fuel Indicator");
            AddIndicator("R FUEL", 90, 145, new Size(32, 73), "Caution R fuel Indicator");
            AddIndicator("FIRE", 678, 108, new Size(60, 31), "Warning Fire");
            AddIndicator("LAW", 678, 149, new Size(60, 31), "Warning LAW");
            AddIndicator("FLAPS", 678, 190, new Size(60, 31), "Warning Flaps");
            AddIndicator("L TANK", 676, 231, new Size(60, 31), true, "Warning L Tank");  
            AddIndicator("R TANK", 676, 272, new Size(60, 31), true, "Warning R Tank");  
            AddIndicator("HYD", 675, 314, new Size(60, 31), "Warning HYD");
            AddIndicator("GEAR", 675, 354, new Size(60, 31), "Warning Gear");
            AddIndicator("OT", 674, 394, new Size(60, 31), "Warning OT");
            AddIndicator("JPTL", 674, 435, new Size(60, 31), "Warning JPTL");
            AddIndicator("EFC", 673, 475, new Size(60, 31), "Warning EFC");
            AddIndicator("GEN", 673, 514, new Size(60, 31), "Warning GEN");
            AddIndicatorPushButton("MASTER CAUTION", 9, 9, new Size(110, 80), "Master Caution Button");
            AddIndicatorPushButton("MASTER WARNING", 687, 9, new Size(110, 80), "Master Warning Button");
            //AddFunction(new PushButton(this, LTWCA, "3198", "198", "Caution/Warning", "Master Caution Button"));
            //AddFunction(new FlagValue(this, "196", "Caution/Warning", "Master Caution", "Master Caution indicator"));
            //AddFunction(new PushButton(this, LTWCA, "3199", "199", "Caution/Warning", "Master Warning Button"));
            //AddFunction(new FlagValue(this, "197", "Caution/Warning", "Master Warning", "Master warning indicator"));
            _interfaceDeviceName = "UFC";
            AddPot("UFC Display Brightness", new Point(564, 42), new Size(50, 50), "UFC Brightness Control Knob");
            AddPot("Radio Volume 1", new Point(196, 210), new Size(50, 50), "UFC COMM 1 Volume Control Knob");
            AddPot("Radio Volume 2", new Point(570, 210), new Size(50, 50), "UFC COMM 2 Volume Control Knob");
            AddEncoder("Radio 1", new Point(160, 405), new Size(100, 100), "UFC COMM 1 Channel Selector Knob");
            AddEncoder("Radio 2", new Point(557, 405), new Size(100, 100), "UFC COMM 2 Channel Selector Knob");
            AddButton("Comm 1", 190, 435, new Size(40, 40), "UFC COMM 1 Channel Selector Pull");
            AddButton("Comm 2", 587, 435, new Size(40, 40), "UFC COMM 2 Channel Selector Pull");
        }

        public override string BezelImage
        {
            get { return "{AV-8B}/Images/AV-8B UFC 1080.png"; }
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

        private void AddEncoder(string name, Point posn, Size size, string interfaceElementName)
        {
            AddEncoder(
                name: name,
                size: size,
                posn: posn,
                knobImage: "{AV-8B}/Images/AV8BNA_Rotary5.png",
                stepValue: 0.1,
                rotationStep: 5,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
        }

        //private void AddTextDisplay(string name, double x, double y, Size size, string testDisp)
        //{
        //    AddTextDisplay(name, x, y, size, 32, testDisp, TextHorizontalAlignment.Left);
        //}

        private void AddTextDisplay(string name, double x, double y, Size size,
            string interfaceElementName, double baseFontsize, string testDisp, TextHorizontalAlignment hTextAlign, string ufcDictionary)
        {
            TextDisplay display = AddTextDisplay(
                name: name,
                posn: new Point(x, y),
                size: size,
                font: "Hornet UFC",
                baseFontsize: baseFontsize,
                horizontalAlignment: hTextAlign,
                verticalAligment: TextVerticalAlignment.Center,
                testTextDisplay: testDisp,
                textColor: Color.FromArgb(0xff, 0x7e, 0xde, 0x72),
                backgroundColor: Color.FromArgb(0xff, 0x26, 0x3f, 0x36),
                useBackground: false,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                textDisplayDictionary: ufcDictionary
                );
        }


        private void AddButton(string name, double x, double y, Size size, string interfaceElementName)
        {
            Point pos = new Point(x, y);
            AddButton(
                name: name,
                posn: pos,
                size: size,
                image: "{AV-8B}/Images/UFC Button Up " + name + ".png",
                pushedImage: "{AV-8B}/Images/UFC Button Up " + name + ".png",
                buttonText: "",
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
        }

        private void AddButtonIP(string name, double x, double y, Size size, string interfaceElementName)
        { AddButtonIP(name, x, y, size, interfaceElementName, true); }
        private void AddButtonIP(string name, double x, double y, Size size, string interfaceElementName, Boolean glyph)
        {
            Point pos = new Point(x, y);
            PushButton button = AddButton(
                name: name,
                posn: pos,
                size: size,
                image: "{Helios}/Images/Buttons/tactile-dark-round.png",
                pushedImage: "{Helios}/Images/Buttons/tactile-dark-round-in.png",
                buttonText: "",
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );

            if (glyph)
            {
                button.Glyph = PushButtonGlyph.Circle;
                button.GlyphThickness = 3;
                button.GlyphColor = Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0);
            }
        }
        private void AddIndicator(string name, double x, double y, Size size, string interfaceElementName) { AddIndicator(name, x, y, size, false, interfaceElementName); }
        private void AddIndicator(string name, double x, double y, Size size, bool _vertical, string interfaceElementName)
        {
            Indicator indicator = AddIndicator(
                name: name,
                posn: new Point(x, y),
                size: size,
                onImage: "{Helios}/Images/Indicators/anunciator.png",
                offImage: "{Helios}/Images/Indicators/anunciator.png",
                onTextColor: Color.FromArgb(0xff, 0x24, 0x8D, 0x22),
                offTextColor: Color.FromArgb(0xff, 0x1C, 0x1C, 0x1C),
                font: _font,
                vertical: _vertical,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
                if (name == "Unknown 1" || name == "Unknown 2")
                    {
                        indicator.Text = ".";
                    }
                    else
                    {
                        indicator.Text = name;
                    }
                indicator.Name = "UFC_" + name;
                //indicator.Name = "Annunciator " + name;
                indicator.OnTextColor = Color.FromArgb(0xff, 0x94, 0xEB, 0xA6);
                indicator.OffTextColor = Color.FromArgb(0xff, 0x10, 0x10, 0x10);
                indicator.TextFormat.FontStyle = FontStyles.Normal;
                indicator.TextFormat.FontWeight = FontWeights.Normal;
                if (_vertical)
                {
                    if (_font == "MS 33558")
                    {
                        indicator.TextFormat.FontSize = 8;
                    }
                    else
                    {
                        indicator.TextFormat.FontSize = 11;
                    }
                }
                else
                {
                    indicator.TextFormat.FontSize = 12;
                }
                indicator.TextFormat.FontFamily = new FontFamily(_font);  // this probably needs to change before release
                indicator.TextFormat.PaddingLeft = 0;
                indicator.TextFormat.PaddingRight = 0;
                indicator.TextFormat.PaddingTop = 0;
                indicator.TextFormat.PaddingBottom = 0;
                indicator.TextFormat.VerticalAlignment = TextVerticalAlignment.Center;
                indicator.TextFormat.HorizontalAlignment = TextHorizontalAlignment.Center;
        }

        private void AddIndicatorPushButton(string name, double x, double y, Size size, string interfaceElementName)
        {
            Color onTextColor;
            if (name == "MASTER WARNING")
            {
                onTextColor = Color.FromArgb(0xff, 0xc7, 0x1e, 0x1e);
            }
            else
            {
                onTextColor = Color.FromArgb(0xff, 0xb3, 0xa2, 0x29);
            }

            IndicatorPushButton indicator = AddIndicatorPushButton(
                name: name,
                posn: new Point(x, y),
                size: size,
                image: "{Helios}/Images/Indicators/indicator.png",
                pushedImage: "{Helios}/Images/Indicators/indicator-push.png",
                textColor: Color.FromArgb(0xff, 0x1C, 0x1C, 0x1C),
                onTextColor: onTextColor,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                font: _font
                );
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

        }
        private void AddThreeWayToggle(string name, double x, double y, Size size, string interfaceElementName)
        {

            AddThreeWayToggle(
                name: name,
                posn: new Point(x, y),
                size: size,
                defaultPosition: ThreeWayToggleSwitchPosition.Two,
                defaultType: ThreeWayToggleSwitchType.OnOnOn,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
        }


        public override bool HitTest(Point location)
        {
            if (_scaledScreenRectL.Contains(location) || _scaledScreenRectLB.Contains(location) || _scaledScreenRectR.Contains(location))
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
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


    [HeliosControl("Helios.AV8B.ODU", "Option Display Unit", "AV-8B", typeof(AV8BDeviceRenderer))]
    class ODU_AV8B : AV8BDevice
    {
        // these three sections are the dead space in the ODU image.

            // these need to be recalculated!!!!!

        private Rect _scaledScreenRectTL = new Rect(0, 0, 398, 116);
        private Rect _scaledScreenRectB = new Rect(76, 384, 648, 87);
        private string _interfaceDeviceName = "ODU";
        private string _ufcNumbers16 = "`0=«;`1=¬;`2=­;`3=®;`4=¯;`5=°;`6=±;`7=²;`8=³;`9=´;~0=µ;0=¡;1=¢;2=£;3=¤;4=¥;5=¦;6=§;7=¨;8=©;9=ª;_=É"; //Numeric mapping into characters in the UFC font
        private string _ufcCueing = "!=È";


        public ODU_AV8B()
            : base("Option Display Unit", new Size(800, 471))
        {
            AddButton("ODU Button 1", 410, 50, new Size(65, 65), "ODU Option Select Pushbutton 1");
            AddButton("ODU Button 2", 84, 150, new Size(65, 65), "ODU Option Select Pushbutton 2");
            AddButton("ODU Button 3", 410, 150, new Size(65, 65), "ODU Option Select Pushbutton 3");
            AddButton("ODU Button 4", 84, 250, new Size(65, 65), "ODU Option Select Pushbutton 4");
            AddButton("ODU Button 5", 410, 250, new Size(65, 65), "ODU Option Select Pushbutton 5");
            //Await discovery of the text values for these displays
            //AddTextDisplay("OptionDisplay1", 486, 57, new Size(238, 56), "Option Display 1", 48, "~~~~", TextHorizontalAlignment.Left, _ufcNumbers16);
            //AddTextDisplay("OptionCueing1", 486, 57, new Size(40, 56), "Option Display 1 Selected", 48, "~", TextHorizontalAlignment.Left, _ufcCueing);
            //AddTextDisplay("OptionDisplay2", 158, 155, new Size(238, 56), "Option Display 2", 48, "~~~~", TextHorizontalAlignment.Left, _ufcNumbers16);
            //AddTextDisplay("OptionCueing2", 158, 155, new Size(40, 56), "Option Display 2 Selected", 48, "~", TextHorizontalAlignment.Left, _ufcCueing);
            //AddTextDisplay("OptionDisplay3", 486, 155, new Size(238, 56), "Option Display 3", 48, "~~~~", TextHorizontalAlignment.Left, _ufcNumbers16);
            //AddTextDisplay("OptionCueing3", 486, 155, new Size(40, 56), "Option Display 3 Selected", 48, "~", TextHorizontalAlignment.Left, _ufcCueing);
            //AddTextDisplay("OptionDisplay4", 158, 254, new Size(238, 56), "Option Display 4", 48, "~~~~", TextHorizontalAlignment.Left, _ufcNumbers16);
            //AddTextDisplay("OptionCueing4", 158, 254, new Size(40, 56), "Option Display 4 Selected", 48, "~", TextHorizontalAlignment.Left, _ufcCueing);
            //AddTextDisplay("OptionDisplay5", 486, 254, new Size(238, 56), "Option Display 5", 48, "~~~~", TextHorizontalAlignment.Left, _ufcNumbers16);
            //AddTextDisplay("OptionCueing5", 486, 254, new Size(40, 56), "Option Display 5 Selected", 48, "~", TextHorizontalAlignment.Left, _ufcCueing);
        }

        public override string BezelImage
        {
            get { return "{AV-8B}/Images/ODU Panel.png"; }
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

        private void AddButton(string name, double x, double y, Size size, string interfaceElementName)
        { AddButton(name, x, y, size, interfaceElementName, true); }
        private void AddButton(string name, double x, double y, Size size, string interfaceElementName, Boolean glyph)
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
                useBackground: true,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                textDisplayDictionary: ufcDictionary
                );
        }

        public override bool HitTest(Point location)
        {
            if (_scaledScreenRectTL.Contains(location) || _scaledScreenRectB.Contains(location))
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
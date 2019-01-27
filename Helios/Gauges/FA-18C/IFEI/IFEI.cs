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

    [HeliosControl("Helios.FA18C.IFEI", "FA18C Integrated Fuel & Engine Indicator", "F/A-18C", typeof(FA18CDeviceRenderer))]
    class IFEI_FA18C : FA18CDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 1, 1);
        private Rect _scaledScreenRect = SCREEN_RECT;

        private string _aircraft;
        private String _font = "Seven Segement";
        private Color _textColor = Color.FromRgb(12, 12, 12);
        private Color _backGroundColor = Color.FromRgb(12, 100, 12);
        private string _imageLocation = "{Helios}/Gauges/FA-18C/IFEI/";
        private bool _useBackGround = true;

        public IFEI_FA18C()
            : base("IFEI", new Size(779, 702))
        {
            double spacing = 70;
            double start = 64;
            AddButton("MODE", 402, start, new Size(87, 62));
            AddButton("QTY", 402, start + spacing, new Size(87, 62));
            AddButton("UP", 402, start + 2 * spacing, new Size(87, 62));
            AddButton("DOWN", 402, start + 3 * spacing, new Size(87, 62));
            AddButton("ZONE", 402, start + 4 * spacing, new Size(87, 62));
            AddButton("ET", 402, start + 5 * spacing, new Size(87, 62));
 
        }

        public override string BezelImage
        {
            get { return _imageLocation + "IFEI.png"; }
        }

        private void AddTextDisplay(string name, double x, double y, Size size, double baseFontsize, string testDisp, bool hTextAlignedRight)
        {
            TextDisplay display = AddTextDisplay(
                name: name,
                pos: new Point(x, y),
                size: size,
                font: "Hornet_UFC",
                baseFontsize: baseFontsize,
                horizontalAlignment: TextHorizontalAlignment.Left,
                verticalAligment: TextVerticalAlignment.Top,
                testTextDisplay: testDisp,
                textColor: Color.FromArgb(0xff, 0x40, 0xb3, 0x29),
                backgroundColor: Color.FromArgb(0xff, 0x00, 0x00, 0x00),
                useBackground: true
                );

            if (hTextAlignedRight)
            {
                display.TextFormat.HorizontalAlignment = TextHorizontalAlignment.Right;
            }
        }

        private void AddButton(string name, double x, double y, Size size)
        {
            Point pos = new Point(x, y);
            AddButton(
                name: name,
                posn: pos,
                size: size,
                image: _imageLocation + "IFEI_" + name + ".png",
                pushedImage: _imageLocation + "IFEI_" + name + "DN.png",
                buttonText: ""
                );
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

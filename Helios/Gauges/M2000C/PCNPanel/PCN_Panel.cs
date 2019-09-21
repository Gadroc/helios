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

namespace GadrocsWorkshop.Helios.Gauges.M2000C
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("HELIOS.M2000C.PCN_PANEL", "PCN Panel", "M2000C Gauges", typeof(M2000CDeviceRenderer))]
    class M2000C_PCNPanel : M2000CDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 690, 530);
        private string _interfaceDeviceName = "PCN Panel";
        private Rect _scaledScreenRect = SCREEN_RECT;

        public M2000C_PCNPanel()
            : base("PCN Panel", new Size(690, 530))
        {
            int row0 = 231, row1 = 234, row2 = 233, row3 = 316, row4 = 323, row5 = 396, row6 = 394, row7 = 482, row8 = 480, row9 = 115;
            int column0 = 121, column1 = 208, column2 = 220, column3 = 321, column4 = 327, column5 = 426, column6 = 507, column7 = 586, column8 = 395, column9 = 453, column10 = 506;
//            AddPushButton("INS PREP Switch", "ins-prep", new Point(column0, row0), new Size(60, 60));
//            AddPushButton("INS DEST Switch", "ins-dest", new Point(column3, row0), new Size(60, 60));
            AddPushButton("INS Button 1", "ins-1" ,new Point(column5, row2), new Size(52, 60));
            AddPushButton("INS Button 2", "ins-2", new Point(column6, row2), new Size(52, 60));
            AddPushButton("INS Button 3", "ins-3", new Point(column7, row2), new Size(52, 60));
            AddPushButton("INS Button 4", "ins-4", new Point(column5, row3), new Size(52, 60));
            AddPushButton("INS Button 5", "ins-5", new Point(column6, row3), new Size(52, 60));
            AddPushButton("INS Button 6", "ins-6", new Point(column7, row3), new Size(52, 60));
            AddPushButton("INS Button 7", "ins-7", new Point(column5, row5), new Size(52, 60));
            AddPushButton("INS Button 8", "ins-8", new Point(column6, row5), new Size(52, 60));
            AddPushButton("INS Button 9", "ins-9", new Point(column7, row5), new Size(52, 60));
            AddPushButton("INS Button 0", "ins-0", new Point(column6, row8), new Size(52, 60));
//            AddPushButton("INS Clear Button", "ins-clear", new Point(column5, row8), new Size(60, 60));
//            AddPushButton("INS ENTER Button", "ins-enter", new Point(column7, row8), new Size(60, 60));

            AddIndicator("PRET", "pret", new Point(column8, row9), new Size(5, 9));
            AddIndicator("ALN", "aln", new Point(column9, row9), new Size(5, 9));
            AddIndicator("MIP", "mip", new Point(column10, row9), new Size(5, 9));

            AddSwitch("INS Parameter Selector", "{M2000C}/Images/PCNPanel/ins-parameter-selector.png", new Point(149, 349), new Size(118, 118), true);
        }

        #region Properties

        public override string BezelImage
        {
            get { return "{M2000C}/Images/PCNPanel/pcn-panel.png"; }
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

        private void AddPushButton(string name, string imagePrefix, Point posn, Size size)
        {
            AddButton(name: name,
                posn: posn,
                size: size,
                image: "{M2000C}/Images/PCNPanel/" + imagePrefix + ".png",
                pushedImage: "{M2000C}/Images/PCNPanel/" + imagePrefix + ".png",
                buttonText: "",
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                fromCenter: true);
        }

        private void AddIndicator(string name, string imagePrefix, Point posn, Size size)
        {
            AddIndicator(
                name: name,
                posn: posn,
                size: size,
                onImage: "{M2000C}/Images/PCNPanel/" + imagePrefix + "-on.png",
                offImage: "{M2000C}/Images/Miscellaneous/void.png", //empty picture to permit the indicator to work because I’ve nothing to display when off
                onTextColor: Color.FromArgb(0xff, 0x7e, 0xde, 0x72), //don’t need it because not using text
                offTextColor: Color.FromArgb(0xff, 0x7e, 0xde, 0x72), //don’t need it because not using text
                font: "", //don’t need it because not using text
                vertical: false, //don’t need it because not using text
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                fromCenter: true,
                withText: false); //added in Composite Visual as an optional value with a default value set to true
        }

        private void AddSwitch(string name, string knobImage, Point posn, Size size, bool fromCenter)
        {
            AddRotarySwitch(name: name,
                posn: posn,
                size: size,
                knobImage: knobImage,
                defaultPosition: 0, //Helios.Gauges.M2000C.RSPositions[] positions,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                fromCenter: fromCenter);
        }
/*
        private void AddSwitch(string name, string imagePrefix, Point posn, Size size, ToggleSwitchPosition defaultPosition, ToggleSwitchType defaultType, bool horizontal)
        {
            AddToggleSwitch(name: name,
                posn: posn,
                size: size,
                defaultPosition: defaultPosition,
                positionOneImage: "{M2000C}/Images/PCNPanel/" + imagePrefix + ".png",
                positionTwoImage: "{M2000C}/Images/Switches/" + imagePrefix + "up.png",
                defaultType: defaultType,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                horizontal: horizontal,
                horizontalRender: horizontal,
                nonClickableZones: null,
                fromCenter: false);
        }
        */
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

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

    [HeliosControl("Helios.AV8B.GearIndicatorPanel", "Gear Indicator Panel", "AV-8B Gauges", typeof(AV8BDeviceRenderer))]
    class GearIndicatorPanel: AV8BDevice
    {
        private string _interfaceDeviceName = "Landing Gear";
        private string _imageLocation = "{AV-8B}/Images/";
        private String _font = "MS 33558";

        public GearIndicatorPanel()
            : base("Gear Indicator Panel", new Size(248, 418))
        {
            AddIndicator("Nose Green", 80, 35, new Size(82, 46), "Nose");
            AddIndicator("Nose Amber", 80, 93, new Size(82, 46), "Nose Wrn");
            AddIndicator("Left Green", 30, 160, new Size(82, 46), "Left");
            AddIndicator("Left Amber", 30, 219, new Size(82, 46), "Left Wrn");
            AddIndicator("Right Green", 132, 160, new Size(82, 46), "Right");
            AddIndicator("Right Amber", 132, 219, new Size(82, 46), "Right Wrn");
            AddIndicator("Main Green", 80, 287, new Size(82, 46), "Main");
            AddIndicator("Main Amber", 80, 345, new Size(82, 46), "Main Wrn");
        }
        public override string BezelImage
        {
            get { return _imageLocation + "WQHD/Panel/Landing Gear Indicator.png"; }
        }
            
        private void AddIndicator(string name, double x, double y, Size size, string interfaceElementName) { AddIndicator(name, x, y, size, false, interfaceElementName); }
        private void AddIndicator(string name, double x, double y, Size size, bool _vertical, string interfaceElementName)
        {
            Indicator indicator = AddIndicator(
                name: name,
                posn: new Point(x, y),
                size: size,
                onImage: _imageLocation + "WQHD/indicator/Gear " + name + ".png",
                offImage: _imageLocation + "_transparent.png",
                onTextColor: System.Windows.Media.Color.FromArgb(0x00, 0xff, 0xff, 0xff),
                offTextColor: System.Windows.Media.Color.FromArgb(0x00, 0x00, 0x00, 0x00),
                font: _font,
                vertical: _vertical,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
                indicator.Text = "";
                indicator.Name = "Gear Indicator Panel_" + name;
        }

        public override bool HitTest(Point location)
        {
            //if (_scaledScreenRect.Contains(location))
            //{
            //    return false;
            //}

            return false;
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

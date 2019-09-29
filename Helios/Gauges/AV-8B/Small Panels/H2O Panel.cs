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

    [HeliosControl("Helios.AV8B.H2OPanel", "H2O Panel", "AV-8B Gauges", typeof(AV8BDeviceRenderer))]
    class H2OPanel: AV8BDevice
    {
        private string _interfaceDeviceName = "LH Flaps & Water";
        private string _font = "MS 33558";
        private string _imageLocation = "{AV-8B}/Images/";

        public H2OPanel()
            : base("H2O Panel", new Size(160, 476))
        {
            AddButton("Combat", 50, 162, new Size(92, 100), "Combat Thrust Button");
            AddIndicator("CMBT", 71, 214, new Size(55, 28), "CMBT indicator");
            AddIndicator("SEL", 71, 183, new Size(55, 28), "SEL indicator");
            AddIndicator("STO", 95,399, new Size(33,54), "STO indicator");
            AddThreeWayToggle("H2O", 55, 18, new Size(100, 134), "H2O Mode Switch");

        }
        public override string BezelImage
        {
            get { return _imageLocation + "WQHD/Panel/H2O.png"; }
        }
            
        private void AddButton(string name, double x, double y, string interfaceElementName) { AddButton(name, x, y, false, interfaceElementName); }
        private void AddButton(string name, double x, double y, Size size, string interfaceElementName) { AddButton(name, x, y, size, false, interfaceElementName); }
        private void AddButton(string name, double x, double y, bool horizontal, string interfaceElementName) { AddButton(name, x, y, new Size(40,40),false, interfaceElementName); }
        private void AddButton(string name, double x, double y, Size size, bool horizontal, string interfaceElementName) { AddButton(name, x, y, size, horizontal, false, interfaceElementName); }
        private void AddButton(string name, double x, double y, Size size, bool horizontal, bool altImage, string interfaceElementName)
        {
            Point pos = new Point(x, y);
            PushButton button = AddButton(
                    name: name,
                    posn: pos,
                    size: size,
                    image: _imageLocation + "WQHD/Button/" + name + " Normal.png",
                    pushedImage: _imageLocation + "WQHD/Button/" + name + " Pushed.png",
                    buttonText: "",
                    interfaceDeviceName: _interfaceDeviceName,
                    interfaceElementName: interfaceElementName,
                    fromCenter: false
                    );
            button.Name = "H2O Panel_" + name;
        }
        private void AddThreeWayToggle(string name, double x, double y, Size size, string interfaceElementName)
        {
            ThreeWayToggleSwitch toggle = AddThreeWayToggle(
                name: name,
                posn: new Point(x, y),
                size: size,
                defaultPosition: ThreeWayToggleSwitchPosition.Two,
                defaultType: ThreeWayToggleSwitchType.OnOnOn,
                positionOneImage: _imageLocation + "WQHD/Switch/" + name + " Up.png",
                positionTwoImage: _imageLocation + "WQHD/Switch/" + name + " Middle.png",
                positionThreeImage: _imageLocation + "WQHD/Switch/" + name + " Down.png",
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
            toggle.Name = "H2O Panel_" + name;
        }

        private void AddIndicator(string name, double x, double y, Size size, string interfaceElementName) { AddIndicator(name, x, y, size, false, interfaceElementName); }
        private void AddIndicator(string name, double x, double y, Size size, bool _vertical, string interfaceElementName)
        {
            Indicator indicator = AddIndicator(
                name: name,
                posn: new Point(x, y),
                size: size,
                onImage: _imageLocation + "WQHD/Indicator/" + name + " On.png",
                offImage: _imageLocation + "WQHD/Indicator/" + name + " Off.png",
                onTextColor: System.Windows.Media.Color.FromArgb(0x00, 0xff, 0xff, 0xff),
                offTextColor: System.Windows.Media.Color.FromArgb(0x00, 0x00, 0x00, 0x00),
                font: _font,
                vertical: _vertical,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
                indicator.Text = "";
                indicator.Name = "H2O Panel_" + name;
        }

        public override bool HitTest(Point location)
        {
            //if (_scaledScreenRect.Contains(location))
            //{
            //    return false;
            //}

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

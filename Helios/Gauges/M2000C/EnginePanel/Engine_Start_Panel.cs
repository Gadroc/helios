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

    [HeliosControl("HELIOS.M2000C.ENGINE_PANEL", "Engine Start Panel", "M2000C Gauges", typeof(M2000CDeviceRenderer))]
    class M2000C_EnginePanel : M2000CDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 700, 251);
        private string _interfaceDeviceName = "Engine Start Panel";
        private Rect _scaledScreenRect = SCREEN_RECT;

        public M2000C_EnginePanel()
            : base("Engine Start Panel", new Size(700, 251))
        {
            int row0 = 44, row1 = 163;

            AddSwitch("Starter Fuel Pump Switch", "long-black-", new Point(208, row0), ToggleSwitchPosition.Two, ToggleSwitchType.OnOn);
            AddSwitch("Left Fuel Pump Switch", "long-black-", new Point(343, row0), ToggleSwitchPosition.Two, ToggleSwitchType.OnOn);
            AddSwitch("Right Fuel Pump Switch", "long-black-", new Point(484, row0), ToggleSwitchPosition.Two, ToggleSwitchType.OnOn);
            AddSwitch("Fuel Shut-Off Switch", "long-black-", new Point(440, 163), ToggleSwitchPosition.Two, ToggleSwitchType.OnOn);

            AddPushButton("Engine Start Switch");

// Need to find a solution because adding the covers prevent the user to use the switches/button under
//            AddCover("Engine Start Switch Cover", "engine-start-", new Point(82, 37), new Size(206, 169), ToggleSwitchPosition.One, ToggleSwitchType.OnOn, false);
//            AddCover("Fuel Cutoff Switch Cover", "coupe-feu-", new Point(395, row1), new Size(137,50), ToggleSwitchPosition.Two, ToggleSwitchType.OnOn, false);

            Add3PosnToggle(
                name: "Ignition Ventilation Selector Switch",
                posn: new Point(246, row1),
                image: "{M2000C}/Images/Switches/long-black-",
                interfaceDevice: _interfaceDeviceName,
                interfaceElement: "Ignition Ventilation Selector Switch",
                fromCenter: false
                );

        }

        #region Properties

        public override string BezelImage
        {
            get { return "{M2000C}/Images/EnginePanel/engine-start-panel.png"; }
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

        private void AddSwitch(string name, string imagePrefix, Point posn, ToggleSwitchPosition defaultPosition, ToggleSwitchType defaultType)
        {
            AddToggleSwitch(name: name,
                posn: posn,
                size: new Size(37, 75),
                defaultPosition: defaultPosition,
                positionOneImage: "{M2000C}/Images/Switches/" + imagePrefix + "down.png",
                positionTwoImage: "{M2000C}/Images/SWitches/" + imagePrefix + "up.png",
                defaultType: defaultType,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                horizontal: true,
                fromCenter: false);
        }

        private void AddCover(string name, string imagePrefix, Point posn, Size size, ToggleSwitchPosition defaultPosition, ToggleSwitchType defaultType, bool horizontal = true)
        {
            AddToggleSwitch(name: name,
                posn: posn,
                size: size,
                defaultPosition: defaultPosition,
                positionOneImage: "{M2000C}/Images/EnginePanel/" + imagePrefix + "guarded.png",
                positionTwoImage: "{M2000C}/Images/EnginePanel/" + imagePrefix + "unguarded.png",
                defaultType: defaultType,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                horizontal: horizontal,
                fromCenter: false);
        }

        private void Add3PosnToggle(string name, Point posn, string image, string interfaceDevice, string interfaceElement, bool fromCenter)
        {
            AddThreeWayToggle(
                name: name,
                posn: posn,
                size: new Size(37, 75),
                positionOneImage: image + "down.png",
                positionTwoImage: image + "mid.png",
                positionThreeImage: image + "up.png",
                defaultPosition: ThreeWayToggleSwitchPosition.One,
                defaultType: ThreeWayToggleSwitchType.OnOnOn,
                interfaceDeviceName: interfaceDevice,
                interfaceElementName: interfaceElement,
                horizontal: true,
                clickType: ClickType.Touch,
                fromCenter: false
                );
        }

        private void AddPushButton(string name)
        {
            AddButton(name: name,
                posn: new Point(98,92),
                size: new Size(102,102), 
                image: "{M2000C}/Images/EnginePanel/start-engine-not-pushed.png", 
                pushedImage: "{M2000C}/Images/EnginePanel/start-engine-pushed.png",
                buttonText: "", 
                interfaceDeviceName: _interfaceDeviceName, 
                interfaceElementName: name, 
                fromCenter: false);
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

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

namespace GadrocsWorkshop.Helios.Gauges.M2000C.TACANChannel
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("HELIOS.M2000C.TACAN_PANEL", "Tacan Panel", "M2000C Gauges", typeof(M2000CDeviceRenderer))]
    class M2000C_TacanPanel : M2000CDevice
    {
        private HeliosValue _channel;
        private HeliosValue _xyMode;
        private GaugeDrumCounter _tensDrum;
        private GaugeDrumCounter _onesDrum;
        private GaugeImage _xModeImage;
        private GaugeImage _yModeImage;
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 256, 280);
        private string _interfaceDeviceName = "Tacan Panel";
        private Rect _scaledScreenRect = SCREEN_RECT;

        public M2000C_TacanPanel()
            : base("Tacan Panel", new Size(350, 312))
        {
            RotarySwitch rsXYModeSelector = new RotarySwitch();
            RotarySwitch rsModeSelector = new RotarySwitch();

            AddRotarySwitch(rsXYModeSelector, "X/Y Selector", "low-", new Point(83, 193), new RotarySwitchPosition[] {
                new RotarySwitchPosition(rsXYModeSelector, 0, "X", 0d),
                new RotarySwitchPosition(rsXYModeSelector, 1, "Y", 10d) });
            AddRotarySwitch(rsModeSelector, "Mode Selector", "low-", new Point(262, 191), new RotarySwitchPosition[] {
                new RotarySwitchPosition(rsModeSelector, 0, "OFF", 180d),
                new RotarySwitchPosition(rsModeSelector, 1, "REC", 200d),
                new RotarySwitchPosition(rsModeSelector, 2, "TR", 220d),
                new RotarySwitchPosition(rsModeSelector, 3, "AA", 240d) }); 
            //                new SwitchPosition("0.0", "X", "3624"),
            //                new SwitchPosition("1.0", "Y", "3624") });

            AddPot("Channel 10 Selector", new Point(83, 192), "up-",
                20d, 26.692d, 0.0d, 0.9228d, 0.0769d, 0.0769d, "Channel 10 Selector", false);
            AddPot("Channel 1 Selector", new Point(262, 190), "up-",
                216d, 36d, 0.0d, 0.9d, 0.6d, 0.1d, "Channel 1 Selector", false);
/*
            new GaugeImage("{Helios}/Gauges/M2000C/TACANChannel/tacan_channel_faceplate.xaml", new Rect(0d, 0d, 243d, 100d));

            AddGaugeDrumCounter("Tacan Hundred","{Helios}/Gauges/M2000C/TACANChannel/tacan_channel_hundreds_tape.xaml", new Point(84d, 11.5d), "##", new Size(10d, 15d), new Size(50d, 75d));
//            _tensDrum = new GaugeDrumCounter("{Helios}/Gauges/M2000C/TACANChannel/tacan_channel_hundreds_tape.xaml", new Point(84d, 11.5d), "##", new Size(10d, 15d), new Size(50d, 75d));
//            _tensDrum.Clip = new RectangleGeometry(new Rect(84d, 11.5d, 100d, 75d));
//            Components.Add(_tensDrum);

            _onesDrum = new GaugeDrumCounter("{Helios}/Gauges/M2000C/Common/drum_tape.xaml", new Point(177d, 11.5d), "#", new Size(10d, 15d), new Size(50d, 75d));
            _onesDrum.Clip = new RectangleGeometry(new Rect(177d, 11.5d, 50d, 75d));
//            Components.Add(_onesDrum);

            _xModeImage = new GaugeImage("{Helios}/Gauges/M2000C/TACANChannel/tacan_channel_x_mode.xaml", new Rect(20d, 20d, 44d, 59d));
  //          Components.Add(_xModeImage);

            _yModeImage = new GaugeImage("{Helios}/Gauges/M2000C/TACANChannel/tacan_channel_y_mode.xaml", new Rect(20d, 20d, 44d, 59d));
            _yModeImage.IsHidden = true;
    //        Components.Add(_yModeImage);*/
        }

        #region Properties

        public override string BezelImage
        {
            get { return "{Helios}/Images/M2000C/TacanPanel/tacan-panel.png"; }
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

        private void AddPot(string name, Point posn, string imagePrefix, double initialRotation, double rotationTravel, double minValue, double maxValue,
            double initialValue, double stepValue, string interfaceElementName, bool fromCenter)
        {
            AddPot(
                name: name,
                posn: posn,
                size: new Size(104, 104),
                knobImage: "{Helios}/Images/M2000C/TacanPanel/" + imagePrefix + "switch-tacan.png",
                initialRotation: initialRotation,
                rotationTravel: rotationTravel,
                minValue: minValue,
                maxValue: maxValue,
                initialValue: initialValue,
                stepValue: stepValue,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: true,
                clickType: ClickType.Touch);
        }

        private void AddRotarySwitch(RotarySwitch rotarySwitch, string name, string imagePrefix, Point posn, RotarySwitchPosition[] switchPositions)
        {
            AddRotarySwitch(rotarySwitch: rotarySwitch,
                name: name,
                posn: posn,
                size: new Size(133, 133),
                knobImage: "{Helios}/Images/M2000C/TacanPanel/" + imagePrefix + "switch-tacan.png",
//                switchPositions: switchPositions,
                defaultPosition: 0,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                fromCenter: true,
                clickType: ClickType.Touch);
         }
         
/*        private void AddGaugeDrumCounter(string name, string gaugeImage, Point posn, string format, Size size, Size rendererSize)
        {
            AddGaugeDrumCounter(name: name,
                gaugeImage: gaugeImage,
                posn: posn,
                format: format,
                size: size,
                rendererSize: rendererSize,
                RectangleGeometry: new RectangleGeometry(new Rect(posn, new Size(rendererSize.Width*format.Length,rendererSize.Height));
        }*/

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

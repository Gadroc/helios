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

    [HeliosControl("HELIOS.M2000C.ENGINESENSORS_PANEL", "Engine Sensors Panel", "M2000C Gauges", typeof(M2000CDeviceRenderer))]
    class M2000C_ENGINESENSORSPanel : M2000CDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 151, 270);
        private string _interfaceDeviceName = "Engine Sensors Panel";
        private Rect _scaledScreenRect = SCREEN_RECT;

        public M2000C_ENGINESENSORSPanel()
            : base("Engine Sensors Panel", new Size(151, 270))
        {
            AddDrum("Engine RPM (%) (Tens)", "{Helios}/Gauges/M2000C/Common/drum_tape.xaml", "tens engine pourcentage", "(0 - 10)", "##",
                new Point(48, 104), new Size(10d, 30d), new Size(14d, 30d));
            AddDrum("Engine RPM (%) (Ones)", "{Helios}/Gauges/M2000C/Common/drum_tape.xaml", "ones engine pourcentage", "(0 - 9)", "#",
                new Point(76, 101), new Size(10d, 36d), new Size(14d, 36d));
            AddNeedle("Engine RPM Needle", "{Helios}/Gauges/M2000C/Common/needleA.xaml", "engine pourcentage", "(0 - 100)", 
                new Point(90, 45), new Size(10d, 70d), new Point(90d, 40d), BindingValueUnits.RPMPercent);
            AddNeedle("Engine T7 Needle", "{Helios}/Gauges/M2000C/Common/needleB.xaml", "engine pourcentage", "(0 - 10)",
                new Point(90, 210), new Size(10d, 80d), new Point(90d, 200d), BindingValueUnits.Numeric);
        }

        #region Properties

        public override string BezelImage
        {
            get { return "{M2000C}/Images/EngineSensorsPanel/engine-sensors-panel.png"; }
        }

        #endregion

        private void AddDrum(string name, string gaugeImage, string actionIdentifier, string valueDescription, string format, Point posn, Size size, Size renderSize)
        {
            AddDrumGauge(name: name,
                gaugeImage: gaugeImage,
                posn: posn,
                size: size,
                renderSize: renderSize,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                actionIdentifier: actionIdentifier,
                valueDescription: valueDescription,
                format: format,
                fromCenter: false);
        }

        private void AddNeedle(string name, string needleImage, string actionIdentifier, string valueDescription, Point posn, Size size, Point centerPoint, BindingValueUnit typeValue)
        {
            AddNeedle(name: name,
                needleImage: needleImage,
                posn: posn,
                size: size,
                centerPoint: centerPoint,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: name,
                actionIdentifier: actionIdentifier,
                valueDescription: valueDescription,
                typeValue: typeValue,
                fromCenter: false);
        }

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

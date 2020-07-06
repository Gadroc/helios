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

    [HeliosControl("Helios.AV8B.EDP", "Engine Panel", "AV-8B Gauges", typeof(AV8BDeviceRenderer))]
    class EDP: AV8BDevice
    {
        private string _interfaceDeviceName = "EDP";

        public EDP()
            : base("Engine Panel", new Size(528,302))
        {
            AddDisplay("Nozzle Position", new edpNoz(), new Point(405d, 140d), new Size(18d, 72d), "Nozzle Position");  //nozzle needle
            AddDisplay("Engine RPM Indicator", new FourDigitDisplay(), new Point(186, 45), new Size(120, 42), "RPM display");
            AddDisplay("Engine Duct Indicator", new ThreeDigitDisplay(), new Point(44, 45), new Size(90, 42), "Duct pressure display");
            AddDisplay("Engine FF Indicator", new ThreeDigitDisplay(), new Point(44, 137), new Size(90, 42), "FF display");
            AddDisplay("Jet Pipe Temp Indicator", new ThreeDigitDisplay(), new Point(214, 137), new Size(90, 42), "JPT display");
            AddDisplay("Stabilizer Direction Indicator", new stabilizerDisplay(), new Point(44, 232), new Size(30, 42), "Stabilzer Arrow");
            AddDisplay("Stabilizer Angle Indicator", new TwoDigitDisplay(), new Point(73, 232), new Size(60, 42), "Stabiliser display");
            AddDisplay("H2O Amount Indicator", new TwoDigitDisplay(), new Point(214, 232), new Size(60, 42), "H2O display");
            AddIndicator("H2O Flow Indicator",new Point(158,234),new Size(32,32), "H2O flow indicator");
            AddButton("BIT button",-49,6,new Size(60,50), "BIT");
            AddPot("EDP Brightness", new Point(528, 236), new Size(75, 75), "Off/Brightness Control");
            GaugeImage _gi = new GaugeImage("{AV-8B}/Images/WQHD/Panel/EDP Reflection.png", new Rect(0d, 0d, 528d, 302d));
            _gi.Opacity = 0.4;
            //Components.Add(_gi);
            //AddPanel("EDP Reflection", new Point(0, 0), new Size(528, 302), "{AV-8B}/Images/WQHD/Panel/EDP Reflection.png", _interfaceDeviceName, "EDP Reflection Element");

        }
        private void AddPanel(string name, Point posn, Size size, string background, string interfaceDevice, string interfaceElement)
        {
            HeliosPanel _panel = AddPanel(
                 name: name,
                 posn: posn,
                 size: size,
                 background: background
                  );
            _panel.FillBackground = false;
            _panel.DrawBorder = false;
            //_panel.BackgroundAlignment = ImageAlignment.Centered;


        }
        private void AddDisplay(string name, BaseGauge _gauge, Point posn, Size displaySize, string interfaceElementName)
        {
            AddDisplay(
                name: name,
                gauge: _gauge,
                posn: posn,
                size: displaySize,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName
                );
            _gauge.Name = "Engine Panel_" + name;
        }
        private void AddIndicator(string name, Point posn, Size size, string interfaceElementName) { AddIndicator(name, posn, size, false, interfaceElementName); }
        private void AddIndicator(string name, Point posn, Size size, bool _vertical, string interfaceElementName)
        {
            Indicator indicator = AddIndicator(
                name: name,
                posn: posn,
                size: size,
                onImage: "{Helios}/Gauges/AV-8B/Engine Panel/edp_water_light.xaml",
                offImage: "{AV-8B}/Images/_transparent.png",
                onTextColor: System.Windows.Media.Color.FromArgb(0x00, 0x24, 0x8D, 0x22),
                offTextColor: System.Windows.Media.Color.FromArgb(0x00, 0x1C, 0x1C, 0x1C),
                font: "MS 33558",
                vertical: _vertical,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
            indicator.Text = "";
            indicator.Name = "Engine Panel_" + name;
        }
        private void AddButton(string name, double x, double y, Size size, string interfaceElementName)
        {
            Point pos = new Point(x, y);
            AddButton(
                name: name,
                posn: pos,
                size: size,
                image: "{AV-8B}/Images/EDP Bit Button Normal.png",
                pushedImage: "{AV-8B}/Images/EDP Bit Button Pressed.png",
                buttonText: "",
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
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
        public override bool HitTest(Point location)
        {
            //if (_scaledScreenRect.Contains(location))
            //{
            //    return false;
            //}

            return false;  // nothing to press on the EDP so return false.
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
        public override string BezelImage
        {
            get { return "{Helios}/Gauges/AV-8B/Engine Panel/edp_faceplate.xaml"; }
        }
    }
}

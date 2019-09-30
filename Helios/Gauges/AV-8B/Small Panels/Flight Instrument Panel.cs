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
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Windows;

    [HeliosControl("Helios.AV8B.FlightInstruments", "Flight Instrument Panel", "AV-8B Gauges", typeof(AV8BDeviceRenderer))]
    class FlightInstrumentPanel: AV8BDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 1, 1);
        private Rect _scaledScreenRect = SCREEN_RECT;
        private string _imageLocation = "{AV-8B}/Images/";
        private string _interfaceDeviceName = "Flight Instruments";
        private static string _generalComponentName = "Flight Instruments";
        private double _GlassReflectionOpacity = 0;
        public const double GLASS_REFLECTION_OPACITY_DEFAULT = 1.0;

        public FlightInstrumentPanel()
            : base(_generalComponentName, new Size(1030, 739))
        {
            AddGauge("VVI Gauge", new VVI1(), new Point(387, 456), new Size(238, 247), _interfaceDeviceName, "VVI", _generalComponentName);
            AddGauge("IAS Gauge", new IAS(), new Point(70, 186), new Size(203, 203), _interfaceDeviceName, "IAS Airspeed", _generalComponentName);
            AddGauge("AOA Gauge", new AOA(), new Point(61, 473), new Size(221, 221), _interfaceDeviceName, new string[2] { "AOA Flag", "Angle of Attack" }, _generalComponentName);
            AddGauge("ADI Gauge", new ADI(), new Point(350, 59), new Size(293, 293), _interfaceDeviceName, new string[4] { "SAI Pitch" , "SAI Bank", "SAI Cage/Pitch Adjust Knob", "SAI Warning Flag" }, _generalComponentName);
            _interfaceDeviceName = "Flight Instruments";
            AddEncoder("ADI Pitch Adjust", new Point(575, 278), new Size(60, 60), "SAI Cage/Pitch Adjust Knob", "WQHD/Knob/Cage Knob.png");
            _interfaceDeviceName = "NAV course";
            AddEncoder("Course Set Knob", new Point(150, 40), new Size(80, 80), "Course Setting");
            _interfaceDeviceName = "Flight Instruments";
            AddGauge("Altimeter Gauge", new Altimeter(), new Point(727, 351), new Size(261, 272), _interfaceDeviceName, new string[2] { "Air Pressure", "Altitude"  }, _generalComponentName);
            AddEncoder("Altimeter Pressure Adjust", new Point(720, 558), new Size(60,60), "Barometric pressure calibration adjust");
        }

        public double GlassReflectionOpacity
        {
            get
            {
                return _GlassReflectionOpacity;
            }
            set
            {
                //double oldValue = _IFEI_gauges.GlassReflectionOpacity;
                //_IFEI_gauges.GlassReflectionOpacity = value;
                //if (value != oldValue)
                {
                    //OnPropertyChanged("GlassReflectionOpacity", oldValue, value, true);
                }
            }
        }

        public override string BezelImage
        {
            get { return _imageLocation + "WQHD/Panel/Flight Instruments.png"; }
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
                    image: _imageLocation + "WQHD/Button /" + name + " Normal.png",
                    pushedImage: _imageLocation + "WQHD/Button/" + name + " Pushed.png",
                    buttonText: "",
                    interfaceDeviceName: _interfaceDeviceName,
                    interfaceElementName: interfaceElementName,
                    fromCenter: false
                    );
            button.Name = _generalComponentName + "_" + name;
        }
        protected override void OnProfileChanged(HeliosProfile oldProfile)
        {
            base.OnProfileChanged(oldProfile);
        }

        private void AddEncoder(string name, Point posn, Size size, string interfaceElementName)
        {
            AddEncoder(name, posn, size, interfaceElementName, "Rotary.png");
        }

        private void AddEncoder(string name, Point posn, Size size, string interfaceElementName, string _knobName)
        {
            AddEncoder(
                name: name,
                size: size,
                posn: posn,
                knobImage: _imageLocation + _knobName,
                stepValue: 0.1,
                rotationStep: 5,
                interfaceDeviceName: _interfaceDeviceName,
                interfaceElementName: interfaceElementName,
                fromCenter: false
                );
        }

        private void AddButton(string name, double x, double y, Size size, string interfaceDevice, string interfaceElement)
        {
            Point pos = new Point(x, y);
            AddButton(
                name: name,
                posn: pos,
                size: size,
                image: _imageLocation +  name + "Normal.png",
                pushedImage: _imageLocation +  name + " Pushed.png",
                buttonText: "",
                interfaceDeviceName: interfaceDevice,
                interfaceElementName: interfaceElement,
                fromCenter: false
                );
        }
        private void AddPart(string name, CompositeVisual Part, Point posn, Size size, string interfaceDevice, string interfaceElement)
        {
            CompositeVisual _part = AddDevice(
                name: name,
                device: Part,
                size: size,
                posn: posn,
                interfaceDeviceName: interfaceDevice,
                interfaceElementName: interfaceElement
                );
            {
                _part.Name = _generalComponentName + "_" + name;
            };
        }

        private void AddGauge(string name, BaseGauge Part, Point posn, Size size, string interfaceDevice, string interfaceElement, string _componentName) =>
            AddGauge(name, Part, posn, size, interfaceDevice, new string[1] { interfaceElement }, _componentName);
        private new void AddGauge(string name, BaseGauge Part, Point posn, Size size, string interfaceDevice, string[] interfaceElementNames) =>
            AddGauge(name, Part, posn, size, interfaceDevice, interfaceElementNames, "");
        private void AddGauge(string name, BaseGauge Part, Point posn, Size size, string interfaceDevice, string[] interfaceElementNames, string _componentName)
        {
            Part.Name = _componentName + name;
            BaseGauge _part = AddGauge(
                name: name,
                gauge: Part,
                size: size,
                posn: posn,
                interfaceDeviceName: interfaceDevice,
                interfaceElementNames: interfaceElementNames
                );
            {
                _part.Name = _componentName + "_" + name;
            };
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

        //public override void WriteXml(XmlWriter writer)
        //{
        //    base.WriteXml(writer);
        //    if (_IFEI_gauges.GlassReflectionOpacity != IFEI_Gauges.GLASS_REFLECTION_OPACITY_DEFAULT)
        //    {
        //        writer.WriteElementString("GlassReflectionOpacity", GlassReflectionOpacity.ToString(CultureInfo.InvariantCulture));
        //    }
        //}

        //public override void ReadXml(XmlReader reader)
        //{
        //    base.ReadXml(reader);
        //    if (reader.Name.Equals("GlassReflectionOpacity"))
        //    {
        //        GlassReflectionOpacity = double.Parse(reader.ReadElementString("GlassReflectionOpacity"), CultureInfo.InvariantCulture);
        //    }
        //}
    }
}


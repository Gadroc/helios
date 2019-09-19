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
    using System.Windows.Media;
    using System.Windows;
    using System.Xml;
    using System.Globalization;

    [HeliosControl("Helios.AV8B.Cockpit", "Front Cockpit", "AV-8B", typeof(AV8BDeviceRenderer))]
    class FrontCockpit : AV8BDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 1, 1);
        private Rect _scaledScreenRect = SCREEN_RECT;
        private string _interfaceDeviceName = "AV-8B Cockpit";

        private String _font = "Hornet IFEI Mono"; // "Segment7 Standard"; //"Seven Segment";
        private Color _textColor = Color.FromArgb(0xff, 220, 220, 220);
        private Color _backGroundColor = Color.FromArgb(100, 100, 20, 50);
        private string _imageLocation = "{AV-8B}/Images/";
        private bool _useBackGround = false;
        private static readonly Double SCREENRES = 1.33333;
 
        public FrontCockpit()
            : base("Front Cockpit", new Size(2560, 1440))
        {
            AddPanel("EDP Surround", new Point(1673, 55), new Size(624, 292), _imageLocation + "WQHD/Panel/EDP Surround.png", _interfaceDeviceName,"EDP Surround Panel Element");
            AddPart("ODU Device", new ODU_1(), new Point(207 * SCREENRES, 39 * SCREENRES), new Size(456 * SCREENRES, 269 * SCREENRES), _interfaceDeviceName, "ODU Element");
            AddPart("Master Arm Device", new MasterArmPanel(), new Point(156,417), new Size(149,558), _interfaceDeviceName, "Master Arm Element");
            AddPart("MFD Left Device", new Left_MPCD_1(), new Point(226 * SCREENRES, 260 * SCREENRES), new Size(420 * SCREENRES, 474 * SCREENRES), _interfaceDeviceName, "MFD Left Element");
            AddPart("MFD Right Device", new Right_MPCD_1(), new Point(1280 * SCREENRES, 260 * SCREENRES), new Size(420 * SCREENRES, 474 * SCREENRES), _interfaceDeviceName, "MFD Left Element");
            AddPart("EDP Device", new EDP(), new Point(1327 * SCREENRES, 64 * SCREENRES), new Size(304 * SCREENRES, 175 * SCREENRES), _interfaceDeviceName, "EDP Element");
            AddPart("UFC Device", new UFC_1(), new Point(640 * SCREENRES, 20 * SCREENRES), new Size(654 * SCREENRES, 657 * SCREENRES), _interfaceDeviceName, "UFC Element");
            AddPart("Flight Instrument Device", new FlightInstrumentPanel(), new Point(998, 858), new Size(774, 556), _interfaceDeviceName, "Flight Instrument Panel");
            AddPart("SMC Device", new SMC_1(), new Point(206 * SCREENRES, 740 * SCREENRES), new Size(531 * SCREENRES, 203 * SCREENRES), _interfaceDeviceName, "SMC Element");
            AddPart("FQIS Device", new FuelPanel.FQIS(), new Point(1331 * SCREENRES, 760 * SCREENRES), new Size(381 * SCREENRES, 151 * SCREENRES), _interfaceDeviceName, "FQIS Element");
            AddPart("H2O Device", new H2OPanel(), new Point(165, 987), new Size(108, 321), _interfaceDeviceName, "H2O Element");
            AddPart("RWR / ECM Device", new RWRPanel(), new Point(2291, 410), new Size(141, 521), _interfaceDeviceName, "RWR Element");
            AddPart("Threat Indicator Device", new ThreatIndicatorPanel(), new Point(2184,122), new Size(186, 268), _interfaceDeviceName, "Threat Indicator Element");

            //AddPot(
            //    name: "Brightness Control",
            //    posn: new Point(82, 630),
            //    size: new Size(60, 60),
            //    knobImage: "{AV-8B}/Images/Common Knob.png",
            //    initialRotation: 219,
            //    rotationTravel: 291,
            //    minValue: 0,
            //    maxValue: 1,
            //    initialValue: 0,
            //    stepValue: 0.1,
            //    interfaceDeviceName: _interfaceDeviceName,
            //    interfaceElementName: "IFEI Brightness Control Knob",
            //    fromCenter: true
            //    );
            //Size ThreeWayToggleSize = new Size(70, 140);
            //Add3PosnToggle(
            //    name: "Video Record DDI",
            //    posn: new Point(236, 570),
            //    size: ThreeWayToggleSize,
            //    image: "{Helios}/Images/Toggles/orange-round-",
            //    interfaceDevice: _interfaceDeviceName,
            //    interfaceElement: "Video Record Selector Switch HMD/LDDI/RDDI",
            //    fromCenter: false
            //    );

        }

        #region Properties
        //public double GlassReflectionOpacity
        //{
        //    get
        //    {
        //        return _IFEI_gauges.GlassReflectionOpacity;
        //    }
        //    set
        //    {
        //        double oldValue = _IFEI_gauges.GlassReflectionOpacity;
        //        _IFEI_gauges.GlassReflectionOpacity = value;
        //        if (value != oldValue)
        //        {
        //            OnPropertyChanged("GlassReflectionOpacity", oldValue, value, true);
        //        }
        //    }
        //}
        #endregion

        protected override void OnProfileChanged(HeliosProfile oldProfile)
        {
            base.OnProfileChanged(oldProfile);
        }

        public override string BezelImage
        {
            get { return _imageLocation + "WQHD/Panel/AV8B Front Cockpit 2.png"; }
        }

        private void AddTextDisplay(string name, double x, double y, Size size, double baseFontsize, string testDisp,
            string interfaceDevice, string interfaceElement)
        {
            TextDisplay display = AddTextDisplay(
                name: name,
                posn: new Point(x, y),
                size: size,
                font: _font,
                baseFontsize: baseFontsize,
                horizontalAlignment: TextHorizontalAlignment.Right,
                verticalAligment: TextVerticalAlignment.Top,
                testTextDisplay: testDisp,
                textColor: _textColor,
                backgroundColor: _backGroundColor,
                useBackground: _useBackGround,
                interfaceDeviceName: interfaceDevice,
                interfaceElementName: interfaceElement,
                textDisplayDictionary: ""
                );
            display.TextFormat.FontWeight = FontWeights.Heavy;
        }

        private void AddButton(string name, double x, double y, Size size, string interfaceDevice, string interfaceElement)
        {
            Point pos = new Point(x, y);
            AddButton(
                name: name,
                posn: pos,
                size: size,
                image: _imageLocation + "IFEI_" + name + ".png",
                pushedImage: _imageLocation + "IFEI_" + name + "_DN.png",
                buttonText: "",
                interfaceDeviceName: interfaceDevice,
                interfaceElementName: interfaceElement,
                fromCenter: false
                );
        }
        private void Add3PosnToggle(string name, Point posn, Size size, string image, string interfaceDevice, string interfaceElement, bool fromCenter)
        {
            AddThreeWayToggle(
                name: name,
                posn: posn,
                size: size,
                positionOneImage: image + "up.png",
                positionTwoImage: image + "norm.png",
                positionThreeImage: image + "down.png",
                defaultPosition: ThreeWayToggleSwitchPosition.Two,
                defaultType: ThreeWayToggleSwitchType.OnOnOn,
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
                _part.Name = name;
            };
        }

        private new void AddGauge(string name, BaseGauge Part, Point posn, Size size, string interfaceDevice, string interfaceElement)
        {
            BaseGauge _part = AddGauge(
                name: name,
                gauge: Part,
                size: size,
                posn: posn,
                interfaceDeviceName: interfaceDevice,
                interfaceElementName: interfaceElement
                );
            {
                _part.Name = name;
            };
        }

        private void AddPanel(string name, Point posn, Size size, string background, string interfaceDevice, string interfaceElement)
        {  HeliosPanel _panel =  AddPanel(
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

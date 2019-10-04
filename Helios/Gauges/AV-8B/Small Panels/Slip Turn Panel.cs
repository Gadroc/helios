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

    [HeliosControl("Helios.AV8B.Panels.SlipTurn", "Slip Turn Panel", "AV-8B Gauges", typeof(AV8BDeviceRenderer))]
    class SlipTurnPanel : AV8BDevice
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 1, 1);
        private Rect _scaledScreenRect = SCREEN_RECT;
        private string _imageLocation = "{AV-8B}/Images/";
        private string _interfaceDeviceName = "Flight Instruments";
        private static string _generalComponentName = "Flight Instruments";
        private double _GlassReflectionOpacity = 0;
        public const double GLASS_REFLECTION_OPACITY_DEFAULT = 1.0;

        public SlipTurnPanel()
            : base(_generalComponentName, new Size(225, 114))
        {
            AddGauge("Slip/Turn Gauge", new SlipBall.SlipTurn(), new Point(0, 0), new Size(225, 114), _interfaceDeviceName, new string[3] { "Slip Ball", "Turn Indicator", "Slip/Turn Flag" }, _generalComponentName);
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
        protected override void OnProfileChanged(HeliosProfile oldProfile)
        {
            base.OnProfileChanged(oldProfile);
        }

        private void AddGauge(string name, BaseGauge Part, Point posn, Size size, string interfaceDevice, string[] interfaceElementNames ,string _componentName)
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


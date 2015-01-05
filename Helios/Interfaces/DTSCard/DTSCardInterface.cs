//  Copyright 2015 Craig Courtney
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

namespace GadrocsWorkshop.Helios.Interfaces.DTSCard
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;

    [HeliosInterface("Helios.DTS.DTSCard", "DTS Card", null, typeof(DTSCardFactory), AutoAdd = false)]
    public class DTSCardInterface : HeliosInterface
    {
        private string _serialNumber;
        private HeliosValue _angle;

        public DTSCardInterface() : base("DTS Card")
        {
            _angle = new HeliosValue(this, new BindingValue(0), "", "angle", "Current angle of the synchro output.", "Angle in degrees.", BindingValueUnits.Degrees);
            _angle.Execute += new HeliosActionHandler(Angle_Execute);
            Values.Add(_angle);
            Actions.Add(_angle);
        }

        public DTSCardInterface(String serialNumber) : this()
        {
            SerialNumber = serialNumber;
        }

        #region Properties

        /// <summary>
        /// Sets and returns the serial number of the DTS Card this interface controls.
        /// </summary>
        public String SerialNumber 
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                _serialNumber = value;
                Name = "DTS Card (" + value + ")";
            }
        }

        public Double Angle
        {
            get
            {
                return _angle.Value.DoubleValue;
            }
            set
            {
                if (!_angle.Value.DoubleValue.Equals(value))
                {
                    Double oldValue = _angle.Value.DoubleValue;
                    _angle.SetValue(new BindingValue(value), BypassTriggers);
                    OnPropertyChanged("Angle", oldValue, value, false);
                }
            }
        }

        #endregion

        void Angle_Execute(object action, HeliosActionEventArgs e)
        {
            BeginTriggerBypass(e.BypassCascadingTriggers);
            Angle = e.Value.DoubleValue;
            EndTriggerBypass(e.BypassCascadingTriggers);
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("SerialNumber", SerialNumber);
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            SerialNumber = reader.ReadElementString("SerialNumber");
        }
    }
}

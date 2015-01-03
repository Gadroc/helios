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

namespace GadrocsWorkshop.Helios.Interfaces.Phidgets
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using global::Phidgets;
    using global::Phidgets.Events;
    using System;
    using System.Globalization;

    [HeliosInterface("Helios.Phidgets.LedBoard", "Phidgets Advanced LED Controller", typeof(PhidgetLedBoardEditor), Factory = typeof(PhidgetsInterfaceFactory))]
    public class PhidgetLEDBoard : PhidgetInterface
    {
        private Int32 _serialNumber = 0;
        private LED _ledBoard;
        private int _voltage = (int)LED.LEDVoltage.VOLTAGE_2_75V;
        private int _currentLimit = (int)LED.LEDCurrentLimit.CURRENT_LIMIT_20mA;

        private LedGroupCollection _ledGroups;

        public PhidgetLEDBoard()
            : base("Phidgets LED Board")
        {
            _ledGroups = new LedGroupCollection();
            _ledGroups.CollectionChanged += LedGroups_CollectionChanged;
        }

        public PhidgetLEDBoard(Int32 serialNumber) : this()
        {
            SerialNumber = serialNumber; 
        }

        #region Properties

        public LedGroupCollection LedGroups
        {
            get { return _ledGroups; }
        }

        public override Int32 SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                if (!_serialNumber.Equals(value))
                {
                    Int32 oldValue = _serialNumber;

                    if (_ledBoard != null)
                    {
                        Detach();
                    }

                    _serialNumber = value;

                    OnPropertyChanged("SerialNumber", oldValue, value, false);

                    Name = "Phidgets LED Board (" + _serialNumber + ")";
                }
            }
        }

        public int Voltage
        {
            get
            {
                return _voltage;
            }
            set
            {
                
                if (!_voltage.Equals(value))
                {
                    int oldValue = _voltage;
                    _voltage = value;
                    OnPropertyChanged("Voltage", oldValue, value, true);
                    if (_ledBoard != null && _ledBoard.Attached)
                    {
                        _ledBoard.Voltage = (LED.LEDVoltage)Voltage;
                    }
                }
            }
        }

        public int CurrentLimit
        {
            get
            {
                return _currentLimit;
            }
            set
            {
                if (!_currentLimit.Equals(value))
                {
                    int oldValue = _currentLimit;
                    _currentLimit = value;
                    OnPropertyChanged("CurrentLimit", oldValue, value, true);
                    if (_ledBoard != null && _ledBoard.Attached)
                    {
                        _ledBoard.CurrentLimit = (LED.LEDCurrentLimit)CurrentLimit;
                    }
                }
            }
        }


        #endregion

        void LedGroups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (LEDGroup group in e.OldItems)
                {
                    if (e.NewItems == null || !e.NewItems.Contains(group))
                    {
                        group.TogglePowerAction.Execute -= TogglePowerAction_Execute;
                        Actions.Remove(group.TogglePowerAction);

                        group.SetBrightnessAction.Execute -= SetBrightnessAction_Execute;
                        Actions.Remove(group.SetBrightnessAction);

                        group.SetPowerAction.Execute -= SetPowerAction_Execute;
                        Actions.Remove(group.SetPowerAction);
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (LEDGroup group in e.NewItems)
                {
                    if (!Actions.Contains(group.TogglePowerAction))
                    {
                        group.TogglePowerAction.Execute += TogglePowerAction_Execute;
                        Actions.Add(group.TogglePowerAction);
                    }
                    if (!Actions.Contains(group.SetBrightnessAction))
                    {
                        group.SetBrightnessAction.Execute += SetBrightnessAction_Execute;
                        Actions.Add(group.SetBrightnessAction);
                    }
                    if (!Actions.Contains(group.SetPowerAction))
                    {
                        group.SetPowerAction.Execute += SetPowerAction_Execute;
                        Actions.Add(group.SetPowerAction);
                    }
                }
            }
        }

        public void SetLedPower(int led, int level)
        {
            if (_ledBoard != null && _ledBoard.Attached)
            {
                _ledBoard.leds[led] = level;
            }
        }

        public void SetGroupPower(LEDGroup group, bool power)
        {
            if (power)
            {
                SetGroupLevel(group, group.Level);
            }
            else
            {
                SetGroupLevel(group, 0);
            }
        }

        public void SetGroupLevel(LEDGroup group, int level)
        {
            if (_ledBoard != null && _ledBoard.Attached)
            {
                foreach (int i in group.Leds)
                {
                    _ledBoard.leds[i] = level;
                }
            }
        }

        private void TogglePowerAction_Execute(object action, HeliosActionEventArgs e)
        {
            IBindingAction myAction = action as IBindingAction;
            if (myAction != null)
            {
                LEDGroup group = myAction.Context as LEDGroup;
                if (group != null)
                {
                    group.Power = !group.Power;
                    SetGroupPower(group, group.Power);
                }
            }
        }

        private void SetPowerAction_Execute(object action, HeliosActionEventArgs e)
        {
            IBindingAction myAction = action as IBindingAction;
            if (myAction != null)
            {
                LEDGroup group = myAction.Context as LEDGroup;
                if (group != null)
                {
                    group.Power = e.Value.BoolValue;
                    SetGroupPower(group, group.Power);
                }
            }
        }

        private void SetBrightnessAction_Execute(object action, HeliosActionEventArgs e)
        {
            IBindingAction myAction = action as IBindingAction;
            if (myAction != null)
            {
                LEDGroup group = myAction.Context as LEDGroup;
                if (group != null)
                {
                    int newLevel = (int)Math.Truncate(e.Value.DoubleValue);
                    if (newLevel != group.Level)
                    {
                        group.Level = newLevel;
                        group.Power = true;
                        SetGroupLevel(group, group.Level);
                    }
                }
            }
        }

        protected override void OnProfileChanged(HeliosProfile oldProfile)
        {
            base.OnProfileChanged(oldProfile);

            if (oldProfile != null)
            {
                oldProfile.ProfileStarted -= Profile_ProfileStarted;
                oldProfile.ProfileStopped -= Profile_ProfileStopped;
            }

            if (Profile != null)
            {
                Profile.ProfileStarted += Profile_ProfileStarted;
                Profile.ProfileStopped += Profile_ProfileStopped;
            }
        }

        private void Profile_ProfileStarted(object sender, EventArgs e)
        {
            Attach();
        }

        private void Profile_ProfileStopped(object sender, EventArgs e)
        {
            Detach();
        }

        public void Attach()
        {
            if (_serialNumber > 0 && _ledBoard == null)
            {
                _ledBoard = new LED();
                _ledBoard.Attach += LedBoard_Attach;
                _ledBoard.open(_serialNumber);
            }
        }

        private void LedBoard_Attach(object sender, AttachEventArgs e)
        {
            _ledBoard.Voltage = (LED.LEDVoltage)Voltage;
            _ledBoard.CurrentLimit = (LED.LEDCurrentLimit)CurrentLimit;
        }

        public void Detach()
        {
            if (_ledBoard != null && _ledBoard.Attached)
            {
                try
                {
                    for (int i = 0; i < _ledBoard.leds.Count; i++)
                    {
                        _ledBoard.leds[i] = 0;
                    }

                    _ledBoard.close();
                    _ledBoard = null;
                }
                catch (PhidgetException e)
                {
                    ConfigManager.LogManager.LogError("Error closing led board", e);
                }
            }
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            SerialNumber = Int32.Parse(reader.ReadElementString("SerialNumber"), CultureInfo.InvariantCulture);
            Voltage = int.Parse(reader.ReadElementString("Voltage"), CultureInfo.InvariantCulture);
            CurrentLimit = int.Parse(reader.ReadElementString("CurrentLimit"), CultureInfo.InvariantCulture);
            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("Groups");
                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("Group");
                    LEDGroup group = new LEDGroup(this, reader.ReadElementString("Name"));
                    group.DefaultLevel = int.Parse(reader.ReadElementString("DefaultLevel"), CultureInfo.InvariantCulture);

                    string leds = reader.ReadElementString("Leds");
                    if (!string.IsNullOrWhiteSpace(leds))
                    {
                        string[] ledSplit = leds.Split(',');
                        foreach (string led in ledSplit)
                        {
                            group.Leds.Add(int.Parse(led, CultureInfo.InvariantCulture));
                        }
                    }
                    LedGroups.Add(group);

                    reader.ReadEndElement();
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read();
            }
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("SerialNumber", SerialNumber.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Voltage", Voltage.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("CurrentLimit", CurrentLimit.ToString(CultureInfo.InvariantCulture));
            writer.WriteStartElement("Groups");
            foreach (LEDGroup group in LedGroups)
            {
                writer.WriteStartElement("Group");
                writer.WriteElementString("Name", group.Name);
                writer.WriteElementString("DefaultLevel", group.DefaultLevel.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Leds", GetLedList(group));
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        private string GetLedList(LEDGroup group)
        {
            string list = "";
            bool first = true;

            foreach (int i in group.Leds)
            {
                if (!first)
                {
                    list += ",";
                }
                else
                {
                    first = false;
                }
                list += i.ToString(CultureInfo.InvariantCulture);
            }

            return list;
        }
    }
}

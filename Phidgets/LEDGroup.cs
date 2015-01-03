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
    using System;
    using System.Collections.Generic;

    public class LEDGroup : NotificationObject
    {
        private List<int> _leds = new List<int>();

        private string _name;
        private int _defaultLevel = 100;    
        private int _level = 100;
        private bool _on = false;

        private HeliosAction _togglePowerAction;
        private HeliosAction _setBrightnessAction;
        private HeliosAction _setPowerAction;

        public LEDGroup(PhidgetLEDBoard parentBoard, string name)
        {
            _name = name;
            _togglePowerAction = new HeliosAction(parentBoard, name, "Power", "Toggle", "Toggles the power to this led group.  Current brightness setting will be retained.");
            _togglePowerAction.Context = this;

            _setPowerAction = new HeliosAction(parentBoard, name, "Power", "Set", "Sets whether this led group is powered on or not. Current brightness setting will be retained.", "True if the leds should be turned on, false if the leds should be turned off.", BindingValueUnits.Boolean);
            _setPowerAction.Context = this;

            _setBrightnessAction = new HeliosAction(parentBoard, name, "Brightness", "Set", "Sets the brightness of the leds in this group.", "0 = off regardless of power, 100 = full brightness", BindingValueUnits.Numeric);
            _setBrightnessAction.Context = this;
        }

        #region Properties

        public bool Power
        {
            get
            {
                return _on;
            }
            set
            {
                if (!_on.Equals(value))
                {
                    bool oldValue = _on;
                    _on = value;
                    OnPropertyChanged("Power", oldValue, value, false);
                }
            }
        }

        public List<int> Leds
        {
            get { return _leds; }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if ((_name == null && value != null)
                    || (_name != null && !_name.Equals(value)))
                {
                    string oldValue = _name;
                    _name = value;
                    OnPropertyChanged("Name", oldValue, value, true);
                    _togglePowerAction.Device = _name;
                    _setPowerAction.Device = _name;
                    _setBrightnessAction.Device = _name;
                }
            }
        }

        public int DefaultLevel
        {
            get
            {
                return _defaultLevel;
            }
            set
            {
                int newValue = Math.Min(100, Math.Max(value, 0));
                if (!_defaultLevel.Equals(newValue))
                {
                    int oldValue = _defaultLevel;
                    _defaultLevel = newValue;
                    OnPropertyChanged("DefaultLevel", oldValue, newValue, true);
                }
            }
        }

        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                int newValue = Math.Min(100, Math.Max(value, 0));
                if (!_level.Equals(newValue))
                {
                    int oldValue = _level;
                    _level = newValue;
                    OnPropertyChanged("Level", oldValue, newValue, false);
                }
            }
        }

        public HeliosAction SetPowerAction
        {
            get
            {
                return _setPowerAction;
            }
        }


        public HeliosAction SetBrightnessAction
        {
            get
            {
                return _setBrightnessAction;
            }
        }

        public HeliosAction TogglePowerAction
        {
            get
            {
                return _togglePowerAction;
            }
        }

        #endregion
    }
}

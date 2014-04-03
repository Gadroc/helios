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

namespace GadrocsWorkshop.Helios
{
    using System;

    /// <summary>
    /// Abstract base class for all triggers.  Trigger represent changes in there source which
    /// can be used to trigger other actions.
    /// </summary>
    public class HeliosTrigger : NotificationObject, IBindingTrigger
    {
        private string _id;
        private string _device;
        private string _name;
        private string _description;
        private string _verb;
        private string _valueDescription;
        private BindingValueUnit _unit;
        private string _bindingDescription;

        private WeakReference _source = new WeakReference(null);
        private WeakReference _context = new WeakReference(null);

        public HeliosTrigger(HeliosObject source, string device, string name, string verb, string description)
            : this(source, device, name, verb, description, "", BindingValueUnits.NoValue)
        {
        }

        public HeliosTrigger(HeliosObject source, string device, string name, string verb, string description, string valueDescription, BindingValueUnit unit)
        {
            _device = device;
            _name = name;
            _verb = verb;
            _description = description;
            _source = new WeakReference(source);
            _valueDescription = valueDescription;
            _unit = unit;
            UpdateId();

            TriggerBindingDescription = "when" + (Device.Length > 0 ? " " + _device : "") + (_name.Length > 0 ? " " + _name + " on" : "") + " " + Source.Name +  " " + _verb;
        }

        private void UpdateId()
        {
            _id = "";
            if (_device != null && _device.Length > 0)
            {
                _id += _device + ".";
            }
            if (_name != null && _name.Length > 0)
            {
                _id += _name + ".";
            }
            _id += _verb;
        }

        #region IBindingElement Members

        public object Context { get { return _context.Target; } set { _context = new WeakReference(value); } }

        public HeliosObject Owner { get { return _source.Target as HeliosObject; } }

        public string Device { get { return _device; } set { _device = value; UpdateId(); } }

        public string Name
        {
            get { return _name; }

            set
            {
                string oldValue = _name;
                _name = value;
                OnPropertyChanged("Name", oldValue, value, false);
                UpdateId();
            }
        }
       

        public BindingValueUnit Unit { get { return _unit; } }

        #endregion

        #region IBindingTrigger Members

        /// <summary>
        /// Event which is fired when ever the trigger is activated.
        /// </summary>
        public event HeliosTriggerHandler TriggerFired;

        public string TriggerID
        {
            get
            {
                return _id;
            }
        }

        public string TriggerName
        {
            get
            {
                return _name + " " + _verb;
            }
        }

        /// <summary>
        /// Name used to identify this binding trigger. (Ex: Button 1 Pressed)
        /// </summary>
        public string TriggerVerb
        {
            get
            {
                return _verb;
            }
        }

        /// <summary>
        /// Gets the description of when this trigger is fired.
        /// </summary>
        public string TriggerDescription
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// Gets the description of the contents of the supplied value when this trigger is fired.
        /// </summary>
        public string TriggerValueDescription
        {
            get
            {
                return _valueDescription;
            }
        }

        /// <summary>
        /// Source object which fires this trigger.
        /// </summary>
        public HeliosObject Source
        {
            get
            {
                return _source.Target as HeliosObject;
            }
        }

        public bool TriggerSuppliesValue
        {
            get
            {
                return !((_unit == null) || (_unit.Equals(BindingValueUnits.NoValue)));
            }
        }

        public string TriggerBindingDescription
        {
            get
            {
                return _bindingDescription;
            }
            set
            {
                if ((_bindingDescription == null && value != null)
                    || (_bindingDescription != null && !_bindingDescription.Equals(value)))
                {
                    string oldValue = _bindingDescription;
                    _bindingDescription = value;
                }
            }
        }


        #endregion

        public void FireTrigger(BindingValue value)
        {
            HeliosTriggerEventArgs args = new HeliosTriggerEventArgs(value);
            HeliosTriggerHandler handler = TriggerFired;
            if (handler != null)
            {
                handler.Invoke(this, args);
            }
        }

    }
}

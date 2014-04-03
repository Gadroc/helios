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

    using GadrocsWorkshop.Helios.Windows.Controls;

    public class HeliosValue : NotificationObject, IBindingAction, IBindingTrigger, IHeliosValue
    {
        private string _device;
        private string _name;
        private string _description;
        private string _valueDescription;
        private BindingValueUnit _unit;

        private WeakReference _owner = new WeakReference(null);
        private WeakReference _context = new WeakReference(null);

        private BindingValue _value;

        private string _id;
        private string _actionId;
        private string _triggerId;

        private Type _editor = typeof(TextStaticEditor);

        public HeliosValue(HeliosObject owner, BindingValue initialValue, string device, string name, string description, string valueDescription, BindingValueUnit unit)
        {
            _device = device;
            _name = name;
            _description = description;
            _valueDescription = valueDescription;
            _owner = new WeakReference(owner);
            _value = initialValue;
            _unit = unit;

            UpdateId();
        }

        private void UpdateId()
        {
            _id = "";
            _actionId = "";
            _triggerId = "";
            if (_device != null && _device.Length > 0)
            {
                _id += _device + ".";
                _actionId += _device + ".";
                _triggerId += _device + ".";
            }
            _triggerId += _name + ".changed";
            _actionId += "set." + _name;
            _id += _name;
        }

        /// <summary>
        /// Fired when a set action is called on this value object.
        /// </summary>
        public event HeliosActionHandler Execute;

        protected void OnFireTrigger(BindingValue value)
        {
            HeliosTriggerEventArgs args = new HeliosTriggerEventArgs(value);
            HeliosTriggerHandler handler = TriggerFired;
            if (handler != null)
            {
                handler.Invoke(this, args);
            }
        }

        /// <summary>
        /// Sets a new value for this helios value object
        /// </summary>
        /// <param name="value">Value to be sent to bindings.</param>
        /// <param name="bypassCascadingTriggers">True if bindings should not trigger further triggers.</param>
        public void SetValue(BindingValue value, bool bypassCascadingTriggers)
        {
            if ((_value == null && value != null)
                || (_value != null && !_value.Equals(value)))
            {
                _value = value;
                if (!bypassCascadingTriggers)
                {
                    OnFireTrigger(value);
                }
            }
        }

        #region IHeliosValue Members

        public string ValueID
        {
            get
            {
                return _id;
            }
        }

        public BindingValue Value
        {
            get
            {
                return _value;
            }
        }

        public string ValueDescription
        {
            get
            {
                return _valueDescription;
            }
            set
            {
                if ((_valueDescription == null && value != null)
                    || (_valueDescription != null && !_valueDescription.Equals(value)))
                {
                    string oldValue = _valueDescription;
                    _valueDescription = value;
                    OnPropertyChanged("ValueDescription", oldValue, value, false);
                }
            }
        }
    
        #endregion

        #region IBindingElement Members

        public object Context { get { return _context.Target; } set { _context = new WeakReference(value); } }

        public HeliosObject Owner { get { return _owner.Target as HeliosObject; } }

        public string Device
        {
            get
            {
                return _device;
            }
            set
            {
                _device = value;
                UpdateId();
            }
        }

        public string Name 
        { 
            get 
            { 
                return _name; 
            }
 
            set 
            {
                string oldValue = _name;
                _name = value;
                OnPropertyChanged("Name", oldValue, value, false);
            }
        }

        public BindingValueUnit Unit { get { return _unit; } }

        #endregion

        #region IBindingAction Members

        public string ActionID
        {
            get
            {
                return _actionId;
            }
        }

        public string ActionName
        {
            get
            {
                return "set " + _name;
            }
        }

        public string ActionVerb
        {
            get
            {
                return "set";
            }
        }

        public HeliosObject Target
        {
            get
            {
                return _owner.Target as HeliosObject;
            }
        }

        public string ActionDescription
        {
            get
            {
                return _description;
            }
        }

        public bool ActionRequiresValue
        {
            get
            {
                return true;
            }
        }

        public string ActionValueDescription
        {
            get
            {
                return _valueDescription;
            }
        }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="value">Value to be processed by this action.</param>
        /// <param name="bypassCascadingTriggers">If true this action will not fire additional triggers.</param>
        public void ExecuteAction(BindingValue value, bool bypassCascadingTriggers)
        {
            HeliosActionEventArgs args = new HeliosActionEventArgs(value, bypassCascadingTriggers);
            HeliosActionHandler handler = Execute;
            if (handler != null)
            {
                handler.Invoke(this, args);
            }
        }

        public Type ValueEditorType
        {
            get { return _editor; }
            set { _editor = value; }
        }

        public string ActionBindingDescription
        {
            get { return "set " + (Device.Length > 0 ? _device + " " : "") + _name + " on " + Owner.Name +  " to  %value%"; }
        }

        public string ActionInputBindingDescription
        {
            get { return "to %value%"; }
        }

        #endregion

        #region IBindingTrigger Members

        public event HeliosTriggerHandler TriggerFired;

        public string TriggerID
        {
            get
            {
                return _triggerId;
            }
        }

        public string TriggerName
        {
            get
            {
                return _name + " changed";
            }
        }

        public string TriggerVerb
        {
            get
            {
                return "changed";
            }
        }

        public string TriggerDescription
        {
            get
            {
                return _description;
            }
        }

        public string TriggerValueDescription
        {
            get
            {
                return _valueDescription;
            }
        }

        public HeliosObject Source
        {
            get
            {
                return _owner.Target as HeliosObject;
            }
        }

        public bool TriggerSuppliesValue
        {
            get
            {
                return true;
            }
        }

        public string TriggerBindingDescription
        {
            get { return "when" + (Device.Length > 0 ? " " + _device + " " : " ") + _name + " on " + Owner.Name +  " changes";  }
        }

        #endregion
    }
}

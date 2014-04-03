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
    /// Delegate to handle action invocation.
    /// </summary>
    /// <param name="source">Action which has been invoked.</param>
    /// <param name="value">Value passed to the action.</param>
    /// <param name="bypassCascadingTriggers">True if cacsading triggers shoudl be surpressed.</param>
    public delegate void HeliosActionHandler(object action, HeliosActionEventArgs e);

    public class HeliosAction : NotificationObject, IBindingAction
    {
        private string _id;
        private string _device;
        private string _name;
        private string _verb;
        private string _description;
        private string _valueDescription;
        private BindingValueUnit _unit;
        private string _bindingDescription = "";
        private string _inputBindingDescription = "";

        private WeakReference _target = new WeakReference(null);
        private WeakReference _context = new WeakReference(null);

        private Type _editor = null;

        public HeliosAction(HeliosObject target, string device, string name, string verb, string description)
            : this(target, device, name, verb, description, "", BindingValueUnits.NoValue)
        {
        }

        public HeliosAction(HeliosObject target, string device, string name, string verb, string description, string valueDescription, BindingValueUnit unit)
        {
            _device = device;
            _target = new WeakReference(target);
            _name = name;
            _verb = verb;
            _description = description;
            _valueDescription = valueDescription;
            _unit = unit;

            UpdateId();

            ActionBindingDescription = _verb + (Device.Length > 0 ? " " + _device : "") + (_name.Length > 0 ? " " + _name + " on" : "") + " " + Target.Name + ( ActionRequiresValue ? " to %value%" : "");
            if (ActionRequiresValue)
            {
                ActionInputBindingDescription = "to %value%";
            }
         }

        private void UpdateId()
        {
            _id = "";
            if (_device != null && _device.Length > 0)
            {
                _id += _device + ".";
            }
            _id += _verb;
            if (_name != null && _name.Length > 0)
            {
                _id += "." + _name;
            }
        }

        public event HeliosActionHandler Execute;

        #region IBindingElement Members

        public object Context { get { return _context.Target; } set { _context = new WeakReference(value); } }

        public HeliosObject Owner { get { return _target.Target as HeliosObject; } }

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

        #region IBindingAction Members

        public string ActionID { get { return _id; } }

        public string ActionName { get { return _verb + " " + _name; } }

        /// <summary>
        /// Name used to identify this binding action. (Ex: Press Button 1)
        /// </summary>
        public string ActionVerb { get { return _verb; } }

        /// <summary>
        /// Target object which this action acts on.
        /// </summary>
        public HeliosObject Target { get { return _target.Target as HeliosObject; } }

        /// <summary>
        /// Short description of what this action does.
        /// </summary>
        public string ActionDescription { get { return _description; } }

        /// <summary>
        /// Description of the valid values that this action accepts.
        /// </summary>
        public string ActionValueDescription { get { return _valueDescription; } }

        public string ActionBindingDescription
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

        public string ActionInputBindingDescription
        {
            get
            {
                return _inputBindingDescription;
            }
            set
            {
                if ((_inputBindingDescription == null && value != null)
                    || (_inputBindingDescription != null && !_inputBindingDescription.Equals(value)))
                {
                    string oldValue = _inputBindingDescription;
                    _inputBindingDescription = value;
                }
            }
        }

        public bool ActionRequiresValue
        {
            get
            {
                return !((_unit == null) || (_unit.Equals(BindingValueUnits.NoValue)));
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

        #endregion
    }
}

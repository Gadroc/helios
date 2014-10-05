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

namespace GadrocsWorkshop.Helios.Interfaces.DirectX
{
    using SharpDX.DirectInput;
    using System;
    using System.Collections.Generic;

    class DirectXControllerButton : DirectXControllerFunction
    {
        private string _name;
        private int _buttonNumber;
        private bool _lastPollValue;
        private HeliosValue _value;
        private HeliosTrigger _press;
        private HeliosTrigger _release;

        private List<IBindingTrigger> _triggers = new List<IBindingTrigger>();

        public DirectXControllerButton(DirectXControllerInterface controllerInterface, int buttonNumber, JoystickState initialState)
        {
            _buttonNumber = buttonNumber;
            _name = "Button " + (_buttonNumber + 1);
            _value = new HeliosValue(controllerInterface, new BindingValue(GetValue(initialState)), "", _name, "Current state for " + _name + ".", "True if pressed, false other wise.", BindingValueUnits.Boolean);

            _press = new HeliosTrigger(controllerInterface, "", _name, "pressed", "Fires when " + _name + " is pressed.", "Always returns true.", BindingValueUnits.Boolean);
            _release = new HeliosTrigger(controllerInterface, "", _name, "released", "Fires when " + _name + " is released.", "Always returns false.", BindingValueUnits.Boolean);

            _triggers.Add(_press);
            _triggers.Add(_release);

            _lastPollValue = GetValue(initialState);
        }

        public override string FunctionType
        {
            get { return "Button"; }
        }

        public override string Name
        {
            get
            {
                return _name;
            }
        }

        public override int ObjectNumber
        {
            get
            {
                return _buttonNumber;
            }
        }

        public override HeliosValue Value
        {
            get
            {
                return _value;
            }
        }

        public override IList<IBindingTrigger> Triggers
        {
            get
            {
                return _triggers;
            }
        }

        public override void PollValue(JoystickState state)
        {
            bool newValue = GetValue(state);

            if (_lastPollValue != newValue)
            {
                BindingValue bindValue = new BindingValue(newValue);
                _lastPollValue = newValue;

                _value.SetValue(bindValue, false);

                if (newValue)
                {
                    _press.FireTrigger(bindValue);
                }
                else
                {
                    _release.FireTrigger(bindValue);
                }
            }
        }

        internal bool GetValue(JoystickState state)
        {
            return state.Buttons[_buttonNumber];
        }
    }
}

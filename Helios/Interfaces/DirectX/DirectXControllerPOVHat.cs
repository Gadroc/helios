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
    using GadrocsWorkshop.Helios.Windows.Controls;
    using SharpDX.DirectInput;
    using System;
    using System.Collections.Generic;

    public class DirectXControllerPOVHat : DirectXControllerFunction
    {
        private string _name;
        private int _povNumber;

        private POVDirection _lastPollValue;
        private HeliosValue _value;

        private HeliosTrigger _upEnter;
        private HeliosTrigger _upExit;

        private HeliosTrigger _upRightEnter;
        private HeliosTrigger _upRightExit;

        private HeliosTrigger _upLeftEnter;
        private HeliosTrigger _upLeftExit;

        private HeliosTrigger _downEnter;
        private HeliosTrigger _downExit;

        private HeliosTrigger _downRightEnter;
        private HeliosTrigger _downRightExit;

        private HeliosTrigger _downLeftEnter;
        private HeliosTrigger _downLeftExit;

        private HeliosTrigger _leftEnter;
        private HeliosTrigger _leftExit;

        private HeliosTrigger _rightEnter;
        private HeliosTrigger _rightExit;

        private HeliosTrigger _centerEnter;
        private HeliosTrigger _centerExit;

        private List<IBindingTrigger> _triggers = new List<IBindingTrigger>();

        public DirectXControllerPOVHat(DirectXControllerInterface controllerInterface, int povNumber)
        {
            _lastPollValue = POVDirection.Center;
            _povNumber = povNumber;
            _name = "POV " + (_povNumber + 1);

            _value = new HeliosValue(controllerInterface, new BindingValue(-1d), "", _name, "Current state for " + _name + ".", "-1 = Centered, 0 = Up, 45 = Up and Right, 90 = Right, 135 = Down and Right, 180 = Down, 225 = Down and Left, 270 = Left, 315 = Up and Left", BindingValueUnits.Boolean);

            _centerEnter = new HeliosTrigger(controllerInterface, _name, "center", "entered", "Fires when hat enters the center position.");
            _triggers.Add(_centerEnter);
            _centerExit = new HeliosTrigger(controllerInterface, _name, "center", "exited", "Fires when the hat leaves the center position.");
            _triggers.Add(_centerExit);

            _rightEnter = new HeliosTrigger(controllerInterface, _name, "right", "entered", "Fires when hat enters the right position.");
            _triggers.Add(_rightEnter);
            _rightExit = new HeliosTrigger(controllerInterface, _name, "right", "exited", "Fires when the hat leaves the right position.");
            _triggers.Add(_rightExit);

            _leftEnter = new HeliosTrigger(controllerInterface, _name, "left", "entered", "Fires when hat enters the left position.");
            _triggers.Add(_leftEnter);
            _leftExit = new HeliosTrigger(controllerInterface, _name, "left", "exited", "Fires when the hat leaves the left position.");
            _triggers.Add(_leftExit);


            _upEnter = new HeliosTrigger(controllerInterface, _name, "up", "entered", "Fires when hat enters the up position.");
            _triggers.Add(_upEnter);
            _upExit = new HeliosTrigger(controllerInterface, _name, "up", "exited", "Fires when the hat leaves the up position.");
            _triggers.Add(_upExit);
            _upLeftEnter = new HeliosTrigger(controllerInterface, _name, "up and left", "entered", "Fires when hat enters the upper left position.");
            _triggers.Add(_upLeftEnter);
            _upLeftExit = new HeliosTrigger(controllerInterface, _name, "up and left", "exited", "Fires when the hat leaves the upper left position.");
            _triggers.Add(_upLeftExit);
            _upRightEnter = new HeliosTrigger(controllerInterface, _name, "up and right", "entered", "Fires when hat enters the upper right position.");
            _triggers.Add(_upRightEnter);
            _upRightExit = new HeliosTrigger(controllerInterface, _name, "up and right", "exited", "Fires when the hat leaves the upper right position.");
            _triggers.Add(_upRightExit);

            _downEnter = new HeliosTrigger(controllerInterface, _name, "down", "entered", "Fires when hat enters the down position.");
            _triggers.Add(_downEnter);
            _downExit = new HeliosTrigger(controllerInterface, _name, "down", "exited", "Fires when the hat leaves the down position.");
            _triggers.Add(_downExit);
            _downLeftEnter = new HeliosTrigger(controllerInterface, _name, "down and left", "entered", "Fires when hat enters the downper left position.");
            _triggers.Add(_downLeftEnter);
            _downLeftExit = new HeliosTrigger(controllerInterface, _name, "down and left", "exited", "Fires when the hat leaves the downper left position.");
            _triggers.Add(_downLeftExit);
            _downRightEnter = new HeliosTrigger(controllerInterface, _name, "down and right", "entered", "Fires when hat enters the downper right position.");
            _triggers.Add(_downRightEnter);
            _downRightExit = new HeliosTrigger(controllerInterface, _name, "down and right", "exited", "Fires when the hat leaves the downper right position.");
            _triggers.Add(_downRightExit);
        }

        public DirectXControllerPOVHat(DirectXControllerInterface controllerInterface, int povNumber, JoystickState initialState)
            : this(controllerInterface, povNumber)
        {
            _lastPollValue = GetValue(initialState);
            _value.SetValue(new BindingValue((double)_lastPollValue), true);
        }

        public override string FunctionType
        {
            get { return "POV"; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override int ObjectNumber
        {
            get { return _povNumber; }
        }

        public override HeliosValue Value
        {
            get { return _value; }
        }

        public override IList<IBindingTrigger> Triggers
        {
            get { return _triggers; }
        }

        public override void PollValue(JoystickState state)
        {
            POVDirection newValue = GetValue(state);

            if (_lastPollValue != newValue)
            {
                BindingValue bindValue = new BindingValue((double)newValue);
                switch (_lastPollValue)
                {
                    case POVDirection.Up:
                        _upExit.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.UpAndRight:
                        _upRightExit.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.Right:
                        _rightExit.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.DownAndRight:
                        _downRightEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.Down:
                        _downExit.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.DownAndLeft:
                        _downLeftExit.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.Left:
                        _leftEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.UpAndLeft:
                        _upLeftExit.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.Center:
                        _centerExit.FireTrigger(BindingValue.Empty);
                        break;
                }

                switch (newValue)
                {
                    case POVDirection.Up:
                        _upEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.UpAndRight:
                        _upRightEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.Right:
                        _rightEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.DownAndRight:
                        _downRightEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.Down:
                        _downEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.DownAndLeft:
                        _downLeftEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.Left:
                        _leftEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.UpAndLeft:
                        _upLeftEnter.FireTrigger(BindingValue.Empty);
                        break;
                    case POVDirection.Center:
                        _centerEnter.FireTrigger(BindingValue.Empty);
                        break;
                }

                _value.SetValue(bindValue, false);
                _lastPollValue = newValue;
            }
        }

        internal POVDirection GetValue(JoystickState state)
        {
            int pov = state.PointOfViewControllers[_povNumber];
            if (pov > 100)
            {
                pov = pov / 100;
            }
            return (POVDirection)pov;
        }
    }
}

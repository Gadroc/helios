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

    class DirectXControllerAxis : DirectXControllerFunction
    {
        public enum AxisType
        {
            X,
            Y,
            Z,
            Rx,
            Ry,
            Rz,
            Slider
        }

        private AxisType _axisType;
        private int _axisNumber;

        private HeliosValue _value;
        private List<IBindingTrigger> _triggers = new List<IBindingTrigger>();

        public DirectXControllerAxis(DirectXControllerInterface controllerInterface, AxisType type, int axisNumber, JoystickState initialState)
        {
            _axisType = type;
            _axisNumber = axisNumber;

            _value = new HeliosValue(controllerInterface, new BindingValue(GetValue(initialState)), "", Name, "Current value for " + Name + ".", "(0 - 65536)", BindingValueUnits.Numeric);
            _triggers.Add(_value);
        }

        public override string FunctionType
        {
            get
            {
                switch (_axisType)
                {
                    case AxisType.X:
                        return "X Axis";
                    case AxisType.Y:
                        return "Y Axis";
                    case AxisType.Z:
                        return "Z Axis";
                    case AxisType.Rx:
                        return "X Rotation";
                    case AxisType.Ry:
                        return "Y Rotation";
                    case AxisType.Rz:
                        return "Z Rotation";
                    case AxisType.Slider:
                        return "Slider";
                    default:
                        return "Unknown";
                }
            }
        }

        public override int ObjectNumber
        {
            get { return _axisNumber; }
        }

        public override string Name
        {
            get
            {
                switch (_axisType)
                {
                    case AxisType.X:
                        return "X Axis";
                    case AxisType.Y:
                        return "Y Axis";
                    case AxisType.Z:
                        return "Z Axis";
                    case AxisType.Rx:
                        return "X Rotation";
                    case AxisType.Ry:
                        return "Y Rotation";
                    case AxisType.Rz:
                        return "Z Rotation";
                    case AxisType.Slider:
                        return "Slider " + _axisNumber;
                    default:
                        return "Unknown";
                }
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
            _value.SetValue(new BindingValue(GetValue(state)), false);
        }

        internal int GetValue(JoystickState state)
        {
            switch (_axisType)
            {
                case AxisType.X:
                    return state.X;
                case AxisType.Y:
                    return state.Y;
                case AxisType.Z:
                    return state.Z;
                case AxisType.Rx:
                    return state.RotationX;
                case AxisType.Ry:
                    return state.RotationY;
                case AxisType.Rz:
                    return state.RotationZ;
                case AxisType.Slider:
                    return state.Sliders[_axisNumber];
                default:
                    return 0;
            }
        }
    }
}

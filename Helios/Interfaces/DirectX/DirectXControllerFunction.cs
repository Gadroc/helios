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

    public abstract class DirectXControllerFunction
    {
        public static DirectXControllerFunction Create(DirectXControllerInterface controllerInterface, Guid objectType, int objectNumber, JoystickState initialState)
        {
            DirectXControllerFunction function = null;

            if (objectType == ObjectGuid.Button)
                function = new DirectXControllerButton(controllerInterface, objectNumber, initialState);
            else if (objectType == ObjectGuid.Slider)
                function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Slider, objectNumber, initialState);
            else if (objectType == ObjectGuid.XAxis)
                function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.X, objectNumber, initialState);
            else if (objectType == ObjectGuid.YAxis)
                function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Y, objectNumber, initialState);
            else if (objectType == ObjectGuid.ZAxis)
                function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Z, objectNumber, initialState);
            else if (objectType == ObjectGuid.RxAxis)
                function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Rx, objectNumber, initialState);
            else if (objectType == ObjectGuid.RyAxis)
                function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Ry, objectNumber, initialState);
            else if (objectType == ObjectGuid.RzAxis)
                function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Rz, objectNumber, initialState);
            else if (objectType == ObjectGuid.PovController)
                function = new DirectXControllerPOVHat(controllerInterface, objectNumber, initialState);

            return function;
        }

        public static DirectXControllerFunction CreateDummy(DirectXControllerInterface controllerInterface, string type, int objectNumber)
        {
            DirectXControllerFunction function = null;
            JoystickState state = new JoystickState();

            switch (type)
            {
                case "X Axis":
                    function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.X, objectNumber, state);
                    break;
                case "Y Axis":
                    function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Y, objectNumber, state);
                    break;
                case "Z Axis":
                    function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Z, objectNumber, state);
                    break;
                case "X Rotation":
                    function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Rx, objectNumber, state);
                    break;
                case "Y Rotation":
                    function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Ry, objectNumber, state);
                    break;
                case "Z Rotation":
                    function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Rz, objectNumber, state);
                    break;
                case "Slider":
                    function = new DirectXControllerAxis(controllerInterface, DirectXControllerAxis.AxisType.Slider, objectNumber, state);
                    break;
                case "Button":
                    function = new DirectXControllerButton(controllerInterface, objectNumber, state);
                    break;
                case "POV":
                    function = new DirectXControllerPOVHat(controllerInterface, objectNumber);
                    break;
            }

            return function;
        }

        public abstract string FunctionType { get; }
        public abstract int ObjectNumber { get; }
        public abstract string Name { get; }
        public abstract HeliosValue Value { get; }
        public abstract IList<IBindingTrigger> Triggers { get; }

        public abstract void PollValue(JoystickState state);
    }
}

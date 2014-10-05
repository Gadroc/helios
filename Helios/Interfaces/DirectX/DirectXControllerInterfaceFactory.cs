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

    public class DirectXControllerInterfaceFactory : HeliosInterfaceFactory
    {

        static private DirectInput _directInput;

        static public DirectInput DirectInput
        {
            get
            {
                // TODO Should this be thread safe?
                if (_directInput == null)
                {
                    _directInput = new DirectInput();
                }
                return _directInput;
            }
        }

        public override List<HeliosInterface> GetInterfaceInstances(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            List<HeliosInterface> interfaces = new List<HeliosInterface>();

            foreach (DirectXControllerGuid guid in GetAvailableControllers(profile))
            {
                DirectXControllerInterface newInterface = new DirectXControllerInterface();
                newInterface.ControllerId = guid;
                interfaces.Add(newInterface);
            }

            return interfaces;
        }

        public override List<HeliosInterface> GetAutoAddInterfaces(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            return new List<HeliosInterface>();
        }

        internal List<DirectXControllerGuid> GetAvailableControllers(HeliosProfile profile)
        {
            List<DirectXControllerGuid> controllers = new List<DirectXControllerGuid>();
            List<DirectXControllerGuid> usedControllers = new List<DirectXControllerGuid>();

            foreach (HeliosInterface profileInterface in profile.Interfaces)
            {
                DirectXControllerInterface controllerInterface = profileInterface as DirectXControllerInterface;
                if (controllerInterface != null)
                {
                    usedControllers.Add(controllerInterface.ControllerId);
                }
            }

            IList<DeviceInstance> gameControllerList = DirectInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);
            foreach(DeviceInstance controllerInstance in gameControllerList)
            {
                DirectXControllerGuid joystickId = new DirectXControllerGuid(controllerInstance.ProductName, controllerInstance.InstanceGuid);
                if (!usedControllers.Contains(joystickId))
                {
                    controllers.Add(joystickId);
                }
            }

            return controllers;
        }

    }
}

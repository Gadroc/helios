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
    using global::Phidgets;
    using System;
    using System.Collections.Generic;

    public class PhidgetsInterfaceFactory : HeliosInterfaceFactory
    {
        private static Manager _manager;

        private static Manager PhidgetManager
        {
            get
            {
                if (_manager == null)
                {
                    _manager = new Manager();
                    _manager.open();
                    System.Threading.Thread.Sleep(1000);
                }
                return _manager;
            }
        }

        public override List<HeliosInterface> GetInterfaceInstances(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            HashSet<int> serialNumbers = new HashSet<int>();
            List<HeliosInterface> interfaces = new List<HeliosInterface>();

            foreach (HeliosInterface currentInterface in profile.Interfaces)
            {
                PhidgetInterface phidgetInterface = currentInterface as PhidgetInterface;
                if (phidgetInterface != null)
                {
                    serialNumbers.Add(phidgetInterface.SerialNumber);
                }
            }

            foreach (Phidget phidget in PhidgetManager.Devices)
            {
                if (!serialNumbers.Contains(phidget.SerialNumber))
                {
                    switch (phidget.Type)
                    {
                        case "PhidgetStepper":
                            if (descriptor.TypeIdentifier.Equals("Helios.Phidgets.UnipolarStepperBoard"))
                            {
                                interfaces.Add(new PhidgetStepperBoard(phidget.SerialNumber));
                            }
                            break;
                        case "PhidgetLED":
                            if (descriptor.TypeIdentifier.Equals("Helios.Phidgets.LedBoard"))
                            {
                                interfaces.Add(new PhidgetLEDBoard(phidget.SerialNumber));
                            }
                            break;
                        case "PhidgetAdvancedServo":
                        case "PhidgetServo":
                            if (descriptor.TypeIdentifier.Equals("Helios.Phidgets.AdvancedServoBoard"))
                            {
                                interfaces.Add(new PhidgetsServoBoard(phidget.SerialNumber));
                            }
                            break;
                        default:
                            break;
                    }
                    ConfigManager.LogManager.LogInfo("Found phidget Type = " + phidget.Type + " Serail Number = " + phidget.SerialNumber.ToString());
                }
            }

            return interfaces;
        }

        private bool IsUnique(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            foreach (HeliosInterface heliosInterface in profile.Interfaces)
            {
                HeliosInterfaceDescriptor interfaceDescriptor = ConfigManager.ModuleManager.InterfaceDescriptors[heliosInterface.GetType()];
                if (interfaceDescriptor.TypeIdentifier.Equals(descriptor.TypeIdentifier))
                {
                    // If any existing interfaces in the profile have the same type identifier do not add them.
                    return false;
                }
            }

            return true;
        }
    }
}

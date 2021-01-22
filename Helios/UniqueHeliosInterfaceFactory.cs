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
    using System.Collections.Generic;

    /// <summary>
    /// Interface factory interface which only allows one instance of the interface type per profile.
    /// </summary>
    public class UniqueHeliosInterfaceFactory : HeliosInterfaceFactory
    {
        public override List<HeliosInterface> GetInterfaceInstances(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            List<HeliosInterface> interfaces = new List<HeliosInterface>();

            if (descriptor == null)
            {
                ConfigManager.LogManager.LogWarning("Descriptor is null passed into UniqueHeliosInterfaceFactory.GetInterfaceInstances.");
            }
            else
            {
                if (IsUnique(descriptor, profile))
                {
                    HeliosInterface newInterface = (HeliosInterface)Activator.CreateInstance(descriptor.InterfaceType);
                    if (newInterface == null)
                    {
                        ConfigManager.LogManager.LogWarning("New interface is null.");
                    }
                    interfaces.Add(newInterface);
                }
                else
                {
                    ConfigManager.LogManager.LogInfo("Unique interface already exists in profile " + descriptor.Name + ". Type: " + descriptor.InterfaceType.BaseType.Name);
                }
            }

            return interfaces;
        }

        public override List<HeliosInterface> GetAutoAddInterfaces(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            List<HeliosInterface> interfaces = new List<HeliosInterface>();

            if (descriptor != null && descriptor.AutoAdd && IsUnique(descriptor, profile))
            {
                interfaces.Add((HeliosInterface)Activator.CreateInstance(descriptor.InterfaceType));
            }

            return interfaces;
        }

        private bool IsUnique(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            foreach (HeliosInterface heliosInterface in profile.Interfaces)
            {
                if (descriptor.InterfaceType.BaseType.Name != "BaseUDPInterface")
                {
                    HeliosInterfaceDescriptor interfaceDescriptor = ConfigManager.ModuleManager.InterfaceDescriptors[heliosInterface.GetType()];
                    if (interfaceDescriptor.TypeIdentifier.Equals(descriptor.TypeIdentifier))
                    {
                        // If any existing interfaces in the profile have the same type identifier do not add them.
                        return false;
                    }
                } else
                {
                    string _a = heliosInterface.TypeIdentifier;
                    string _b = heliosInterface.BaseTypeIdentifier;
                    HeliosInterfaceDescriptor interfaceDescriptor = ConfigManager.ModuleManager.InterfaceDescriptors[heliosInterface.GetType()];
                    if (interfaceDescriptor.TypeIdentifier.Equals(descriptor.TypeIdentifier) || heliosInterface.BaseTypeIdentifier == "BaseUDPInterface")
                    {
                        // If any existing interfaces in the profile have the same type identifier do not add them.
                        return false;
                    }                    
                }
            }

            return true;
        }
    }
}

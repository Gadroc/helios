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
    /// Base class for constructing interface instances.  This class is responsbile for creating 
    /// availble interface instances for adding to profiles.  Default behavior for this class 
    /// is to create a single instance of the object.
    /// </summary>
    public class HeliosInterfaceFactory : NotificationObject
    {
        public virtual List<HeliosInterface> GetInterfaceInstances(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            List<HeliosInterface> interfaces = new List<HeliosInterface>();

            if (descriptor != null)
            {
                interfaces.Add((HeliosInterface)Activator.CreateInstance(descriptor.InterfaceType));
            }

            return interfaces;
        }

        public virtual List<HeliosInterface> GetAutoAddInterfaces(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            List<HeliosInterface> interfaces = new List<HeliosInterface>();

            if (descriptor != null && descriptor.AutoAdd)
            {
                interfaces.Add((HeliosInterface)Activator.CreateInstance(descriptor.InterfaceType));
            }

            return interfaces;
        }
    }
}

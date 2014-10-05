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
    using System;

    class DirectXControllerGuid
    {
        public string ProductName { get; set; }
        public Guid InstanceGuid { get; set; }

        public DirectXControllerGuid(string productName, Guid instanceGuid)
        {
            ProductName = productName;
            InstanceGuid = instanceGuid;
        }

        public override string ToString()
        {
            return ProductName;
        }

        public override bool Equals(object obj)
        {
            if (obj is DirectXControllerGuid)
            {
                DirectXControllerGuid otherGuid = (DirectXControllerGuid)obj;
                return InstanceGuid.Equals(otherGuid.InstanceGuid);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return InstanceGuid.GetHashCode();
        }
    }
}

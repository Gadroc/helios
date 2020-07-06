﻿//  Copyright 2014 Craig Courtney
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

namespace GadrocsWorkshop.Helios.Interfaces.Falcon
{
    internal class HeliosValueWithCorrectedDeviceName : HeliosValue, IBindingElement2
    {
        public HeliosValueWithCorrectedDeviceName(FalconInterface falconInterface, BindingValue empty, string device, string name, string description, string valueDescription, BindingValueUnit unit, string correctedDeviceName): base(falconInterface, empty, device, name, description, valueDescription, unit)
        {
            DeviceInUserInterface = correctedDeviceName;
        }

        public string DeviceInUserInterface { get; }
    }
}
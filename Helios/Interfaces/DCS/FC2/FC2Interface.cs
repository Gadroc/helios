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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.FC2
{
    using System;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.UDPInterface;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;

    [HeliosInterface("Helios.FC2", "Flaming Cliffs 2", typeof(FC2InterfaceEditor), typeof(UniqueHeliosInterfaceFactory))]
    public class FC2Interface : BaseUDPInterface
    {
        public FC2Interface()
            : base("Flaming Cliffs 2")
        {
            Functions.Add(new NetworkValue(this, "1", "ADI", "pitch", "Current pitch of the aircraft.", "(-180 to 180)", BindingValueUnits.Degrees, null));
            Functions.Add(new NetworkValue(this, "2", "ADI", "bank", "Current bank of the aircraft.", "(-180 to 180)", BindingValueUnits.Degrees, null));
            Functions.Add(new NetworkValue(this, "3", "ADI", "side slip", "Current yaw of the aircraft.", "(-180 to 180)", BindingValueUnits.Degrees, null));
            Functions.Add(new NetworkValue(this, "4", "", "barametric altitude", "Current barometric altitude the aircraft.", "", BindingValueUnits.Meters, null));
            Functions.Add(new NetworkValue(this, "5", "", "rardar altitude", "Current radar altitude of the aircraft.", "", BindingValueUnits.Meters, null));
            Functions.Add(new NetworkValue(this, "6", "HSI", "ADF bearing", "Current ADF heading.", "(0 - 360)", BindingValueUnits.Degrees, null));
            Functions.Add(new NetworkValue(this, "7", "HSI", "RMI bearing", "Current RMI heading.", "(0 - 360)", BindingValueUnits.Degrees, null));
            Functions.Add(new NetworkValue(this, "8", "HSI", "heading", "Current compass heading.", "(0 - 360)", BindingValueUnits.Degrees, null));
            Functions.Add(new NetworkValue(this, "9", "left engine", "RPM", "Current RPM of the left engine.", "", BindingValueUnits.RPMPercent, null));
            Functions.Add(new NetworkValue(this, "10", "right engine", "RPM", "Current RPM of the right engine.", "", BindingValueUnits.RPMPercent, null));
            Functions.Add(new NetworkValue(this, "11", "left engine", "temperature", "Current temperature of the left engine.", "", BindingValueUnits.Celsius, null));
            Functions.Add(new NetworkValue(this, "12", "right engine", "temperature", "Current temperature of the right engine.", "", BindingValueUnits.Celsius, null));
            Functions.Add(new NetworkValue(this, "13", "", "vertical velocity", "Current vertical velocity of the aircraft.", "", BindingValueUnits.MetersPerSecond, null));
            Functions.Add(new NetworkValue(this, "14", "", "indicated airspeed", "Current indicated air speed of the aircraft.", "", BindingValueUnits.MetersPerSecond, null));
            Functions.Add(new NetworkValue(this, "15", "HSI", "distance to waypoint", "Number of meters till the next waypoint.", "", BindingValueUnits.Meters, null));
            Functions.Add(new NetworkValue(this, "16", "", "angle of attack", "Current angle of attack for the aircraft.", "", BindingValueUnits.Degrees, null));
            Functions.Add(new NetworkValue(this, "17", "ADI", "glide deviation", "ILS Glide Deviation", "-1 to 1", BindingValueUnits.Numeric, null));
            Functions.Add(new NetworkValue(this, "18", "ADI", "side deviation", "ILS Side Deiviation", "-1 to 1", BindingValueUnits.Numeric, null));
        }
    }
}

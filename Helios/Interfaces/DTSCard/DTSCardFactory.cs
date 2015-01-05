//  Copyright 2015 Craig Courtney
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

namespace GadrocsWorkshop.Helios.Interfaces.DTSCard
{
    using System;
    using System.Collections.Generic;

    public class DTSCardFactory : HeliosInterfaceFactory
    {

        public override List<HeliosInterface> GetInterfaceInstances(HeliosInterfaceDescriptor descriptor, HeliosProfile profile)
        {
            List<HeliosInterface> cardInterfaces = new List<HeliosInterface>();
            foreach (String serialNumber in DTSCard.CardSerialNumbers)
            {
                // TODO Check to see if card already in profile.
                cardInterfaces.Add(new DTSCardInterface(serialNumber));
            }

            return cardInterfaces;
        }
    }
}

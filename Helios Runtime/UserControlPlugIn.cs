//  Copyright 2013 Craig Courtney
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

using System;
using System.Linq;

using GadrocsWorkshop.Helios;
using System.ComponentModel.Composition;

namespace GadrocsWorkshop.Helios.Runtime
{
    [Export(typeof(IPlugIn))]
    [ExportMetadata("Id", "GadrocsWorkshop.UserControls")]
    [ExportMetadata("Name", "User Control")]
    [ExportMetadata("Description", "Allows the creation and use of user defined controls. Controls are created and edited with the Helios Control Editor.")]
    public class UserControlPlugIn : IPlugIn
    {
        public System.Collections.Generic.IEnumerable<IDisplay> GetDisplays()
        {
            return Enumerable.Empty<IDisplay>();
        }

        public System.Collections.Generic.IEnumerable<IDevice> GetDevices()
        {
            return Enumerable.Empty<IDevice>();
        }

        public System.Collections.Generic.IEnumerable<IControlType> GetControlTypes()
        {
            return Enumerable.Empty<IControlType>();
        }

        public IControl CreateControl(string typeId)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            // TODO: Loop through existing controls in the system and catalog them
        }

        public void Destroy()
        {
            // No-Op
        }
    }
}

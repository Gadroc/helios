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

namespace GadrocsWorkshop.Helios.PlugIns.LuaUserControl
{
    using System;
    using System.Linq;

    using GadrocsWorkshop.Helios;
    using System.ComponentModel.Composition;

    [Export(typeof(IPlugIn))]
    [ExportMetadata("Id", "GadrocsWorkshop.LuaUserControls")]
    [ExportMetadata("Name", "Lua User Control")]
    [ExportMetadata("Description", "Allows the creation and use of user defined controls in lua. Controls are created and edited with the Helios Control Editor.")]
    public class LuaUserControlPlugIn : IPlugIn
    {
        public System.Collections.Generic.IEnumerable<IDisplay> GetDisplays()
        {
            return Enumerable.Empty<IDisplay>();
        }

        public System.Collections.Generic.IEnumerable<IDevice> GetDevices()
        {
            return Enumerable.Empty<IDevice>();
        }

        public System.Collections.Generic.IEnumerable<IControl> GetControls()
        {
            return Enumerable.Empty<IControl>();
        }

        public IControl CreateControl(string typeId)
        {
            throw new NotImplementedException();
        }

        public bool IsUnique
        {
            get
            {
                return true;
            }
        }

        public bool IsAutoActive
        {
            get
            {
                return true;
            }
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

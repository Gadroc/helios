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

namespace GadrocsWorkshop.Helios.ProfileEditor.ViewModel
{
    using System;

    [Flags()]
    public enum ProfileExplorerTreeItemType
    {
        Folder =    0x001,
        Monitor =   0x002,
        Panel =     0x004,
        Interface = 0x008,
        Visual =    0x010,
        Action =    0x020,
        Trigger =   0x040,
        Value =     0x080,
        Binding =   0x100,
        Profile =   0x200
    }
}

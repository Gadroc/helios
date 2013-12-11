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
    using System.Collections.Generic;

    using GadrocsWorkshop.Helios.Visuals;

    /// <summary>
    /// Lua based control.
    /// </summary>
    public class LuaUserControl : IControl
    {
        private IList<Visual> _visuals;
        private IList<IControl> _children;

        public LuaUserControl()
        {
            _visuals = new List<Visual>();
            _children = new List<IControl>();
        }

        public string PlugInId
        { get; internal set; }

        public string TypeId
        { get; set; }

        public string TypeName
        { get; set; }

        public string TypeDescription
        { get; set; }

        public bool IsRemoteable
        { get; set; }

        public int Width
        { get; set; } 

        public int Height
        { get; set; }

        public bool AllowChildren
        { get; set; }

        public IEnumerable<IControl> Children
        {
            get
            {
                return _children;
            }
        }

        public IEnumerable<Visual> Visuals
        {
            get
            {
                return _visuals;
            }
        }

        public void Serialize(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}

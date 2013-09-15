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
using System.Collections.Generic;

using GadrocsWorkshop.Helios.ComponentModel;

namespace GadrocsWorkshop.Helios.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class UserControl : PropertyObject, IControl
    {
        private IList<IVisual> _visuals;
        private int _width;
        private int _height;

        public UserControl()
        {
            _visuals = new List<IVisual>();
        }

        public IControlType ControlType
        {
            get { throw new NotImplementedException(); }
        }

        public int Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value, "Width", PropertyInfo.Undoable); }
        }

        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value, "Height", PropertyInfo.Undoable); }
        }

        public bool AllowChildren
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<IVisual> Visuals
        {
            get { return _visuals; }
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

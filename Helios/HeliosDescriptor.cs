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

    using GadrocsWorkshop.Helios.ComponentModel;

    public class HeliosDescriptor
    {
        HeliosControlAttribute _controlAttribute;
        Type _controlType;

        public HeliosDescriptor(Type type, HeliosControlAttribute attribute)
        {
            _controlType = type;
            _controlAttribute = attribute;
        }

        public Type ControlType
        {
            get
            {
                return _controlType;
            }
        }

        public string TypeIdentifier
        {
            get
            {
                return _controlAttribute.TypeIdentifier;
            }
        }

        public string Name
        {
            get
            {
                return _controlAttribute.Name;
            }
        }

        public string Category
        {
            get
            {
                return _controlAttribute.Category;
            }
        }

        public Type Renderer
        {
            get
            {
                return _controlAttribute.Renderer;
            }
        }
    }
}

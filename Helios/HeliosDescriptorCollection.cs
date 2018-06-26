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
    using System.Collections;
    using System.Collections.Generic;

    public class HeliosDescriptorCollection : ICollection<HeliosDescriptor>
    {
        List<HeliosDescriptor> _controlDescriptors = new List<HeliosDescriptor>();
        Dictionary<string, HeliosDescriptor> _typeIdentifiers = new Dictionary<string, HeliosDescriptor>();
        Dictionary<Type, HeliosDescriptor> _types = new Dictionary<Type, HeliosDescriptor>();

        public HeliosDescriptor this[string typeIdentifier]
        {
            get
            {
                if (_typeIdentifiers.ContainsKey(typeIdentifier))
                {
                    return _typeIdentifiers[typeIdentifier];
                }
                return null;
            }
        }

        public HeliosDescriptor this[Type type]
        {
            get
            {
                if (_types.ContainsKey(type))
                {
                    return _types[type];
                }
                return null;
            }
        }

        #region ICollection<HeliosControlAttribute> Members

        public void Add(HeliosDescriptor item)
        {
            try
            {
                _controlDescriptors.Add(item);
                _typeIdentifiers.Add(item.TypeIdentifier, item);
                _types.Add(item.ControlType, item);
            }
            catch (Exception e)
            {
                ConfigManager.LogManager.LogError("Failed to add Helios Item: " + item.Name, e);
            }
        }

        public void Clear()
        {
            _controlDescriptors.Clear();
            _typeIdentifiers.Clear();
            _types.Clear();
        }

        public bool Contains(HeliosDescriptor item)
        {
            return _controlDescriptors.Contains(item);
        }

        public void CopyTo(HeliosDescriptor[] array, int arrayIndex)
        {
            _controlDescriptors.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get 
            {
                return _controlDescriptors.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(HeliosDescriptor item)
        {
            _typeIdentifiers.Remove(item.TypeIdentifier);
            _types.Remove(item.ControlType);
            return _controlDescriptors.Remove(item);
        }

        #endregion

        #region IEnumerable<HeliosControlAttribute> Members

        public IEnumerator<HeliosDescriptor> GetEnumerator()
        {
            return _controlDescriptors.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _controlDescriptors.GetEnumerator();
        }

        #endregion
    }
}

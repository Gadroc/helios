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

namespace GadrocsWorkshop.Helios.ComponentModel
{
    /// <summary>
    /// Base class for objects which propigate property data change events.
    /// </summary>
    public class PropertyObject : System.ComponentModel.INotifyPropertyChanged, System.ComponentModel.INotifyPropertyChanging
    {
        private struct ChildPropertyInfo
        {
            public string PropertyName;
            public PropertyInfo PropertyInfo;

            public ChildPropertyInfo(string name, PropertyInfo info)
            {
                PropertyName = name;
                PropertyInfo = info;
            }
        }

        private Dictionary<PropertyObject, ChildPropertyInfo> _childPropertyInfo;

        public PropertyObject()
        {
            _childPropertyInfo = new Dictionary<PropertyObject, ChildPropertyInfo>();
        }

        #region Child Property Propigation

        /// <summary>
        /// Registers a child property object for event propigation.
        /// </summary>
        /// <param name="child">Child object which will be monitored for changes.</param>
        /// <param name="name">Property name used for event propigation.</param>
        /// <param name="propertyInfo">Property meta-data info used for propigation.</param>
        protected void RegisterChildPropertyObject(PropertyObject child, string name, PropertyInfo propertyInfo)
        {
            if (!_childPropertyInfo.ContainsKey(child))
            {
                ChildPropertyInfo childInfo = new ChildPropertyInfo(name, propertyInfo);
                _childPropertyInfo.Add(child, childInfo);
                child.PropertyChanged += Child_PropertyChanged;
            }
            else
            {
                // Log Error message here!
            }
        }

        /// <summary>
        /// Removes a child object from event propigation.
        /// </summary>
        /// <param name="child">Child object which will be removed from propigation.</param>
        protected void DeregisterChildPropertyObject(PropertyObject child)
        {
            child.PropertyChanged -= Child_PropertyChanged;
            if (_childPropertyInfo.ContainsKey(child))
            {
                _childPropertyInfo.Remove(child);
            }
        }

        private void Child_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedEventArgs childPropertyInfo = e as PropertyChangedEventArgs;
            if (childPropertyInfo != null)
            {
                ChildPropertyInfo childInfo = _childPropertyInfo[childPropertyInfo.PropertySource];
                PropertyChangedEventArgs childArgs = new PropertyChangedEventArgs(this, childInfo.PropertyName, childPropertyInfo, childInfo.PropertyInfo);
                OnPropertyChanged(childArgs);
            }
        }

        #endregion

        /// <summary>
        /// Helper method to set property values.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="backingStore">Backing store variable for the property</param>
        /// <param name="value">New value to set the property to</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="onChanged">Action which should be called when the property has changed</param>
        /// <param name="onChanging">Action which should be called if this property is going to change</param>
        protected void SetProperty<T>(ref T backingStore, T value, string propertyName, Action<T, T> onChanged = null, Action<T> onChanging = null)
        {
            SetProperty(ref backingStore, value, propertyName, 0, onChanged, onChanging);
        }

        /// <summary>
        /// Helper method to set property values.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="backingStore">Backing store variable for the property</param>
        /// <param name="value">New value to set the property to</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyInfo">Flags indicating special actions when this property changes</param>
        /// <param name="onChanged">Action which should be called when the property has changed</param>
        /// <param name="onChanging">Action which should be called if this property is going to change</param>
        protected void SetProperty<T>(ref T backingStore, T value, string propertyName, PropertyInfo propertyInfo, Action<T, T> onChanged = null, Action<T> onChanging = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value)) return;

            T oldValue = backingStore;

            if (onChanging != null) onChanging(value);

            OnPropertyChanging(new PropertyChangingEventArgs(this, propertyName, oldValue, value));

            backingStore = value;

            if (onChanged != null) onChanged(oldValue, value);

            OnPropertyChanged(new PropertyChangedEventArgs(this, propertyName, oldValue, value, propertyInfo));
        }

        /// <summary>
        /// Helper method to set property values with a WeakRerence backing store.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="backingStore">Backing store variable for the property</param>
        /// <param name="value">New value to set the property to</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="onChanged">Action which should be called when the property has changed</param>
        /// <param name="onChanging">Action which should be called if this property is going to change</param>
        protected void SetWeakReferenceProperty<T>(ref WeakReference backingStore, T value, string propertyName, Action<T, T> onChanged = null, Action<T> onChanging = null)
        {
            SetWeakReferenceProperty(ref backingStore, value, propertyName, 0, onChanged, onChanging);
        }

        /// <summary>
        /// Helper method to set property values with a WeakRerence backing store.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="backingStore">Backing store variable for the property</param>
        /// <param name="value">New value to set the property to</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyInfo">Flags indicating special actions for this propety</param>
        /// <param name="onChanged">Action which should be called when the property has changed</param>
        /// <param name="onChanging">Action which should be called if this property is going to change</param>
        protected void SetWeakReferenceProperty<T>(ref WeakReference backingStore, T value, string propertyName, PropertyInfo propertyInfo, Action<T, T> onChanged = null, Action<T> onChanging = null)
        {
            T oldValue = (T)backingStore.Target;

            if (EqualityComparer<T>.Default.Equals(oldValue, value)) return;

            OnPropertyChanging(new PropertyChangingEventArgs(this, propertyName, oldValue, value));

            backingStore = new WeakReference(value);

            if (onChanged != null) onChanged(oldValue, value);

            OnPropertyChanged(new PropertyChangedEventArgs(this, propertyName, oldValue, value, propertyInfo));
        }

        #region INotifyPropertyChanged Implementation

        [field: NonSerialized]
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            System.ComponentModel.PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        [field: NonSerialized]
        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            System.ComponentModel.PropertyChangingEventHandler handler = PropertyChanging;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }

            if (e.Canceled)
            {
                throw new ArgumentException(e.Message);
            }
        }

        #endregion
    }
}

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

namespace GadrocsWorkshop.Helios.ViewModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ViewModel object which warps a native object.
    /// </summary>
    /// <typeparam name="T">Object type which this view wraps.</typeparam>
    public abstract class WrapViewModel<T> : BaseViewModel
    {
        private T _model = default(T);

        /// <summary>
        /// Wrapped model object for this view model.
        /// </summary>
        public T Model
        {
            get { return _model; }
            set
            {
                if (_model != null)
                {
                    throw new InvalidOperationException("Can not change model on an already instatiated wrapped view model object.");
                }
                _model = value;
                OnModelSet(_model);
            }
        }

        /// <summary>
        /// Called when the model object has been set on this viewmodel.
        /// </summary>
        /// <param name="model">Model object which is being created.</param>
        protected virtual void OnModelSet(T model)
        {
        }

        /// <summary>
        /// Helper method to set property values.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="value">New value to set the property to</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="onChanged">Action which should be called when the property has changed</param>
        /// <param name="onChanging">Action which should be called if this property is going to change</param>
        protected void SetWrappedProperty<Y>(Y value, string propertyName, Action<Y, Y> onChanged = null, Action<Y> onChanging = null)
        {
            SetWrappedProperty(value, propertyName, 0, onChanged, onChanging);
        }

        /// <summary>
        /// Helper method to set property values.
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="value">New value to set the property to</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyInfo">Flags indicating special actions when this property changes</param>
        /// <param name="onChanged">Action which should be called when the property has changed</param>
        /// <param name="onChanging">Action which should be called if this property is going to change</param>
        protected void SetWrappedProperty<Y>(Y value, string propertyName, PropertyInfo propertyInfo, Action<Y, Y> onChanged = null, Action<Y> onChanging = null)
        {
            System.Reflection.PropertyInfo property = Model.GetType().GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException("Property name not valid on wrapped object.", "propertyName");
            }

            if (EqualityComparer<Y>.Default.Equals((Y)property.GetValue(Model), value)) return;

            Y oldValue = (Y)property.GetValue(Model);

            if (onChanging != null) onChanging(value);

            OnPropertyChanging(new PropertyChangingEventArgs(this, propertyName, oldValue, value));

            property.SetValue(Model, value);

            if (onChanged != null) onChanged(oldValue, value);

            OnPropertyChanged(new PropertyChangedEventArgs(this, propertyName, oldValue, value, propertyInfo));
        }
    }
}

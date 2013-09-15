using System;

namespace GadrocsWorkshop.Helios.ComponentModel
{
    /// <summary>
    /// This class extends <see cref="T:PropertyChangedEventArgs"/> and
    /// allows for storing the old and new values of the changed property.
    /// </summary>
    public class PropertyChangedEventArgs : System.ComponentModel.PropertyChangedEventArgs
    {
        // Source object whose property changed.
        private PropertyObject _propertySource;

        // Holds the new value of the property.
        private Object _newValue;

        // Holds the old value of the property.
        private Object _oldValue;

        // Holds the child object that is property belongs to.
        private PropertyChangedEventArgs _childNotification;

        // Holds flag indicating whether this change should be undoable
        private PropertyInfo _propertyInfo;

        #region Constructors

        /// <summary>
        /// Constructs a child notification event based of childs propertynotification
        /// </summary>
        /// <param name="eventSource">Object whose child object has a property changed.</param>
        /// <param name="childPropertyName">Name of the property which represents the child object whose property has chagned.</param>
        /// <param name="childNotification">Event args for child's property notification.</param>
        public PropertyChangedEventArgs(PropertyObject eventSource, string childPropertyName, PropertyChangedEventArgs childNotification, PropertyInfo propertyInfo)
            : this(eventSource, childPropertyName, null, null, propertyInfo)
        {
            _childNotification = childNotification;
        }

        /// <summary>
        /// Constructs a new notification event for a direct property change.
        /// <see cref="PropertyDataChangedEventArgs"/> class.
        /// </summary>
        /// <param name="eventSource">Object whose property has changed.</param>
        /// <param name="propertyName">
        /// The name of the property that is associated with this
        /// notification.
        /// </param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="undoable">True if this property change is undoable.</param>
        public PropertyChangedEventArgs(PropertyObject eventSource, String propertyName, Object oldValue, Object newValue, PropertyInfo propertyInfo)
            : base(propertyName)
        {
            _propertySource = eventSource;
            _oldValue = oldValue;
            _newValue = newValue;
            _propertyInfo = propertyInfo;
        }

        /// <summary>
        /// Constructs a new notification event for a undoable direct property change.
        /// <see cref="PropertyDataChangedEventArgs"/> class.
        /// </summary>
        /// <param name="eventSource">Object whose property has changed.</param>
        /// <param name="propertyName">
        /// The name of the property that is associated with this
        /// notification.
        /// </param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public PropertyChangedEventArgs(PropertyObject eventSource, String propertyName, Object oldValue, Object newValue)
            : this(eventSource, propertyName, oldValue, newValue, 0)
        {
        }

        #endregion // Constructors

        #region Properties

        public PropertyObject PropertySource
        {
            get { return _propertySource; }
        }

        public bool HasChildNotification
        {
            get { return _childNotification != null; }
        }

        public PropertyChangedEventArgs ChildNotification
        {
            get { return _childNotification; }
        }

        /// <summary>
        /// Gets the new value of the property.
        /// </summary>
        /// <value>The new value.</value>
        public Object NewValue
        {
            get
            {
                return this._newValue;
            }
        }

        /// <summary>
        /// Gets the old value of the property.
        /// </summary>
        /// <value>The old value.</value>
        public Object OldValue
        {
            get
            {
                return this._oldValue;
            }
        }

        /// <summary>
        /// Property meta-data indicating what actions should happen when this property changes.
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
        }

        #endregion // Properties
    }
}

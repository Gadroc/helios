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
using System.Globalization;

namespace GadrocsWorkshop.Helios.Binding
{
    /// <summary>
    /// Binding values are passed between triggers and actions.
    /// </summary>
    public class BindingValue
    {
        public static BindingValue Empty = new BindingValue(BindingUom.GetUom("other"));

        private bool _boolValue = false;
        private string _stringValue = "";
        private double _doubleValue = 0d;

        private BindingValueType _nativeValueType;
        private BindingValueType _convertedValueTypes;

        private BindingUom _uom;

        #region Constructors

        private BindingValue(BindingUom uom)
        {
            _uom = uom;
        }

        public BindingValue(bool value, BindingUom uom) : this(uom)
        {
            _boolValue = value;
            _nativeValueType = BindingValueType.Boolean;
            _convertedValueTypes = BindingValueType.Boolean;
        }

        public BindingValue(string value, BindingUom uom)
            : this(uom)
        {
            _stringValue = value;
            if (_stringValue == null)
            {
                _stringValue = "";
            }
            _nativeValueType = BindingValueType.String;
            _convertedValueTypes = BindingValueType.String;
        }

        public BindingValue(double value, BindingUom uom)
            : this(uom)
        {
            _doubleValue = value;
            _nativeValueType = BindingValueType.Double;
            _convertedValueTypes = BindingValueType.Double;
        }

        #endregion

        /// <summary>
        /// Unit of measure for this binding value.
        /// </summary>
        public BindingUom Uom
        {
            get { return _uom; }
        }

        /// <summary>
        /// Returns true if this is an instance of an empty binding value.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.Equals(BindingValue.Empty); }
        }

        #region Custom Value Extractors

        /// <summary>
        /// Gets the value of this object in string format.  Values which
        /// can not be convert to string will return "";
        /// </summary>
        public string StringValue
        {
            get
            {
                if (!_convertedValueTypes.HasFlag(BindingValueType.String))
                {
                    ConvertToString();
                }
                return _stringValue;
            }
        }

        /// <summary>
        /// Gets the value of this object in bool format.  Values which can 
        /// not be converted to bool will return false.
        /// </summary>
        public bool BoolValue
        {
            get
            {
                if (!_convertedValueTypes.HasFlag(BindingValueType.Boolean))
                {
                    ConvertToBool();
                }
                return _boolValue;
            }
        }

        /// <summary>
        /// Gets the value of this object in double format. Values which can
        /// not be converted to double will return 0.
        /// </summary>
        public double DoubleValue
        {
            get
            {
                if (!_convertedValueTypes.HasFlag(BindingValueType.Double))
                {
                    ConvertToDouble();
                }
                return _doubleValue;
            }
        }

        /// <summary>
        /// Gets the native type of this value.
        /// </summary>
        public BindingValueType NaitiveType
        {
            get
            {
                return _nativeValueType;
            }
        }

        #endregion

        #region Value Type Convertors

        private void ConvertToDouble()
        {
            switch (_nativeValueType)
            {
                case BindingValueType.Boolean:
                    _doubleValue = _boolValue ? 1 : 0;
                    break;

                case BindingValueType.String:
                    if (!double.TryParse(_stringValue, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out _doubleValue))
                    {
                        _doubleValue = 0;
                    }
                    break;
            }

            _convertedValueTypes = _convertedValueTypes | BindingValueType.Double;
        }

        private void ConvertToBool()
        {
            switch (_nativeValueType)
            {
                case BindingValueType.String:
                    _boolValue = !(_stringValue.Equals("0", StringComparison.InvariantCultureIgnoreCase) ||
                        _stringValue.Equals("false", StringComparison.InvariantCultureIgnoreCase) ||
                        _stringValue.Equals("no", StringComparison.InvariantCultureIgnoreCase) ||
                        _stringValue.Equals("off", StringComparison.InvariantCultureIgnoreCase));
                    break;

                case BindingValueType.Double:
                    _boolValue = (_doubleValue == 0f);
                    break;
            }

            _convertedValueTypes = _convertedValueTypes | BindingValueType.Boolean;
        }

        private void ConvertToString()
        {
            switch (_nativeValueType)
            {
                case BindingValueType.Boolean:
                    _stringValue = _boolValue ? "1" : "0";
                    break;

                case BindingValueType.Double:
                    _stringValue = _doubleValue.ToString(CultureInfo.InvariantCulture);
                    break;
            }

            _convertedValueTypes = _convertedValueTypes | BindingValueType.String;
        }

        #endregion

        public override bool Equals(object obj)
        {
            object compareObject = obj;

            if (obj == this)
            {
                return true;
            }
            else if (obj == Empty || this == Empty)
            {
                return false;
            }

            if (obj is BindingValue)
            {
                BindingValue bvObj = (BindingValue)obj;
                switch (bvObj._nativeValueType)
                {
                    case BindingValueType.Boolean:
                        compareObject = bvObj.BoolValue;
                        break;

                    case BindingValueType.String:
                        compareObject = bvObj.StringValue;
                        break;

                    case BindingValueType.Double:
                        compareObject = bvObj.DoubleValue;
                        break;
                }
            }

            if (compareObject is string)
            {
                return StringValue.Equals(compareObject);
            }
            else if (compareObject is double)
            {
                return DoubleValue.Equals(compareObject);
            }
            else if (compareObject is bool)
            {
                return BoolValue.Equals(compareObject);
            }


            return false;
        }

        public override int GetHashCode()
        {
            switch (_nativeValueType)
            {
                case BindingValueType.Boolean:
                    return _boolValue.GetHashCode();

                case BindingValueType.Double:
                    return _doubleValue.GetHashCode();

                case BindingValueType.String:
                    return _stringValue.GetHashCode();
            }

            return base.GetHashCode();
        }
    }
}

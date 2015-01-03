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

namespace GadrocsWorkshop.Helios.Interfaces.Phidgets
{
    using global::Phidgets;
    using System;

    public class PhidgetsServo : NotificationObject
    {
        private int _servoNum;
        private CalibrationPointCollectionDouble _calibration;
        private PhidgetsServoBoard _servoBoard;

        private bool _speedRamping = true;

        private double _acceleration = 1d;
        private double _velocityLimit = 0d;

        private double _minAcceleration = double.MinValue;
        private double _maxAcceleration = double.MaxValue;

        private double _minVelocity = double.MinValue;
        private double _maxVelocity = double.MaxValue;

        private ServoServo.ServoType _servoType = ServoServo.ServoType.DEFAULT;
        private double _minPulseWidth = 1000d;
        private double _maxPulseWidth = 2000d;
        private double _servoMaxVelocity = 300d;
        private double _servoDegrees = 180d;

        private HeliosValue _targetPosition;
        private HeliosValue _value;

        public PhidgetsServo(PhidgetsServoBoard servoBoard, int servoNumber)
        {
            _servoNum = servoNumber;
            _servoBoard = servoBoard;

            _targetPosition = new HeliosValue(servoBoard, new BindingValue(0d), "Servo " + _servoNum.ToString(), "target position", "Sets the raw target position of this stepper.", "", BindingValueUnits.Numeric);
            _targetPosition.Execute += TargetPosition_Execute;

            _value = new HeliosValue(servoBoard, new BindingValue(0d), "Servo " + _servoNum.ToString(), "input value", "Sets the input value to be displayed on this stepper.", "Input value will be interpolated with the calibration data and set the target position for the stepper as appropriate.", BindingValueUnits.Numeric);
            _value.Execute += Value_Execute;

            _calibration = new CalibrationPointCollectionDouble(-6000d, 0d, 6000d, 180d);
            _calibration.CalibrationChanged += new EventHandler(_calibration_CalibrationChanged);
        }

        void _calibration_CalibrationChanged(object sender, EventArgs e)
        {
            _servoBoard.SetTargetPosition(_servoNum, _calibration.Interpolate(_value.Value.DoubleValue));
        }

        #region Properties

        public double Acceleration
        {
            get
            {
                return _acceleration;
            }
            set
            {
                if (!_acceleration.Equals(value))
                {
                    if (value < _minAcceleration || value > _maxAcceleration)
                    {
                        throw new ArgumentException("Acceleration is outside allowable values (" + _minAcceleration + ", " + _maxAcceleration + ")");
                    }

                    double oldValue = _acceleration;
                    _acceleration = value;
                    _servoBoard.SetAcceleration(_servoNum, _acceleration);
                    OnPropertyChanged("Acceleration", oldValue, value, true);
                }
            }
        }

        public double VelocityLimit
        {
            get
            {
                return _velocityLimit;
            }
            set
            {
                if (!_velocityLimit.Equals(value))
                {
                    if (value < _minVelocity || value > _maxVelocity)
                    {
                        throw new ArgumentException("VelocityLimit is outside allowable values (" + _minVelocity + ", " + _maxVelocity + ")");
                    }

                    double oldValue = _velocityLimit;
                    _velocityLimit = value;
                    _servoBoard.SetVelocityLimit(_servoNum, _velocityLimit);
                    OnPropertyChanged("VelocityLimit", oldValue, value, true);
                }
            }
        }

        public bool SpeedRamping
        {
            get
            {
                return _speedRamping;
            }
            set
            {
                if (!_speedRamping.Equals(value))
                {
                    bool oldValue = _speedRamping;
                    _speedRamping = value;
                    _servoBoard.SetSpeedRamping(_servoNum, _speedRamping);
                    OnPropertyChanged("SpeedRamping", oldValue, value, true);
                }
            }
        }

        public double ServoMinPulseWidth
        {
            get
            {
                return _minPulseWidth;
            }
            set
            {
                if (!_minPulseWidth.Equals(value))
                {
                    double oldValue = _minPulseWidth;
                    _minPulseWidth = value;
                    OnPropertyChanged("ServoMinPulseWidth", oldValue, value, true);
                    _servoBoard.SetServoProperties(_servoNum, _servoType, _minPulseWidth, _maxPulseWidth, _servoMaxVelocity, _servoDegrees);
                }
            }
        }

        public double ServoMaxPulseWidth
        {
            get
            {
                return _maxPulseWidth;
            }
            set
            {
                if (!_maxPulseWidth.Equals(value))
                {
                    double oldValue = _maxPulseWidth;
                    _maxPulseWidth = value;
                    OnPropertyChanged("ServoMaxPulseWidth", oldValue, value, true);
                    _servoBoard.SetServoProperties(_servoNum, _servoType, _minPulseWidth, _maxPulseWidth, _servoMaxVelocity, _servoDegrees);
                }
            }
        }

        public double ServoMaxVelocity
        {
            get
            {
                return _servoMaxVelocity;
            }
            set
            {
                if (!_servoMaxVelocity.Equals(value))
                {
                    double oldValue = _servoMaxVelocity;
                    _servoMaxVelocity = value;
                    OnPropertyChanged("ServoMaxVelocity", oldValue, value, true);
                    _servoBoard.SetServoProperties(_servoNum, _servoType, _minPulseWidth, _maxPulseWidth, _servoMaxVelocity, _servoDegrees);
                }
            }
        }

        public double ServoDegrees
        {
            get
            {
                return _servoDegrees;
            }
            set
            {
                if (!_servoDegrees.Equals(value))
                {
                    double oldValue = _servoDegrees;
                    _servoDegrees = value;
                    OnPropertyChanged("ServoDegrees", oldValue, value, true);
                    _servoBoard.SetServoProperties(_servoNum, _servoType, _minPulseWidth, _maxPulseWidth, _servoMaxVelocity, _servoDegrees);
                }
            }
        }

        public ServoServo.ServoType ServoType
        {
            get
            {
                return _servoType;
            }
            set
            {
                if (!_servoType.Equals(value))
                {
                    ServoServo.ServoType oldValue = _servoType;
                    _servoType = value;
                    OnPropertyChanged("ServoType", oldValue, value, true);
                    _servoBoard.SetServoProperties(_servoNum, _servoType, _minPulseWidth, _maxPulseWidth, _servoMaxVelocity, _servoDegrees);
                }
            }
        }

        internal double MinAcceleration
        {
            get { return _minAcceleration; }
            set { _maxAcceleration = value; }
        }

        internal double MaxAcceleration
        {
            get { return _maxAcceleration; }
            set { _maxAcceleration = value; }
        }

        internal double MinVelocity
        {
            get { return _minVelocity; }
            set { _minVelocity = value; }
        }

        internal double MaxVelocity
        {
            get { return _maxVelocity; }
            set { _maxVelocity = value; }
        }

        public int ServoNumber
        {
            get
            {
                return _servoNum;
            }
        }

        public HeliosValue ServoValue
        {
            get { return _value; }
        }

        public HeliosValue TargetPosition
        {
            get { return _targetPosition; }
        }

        public CalibrationPointCollectionDouble Calibration
        {
            get { return _calibration; }
        }

        #endregion

        private void Value_Execute(object action, HeliosActionEventArgs e)
        {
            _value.SetValue(e.Value, e.BypassCascadingTriggers);
            _targetPosition.SetValue(new BindingValue(_calibration.Interpolate(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            _servoBoard.SetTargetPosition(_servoNum, (long)_targetPosition.Value.DoubleValue);
        }

        private void TargetPosition_Execute(object action, HeliosActionEventArgs e)
        {
            _targetPosition.SetValue(e.Value, e.BypassCascadingTriggers);
            _servoBoard.SetTargetPosition(_servoNum, (long)e.Value.DoubleValue);
        }

    }
}
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
    using System;

    public class PhidgetsStepper : NotificationObject
    {
        private int _motorNum;
        private CalibrationPointCollectionLong _calibration;
        private PhidgetStepperBoard _stepperBoard;

        private double _acceleration = 0d;
        private double _velocityLimit = 0d;

        private double _minAcceleration = double.MinValue;
        private double _maxAcceleration = double.MaxValue;

        private double _minVelocity = double.MinValue;
        private double _maxVelocity = double.MaxValue;

        private HeliosValue _targetPosition;
        private HeliosValue _value;
        private HeliosAction _zero;
        private HeliosAction _increment;
        private HeliosAction _decrement;

        public PhidgetsStepper(PhidgetStepperBoard stepperBoard, int motorNumber)
        {
            _motorNum = motorNumber;
            _stepperBoard = stepperBoard;

            _zero = new HeliosAction(stepperBoard, "Motor " + _motorNum.ToString(), "zero", "", "Sets the current position as zero.");
            _zero.Execute += Zero_Execute;

            _targetPosition = new HeliosValue(stepperBoard, new BindingValue(0d), "Motor " + _motorNum.ToString(), "target position", "Sets the raw target position of this stepper.", "", BindingValueUnits.Numeric);
            _targetPosition.Execute += TargetPosition_Execute;

            _value = new HeliosValue(stepperBoard, new BindingValue(0d), "Motor " + _motorNum.ToString(), "input value", "Sets the input value to be displayed on this stepper.", "Input value will be interpolated with the calibration data and set the target position for the stepper as appropriate.", BindingValueUnits.Numeric);
            _value.Execute += Value_Execute;

            _calibration = new CalibrationPointCollectionLong(-6000d, -376, 6000d, 376);
            _calibration.CalibrationChanged += new EventHandler(_calibration_CalibrationChanged);

            _increment = new HeliosAction(stepperBoard, "Motor " + _motorNum.ToString(), "increment", "", "Increments the stepper position.", "Number of steps to increment steppers position. If empty or not a number 1 will be used.", BindingValueUnits.Numeric);
            _increment.Execute += IncrementPosition_Execute;

            _decrement = new HeliosAction(stepperBoard, "Motor " + _motorNum.ToString(), "decrement", "", "Decrements the stepper position.", "Number of steps to decrement steppers position. If empty or not a number 1 will be used.", BindingValueUnits.Numeric);
            _decrement.Execute += DecrementPosition_Execute;
        }

        void _calibration_CalibrationChanged(object sender, EventArgs e)
        {
            _stepperBoard.SetTargetPosition(_motorNum, _calibration.Interpolate(_value.Value.DoubleValue));
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
                    _stepperBoard.SetAcceleration(_motorNum, _acceleration);
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
                    _stepperBoard.SetVelocityLimit(_motorNum, _velocityLimit);
                    OnPropertyChanged("VelocityLimit", oldValue, value, true);
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

        public int MotorNumber
        {
            get
            {
                return _motorNum;
            }
        }

        public HeliosValue StepperValue
        {
            get { return _value; }
        }

        public HeliosValue TargetPosition
        {
            get { return _targetPosition; }
        }

        public HeliosAction Zero
        {
            get { return _zero; }
        }

        public HeliosAction Increment
        {
            get { return _increment; }
        }

        public HeliosAction Decrement
        {
            get { return _decrement; }
        }

        public CalibrationPointCollectionLong Calibration
        {
            get { return _calibration; }
        }

        #endregion

        private void Zero_Execute(object action, HeliosActionEventArgs e)
        {
            _stepperBoard.ZeroCurrentPosition(_motorNum);
        }

        private void Value_Execute(object action, HeliosActionEventArgs e)
        {
            _value.SetValue(e.Value, e.BypassCascadingTriggers);
            _targetPosition.SetValue(new BindingValue(_calibration.Interpolate(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            _stepperBoard.SetTargetPosition(_motorNum, (long)_targetPosition.Value.DoubleValue);
        }

        private void TargetPosition_Execute(object action, HeliosActionEventArgs e)
        {
            _targetPosition.SetValue(e.Value, e.BypassCascadingTriggers);
            _stepperBoard.SetTargetPosition(_motorNum, (long)e.Value.DoubleValue);
        }

        private void IncrementPosition_Execute(object action, HeliosActionEventArgs e)
        {
            long incrementValue = (long)e.Value.DoubleValue;
            _targetPosition.SetValue(new BindingValue(_targetPosition.Value.DoubleValue + (incrementValue > 0 ? incrementValue : 1)), e.BypassCascadingTriggers);
            _stepperBoard.SetTargetPosition(_motorNum, (long)_targetPosition.Value.DoubleValue);
        }

        private void DecrementPosition_Execute(object action, HeliosActionEventArgs e)
        {
            long incrementValue = (long)e.Value.DoubleValue;
            _targetPosition.SetValue(new BindingValue(_targetPosition.Value.DoubleValue - (incrementValue > 0 ? incrementValue : 1)), e.BypassCascadingTriggers);
            _stepperBoard.SetTargetPosition(_motorNum, (long)_targetPosition.Value.DoubleValue);
        }
    }
}

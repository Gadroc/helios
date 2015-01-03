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

namespace GadrocsWorkshop.Helios.Interfaces.Eos
{
    using GadrocsWorkshop.Eos;
    using System;
    using System.Collections.Generic;

    public class EosStepper : EosOutput
    {
        private WeakReference _board = new WeakReference(null);
        private byte _number;
        private uint _maxSpeed = 3200;
        private ulong _idleSleep = 1000000;

        private CalibrationPointCollectionLong _calibration;

        private HeliosValue _targetPosition;
        private HeliosValue _value;
        private HeliosAction _zero;
        private HeliosAction _increment;
        private HeliosAction _decrement;

        public EosStepper(EosBoard board, byte number) : base(board)
        {
            _number = number;
            Name = "Servo " + number;

            _calibration = new CalibrationPointCollectionLong(-6000d, -6000, 6000d, 6000);
            _calibration.CalibrationChanged += new EventHandler(_calibration_CalibrationChanged);

            _zero = new HeliosAction(board.EOSInterface, "Stepper " + Number.ToString(), "zero", "", "Sets the current position as zero.");
            _zero.Execute += Zero_Execute;
            Actions.Add(_zero);

            _targetPosition = new HeliosValue(board.EOSInterface, new BindingValue(0d), "Stepper " + Number.ToString(), "target position", "Sets the raw target position of this stepper.", "", BindingValueUnits.Numeric);
            _targetPosition.Execute += TargetPosition_Execute;
            Actions.Add(_targetPosition);

            _value = new HeliosValue(board.EOSInterface, new BindingValue(0d), "Stepper " + Number.ToString(), "input value", "Sets the input value to be displayed on this stepper.", "Input value will be interpolated with the calibration data and set the target position for the stepper as appropriate.", BindingValueUnits.Numeric);
            _value.Execute += Value_Execute;
            Actions.Add(_value);

            _increment = new HeliosAction(board.EOSInterface, "Stepper " + Number.ToString(), "increment", "", "Increments the stepper position.", "Number of steps to increment steppers position. If empty or not a number 1 will be used.", BindingValueUnits.Numeric);
            _increment.Execute += IncrementPosition_Execute;
            Actions.Add(_increment);

            _decrement = new HeliosAction(board.EOSInterface, "Stepper " + Number.ToString(), "decrement", "", "Decrements the stepper position.", "Number of steps to decrement steppers position. If empty or not a number 1 will be used.", BindingValueUnits.Numeric);
            _decrement.Execute += DecrementPosition_Execute;
            Actions.Add(_decrement);
        }

        public byte Number
        {
            get { return _number; }
        }

        public CalibrationPointCollectionLong Calibration
        {
            get { return _calibration; }
        }

        public uint MaxSpeed
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                uint oldValue = _maxSpeed;
                if (oldValue != value)
                {
                    _maxSpeed = value;
                    Board.Device.SetStepperConfig(Number, _maxSpeed, _idleSleep);
                    OnPropertyChanged("PulseInterval", oldValue, value, true);
                }
            }
        }

        public ulong IdleSleep
        {
            get
            {
                return _idleSleep;
            }
            set
            {
                ulong oldValue = _idleSleep;
                if (oldValue != value)
                {
                    _idleSleep = value;
                    Board.Device.SetStepperConfig(Number, _maxSpeed, _idleSleep);
                    OnPropertyChanged("SleepEnabled", oldValue, value, true);
                }
            }
        }

        public HeliosValue StepperValue
        {
            get { return _value; }
        }

        void _calibration_CalibrationChanged(object sender, EventArgs e)
        {
            if (Board.Device != null)
            {
                _targetPosition.SetValue(new BindingValue(_calibration.Interpolate(_value.Value.DoubleValue)), true);
                Board.Device.SetStepperTargetPosition(Number, (long)_targetPosition.Value.DoubleValue);
            }
        }

        private void Zero_Execute(object action, HeliosActionEventArgs e)
        {
            Board.Device.ZeroStepperPosition(Number);
            _value.SetValue(new BindingValue(0), e.BypassCascadingTriggers);
        }

        private void Value_Execute(object action, HeliosActionEventArgs e)
        {
            _value.SetValue(e.Value, e.BypassCascadingTriggers);
            _targetPosition.SetValue(new BindingValue(_calibration.Interpolate(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            Board.Device.SetStepperTargetPosition(Number, (long)_targetPosition.Value.DoubleValue);
        }

        private void TargetPosition_Execute(object action, HeliosActionEventArgs e)
        {
            _targetPosition.SetValue(e.Value, e.BypassCascadingTriggers);
            Board.Device.SetStepperTargetPosition(Number, (long)e.Value.DoubleValue);
        }

        private void IncrementPosition_Execute(object action, HeliosActionEventArgs e)
        {
            long incrementValue = (long)e.Value.DoubleValue;
            _targetPosition.SetValue(new BindingValue(_targetPosition.Value.DoubleValue + (incrementValue > 0 ? incrementValue : 1)), e.BypassCascadingTriggers);
            Board.Device.SetStepperTargetPosition(Number, (long)_targetPosition.Value.DoubleValue);
        }

        private void DecrementPosition_Execute(object action, HeliosActionEventArgs e)
        {
            long incrementValue = (long)e.Value.DoubleValue;
            _targetPosition.SetValue(new BindingValue(_targetPosition.Value.DoubleValue - (incrementValue > 0 ? incrementValue : 1)), e.BypassCascadingTriggers);
            Board.Device.SetStepperTargetPosition(Number, (long)_targetPosition.Value.DoubleValue);
        }

        public void ParseConfigPacket(EosPacket packet)
        {
            if (packet.Source == Board.Address && packet.Command == (byte)EosBusCommands.STEPPER_GET_CONFIG_RESPONSE)
            {
                byte stepper = packet.Data[0];

                if (stepper == Number)
                {
                    uint _lastMaxSpeed = _maxSpeed;
                    ulong _lastIdleSleep = _idleSleep;

                    _maxSpeed = ReadInt(packet.Data, 1);
                    _idleSleep = ReadLong(packet.Data, 3);

                    if (_lastMaxSpeed != _maxSpeed)
                    {
                        OnPropertyChanged("MaxSpeed", _lastMaxSpeed, _maxSpeed, true);
                    }
                    if (_lastIdleSleep != _idleSleep)
                    {
                        OnPropertyChanged("IdleSleep", _lastIdleSleep, _idleSleep, true);
                    }
                }
            }
        }

        private uint ReadInt(List<byte> data, int index)
        {
            return (uint)((data[index] << 8) | (data[index + 1]));
        }

        private ulong ReadLong(List<byte> data, int index)
        {
            return (ulong)((data[index] << 24) | (data[index + 1] << 16) | (data[index + 2] << 8) | (data[index + 3]));
        }
    }

}

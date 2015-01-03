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

    public class EosServo : EosOutput
    {
        private WeakReference _board = new WeakReference(null);
        private byte _number;
        private int _minValue;
        private int _maxValue;
        private int _defaultValue;

        private HeliosValue _signalValue;
        private HeliosValue _value;

        private CalibrationPointCollectionLong _calibration;

        public EosServo(EosBoard board, byte number) : base(board)
        {
            _number = number;
            Name = "Servo " + number;

            _calibration = new CalibrationPointCollectionLong(0d, 1000, 180d, 2000);
            _calibration.CalibrationChanged += new EventHandler(_calibration_CalibrationChanged);

            _signalValue = new HeliosValue(board.EOSInterface, new BindingValue(0d), "Servo " + Number.ToString(), "signal value", "Sets the raw signal pulse in microseconds.", "", BindingValueUnits.Numeric);
            _signalValue.Execute += SignalValue_Execte;
            Actions.Add(_signalValue);

            _value = new HeliosValue(board.EOSInterface, new BindingValue(0d), "Servo " + Number.ToString(), "input value", "Sets the input value to be displayed on this servo.", "Input value will be interpolated with the calibration data and set the target position for the servo as appropriate.", BindingValueUnits.Numeric);
            _value.Execute += Value_Execute;
            Actions.Add(_value);
        }

        public byte Number
        {
            get { return _number; }
        }

        public CalibrationPointCollectionLong Calibration
        {
            get { return _calibration; }
        }

        public int MinimumValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                int oldValue = _minValue;
                if (oldValue != value)
                {
                    _minValue = value;
                    Board.Device.SetServoConfig(Number, _minValue, _maxValue, _defaultValue);
                    OnPropertyChanged("MinimumValue", oldValue, value, true);
                }
            }
        }

        public int MaximumValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                int oldValue = _maxValue;
                if (oldValue != value)
                {
                    _maxValue = value;
                    Board.Device.SetServoConfig(Number, _minValue, _maxValue, _defaultValue);
                    OnPropertyChanged("MaximumValue", oldValue, value, true);
                }
            }
        }

        public int DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            set
            {
                int oldValue = _defaultValue;
                if (oldValue != value)
                {
                    _defaultValue = value;
                    Board.Device.SetServoConfig(Number, _minValue, _maxValue, _defaultValue);
                    OnPropertyChanged("DefaultValue", oldValue, value, true);
                }

            }
        }

        public HeliosValue ServoValue
        {
            get { return _value; }
        }

        public void ParseConfigPacket(EosPacket packet)
        {
            if (packet.Source == Board.Address && packet.Command == (byte)EosBusCommands.SERVO_GET_CONFIG_RESPONSE)
            {
                byte servo = packet.Data[0];

                if (servo == Number)
                {
                    int _lastMin = _minValue;
                    int _lastMax = _maxValue;
                    int _lastDefault = _defaultValue;

                    _minValue = ReadInt(packet.Data, 1);
                    _maxValue = ReadInt(packet.Data, 3);
                    _defaultValue = ReadInt(packet.Data, 5);

                    if (_lastMin != _minValue)
                    {
                        OnPropertyChanged("MinimumValue", _lastMin, _minValue, true);
                    }
                    if (_lastMax != _maxValue)
                    {
                        OnPropertyChanged("MaximumValue", _lastMax, _maxValue, true);
                    }
                    if (_lastDefault != _defaultValue)
                    {
                        OnPropertyChanged("DefaultValue", _lastDefault, _defaultValue, true);
                    }
                }
            }
        }

        private int ReadInt(List<byte> data, int index)
        {
            return (data[index] << 8) | (data[index + 1]);
        }


        private void SignalValue_Execte(object action, HeliosActionEventArgs e)
        {
            _signalValue.SetValue(e.Value, e.BypassCascadingTriggers);
            Board.Device.SetServoValue(Number, (int)_signalValue.Value.DoubleValue);
        }

        private void Value_Execute(object action, HeliosActionEventArgs e)
        {
            _value.SetValue(e.Value, e.BypassCascadingTriggers);
            _signalValue.SetValue(new BindingValue(_calibration.Interpolate(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            Board.Device.SetServoValue(Number, (int)_signalValue.Value.DoubleValue);
        }

        void _calibration_CalibrationChanged(object sender, EventArgs e)
        {
            if (Board.Device != null)
            {
                _signalValue.SetValue(new BindingValue(_calibration.Interpolate(_value.Value.DoubleValue)), true);
                Board.Device.SetServoValue(Number, (int)_signalValue.Value.DoubleValue);
            }
        }
    }
}

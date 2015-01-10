//  Copyright 2015 Craig Courtney
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
    using System;

    public class EosCoilOutput : EosOutput
    {
        private WeakReference _board = new WeakReference(null);
        private byte _number;

        private HeliosValue _coilValue;
        private HeliosValue _value;

        private CalibrationPointCollectionLong _calibration;

        public EosCoilOutput(EosBoard board, byte number)
            : base(board)
        {
            _number = number;
            Name = "Coil " + number;

            _calibration = new CalibrationPointCollectionLong(-255, -255, 255, 255);
            _calibration.CalibrationChanged += new EventHandler(_calibration_CalibrationChanged);

            _coilValue = new HeliosValue(board.EOSInterface, new BindingValue(0d), "Coil " + Number.ToString(), "coil value", "Sets the coil output value.", "-255 to 255", BindingValueUnits.Numeric);
            _coilValue.Execute += CoilValue_Execte;
            Actions.Add(_coilValue);

            _value = new HeliosValue(board.EOSInterface, new BindingValue(0d), "Coil " + Number.ToString(), "input value", "Sets the input value to be displayed on this coil.", "Input value will be interpolated with the calibration data and set the target position for the servo as appropriate.", BindingValueUnits.Numeric);
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

        public HeliosValue CoilValue
        {
            get { return _value; }
        }

        private void CoilValue_Execte(object action, HeliosActionEventArgs e)
        {
            _coilValue.SetValue(e.Value, e.BypassCascadingTriggers);
            Board.Device.SetCoilPosition(Number, (int)_coilValue.Value.DoubleValue);
        }

        private void Value_Execute(object action, HeliosActionEventArgs e)
        {
            _value.SetValue(e.Value, e.BypassCascadingTriggers);
            _coilValue.SetValue(new BindingValue(_calibration.Interpolate(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            Board.Device.SetCoilPosition(Number, (int)_coilValue.Value.DoubleValue);
        }

        void _calibration_CalibrationChanged(object sender, EventArgs e)
        {
            if (Board.Device != null)
            {
                _coilValue.SetValue(new BindingValue(_calibration.Interpolate(_value.Value.DoubleValue)), true);
                Board.Device.SetCoilPosition(Number, (int)_coilValue.Value.DoubleValue);
            }
        }
    }
}

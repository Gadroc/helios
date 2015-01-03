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
    using System;

    public class EosAnalogInput : EosInput
    {
        private WeakReference _board = new WeakReference(null);
        private byte _number;
        private int _state;

        private HeliosValue _value;

        public EosAnalogInput(EosBoard board, byte number)
        {
            _board = new WeakReference(board);
            _number = number;
            Name = "Analog Input " + number;

            _value = new HeliosValue(board.EOSInterface, new BindingValue(0), board.Name, Name, "Analog input", "0-1024", BindingValueUnits.Numeric);
            Triggers.Add(_value);
        }

        private EosBoard Board
        {
            get { return (EosBoard)_board.Target; }
        }

        public byte Number
        {
            get { return _number; }
        }

        public int State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    int oldValue = _state;
                    _state = value;

                    _value.SetValue(new BindingValue(value), false);

                    OnPropertyChanged("State", oldValue, value, false);
                }
            }
        }
    }
}

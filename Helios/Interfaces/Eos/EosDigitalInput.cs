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

    public class EosDigitalInput : EosInput
    {
        private WeakReference _board = new WeakReference(null);
        private byte _number;
        private bool _state;

        private HeliosTrigger _onTrigger;
        private HeliosTrigger _offTrigger;

        public EosDigitalInput(EosBoard board, byte number)
        {
            _board = new WeakReference(board);
            _number = number;
            Name = "Digital Input " + number;

            _onTrigger = new HeliosTrigger(board.EOSInterface, board.Name, Name, "On", "Triggered when this digital input is moved to the on state.");
            Triggers.Add(_onTrigger);

            _offTrigger = new HeliosTrigger(board.EOSInterface, board.Name, Name, "Off", "Triggered when this digitla input is moved to the off state.");
            Triggers.Add(_offTrigger);
        }

        private EosBoard Board
        {
            get { return (EosBoard)_board.Target; }
        }

        public byte Number
        {
            get { return _number; }
        }

        public bool State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    if (_state)
                    {
                        _onTrigger.FireTrigger(new BindingValue(true));
                    }
                    else
                    {
                        _offTrigger.FireTrigger(new BindingValue(false));
                    }
                    OnPropertyChanged("State", !value, value, false);
                }
            }
        }
    }
}

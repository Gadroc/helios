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

    public class EosTextOutput : EosOutput
    {
        private WeakReference _board = new WeakReference(null);
        private byte _number;

        public EosTextOutput(EosBoard board, byte number) : base(board)
        {
            _number = number;
            Name = "Text " + number;

            HeliosAction setText = new HeliosAction(board.EOSInterface, board.Name, Name, "set", "Sets the text displpayed.", "", BindingValueUnits.Text);
            setText.Execute += SetText_Execute;
            Actions.Add(setText);
        }

        public byte Number
        {
            get { return _number; }
        }

        private void SetText_Execute(object action, HeliosActionEventArgs e)
        {
            Board.Device.SetDisplayText(_number, e.Value.StringValue);
        }
    }
}

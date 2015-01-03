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

    public class EosLedOutput : EosOutput
    {
        private WeakReference _board = new WeakReference(null);
        private byte _number;

        public EosLedOutput(EosBoard board, byte number) : base(board)
        {
            _number = number;
            Name = "LED " + number;

            HeliosAction backlightBrightness = new HeliosAction(board.EOSInterface, board.Name, Name, "brightness", "Sets the brightness level of the led outputs.", "0-255", BindingValueUnits.Numeric);
            backlightBrightness.Execute += LedBrightness_Execute;
            Actions.Add(backlightBrightness);

            HeliosAction backlightPower = new HeliosAction(board.EOSInterface, board.Name, Name, "power", "Turns led power on and off.", "False turns backlight off, true turns it on.", BindingValueUnits.Boolean);
            backlightPower.Execute += LedPower_Execute;
            Actions.Add(backlightPower);
        }

        public byte Number
        {
            get { return _number; }
        }

        private void LedPower_Execute(object action, HeliosActionEventArgs e)
        {
            Board.Device.SetLedPower(_number, e.Value.BoolValue);
        }

        private void LedBrightness_Execute(object action, HeliosActionEventArgs e)
        {
            Board.Device.SetLedLevel(_number, (byte)e.Value.DoubleValue);
        }
    }
}

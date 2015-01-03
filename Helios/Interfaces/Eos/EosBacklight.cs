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

    public class EosBacklight : EosOutput
    {
        public EosBacklight(EosBoard board) : base(board)
        {
            Name = "Backlight";

            HeliosAction backlightBrightness = new HeliosAction(board.EOSInterface, board.Name, "backlight brightness", "set", "Sets the brightness level of the backlight outputs.", "0-255", BindingValueUnits.Numeric);
            backlightBrightness.Execute += BacklighBrightness_Execute;
            Actions.Add(backlightBrightness);

            HeliosAction backlightPower = new HeliosAction(board.EOSInterface, board.Name, "backlight power", "set", "Turns backlight power on and off.", "False turns backlight off, true turns it on.", BindingValueUnits.Boolean);
            backlightPower.Execute += BacklightPower_Execute;
            Actions.Add(backlightPower);
        }

        private void BacklightPower_Execute(object action, HeliosActionEventArgs e)
        {
            Board.Device.SetBacklightPower(e.Value.BoolValue);
        }

        private void BacklighBrightness_Execute(object action, HeliosActionEventArgs e)
        {
            Board.Device.SetBacklightLevel((byte)e.Value.DoubleValue);
        }
    }
}

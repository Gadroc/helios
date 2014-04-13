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
    using GadrocsWorkshop.Helios.Interfaces.Phidgets.UndoEvents;
    using System;

    public class LedViewModel : NotificationObject
    {
        private PhidgetLEDBoard _board;
        private int _led;
        private LEDGroup _group;

        public LedViewModel(PhidgetLEDBoard board, LEDGroup group, int led)
        {
            _board = board;
            _group = group;
            _led = led;
        }

        #region Properties

        public PhidgetLEDBoard Board
        {
            get { return _board; }
        }

        public LEDGroup Group
        {
            get { return _group; }
        }

        public int Id
        {
            get { return _led; }
        }

        public bool IsSelected
        {
            get
            {
                return _group.Leds.Contains(_led);
            }
            set
            {
                if (value && !_group.Leds.Contains(_led))
                {
                    _group.Leds.Add(_led);
                    OnPropertyChanged("IsSelected", false, true, false);
                    ConfigManager.UndoManager.AddUndoItem(new AddLedUndoEvent(this, true));
                }
                else if (!value && _group.Leds.Contains(_led))
                {
                    _group.Leds.Remove(_led);
                    OnPropertyChanged("IsSelected", true, false, false);
                    ConfigManager.UndoManager.AddUndoItem(new AddLedUndoEvent(this, false));
                }
            }
        }

        #endregion
    }
}

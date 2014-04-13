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
    using System.Collections.ObjectModel;

    public class LedGroupViewModel
    {
        private PhidgetLEDBoard _board;
        private LEDGroup _group;
        private ObservableCollection<LedViewModel> _leds = new ObservableCollection<LedViewModel>();

        public LedGroupViewModel(PhidgetLEDBoard board, LEDGroup group)
        {
            _board = board;
            _group = group;
            for(int i=0; i < 64; i++)
            {
                _leds.Add(new LedViewModel(board, group, i));
            }
        }

        #region Properties

        public PhidgetLEDBoard Board
        {
            get { return _board; }
        }

        public LEDGroup Group
        {
            get
            {
                return _group;
            }
        }

        public ObservableCollection<LedViewModel> Leds
        {
            get { return _leds; }
        }

        #endregion
    }
}

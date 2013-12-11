//  Copyright 2013 Craig Courtney
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

namespace GadrocsWorkshop.Helios
{
    using System;

    /// <summary>
    /// Event arguments representing a change to an analog intput.
    /// </summary>
    public class AnalogInputEventArgs : EventArgs
    {
        private int _index;
        private int _oldValue;
        private int _newValue;

        /// <summary>
        /// Creates an event representing a chagne to an analog input.
        /// </summary>
        /// <param name="index">Index of the analog input</param>
        /// <param name="oldValue">Old value for this analog input.</param>
        /// <param name="newValue">New value for this analog input.</param>
        public AnalogInputEventArgs(int index, int oldValue, int newValue)
        {
            _index = index;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public int Index
        {
            get { return _index; }
        }

        public int OldValue
        {
            get { return _oldValue; }
        }

        public int NewValue
        {
            get { return _newValue; }
        }
    }
}

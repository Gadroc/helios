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
    /// Event arguments representing a change in a digital outputs state.
    /// </summary>
    public class DigitalInputEventArgs : EventArgs
    {
        private int _index;
        private bool _value;

        /// <summary>
        /// Creates an event representing a change in a digital outputs state.
        /// </summary>
        /// <param name="index">Index of the digital input.</param>
        /// <param name="value">New value of the digital input.</param>
        public DigitalInputEventArgs(int index, bool value)
        {
            _index = index;
            _value = value;
        }

        /// <summary>
        /// Index of the digital input which has changed.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }

        /// <summary>
        /// New value of the digital input.
        /// </summary>
        public bool Value
        {
            get { return _value; }
        }
    }
}

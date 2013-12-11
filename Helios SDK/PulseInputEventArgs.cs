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
    /// Event arguments representing a set of pulse input.
    /// </summary>
    public class PulseInputEventArgs : EventArgs
    {
        private int _index;
        private int _count;

        /// <summary>
        /// Creates an event representing a set of pulse inputs.
        /// </summary>
        /// <param name="index">Index of the pulse input.</param>
        /// <param name="count">Sum of pulse count since last event.  Since some devices may work on polling this count
        ///                     represents total movement not individual pulses.  If 3 positive and 1 negative pulse happens
        ///                     between polling event the pulse count will be 2.  In additoin this count can be negative
        ///                     if there are more negative pulses than positive ones.</param>
        public PulseInputEventArgs(int index, int count)
        {
            _index = index;
            _count = count;
        }

        /// <summary>
        /// Index of the pulse input.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }

        /// <summary>
        /// Sum of pulse count since last event.  Since some devices may work on polling this count
        /// represents total movement not individual pulses.  If 3 positive and 1 negative pulse happens
        /// between polling event the pulse count will be 2.  In additoin this count can be negative
        /// if there are more negative pulses than positive ones.</param>
        /// </summary>
        public int Count
        {
            get { return _count; }

        }
    }
}

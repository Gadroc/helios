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

namespace GadrocsWorkshop.Helios
{
    public class CalibrationPointLong : NotificationObject
    {
        private double _input;
        private long _output;

        public CalibrationPointLong(double input, long outputValue)
        {
            _input = input;
            _output = outputValue;
        }

        public double Value
        {
            get
            {
                return _input;
            }
            set
            {
                if (!_input.Equals(value))
                {
                    double oldValue = _input;
                    _input = value;
                    OnPropertyChanged("Value", oldValue, value, false);
                }
            }
        }

        public long Multiplier
        {
            get
            {
                return _output;
            }
            set
            {
                long oldValue = _output;
                _output = value;
                OnPropertyChanged("Multiplie", oldValue, value, false);
            }
        }
    }
}

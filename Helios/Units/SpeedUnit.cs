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

namespace GadrocsWorkshop.Helios.Units
{
    using System;

    public class SpeedUnit : BindingValueUnit
    {
        private DistanceUnit _distance;
        private TimeUnit _time;
        private string _abbreviation;
        private string _description;


        public SpeedUnit(DistanceUnit distance, TimeUnit per, string abbreviation, string description)
        {
            _distance = distance;
            _time = per;
            _abbreviation = abbreviation;
            _description = description;
        }

        public override BindingValueUnitType Type
        {
            get { return BindingValueUnitType.Speed; }
        }

        public override string ShortName
        {
            get { return _abbreviation; }
        }

        public override string LongName
        {
            get { return _description; }
        }

        public DistanceUnit DistanceUnit
        {
            get { return _distance; }
        }

        public TimeUnit TimeUnit
        {
            get { return _time; }
        }

        public override int GetHashCode()
        {
            int hash = 7;
            hash = 31 * hash + (_distance == null ? 0 : _distance.GetHashCode());
            hash = 31 * hash + (_time == null ? 0 : _time.GetHashCode());
            return hash;
        }

        public override bool Equals(object obj)
        {
            SpeedUnit o = obj as SpeedUnit;

            if (o == null) return false;
            if (!Object.Equals(o._distance, _distance)) return false;
            if (!Object.Equals(o._time, _time)) return false;

            return true;
        }
    }
}

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

    public class MassFlowUnit : BindingValueUnit
    {
        private MassUnit _mass;
        private TimeUnit _time;
        private string _abbreviation;
        private string _description;


        public MassFlowUnit(MassUnit mass, TimeUnit per, string abbreviation, string description)
        {
            _mass = mass;
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

        public MassUnit MassUnit
        {
            get { return _mass; }
        }

        public TimeUnit TimeUnit
        {
            get { return _time; }
        }

        public override int GetHashCode()
        {
            int hash = 7;
            hash = 31 * hash + (_mass == null ? 0 : _mass.GetHashCode());
            hash = 31 * hash + (_time == null ? 0 : _time.GetHashCode());
            return hash;
        }

        public override bool Equals(object obj)
        {
            MassFlowUnit o = obj as MassFlowUnit;

            if (o == null) return false;
            if (!Object.Equals(o._mass, _mass)) return false;
            if (!Object.Equals(o._time, _time)) return false;

            return true;
        }
    }
}

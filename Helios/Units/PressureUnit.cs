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

    public class PressureUnit : BindingValueUnit
    {
        private MassUnit _mass;
        private AreaUnit _area;
        private string _abbreviation;
        private string _description;

        protected PressureUnit()
        {
        }

        public PressureUnit(MassUnit mass, AreaUnit per, string abbreviation, string description)
        {
            _mass = mass;
            _area = per;
            _abbreviation = abbreviation;
            _description = description;
        }

        public override BindingValueUnitType Type
        {
            get { return BindingValueUnitType.Pressure; }
        }

        public override string ShortName
        {
            get { return _abbreviation; }
        }

        public override string LongName
        {
            get { return _description; }
        }

        public virtual double MassConversionFactor
        {
            get { return _mass.ConversionFactor; }
        }

        public virtual double AreaConversionFactor
        {
            get { return _area.ConversionFactor; }
        }

        public override int GetHashCode()
        {
            int hash = 7;
            hash = 31 * hash + (_mass == null ? 0 : _mass.GetHashCode());
            hash = 31 * hash + (_area == null ? 0 : _area.GetHashCode());
            return hash;
        }

        public override bool Equals(object obj)
        {
            PressureUnit o = obj as PressureUnit;

            if (o == null) return false;
            if (!Object.Equals(o._mass, _mass)) return false;
            if (!Object.Equals(o._area, _area)) return false;

            return true;
        }
    }
}

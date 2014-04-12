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

namespace GadrocsWorkshop.Helios.Interfaces.Falcon
{
    using System;

    public struct RadarContact
    {
        public RadarSymbols Symbol;
        public double Bearing;
        public double RelativeBearing;
        public bool MissileActivity;
        public bool MissileLaunch;
        public bool Selected;
        public double Lethality;
        public bool NewDetection;
        public bool Visible;
    }
}

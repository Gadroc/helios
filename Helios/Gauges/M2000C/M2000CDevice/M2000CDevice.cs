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

namespace GadrocsWorkshop.Helios.Gauges.M2000C
{
    using System.Windows;
    using GadrocsWorkshop.Helios.M2000C;

    abstract class M2000CDevice : M2000CCompositeVisual
    {
        public M2000CDevice(string name, Size size)
            : base(name, size)
        {
            DefaultInterfaceName = "DCS M2000C";

        }

        #region Properties

        public abstract string BezelImage { get; }

        #endregion
    }
}

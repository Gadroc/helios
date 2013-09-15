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

using System;
using System.Diagnostics;

namespace GadrocsWorkshop.Helios.Binding
{
    /// <summary>
    /// Represents a simple unit of measure.
    /// </summary>
    internal class BindingSimpleUom : BindingUom
    {

        #region Properties

        /// <summary>
        /// Conversion factor which is used 
        /// </summary>
        public double ConversionFactor { get; set; }

        #endregion

        #region Methods

        protected override double ConvertTo(double value, BindingUom to)
        {
            Debug.Assert(to != null, "Cannot convert to a null UOM");

            if (to.UnitType == null || UnitType == null || !to.UnitType.Equals(UnitType))
            {
                throw new ArgumentException("Can not convert between differing UomTypes.");
            }

            if (value == 0d)
            {
                return 0d;
            }

            double toFactor = ((BindingSimpleUom)to).ConversionFactor;
            double fromFactor = ConversionFactor;

            double returnValue = (value * fromFactor) / toFactor;
            Trace.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 2, "Converted {0}{1} to {2}{3} using simpleuom.", value, DisplayName, returnValue, to.DisplayName);
            return returnValue;
        }

        #endregion
    }
}

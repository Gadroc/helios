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

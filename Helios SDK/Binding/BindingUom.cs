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

namespace GadrocsWorkshop.Helios.Binding
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Unit of Measure
    /// </summary>
    public abstract class BindingUom
    {
        private static readonly TraceSource _source = new TraceSource("Helios");

        #region Properties

        protected static TraceSource Trace { get { return _source; } }

        /// <summary>
        /// Unique ID used in proeprty and configuration files used to specify units of measure
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Text which will be displayed in the UI for this unit of measure
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Long description of this unit of mesaure used for editor UI help text
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Abbreviation of this unit of measure
        /// </summary>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Type of unit this represents
        /// </summary>
        public BindingUomType UnitType { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts a value from this unit of measure to another.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="to">Unit of measure to convert value to.</param>
        /// <returns>Value in the new unit of measure.</returns>
        protected abstract double ConvertTo(double value, BindingUom to);

        #endregion

        #region Static methods for loading uoms and converting values

        private static Dictionary<String, BindingUom> _uomList;

        static BindingUom()
        {
            BindingUom._uomList = new Dictionary<string, BindingUom>();

            // TODO: Load in uom xml files here
        }

        /// <summary>
        /// Loads a set of UOMs from a config file.
        /// </summary>
        /// <param name="stream"></param>
        private static void LoadUomStream(Stream stream)
        {
        }

        /// <summary>
        /// List of available UOMs
        /// </summary>
        public static IEnumerable<BindingUom> UomList
        {
            get
            {
                return _uomList.Values;
            }
        }

        /// <summary>
        /// Returns a Uom object based on it's id.
        /// </summary>
        /// <param name="id">Id of the Uom to return</param>
        /// <returns>Uom object containing data necessary for UoM conversion.</returns>
        public static BindingUom GetUom(string id)
        {
            return _uomList[id];
        }

        /// <summary>
        /// Checks to see if it is possible to convert from one uom to another.
        /// </summary>
        /// <param name="from">UOM to convert from.</param>
        /// <param name="to">UOM to convert to.</param>
        /// <returns>True if the value can be converted.</returns>
        public static bool CanConvert(BindingUom from, BindingUom to)
        {
            if (from != null && to.UnitType.Equals(from.UnitType) && !to.UnitType.Id.Equals("other"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts a value from one unit of measure to another.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="from">Unit of measure to convert from.</param>
        /// <param name="to">Unit of measure to convert to.</param>
        /// <returns>Value in the new unit of measure.</returns>
        public static double Convert(double value, BindingUom from, BindingUom to)
        {

            if (from == null)
            {
                throw new NullReferenceException("Can not convert from null unit of measure");
            }

            return from.ConvertTo(value, to);
        }

        #endregion
    }
}

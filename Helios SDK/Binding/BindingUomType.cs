using System;

namespace GadrocsWorkshop.Helios.Binding
{
    public class BindingUomType
    {
        /// <summary>
        /// Unique ID used in proeprty and configuration files used to specify units of measure
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Text which will be displayed in the UI for this unit of measure
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Flag indicating this Uom is a compound uom.
        /// </summary>
        public Boolean Compound { get; set; }

        /// <summary>
        /// For compound types this is the type of the base unit (Ex: for speed this would be distance)
        /// </summary>
        public BindingUomType BaseType { get; set; }


        /// <summary>
        /// For compunt types this is set to the per unit type (Ex: for speed this would be time)
        /// </summary>
        public BindingUomType PerType { get; set; }

        /// <summary>
        /// Base unit which is used to calculate conversion factors for this type of unit.  Note
        /// this is not applicable for compound unit types.
        /// </summary>
        public string BaseUnit { get; set; }
    }
}

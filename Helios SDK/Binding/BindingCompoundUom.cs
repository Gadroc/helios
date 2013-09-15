using System;
using System.Diagnostics;

namespace GadrocsWorkshop.Helios.Binding
{
    /// <summary>
    /// Compound unit of measures which is represented by a simpleuom over a another simpleuom (eq: Miles per Hour).
    /// </summary>
    internal class BindingCompoundUom : BindingUom
    {
        public double BaseFactor { get; set; }

        public double PerFactor { get; set; }

        public BindingCompoundUom()
        {
        }

        public BindingCompoundUom(BindingUomType unitType, BindingSimpleUom baseUom, BindingSimpleUom perUom)
        {
            this.Id = baseUom.Id + perUom.Id;
            this.DisplayName = baseUom.DisplayName + "/" + perUom.DisplayName;
            this.Description = baseUom.DisplayName + " per " + perUom.DisplayName;
            this.BaseFactor = baseUom.ConversionFactor;
            this.PerFactor = perUom.ConversionFactor;
            this.UnitType = unitType;
        }

        protected override double ConvertTo(double value, BindingUom to)
        {
            if (!to.UnitType.Equals((object)this.UnitType))
                throw new ArgumentException("Can not convert between differing UomTypes.");
            if (value == 0.0)
                return 0.0;
            double baseFactor1 = ((BindingCompoundUom)to).BaseFactor;
            double perFactor1 = ((BindingCompoundUom)to).PerFactor;
            double baseFactor2 = this.BaseFactor;
            double perFactor2 = this.PerFactor;
            double num = value * baseFactor2 / baseFactor1 / (perFactor2 / perFactor1);
            Trace.TraceEvent(TraceEventType.Verbose, 2, "Converted {0}{1} to {2}{3} using compound uom.", (object)value, (object)this.DisplayName, (object)num, (object)to.DisplayName);
            return num;
        }
    }
}

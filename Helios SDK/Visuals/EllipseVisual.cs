using System;

namespace GadrocsWorkshop.Helios.Visuals
{
    /// <summary>
    /// Renders an ellipse on the control.
    /// </summary>
    public class EllipseVisual : ColorVisual
    {

        public override byte VisualType
        {
            get { return 1; }
        }

        public override string VisualTypeName
        {
            get { return "ellipse"; }
        }
    }
}

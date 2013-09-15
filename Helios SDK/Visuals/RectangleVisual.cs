using System;

namespace GadrocsWorkshop.Helios.Visuals
{
    /// <summary>
    /// Renders an rectangle on the control.
    /// </summary>
    public class RectangleVisual : ColorVisual
    {
        public override byte VisualType
        {
            get { return 0; }
        }

        public override string VisualTypeName
        {
            get { return "rectangle"; }
        }
    }
}

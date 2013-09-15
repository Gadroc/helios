using System;

using GadrocsWorkshop.Helios.ComponentModel;

namespace GadrocsWorkshop.Helios.Visuals
{
    /// <summary>
    /// Visual which is drawn in a specified color.
    /// </summary>
    public abstract class ColorVisual : VisualBase
    {
        private Color _color;

        /// <summary>
        /// Color to render this visual with.
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { SetProperty(ref _color, value, "Color", PropertyInfo.Undoable); }
        }

    }
}

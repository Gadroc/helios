using System;

using GadrocsWorkshop.Helios.ComponentModel;

namespace GadrocsWorkshop.Helios.Visuals
{
    /// <summary>
    /// Renders text onto this control.
    /// </summary>
    public class TextVisual : ColorVisual
    {
        private string _text;

        /// <summary>
        /// Text to be displayed on this control
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value, "Text", PropertyInfo.Undoable); }
        }

        public override byte VisualType
        {
            get { return 2; }
        }

        public override string VisualTypeName
        {
            get { return "text"; }
        }
    }
}

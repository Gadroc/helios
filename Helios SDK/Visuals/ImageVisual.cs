using System;

using GadrocsWorkshop.Helios.ComponentModel;

namespace GadrocsWorkshop.Helios.Visuals
{
    /// <summary>
    /// Renders an image on the control.
    /// </summary>
    public class ImageVisual : VisualBase
    {
        private string _imagePath;
        private ImageTileMode _tileMode;

        public string ImagePath
        {
            get { return _imagePath; }
            set { SetProperty(ref _imagePath, value, "ImagePath", PropertyInfo.Undoable); }
        }

        public ImageTileMode TileMode
        {
            get { return _tileMode; }
            set { SetProperty(ref _tileMode, value, "TileMode", PropertyInfo.Undoable); }
        }

        public override byte VisualType
        {
            get { return 3; }
        }

        public override string VisualTypeName
        {
            get { return "image"; }
        }
    }
}

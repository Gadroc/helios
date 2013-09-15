using System;
using System.ComponentModel;

using GadrocsWorkshop.Helios.ComponentModel;

namespace GadrocsWorkshop.Helios.Visuals
{
    /// <summary>
    /// Base class providing common features of all visuals.
    /// </summary>
    public abstract class VisualBase : PropertyObject, IVisual
    {
        private string _id;
        private bool _static;
        private int _top;
        private int _left;
        private int _width;
        private int _height;

        private int _rotationCenterHorizontal;
        private int _rotationCenterVertical;

        public abstract byte VisualType { get; }
        public abstract string VisualTypeName { get; }

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value, "Id", PropertyInfo.Undoable); }
        }

        public bool IsStatic
        {
            get { return _static; }
            set { SetProperty(ref _static, value, "IsStatic", PropertyInfo.Undoable); }
        }

        public int Top
        {
            get { return _top; }
            set { SetProperty(ref _top, value, "Top", PropertyInfo.Undoable); }
        }

        public int Left
        {
            get { return _left; }
            set { SetProperty(ref _left, value, "Left", PropertyInfo.Undoable); }
        }

        public int Width
        {
            get { return _width; }
            set { SetProperty(ref _width, value, "Width", PropertyInfo.Undoable); }
        }

        public int Height
        {
            get { return _height; }
            set { SetProperty(ref _height, value, "Height", PropertyInfo.Undoable); }
        }

        public int RotationCenterVertical
        {
            get { return _rotationCenterVertical; }
            set { SetProperty(ref _rotationCenterVertical, value, "RotationCenterVertical", PropertyInfo.Undoable); }
        }

        public int RotationCenterHorizontal
        {
            get { return _rotationCenterHorizontal; }
            set { SetProperty(ref _rotationCenterHorizontal, value, "RotationCenterHorizontal", PropertyInfo.Undoable); }
        }

    }
}

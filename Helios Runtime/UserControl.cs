using System;
using System.Collections.Generic;

using GadrocsWorkshop.Helios.ComponentModel;

namespace GadrocsWorkshop.Helios.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class UserControl : PropertyObject, IControl
    {
        private IList<IVisual> _visuals;
        private int _width;
        private int _height;

        public UserControl()
        {
            _visuals = new List<IVisual>();
        }

        public IControlType ControlType
        {
            get { throw new NotImplementedException(); }
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

        public bool AllowChildren
        {
            get
            {
                return false;
            }
        }

        public IEnumerable<IVisual> Visuals
        {
            get { return _visuals; }
        }

        public void Serialize(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}

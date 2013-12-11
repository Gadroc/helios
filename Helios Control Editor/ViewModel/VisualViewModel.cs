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

namespace GadrocsWorkshop.Helios.ControlEditor.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using GadrocsWorkshop.Helios.ViewModel;
    using GadrocsWorkshop.Helios.Visuals;

    public class VisualViewModel : WrapViewModel<Visual>
    {
        private WeakReference _parent;
        private ObservableCollection<VisualViewModel> _children;

        #region Constructor

        public VisualViewModel() : this(null)
        {
        }

        public VisualViewModel(BaseViewModel parent)
        {
            _children = new ObservableCollection<VisualViewModel>();
            // TODO: Watch for changes to this collection and update our children.
            Parent = parent;
        }

        public VisualViewModel(Visual visual, BaseViewModel parent) : this(parent)
        {
            Model = visual;
        }

        protected override void OnModelSet(Visual model)
        {
            // Loop through children of the visual and create view models
            foreach (Visual child in model.Children)
            {
                _children.Add(new VisualViewModel(child, this));
            }
        }

        #endregion

        public BaseViewModel Parent
        {
            get { return (BaseViewModel)_parent.Target; }
            set { _parent = new WeakReference(value); }
        }

        public ObservableCollection<VisualViewModel> Children
        {
            get { return _children; }
        }

        #region Wrapped Properties

        public byte VisualType
        {
            get { return Model.VisualType; }
        }

        public string VisualTypeName
        {
            get { return Model.VisualTypeName; }
        }

        public override string DisplayName
        {
            get { return Id; }
        }
        
        public string Id
        {
            get { return Model.Id; }
            set { SetWrappedProperty(value, "Id", PropertyInfo.Undoable | PropertyInfo.DisplayName); }
        }

        public string Description
        {
            get { return Model.Description; }
            set { SetWrappedProperty(value, "Description", PropertyInfo.Undoable); }
        }

        public bool IsStatic
        {
            get { return Model.IsStatic; }
            set { SetWrappedProperty(value, "IsStatic", PropertyInfo.Undoable); }
        }

        public float Top
        {
            get { return Model.Top; }
            set { SetWrappedProperty(value, "Top", PropertyInfo.Undoable); }
        }

        public float Left
        {
            get { return Model.Left; }
            set { SetWrappedProperty(value, "Left", PropertyInfo.Undoable); }
        }

        public float Width
        {
            get { return Model.Width; }
            set { SetWrappedProperty(value, "Width", PropertyInfo.Undoable); }
        }

        public float Height
        {
            get { return Model.Height; }
            set { SetWrappedProperty(value, "Height", PropertyInfo.Undoable); }
        }

        public float RotationCenterVertical
        {
            get { return Model.RotationCenterVertical; }
            set { SetWrappedProperty(value, "RotationCenterVertical", PropertyInfo.Undoable); }
        }

        public float RotationCenterHorizontal
        {
            get { return Model.RotationCenterHorizontal; }
            set { SetWrappedProperty(value, "RotationCenterHorizontal", PropertyInfo.Undoable); }
        }

        public string ImagePath
        {
            get { return Model.ImagePath; }
            set { SetWrappedProperty(value, "ImagePath", PropertyInfo.Undoable); }
        }

        public Color Color
        {
            get { return Model.Color; }
            set { SetWrappedProperty(value, "Color", PropertyInfo.Undoable); }
        }

        #endregion
    }
}

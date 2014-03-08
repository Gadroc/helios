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
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using GadrocsWorkshop.Helios.PlugIns.LuaUserControl;
    using GadrocsWorkshop.Helios.Runtime;
    using GadrocsWorkshop.Helios.ViewModel;
    using GadrocsWorkshop.Helios.Visuals;

    public class UserControlViewModel : WrapViewModel<LuaUserControl>
    {
        private bool _new;
        private ObservableCollection<object> _visualTree;
        private ObservableCollection<VisualViewModel> _visuals;
        private ControlInstance _previewInstance;

        #region Commands

        public ICommand AddRectangleCommand { get; private set; }
        public ICommand AddImageCommand { get; private set; }
        public ICommand AddContainerCommand { get; private set; }

        /// <summary>
        /// Adds a rectangle visual to this control.
        /// </summary>
        /// <param name="sender"></param>
        private void AddRectangle(object sender)
        {
            RectangleVisual visual = new RectangleVisual();
            visual.Id = "Rectangle";
            visual.Top = 0;
            visual.Left = 0;
            visual.Width = 100;
            visual.Height = 100;
            visual.Color = new Color(255, 0, 0);

            AddUniqueIdChild(FindSelectedVisual(Children), visual);
        }

        private void AddImage(object sender)
        {
        }

        private void AddContainer(object sender)
        {
        }

        /// <summary>
        /// Adds a new visual child but giving it a unique id across the entire control.
        /// </summary>
        /// <param name="parent">Parent to add this visual to.  Null will add visual directly to the control.</param>
        /// <param name="visual">Visual which will be added to the control.</param>
        private void AddUniqueIdChild(VisualViewModel parent, Visual visual)
        {
            ObservableCollection<VisualViewModel> peers = parent == null ? Children : parent.Children;

            int count = 1;
            while (NameExists(Children, visual.Id + count))
            {
                count++;
            }
            visual.Id += count;

            VisualViewModel viewModel = new VisualViewModel(visual, this);
            peers.Add(viewModel);

            viewModel.IsSelected = true;
            if (parent != null)
            {
                parent.IsExpanded = true;
            }

            // Add to control
            if (parent == null)
            {
                Model.Visuals.Add(visual);
            }
            else
            {
                parent.Model.Children.Add(visual);
            }
            UpdatePreviewInstance();
        }

        /// <summary>
        ///  Checks to see if a name already exists on a visual in this control.
        /// </summary>
        /// <param name="peers">Peers to check.</param>
        /// <param name="name">Name to check for.</param>
        /// <returns>True the name was found, false if it was nout found.</returns>
        private bool NameExists(ObservableCollection<VisualViewModel> peers, string name)
        {
            foreach (VisualViewModel peer in peers)
            {
                if (peer.Id.Equals(name, StringComparison.CurrentCultureIgnoreCase)) 
                {
                    return true;
                }
                if (NameExists(peer.Children, name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds the currently selected visual.
        /// </summary>
        /// <param name="peers">Peers to check for selection.</param>
        /// <returns>Viewmodel object representing selected visual, null if none are selected.</returns>
        private VisualViewModel FindSelectedVisual(ObservableCollection<VisualViewModel> peers)
        {
            VisualViewModel retValue = null;

            foreach (VisualViewModel child in peers)
            {
                if (child.IsSelected)
                {
                    retValue = child;
                }
                else
                {
                    retValue = FindSelectedVisual(child.Children);
                }

                if (retValue != null)
                {
                    break;
                }
            }

            return retValue;
        }

        #endregion

        /// <summary>
        /// Creates a new usercontrol.
        /// </summary>
        public UserControlViewModel() : this(new LuaUserControl())
        {
            // Set Model Defaults
            // TODO Pull Model Defaults from user settings.

            Model.Width = 300;
            Model.Height = 300;
            Model.TypeId = "GadrocsWorkshop.Helios.NewControl";
            Model.TypeName = "New Control";

            _new = true;
        }

        /// <summary>
        /// Constructor to edit an existing control.
        /// </summary>
        /// <param name="control"></param>
        public UserControlViewModel(LuaUserControl control)
        {
            Model = control;
            _visuals = new ObservableCollection<VisualViewModel>();

            _visualTree = new ObservableCollection<object>();
            _visualTree.Add(this);

            AddRectangleCommand = new RelayCommand(AddRectangle);
            AddImageCommand = new RelayCommand(AddImage);
            AddContainerCommand = new RelayCommand(AddContainer);

            UpdatePreviewInstance();
        }

        private void UpdatePreviewInstance()
        {
            ControlInstance instance = new ControlInstance(Model);

            instance.Width = Model.Width;
            instance.Height = Model.Height;

            PreviewInstance = instance;
        }

        public ObservableCollection<VisualViewModel> Children
        {
            get { return _visuals; }
        }

        public ObservableCollection<object> VisualTree
        {
            get { return _visualTree; }
        }

        public ControlInstance PreviewInstance
        {
            get { return _previewInstance; }
            set { SetProperty(ref _previewInstance, value, "PreviewInstance", PropertyInfo.None); }
        }

        public override bool IsExpanded
        {
            get
            {
                return true;
            }
            set
            {
                base.IsExpanded = true;
            }
        }

        public override string DisplayName
        {
            get { return TypeName; }
        }

        public bool IsNew
        {
            get { return _new; }
        }

        public string TypeId
        {
            get { return Model.TypeId; }
            set { SetWrappedProperty(value, "TypeId", PropertyInfo.Undoable); }
        }

        public string TypeName
        {
            get { return Model.TypeName; }
            set { SetWrappedProperty(value, "TypeName", PropertyInfo.Undoable | PropertyInfo.DisplayName); }
        }

        public string Description
        {
            get { return Model.TypeDescription; }
            set { SetWrappedProperty(value, "TypeDescription", PropertyInfo.Undoable); }
        }

        public int Width
        {
            get { return Model.Width; }
            set { SetWrappedProperty(value, "Width", PropertyInfo.Undoable); UpdatePreviewInstance(); }
        }

        public int Height
        {
            get { return Model.Height; }
            set { SetWrappedProperty(value, "Height", PropertyInfo.Undoable); UpdatePreviewInstance(); }
        }
    }
}

//  Copyright 2014 Craig Courtney
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

namespace GadrocsWorkshop.Helios.ProfileEditor
{
    using GadrocsWorkshop.Helios.ProfileEditor.UndoEvents;
    using GadrocsWorkshop.Helios.ProfileEditor.ViewModel;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public enum BindingPanelType
    {
        Input,
        Output
    }

    /// <summary>
    /// Interaction logic for InputBindingsPanel.xaml
    /// </summary>
    public partial class BindingsPanel : UserControl
    {
        private HeliosProfile _actionTriggerProfile = null;
        private ProfileExplorerTreeItem _actionsList = null;
        private ProfileExplorerTreeItem _triggerList = null;

        static BindingsPanel()
        {
            Type ownerType = typeof(BindingsPanel);

            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ApplicationCommands.Delete, Delete_Executed, Delete_CanExecute));
        }

        public BindingsPanel()
        {
            InitializeComponent();

            ProfileExplorerItems = new ProfileExplorerTreeItemCollection();
            LoadBindings(this);
            LoadSources(this);
        }

        #region Properties

        public BindingPanelType BindingType
        {
            get { return (BindingPanelType)GetValue(BindingTypeProperty); }
            set { SetValue(BindingTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindingType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingTypeProperty =
            DependencyProperty.Register("BindingType", typeof(BindingPanelType), typeof(BindingsPanel), new PropertyMetadata(BindingPanelType.Input, OnBindingTypeChanged));


        public ProfileExplorerTreeItemCollection ProfileExplorerItems
        {
            get { return (ProfileExplorerTreeItemCollection)GetValue(ProfileExplorerItemsProperty); }
            set { SetValue(ProfileExplorerItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProfileExplorerItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfileExplorerItemsProperty =
            DependencyProperty.Register("ProfileExplorerItems", typeof(ProfileExplorerTreeItemCollection), typeof(BindingsPanel), new PropertyMetadata(null));

        public ProfileExplorerTreeItemCollection BindingItems
        {
            get { return (ProfileExplorerTreeItemCollection)GetValue(BindingItemsProperty); }
            set { SetValue(BindingItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindingItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingItemsProperty =
            DependencyProperty.Register("BindingItems", typeof(ProfileExplorerTreeItemCollection), typeof(BindingsPanel), new PropertyMetadata(null));

        public HeliosObject BindingObject
        {
            get { return (HeliosObject)GetValue(BindingObjectProperty); }
            set { SetValue(BindingObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Profile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingObjectProperty =
            DependencyProperty.Register("BindingObject", typeof(HeliosObject), typeof(BindingsPanel), new PropertyMetadata(null, OnBindingFocusChanged));

        public HeliosBinding SelectedBinding
        {
            get { return (HeliosBinding)GetValue(SelectedBindingProperty); }
            set { SetValue(SelectedBindingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBinding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBindingProperty =
            DependencyProperty.Register("SelectedBinding", typeof(HeliosBinding), typeof(BindingsPanel), new PropertyMetadata(null));

        public StaticValueEditor ValueEditor
        {
            get { return (StaticValueEditor)GetValue(ValueEditorProperty); }
            set { SetValue(ValueEditorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueEditor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueEditorProperty =
            DependencyProperty.Register("ValueEditor", typeof(StaticValueEditor), typeof(BindingsPanel), new PropertyMetadata(null));

        #endregion

        private static void OnBindingFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            BindingsPanel p = d as BindingsPanel;
            LoadBindings(p);
            LoadSources(p);
        }

        private static void OnBindingTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            BindingsPanel p = d as BindingsPanel;
            LoadBindings(p);
            LoadSources(p);
        }

        private static void LoadBindings(BindingsPanel p)
        {
            if (p.BindingObject != null)
            {
                if (p.BindingItems != null)
                {
                    p.BindingItems.Disconnect();
                }
                p.BindingItems = new ProfileExplorerTreeItemCollection();

                if (p.BindingType == BindingPanelType.Input)
                {
                    ProfileExplorerTreeItem inputBindings = new ProfileExplorerTreeItem(p.BindingObject, ProfileExplorerTreeItemType.Action | ProfileExplorerTreeItemType.Binding);
                    if (inputBindings.HasChildren)
                    {
                        foreach (ProfileExplorerTreeItem item in inputBindings.Children)
                        {
                            p.BindingItems.Add(item);
                        }
                    }
                }
                else if (p.BindingType == BindingPanelType.Output)
                {
                    ProfileExplorerTreeItem outputBindings = new ProfileExplorerTreeItem(p.BindingObject, ProfileExplorerTreeItemType.Trigger | ProfileExplorerTreeItemType.Binding);
                    if (outputBindings.HasChildren)
                    {
                        foreach (ProfileExplorerTreeItem item in outputBindings.Children)
                        {
                            p.BindingItems.Add(item);
                        }
                    }
                }
            }
            else
            {
                p.BindingItems = null;
            }
        }

        private static void LoadSources(BindingsPanel p)
        {
            ProfileExplorerTreeItemType triggerTypes = ProfileExplorerTreeItemType.Interface | ProfileExplorerTreeItemType.Monitor | ProfileExplorerTreeItemType.Panel | ProfileExplorerTreeItemType.Visual | ProfileExplorerTreeItemType.Trigger;
            ProfileExplorerTreeItemType actionTypes = ProfileExplorerTreeItemType.Interface | ProfileExplorerTreeItemType.Monitor | ProfileExplorerTreeItemType.Panel | ProfileExplorerTreeItemType.Visual | ProfileExplorerTreeItemType.Action;

            if (p.BindingObject != null)
            {
                if (p.BindingObject.Profile != p._actionTriggerProfile)
                {
                    HeliosProfile newProfile = p.BindingObject.Profile;

                    if (p._actionsList != null)
                    {
                        p._actionsList.Disconnect();
                    }
                    if (p._triggerList != null)
                    {
                        p._triggerList.Disconnect();
                    }

                    p._actionsList = new ProfileExplorerTreeItem(newProfile, actionTypes);
                    p._triggerList = new ProfileExplorerTreeItem(newProfile, triggerTypes);
                    p._actionTriggerProfile = newProfile;
                }
            }

            if (p.BindingType == BindingPanelType.Input)
            {
                if (p._triggerList == null)
                {
                    p.ProfileExplorerItems = null;
                }
                else
                {
                    p.ProfileExplorerItems = p._triggerList.Children;
                }
            }
            else if (p.BindingType == BindingPanelType.Output)
            {
                if (p._actionsList == null)
                {
                    p.ProfileExplorerItems = null;
                }
                else
                {
                    p.ProfileExplorerItems = p._actionsList.Children;
                }
            }
        }

        private void UpdateValueEditor()
        {
            StaticValueEditor editor = null;

            if (BindingObject != null && SelectedBinding != null && SelectedBinding.ValueSource != BindingValueSources.TriggerValue)
            {
                if (SelectedBinding.Action.ValueEditorType == null || SelectedBinding.ValueSource == BindingValueSources.LuaScript)
                {
                    editor = new TextStaticEditor();
                }
                else
                {
                    editor = (StaticValueEditor)Activator.CreateInstance(SelectedBinding.Action.ValueEditorType);
                }

                Binding bind = new Binding("SelectedBinding.Value");
                editor.Profile = BindingObject.Profile;
                editor.StaticValue = SelectedBinding.Value;

                bind.Source = this;
                bind.Mode = BindingMode.TwoWay;
                bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                editor.SetBinding(StaticValueEditor.StaticValueProperty, bind);
            }

            ValueEditor = editor;
        }

        private void BindingsTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ProfileExplorerTreeItem selectedItem = e.NewValue as ProfileExplorerTreeItem;
            if (selectedItem != null && selectedItem.ItemType.HasFlag(ProfileExplorerTreeItemType.Binding))
            {
                SelectedBinding = (HeliosBinding)selectedItem.ContextItem;
                UpdateValueEditor();
            }
            else
            {
                SelectedBinding = null;
            }
        }

        private void ValueSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateValueEditor();
        }

        #region Command Handlers

        private static void Delete_CanExecute(object target, CanExecuteRoutedEventArgs e)
        {
            BindingsPanel editor = target as BindingsPanel;
            HeliosBinding binding = e.Parameter as HeliosBinding;
            if (editor != null)
            {
                e.Handled = true;
                e.CanExecute = binding != null || editor.SelectedBinding != null;
            }
        }

        private static void Delete_Executed(object target, ExecutedRoutedEventArgs e)
        {
            BindingsPanel editor = target as BindingsPanel;
            HeliosBinding binding = e.Parameter as HeliosBinding;
            if (editor != null)
            {
                BindingDeleteUndoEvent undo;
                if (binding != null)
                {
                    undo = new BindingDeleteUndoEvent(binding);
                }
                else
                {
                    undo = new BindingDeleteUndoEvent(editor.SelectedBinding);
                }
                undo.Do();
                ConfigManager.UndoManager.AddUndoItem(undo);
                e.Handled = true;
            }
        }

        #endregion

    }
}

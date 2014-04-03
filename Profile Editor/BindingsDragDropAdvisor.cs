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
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    class BindingsDragDropAdvisor : IDropTargetAdvisor, IDragSourceAdvisor
    {
        TreeView _targetTreeView = null;
        TreeView _sourceTreeView = null;

        public System.Windows.UIElement TargetUI
        {
            get
            {
                return _targetTreeView;
            }
            set
            {
                _targetTreeView = value as TreeView;
            }
        }

        public bool ApplyMouseOffset
        {
            get { return false; }
        }

        public Point GetVisualFeedbackLocation(Point location)
        {            
            return location;
        }

        public bool IsValidDataObject(IDataObject Object, Point dropPoint)
        {
            ProfileExplorerTreeItem dropTarget = GetExplorerTreeItem(_targetTreeView, dropPoint);
            if (dropTarget != null)
            {
                if (Object.GetDataPresent("Helios.Trigger") && dropTarget.ItemType.HasFlag(ProfileExplorerTreeItemType.Action))
                {
                    return true;
                }

                if (Object.GetDataPresent("Helios.Action") && dropTarget.ItemType.HasFlag(ProfileExplorerTreeItemType.Trigger))
                {
                    return true;
                }
            }
            return false;
        }

        public void OnDropCompleted(IDataObject obj, Point dropPoint)
        {
            ProfileExplorerTreeItem dropItem = GetExplorerTreeItem(_targetTreeView, dropPoint);

            IBindingAction action;
            IBindingTrigger trigger;

            if (dropItem.ItemType.HasFlag(ProfileExplorerTreeItemType.Trigger))
            {
                action = (IBindingAction)obj.GetData("Helios.Action");
                trigger = (IBindingTrigger)dropItem.ContextItem;
            }
            else
            {
                trigger = (IBindingTrigger)obj.GetData("Helios.Trigger");
                action = (IBindingAction)dropItem.ContextItem;
            }

            AddNewBinding(trigger, action);

            ProfileExplorerTreeItem newTreeItem = dropItem.Children.Last();
            newTreeItem.IsSelected = true;
        }

        private void AddNewBinding(IBindingTrigger trigger, IBindingAction action)
        {
            HeliosBinding binding = new HeliosBinding(trigger, action);

            if (action.ActionRequiresValue && (ConfigManager.ModuleManager.CanConvertUnit(trigger.Unit, action.Unit)))
            {
                binding.ValueSource = BindingValueSources.TriggerValue;
            }
            else
            {
                binding.ValueSource = BindingValueSources.StaticValue;
            }

            BindingAddUndoEvent undoEvent = new BindingAddUndoEvent(binding);
            undoEvent.Do();
            ConfigManager.UndoManager.AddUndoItem(undoEvent);
        }


        public UIElement GetVisualFeedback(IDataObject obj)
        {
            return null;
        }

        public UIElement GetTargetTopContainer()
        {
            return _targetTreeView;
        }

        private ProfileExplorerTreeItem GetExplorerTreeItem(UIElement dragElement)
        {
            TreeViewItem treeItem = GetAncestor(dragElement, typeof(TreeViewItem)) as TreeViewItem;
            if (treeItem != null)
            {
                return treeItem.Header as ProfileExplorerTreeItem;
            }
            return null;
        }

        private ProfileExplorerTreeItem GetExplorerTreeItem(TreeView treeView, Point location)
        {
            IInputElement targetElement = treeView.InputHitTest(location);
            return GetExplorerTreeItem((UIElement)targetElement);
        }

        private DependencyObject GetAncestor(UIElement element, Type parentType)
        {
            DependencyObject item = element;
            while (item != null && item.GetType() != parentType)
            {
                item = VisualTreeHelper.GetParent(item);
            }
            return item;
        }

        public UIElement SourceUI
        {
            get
            {
                return _sourceTreeView;
            }
            set
            {
                _sourceTreeView = value as TreeView;
            }
        }

        public DragDropEffects SupportedEffects
        {
            get { return DragDropEffects.Copy; }
        }

        public DataObject GetDataObject(UIElement draggedElement, Point location)
        {
            DataObject obj = null;
            ProfileExplorerTreeItem item = GetExplorerTreeItem(_sourceTreeView, location);
            if (item != null)
            {
                obj = new DataObject();
                string dataType = "Helios." + item.ItemType.ToString();
                obj.SetData(dataType, item.ContextItem);
            }

            return obj;
        }

        public void FinishDrag(UIElement draggedElement, Point location, DragDropEffects finalEffects)
        {
            // No-Op
        }

        public bool IsDraggable(UIElement dragElement, Point location)
        {
            ProfileExplorerTreeItem item = GetExplorerTreeItem(_sourceTreeView, location);
            return item != null && (item.ItemType.HasFlag(ProfileExplorerTreeItemType.Action) || item.ItemType.HasFlag(ProfileExplorerTreeItemType.Trigger));
        }

        public UIElement GetSourceTopContainer()
        {
            return _sourceTreeView;
        }
    }
}

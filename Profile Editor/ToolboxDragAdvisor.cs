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
    using GadrocsWorkshop.Helios.ProfileEditor.ViewModel;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    class ToolboxDragAdvisor : IDragSourceAdvisor
    {
        private ItemsControl _itemsControl;
        private UIElement _source;

        public UIElement SourceUI
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                _itemsControl = GetAncestor(_source, typeof(ItemsControl)) as ItemsControl;
            }
        }

        public DragDropEffects SupportedEffects
        {
            get { return DragDropEffects.Copy; }
        }

        public DataObject GetDataObject(UIElement draggedElement, Point location)
        {
            DataObject data = new DataObject();
            ToolboxItem item = GetToolboxItem(draggedElement);
            if (item != null)
            {
                item.IsBeingDragged = true;
                data.SetData("Helios.Visual", item.CreateControl());
            }
            return data;
        }

        public void FinishDrag(UIElement draggedElement, Point location, DragDropEffects finalEffects)
        {
            ToolboxItem item = GetToolboxItem(draggedElement);
            if (item != null)
            {
                item.IsBeingDragged = false;
            }
        }

        public bool IsDraggable(UIElement dragElement, Point location)
        {
            return GetToolboxItem(dragElement) != null;
        }

        public UIElement GetSourceTopContainer()
        {
            return _source;
        }

        private ToolboxItem GetToolboxItem(UIElement dragElement)
        {
            ToolboxItem item = null;
            ContentPresenter itemPresenter = _itemsControl.ContainerFromElement(dragElement) as ContentPresenter;
            if (itemPresenter != null)
            {
                item = itemPresenter.Content as ToolboxItem;
            }
            return item;
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
    }
}

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
    using System.Windows;
    using System.Windows.Controls;

    class ProfileExplorerItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ProfileExplorerTreeItem listItem = item as ProfileExplorerTreeItem;
            FrameworkElement element = container as FrameworkElement;
            if (listItem != null && element != null)
            {
                if (listItem.ItemType == ProfileExplorerTreeItemType.Binding)
                {
                    if (listItem.Parent.ItemType == ProfileExplorerTreeItemType.Action)
                    {
                        return element.FindResource("InputBindingNodeTemplate") as DataTemplate;
                    }
                    else
                    {
                        return element.FindResource("OutputBindingNodeTemplate") as DataTemplate;
                    }
                }
                else if (listItem.ItemType == ProfileExplorerTreeItemType.Action)
                {
                    return element.FindResource("ActionNodeTemplate") as DataTemplate;
                }
                else if (listItem.ItemType == ProfileExplorerTreeItemType.Trigger)
                {
                    return element.FindResource("TriggerNodeTemplate") as DataTemplate;
                }
                else
                {
                    return element.FindResource("ContainerNodeTemplate") as DataTemplate;
                }
            }
            return null;
        }  
    }
}

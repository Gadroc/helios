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

namespace GadrocsWorkshop.Helios.PlugInExplorer
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class InfoTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is PluginTreeItem)
            {
                PluginTreeItem treeitem = item as PluginTreeItem;

                if (treeitem.ItemType == PluginTreeItem.TreeItemType.Display)
                    return element.FindResource("DisplayData") as DataTemplate;
                else if (treeitem.ItemType == PluginTreeItem.TreeItemType.Folder)
                    return element.FindResource("EmptyData") as DataTemplate;
                else
                    return element.FindResource("GeneralData") as DataTemplate;
            }

            return null;
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;

namespace GadrocsWorkshop.Helios.PlugInExplorer
{
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

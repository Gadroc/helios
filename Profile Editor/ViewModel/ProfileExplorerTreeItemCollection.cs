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

namespace GadrocsWorkshop.Helios.ProfileEditor.ViewModel
{
    using GadrocsWorkshop.Helios.Collections;

    public class ProfileExplorerTreeItemCollection :  NoResetObservablecollection<ProfileExplorerTreeItem> // KeyedObservableCollection<string, ProfileExplorerTreeItem>
    {
        //public override string GetKeyForItem(ProfileExplorerTreeItem item)
        //{
        //    if (item.ItemType.HasFlag(ProfileExplorerTreeItemType.Binding))
        //    {
        //        return item.ContextItem.GetHashCode().ToString();
        //    }
        //    else
        //    {
        //        item.PropertyChanged += Item_PropertyChanged;
        //    }
        //    return item.Name;
        //}

        //void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    PropertyNotificationEventArgs args = e as PropertyNotificationEventArgs;
        //    if (args != null && e.PropertyName.Equals("Name"))
        //    {
        //        KeyChanged(args.OldValue as string, args.NewValue as string);
        //    }
        //}

        //protected override void RemoveItem(int index)
        //{
        //    this[index].PropertyChanged -= Item_PropertyChanged;
        //    base.RemoveItem(index);
        //}

        public void Disconnect()
        {
            foreach (ProfileExplorerTreeItem item in this)
            {
                item.Disconnect();
            }
        }
    }
}

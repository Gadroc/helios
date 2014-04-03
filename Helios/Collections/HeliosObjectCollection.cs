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

namespace GadrocsWorkshop.Helios.Collections
{
    using GadrocsWorkshop.Helios.ComponentModel;

    public class HeliosObjectCollection<T> : KeyedObservableCollection<string, T> where T : HeliosObject
    {
        protected override void InsertItem(int index, T item)
        {
            item.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Item_PropertyChanged);

            base.InsertItem(index, item);
        }

        public string GetUniqueName(T item)
        {
            int addValue = 0;
            string checkName = item.Name;
            while (ContainsKey(checkName))
            {
                addValue++;
                checkName = item.Name + " " + addValue;
            }

            return checkName;
        }

        protected override void RemoveItem(int index)
        {
            this[index].PropertyChanged -= Item_PropertyChanged;
            base.RemoveItem(index);
        }

        void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyNotificationEventArgs args = e as PropertyNotificationEventArgs;
            if (args != null && e.PropertyName.Equals("Name"))
            {
                T item = sender as T;
                if (item != null)
                {                    
                    KeyChanged((string)args.OldValue, item.Name);
                }
            }
        }

        public override string GetKeyForItem(T item)
        {
            return item.Name;
        }
    }
}

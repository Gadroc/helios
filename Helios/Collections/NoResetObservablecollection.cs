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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    
    public class NoResetObservablecollection<T> : ObservableCollection<T>
    {
        public void Add(NoResetObservablecollection<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public void Remove(NoResetObservablecollection<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public void AddSlave(NoResetObservablecollection<T> items)
        {
            Add(items);
            items.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(SlaveItems_CollectionChanged);
            items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(SlaveItems_CollectionChanged);
        }

        public void RemoveSlave(NoResetObservablecollection<T> items)
        {
            Remove(items);
            items.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(SlaveItems_CollectionChanged);
        }

        void SlaveItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove ||
                e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                foreach (T item in e.OldItems)
                {
                    Remove(item);
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add ||
                e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                foreach (T item in e.NewItems)
                {
                    Add(item);
                }
            }
        }

        /// <summary>
        /// Clears all items in the collection by removing them individually.
        /// </summary>
        protected override void ClearItems()
        {
            IList<T> items = new List<T>(this);
            foreach (T item in items)
            {
                Remove(item);
            }
        }
    }
}

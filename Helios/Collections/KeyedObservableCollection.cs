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

    public abstract class KeyedObservableCollection<K,T> : NoResetObservablecollection<T>
    {
        private Dictionary<K, T> _dictionary = new Dictionary<K, T>();

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    foreach (T item in e.NewItems)
                    {
                        _dictionary.Add(GetKeyForItem(item), item);
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    foreach (T item in e.OldItems)
                    {
                        _dictionary.Remove(GetKeyForItem(item));
                    }
                    break;

                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                    foreach (T item in e.OldItems)
                    {
                        _dictionary.Remove(GetKeyForItem(item));
                    }
                    foreach (T item in e.NewItems)
                    {
                        _dictionary.Add(GetKeyForItem(item), item);
                    }
                    break;
            }
            base.OnCollectionChanged(e);
        }

        public T this[K key]
        {
            get
            {
                return _dictionary[key];
            }
        }

        public bool ContainsKey(K key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void RemoveKey(K key)
        {
            T item = _dictionary[key];
            Remove(item);
        }

        public abstract K GetKeyForItem(T item);

        protected void KeyChanged(K oldKey, K newKey)
        {
            if (_dictionary.ContainsKey(oldKey))
            {
                T item = _dictionary[oldKey];
                _dictionary.Remove(oldKey);
                if (!_dictionary.ContainsKey(newKey))
                {
                    _dictionary.Add(newKey, item);
                }
                else
                {
                    ConfigManager.LogManager.LogError("Duplicate item keys in collection, some data may be lost.");
                }
            }
        }
    }
}

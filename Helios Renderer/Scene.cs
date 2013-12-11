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

using System;
using System.Collections.Generic;

namespace GadrocsWorkshop.Helios.Renderer
{

    internal delegate void SceneChangeHandler(object sender, ControlState control);

    /// <summary>
    /// Scene of controls which will be rendered
    /// </summary>
    internal class Scene : IList<ControlState>
    {
        private List<ControlState> _list;

        /// <summary>
        /// Event triggered when a new control is added to the scene.
        /// </summary>
        public event SceneChangeHandler ControlAdded;

        /// <summary>
        /// Event triggered when a control is removed from the scene.
        /// </summary>
        public event SceneChangeHandler ControlRemoved;

        public Scene()
        {
            _list = new List<ControlState>();
        }

        public int IndexOf(ControlState item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, ControlState item)
        {
            _list.Insert(index, item);
            OnControlAdded(item);
        }

        public void RemoveAt(int index)
        {
            ControlState oldControl = _list[index];
            _list.RemoveAt(index);
            OnControlRemoved(oldControl);
        }

        public ControlState this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                ControlState oldControl = _list[index];
                _list[index] = value;
                OnControlRemoved(oldControl);
                OnControlAdded(value);
            }
        }

        public void Add(ControlState item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            foreach (ControlState control in _list)
            {
                OnControlRemoved(control);
            }
            _list.Clear();
        }

        public bool Contains(ControlState item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(ControlState[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(ControlState item)
        {
            bool retValue = _list.Remove(item);
            OnControlRemoved(item);
            return retValue;
        }

        public IEnumerator<ControlState> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)_list).GetEnumerator();
        }

        private void OnControlAdded(ControlState control)
        {
            SceneChangeHandler handler = ControlAdded;
            if (handler != null)
            {
                handler.Invoke(this, control);
            }
        }

        private void OnControlRemoved(ControlState control)
        {
            SceneChangeHandler handler = ControlRemoved;
            if (handler != null)
            {
                handler.Invoke(this, control);
            }
        }
    }
}

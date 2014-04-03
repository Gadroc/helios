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

namespace GadrocsWorkshop.Helios
{
    using System;
    using System.Windows;

    using GadrocsWorkshop.Helios.Collections;

    public class HeliosVisualCollection : HeliosObjectCollection<HeliosVisual>
    {
        private Rect _rectangle = Rect.Empty;

        public event EventHandler CollectionSizeChanged;

        public void MoveUp(HeliosVisual item)
        {
            int index = IndexOf(item);
            int newIndex = index + 1;
            if (newIndex < Count)
            {
                Move(index, newIndex);
            }
        }

        public void MoveDown(HeliosVisual item)
        {
            int index = IndexOf(item);
            int newIndex = index - 1;
            if (newIndex >= 0)
            {
                Move(index, newIndex);
            }
        }

        /// <summary>
        /// Returns the rectangle which encloses all the visuals in this colleciton.
        /// </summary>
        public Rect Rectangle
        {
            get
            {
                if (_rectangle == Rect.Empty && Count > 0)
                {
                    _rectangle = this[0].DisplayRectangle;
                    foreach (HeliosVisual visual in this)
                    {
                        _rectangle.Union(visual.DisplayRectangle);
                    }
                }
                return _rectangle;
            }
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (HeliosVisual item in e.OldItems)
                {
                    item.Moved -= new EventHandler(Item_ResizeMove);
                    item.Resized -= new EventHandler(Item_ResizeMove);
                }
            }

            if (e.NewItems != null)
            {
                foreach (HeliosVisual item in e.NewItems)
                {
                    item.Moved += new EventHandler(Item_ResizeMove);
                    item.Resized += new EventHandler(Item_ResizeMove);
                }
            }

            OnRectangleUpdate();

            base.OnCollectionChanged(e);
        }

        void Item_ResizeMove(object sender, EventArgs e)
        {
            OnRectangleUpdate();
        }

        private void OnRectangleUpdate()
        {
            _rectangle = Rect.Empty;
            EventHandler handler = CollectionSizeChanged;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

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

namespace GadrocsWorkshop.Helios.ProfileEditor.UndoEvents
{
    using System.Collections.Generic;

    class ControlDeleteUndoEvent : IUndoItem
    {
        private HeliosVisualContainer _container;
        private List<HeliosVisual> _controls;
        private List<int> _indexes;

        public ControlDeleteUndoEvent(HeliosVisualContainer container, List<HeliosVisual> controls, List<int> indexes)
        {
            _container = container;
            _controls = controls;
            _controls.Reverse();
            _indexes = indexes;
            _indexes.Reverse();
        }

        #region IUndoEvent Members

        public void Undo()
        {
            for (int i = 0; i < _controls.Count; i++)
            {
                if (_indexes[i] < _container.Children.Count)
                {
                    _container.Children.Insert(_indexes[i], _controls[i]);
                }
                else
                {
                    _container.Children.Add(_controls[i]);
                }
            }
        }

        public void Do()
        {
            for (int i = 0; i < _controls.Count; i++)
            {
                _container.Children.Remove(_controls[i]);
            }
        }

        #endregion
    }
}

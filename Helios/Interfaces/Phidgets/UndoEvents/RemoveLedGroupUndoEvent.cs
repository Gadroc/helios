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

namespace GadrocsWorkshop.Helios.Interfaces.Phidgets.UndoEvents
{
    using System;

    class RemoveLedGroupUndoEvent : IUndoItem
    {
        private LedGroupCollection _container;
        private LEDGroup _group;
        private int _index;

        public RemoveLedGroupUndoEvent(LedGroupCollection container, LEDGroup group, int index)
        {
            _container = container;
            _group = group;
            _index = index;
        }

        #region IUndoEvent Members

        public void Undo()
        {
            _container.Insert(_index, _group);
        }

        public void Do()
        {
            _container.Remove(_group);
        }

        #endregion
    }
}

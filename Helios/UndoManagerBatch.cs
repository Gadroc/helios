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
    using System.Collections.Generic;

    class UndoManagerBatch : List<IUndoItem>, IUndoItem
    {
        public void Undo()
        {
            for(int i=Count - 1; i > -1 && i<Count; i--)
            {
                this[i].Undo();
            }
        }

        public void Do()
        {
            foreach (IUndoItem item in this)
            {
                item.Do();
            }
        }
    }
}

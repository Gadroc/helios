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

namespace GadrocsWorkshop.Helios.Interfaces.Phidgets
{
    using GadrocsWorkshop.Helios.Collections;
    using System;

    public class LedGroupsViewModel : NoResetObservablecollection<LedGroupViewModel>
    {
        private PhidgetLEDBoard _board;

        public LedGroupsViewModel(PhidgetLEDBoard board)
        {
            _board = board;
            foreach (LEDGroup group in _board.LedGroups)
            {
                Add(new LedGroupViewModel(_board, group));
            }

            _board.LedGroups.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(LedGroups_CollectionChanged);
        }

        void LedGroups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (LEDGroup group in e.OldItems)
                {
                    if (e.NewItems == null || !e.NewItems.Contains(group))
                    {
                        LedGroupViewModel model = GetViewModelForGroup(group);
                        if (model != null)
                        {
                            Remove(model);
                        }
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (LEDGroup group in e.NewItems)
                {
                    LedGroupViewModel model = GetViewModelForGroup(group);
                    if (model == null)
                    {
                        Add(new LedGroupViewModel(_board, group));
                    }
                }
            }
        }

        private LedGroupViewModel GetViewModelForGroup(LEDGroup group)
        {
            foreach (LedGroupViewModel groupModel in this)
            {
                if (groupModel.Group == group)
                {
                    return groupModel;
                }
            }
            return null;
        }
    }
}

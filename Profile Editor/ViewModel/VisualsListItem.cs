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
    public class VisualsListItem : NotificationObject
    {
        private HeliosVisual _control;
        private bool _selected;

        public VisualsListItem(HeliosVisual control, bool isSelected)
        {
            _control = control;
            _selected = isSelected;
        }

        public HeliosVisual Control { get { return _control; } }

        public bool IsSelected
        {
            get
            {
                return _selected;
            }
            set
            {
                if (!_selected.Equals(value))
                {
                    bool oldValue = _selected;
                    _selected = value;
                    OnPropertyChanged("IsSelected", oldValue, value, false);
                }
            }
        }
    }
}

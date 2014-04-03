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
    using GadrocsWorkshop.Helios.Collections;

    public class ToolboxGroup : NoResetObservablecollection<ToolboxItem>
    {
        private readonly string _name;

        private IDragSourceAdvisor _advisor;

        public ToolboxGroup(string name)
        {
            _name = name;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public IDragSourceAdvisor DragAdvisor
        {
            get
            {
                return _advisor;
            }
            set
            {
                if ((_advisor == null && value != null)
                    || (_advisor != null && !_advisor.Equals(value)))
                {
                    _advisor = value;
                }
            }
        }
    }
}

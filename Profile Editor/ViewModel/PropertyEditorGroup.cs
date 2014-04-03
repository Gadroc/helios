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
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System.Windows.Controls;

    public class PropertyEditorGroup
    {
        private string _name;
        private Control _propertyEditor;

        public PropertyEditorGroup(HeliosPropertyEditor propertyEditor)
        {
            _name = propertyEditor.Category;
            _propertyEditor = propertyEditor;
        }

        public PropertyEditorGroup(string name, Control propertyEditor)
        {
            _name = name;
            _propertyEditor = propertyEditor;
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public Control PropertyEditor
        {
            get
            {
                return _propertyEditor;
            }
        }
    }
}

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

namespace GadrocsWorkshop.Helios.Interfaces.Falcon
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;

    public class FalconKeyCallbackFilter
    {
        public FalconKeyCallbackFilter(ICollectionView filteredView, TextBox textBox)
        {
            string filterText = "";
            if (filteredView != null)
            {
                filteredView.Filter = delegate(object obj)
                {
                    if (String.IsNullOrEmpty(filterText))
                        return true;

                    FalconKeyCallback callback = obj as FalconKeyCallback;
                    int nameIndex = callback.Name.IndexOf(filterText, 0, StringComparison.InvariantCultureIgnoreCase);
                    int descIndex = callback.Description.IndexOf(filterText, 0, StringComparison.InvariantCultureIgnoreCase);
                    return nameIndex > -1 || descIndex > -1;
                };

                textBox.TextChanged += delegate
                {
                    filterText = textBox.Text;
                    filteredView.Refresh();
                };
            }
        }
    }
}

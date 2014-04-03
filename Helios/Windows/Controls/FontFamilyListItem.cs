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

namespace GadrocsWorkshop.Helios.Windows.Controls
{
    using System;
	using System.Windows.Media;
	using System.Globalization;
	
    public class FontFamilyListItem : IComparable
    {
        private string _displayName;
        private FontFamily _fontFamily;

        public FontFamilyListItem(FontFamily fontFamily)
        {
            _displayName = TextFormat.GetFontDisplayName(fontFamily);
            _fontFamily = fontFamily;
        }

        #region Properties

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }
        }

        #endregion

        public override string ToString()
        {
            return _displayName;
        }

        public int CompareTo(object obj)
        {
            return string.Compare(_displayName, obj.ToString(), true, CultureInfo.CurrentCulture);
        }
    }
}

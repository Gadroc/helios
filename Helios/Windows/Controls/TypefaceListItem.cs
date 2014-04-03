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
    using System.Windows;
    using System.Windows.Media;

    public class TypefaceListItem : IComparable
    {
        private string _displayName;
        private Typeface _typeface;
        private bool _simulated;

        public TypefaceListItem(Typeface typeface)
        {
            _displayName = TextFormat.GetTypefaceDisplayName(typeface);
            _typeface = typeface;
            _simulated = typeface.IsBoldSimulated || typeface.IsObliqueSimulated;

            if (_simulated)
            {
                _displayName += " (simulated)";
            }
        }

        #region Properties

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
        }

        public Typeface Typeface
        {
            get
            {
                return _typeface;
            }
        }

        #endregion

        public int CompareTo(object obj)
        {
            TypefaceListItem item = obj as TypefaceListItem;
            if (item == null)
            {
                return -1;
            }

            // Sort all simulated faces after all non-simulated faces.
            if (_simulated != item._simulated)
            {
                return _simulated ? 1 : -1;
            }

            // If weight differs then sort based on weight (lightest first).
            int difference = Typeface.Weight.ToOpenTypeWeight() - item.Typeface.Weight.ToOpenTypeWeight();
            if (difference != 0)
            {
                return difference > 0 ? 1 : -1;
            }

            // If style differs then sort based on style (Normal, Italic, then Oblique).
            FontStyle thisStyle = Typeface.Style;
            FontStyle otherStyle = item.Typeface.Style;

            if (thisStyle != otherStyle)
            {
                if (thisStyle == FontStyles.Normal)
                {
                    // This item is normal style and should come first.
                    return -1;
                }
                else if (otherStyle == FontStyles.Normal)
                {
                    // The other item is normal style and should come first.
                    return 1;
                }
                else
                {
                    // Neither is normal so sort italic before oblique.
                    return (thisStyle == FontStyles.Italic) ? -1 : 1;
                }
            }

            // If stretch differs then sort based on stretch (Normal first, then numerically).
            FontStretch thisStretch = Typeface.Stretch;
            FontStretch otherStretch = item.Typeface.Stretch;

            if (thisStretch != otherStretch)
            {
                if (thisStretch == FontStretches.Normal)
                {
                    // This item is normal stretch and should come first.
                    return -1;
                }
                else if (otherStretch == FontStretches.Normal)
                {
                    // The other item is normal stretch and should come first.
                    return 1;
                }
                else
                {
                    // Neither is normal so sort numerically.
                    return thisStretch.ToOpenTypeStretch() < otherStretch.ToOpenTypeStretch() ? -1 : 0;
                }
            }

            // They're the same.
            return 0;

        }
    }
}

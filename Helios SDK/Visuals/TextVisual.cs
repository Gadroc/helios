//  Copyright 2013 Craig Courtney
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

namespace GadrocsWorkshop.Helios.Visuals
{
    /// <summary>
    /// Renders text onto this control.
    /// </summary>
    public class TextVisual : Visual
    {
        private string _text;

        /// <summary>
        /// Text to be displayed on this control
        /// </summary>
        public string Text { get; set; }

        public override byte VisualType
        {
            get { return 2; }
        }

        public override string VisualTypeName
        {
            get { return "text"; }
        }
    }
}

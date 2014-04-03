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
    public interface IBindingElement
    {
        /// <summary>
        /// Owning object of this binding element.
        /// </summary>
        HeliosObject Owner { get; }

        /// <summary>
        /// Returns the device which this will be listed under.
        /// </summary>
        string Device { get; set; }

        /// <summary>
        /// Returns the name for this value
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Returns a context object for this binding element
        /// </summary>
        object Context { get; set; }

        /// <summary>
        /// Returns units for the values produced and accepted for this binding element.
        /// </summary>
        BindingValueUnit Unit { get; }
    }
}

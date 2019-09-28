//  Copyright 2014 Craig Courtney
//    
//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later versionCannot find interface trigger
//
//  Helios is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GadrocsWorkshop.Helios.Controls;

namespace GadrocsWorkshop.Helios
{
    public class NonClickableZone
    {
        private Rect _pastToChildZone;
        public ToggleSwitchPosition GuardedChildPosition { get; set; }
        public HeliosVisual ChildVisual { get; set; }
        public ToggleSwitchPosition PositionWhenApplicable { get; set; }
        public bool AllPositions { get; set; }

        public NonClickableZone(Rect nonClickableZone, ToggleSwitchPosition position, HeliosVisual childVisual, ToggleSwitchPosition guardedChildPosition)
        {
            _pastToChildZone = nonClickableZone;
            PositionWhenApplicable = position;
            ChildVisual = childVisual;
            GuardedChildPosition = guardedChildPosition;
        }

        public NonClickableZone(Rect nonClickableZone, bool allPosition, HeliosVisual childVisual)
        {
            _pastToChildZone = nonClickableZone;
            AllPositions = allPosition;
            ChildVisual = childVisual;
        }

        #region Properties
        public Rect PastToChildZone
        {
            get
            {
                return _pastToChildZone;
            }

            set
            {
                _pastToChildZone = value;
            }
        }

        public bool isClickInZone(Point location)
        {
            if(_pastToChildZone.Contains(location)) { return true; } else { return false; }
        }
        #endregion
    }
}

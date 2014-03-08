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
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Base class providing common features of all visuals.
    /// </summary>
    public abstract class Visual
    {
        private List<Visual> _children;
        private float _rotationCenterHorizontal;
        private float _rotationCenterVertical;

        public Visual()
        {
            _children = new List<Visual>();
        }

        /// <summary>
        /// Returns the type id for this visual
        /// </summary>
        public abstract byte VisualType { get; }

        /// <summary>
        /// Returns the type id for this visual
        /// </summary>
        public abstract string VisualTypeName { get; }

        /// <summary>
        /// Id used for this visual state manipulation.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Description of this visual.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Flag indicating wheter this visual is static. Static visuals do not
        /// change during runtime. Statics can be optmized 
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Top of the visual.
        /// </summary>
        public float Top { get; set; }

        /// <summary>
        /// Left of the visual.
        /// </summary>
        public float Left { get; set; }

        /// <summary>
        /// Width of this visual.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Height of this visual.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Horizontal point which this visual will be rotated around.
        /// </summary>
        public float RotationCenterVertical { get; set; }

        /// <summary>
        /// Veritcal point which this visual will be rotated around.
        /// </summary>
        public float RotationCenterHorizontal { get; set; }

        /// <summary>
        /// Path to the image for this visual.
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Color to render this visual with.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Children visuals which will be displayed relative to this visual
        /// </summary>
        public IList<Visual> Children
        {
            get { return _children; }
        }
    }
}

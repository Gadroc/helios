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

namespace GadrocsWorkshop.Helios.Runtime
{
    using System;
    using System.Collections.Generic;

    using GadrocsWorkshop.Helios.Visuals;

    /// <summary>
    /// State data for display visuals.  All modifications / reads to a visual state should
    /// be done while holding a lock on it's parent controlinstance.  Not doing so could lead
    /// to incorrect value or rendering.
    /// </summary>
    public class VisualState
    {
        private Visual _visual;
        private List<VisualState> _children;

        private float _width;
        private float _height;
        private float _xoffset;
        private float _yoffset;
        private float _rotation;

        public VisualState(Visual visual)
        {
            IsMatrixDirty = true;
            IsSizeDirty = true;

            _visual = visual;
            _children = new List<VisualState>();

            Width = visual.Width;
            Height = visual.Height;
            Color = visual.Color;
        }

        /// <summary>
        /// Visual that this state is for
        /// </summary>
        public Visual Visual
        {
            get
            {
                return _visual;
            }
        }

        /// <summary>
        /// Children visualstates for this visualstate
        /// </summary>
        public IEnumerable<VisualState> Children
        {
            get
            {
                return _children;
            }
        }

        /// <summary>
        /// Flag indicating whether this visual states matrix needs regenerated
        /// </summary>
        public bool IsMatrixDirty { get; set; }

        /// <summary>
        /// Flag indicating whether this visual states size has changed
        /// </summary>
        public bool IsSizeDirty { get; set; }

        /// <summary>
        /// Returns the path to the image for this visual.
        /// </summary>
        public string ImagePath
        {
            get
            {
                return Visual.ImagePath;
            }
        }

        /// <summary>
        /// Top of the visual.
        /// </summary>
        public float Top
        {
            get
            {
                return Visual.Top + YOffset;
            }
        }

        /// <summary>
        /// Left of the visual.
        /// </summary>
        public float Left
        {
            get
            {
                return Visual.Left + XOffset;
            }
        }

        /// <summary>
        /// Width of this visual.
        /// </summary>
        public float Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        /// <summary>
        /// Height of this visual.
        /// </summary>
        public float Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
            }
        }

        /// <summary>
        /// Horizontal point which this visual will be rotated around.
        /// </summary>
        public float RotationCenterHorizontal
        {
            get
            {
                return Visual.RotationCenterHorizontal;
            }
        }

        /// <summary>
        /// Veritcal point which this visual will be rotated around.
        /// </summary>
        public float RotationCenterVertical
        {
            get
            {
                return Visual.RotationCenterVertical;
            }
        }

        /// <summary>
        /// Indicates whether this control is visible or not.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// The amount of offset from default X position for this visual
        /// </summary>
        public float XOffset 
        {
            get 
            {
                return _xoffset; 
            }
            set
            {
                if (_xoffset != value)
                {
                    _xoffset = value;
                    IsMatrixDirty = true;
                }
            }
        }

        /// <summary>
        /// The amount of offset from default Y position for this visual
        /// </summary>
        public float YOffset
        {
            get
            {
                return _yoffset;
            }
            set
            {
                if (_yoffset != value)
                {
                    _yoffset = value;
                    IsMatrixDirty = true;
                }
            }
        }

        /// <summary>
        /// The amount of rotation for this visual.
        /// </summary>
        public float Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                if (_rotation != value)
                {
                    _rotation = value;
                    IsMatrixDirty = true;
                }
            }
        }

        /// <summary>
        /// Opacity to display this visual with
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// Current color of this visual
        /// </summary>
        /// TODO Need to trigger brush recreate
        public Color Color { get; set; }

        /// <summary>
        /// Text which will be displayed on this visual
        /// </summary>
        /// TODO Need to redraw text
        public string Text { get; set; }
    }
}

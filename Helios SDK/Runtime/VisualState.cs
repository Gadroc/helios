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

    using GadrocsWorkshop.Helios.Visuals;

    /// <summary>
    /// State data for display visuals.
    /// </summary>
    public class VisualState
    {
        private Visual _visual;
        private VisualOverride _overrides;

        public VisualState(Visual visual, VisualOverride overrides = null)
        {
            _visual = visual;
            _overrides = overrides;
            if (overrides != null && overrides.Color != null)
            {
                Color = overrides.Color.Value;
            }
            else
            {
                Color = visual.Color;
            }
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
        /// Returns the path to the image for this visual.
        /// </summary>
        public string ImagePath
        {
            get
            {
                if (_overrides != null & !string.IsNullOrWhiteSpace(_overrides.ImagePath))
                {
                    return _overrides.ImagePath;
                }
                else
                {
                    return Visual.ImagePath;
                }
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
                return Visual.Width;
            }
        }

        /// <summary>
        /// Height of this visual.
        /// </summary>
        public float Height
        {
            get
            {
                return Visual.Height;
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
        public float XOffset { get; set; }

        /// <summary>
        /// The amount of offset from default Y position for this visual
        /// </summary>
        public float YOffset { get; set; }

        /// <summary>
        /// The amount of rotation for this visual.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Opacity to display this visual with
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// Current color of this visual
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Text which will be displayed on this visual
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Data space for use by the rendering engine.  This data is opaque and should not be modified outside
        /// the rendering engine.
        /// </summary>
        public object RenderData { get; set; }

    }
}

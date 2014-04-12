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

namespace GadrocsWorkshop.Helios.Interfaces.Falcon.OpenFalcon
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.Falcon.DED", "DED", "Open Falcon Textures", typeof(OpenFalconTextureDisplayRenderer))]
    public class DED : OpenFalconTextureDisplay
    {
        private static readonly Rect _defaultRect = new Rect(5, 270, 160, 63);

        public DED()
            : base("DED", new Size(367, 138))
        {
        }

        protected override OpenFalconTextureDisplay.FalconTextures Texture
        {
            get { return OpenFalconTextureDisplay.FalconTextures.DED; }
        }

        internal override string DefaultImage
        {
            get { return "{Helios}/Interfaces/Falcon/OpenFalcon/ded.png"; }
        }

        protected override Rect DefaultRect
        {
            get { return _defaultRect; }
        }
    }
}

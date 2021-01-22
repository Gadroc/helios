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

namespace GadrocsWorkshop.Helios.Gauges.AV8B
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.AV8B.trimPositionAileron", "AV-8B Aileron Trim Position", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class trimPositionAileron: trimPosition
    {
        public trimPositionAileron()
            : base(new GaugeImage("{Helios}/Gauges/AV-8B/Trim Position/Trim_aileron_faceplate.xaml", new Rect(0d, 0d, 300d, 300d)), "Aileron Trim Position", new Size(300d, 300d)) { }

    }
}

﻿//  Copyright 2014 Craig Courtney
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

/// <summary>
/// This is the revised version of the Rigt MDFD which is larger with improved images.  
/// There is a left and right to ensure correct autobinding.
/// It has a slightly different name because the old version is retained to help with backward compatability
/// </summary>
/// 


namespace GadrocsWorkshop.Helios.Gauges.AV8B
{ 
    using GadrocsWorkshop.Helios.Controls;
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Globalization;
    using System.Windows;

    [HeliosControl("AV8B.MPCDR", "MPCD (Right)", "AV-8B Gauges", typeof(MFDRenderer))]
    class Right_MPCD_1 : MPCD
    {
        public Right_MPCD_1()
            : base("Right MPCD", "Right MFCD")
        {
        }
    }
}

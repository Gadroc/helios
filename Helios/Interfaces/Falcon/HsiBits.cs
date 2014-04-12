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

namespace GadrocsWorkshop.Helios.Interfaces.Falcon
{
    using System;

    [Flags]
    enum HsiBits : uint
    {
        ToTrue = 0x01,    // HSI_FLAG_TO_TRUE
        IlsWarning = 0x02,    // HSI_FLAG_ILS_WARN
        CourseWarning = 0x04,    // HSI_FLAG_CRS_WARN
        Init = 0x08,    // HSI_FLAG_INIT
        TotalFlags = 0x10,    // HSI_FLAG_TOTAL_FLAGS; never set
        ADI_OFF = 0x20,    // ADI OFF Flag
        ADI_AUX = 0x40,    // ADI AUX Flag
        ADI_GS = 0x80,    // ADI GS FLAG
        ADI_LOC = 0x100,   // ADI LOC FLAG
        HSI_OFF = 0x200,   // HSI OFF Flag
        BUP_ADI_OFF = 0x400,   // Backup ADI Off Flag
        VVI = 0x800,   // VVI OFF Flag
        AOA = 0x1000,  // AOA OFF Flag
        AVTR = 0x2000,  // AVTR Light
        OuterMarker = 0x4000,  // MARKER beacon light for outer marker
        MiddleMarker = 0x8000,  // MARKER beacon light for middle marker
        FromTrue = 0x10000, // HSI_FLAG_TO_TRUE == 2, FROM
    };
}

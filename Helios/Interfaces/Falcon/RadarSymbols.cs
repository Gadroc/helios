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

    public enum RadarSymbols : int
    {
        NONE = 0,// nothing
        UNKNOWN = 1,// U
        ADVANCED_INTERCEPTOR = 2, //not used
        BASIC_INTERCEPTOR = 3, //not used
        ACTIVE_MISSILE = 4, //M
        HAWK = 5, //H
        PATRIOT = 6, //P
        SA2 = 7, //2
        SA3 = 8, //3
        SA4 = 9, //4
        SA5 = 10, //5
        SA6 = 11, //6
        SA8 = 12, //8
        SA9 = 13, //9
        SA10 = 14, //10
        SA13 = 15, //13
        AAA = 16, //A or S alternating
        SEARCH = 17, //S
        NAVAL = 18, //boat symbol
        CHAPARAL = 19, //C
        SA15 = 20, //15 or M alternating
        NIKE = 21, //N
        A1 = 22, //A with a single dot under it
        A2 = 23, //A with two dots under it
        A3 = 24, //A with three dots under it
        PDOT = 25, //P with a dot under it
        PSLASH = 26, //P with a vertical bar after it
        UNK1 = 27, //U with one dot under it
        UNK2 = 28, //U with two dots under it
        UNK3 = 29, //U with three dots under it
        KSAM = 30, //C
        V1 = 31, //1
        V4 = 32, //4
        V5 = 33, //5
        V6 = 34, //6
        V14 = 35, //14
        V15 = 36, //15
        V16 = 37, //16
        V18 = 38, //18
        V19 = 39, //19
        V20 = 40, //20
        V21 = 41, //21
        V22 = 42, //22
        V23 = 43, //23
        V25 = 44, //25
        V27 = 45, //27
        V29 = 46, //29
        V30 = 47, //30
        V31 = 48, //31
        VP = 49, //P
        VPD = 50, //PD
        VA = 51, //A
        VB = 52, //B
        VS = 53, //S
        Aa = 54, //A with a vertical bar after it
        Ab = 55, //A with vertical bars before and after it
        Ac = 56, //A with vertical bars before and after it and one through the middle
        MIB_F_S = 57, //F or S alternating
        MIB_F_A = 58, //F or A alternating
        MIB_F_M = 59, //F or M alternating
        MIB_F_U = 60, //F or U alternating
        MIB_F_BW = 61, //F or basic interceptor shape
        MIB_BW_S = 62, //S or basic interceptor shape
        MIB_BW_A = 63, //A or basic interceptor shape
        MIB_BW_M = 64, //M or basic interceptor shape
    }
}

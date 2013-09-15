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

using System;
using System.Runtime.InteropServices;

namespace GadrocsWorkshop.Helios.Visuals
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Color
    {
      [FieldOffset(0)]
      public byte R;
      [FieldOffset(1)]
      public byte G;
      [FieldOffset(2)]
      public byte B;
      [FieldOffset(3)]
      public byte A;
      [FieldOffset(0)]
      public int Int;
    }
}

//  Copyright 2019 Piet Van Nes
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

//  06/05/19 KiwiLostInMelb - Cut down version of Native Methods just for the KB Receiver

namespace GadrocsWorkshop.Helios.KeyPressReceiver
{
  using System;
  using System.ComponentModel;
  using System.Runtime.InteropServices;

  class NativeMethods
  {
    private NativeMethods()
    {
    }

    #region Keyboard Emulation

    internal const int INPUT_MOUSE = 0;
    internal const int INPUT_KEYBOARD = 1;
    internal const int INPUT_HARDWARE = 2;
    internal const uint KEY_EXTENDED = 0x0001;
    internal const uint KEY_UP = 0x0002;
    internal const uint KEY_UNICODE = 0x0004;
    internal const uint KEY_SCANCODE = 0x0008;

    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
      public int dx;
      public int dy;
      public uint mouseData;
      public uint dwFlags;
      public uint time;
      public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
      public ushort wVk;
      public ushort wScan;
      public uint dwFlags;
      public uint time;
      public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HARDWAREINPUT
    {
      public uint uMsg;
      public ushort wParamL;
      public ushort wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct MOUSEKEYBDHARDWAREINPUT
    {
      [FieldOffset(0)]
      public MOUSEINPUT mi;

      [FieldOffset(0)]
      public KEYBDINPUT ki;

      [FieldOffset(0)]
      public HARDWAREINPUT hi;
    }

    internal struct INPUT
    {
      public int type;
      public MOUSEKEYBDHARDWAREINPUT mkhi;

      public INPUT(ushort wVk, ushort wScan, uint dwFlags, uint time, IntPtr dwExtraInfo)
      {
        /// https://stackoverflow.com/questions/8962850/sendinput-fails-on-64bit
        type = INPUT_KEYBOARD;
        mkhi = new MOUSEKEYBDHARDWAREINPUT();
        /*Virtual Key code.  Must be from 1-254.  If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0.*/
        mkhi.ki.wVk = wVk;
        /*A hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the foreground application.*/
        mkhi.ki.wScan = wScan;
        /*Specifies various aspects of a keystroke.  See the KEYEVENTF_ constants for more information.*/
        mkhi.ki.dwFlags = dwFlags;
        /*The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time stamp.*/
        mkhi.ki.time = time;
        /*An additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information.*/
        mkhi.ki.dwExtraInfo = dwExtraInfo;
      }
    }

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [DllImport("user32.dll")]
    internal static extern IntPtr GetKeyboardLayout(uint idThread);

    [DllImport("user32.dll")]
    internal static extern short VkKeyScanEx(char ch, IntPtr dwhkl);

    [DllImport("user32.dll")]
    internal static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

    #endregion

  }
}

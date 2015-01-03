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

namespace GadrocsWorkshop.Helios.ControlCenter
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Input;

    // RECT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }

    // POINT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    // WINDOWPLACEMENT stores the position, size, and state of a window
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public POINT minPosition;
        public POINT maxPosition;
        public RECT normalPosition;
    }

    //[Serializable]
    //[StructLayout(LayoutKind.Sequential)]
    //public struct TouchMonitorInfo
    //{
    //    public uint Index;
    //    public uint Mode;
    //    public RECT Monitor;
    //    public RECT Working;
    //    public RECT VirtualDesktop;
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    //    public string DeviceName;
    //}

    internal class NativeMethods
    {
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll")]
        public static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, out WINDOWPLACEMENT lpwndpl);

        //#region eGalax Touch Screen API

        //// EGalax Touch Controls
        //public const ulong BRANCH_ENABLED = 0x80000000;
        //public const ulong BFORMAT_CAL = 0x00000001;
        //public const ulong BFORMAT_LINZ = 0x00000002;
        //public const ulong BFORMAT_EDGE = 0x00000004;
        //public const ulong BFORMAT_CONST_TOUCH = 0x00000008;
        //public const ulong BFORMAT_MONITOR = 0x00000010;
        //public const ulong BFORMAT_SPLIT = 0x00000020;
        //public const ulong BFORMAT_ORIENT = 0x00000040;

        //[DllImport("xtkutility.dll")]
        //public static extern IntPtr CreateDevice([MarshalAs(UnmanagedType.LPStr)]string szSymbolicName);

        //[DllImport("xtkutility.dll")]
        //public static extern bool CloseDevice(IntPtr hFile);

        //public delegate bool EnumerateTouchscreenCallback(IntPtr pContext, [MarshalAs(UnmanagedType.LPStr)]string szSymbolicName, uint nType);

        //[DllImport("xtkutility.dll")]
        //public static extern int EnumerateTouchInstance(IntPtr hWnd, IntPtr pContext, EnumerateTouchscreenCallback pCallback);

        //[DllImport("xtkutility.dll")]
        //public static extern void EnableTouch(IntPtr hFile, bool bEnable);

        //[DllImport("xtkutility.dll")]
        //public static extern bool StartDeviceThread(IntPtr hFile);

        //[DllImport("xtkutility.dll")]
        //public static extern bool StopDeviceThread(IntPtr hFile);

        //[DllImport("xtkutility.dll")]
        //public static extern void SetBranchFormat(IntPtr hFile, ref ulong pInfo);

        //[DllImport("xtkutility.dll")]
        //public static extern bool GetBranchFormat(IntPtr hFile, ref ulong pInfo);

        //[DllImport("xtkutility.dll")]
        //public static extern int RegisterTPNotifyMessage(IntPtr hFile, IntPtr hWnd, uint imsg);

        //[DllImport("xtkutility.dll")]
        //public static extern bool GetMonitorInfo32(IntPtr hFile, ref TouchMonitorInfo monitorInfo);

        //#endregion

        #region Hotkey API

        public const int WM_HOTKEY = 0x0312;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifiers, Keys vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        #endregion

        [DllImport("shell32.dll")] //shell routine to enable jumplist and recenfiles
        public static extern void SHAddToRecentDocs(UInt32 uFlags, [MarshalAs(UnmanagedType.LPWStr)] String pv);

        [DllImport("dwmapi.dll", EntryPoint = "DwmIsCompositionEnabled")]
        public static extern int DwmIsCompositionEnabled(out bool enabled);
    }

}

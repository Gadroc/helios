using System;
using System.Runtime.InteropServices;

namespace GadrocsWorkshop.Helios.Editor.UI
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = false)]
        internal static extern IntPtr GetDesktopWindow();
    }
}

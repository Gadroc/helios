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

namespace GadrocsWorkshop.Helios.Saitek
{
    internal delegate void DirectOutput_EnumerateCallback(IntPtr handle, IntPtr context); 
    internal delegate void DirectOutput_DeviceChange(IntPtr handle, bool added, IntPtr context); 
    internal delegate void DirectOutput_PageChange(IntPtr handle, Int16 page, bool setActive, IntPtr context);
    internal delegate void DirectOutput_SoftButtonChange(IntPtr handle, Int16 buttons, IntPtr context);

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetDllDirectory(string lpPathName);

        internal const string DirectOutputDll = "DIRECTOUTPUT.DLL";
        internal const UInt32 S_OK = 0;
        internal const UInt32 E_HANDLE = 0x80070006;
        internal const UInt32 E_PAGENOTACTIVE = 0xFF040001;
        internal const UInt32 E_BUFFERTOOSMALL = 0xFF04006F;
        internal const UInt32 E_INVALIDARG = 0x80070057;
        internal const UInt32 E_NOTIMPL = 0x80004001;

        internal const UInt32 FLAG_SET_AS_ACTIVE = 0x00000001;

        internal const string DeviceType_X52Pro = "{29DAD506-F93B-4F20-85FA-1E02C04FAC17}";
        internal const string DeviceType_Fip = "{3E083CD8-6A37-4A58-80A8-3D6A2C07513E}";

        /// <summary>
        /// Initialize the DirectOutput library
        /// </summary>
        /// <param name="pluginName">Name of the plugin, used for debugging purposes. Can be null.</param>
        /// <returns>S_OK : succeeded</returns>
        [DllImport(DirectOutputDll, EntryPoint = "DirectOutput_Initialize")]
        internal extern static UInt32 DirectOutput_Initialize([MarshalAs(UnmanagedType.LPWStr)]string pluginName);

        /// <summary>
        /// Cleanup the DirectOutput library.
        /// </summary>
        /// <returns>S_OK : succeeded</returns>
        [DllImport(DirectOutputDll)]
        internal extern static UInt32 DirectOutput_Deinitialize();

        /// <summary>
        /// Enumerates all DirectOutput devices currently attached. Device change callback will be called
        /// for each device.
        /// </summary>
        /// <returns>S_OK: succeeded, E_HANDLE: DirectOutput was not initialized.</returns>
        [DllImport(DirectOutputDll)]
        internal extern static UInt32 DirectOutput_Enumerate();

        /// <summary>
        /// Enumerates all DirectOutput devices currently attached.
        /// </summary>
        /// <param name="callback">Callback for each enumerated device.</param>
        /// <param name="context">Application supplied contect pointer that will be passed to the callback function</param>
        /// <returns>S_OK: succeeded, E_HANDLE: DirectOutput was not initialized.</returns>
        [DllImport(DirectOutputDll)]
        internal extern static UInt32 DirectOutput_Enumerate(DirectOutput_EnumerateCallback callback, IntPtr context);

        /// <summary>
        /// Register a callback that will be called whenever a device is added or removed, or when DirectOutput_Enumerate() is called.
        /// </summary>
        /// <param name="deviceChangeCallback">Callback for device changes.</param>
        /// <param name="context">Application supplied contect pointer that will be passed to the callback function</param>
        /// <returns>S_OK : succeeded</returns>
        [DllImport(DirectOutputDll)]
        public static extern int DirectOutput_RegisterDeviceChangeCallback(DirectOutput_DeviceChange deviceChangeCallback, IntPtr context);

        /// <summary>
        /// Get the device GUID.
        /// </summary>
        /// <param name="device">Device handle passed from enumerate or device callback.</param>
        /// <param name="guid">Output parameter which returns device type guid.</param>
        /// <returns>S_OK : succeeded, E_HANDLE : hDevice is not a valid device handle, E_INVALIDARG : guid is NULL</returns>
        [DllImport(DirectOutputDll)]
        internal extern static UInt32 DirectOutput_GetDeviceType(IntPtr device, out Guid guid);

        /// <summary>
        /// Get the device instance GUID used by IDirectInput::CreateDevice
        /// </summary>
        /// <param name="device">Device handle passed from enumerate or device callback.</param>
        /// <param name="guid">Pointer to GUID to receive device's DirectInput Instance Guid.</param>
        /// <returns>S_OK : succeeded, E_HANDLE : hDevice is not a valid device handle, E_INVALIDARG : guid is NULL, E_NOTIMPL : device does not support DirectInput</returns>
        [DllImport(DirectOutputDll)]
        internal extern static UInt32 DirectOutput_GetDeviceInstance(IntPtr device, out Guid guid);

        /// <summary>
        /// Register a callback that is called when the soft buttons are changed and the callee's page is active.
        /// </summary>
        /// <param name="device">Device handle passed from enumerate or device callback.</param>
        /// <param name="callback">Callback for soft button changes.</param>
        /// <param name="context">Application supplied contect pointer that will be passed to the callback function</param>
        /// <returns>S_OK : succeeded, E_HANDLE : hDevice is not a valid device handle</returns>
        [DllImport(DirectOutputDll)]
        internal extern static UInt32 DirectOutput_RegisterSoftButtonCallback(IntPtr device, DirectOutput_SoftButtonChange callback, IntPtr context);

        /// <summary>
        /// Adds a page to the device
        /// </summary>
        /// <param name="device">Device handle passed from enumerate or device callback.</param>
        /// <param name="page">caller assigned page id to add</param>
        /// <param name="name">Only used for debugging, can be NULL</param>
        /// <param name="flags">FLAG_SET_AS_ACTIVE if this page should be activated</param>
        /// <returns></returns>
        [DllImport(DirectOutputDll)]
        internal static extern UInt32 DirectOutput_AddPage(IntPtr device, uint page, string name, uint flags);

        /// <summary>
        /// Removes a page from the device
        /// </summary>
        /// <param name="device">Device handle passed from enumerate or device callback.</param>
        /// <param name="page">caller assigned page id to remove</param>
        /// <returns>S_OK : succeeded
        ///     E_HANDLE : hDevice is not a valid device handle
        ///     E_INVALIDARG : dwPage is not a valid page id</returns>
        [DllImport(DirectOutputDll)]
        internal static extern UInt32 DirectOutput_RemovePage(IntPtr device, uint page);

        /// <summary>
        /// Set the state of a LED on the device.
        /// </summary>
        /// <param name="device">Device handle passed from enumerate or device callback.</param>
        /// <param name="page">page to display the led on</param>
        /// <param name="index">index of the led</param>
        /// <param name="value">value of the led</param>
        /// <returns>S_OK : succeeded, E_HANDLE : hDevice is not a valid device handle, E_NOTIMPL : hDevice does not have any leds, E_INVALIDARG : dwPage or dwIndex is not a valid id, E_PAGENOTACTIVE : dwPage is not the active page</returns>
        [DllImport(DirectOutputDll)]
        internal extern static UInt32 DirectOutput_SetLed(IntPtr device, uint page, uint index, uint value);

        /// <summary>
        /// Set the image on the device.
        /// </summary>
        /// <param name="device">opaque device handle</param>
        /// <param name="page">page to display the image on</param>
        /// <param name="index">index of the image</param>
        /// <param name="size">the count of bytes in bitmap</param>
        /// <param name="bitmap">the raw bytes from a BMP (only the bytes that contain pixel data - must be correct format and size)</param>
        /// <returns>S_OK : succeeded
        ///     E_HANDLE : hDevice is not a valid device handle
        ///     E_NOTIMPL : hDevice does not have any images
        ///     E_INVALIDARG : dwPage or dwIndex is not a valid id
        ///     E_PAGENOTACTIVE : dwPage is not the active page
        ///     E_BUFFERTOOSMALL : cbValue is not of the correct size</returns>
        [DllImport(DirectOutputDll)]
        internal extern static UInt32 DirectOutput_SetImage(IntPtr device, uint page, uint index, uint size, IntPtr bitmap);
    }
}

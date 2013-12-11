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

namespace GadrocsWorkshop.Helios.Saitek
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;
    using System.Text;

    using GadrocsWorkshop.Helios;
    using System.Runtime.InteropServices;

    [Export(typeof(IPlugIn))]
    [ExportMetadata("Id", "GadrocsWorkshop.Saitek")]
    [ExportMetadata("Name", "Saitek Pro Flight PlugIn")]
    [ExportMetadata("Description", "Exposes Saitek Pro Flight Flight Instrument Panels as displays and input devices.")]
    public class SaitekPlugin : IPlugIn
    {
        private const int VendorId = 0x06A3;
        private const int ProductId = 0xA2AE;

        private const string DIRECTOUT_PATH = "c:\\program files\\saitek\\directoutput";
        private const string DIRECTOUT32_PATH = "c:\\program files (x86)\\saitek\\directoutput\\";

        private static Guid DEVICE_TYPE_X52PRO = new Guid("{29dad506-f93b-4f20-85fa-1e02c04fac17}");
        private static Guid DEVICE_TYPE_FIP = new Guid("{3e083cd8-6a37-4a58-80a8-3d6a2c07513e}");
        private static Guid PRODUCT_GUID_FIP = new Guid("{a2ae06a3-0000-0000-0000-504944564944}");

        private bool _initialized = false;

        private IList<FipDisplay> _dispays = new List<FipDisplay>();

        public bool IsUnique
        {
            get { return true; }
        }

        public bool IsAutoActive
        {
            get { return false; }
        }

        public IEnumerable<IDisplay> GetDisplays()
        {
            return _dispays;
        }

        public IEnumerable<IDevice> GetDevices()
        {
            // TODO Implement device for the display buttons and rotaries.
            return Enumerable.Empty<IDevice>();
        }

        public IEnumerable<IControl> GetControls()
        {
            // No controls are implemented in this library so return an empty list.
            return Enumerable.Empty<IControl>();
        }

        public IControl CreateControl(string typeId)
        {
            return null;
        }

        public void Initialize()
        {
            try
            {
                string path = DIRECTOUT_PATH;
                if (!Environment.Is64BitProcess && Directory.Exists(DIRECTOUT32_PATH))
                {
                    path = DIRECTOUT32_PATH;
                }
                // Don't initialize if DirectOutput api does not exist.
                if (Directory.Exists(path))
                {
                    StringBuilder currentPath = new StringBuilder(1024);
                    NativeMethods.GetDllDirectory(currentPath.Length, currentPath);

                    NativeMethods.SetDllDirectory(path);
                    NativeMethods.LoadLibrary(NativeMethods.DirectOutputDll);
                    NativeMethods.SetDllDirectory(currentPath.ToString());

                    NativeMethods.DirectOutput_Initialize("Helios");
                    NativeMethods.DirectOutput_Enumerate(DeviceEnumeration, IntPtr.Zero);
                    NativeMethods.SetDllDirectory(null);
                    _initialized = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Destroy()
        {
            if (_initialized)
            {
                NativeMethods.DirectOutput_Deinitialize();
            }
        }

        public void DeviceEnumeration(IntPtr handle, IntPtr context)
        {
            Guid deviceType = Guid.Empty;
            if (NativeMethods.DirectOutput_GetDeviceType(handle, out deviceType) == NativeMethods.S_OK)
            {
                if (deviceType.Equals(DEVICE_TYPE_FIP))
                {
                    //Guid deviceInstance = new Guid();
                    //NativeMethods.DirectOutput_GetDeviceInstance(handle, out deviceInstance);

                    //Console.WriteLine(deviceInstance);

                    //foreach (HidDevice hidDevice in HidDevices.Enumerate(VendorId, ProductId))
                    //{
                    //    hidDevice.OpenDevice();
                    //    StringBuilder sb = new StringBuilder(200);
                    //    NativeMethods.HidD_GetSerialNumberString(hidDevice.ReadHandle, sb, 200);
                    //    Console.WriteLine(sb);
                    //    hidDevice.CloseDevice();
                    //}
                    FipDisplay display = new FipDisplay(handle);
                    _dispays.Add(display);
                }
            }
        }

        public void DeviceChanged(IntPtr handle, bool bAdded, IntPtr context)
        {

        }
    }
}


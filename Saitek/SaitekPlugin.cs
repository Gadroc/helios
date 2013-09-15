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
using System.Collections.Generic;
using System.ComponentModel.Composition;

using GadrocsWorkshop.Helios;
using System.Linq;
using System.IO;

namespace GadrocsWorkshop.Helios.Saitek
{
    [Export(typeof(IPlugIn))]
    [ExportMetadata("Id", "GadrocsWorkshop.Saitek")]
    [ExportMetadata("Name", "Saitek Pro Flight PlugIn")]
    [ExportMetadata("Description", "PlugIn to use Saitek Pro Flight displays and input devices.")]
    public class SaitekPlugin : IPlugIn
    {
        private const string DIRECTOUT_PATH = "c:\\program files\\saitek\\directoutput";
        private const string DIRECTOUT32_PATH = "c:\\program files (x86)\\saitek\\directoutput\\";

        private bool _initialized = false;
        private Guid _fipGuid = new Guid(NativeMethods.DeviceType_Fip);

        private IList<FipDisplay> _dispays = new List<FipDisplay>();

        public IEnumerable<IDisplay> GetDisplays()
        {
            return _dispays;
        }

        public IEnumerable<IDevice> GetDevices()
        {
            return Enumerable.Empty<IDevice>();
        }

        public IEnumerable<IControlType> GetControlTypes()
        {
            return Enumerable.Empty<IControlType>();
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
                if (IntPtr.Size == 4 && Directory.Exists(DIRECTOUT32_PATH))
                {
                    path = DIRECTOUT32_PATH;
                }
                // Don't initialize if DirectOutput api does not exist.
                if (Directory.Exists(path))
                {
                    NativeMethods.SetDllDirectory(path);
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
                if (deviceType.Equals(_fipGuid))
                {
                    FipDisplay display = new FipDisplay(handle, "Saitek ProFlight Instrument Panel");
                    _dispays.Add(display);
                }
            }
        }
    }
}


using System;
using System.Collections.Generic;

using GadrocsWorkshop.Helios;
using GadrocsWorkshop.Helios.Profile;

namespace GadrocsWorkshop.Helios.Saitek
{
    public class FipDisplay : IDisplay
    {
        private IntPtr _device;
        private string _id;
        private string _name;
        private IEnumerable<ControlInstance> _controls;

        internal FipDisplay(IntPtr device, string name)
        {
            _device = device;
            _name = name;

            Guid deviceInstance = new Guid();
            NativeMethods.DirectOutput_GetDeviceInstance(_device, out deviceInstance);
            _id = deviceInstance.ToString();
        }

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public string PlugInId
        {
            get
            {
                return "GadrocsWorkshop.Saitek";
            }
        }
     
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int Width
        {
            get { return 320; }
        }

        public int Height
        {
            get { return 240; }
        }

        public IEnumerable<ControlInstance> Controls
        {
            get { return _controls; }
            set { _controls = value; }
        }

        public void Identify(string label)
        {
            // TODO: Setup visual tree and display with renderer.
        }

        public void Initialize()
        {
            NativeMethods.DirectOutput_AddPage(_device, 1, "Helios", NativeMethods.FLAG_SET_AS_ACTIVE);
        }

        public void Dispose()
        {
            NativeMethods.DirectOutput_RemovePage(_device, 1);
        }

    }
}

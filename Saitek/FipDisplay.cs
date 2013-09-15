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

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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.M2000C
{
    using GadrocsWorkshop.Helios.ComponentModel;
    //using GadrocsWorkshop.Helios.Interfaces.DCS.M2000C.Functions;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using Microsoft.Win32;
    using System;

    [HeliosInterface("Helios.M2000C", "DCS M-2000C", typeof(M2000CInterfaceEditor), typeof(UniqueHeliosInterfaceFactory))]
    public class M2000CInterface : BaseUDPInterface
    {
        private string _dcsPath;

        private bool _phantomFix;
        private int _phantomLeft;
        private int _phantomTop;

        private long _nextCheck = 0;
        #region Devices
        private const string FLIGHTINST = "1";
        private const string NAVINST = "2";
        private const string ENGINE = "3";
        private const string INSTPANEL = "4";
        private const string VTH_VTB = "5";
        private const string PCA_PPA = "6";
        private const string ENGPANEL = "7";
        private const string PWRPNL = "8";
        private const string PCN_NAV = "9";
        private const string RADAR_RDI = "10";
        private const string RADAR = "11";
        private const string EW_RWR = "12";
        private const string RWR = "13";
        private const string SUBSYSTEMS = "14";
        private const string MAGIC = "15";
        private const string SYSLIGHTS = "16";
        private const string AFCS = "17";
        private const string ELECTRIC = "18";
        private const string UVHF = "19";
        private const string UHF = "20";
        private const string INTERCOM = "21";
        private const string MISCPANELS = "22";
        private const string TACAN = "23";
        private const string VORILS = "24";
        private const string ECS = "25";
        private const string FBW = "26";
        private const string DDM = "27";
        private const string DDM_IND = "28";
        private const string WEAPONS_CONTROL = "29";
        #endregion
        public M2000CInterface()
            : base("DCS M-2000C")
        {
            DCSConfigurator config = new DCSConfigurator("DCSM2000C", DCSPath);
            config.ExportConfigPath = "Config\\Export";
            config.ExportFunctionsPath = "pack://application:,,,/Helios;component/Interfaces/DCS/M2000C/ExportFunctions.lua";
            Port = config.Port;
            _phantomFix = config.PhantomFix;
            _phantomLeft = config.PhantomFixLeft;
            _phantomTop = config.PhantomFixTop;
        }

    private string DCSPath
    {
        get
        {
            if (_dcsPath == null)
            {
                RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS M-2000C");
                if (pathKey != null)
                {
                    _dcsPath = (string)pathKey.GetValue("Path");
                    pathKey.Close();
                    ConfigManager.LogManager.LogDebug("DCS M-2000C Interface Editor - Found DCS Path (Path=\"" + _dcsPath + "\")");
                }
                else
                {
                    _dcsPath = "";
                }
            }
            return _dcsPath;
        }
    }

    protected override void OnProfileChanged(HeliosProfile oldProfile)
    {
        base.OnProfileChanged(oldProfile);

        if (oldProfile != null)
        {
            oldProfile.ProfileTick -= Profile_Tick;
        }

        if (Profile != null)
        {
            Profile.ProfileTick += Profile_Tick;
        }
    }

    void Profile_Tick(object sender, EventArgs e)
    {
        if (_phantomFix && System.Environment.TickCount - _nextCheck >= 0)
        {
            System.Diagnostics.Process[] dcs = System.Diagnostics.Process.GetProcessesByName("DCS");
            if (dcs.Length == 1)
            {
                IntPtr hWnd = dcs[0].MainWindowHandle;
                NativeMethods.Rect dcsRect;
                NativeMethods.GetWindowRect(hWnd, out dcsRect);

                if (dcsRect.Width > 640 && (dcsRect.Left != _phantomLeft || dcsRect.Top != _phantomTop))
                {
                    NativeMethods.MoveWindow(hWnd, _phantomLeft, _phantomTop, dcsRect.Width, dcsRect.Height, true);
                }
            }
            _nextCheck = System.Environment.TickCount + 5000;
        }
    }
}
}

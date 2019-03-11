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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.Mi8Simple
{
    using GadrocsWorkshop.Helios.ComponentModel;
    //using GadrocsWorkshop.Helios.Interfaces.DCS.Mi8Simple.Functions;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using Microsoft.Win32;
    using System;

    [HeliosInterface("Helios.Mi8Simple", "DCS  Mi8 (Simple)", typeof(Mi8SimpleInterfaceEditor), typeof(UniqueHeliosInterfaceFactory))]
    public class  Mi8SimpleInterface : BaseUDPInterface
    {
        private string _dcsPath;

        private bool _phantomFix;
        private int _phantomLeft;
        private int _phantomTop;

        private long _nextCheck = 0;
        #region Devices
        private const string ELEC_INTERFACE = "0";
        private const string FUELSYS_INTERFACE = "1";
        private const string ENGINE_INTERFACE = "2";
        private const string HYDRO_SYS_INTERFACE = "3";
        private const string OPB_1 = "4";
        private const string AGB_3K_LEFT = "5";
        private const string AGB_3K_RIGHT = "6";
        private const string SPUU_52 = "7";
        private const string NAVLIGHT_SYSTEM = "8";
        private const string SOUND_SYSTEM = "9";
        private const string VMS = "10";
        private const string WEAPON_SYS = "11";
        private const string VECTOR = "12";
        private const string GMK1A = "13";
        private const string DISS_15 = "14";
        private const string AUTOPILOT = "15";
        private const string CPT_MECH = "16";
        private const string RADAR_ALTIMETER = "17";
        private const string FIRE_EXTING_INTERFACE = "18";
        private const string MISC_SYSTEMS_INTERFACE = "19";
        private const string SYS_CONTROLLER = "20";
        private const string PULSE_TIMER = "21";
        private const string HEAD_WRAPPER = "22";
        private const string TURN_SLIP_IND = "23";
        private const string FM_PROXY = "24";
        private const string BAR_ALTIMETER_L = "25";
        private const string BAR_ALTIMETER_R = "26";
        private const string IAS_IND_L = "27";
        private const string IAS_IND_R = "28";
        private const string VARIOMETER_L = "29";
        private const string VARIOMETER_R = "30";
        private const string CORRECTION_INTERRUPT = "31";
        private const string REMOTE_COMPASS = "32";
        private const string HSI_L = "33";
        private const string HSI_R = "34";
        private const string SPU_7 = "35";
        private const string JADRO_1A = "36";
        private const string R_863 = "37";
        private const string R_828 = "38";
        private const string ARC_9 = "39";
        private const string ARC_UD = "40";
        private const string CARGO_CAM = "41";
        private const string KNEEBOARD = "42";
        private const string MACROS = "43";
        private const string CLOCK = "44";
        private const string LIGHT_SYSTEM = "45";
        private const string PKV = "46";
        private const string UV_26 = "47";
        private const string HELMET_DEVICE = "48";
        private const string STANDBY_COMPASS = "49";
        private const string EXT_CARGO_EQUIPMENT = "50";
        private const string SIGNAL_FLARES = "51";
        private const string HEATER_KO50 = "52";
        private const string AUTOPILOT_ADJUSTMENT = "53";
        private const string SARPP12DM = "54";
        private const string RECORDER_P503B = "55";
        private const string IFF = "56";
        private const string EXTERNAL_CARGO_SPEECH = "57";
        private const string EXTERNAL_CARGO_VIEW = "58";
        #endregion
        public Mi8SimpleInterface()
            : base("DCS Mil Mi-8 (Simple)")
        {
            DCSConfigurator config = new DCSConfigurator("DCSMi8", DCSPath);
            config.ExportConfigPath = "Config\\Export";
            config.ExportFunctionsPath = "pack://application:,,,/Helios;component/Interfaces/DCS/Mi8Simple/ExportFunctions.lua";
            Port = config.Port;
            _phantomFix = config.PhantomFix;
            _phantomLeft = config.PhantomFixLeft;
            _phantomTop = config.PhantomFixTop;

            #region Indicators
            // !!!! Any duplicate "name" values in a function will cause Helios to go bang.  Make sure that when you change the name, that it is unique
            AddFunction(new FlagValue(this, "781", "AP Indicators", "AP heading on", ""));
            AddFunction(new FlagValue(this, "782", "AP Indicators", "AP heading off", ""));
            AddFunction(new FlagValue(this, "783", "AP Indicators", "AP pitch roll on", ""));
            AddFunction(new FlagValue(this, "784", "AP Indicators", "AP height on", ""));
            AddFunction(new FlagValue(this, "785", "AP Indicators", "AP height off", ""));
            AddFunction(new FlagValue(this, "316", "ENGINE Indicators", "ENGINE RT LEFT ON", ""));
            AddFunction(new FlagValue(this, "317", "ENGINE Indicators", "ENGINE RT RIGHT ON", ""));
            AddFunction(new FlagValue(this, "318", "SARPP Indicators", "SARPP ON", ""));
            AddFunction(new FlagValue(this, "325", "Descent Indicators", "Descent Siren", ""));
            AddFunction(new FlagValue(this, "326", "LOCK Indicators", "LOCK OPEN", ""));
            AddFunction(new FlagValue(this, "327", "DOORS Indicators", "DOORS OPEN", ""));
            AddFunction(new FlagValue(this, "340", "TURN Indicators", "TURN ON RI 65", ""));
            AddFunction(new FlagValue(this, "360", "FROST Indicators", "FROST", ""));
            AddFunction(new FlagValue(this, "362", "LEFT Indicators", "LEFT ENG HEATING", ""));
            AddFunction(new FlagValue(this, "363", "RIGHT Indicators", "RIGHT ENG HEATING", ""));
            AddFunction(new FlagValue(this, "361", "ANTI Indicators", "ANTI ICE ON", ""));
            AddFunction(new FlagValue(this, "364", "LEFT Indicators", "LEFT PZU FRONT", ""));
            AddFunction(new FlagValue(this, "365", "RIGHT Indicators", "RIGHT PZU FRONT", ""));
            AddFunction(new FlagValue(this, "366", "LEFT Indicators", "LEFT PZU BACK", ""));
            AddFunction(new FlagValue(this, "367", "RIGHT Indicators", "RIGHT PZU BACK", ""));
            AddFunction(new FlagValue(this, "373", "SECTION Indicators", "SECTION 1", ""));
            AddFunction(new FlagValue(this, "375", "SECTION Indicators", "SECTION 2", ""));
            AddFunction(new FlagValue(this, "374", "SECTION Indicators", "SECTION 3", ""));
            AddFunction(new FlagValue(this, "376", "SECTION Indicators", "SECTION 4", ""));
            AddFunction(new FlagValue(this, "368", "RIO Indicators", "RIO heating ok", ""));
            AddFunction(new FlagValue(this, "377", "LEFT Indicators", "LEFT ENG FIRE", ""));
            AddFunction(new FlagValue(this, "378", "RIGHT Indicators", "RIGHT ENG FIRE", ""));
            AddFunction(new FlagValue(this, "379", "KO50 Indicators", "KO50 FIRE", ""));
            AddFunction(new FlagValue(this, "380", "REDUC Indicators", "REDUC AI9 FIRE", ""));
            AddFunction(new FlagValue(this, "381", "FIRE Indicators", "FIRE LENG 1 QUEUE", ""));
            AddFunction(new FlagValue(this, "382", "FIRE Indicators", "FIRE RENG 1 QUEUE", ""));
            AddFunction(new FlagValue(this, "383", "FIRE Indicators", "FIRE KO50 1 QUEUE", ""));
            AddFunction(new FlagValue(this, "384", "FIRE Indicators", "FIRE REDUCT 1 QUEUE", ""));
            AddFunction(new FlagValue(this, "385", "FIRE Indicators", "FIRE LENG 2 QUEUE", ""));
            AddFunction(new FlagValue(this, "386", "FIRE Indicators", "FIRE RENG 2 QUEUE", ""));
            AddFunction(new FlagValue(this, "387", "FIRE Indicators", "FIRE KO50 2 QUEUE", ""));
            AddFunction(new FlagValue(this, "388", "FIRE Indicators", "FIRE REDUCT 2 QUEUE", ""));
            AddFunction(new FlagValue(this, "398", "CHECK Indicators", "CHECK SENSORS", ""));
            AddFunction(new FlagValue(this, "407", "HYDRO Indicators", "HYDRO main on", ""));
            AddFunction(new FlagValue(this, "408", "HYDRO Indicators", "HYDRO aux on", ""));
            AddFunction(new FlagValue(this, "414", "APD9 Indicators", "APD9 on", ""));
            AddFunction(new FlagValue(this, "416", "APD9 Indicators", "APD9 oil pressure", ""));
            AddFunction(new FlagValue(this, "417", "APD9 Indicators", "APD9 rpm", ""));
            AddFunction(new FlagValue(this, "418", "APD9 Indicators", "APD9 rpm high", ""));
            AddFunction(new FlagValue(this, "420", "APD Indicators", "APD on", ""));
            AddFunction(new FlagValue(this, "424", "APD Indicators", "APD starter on", ""));
            AddFunction(new FlagValue(this, "434", "FUEL Indicators", "FUEL left closed", ""));
            AddFunction(new FlagValue(this, "435", "FUEL Indicators", "FUEL right closed", ""));
            AddFunction(new FlagValue(this, "436", "FUEL Indicators", "FUEL ring closed", ""));
            AddFunction(new FlagValue(this, "441", "FUEL Indicators", "FUEL center on", ""));
            AddFunction(new FlagValue(this, "442", "FUEL Indicators", "FUEL left on", ""));
            AddFunction(new FlagValue(this, "443", "FUEL Indicators", "FUEL right on", ""));
            AddFunction(new FlagValue(this, "461", "HEATER Indicators", "HEATER", ""));
            AddFunction(new FlagValue(this, "462", "IGNITION Indicators", "IGNITION", ""));
            AddFunction(new FlagValue(this, "463", "KO50 Indicators", "KO50 ON", ""));
            AddFunction(new FlagValue(this, "341", "LEFT Indicators", "LEFT PITOT HEATER OK", ""));
            AddFunction(new FlagValue(this, "490", "RIGHT Indicators", "RIGHT PITOT HEATER OK", ""));
            AddFunction(new FlagValue(this, "509", "LEFT Indicators", "LEFT PZU ON", ""));
            AddFunction(new FlagValue(this, "510", "RIGHT Indicators", "RIGHT PZU ON", ""));
            AddFunction(new FlagValue(this, "504", "ELEC Indicators", "ELEC turn VU1", ""));
            AddFunction(new FlagValue(this, "505", "ELEC Indicators", "ELEC turn VU2", ""));
            AddFunction(new FlagValue(this, "506", "ELEC Indicators", "ELEC turn VU3", ""));
            AddFunction(new FlagValue(this, "507", "ELEC Indicators", "ELEC DC ground", ""));
            AddFunction(new FlagValue(this, "508", "ELEC Indicators", "ELEC test equipment", ""));
            AddFunction(new FlagValue(this, "543", "ELEC Indicators", "ELEC gen1 fail", ""));
            AddFunction(new FlagValue(this, "544", "ELEC Indicators", "ELEC gen2 fail", ""));
            AddFunction(new FlagValue(this, "545", "ELEC Indicators", "ELEC AC ground", ""));
            AddFunction(new FlagValue(this, "546", "ELEC Indicators", "ELEC PO 500", ""));
            AddFunction(new FlagValue(this, "86", "CIRCUIT Indicators", "CIRCUIT FROM BATTERY", ""));
            AddFunction(new FlagValue(this, "68", "CLOG Indicators", "CLOG TF LEFT", ""));
            AddFunction(new FlagValue(this, "69", "CLOG Indicators", "CLOG TF RIGHT", ""));
            AddFunction(new FlagValue(this, "70", "CHIP Indicators", "CHIP LEFT ENG", ""));
            AddFunction(new FlagValue(this, "71", "CHIP Indicators", "CHIP RIGHT ENG", ""));
            AddFunction(new FlagValue(this, "72", "VIBRATION Indicators", "VIBRATION LEFT HIGH", ""));
            AddFunction(new FlagValue(this, "73", "VIBRATION Indicators", "VIBRATION RIGHT HIGH", ""));
            AddFunction(new FlagValue(this, "74", "FIRE Indicators", "FIRE", ""));
            AddFunction(new FlagValue(this, "76", "LEFT Indicators", "LEFT ENG TURN OFF", ""));
            AddFunction(new FlagValue(this, "77", "RIGHT Indicators", "RIGHT ENG TURN OFF", ""));
            AddFunction(new FlagValue(this, "78", "FT Indicators", "FT LEFT HIGH", ""));
            AddFunction(new FlagValue(this, "79", "FT Indicators", "FT RIGHT HIGH", ""));
            AddFunction(new FlagValue(this, "80", "OIL Indicators", "OIL PRESSURE LEFT", ""));
            AddFunction(new FlagValue(this, "81", "OIL Indicators", "OIL PRESSURE RIGHT", ""));
            AddFunction(new FlagValue(this, "82", "ER Indicators", "ER LEFT", ""));
            AddFunction(new FlagValue(this, "83", "ER Indicators", "ER RIGHT", ""));
            AddFunction(new FlagValue(this, "84", "EEC Indicators", "EEC LEFT OFF", ""));
            AddFunction(new FlagValue(this, "85", "EEC Indicators", "EEC RIGHT OFF", ""));
            AddFunction(new FlagValue(this, "873", "CHIP Indicators", "CHIP MAIN REDUCTOR", ""));
            AddFunction(new FlagValue(this, "874", "CHIP Indicators", "CHIP INTER REDUCTOR", ""));
            AddFunction(new FlagValue(this, "875", "CHIP Indicators", "CHIP TAIL REDUCTOR", ""));
            AddFunction(new FlagValue(this, "64", "300 Indicators", "300 Left", ""));
            AddFunction(new FlagValue(this, "65", "DISS Indicators", "DISS OFF", ""));
            AddFunction(new FlagValue(this, "555", "BD1 Indicators", "BD1", ""));
            AddFunction(new FlagValue(this, "556", "BD2 Indicators", "BD2", ""));
            AddFunction(new FlagValue(this, "557", "BD3 Indicators", "BD3", ""));
            AddFunction(new FlagValue(this, "558", "BD4 Indicators", "BD4", ""));
            AddFunction(new FlagValue(this, "559", "BD5 Indicators", "BD5", ""));
            AddFunction(new FlagValue(this, "560", "BD6 Indicators", "BD6", ""));
            AddFunction(new FlagValue(this, "711", "BD1Bomb Indicators", "BD1Bomb", ""));
            AddFunction(new FlagValue(this, "712", "BD2Bomb Indicators", "BD2Bomb", ""));
            AddFunction(new FlagValue(this, "713", "BD3Bomb Indicators", "BD3Bomb", ""));
            AddFunction(new FlagValue(this, "714", "BD4Bomb Indicators", "BD4Bomb", ""));
            AddFunction(new FlagValue(this, "715", "BD5Bomb Indicators", "BD5Bomb", ""));
            AddFunction(new FlagValue(this, "716", "BD6Bomb Indicators", "BD6Bomb", ""));
            AddFunction(new FlagValue(this, "562", "PUS1 Indicators", "PUS1", ""));
            AddFunction(new FlagValue(this, "563", "PUS3 Indicators", "PUS3", ""));
            AddFunction(new FlagValue(this, "564", "PUS4 Indicators", "PUS4", ""));
            AddFunction(new FlagValue(this, "565", "PUS6 Indicators", "PUS6", ""));
            AddFunction(new FlagValue(this, "561", "EmergExplode Indicators", "EmergExplode", ""));
            AddFunction(new FlagValue(this, "705", "EmergExplodeSec Indicators", "EmergExplodeSec", ""));
            AddFunction(new FlagValue(this, "710", "BV Indicators", "BV Net On", ""));
            AddFunction(new FlagValue(this, "566", "RS Indicators", "RS Net On", ""));
            AddFunction(new FlagValue(this, "567", "GUV Indicators", "GUV Net On", ""));
            //AddFunction(new FlagValue(this, " c", "MV Indicators", "MV Net On", ""));
            AddFunction(new FlagValue(this, "568", "FKP Indicators", "FKP On", ""));
            AddFunction(new FlagValue(this, "778", "Caution Indicators", "Caution Weap", ""));
            //AddFunction(new FlagValue(this, " c", "LeftSignal Indicators", "LeftSignal", ""));
            //AddFunction(new FlagValue(this, " c", "RightSignal Indicators", "RightSignal", ""));
            AddFunction(new FlagValue(this, "586", "Ready1 Indicators", "Ready1", ""));
            AddFunction(new FlagValue(this, "587", "Ready2 Indicators", "Ready2", ""));
            AddFunction(new FlagValue(this, "588", "Ready3 Indicators", "Ready3", ""));
            AddFunction(new FlagValue(this, "306", "Record Indicators", "Record P503B", ""));
            AddFunction(new FlagValue(this, "302", "IFF Indicators", "IFF KD", ""));
            AddFunction(new FlagValue(this, "303", "IFF Indicators", "IFF KP", ""));
            AddFunction(new FlagValue(this, "912", "IFF Indicators", "IFF TurnOnReserve", ""));
            AddFunction(new FlagValue(this, "87", "IFF Indicators", "IFF Failure", ""));
            #endregion
            #region Switches and Toggles

            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3015", "538", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "Generator 1 Switch", "%0.1f"));    // elements["PTR-EEP-TMB-GEN1"]        = default_2_position_tumb(_("Generator 1 Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_15, 538)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3016", "539", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "Generator 2 Switch", "%0.1f"));    // elements["PTR-EEP-TMB-GEN2"]        = default_2_position_tumb(_("Generator 2 Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_16, 539)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3014", "540", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "AC Ground Power Switch", "%0.1f"));    // elements["PTR-EEP-TMB-AER"]     = default_2_position_tumb(_("AC Ground Power Switch, ON/OFF"),devices.ELEC_INTERFACE, device_commands.Button_14, 540)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3003", "495", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "Battery 1 Switch", "%0.1f"));    // elements["PTR-RSPE-TMB-AKK1"]       = default_2_position_tumb(_("Battery 1 Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_3, 495)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3002", "496", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "Battery 2 Switch", "%0.1f"));    // elements["PTR-RSPE-TMB-AKK2"]       = default_2_position_tumb(_("Battery 2 Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_2, 496)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3001", "497", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "Standby Generator Switch", "%0.1f"));    // elements["PTR-RSPE-TMB-RESGEN"]     = default_2_position_tumb(_("Standby Generator Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_1, 497)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3007", "499", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "Rectifier 1 Switch", "%0.1f"));    // elements["PTR-RSPE-TMB-RECT1"]      = default_2_position_tumb(_("Rectifier 1 Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_7, 499)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3005", "500", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "Rectifier 2 Switch", "%0.1f"));    // elements["PTR-RSPE-TMB-RECT2"]      = default_2_position_tumb(_("Rectifier 2 Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_5, 500)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3006", "501", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "Rectifier 3 Switch", "%0.1f"));    // elements["PTR-RSPE-TMB-RECT3"]      = default_2_position_tumb(_("Rectifier 3 Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_6, 501)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3004", "502", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "DC Ground Power Switch", "%0.1f"));    // elements["PTR-RSPE-TMB-AERF"]       = default_2_position_tumb(_("DC Ground Power Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_4, 502)
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3009", "503", "1.0", "ON", "0.0", "OFF", "ELEC INTERFACE", "Equipment Test Switch", "%0.1f"));    // elements["PTR-RSPE-TMB-TESTEQU"]    = default_2_position_tumb(_("Equipment Test Switch, ON/OFF"), devices.ELEC_INTERFACE, device_commands.Button_9, 503)


            #endregion
        }

        private string DCSPath
        {
        get
        {
            if (_dcsPath == null)
            {
                RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS Mi-8");
                if (pathKey != null)
                {
                    _dcsPath = (string)pathKey.GetValue("Path");
                    pathKey.Close();
                    ConfigManager.LogManager.LogDebug("DCS Mil Mi-8 (Simple) Interface Editor - Found DCS Path (Path=\"" + _dcsPath + "\")");
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

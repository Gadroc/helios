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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.F14B
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Interfaces.DCS.F14B;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using Microsoft.Win32;
    using System;

    //[HeliosInterface("Helios.F14B", "DCS F-14B", typeof(F14BInterfaceEditor), typeof(UniqueHeliosInterfaceFactory))]
    public class F14BInterface : BaseUDPInterface
    {
        private string _dcsPath;

        private bool _phantomFix;
        private int _phantomLeft;
        private int _phantomTop;

        private long _nextCheck = 0;

        #region Devices
        private const string PROXY = "1";  // must be 1, hardcoded device ID of 1 is used by mission files (see proxy_device.lua)
        private const string ICS = "2";  // hardcoded for SimpleRadioStandalone integration
        private const string ARC159 = "3"; // hardcoded for SimpleRadioStandalone integration
        private const string ARC182 = "4"; // hardcoded for SimpleRadioStandalone integration
        private const string COUNTERMEASURES = "5"; // hardcoded for the ground crew menu options (countermeasures loadout)
        private const string EPOXY = "6"; // hardcoded for some glue
        private const string SENSORS = "7";
        private const string FMSENSOR = "8";
        private const string MULTICREW = "9";
        private const string BITPANEL = "10";
        private const string COCKPITMECHANICS = "11";
        private const string HYDRAULICS = "12";
        private const string AICS = "13";
        private const string ELECTRICS = "14";
        private const string PNEUMATICS = "15";
        private const string WINGSWEEP = "16";
        private const string GEARHOOK = "17";
        private const string FLAPS = "18";
        private const string ENGINE = "19";
        private const string FUELSYSTEM = "20";
        private const string AFCS = "21";
        private const string CADC = "22";
        private const string CAP = "23";
        private const string ACCELEROMETER = "24";
        private const string AOASYSTEM = "25";
        private const string CLOCK = "26";
        private const string MACHANDAIRSPEED = "27";
        private const string BAROALTIMETER = "28";
        private const string RADARALTIMETER = "29";
        private const string STDBYAI = "30";
        private const string TURNANDSLIP = "31";
        private const string VERTICALVEL = "32";
        private const string STANDBYCOMPASS = "33";
        private const string WARNINGLIGHTS = "34";
        private const string FCINSTRUMENTS = "35";
        private const string BDHI = "36";
        private const string TCS = "37";
        private const string LANTIRN = "38";
        private const string RADAR = "39";
        private const string HUD = "40";
        private const string HSD = "41";
        private const string VDI = "42";
        private const string TID = "43";
        private const string ECMD = "44";
        private const string HELMET_DEVICE = "45";
        private const string NAV_INTERFACE = "46";
        private const string TACAN = "47";
        private const string ILS = "48";
        private const string IMU = "49";
        private const string INS = "50";
        private const string AHRS = "51";
        private const string DATALINK = "52";
        private const string DECM = "53";
        private const string RWR = "54";
        private const string WEAPONS = "55";
        private const string WALKMAN = "56";
        private const string HOTAS = "57";
        private const string HCU = "58";
        //private const string ICSRADIO
        //private const string TESTRADIO
        //private const string TESTINTERCOM
        private const string DEBUGDISPLAY = "59";
        private const string CONTROLS = "60";
        private const string KNEEBOARD = "61";
        private const string JESTERAI = "62";
        private const string WCS = "63";
        private const string IFF = "64";
        private const string AUTO = "65";
        #endregion

        public F14BInterface()
            : base("DCS F-14B")
        {
            DCSConfigurator config = new DCSConfigurator("DCSF14B", DCSPath);
            config.ExportConfigPath = "Config\\Export";
            config.ExportFunctionsPath = "pack://application:,,,/Helios;component/Interfaces/DCS/F14B/ExportFunctions.lua";
            Port = config.Port;
            _phantomFix = config.PhantomFix;
            _phantomLeft = config.PhantomFixLeft;
            _phantomTop = config.PhantomFixTop;

            #region hydraulics
            AddFunction(Switch.CreateToggleSwitch(this, HYDRAULICS, "629", "629", "0", "Position 1", "1", "Position 2", "HYDRAULICS", "Hydraulic Transfer Pump Switch", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, HYDRAULICS, "631", "631", "0", "Position 1", "1", "Position 2", "HYDRAULICS", "Hydraulic Isolation Switch", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, HYDRAULICS, "928", "928", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "HYDRAULICS", "Hydraulic Emergency Flight Control Switch", "%1d"));
            #endregion
            #region MASTER RESET
            AddFunction(new PushButton(this, CADC, "1071", "1071", "CADC", "MASTER RESET", "1", "0", "%1d"));
            #endregion
            #region AICS
            AddFunction(Switch.CreateToggleSwitch(this, AICS, "2100", "2100", "0", "Position 1", "1", "Position 2", "AICS", "Stow Inlet Ramps Left Switch", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AICS, "2101", "2101", "0", "Position 1", "1", "Position 2", "AICS", "Stow Inlet Ramps Right Switch", "%1d"));
            #endregion
            #region Wing sweep
            #endregion
            #region Radar altimeter
            #endregion
            #region Airspeed indicator
            #endregion
            #region Altimeter
            AddFunction(new Axis(this, BAROALTIMETER, "306", "306", 0.1d, 0.0d, 1.0d, "BAROALTIMETER", "Altimeter Pressure Setting"));
            #endregion
            #region RIO Altimeter
            AddFunction(new Axis(this, BAROALTIMETER, "20306", "20306", 0.1d, 0.0d, 1.0d, "BAROALTIMETER", "Altimeter Pressure Setting (RIO)"));
            #endregion
            #region Gear
            AddFunction(new PushButton(this, GEARHOOK, "497", "497", "GEARHOOK", "Launch Bar Abort", "1", "0", "%1d"));
            #endregion
            #region Hook
            #endregion
            #region Brakes
            AddFunction(Switch.CreateThreeWaySwitch(this, GEARHOOK, "1072", "1072", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "GEARHOOK", "Anti-Skid Spoiler BK Switch", "%1d"));
            #endregion
            #region SAS
            AddFunction(Switch.CreateToggleSwitch(this, AFCS, "2106", "2106", "0", "Position 1", "1", "Position 2", "AFCS", "AFCS Stability Augmentation - Pitch", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AFCS, "2107", "2107", "0", "Position 1", "1", "Position 2", "AFCS", "AFCS Stability Augmentation - Roll", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AFCS, "2108", "2108", "0", "Position 1", "1", "Position 2", "AFCS", "AFCS Stability Augmentation - Yaw", "%1d"));
            #endregion
            #region Autopilot
            AddFunction(Switch.CreateThreeWaySwitch(this, AFCS, "2109", "2109", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "AFCS", "Autopilot - Vector / Automatic Carrier Landing", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AFCS, "2110", "2110", "0", "Position 1", "1", "Position 2", "AFCS", "Autopilot - Altitude Hold", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, AFCS, "2111", "2111", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "AFCS", "Autopilot - Heading / Ground Track", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AFCS, "2112", "2112", "0", "Position 1", "1", "Position 2", "AFCS", "Autopilot - Engage", "%1d"));
            #endregion
            #region Flaps
            AddFunction(new Axis(this, FLAPS, "225", "225", 0.1d, 0.0d, 1.0d, "FLAPS", "Flaps Lever"));
            #endregion
            #region Engine
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, "THROT_L", "THROT_L", "0", "Position 1", "1", "Position 2", "ENGINE", "Left Engine Fuel Cutoff", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, "THROT_R", "THROT_R", "0", "Position 1", "1", "Position 2", "ENGINE", "Right Engine Fuel Cutoff", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ENGINE, "2104", "2104", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ENGINE", "Throttle Mode", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ENGINE, "2103", "2103", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ENGINE", "Throttle Temp", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ENGINE, "941", "941", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ENGINE", "Engine/Probe Anti-Ice", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, "2105", "2105", "0", "Position 1", "1", "Position 2", "ENGINE", "Engine Airstart", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ENGINE, "2102", "2102", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ENGINE", "Engine Crank", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, "16007", "16007", "0", "Position 1", "1", "Position 2", "ENGINE", "Left Engine Mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, "16008", "16008", "0", "Position 1", "1", "Position 2", "ENGINE", "Right Engine Mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, "16005", "16005", "0", "Position 1", "1", "Position 2", "ENGINE", "Asymmetric Thrust Limiter Cover", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, "16006", "16006", "0", "Position 1", "1", "Position 2", "ENGINE", "Asymmetric Thrust Limiter", "%1d"));
            #endregion
            #region Fire system
            // AddFunction(new PushButton(this, FIRE, "15083", "15083", "FIRE", "Fire Ext Bottle - Left", "1", "0", "%1d"));
            // AddFunction(new PushButton(this, FIRE, "15082", "15082", "FIRE", "Fire Ext Bottle - Right", "1", "0", "%1d"));
            #endregion
            #region Fuel system
            AddFunction(Switch.CreateThreeWaySwitch(this, FUELSYSTEM, "1095", "1095", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "FUELSYSTEM", "Fuel Feed", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, FUELSYSTEM, "1001", "1001", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "FUELSYSTEM", "Wing/Ext Trans", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, FUELSYSTEM, "1074", "1074", "0", "Position 1", "1", "Position 2", "FUELSYSTEM", "Fuel Dump", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, FUELSYSTEM, "1073", "1073", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "FUELSYSTEM", "Refuel Probe", "%1d"));
            AddFunction(new Axis(this, FUELSYSTEM, "1050", "1050", 0.1d, 0.0d, 1.0d, "FUELSYSTEM", "BINGO Fuel Level Knob"));

            #endregion
            #region Electrics
            AddFunction(Switch.CreateThreeWaySwitch(this, ELECTRICS, "937", "937", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ELECTRICS", "Left Generator Switch", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ELECTRICS, "936", "936", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ELECTRICS", "Right Generator Switch", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ELECTRICS, "926", "926", "0", "Position 1", "1", "Position 2", "ELECTRICS", "Emergency Generator Switch", "%1d"));
            #endregion
            #region Cockpit mechanics
            #endregion
            #region Enivornment control
            AddFunction(Switch.CreateToggleSwitch(this, COCKPITMECHANICS, "8114", "8114", "0", "Position 1", "1", "Position 2", "COCKPITMECHANICS", "Pilot Oxygen On", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, COCKPITMECHANICS, "119", "119", "0", "Position 1", "1", "Position 2", "COCKPITMECHANICS", "RIO Oxygen On", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, COCKPITMECHANICS, "939", "939", "0", "Position 1", "1", "Position 2", "COCKPITMECHANICS", "Cabin Pressure Dump", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, COCKPITMECHANICS, "942", "942", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "COCKPITMECHANICS", "Wind Shield Air", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, COCKPITMECHANICS, "940", "940", "0", "Position 1", "1", "Position 2", "COCKPITMECHANICS", "Temp Auto / Man", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, COCKPITMECHANICS, "938", "938", "0", "Position 1", "1", "Position 2", "COCKPITMECHANICS", "Ram Air", "%1d"));
            #endregion
            #region BIT panel
            #endregion
            #region Light panel
            AddFunction(Switch.CreateToggleSwitch(this, AOASYSTEM, "915", "915", "0", "Position 1", "1", "Position 2", "AOASYSTEM", "Hook Bypass", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, COCKPITMECHANICS, "918", "918", "0", "Position 1", "1", "Position 2", "COCKPITMECHANICS", "Taxi Light", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, COCKPITMECHANICS, "924", "924", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "COCKPITMECHANICS", "Red Flood Light", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, COCKPITMECHANICS, "921", "921", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "COCKPITMECHANICS", "White Flood Light", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, COCKPITMECHANICS, "913_22", "913_22", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "COCKPITMECHANICS", "Position Lights Wing", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, COCKPITMECHANICS, "916", "916", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "COCKPITMECHANICS", "Position Lights Tail", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, COCKPITMECHANICS, "919", "919", "0", "Position 1", "1", "Position 2", "COCKPITMECHANICS", "Position Lights Flash", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, COCKPITMECHANICS, "923", "923", "0", "Position 1", "1", "Position 2", "COCKPITMECHANICS", "Anti-Collision Lights", "%1d"));
            #endregion
            #region Light panel RIO
            AddFunction(Switch.CreateThreeWaySwitch(this, COCKPITMECHANICS, "194", "194", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "COCKPITMECHANICS", "Red Flood Light (RIO)", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, COCKPITMECHANICS, "159", "159", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "COCKPITMECHANICS", "White Flood Light (RIO)", "%1d"));
            #endregion
            #region DISPLAY Panel: Power
            AddFunction(Switch.CreateToggleSwitch(this, VDI, "1010", "1010", "0", "Position 1", "1", "Position 2", "VDI", "VDI Power On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, HUD, "1009", "1009", "0", "Position 1", "1", "Position 2", "HUD", "HUD Power On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, HSD, "1008", "1008", "0", "Position 1", "1", "Position 2", "HSD", "HSD/ECMD Power On/Off", "%1d"));
            #endregion
            #region DISPLAY Panel: Steer CMD
            #endregion
            #region DISPLAY Panel: HSD
            AddFunction(Switch.CreateToggleSwitch(this, HSD, "1016", "1016", "0", "Position 1", "1", "Position 2", "HSD", "HSD Display Mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, HSD, "1017", "1017", "0", "Position 1", "1", "Position 2", "HSD", "HSD ECM Override", "%1d"));
            #endregion
            #region HSD
            AddFunction(new Axis(this, HSD, "1039", "1039", 0.1d, 0.0d, 1.0d, "HSD", "HSD Selected Heading"));
            AddFunction(new Axis(this, HSD, "1040", "1040", 0.1d, 0.0d, 1.0d, "HSD", "HSD Selected Course"));
            AddFunction(new Axis(this, HSD, "1043", "1043", 0.1d, 0.0d, 1.0d, "HSD", "HSD Brightness"));
            AddFunction(new PushButton(this, HSD, "1041", "1041", "HSD", "HSD Test", "1", "0", "%1d"));
            #endregion
            #region ECMD
            AddFunction(new Axis(this, ECMD, "2023", "2023", 0.1d, 0.0d, 1.0d, "ECMD", "ECMD Brightness"));
            AddFunction(new PushButton(this, ECMD, "2024", "2024", "ECMD", "ECMD Test", "1", "0", "%1d"));
            #endregion
            #region ECMD Panel
            #endregion
            #region TACAN CMD
            AddFunction(new PushButton(this, TACAN, "292", "292", "TACAN", "TACAN CMD Button (RIO)", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TACAN, "135", "135", "TACAN", "TACAN CMD Button", "1", "0", "%1d"));
            #endregion
            #region TACAN Pilot Panel
            AddFunction(new Axis(this, TACAN, "2036", "2036", 0.1d, 0.0d, 1.0d, "TACAN", "TACAN Volume"));
            AddFunction(Switch.CreateToggleSwitch(this, TACAN, "2042", "2042", "0", "Position 1", "1", "Position 2", "TACAN", "TACAN Mode Normal/Inverse", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, TACAN, "2043", "2043", "0", "Position 1", "1", "Position 2", "TACAN", "TACAN Channel X/Y", "%1d"));
            AddFunction(new PushButton(this, TACAN, "2115", "2115", "TACAN", "TACAN BIT", "1", "0", "%1d"));
            #endregion
            #region TACAN RIO Panel
            AddFunction(new Axis(this, TACAN, "375", "375", 0.1d, 0.0d, 1.0d, "TACAN", "TACAN Volume (RIO)"));
            AddFunction(Switch.CreateToggleSwitch(this, TACAN, "373", "373", "0", "Position 1", "1", "Position 2", "TACAN", "TACAN Mode Normal/Inverse (RIO)", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, TACAN, "372", "372", "0", "Position 1", "1", "Position 2", "TACAN", "TACAN Channel X/Y (RIO)", "%1d"));
            AddFunction(new PushButton(this, TACAN, "371", "371", "TACAN", "TACAN BIT (RIO)", "1", "0", "%1d"));
            #endregion
            #region AN/ARA-63 Panel
            AddFunction(Switch.CreateToggleSwitch(this, ILS, "910", "910", "0", "Position 1", "1", "Position 2", "ILS", "AN/ARA-63 Power Switch", "%1d"));
            AddFunction(new PushButton(this, ILS, "911", "911", "ILS", "AN/ARA-63 BIT Button", "1", "0", "%1d"));
            #endregion
            #region Pilot TONE VOLUME panel
            AddFunction(new Axis(this, ICS, "2040", "2040", 0.1d, 0.0d, 1.0d, "ICS", "ALR-67 Volume"));
            AddFunction(new Axis(this, ICS, "2039", "2039", 0.1d, 0.0d, 1.0d, "ICS", "Sidewinder Volume"));
            #endregion
            #region ICS Pilot
            AddFunction(new Axis(this, ICS, "2047", "2047", 0.1d, 0.0d, 1.0d, "ICS", "ICS Volume"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ICS, "2044", "2044", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ICS", "ICS Function Selector", "%1d"));
            #endregion
            #region ICS RIO
            AddFunction(new Axis(this, ICS, "400", "400", 0.1d, 0.0d, 1.0d, "ICS", "ICS Volume (RIO)"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ICS, "402", "402", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ICS", "ICS Function Selector (RIO)", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ICS, "381", "381", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ICS", "XMTR SEL Switch (RIO)", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ICS, "380", "380", "0", "Position 1", "1", "Position 2", "ICS", "V/UHF 2 ANT Switch (RIO)", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ICS, "382", "382", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "ICS", "KY MODE Switch (RIO)", "%1d"));
            #endregion
            #region UHF ARC-159
            AddFunction(Switch.CreateToggleSwitch(this, ARC159, "2035", "2035", "0", "Position 1", "1", "Position 2", "ARC159", "UHF ARC-159 Squelch Switch", "%1d"));
            AddFunction(new PushButton(this, ARC159, "8115", "8115", "ARC159", "UHF ARC-159 Read", "1", "0", "%1d"));
            AddFunction(new Axis(this, ARC159, "2031", "2031", 0.1d, 0.0d, 1.0d, "ARC159", "UHF ARC-159 Volume Pilot"));
            AddFunction(new Axis(this, ARC159, "383", "383", 0.1d, 0.0d, 1.0d, "ARC159", "UHF ARC-159 Volume RIO"));
            AddFunction(new Axis(this, ARC159, "2027", "2027", 0.1d, 0.0d, 1.0d, "ARC159", "UHF ARC-159 Display Brightness"));
            #endregion
            #region UHF ARC-159 End
            #endregion
            #region VHF/UHF ARC-182
            AddFunction(Switch.CreateToggleSwitch(this, ARC182, "359", "359", "0", "Position 1", "1", "Position 2", "ARC182", "VHF/UHF ARC-182 FM/AM Switch", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ARC182, "351", "351", "0", "Position 1", "1", "Position 2", "ARC182", "VHF/UHF ARC-182 Squelch Switch", "%1d"));
            AddFunction(new Axis(this, ARC182, "350", "350", 0.1d, 0.0d, 1.0d, "ARC182", "VHF/UHF ARC-182 Volume RIO"));
            AddFunction(new Axis(this, ARC182, "2038", "2038", 0.1d, 0.0d, 1.0d, "ARC182", "VHF/UHF ARC-182 Volume Pilot"));
            AddFunction(new Axis(this, ARC182, "360", "360", 0.1d, 0.0d, 1.0d, "ARC182", "VHF/UHF ARC-182 Display Brightness"));
            #endregion
            #region KY-28
            AddFunction(new PushButton(this, ICS, "361", "361", "ICS", "KY-28 ZEROIZE", "1", "0", "%1d"));
            #endregion
            #region UHF/VHF/UHF Pilot/RIO Remote Display
            AddFunction(new Axis(this, ARC159, "1031", "1031", 0.1d, 0.0d, 1.0d, "ARC159", "UHF Radio Remote Display Brightness"));
            AddFunction(new Axis(this, ARC159, "406", "406", 0.1d, 0.0d, 1.0d, "ARC159", "UHF Radio Remote Display Brightness (RIO)"));
            AddFunction(new Axis(this, ARC182, "1030", "1030", 0.1d, 0.0d, 1.0d, "ARC182", "VHF/UHF Radio Remote Display Brightness"));
            AddFunction(new PushButton(this, ARC159, "BIT_TEST", "BIT_TEST", "ARC159", "UHF Radio Remote Display Test", "1", "0", "%1d"));
            AddFunction(new PushButton(this, ARC159, "405", "405", "ARC159", "UHF Radio Remote Display Test (2)", "1", "0", "%1d"));
            AddFunction(new PushButton(this, ARC182, "BIT_TEST", "BIT_TEST", "ARC182", "VHF/UHF Radio Remote Display Test", "1", "0", "%1d"));

            #endregion
            #region DECM Panel
            AddFunction(new Axis(this, DECM, "9950", "9950", 0.1d, 0.0d, 1.0d, "DECM", "DECM ALQ-100 Volume"));
            #endregion
            #region RWR Control Panel ALR-67
            AddFunction(new Axis(this, RWR, "16011", "16011", 0.1d, 0.0d, 1.0d, "RWR", "AN/ALR-67 Display Brightness (RIO)"));
            AddFunction(new Axis(this, RWR, "376", "376", 0.1d, 0.0d, 1.0d, "RWR", "AN/ALR-67 Display Brightness"));
            AddFunction(Switch.CreateToggleSwitch(this, RWR, "2139", "2139", "0", "Position 1", "1", "Position 2", "RWR", "AN/ALR-67 Power", "%1d"));
            AddFunction(new Axis(this, ICS, "2138", "2138", 0.1d, 0.0d, 1.0d, "ICS", "AN/ALR-67 Volume"));
            #endregion
            #region AN/ALE-39 Mode Panel
            AddFunction(Switch.CreateThreeWaySwitch(this, COUNTERMEASURES, "390", "390", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "COUNTERMEASURES", "AN/ALE-37 Power/Mode", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, COUNTERMEASURES, "398", "398", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "COUNTERMEASURES", "AN/ALE-37 Flare Mode", "%1d"));
            AddFunction(new PushButton(this, COUNTERMEASURES, "391", "391", "COUNTERMEASURES", "AN/ALE-37 Flare Salvo", "1", "0", "%1d"));
            AddFunction(new Axis(this, COUNTERMEASURES, "386", "386", 0.1d, 0.0d, 1.0d, "COUNTERMEASURES", "AN/ALE-37 Chaff Counter"));
            AddFunction(new Axis(this, COUNTERMEASURES, "385", "385", 0.1d, 0.0d, 1.0d, "COUNTERMEASURES", "AN/ALE-37 Flare Counter"));
            AddFunction(new Axis(this, COUNTERMEASURES, "399", "399", 0.1d, 0.0d, 1.0d, "COUNTERMEASURES", "AN/ALE-37 Jammer Counter"));
            #endregion
            #region AN/ALE-39 Program Panel
            AddFunction(new PushButton(this, COUNTERMEASURES, "216", "216", "COUNTERMEASURES", "AN/ALE-37 Programmer Reset", "1", "0", "%1d"));
            #endregion
            #region INS
            #endregion
            #region AHRS / compass
            AddFunction(Switch.CreateThreeWaySwitch(this, AHRS, "905", "905", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "AHRS", "Compass Mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AHRS, "906", "906", "0", "Position 1", "1", "Position 2", "AHRS", "Compass N-S Hemisphere", "%1d"));
            AddFunction(new Axis(this, AHRS, "909", "909", 0.1d, 0.0d, 1.0d, "AHRS", "Compass LAT Correction"));
            #endregion
            #region Copied temp so easy to copy: (I'm lazy, sue me)
            #endregion
            #region Spoiler Overrides
            AddFunction(Switch.CreateToggleSwitch(this, ELECTRICS, "902", "902", "0", "Position 1", "1", "Position 2", "ELECTRICS", "Inboard Spoiler Override Cover", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ELECTRICS, "903", "903", "0", "Position 1", "1", "Position 2", "ELECTRICS", "Outboard Spoiler Override Cover", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ELECTRICS, "908", "908", "0", "Position 1", "1", "Position 2", "ELECTRICS", "Inboard Spoiler Override", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ELECTRICS, "907", "907", "0", "Position 1", "1", "Position 2", "ELECTRICS", "Outboard Spoiler Override", "%1d"));
            #endregion
            #region Gun Elevation
            AddFunction(new Axis(this, WEAPONS, "1000", "1000", 0.1d, 0.0d, 1.0d, "WEAPONS", "Gun Elevation Lead Adjustment"));
            AddFunction(new Axis(this, WEAPONS, "1022", "1022", 0.1d, 0.0d, 1.0d, "WEAPONS", "Gun Ammunition Counter Adjustment"));
            #endregion
            #region DISPLAY Panel
            AddFunction(new Axis(this, HUD, "1007", "1007", 0.1d, 0.0d, 1.0d, "HUD", "HUD Pitch Ladder Brightness"));
            AddFunction(Switch.CreateToggleSwitch(this, VDI, "1019", "1019", "0", "Position 1", "1", "Position 2", "VDI", "VDI Display Mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, VDI, "1018", "1018", "0", "Position 1", "1", "Position 2", "VDI", "VDI Landing Mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, HUD, "1021", "1021", "0", "Position 1", "1", "Position 2", "HUD", "HUD De-clutter On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, HUD, "1020", "1020", "0", "Position 1", "1", "Position 2", "HUD", "HUD AWL Mode", "%1d"));
            #endregion
            #region DISPLAY End
            #endregion
            #region Stdby ADI Pilot
            AddFunction(new PushButton(this, ACCELEROMETER, "228", "228", "ACCELEROMETER", "Accelerometer Reset", "1", "0", "%1d"));
            #endregion
            #region VDI & HUD Indicator controls
            AddFunction(new Axis(this, HUD, "1034", "1034", 0.1d, 0.0d, 1.0d, "HUD", "HUD Trim"));
            AddFunction(new Axis(this, VDI, "1035", "1035", 0.1d, 0.0d, 1.0d, "VDI", "VSDI Screen Trim"));
            AddFunction(new Axis(this, VDI, "1038", "1038", 0.1d, 0.0d, 1.0d, "VDI", "VDI Screen Contrast"));
            AddFunction(new Axis(this, VDI, "1036", "1036", 0.1d, 0.0d, 1.0d, "VDI", "VSDI Screen Brightness"));
            AddFunction(new Axis(this, HUD, "1037", "1037", 0.1d, 0.0d, 1.0d, "HUD", "HUD Brightness"));
            AddFunction(new PushButton(this, VDI, "VDI", "VDI", "VDI", "VDI filter", "1", "0", "%1d"));
            #endregion
            #region Under HUD / Master Arm / Gun/Weapons Panel
            AddFunction(Switch.CreateThreeWaySwitch(this, WEAPONS, "1047", "1047", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "WEAPONS", "Master Arm Switch", "%1d"));
            AddFunction(new PushButton(this, WEAPONS, "1048", "1048", "WEAPONS", "ACM Jettison", "1", "0", "%1d"));
            AddFunction(new PushButton(this, WARNINGLIGHTS, "9199", "9199", "WARNINGLIGHTS", "Master Caution Reset", "1", "0", "%1d"));
            AddFunction(new PushButton(this, WEAPONS, "GUN_RATE", "GUN_RATE", "WEAPONS", "Gun rate", "1", "0", "%1d"));
            AddFunction(new PushButton(this, WEAPONS, "SW_COOL", "SW_COOL", "WEAPONS", "Sidewinder cool", "1", "0", "%1d"));
            AddFunction(new PushButton(this, WEAPONS, "MSL_PREP", "MSL_PREP", "WEAPONS", "Missile prepare", "1", "0", "%1d"));
            AddFunction(new PushButton(this, WEAPONS, "MSL_MODE", "MSL_MODE", "WEAPONS", "Missile mode", "1", "0", "%1d"));
            AddFunction(new PushButton(this, WEAPONS, "239", "239", "WEAPONS", "Emergency stores jettison", "1", "0", "%1d"));
            AddFunction(new Axis(this, CLOCK, "1051", "1051", 0.1d, 0.0d, 1.0d, "CLOCK", "Clock Wind"));
            AddFunction(new PushButton(this, CLOCK, "CLOCK_RESET", "CLOCK_RESET", "CLOCK", "Clock Timer Start/Stop/Reset", "1", "0", "%1d"));
            AddFunction(new Axis(this, CLOCK, "1052", "1052", 0.1d, 0.0d, 1.0d, "CLOCK", "Clock Wind (2)"));
            AddFunction(new PushButton(this, CLOCK, "1053", "1053", "CLOCK", "Clock Timer Start/Stop/Reset (2)", "1", "0", "%1d"));
            #endregion
            #region RIO TID
            AddFunction(new PushButton(this, TID, "226", "226", "TID", "TID Non attack", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "1118", "1118", "TID", "TID Jam strobe", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "1117", "1117", "TID", "TID Data link", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "1116", "1116", "TID", "TID Sym Elem", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "1115", "1115", "TID", "TID Alt num", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "2004", "2004", "TID", "TID Reject Image Device disable no function", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "2113", "2113", "TID", "TID Launch zone", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "2114", "2114", "TID", "TID Velocity vector", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "52", "52", "TID", "collision steering", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "53", "53", "TID", "TID track hold", "1", "0", "%1d"));
            AddFunction(new Axis(this, TID, "48", "48", 0.1d, 0.0d, 1.0d, "TID", "TID Brightness"));
            AddFunction(new Axis(this, TID, "49", "49", 0.1d, 0.0d, 1.0d, "TID", "TID Contrast"));
            #endregion
            #region RIO HCU
            AddFunction(Switch.CreateToggleSwitch(this, HCU, "2007", "2007", "0", "Position 1", "1", "Position 2", "HCU", "HCU TCS mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, HCU, "2008", "2008", "0", "Position 1", "1", "Position 2", "HCU", "HCU radar mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, HCU, "2009", "2009", "0", "Position 1", "1", "Position 2", "HCU", "HCU DDD mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, HCU, "2010", "2010", "0", "Position 1", "1", "Position 2", "HCU", "HCU TID mode", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, TCS, "2011", "2011", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "TCS", "TV/IR switch", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, RADAR, "2012", "2012", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "RADAR", "WCS switch", "%1d"));
            AddFunction(new PushButton(this, RADAR, "2013", "2013", "RADAR", "Power reset", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "2014", "2014", "RADAR", "Light test", "1", "0", "%1d"));
            #endregion
            #region RIO DDD
            AddFunction(new PushButton(this, RADAR, "40", "40", "RADAR", "RADAR 5 NM", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "41", "41", "RADAR", "RADAR 10 NM", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "42", "42", "RADAR", "RADAR 20 NM", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "43", "43", "RADAR", "RADAR 50 NM", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "44", "44", "RADAR", "RADAR 100 NM", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "45", "45", "RADAR", "RADAR 200 NM", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "DDD_CENTRw", "DDD_CENTRw", "RADAR", "DDD filter", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "10", "10", "RADAR", "RADAR pulse search", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "11", "11", "RADAR", "RADAR track while scan manual", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "12", "12", "RADAR", "RADAR track while scan auto", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "13", "13", "RADAR", "RADAR range while scan", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "14", "14", "RADAR", "RADAR pulse doppler search", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "15", "15", "RADAR", "RADAR pulse single target track", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "9916", "9916", "RADAR", "RADAR pulse doppler single target track", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "17", "17", "RADAR", "DDD Interrogate Friend or Foe", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "18", "18", "RADAR", "DDD Infrared no function", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "19", "19", "RADAR", "DDD RADAR", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "1812", "1812", "RADAR", "CCM SPL no function", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "1813", "1813", "RADAR", "CCM ALT DIFF no function", "1", "0", "%1d"));
            AddFunction(new PushButton(this, RADAR, "1814", "1814", "RADAR", "CCM VGS no function", "1", "0", "%1d"));
            AddFunction(new Axis(this, RADAR, "20", "20", 0.1d, 0.0d, 1.0d, "RADAR", "IR gain no function"));
            AddFunction(new Axis(this, RADAR, "21", "21", 0.1d, 0.0d, 1.0d, "RADAR", "IR volume no function"));
            AddFunction(new Axis(this, RADAR, "22", "22", 0.1d, 0.0d, 1.0d, "RADAR", "IR threshold no function"));
            AddFunction(new Axis(this, RADAR, "23", "23", 0.1d, 0.0d, 1.0d, "RADAR", "Brightness"));
            AddFunction(new Axis(this, RADAR, "24", "24", 0.1d, 0.0d, 1.0d, "RADAR", "Pulse video"));
            AddFunction(new Axis(this, RADAR, "25", "25", 0.1d, 0.0d, 1.0d, "RADAR", "Erase"));
            AddFunction(new Axis(this, RADAR, "26", "26", 0.1d, 0.0d, 1.0d, "RADAR", "Pulse gain"));
            AddFunction(new Axis(this, RADAR, "27", "27", 0.1d, 0.0d, 1.0d, "RADAR", "ACM threshold no function"));
            AddFunction(new Axis(this, RADAR, "28", "28", 0.1d, 0.0d, 1.0d, "RADAR", "JAM/JET no function"));
            AddFunction(new Axis(this, RADAR, "29", "29", 0.1d, 0.0d, 1.0d, "RADAR", "PD threshold clutter"));
            AddFunction(new Axis(this, RADAR, "30", "30", 0.1d, 0.0d, 1.0d, "RADAR", "PD threshold clear no function"));
            AddFunction(Switch.CreateThreeWaySwitch(this, RADAR, "34", "34", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "RADAR", "Aspect", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, RADAR, "35", "35", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "RADAR", "Closing Velocity scale", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, RADAR, "36", "36", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "RADAR", "Target size no function", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, RADAR, "37", "37", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "RADAR", "Main Lobe Clutter filter", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, RADAR, "38", "38", "0", "Position 1", "1", "Position 2", "RADAR", "Automatic Gain Control no function", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, RADAR, "3900", "3900", "0", "Position 1", "1", "Position 2", "RADAR", "Parametric amplifier no function", "%1d"));
            #endregion
            #region RIO RADAR panel
            AddFunction(new Axis(this, RADAR, "81", "81", 0.1d, 0.0d, 1.0d, "RADAR", "Radar elevation center"));
            AddFunction(new Axis(this, RADAR, "82", "82", 0.1d, 0.0d, 1.0d, "RADAR", "Radar azimuth center"));
            AddFunction(Switch.CreateToggleSwitch(this, RADAR, "83", "83", "0", "Position 1", "1", "Position 2", "RADAR", "Stabilize", "%1d"));
            #endregion
            #region RIO TCS controls
            AddFunction(Switch.CreateThreeWaySwitch(this, TCS, "87", "87", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "TCS", "TCS Acquisition", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, TCS, "88", "88", "0", "Position 1", "1", "Position 2", "TCS", "TCS FOV", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, TCS, "89", "89", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "TCS", "TCS Slave", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, TCS, "90", "90", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "TCS", "Record power no function", "%1d"));
            AddFunction(new Axis(this, TCS, "16016", "16016", 0.1d, 0.0d, 1.0d, "TCS", "Record reset no function"));
            #endregion
            #region RIO armament panel
            AddFunction(Switch.CreateThreeWaySwitch(this, WEAPONS, "63", "63", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "WEAPONS", "Mech fuse", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, WEAPONS, "75", "75", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "WEAPONS", "Missile option", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "60", "60", "0", "Position 1", "1", "Position 2", "WEAPONS", "Bomb single/pairs", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "61", "61", "0", "Position 1", "1", "Position 2", "WEAPONS", "Bomb step/ripple", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "62", "62", "0", "Position 1", "1", "Position 2", "WEAPONS", "A/G gun mode", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "66", "66", "0", "Position 1", "1", "Position 2", "WEAPONS", "Jettison racks/weapons", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "73", "73", "0", "Position 1", "1", "Position 2", "WEAPONS", "Jettison left tank", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "67", "67", "0", "Position 1", "1", "Position 2", "WEAPONS", "Jettison right tank", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, WEAPONS, "68", "68", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "WEAPONS", "Jettison station 1", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "65", "65", "0", "Position 1", "1", "Position 2", "WEAPONS", "Jettison station 3", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "69", "69", "0", "Position 1", "1", "Position 2", "WEAPONS", "Jettison station 4", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "70", "70", "0", "Position 1", "1", "Position 2", "WEAPONS", "Jettison station 5", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPONS, "64", "64", "0", "Position 1", "1", "Position 2", "WEAPONS", "Jettison station 6", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, WEAPONS, "71", "71", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "WEAPONS", "Jettison station 8", "%1d"));
            AddFunction(new PushButton(this, WEAPONS, "74", "74", "WEAPONS", "A/A Launch", "1", "0", "%1d"));
            AddFunction(new PushButton(this, TID, "9964", "9964", "TID", "Next Launch", "1", "0", "%1d"));
            #endregion
            #region Computer Address Panel (CAP)
            AddFunction(new PushButton(this, CAP, "123", "123", "CAP", "CAP btn 4", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "124", "124", "CAP", "CAP btn 5", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "125", "125", "CAP", "CAP btn 3", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "126", "126", "CAP", "CAP btn 2", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "127", "127", "CAP", "CAP btn 1", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "128", "128", "CAP", "CAP TNG NBR", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "129", "129", "CAP", "CAP btn 10", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "130", "130", "CAP", "CAP btn 9", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "131", "131", "CAP", "CAP btn 8", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "132", "132", "CAP", "CAP btn 7", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "133", "133", "CAP", "CAP btn 6", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "134", "134", "CAP", "CAP PGM RSTRT", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "136", "136", "CAP", "CAP LONG 6", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "137", "137", "CAP", "CAP LAT 1", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "138", "138", "CAP", "CAP NBR 2", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "139", "139", "CAP", "CAP 7", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "140", "140", "CAP", "CAP HDG 8", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "141", "141", "CAP", "CAP SPD 3", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "143", "143", "CAP", "CAP ALT 4", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "142", "142", "CAP", "CAP 9", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "144", "144", "CAP", "CAP BRG 0", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "145", "145", "CAP", "CAP RNG 5", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "146", "146", "CAP", "CAP N+E", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "147", "147", "CAP", "CAP S-W", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "148", "148", "CAP", "CAP clear", "1", "0", "%1d"));
            AddFunction(new PushButton(this, CAP, "149", "149", "CAP", "CAP enter", "1", "0", "%1d"));
            #endregion
            #region datalink
            AddFunction(Switch.CreateThreeWaySwitch(this, DATALINK, "413", "413", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "DATALINK", "Datalink Power", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, DATALINK, "175", "175", "0", "Position 1", "1", "Position 2", "DATALINK", "Datalink Antenna no function", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, DATALINK, "176", "176", "0", "Position 1", "1", "Position 2", "DATALINK", "Datalink Reply no function", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, DATALINK, "177", "177", "0", "Position 1", "1", "Position 2", "DATALINK", "Datalink CAINS/TAC", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, DATALINK, "191", "191", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "DATALINK", "Datalink Antijam no function", "%1d"));
            AddFunction(new PushButton(this, DATALINK, "117", "117", "DATALINK", "ACLS test", "1", "0", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, DATALINK, "96", "96", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "DATALINK", "Beacon Power", "%1d"));
            #endregion
            #region IFF panel
            AddFunction(Switch.CreateThreeWaySwitch(this, IFF, "161", "161", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "IFF", "IFF audio/light no function", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, IFF, "162", "162", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "IFF", "IFF M1 no function", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, IFF, "163", "163", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "IFF", "IFF M2 no function", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, IFF, "164", "164", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "IFF", "IFF M3/A no function", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, IFF, "165", "165", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "IFF", "IFF MC no function", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, IFF, "166", "166", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "IFF", "IFF RAD no function", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, IFF, "167", "167", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "IFF", "IFF Ident no function", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, IFF, "181", "181", "0", "Position 1", "1", "Position 2", "IFF", "IFF M4 no function", "%1d"));
            AddFunction(new PushButton(this, IFF, "185", "185", "IFF", "IFF test no function", "1", "0", "%1d"));
            AddFunction(new PushButton(this, IFF, "186", "186", "IFF", "IFF reply no function", "1", "0", "%1d"));
            #endregion
            #region LIQUID cooling
            AddFunction(Switch.CreateThreeWaySwitch(this, RADAR, "95", "95", "-1", "Position 1", "0", "Position 2", "1", "Position 3", "RADAR", "Liquid cooling", "%1d"));
            #endregion
            #region LANTIRN
            AddFunction(Switch.CreateToggleSwitch(this, LANTIRN, "668", "668", "0", "Position 1", "1", "Position 2", "LANTIRN", "LANTIRN Laser Arm Switch", "%1d"));
            AddFunction(new PushButton(this, TID, "670", "670", "TID", "Video Output Toggle TCS/LANTIRN", "1", "0", "%1d"));
            AddFunction(new PushButton(this, LANTIRN, "669", "669", "LANTIRN", "LANTIRN Operate Mode Unstow", "1", "0", "%1d"));
            AddFunction(new PushButton(this, LANTIRN, "671", "671", "LANTIRN", "LANTIRN IBIT", "1", "0", "%1d"));
            #endregion
        }

        private string DCSPath
        {
            get
            {
                if (_dcsPath == null)
                {
                    RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS F-14B");
                    if (pathKey != null)
                    {
                        _dcsPath = (string)pathKey.GetValue("Path");
                        pathKey.Close();
                        ConfigManager.LogManager.LogDebug("DCS F-14B Interface Editor - Found DCS Path (Path=\"" + _dcsPath + "\")");
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

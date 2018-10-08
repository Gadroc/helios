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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.M2000CSimple
{
    using GadrocsWorkshop.Helios.ComponentModel;
    //using GadrocsWorkshop.Helios.Interfaces.DCS.M2000CSimple.Functions;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using Microsoft.Win32;
    using System;

    [HeliosInterface("Helios.M2000CSimple", "DCS M2000C (Simple)", typeof(M2000CSimpleInterfaceEditor), typeof(UniqueHeliosInterfaceFactory))]
    public class M2000CSimpleInterface : BaseUDPInterface
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
        public M2000CSimpleInterface()
            : base("DCS Mirage-2000C (Simple)")
        {
            DCSConfigurator config = new DCSConfigurator("DCSM2000CSIMPLE", DCSPath);
            config.ExportConfigPath = "Config\\Export";
            config.ExportFunctionsPath = "pack://application:,,,/Helios;component/Interfaces/DCS/M2000CSimple/ExportFunctions.lua";
            Port = config.Port;
            _phantomFix = config.PhantomFix;
            _phantomLeft = config.PhantomFixLeft;
            _phantomTop = config.PhantomFixTop;

            #region Indicators
            // !!!! Any duplicate "name" values in a function will cause Helios to go bang.  Make sure that when you change the name, that it is unique
            AddFunction(new FlagValue(this, "185", "Indicators", "Indicators 185", "LIM, MIP,"));
            AddFunction(new FlagValue(this, "186", "Indicators", "Indicators 186", "IFF, MIP, Acc"));
            AddFunction(new FlagValue(this, "187", "Indicators", "Indicators 187", "LED green, ADI"));
            AddFunction(new FlagValue(this, "188", "Indicators", "Indicators 188", "LED green, ADI"));
            AddFunction(new FlagValue(this, "229", "Indicators", "Indicators 229", "RWR V"));
            AddFunction(new FlagValue(this, "230", "Indicators", "Indicators 230", "RWR BR"));
            AddFunction(new FlagValue(this, "231", "Indicators", "Indicators 231", "RWR DA"));
            AddFunction(new FlagValue(this, "232", "Indicators", "Indicators 232", "RWR D2M"));
            AddFunction(new FlagValue(this, "233", "Indicators", "Indicators 233", "RWR LL"));
            AddFunction(new FlagValue(this, "267", "Indicators", "Indicators 267", "Autopilot button P light"));
            AddFunction(new FlagValue(this, "268", "Indicators", "Indicators 268", "Autopilot button MIS light"));
            AddFunction(new FlagValue(this, "270", "Indicators", "Indicators 270", "Autopilot button AUT light"));
            AddFunction(new FlagValue(this, "271", "Indicators", "Indicators 271", "Autopilot button MAN light"));
            AddFunction(new FlagValue(this, "273", "Indicators", "Indicators 273", "Autopilot button MAG-P light"));
            AddFunction(new FlagValue(this, "274", "Indicators", "Indicators 274", "Autopilot button MAG-MAG light"));
            AddFunction(new FlagValue(this, "280", "Indicators", "Indicators 280", "button TOT light"));
            AddFunction(new FlagValue(this, "281", "Indicators", "Indicators 281", "button PAP light"));
            AddFunction(new FlagValue(this, "236", "Indicators", "Indicators 236", "Weapon panel 1. line B1 S"));
            AddFunction(new FlagValue(this, "238", "Indicators", "Indicators 238", "Weapon panel 1. line B2 S"));
            AddFunction(new FlagValue(this, "240", "Indicators", "Indicators 240", "Weapon panel 1. line B3 S"));
            AddFunction(new FlagValue(this, "242", "Indicators", "Indicators 242", "Weapon panel 1. line B4 S"));
            AddFunction(new FlagValue(this, "244", "Indicators", "Indicators 244", "Weapon panel 1. line B5 S"));
            AddFunction(new FlagValue(this, "251", "Indicators", "Indicators 251", "Weapon panel 2. line B1 S"));
            AddFunction(new FlagValue(this, "252", "Indicators", "Indicators 252", "Weapon panel 2. line B1 P"));
            AddFunction(new FlagValue(this, "254", "Indicators", "Indicators 254", "Weapon panel 2. line B2 S"));
            AddFunction(new FlagValue(this, "255", "Indicators", "Indicators 255", "Weapon panel 2. line B2 P"));
            AddFunction(new FlagValue(this, "257", "Indicators", "Indicators 257", "Weapon panel 2. line B3 S"));
            AddFunction(new FlagValue(this, "258", "Indicators", "Indicators 258", "Weapon panel 2. line B3 P"));
            AddFunction(new FlagValue(this, "260", "Indicators", "Indicators 260", "Weapon panel 2. line B4 S"));
            AddFunction(new FlagValue(this, "261", "Indicators", "Indicators 261", "Weapon panel 2. line B4 P"));
            AddFunction(new FlagValue(this, "263", "Indicators", "Indicators 263", "Weapon panel 2. line B5 S"));
            AddFunction(new FlagValue(this, "264", "Indicators", "Indicators 264", "Weapon panel 2. line B5 P"));
            AddFunction(new FlagValue(this, "246", "Indicators", "Indicators 246", "button KL1 light"));
            AddFunction(new FlagValue(this, "247", "Indicators", "Indicators 247", "button KL2 light"));
            AddFunction(new FlagValue(this, "321", "Indicators", "Indicators 321", "ADI ILS light"));
            AddFunction(new FlagValue(this, "374", "Indicators", "Indicators 374", "Fire light 1"));
            AddFunction(new FlagValue(this, "375", "Indicators", "Indicators 375", "Fire light 2"));
            AddFunction(new FlagValue(this, "283", "Indicators", "Indicators 283", "AUTOPILOT P"));
            AddFunction(new FlagValue(this, "284", "Indicators", "Indicators 284", "AUTOPILOT A"));
            AddFunction(new FlagValue(this, "286", "Indicators", "Indicators 286", "AUTOPILOT Alt 1"));
            AddFunction(new FlagValue(this, "287", "Indicators", "Indicators 287", "AUTOPILOT blank Alt"));
            AddFunction(new FlagValue(this, "289", "Indicators", "Indicators 289", "AUTOPILOT Alt 2"));
            AddFunction(new FlagValue(this, "290", "Indicators", "Indicators 290", "AUTOPILOT AFF"));
            AddFunction(new FlagValue(this, "292", "Indicators", "Indicators 292", "AUTOPILOT blank1"));
            AddFunction(new FlagValue(this, "293", "Indicators", "Indicators 293", "AUTOPILOT blank2"));
            AddFunction(new FlagValue(this, "295", "Indicators", "Indicators 295", "AUTOPILOT left"));
            AddFunction(new FlagValue(this, "296", "Indicators", "Indicators 296", "AUTOPILOT blank L"));
            AddFunction(new FlagValue(this, "297", "Indicators", "Indicators 297", "AUTOPILOT G"));
            AddFunction(new FlagValue(this, "298", "Indicators", "Indicators 298", "AUTOPILOT blank G"));
            AddFunction(new FlagValue(this, "373", "Indicators", "Indicators 373", "Afterburner light"));
            AddFunction(new FlagValue(this, "376", "Indicators", "Indicators 376", "Starter light"));
            AddFunction(new FlagValue(this, "198", "Indicators", "Indicators 198", "Tranfere"));
            AddFunction(new FlagValue(this, "199", "Indicators", "Indicators 199", "Master-warning"));
            AddFunction(new FlagValue(this, "200", "Indicators", "Indicators 200", "Master-caution"));
            AddFunction(new FlagValue(this, "362", "Indicators", "Indicators 362", "Fuel Left RL"));
            AddFunction(new FlagValue(this, "363", "Indicators", "Indicators 363", "Fuel Center RL"));
            AddFunction(new FlagValue(this, "364", "Indicators", "Indicators 364", "Fuel Right RL"));
            AddFunction(new FlagValue(this, "365", "Indicators", "Indicators 365", "Fuel Left AV"));
            AddFunction(new FlagValue(this, "366", "Indicators", "Indicators 366", "Fuel Right AV"));
            AddFunction(new FlagValue(this, "367", "Indicators", "Indicators 367", "Fuel Left V"));
            AddFunction(new FlagValue(this, "368", "Indicators", "Indicators 368", "Fuel Right V"));
            AddFunction(new FlagValue(this, "405", "Indicators", "Indicators 405", "Gearhandle Innenleuchte, red"));
            AddFunction(new FlagValue(this, "410", "Indicators", "Indicators 410", "A Warnlamp"));
            AddFunction(new FlagValue(this, "411", "Indicators", "Indicators 411", "F Warnlamp"));
            AddFunction(new FlagValue(this, "412", "Indicators", "Indicators 412", "DIRA Warnlamp, blau"));
            AddFunction(new FlagValue(this, "413", "Indicators", "Indicators 413", "FREIN"));
            AddFunction(new FlagValue(this, "414", "Indicators", "Indicators 414", "Cross"));
            AddFunction(new FlagValue(this, "415", "Indicators", "Indicators 415", "SPAD"));
            AddFunction(new FlagValue(this, "416", "Indicators", "Indicators 416", "Red Warnlamp under BIP"));
            AddFunction(new FlagValue(this, "417", "Indicators", "Indicators 417", "Left Gear"));
            AddFunction(new FlagValue(this, "418", "Indicators", "Indicators 418", "Nose Gear"));
            AddFunction(new FlagValue(this, "419", "Indicators", "Indicators 419", "Right Gear"));
            AddFunction(new FlagValue(this, "677", "Indicators", "Indicators 677", "COM left green lamp"));
            AddFunction(new FlagValue(this, "519", "Indicators", "Indicators 519", "Oxy flow lamp"));
            AddFunction(new FlagValue(this, "490", "Indicators", "Indicators 490", "Left consule VAL"));
            AddFunction(new FlagValue(this, "492", "Indicators", "Indicators 492", "Left consule A"));
            AddFunction(new FlagValue(this, "494", "Indicators", "Indicators 494", "Left consule DEC"));
            AddFunction(new FlagValue(this, "496", "Indicators", "Indicators 496", "Left consule VISU"));
            AddFunction(new FlagValue(this, "505", "Indicators", "Indicators 505", "Left consule PSIC"));
            AddFunction(new FlagValue(this, "510", "Indicators", "Indicators 510", "Left consule ELEC"));
            AddFunction(new FlagValue(this, "511", "Indicators", "Indicators 511", "Left consule HYD"));
            AddFunction(new FlagValue(this, "512", "Indicators", "Indicators 512", "Left consule Red"));
            AddFunction(new FlagValue(this, "513", "Indicators", "Indicators 513", "Left consule Green"));
            AddFunction(new FlagValue(this, "525", "Indicators", "Indicators 525", "WP BATT"));
            AddFunction(new FlagValue(this, "526", "Indicators", "Indicators 526", "TR"));
            AddFunction(new FlagValue(this, "527", "Indicators", "Indicators 527", "ALT 1"));
            AddFunction(new FlagValue(this, "528", "Indicators", "Indicators 528", "ALT 2"));
            AddFunction(new FlagValue(this, "529", "Indicators", "Indicators 529", "HUILLE"));
            AddFunction(new FlagValue(this, "530", "Indicators", "Indicators 530", "T7"));
            AddFunction(new FlagValue(this, "531", "Indicators", "Indicators 531", "CALC C"));
            AddFunction(new FlagValue(this, "532", "Indicators", "Indicators 532", "SOURIS"));
            AddFunction(new FlagValue(this, "533", "Indicators", "Indicators 533", "PELLE"));
            AddFunction(new FlagValue(this, "534", "Indicators", "Indicators 534", "B.P"));
            AddFunction(new FlagValue(this, "535", "Indicators", "Indicators 535", "BP.G"));
            AddFunction(new FlagValue(this, "536", "Indicators", "Indicators 536", "BP.D"));
            AddFunction(new FlagValue(this, "537", "Indicators", "Indicators 537", "TRANSF"));
            AddFunction(new FlagValue(this, "538", "Indicators", "Indicators 538", "NIVEAU"));
            AddFunction(new FlagValue(this, "539", "Indicators", "Indicators 539", "HYD 1"));
            AddFunction(new FlagValue(this, "540", "Indicators", "Indicators 540", "HYD 2"));
            AddFunction(new FlagValue(this, "541", "Indicators", "Indicators 541", "HYD 3"));
            AddFunction(new FlagValue(this, "542", "Indicators", "Indicators 542", "EP"));
            AddFunction(new FlagValue(this, "543", "Indicators", "Indicators 543", "BINGO"));
            AddFunction(new FlagValue(this, "544", "Indicators", "Indicators 544", "P.CAB"));
            AddFunction(new FlagValue(this, "545", "Indicators", "Indicators 545", "TEMP"));
            AddFunction(new FlagValue(this, "546", "Indicators", "Indicators 546", "REG O2"));
            AddFunction(new FlagValue(this, "547", "Indicators", "Indicators 547", "5nm O2"));
            AddFunction(new FlagValue(this, "548", "Indicators", "Indicators 548", "O2 HA"));
            AddFunction(new FlagValue(this, "549", "Indicators", "Indicators 549", "ANEMO"));
            AddFunction(new FlagValue(this, "550", "Indicators", "Indicators 550", "CC"));
            AddFunction(new FlagValue(this, "551", "Indicators", "Indicators 551", "DSV"));
            AddFunction(new FlagValue(this, "552", "Indicators", "Indicators 552", "CONDIT"));
            AddFunction(new FlagValue(this, "553", "Indicators", "Indicators 553", "CONF"));
            AddFunction(new FlagValue(this, "554", "Indicators", "Indicators 554", "PA"));
            AddFunction(new FlagValue(this, "555", "Indicators", "Indicators 555", "MAN"));
            AddFunction(new FlagValue(this, "556", "Indicators", "Indicators 556", "DOM"));
            AddFunction(new FlagValue(this, "557", "Indicators", "Indicators 557", "BECS"));
            AddFunction(new FlagValue(this, "558", "Indicators", "Indicators 558", "U.S.EL"));
            AddFunction(new FlagValue(this, "559", "Indicators", "Indicators 559", "ZEICHEN"));
            AddFunction(new FlagValue(this, "560", "Indicators", "Indicators 560", "GAIN"));
            AddFunction(new FlagValue(this, "561", "Indicators", "Indicators 561", "RPM"));
            AddFunction(new FlagValue(this, "562", "Indicators", "Indicators 562", "DECOL"));
            AddFunction(new FlagValue(this, "563", "Indicators", "Indicators 563", "PARK."));
            AddFunction(new FlagValue(this, "564", "Indicators", "Indicators 564", "Right Console PRET"));
            AddFunction(new FlagValue(this, "565", "Indicators", "Indicators 565", "ALN"));
            AddFunction(new FlagValue(this, "566", "Indicators", "Indicators 566", "MIP"));
            AddFunction(new FlagValue(this, "567", "Indicators", "Indicators 567", "N.DEG"));
            AddFunction(new FlagValue(this, "568", "Indicators", "Indicators 568", "SEC"));
            AddFunction(new FlagValue(this, "569", "Indicators", "Indicators 569", "UNI"));
            AddFunction(new FlagValue(this, "669", "Indicators", "Indicators 669", "M01"));
            AddFunction(new FlagValue(this, "670", "Indicators", "Indicators 670", "M02"));
            AddFunction(new FlagValue(this, "671", "Indicators", "Indicators 671", "M03"));
            AddFunction(new FlagValue(this, "571", "Indicators", "Indicators 571", "PREP"));
            AddFunction(new FlagValue(this, "573", "Indicators", "Indicators 573", "DEST"));
            AddFunction(new FlagValue(this, "577", "Indicators", "Indicators 577", "BAD"));
            AddFunction(new FlagValue(this, "579", "Indicators", "Indicators 579", "REC"));
            AddFunction(new FlagValue(this, "581", "Indicators", "Indicators 581", "VAL"));
            AddFunction(new FlagValue(this, "583", "Indicators", "Indicators 583", "MRC"));
            AddFunction(new FlagValue(this, "595", "Indicators", "Indicators 595", "EFF"));
            AddFunction(new FlagValue(this, "597", "Indicators", "Indicators 597", "INS"));
            AddFunction(new FlagValue(this, "668", "Indicators", "Indicators 668", "ENC"));
            AddFunction(new FlagValue(this, "632", "Indicators", "Indicators 632", "TACAN C"));
            AddFunction(new FlagValue(this, "634", "Indicators", "Indicators 634", "TACAN F"));
            AddFunction(new FlagValue(this, "675", "Indicators", "Indicators 675", "COM Panel, lamp red"));
            AddFunction(new FlagValue(this, "676", "Indicators", "Indicators 676", "COM Panel, lamp red, over COM"));
            #endregion
            #region Displays
            //
            // The following are not FlagValues.  Arguments in the range 2000+ are usually manufactured in the exports.lua from other values.
            // These particular values were chosen for Ikarus' needs, but there is no published Exports.lua from S-D-A to explain how these are made
            //
            //AddFunction(new FlagValue(this, "2001", "Indicators", "Indicators 2001", "ECM_CHF"));
            //AddFunction(new FlagValue(this, "2002", "Indicators", "Indicators 2002", "ECM_FLR"));
            //AddFunction(new FlagValue(this, "2003", "Indicators", "Indicators 2003", "FUEL"));
            //AddFunction(new FlagValue(this, "2010", "Indicators", "Indicators 2010", "PCA_UR1"));
            //AddFunction(new FlagValue(this, "2011", "Indicators", "Indicators 2011", "PCA_UR2"));
            //AddFunction(new FlagValue(this, "2012", "Indicators", "Indicators 2012", "PCA_UR3"));
            //AddFunction(new FlagValue(this, "2013", "Indicators", "Indicators 2013", "PCA_UR4"));
            //AddFunction(new FlagValue(this, "2014", "Indicators", "Indicators 2014", "PCA_UR5"));
            //AddFunction(new FlagValue(this, "2015", "Indicators", "Indicators 2015", "PCA_BR1"));
            //AddFunction(new FlagValue(this, "2016", "Indicators", "Indicators 2016", "PCA_BR2"));
            //AddFunction(new FlagValue(this, "2017", "Indicators", "Indicators 2017", "PCA_BR3"));
            //AddFunction(new FlagValue(this, "2018", "Indicators", "Indicators 2018", "PCA_BR4"));
            //AddFunction(new FlagValue(this, "2019", "Indicators", "Indicators 2019", "PCA_BR5"));
            //AddFunction(new FlagValue(this, "2020", "Indicators", "Indicators 2020", "COM1"));
            //AddFunction(new FlagValue(this, "2021", "Indicators", "Indicators 2021", "COM2"));
            //AddFunction(new FlagValue(this, "2022", "Indicators", "Indicators 2022", "PPA1"));
            //AddFunction(new FlagValue(this, "2023", "Indicators", "Indicators 2023", "PPA2"));
            //AddFunction(new FlagValue(this, "2024", "Indicators", "Indicators 2024", "PCN_sub_L_T"));
            //AddFunction(new FlagValue(this, "2025", "Indicators", "Indicators 2025", "PCN_sub_R_T"));
            //AddFunction(new FlagValue(this, "2026", "Indicators", "Indicators 2026", "PCN_sub_L_B"));
            //AddFunction(new FlagValue(this, "2027", "Indicators", "Indicators 2027", "PCN_sub_R_B"));
            //AddFunction(new FlagValue(this, "2028", "Indicators", "Indicators 2028", "PCN_main_L"));
            //AddFunction(new FlagValue(this, "2029", "Indicators", "Indicators 2029", "PCN_main_R"));
            //AddFunction(new FlagValue(this, "2030", "Indicators", "Indicators 2030", "PCN_BR1"));
            //AddFunction(new FlagValue(this, "2031", "Indicators", "Indicators 2031", "PCN_BR2"));
            //AddFunction(new FlagValue(this, "2032", "Indicators", "Indicators 2032", "Mode 1. Drum"));
            //AddFunction(new FlagValue(this, "2033", "Indicators", "Indicators 2033", "Mode 2. Drum"));
            //AddFunction(new FlagValue(this, "2034", "Indicators", "Indicators 2034", "Mode 3. Drum"));
            //AddFunction(new FlagValue(this, "2035", "Indicators", "Indicators 2035", "Mode 4. Drum"));
            //AddFunction(new FlagValue(this, "2036", "Indicators", "Indicators 2036", "Mode VOR ILS"));
            //AddFunction(new FlagValue(this, "2037", "Indicators", "Indicators 2037", "Mode TACAN"));
            #endregion
            #region  Landing Gear
            //AddFunction(Switch.CreateToggleSwitch(this, INSTPANEL, "3404", "404", "Landing Gear", "Landing Gear Lever", "%0.1f"));    // elements["PTN_404"] = default_animated_lever(_("Landing Gear Lever"), devices.INSTPANEL, device_commands.Button_404, 404,5.0)
            //AddFunction(Switch.CreateToggleSwitch(this, INSTPANEL, "3408", "408", "Landing Gear", "Landing Gear Emergency Release Lever", "%0.1f"));    // elements["PTN_408"] = default_animated_lever(_("Landing Gear Emergency Release Lever"), devices.INSTPANEL, device_commands.Button_408, 408, 5.0)
            // 
            #endregion
            #region  HUD/VTB"
            AddFunction(new Switch(this, VTH_VTB, "201", new SwitchPosition[] { }, "HUD/VTB", "HUD Power Switch", "%0.1f"));    // elements["PTN_201"] = multiposition_switch_limited(_("HUD Power Switch"), devices.VTH_VTB, device_commands.Button_201, 201, 3, 0.5, false, 0)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3203", "203", "HUD/VTB", "HUD Declutter Switch", "%0.1f"));    // elements["PTN_203"] = default_2_way_spring_switch(_("HUD Declutter Switch"), devices.VTH_VTB, device_commands.Button_203, 203, true)
            AddFunction(new Switch(this, VTH_VTB, "204", new SwitchPosition[] { }, "HUD/VTB", "HUD Altimeter Selector Switch", "%0.1f"));    // elements["PTN_204"] = multiposition_switch_limited(_("HUD Altimeter Selector Switch"), devices.VTH_VTB, device_commands.Button_204, 204, 3, 0.5, true, 0)
            AddFunction(new Switch(this, VTH_VTB, "205", new SwitchPosition[] { }, "HUD/VTB", "Radar Altimeter Power Switch", "%0.1f"));    // elements["PTN_205"] = multiposition_switch_limited(_("Radar Altimeter Power Switch"), devices.VTH_VTB, device_commands.Button_205, 205, 3, 0.5, false, 0)
            AddFunction(new PushButton(this, VTH_VTB, "3206", "206", "HUD/VTB", "Auxiliary Gunsight"));    // elements["PTN_206"] = default_2_position_tumb(_("Auxiliary Gunsight"), devices.VTH_VTB, device_commands.Button_206, 206)
            AddFunction(new Axis(this, VTH_VTB, "3207", "207", 0.15d, 0d, 1d, "HUD/VTB", "Auxiliary Gunsight Deflection"));    // elements["PTN_207"] = default_axis_cycle(_("Auxiliary Gunsight Deflection"), devices.VTH_VTB, device_commands.Button_207, 207, 0, 0.01, true, 0)
            AddFunction(new PushButton(this, VTH_VTB, "3208", "208", "HUD/VTB", "A/G Gun Reticle Switch"));    // elements["PTN_208"] = default_2_position_tumb(_("A/G Gun Reticle Switch"), devices.VTH_VTB, device_commands.Button_208, 208)
            AddFunction(new Axis(this, VTH_VTB, "3209", "209", 0.15d, 0d, 1d, "HUD/VTB", "Target Wingspan Knob"));    // elements["PTN_209"] = default_axis_limited(_("Target Wingspan Knob"), devices.VTH_VTB, device_commands.Button_209, 209, 0, -0.03, true, 0, 99)
            AddFunction(new PushButton(this, VTH_VTB, "3210", "210", "HUD/VTB", "HUD Clear Button"));    // elements["PTN_210"] = default_button(_("HUD Clear Button"), devices.VTH_VTB, device_commands.Button_210, 210, 0, 1)
            AddFunction(new Axis(this, VTH_VTB, "3192", "192", 0.15d, 0d, 1d, "HUD/VTB", "Minimum Altitude Selector"));    // elements["PTN_192"] = default_axis(_("Minimum Altitude Selector"), devices.VTH_VTB, device_commands.Button_192, 192, 0, 0.2, true, 0)
            AddFunction(new PushButton(this, VTH_VTB, "3470", "470", "HUD/VTB", "Radar WOW Emitter Authorize Switch"));    // elements["PTN_470"] = default_2_position_tumb(_("Radar WOW Emitter Authorize Switch"), devices.VTH_VTB, device_commands.Button_470, 470)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3213", "213", "HUD/VTB", "Target Data Manual Entry Begin/End", "%0.1f"));    // elements["PTN_213"] = default_2_way_spring_switch(_("Target Data Manual Entry Begin/End"), devices.VTH_VTB, device_commands.Button_213, 213, true)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3214", "214", "HUD/VTB", "Bullseye Waypoint Selector", "%0.1f"));    // elements["PTN_214"] = default_2_way_spring_switch(_("Bullseye Waypoint Selector"), devices.VTH_VTB, device_commands.Button_214, 214, true)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3215", "215", "HUD/VTB", "Target Range from Bullseye", "%0.1f"));    // elements["PTN_215"] = default_2_way_spring_switch(_("Target Range from Bullseye"), devices.VTH_VTB, device_commands.Button_215, 215, true)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3216", "216", "HUD/VTB", "Target Bearing from Bullseye", "%0.1f"));    // elements["PTN_216"] = default_2_way_spring_switch(_("Target Bearing from Bullseye"), devices.VTH_VTB, device_commands.Button_216, 216, true)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3217", "217", "HUD/VTB", "Target Heading", "%0.1f"));    // elements["PTN_217"] = default_2_way_spring_switch(_("Target Heading"), devices.VTH_VTB, device_commands.Button_217, 217, true)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3218", "218", "HUD/VTB", "Target Altitude", "%0.1f"));    // elements["PTN_218"] = default_2_way_spring_switch(_("Target Altitude"), devices.VTH_VTB, device_commands.Button_218, 218, true)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3219", "219", "HUD/VTB", "Target Mach Number", "%0.1f"));    // elements["PTN_219"] = default_2_way_spring_switch(_("Target Mach Number"), devices.VTH_VTB, device_commands.Button_219, 219, true)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3220", "220", "HUD/VTB", "Target Age", "%0.1f"));    // elements["PTN_220"] = default_2_way_spring_switch(_("Target Age"), devices.VTH_VTB, device_commands.Button_220, 220, true)
            AddFunction(new PushButton(this, VTH_VTB, "3221", "221", "HUD/VTB", "VTB Power Switch"));    // elements["PTN_221"] = default_2_position_tumb(_("VTB Power Switch"), devices.VTH_VTB, device_commands.Button_221, 221)
            //AddFunction(Switch.CreateToggleSwitch(this, VTH_VTB, "3222", "222", "HUD/VTB", "VTB Declutter", "%0.1f"));    // elements["PTN_222"] = default_2_way_spring_switch(_("VTB Declutter"), devices.VTH_VTB, device_commands.Button_222, 222, true)
            AddFunction(new PushButton(this, VTH_VTB, "3223", "223", "HUD/VTB", "VTB Orientation Selector (Inop)"));    // elements["PTN_223"] = default_2_position_tumb(_("VTB Orientation Selector (Inop)"), devices.VTH_VTB, device_commands.Button_223, 223)
            AddFunction(new Switch(this, VTH_VTB, "224", new SwitchPosition[] { }, "HUD/VTB", "Icons and Rulers Brightness", "%0.1f"));    // elements["PTN_224"] = multiposition_switch_limited(_("Icons and Rulers Brightness"), devices.VTH_VTB, device_commands.Button_224, 224, 8, 0.1, false, 0)
            AddFunction(new Switch(this, VTH_VTB, "225", new SwitchPosition[] { }, "HUD/VTB", "Video Brightness", "%0.1f"));    // elements["PTN_225"] = multiposition_switch_limited(_("Video Brightness"), devices.VTH_VTB, device_commands.Button_225, 225, 8, 0.1, false, 0)
            AddFunction(new Switch(this, VTH_VTB, "226", new SwitchPosition[] { }, "HUD/VTB", "Display Contrast", "%0.1f"));    // elements["PTN_226"] = multiposition_switch_limited(_("Display Contrast"), devices.VTH_VTB, device_commands.Button_226, 226, 8, 0.1, false, 0)
            AddFunction(new Switch(this, VTH_VTB, "227", new SwitchPosition[] { }, "HUD/VTB", "Display Brightness", "%0.1f"));    // elements["PTN_227"] = multiposition_switch_limited(_("Display Brightness"), devices.VTH_VTB, device_commands.Button_227, 227, 8, 0.1, false, 0)

            #endregion
            #region  PCA/PPA
            AddFunction(new PushButton(this, PCA_PPA, "3463", "463", "PCA/PPA", "Gun Arm/Safe Switch"));    // elements["PTN_463"] = default_2_position_tumb(_("Gun Arm/Safe Switch"), devices.PCA_PPA, device_commands.Button_463, 463)
            AddFunction(new PushButton(this, PCA_PPA, "3234", "234", "PCA/PPA", "Master Arm Switch"));    // elements["PTN_234"] = default_2_position_tumb(_("Master Arm Switch"), devices.PCA_PPA, device_commands.Button_234, 234)
            AddFunction(new PushButton(this, PCA_PPA, "3248", "248", "PCA/PPA", "Selective Jettison Switch Cover"));    // elements["PTN_248"] = default_2_position_tumb(_("Selective Jettison Switch Cover"), devices.PCA_PPA, device_commands.Button_248, 248)
            AddFunction(new PushButton(this, PCA_PPA, "3249", "249", "PCA/PPA", "Selective Jettison Switch"));    // elements["PTN_249"] = default_2_position_tumb(_("Selective Jettison Switch"), devices.PCA_PPA, device_commands.Button_249, 249)
                                                                                                                  // 
            AddFunction(new PushButton(this, PCA_PPA, "3235", "235", "PCA/PPA", "PCA Button 1"));    // elements["PTN_235"] = default_button(_("PCA Button 1"), devices.PCA_PPA, device_commands.Button_235, 235, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3237", "237", "PCA/PPA", "PCA Button 2"));    // elements["PTN_237"] = default_button(_("PCA Button 2"), devices.PCA_PPA, device_commands.Button_237, 237, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3239", "239", "PCA/PPA", "PCA Button 3"));    // elements["PTN_239"] = default_button(_("PCA Button 3"), devices.PCA_PPA, device_commands.Button_239, 239, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3241", "241", "PCA/PPA", "PCA Button 4"));    // elements["PTN_241"] = default_button(_("PCA Button 4"), devices.PCA_PPA, device_commands.Button_241, 241, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3243", "243", "PCA/PPA", "PCA Button 5"));    // elements["PTN_243"] = default_button(_("PCA Button 5"), devices.PCA_PPA, device_commands.Button_243, 243, 0, 1)
                                                                                                     // 
            AddFunction(new PushButton(this, PCA_PPA, "3245", "245", "PCA/PPA", "Gun Mode Button"));    // elements["PTN_245"] = default_button(_("Gun Mode Button"), devices.PCA_PPA, device_commands.Button_245, 245, 0, 1)
                                                                                                        // 
            AddFunction(new PushButton(this, PCA_PPA, "3250", "250", "PCA/PPA", "Weapons Selector Button 1"));    // elements["PTN_250"] = default_button(_("Weapons Selector Button 1"), devices.PCA_PPA, device_commands.Button_250, 250, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3253", "253", "PCA/PPA", "Weapons Selector Button 2"));    // elements["PTN_253"] = default_button(_("Weapons Selector Button 2"), devices.PCA_PPA, device_commands.Button_253, 253, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3256", "256", "PCA/PPA", "Weapons Selector Button 3"));    // elements["PTN_256"] = default_button(_("Weapons Selector Button 3"), devices.PCA_PPA, device_commands.Button_256, 256, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3259", "259", "PCA/PPA", "Weapons Selector Button 4"));    // elements["PTN_259"] = default_button(_("Weapons Selector Button 4"), devices.PCA_PPA, device_commands.Button_259, 259, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3262", "262", "PCA/PPA", "Weapons Selector Button 5"));    // elements["PTN_262"] = default_button(_("Weapons Selector Button 5"), devices.PCA_PPA, device_commands.Button_262, 262, 0, 1)
                                                                                                                  // 
            AddFunction(new PushButton(this, PCA_PPA, "3266", "266", "PCA/PPA", "S530 Standby"));    // elements["PTN_266"] = default_button(_("S530 Standby"), devices.PCA_PPA, device_commands.Button_266, 266, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3269", "269", "PCA/PPA", "AUTO/MAN Button"));    // elements["PTN_269"] = default_button(_("AUTO/MAN Button"), devices.PCA_PPA, device_commands.Button_269, 269, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3272", "272", "PCA/PPA", "MAGIC Standby"));    // elements["PTN_272"] = default_button(_("MAGIC Standby"), devices.PCA_PPA, device_commands.Button_272, 272, 0, 1)
            AddFunction(new PushButton(this, PCA_PPA, "3279", "279", "PCA/PPA", "530D/Rockets/Guns Firing Mode Selector"));    // elements["PTN_279"] = default_button(_("530D/Rockets/Guns Firing Mode Selector"), devices.PCA_PPA, device_commands.Button_279, 279, 0, 1)

            AddFunction(new Switch(this, PCA_PPA, "265", new SwitchPosition[] { }, "PCA/PPA", "Missile Selector Switch", "%0.1f"));    // elements["PTN_265"] = default_3_position_tumb(_("Missile Selector Switch"), devices.PCA_PPA, device_commands.Button_265, 265, false, false)
            //AddFunction(Switch.CreateToggleSwitch(this, PCA_PPA, "3275", "275", "PCA/PPA", "PPA Test Switch", "%0.1f"));    // elements["PTN_275"] = default_2_way_spring_switch(_("PPA Test Switch"), devices.PCA_PPA, device_commands.Button_275, 275, false)
            AddFunction(new Switch(this, PCA_PPA, "276", new SwitchPosition[] { }, "PCA/PPA", "Bomb Fuze Selector Switch", "%0.1f"));    // elements["PTN_276"] = multiposition_switch_limited(_("Bomb Fuze Selector Switch"), devices.PCA_PPA, device_commands.Button_276, 276, 3, 0.5, true, 0)
                                                                                                                                         //AddFunction(Switch.CreateToggleSwitch(this, PCA_PPA, "3277", "277", "PCA/PPA", "Bomb Release Quantity Selector Switch", "%0.1f"));    // elements["PTN_277"] = default_2_way_spring_switch(_("Bomb Release Quantity Selector Switch"), devices.PCA_PPA, device_commands.Button_277, 277, true)
                                                                                                                                         //AddFunction(Switch.CreateToggleSwitch(this, PCA_PPA, "3278", "278", "PCA/PPA", "Bomb Release Interval Selector Switch", "%0.1f"));    // elements["PTN_278"] = default_2_way_spring_switch(_("Bomb Release Interval Selector Switch"), devices.PCA_PPA, device_commands.Button_278, 278, true)
                                                                                                                                         // 
            AddFunction(new PushButton(this, PCA_PPA, "3409", "409", "PCA/PPA", "Emergency Jettison"));    // elements["PTN_409"] = default_button(_("Emergency Jettison"), devices.PCA_PPA, device_commands.Button_409, 409, 0, 1)

            #endregion
            #region  AFCS
            AddFunction(new PushButton(this, AFCS, "3282", "282", "AFCS", "Autopilot Master Button"));    // elements["PTN_282"] = default_button(_("Autopilot Master Button"), devices.AFCS, device_commands.Button_282, 282, 0, 1)
            AddFunction(new PushButton(this, AFCS, "3285", "285", "AFCS", "Altitude Hold Button"));    // elements["PTN_285"] = default_button(_("Altitude Hold Button"), devices.AFCS, device_commands.Button_285, 285, 0, 1)
            AddFunction(new PushButton(this, AFCS, "3288", "288", "AFCS", "Selected Altitude Hold Button"));    // elements["PTN_288"] = default_button(_("Selected Altitude Hold Button"), devices.AFCS, device_commands.Button_288, 288, 0, 1)
            AddFunction(new PushButton(this, AFCS, "3294", "294", "AFCS", "Approach Hold Button"));    // elements["PTN_294"] = default_button(_("Approach Hold Button"), devices.AFCS, device_commands.Button_294, 294, 0, 1)
            AddFunction(new PushButton(this, AFCS, "3302", "302", "AFCS", "Autopilot Lights Test Button"));    // elements["PTN_302"] = default_button(_("Autopilot Lights Test Button"), devices.AFCS, device_commands.Button_302, 302, 0, 1)
            AddFunction(new Switch(this, AFCS, "299", new SwitchPosition[] { }, "AFCS", "Altitude 10 000 ft Selector", "%0.1f"));    // elements["PTN_299"] = default_multiposition_knob(_("Altitude 10,000 ft Selector"), devices.AFCS, device_commands.Button_299, 299,  6, 0.1, false, 0)
            AddFunction(new Switch(this, AFCS, "300", new SwitchPosition[] { }, "AFCS", "Altitude 1 000 ft Selector", "%0.1f"));    // elements["PTN_300"] = default_multiposition_knob(_("Altitude 1,000 ft Selector"),  devices.AFCS, device_commands.Button_300, 300, 10, 0.1, false, 0)
            AddFunction(new Switch(this, AFCS, "301", new SwitchPosition[] { }, "AFCS", "Altitude 100 ft Selector", "%0.1f"));    // elements["PTN_301"] = default_multiposition_knob(_("Altitude 100 ft Selector"),  devices.AFCS, device_commands.Button_301, 301, 10, 0.1, false4, 0)
            AddFunction(new PushButton(this, AFCS, "3514", "514", "AFCS", "Autopilot Test Switch Cover"));    // elements["PTN_514"] = default_2_position_tumb(_("Autopilot Test Switch Cover"),  devices.AFCS, device_commands.Button_514, 514)
            AddFunction(new PushButton(this, AFCS, "3515", "515", "AFCS", "Autopilot Test Switch"));    // elements["PTN_515"] = default_2_position_tumb(_("Autopilot Test Switch"),  devices.AFCS, device_commands.Button_515, 515)
            AddFunction(new PushButton(this, AFCS, "3516", "516", "AFCS", "FBW Test Switch Cover"));    // elements["PTN_516"] = default_2_position_tumb(_("FBW Test Switch Cover"),  devices.AFCS, device_commands.Button_516, 516)
            AddFunction(new Switch(this, AFCS, "517", new SwitchPosition[] { }, "AFCS", "FBW Test Switch", "%0.1f"));    // elements["PTN_517"] = multiposition_switch_limited(_("FBW Test Switch"),  devices.AFCS, device_commands.Button_517, 517, 3, 0.5, false, 0)
            AddFunction(new PushButton(this, AFCS, "3479", "479", "AFCS", "FBW Channel 5 Switch Cover"));    // elements["PTN_479"] = default_2_position_tumb(_("FBW Channel 5 Switch Cover"), devices.AFCS, device_commands.Button_479, 479)
            AddFunction(new PushButton(this, AFCS, "3480", "480", "AFCS", "FBW Channel 5 Switch"));    // elements["PTN_480"] = default_2_position_tumb(_("FBW Channel 5 Switch"), devices.AFCS, device_commands.Button_480, 480)
                                                                                                       //AddFunction(Switch.CreateToggleSwitch(this, AFCS, "3508", "508", "AFCS", "Trim Control Mode Dial", "%0.1f"));    // elements["PTN_508"] = default_animated_lever(_("Trim Control Mode Dial"), devices.AFCS, device_commands.Button_508, 508,5.0)
                                                                                                       //AddFunction(Switch.CreateToggleSwitch(this, AFCS, "3509", "509", "AFCS", "Rudder Trim Paddle", "%0.1f"));    // elements["PTN_509"] = default_2_way_spring_switch(_("Rudder Trim Paddle"), devices.AFCS, device_commands.Button_509, 509,true)

            #endregion
            #region  FBW
            AddFunction(new PushButton(this, ENGINE, "3330", "330", "FBW", "FBW Spin Mode Switch"));    // elements["PTN_330"] = default_2_position_tumb(_("FBW Spin Mode Switch"), devices.ENGINE, device_commands.Button_330, 330)
            AddFunction(new PushButton(this, ENGINE, "3420", "420", "FBW", "FBW Gain Mode Switch Cover"));    // elements["PTN_420"] = default_2_position_tumb(_("FBW Gain Mode Switch Cover"),  devices.ENGINE, device_commands.Button_420, 420)
            AddFunction(new PushButton(this, ENGINE, "3421", "421", "FBW", "FBW Gain Mode Switch"));    // elements["PTN_421"] = default_2_position_tumb(_("FBW Gain Mode Switch"),  devices.ENGINE, device_commands.Button_421, 421)
            AddFunction(new PushButton(this, ENGINE, "3422", "422", "FBW", "FBW G-Limiter Switch"));    // elements["PTN_422"] = default_2_position_tumb(_("FBW G-Limiter Switch"),  devices.ENGINE, device_commands.Button_422, 422)
            AddFunction(new PushButton(this, AFCS, "3423", "423", "FBW", "FBW Reset Button"));    // elements["PTN_423"] = default_button(_("FBW Reset Button"),  devices.AFCS, device_commands.Button_423, 423)

            #endregion
            #region  HSI
            AddFunction(new Axis(this, NAVINST, "3340", "340", 0.15d, 0d, 1d, "HSI", "HSI VAD Selector"));    // elements["PTN_340"] = default_axis_cycle(_("HSI VAD Selector"),devices.NAVINST, device_commands.Button_340, 340)
            AddFunction(new Switch(this, NAVINST, "341", new SwitchPosition[] { }, "HSI", "HSI Mode Selector Switch", "%0.1f"));    // elements["PTN_341"] = multiposition_switch_limited(_("HSI Mode Selector Switch"), devices.NAVINST, device_commands.Button_341, 341, 7, 0.1, false, 0)
                                                                                                                                    // 
            #endregion
            #region  PELLES, SOURIES AND BECS
            AddFunction(new PushButton(this, ENGINE, "3460", "460", "PELLES, SOURIES AND BECS", "Intake Slats Operation Switch"));    // elements["PTN_460"] = default_2_position_tumb(_("Intake Slats Operation Switch"), devices.ENGINE, device_commands.Button_460, 460)
            AddFunction(new PushButton(this, ENGINE, "3461", "461", "PELLES, SOURIES AND BECS", "Intake Cones Operation Switch"));    // elements["PTN_461"] = default_2_position_tumb(_("Intake Cones Operation Switch"), devices.ENGINE, device_commands.Button_461, 461)
            AddFunction(new Switch(this, SUBSYSTEMS, "462", new SwitchPosition[] { }, "PELLES, SOURIES AND BECS", "Slats Operation Switch", "%0.1f"));    // elements["PTN_462"] = default_3_position_tumb(_("Slats Operation Switch"), devices.SUBSYSTEMS, device_commands.Button_462, 462, false, true)
            AddFunction(new Axis(this, SUBSYSTEMS, "3396", "396", 0.15d, 0d, 1d, "PELLES, SOURIES AND BECS", "Pedal Adjustment Lever"));    // elements["PTN_396"] = default_axis_limited(_("Pedal Adjustment Lever"),devices.SUBSYSTEMS,device_commands.Button_396,396, 0.5, -0.1, true, 0)
            AddFunction(new PushButton(this, SUBSYSTEMS, "3395", "395", "PELLES, SOURIES AND BECS", "Hydraulic System Selector"));    // elements["PTN_395"] = default_2_position_tumb(_("Hydraulic System Selector"), devices.SUBSYSTEMS, device_commands.Button_395, 395)
                                                                                                                                      // 
            #endregion
            #region  RADAR
            AddFunction(new Switch(this, RADAR, "481", new SwitchPosition[] { }, "RADAR", "Radar Illumination Switch", "%0.1f"));    // elements["PTN_481"] = multiposition_switch_limited(_("Radar Illumination Switch"), devices.RADAR, device_commands.Button_481, 481, 4, 0.5, false, -1)
            AddFunction(new PushButton(this, RADAR, "3482", "482", "RADAR", "Radar Test Button"));    // elements["PTN_482"] = default_button(_("Radar Test Button"), devices.RADAR, device_commands.Button_482, 482, 0, 1)
            AddFunction(new PushButton(this, RADAR, "3483", "483", "RADAR", "Radar Rearm Button"));    // elements["PTN_483"] = default_button(_("Radar Rearm Button"), devices.RADAR, device_commands.Button_483, 483, 0, 1)
            AddFunction(new PushButton(this, RADAR, "3484", "484", "RADAR", "Radar Doppler Reject Switch"));    // elements["PTN_484"] = default_2_position_tumb(_("Radar Doppler Reject Switch"), devices.RADAR, device_commands.Button_484, 484, 3, 0.5, false, 0)
            AddFunction(new Switch(this, RADAR, "485", new SwitchPosition[] { }, "RADAR", "Radar Contrast Switch", "%0.1f"));    // elements["PTN_485"] = multiposition_switch_limited(_("Radar Contrast Switch"), devices.RADAR, device_commands.Button_485, 485, 4, 0.5, false, -1)
            AddFunction(new Switch(this, RADAR, "486", new SwitchPosition[] { }, "RADAR", "Radar Power Selector", "%0.1f"));    // elements["PTN_486"] = multiposition_switch_limited(_("Radar Power Selector"), devices.RADAR, device_commands.Button_486, 486, 4, 0.33, false, 0)
            AddFunction(new Axis(this, RADAR, "3488", "488", 0.15d, 0d, 1d, "RADAR", "Radar Gain Dial"));    // elements["PTN_488"] = default_axis_limited(_("Radar Gain Dial"), devices.RADAR, device_commands.Button_488, 488, 10, 0.3, false, 0)
            AddFunction(new PushButton(this, RADAR, "3491", "491", "RADAR", "A/G Radar A Mode Switch"));    // elements["PTN_491"] = default_2_position_tumb(_("A/G Radar A Mode Switch"), devices.RADAR, device_commands.Button_491, 491, 0, 1)
            AddFunction(new PushButton(this, RADAR, "3493", "493", "RADAR", "A/G Radar DEC Mode Switch"));    // elements["PTN_493"] = default_2_position_tumb(_("A/G Radar DEC Mode Switch"), devices.RADAR, device_commands.Button_493, 493, 0, 1)
            AddFunction(new PushButton(this, RADAR, "Button_495", "495", "RADAR", "A/G Radar VISU Mode Switch"));    // elements["PTN_495"] = default_2_position_tumb(_("A/G Radar VISU Mode Switch"), devices.RADAR, device_commands.Button_495, 495, 0, 1)
            AddFunction(new PushButton(this, RADAR, "3499", "499", "RADAR", "Radar Grid Selector Switch"));    // elements["PTN_499"] = default_2_position_tumb(_("Radar Grid Selector Switch"), devices.RADAR, device_commands.Button_499, 499, 0, 1)
            AddFunction(new PushButton(this, RADAR, "3500", "500", "RADAR", "Target Memory Time Selector Switch"));    // elements["PTN_500"] = default_2_position_tumb(_("Target Memory Time Selector Switch"), devices.RADAR, device_commands.Button_500, 500, 0, 1)
            AddFunction(new Switch(this, RADAR, "502", new SwitchPosition[] { }, "RADAR", "Radar Scan Lines Selector", "%0.1f"));    // elements["PTN_502"] = multiposition_switch_limited(_("Radar Scan Lines Selector"), devices.RADAR, device_commands.Button_502, 502, 3, 0.5, true, 0)
            //AddFunction(Switch.CreateToggleSwitch(this, RADAR, "3503", "503", "RADAR", "Radar Range Selector Switch", "%0.1f"));    // elements["PTN_503"] = default_2_way_spring_switch(_("Radar Range Selector Switch"), devices.RADAR, device_commands.Button_503, 503,true)
            AddFunction(new PushButton(this, RADAR, "3504", "504", "RADAR", "A/A Radar STT Selector Button"));    // elements["PTN_504"] = default_button(_("A/A Radar STT Selector Button"), devices.RADAR, device_commands.Button_504, 504, 0, 1)
            AddFunction(new Switch(this, RADAR, "506", new SwitchPosition[] { }, "RADAR", "Radar Azimuth Selector", "%0.1f"));    // elements["PTN_506"] = multiposition_switch_limited(_("Radar Azimuth Selector"), devices.RADAR, device_commands.Button_506, 506, 3, 0.5, false, 0)
            AddFunction(new Switch(this, RADAR, "109", new SwitchPosition[] { }, "RADAR", "Radar PRF Switch", "%0.1f"));    // elements["PTN_109"] = default_3_position_tumb(_("Radar PRF Switch"), devices.RADAR, device_commands.Button_109, 109, false, true)
            AddFunction(new PushButton(this, RADAR, "3710", "710", "RADAR", "TDC Mode Switch"));    // elements["PTN_710"] = default_2_position_tumb(_("TDC Mode Switch"), devices.RADAR, device_commands.Button_710, 710, 0, 1)

            #endregion
            #region  RADAR IFF
            AddFunction(new Switch(this, RADAR, "598", new SwitchPosition[] { }, "RADAR IFF", "Radar IFF Mode Switch", "%0.1f"));    // elements["PTN_598"] = multiposition_switch_limited(_("Radar IFF Mode Switch"),   devices.RADAR, device_commands.Button_598, 598, 6, 0.2, false, 0)
            AddFunction(new PushButton(this, RADAR, "3599", "599", "RADAR IFF", "Radar IFF L/R Selector"));    // elements["PTN_599"] = default_2_position_tumb(_("Radar IFF L/R Selector"),       devices.RADAR, device_commands.Button_599, 599, 0, 1)
            AddFunction(new Switch(this, RADAR, "600", new SwitchPosition[] { }, "RADAR IFF", "Radar IFF Power Switch", "%0.1f"));    // elements["PTN_600"] = multiposition_switch_limited(_("Radar IFF Power Switch"),  devices.RADAR, device_commands.Button_600, 600, 3, 0.5, false, 0)
            AddFunction(new Switch(this, RADAR, "601", new SwitchPosition[] { }, "RADAR IFF", "Radar IFF Code-4 Selector", "%0.1f"));    // elements["PTN_601"] = default_multiposition_knob(_("Radar IFF Code-4 Selector"), devices.RADAR, device_commands.Button_601, 601, 10, 0.1, false, 0)
            AddFunction(new Switch(this, RADAR, "602", new SwitchPosition[] { }, "RADAR IFF", "Radar IFF Code-3 Selector", "%0.1f"));    // elements["PTN_602"] = default_multiposition_knob(_("Radar IFF Code-3 Selector"), devices.RADAR, device_commands.Button_602, 602, 10, 0.1, false, 0)
            AddFunction(new Switch(this, RADAR, "603", new SwitchPosition[] { }, "RADAR IFF", "Radar IFF Code-2 Selector", "%0.1f"));    // elements["PTN_603"] = default_multiposition_knob(_("Radar IFF Code-2 Selector"), devices.RADAR, device_commands.Button_603, 603, 10, 0.1, false, 0)
            AddFunction(new Switch(this, RADAR, "604", new SwitchPosition[] { }, "RADAR IFF", "Radar IFF Code-1 Selector", "%0.1f"));    // elements["PTN_604"] = default_multiposition_knob(_("Radar IFF Code-1 Selector"), devices.RADAR, device_commands.Button_604, 604, 10, 0.1, false, 0)
                                                                                                                                         // 
            #endregion
            #region  ELECTRICAL PANEL
            AddFunction(new PushButton(this, PWRPNL, "3520", "520", "ELECTRICAL PANEL", "Main Battery Switch"));    // elements["PTN_520"] = default_2_position_tumb(_("Main Battery Switch"),devices.PWRPNL, device_commands.Button_520, 520)
            AddFunction(new PushButton(this, PWRPNL, "3521", "521", "ELECTRICAL PANEL", "Electric Power Transfer Switch"));    // elements["PTN_521"] = default_2_position_tumb(_("Electric Power Transfer Switch"),devices.PWRPNL, device_commands.Button_521, 521)
            AddFunction(new PushButton(this, PWRPNL, "3522", "522", "ELECTRICAL PANEL", "Alternator 1 Switch"));    // elements["PTN_522"] = default_2_position_tumb(_("Alternator 1 Switch"),devices.PWRPNL, device_commands.Button_522, 522)
            AddFunction(new PushButton(this, PWRPNL, "3523", "523", "ELECTRICAL PANEL", "Alternator 2 Switch"));    // elements["PTN_523"] = default_2_position_tumb(_("Alternator 2 Switch"),devices.PWRPNL, device_commands.Button_523, 523)
            //AddFunction(Switch.CreateToggleSwitch(this, PWRPNL, "3524", "524", "ELECTRICAL PANEL", "Lights Test Switch", "%0.1f"));    // elements["PTN_524"] = default_2_way_spring_switch(_("Lights Test Switch"),devices.PWRPNL, device_commands.Button_524, 524,true)
            AddFunction(new PushButton(this, PWRPNL, "3654", "654", "ELECTRICAL PANEL", "Alert Network (QRA) Switch"));    // elements["PTN_654"] = default_2_position_tumb(_("Alert Network (QRA) Switch"),devices.PWRPNL, device_commands.Button_654, 654)

            #endregion
            #region  PCN
            AddFunction(new PushButton(this, PCN_NAV, "3570", "570", "PCN", "INS PREP Switch"));    // elements["PTN_570"] = default_button(_("INS PREP Switch"),     devices.PCN_NAV, device_commands.Button_570, 570, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3572", "572", "PCN", "INS DEST Switch"));    // elements["PTN_572"] = default_button(_("INS DEST Switch"),     devices.PCN_NAV, device_commands.Button_572, 572, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3576", "576", "PCN", "Offset Waypoint/Target"));    // elements["PTN_576"] = default_button(_("Offset Waypoint/Target"), devices.PCN_NAV, device_commands.Button_576, 576, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3578", "578", "PCN", "INS Update"));    // elements["PTN_578"] = default_button(_("INS Update"),             devices.PCN_NAV, device_commands.Button_578, 578, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3580", "580", "PCN", "Validate Data Entry"));    // elements["PTN_580"] = default_button(_("Validate Data Entry"),    devices.PCN_NAV, device_commands.Button_580, 580, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3582", "582", "PCN", "Mark Position"));    // elements["PTN_582"] = default_button(_("Mark Position"),          devices.PCN_NAV, device_commands.Button_582, 582, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3667", "667", "PCN", "AUTO Navigation"));    // elements["PTN_667"] = default_button(_("AUTO Navigation"),        devices.PCN_NAV, device_commands.Button_667, 667, 0, 1)
            AddFunction(new Switch(this, PCN_NAV, "574", new SwitchPosition[] { }, "PCN", "INS Parameter Selector", "%0.1f"));    // elements["PTN_574"] = default_multiposition_knob(_("INS Parameter Selector"), devices.PCN_NAV, device_commands.Button_574,   574, 11, 0.1, false, 0)
            AddFunction(new Axis(this, SYSLIGHTS, "3575", "575", 0.15d, 0d, 1d, "PCN", "Light Brightnes Control/Test"));    // elements["PTN_575"] = default_axis_limited(_("Light Brightnes Control/Test"), devices.SYSLIGHTS, device_commands.Button_575, 575, 10, 0.1, false, 0)
            AddFunction(new PushButton(this, PCN_NAV, "3584", "584", "PCN", "INS Button 1"));    // elements["PTN_584"] = default_button(_("INS Button 1"), devices.PCN_NAV, device_commands.Button_584, 584, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3585", "585", "PCN", "INS Button 2"));    // elements["PTN_585"] = default_button(_("INS Button 2"), devices.PCN_NAV, device_commands.Button_585, 585, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3586", "586", "PCN", "INS Button 3"));    // elements["PTN_586"] = default_button(_("INS Button 3"), devices.PCN_NAV, device_commands.Button_586, 586, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3587", "587", "PCN", "INS Button 4"));    // elements["PTN_587"] = default_button(_("INS Button 4"), devices.PCN_NAV, device_commands.Button_587, 587, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3588", "588", "PCN", "INS Button 5"));    // elements["PTN_588"] = default_button(_("INS Button 5"), devices.PCN_NAV, device_commands.Button_588, 588, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3589", "589", "PCN", "INS Button 6"));    // elements["PTN_589"] = default_button(_("INS Button 6"), devices.PCN_NAV, device_commands.Button_589, 589, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3590", "590", "PCN", "INS Button 7"));    // elements["PTN_590"] = default_button(_("INS Button 7"), devices.PCN_NAV, device_commands.Button_590, 590, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3591", "591", "PCN", "INS Button 8"));    // elements["PTN_591"] = default_button(_("INS Button 8"), devices.PCN_NAV, device_commands.Button_591, 591, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3592", "592", "PCN", "INS Button 9"));    // elements["PTN_592"] = default_button(_("INS Button 9"), devices.PCN_NAV, device_commands.Button_592, 592, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3593", "593", "PCN", "INS Button 0"));    // elements["PTN_593"] = default_button(_("INS Button 0"), devices.PCN_NAV, device_commands.Button_593, 593, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3594", "594", "PCN", "INS Clear Button"));    // elements["PTN_594"] = default_button(_("INS Clear Button"), devices.PCN_NAV, device_commands.Button_594, 594, 0, 1)
            AddFunction(new PushButton(this, PCN_NAV, "3596", "596", "PCN", "INS ENTER Button"));    // elements["PTN_596"] = default_button(_("INS ENTER Button"), devices.PCN_NAV, device_commands.Button_596, 596, 0, 1)
                                                                                                     // 
            #endregion
            #region  PSM
            AddFunction(new Switch(this, PCN_NAV, "627", new SwitchPosition[] { }, "PSM", "INS Mode Selector", "%0.1f"));    // elements["PTN_627"] = multiposition_switch_limited(_("INS Mode Selector"), devices.PCN_NAV, device_commands.Button_627, 627, 8, 0.1, false, 0)
            AddFunction(new Switch(this, PCN_NAV, "629", new SwitchPosition[] { }, "PSM", "INS Operational Mode", "%0.1f"));    // elements["PTN_629"] = multiposition_switch_limited(_("INS Operational Mode"), devices.PCN_NAV, device_commands.Button_629, 629, 5, 0.1, true, 0)
            AddFunction(new Switch(this, PCN_NAV, "665", new SwitchPosition[] { }, "PSM", "INS Auxiliary Heading/Horizon", "%0.1f"));    // elements["PTN_665"] = multiposition_switch_limited(_("INS Auxiliary Heading/Horizon"), devices.PCN_NAV, device_commands.Button_665, 665, 3, 0.5, false, 0)
                                                                                                                                         // 
            #endregion
            #region  EW PANEL
            AddFunction(new Axis(this, SYSLIGHTS, "3228", "228", 0.15d, 0d, 1d, "EW PANEL", "RWR Light Brightnes Control"));    // elements["PTN_228"] = default_axis_limited(_("RWR Light Brightnes Control"), devices.SYSLIGHTS, device_commands.Button_228, 228, 10, 0.1, false, 0)
            AddFunction(new Switch(this, RWR, "605", new SwitchPosition[] { }, "EW PANEL", "EW Mode Selector Switch", "%0.1f"));    // elements["PTN_605"] = default_3_position_tumb(_("EW Mode Selector Switch"), devices.RWR, device_commands.Button_605, 605, false, true)
            AddFunction(new Switch(this, RWR, "606", new SwitchPosition[] { }, "EW PANEL", "BR Power Switch", "%0.1f"));    // elements["PTN_606"] = multiposition_switch_limited(_("BR Power Switch"), devices.RWR, device_commands.Button_606, 606, 3, 0.5, false, 0)
            AddFunction(new Switch(this, RWR, "607", new SwitchPosition[] { }, "EW PANEL", "RWR Power Switch", "%0.1f"));    // elements["PTN_607"] = multiposition_switch_limited(_("RWR Power Switch"), devices.RWR, device_commands.Button_607, 607, 3, 0.5, false, 0)
            AddFunction(new Switch(this, DDM_IND, "608", new SwitchPosition[] { }, "EW PANEL", "D2M Power Switch", "%0.1f"));    // elements["PTN_608"] = multiposition_switch_limited(_("D2M Power Switch"), devices.DDM_IND, device_commands.Button_608, 608, 3, 0.5, false, 0)
            AddFunction(new Switch(this, RWR, "609", new SwitchPosition[] { }, "EW PANEL", "Decoy Release Mode Switch", "%0.1f"));    // elements["PTN_609"] = multiposition_switch_limited(_("Decoy Release Mode Switch"), devices.RWR, device_commands.Button_609, 609, 3, 0.5, false, 0)
            AddFunction(new Switch(this, RWR, "610", new SwitchPosition[] { }, "EW PANEL", "Decoy Release Program Knob", "%0.1f"));    // elements["PTN_610"] = multiposition_switch_limited(_("Decoy Release Program Knob"), devices.RWR, device_commands.Button_610, 610, 11, 0.1, false, 0)
            #endregion  
            #region  ECM BOX
            AddFunction(new Switch(this, RWR, "194", new SwitchPosition[] { }, "ECM BOX", "ECM Box Mode Switch", "%0.1f"));    // elements["PTN_194"] = multiposition_switch_limited(_("ECM Box Mode Switch"), devices.RWR, device_commands.Button_194, 194, 3, 0.5, false, 0)
            AddFunction(new PushButton(this, RWR, "3195", "195", "ECM BOX", "ECM Box Switch"));    // elements["PTN_195"] = default_2_position_tumb(_("ECM Box Switch"), devices.RWR, device_commands.Button_195, 195)
            AddFunction(new PushButton(this, RWR, "3196", "196", "ECM BOX", "ECM Box Lights Switch"));    // elements["PTN_196"] = default_2_position_tumb(_("ECM Box Lights Switch"), devices.RWR, device_commands.Button_196, 196)
            AddFunction(new Axis(this, RWR, "3197", "197", 0.15d, 0d, 1d, "ECM BOX", "ECM Box LCD Display Brightness"));    // elements["PTN_197"] = default_axis_limited(_("ECM Box LCD Display Brightness"), devices.RWR, device_commands.Button_197, 197, 5, 0.5, false, 0)
            #endregion  
            #region  Panel Lights
            AddFunction(new PushButton(this, SYSLIGHTS, "3449", "449", "Panel Lights", "Police Lights Switch"));    // elements["PTN_449"] = default_2_position_tumb(_("Police Lights Switch"), devices.SYSLIGHTS, device_commands.Button_449, 449)
            AddFunction(new Switch(this, SYSLIGHTS, "450", new SwitchPosition[] { }, "Panel Lights", "Landing Lights Switch", "%0.1f"));    // elements["PTN_450"] = multiposition_switch_limited(_("Landing Lights Switch"), devices.SYSLIGHTS, device_commands.Button_450, 450, 3, 0.5, false, 0)
            AddFunction(new Switch(this, SYSLIGHTS, "452", new SwitchPosition[] { }, "Panel Lights", "SERPAM Recorder Switch", "%0.1f"));    // elements["PTN_452"] = multiposition_switch_limited(_("SERPAM Recorder Switch"), devices.SYSLIGHTS, device_commands.Button_452, 452, 3, 0.5, false, 0)
            AddFunction(new Switch(this, SYSLIGHTS, "453", new SwitchPosition[] { }, "Panel Lights", "Anti-Collision Lights Switch", "%0.1f"));    // elements["PTN_453"] = multiposition_switch_limited(_("Anti-Collision Lights Switch"), devices.SYSLIGHTS, device_commands.Button_453, 453, 3, 0.5, false, 0)
            AddFunction(new Switch(this, SYSLIGHTS, "454", new SwitchPosition[] { }, "Panel Lights", "Navigation Lights Switch", "%0.1f"));    // elements["PTN_454"] = multiposition_switch_limited(_("Navigation Lights Switch"), devices.SYSLIGHTS, device_commands.Button_454, 454, 3, 0.5, false, 0)
            AddFunction(new Switch(this, SYSLIGHTS, "455", new SwitchPosition[] { }, "Panel Lights", "Formation Lights Switch", "%0.1f"));    // elements["PTN_455"] = multiposition_switch_limited(_("Formation Lights Switch"), devices.SYSLIGHTS, device_commands.Button_455, 455, 3, 0.5, false, 0)
            AddFunction(new Axis(this, SYSLIGHTS, "3639", "639", 0.15d, 0d, 1d, "Panel Lights", "Dashboard U.V. Lights Knob"));    // elements["PTN_639"] = default_axis_limited(_("Dashboard U.V. Lights Knob"), devices.SYSLIGHTS, device_commands.Button_639, 639, 10, 0.3, false, 0)
            AddFunction(new Axis(this, SYSLIGHTS, "3640", "640", 0.15d, 0d, 1d, "Panel Lights", "Dashboard Panel Lights Knob"));    // elements["PTN_640"] = default_axis_limited(_("Dashboard Panel Lights Knob"), devices.SYSLIGHTS, device_commands.Button_640, 640, 10, 0.3, false, 0)
            AddFunction(new Axis(this, SYSLIGHTS, "3641", "641", 0.15d, 0d, 1d, "Panel Lights", "Red Flood Lights Knob"));    // elements["PTN_641"] = default_axis_limited(_("Red Flood Lights Knob"), devices.SYSLIGHTS, device_commands.Button_641, 641, 10, 0.3, false, 0)
            AddFunction(new Axis(this, SYSLIGHTS, "3642", "642", 0.15d, 0d, 1d, "Panel Lights", "Console Panel Lights Knob"));    // elements["PTN_642"] = default_axis_limited(_("Console Panel Lights Knob"), devices.SYSLIGHTS, device_commands.Button_642, 642, 10, 0.3, false, 0)
            AddFunction(new Axis(this, SYSLIGHTS, "3643", "643", 0.15d, 0d, 1d, "Panel Lights", "Casution/Advisory Lights Rheostat"));    // elements["PTN_643"] = default_axis_limited(_("Casution/Advisory Lights Rheostat"), devices.SYSLIGHTS, device_commands.Button_643, 643, 10, 0.3, false, 0)
            AddFunction(new Axis(this, SYSLIGHTS, "3644", "644", 0.15d, 0d, 1d, "Panel Lights", "White Flood Lights Knob"));    // elements["PTN_644"] = default_axis_limited(_("White Flood Lights Knob"), devices.SYSLIGHTS, device_commands.Button_644, 644, 10, 0.3, false, 0)
            AddFunction(new Axis(this, SYSLIGHTS, "Button_920", "920", 0.15d, 0d, 1d, "Panel Lights", "Refuel Lights Brightness Knob"));    // elements["PTN_920"] = default_axis_limited(_("Refuel Lights Brightness Knob"),devices.SYSLIGHTS,device_commands.Button_920, 920, 10, 0.3, false, 0)
            #endregion  
            #region  Engine Start Panel
            AddFunction(new PushButton(this, ENGPANEL, "3468", "468", "Engine Start Panel", "Engine In-Flight Start Switch"));    // elements["PTN_468"] = default_2_position_tumb(_("Engine In-Flight Start Switch"), devices.ENGPANEL, device_commands.Button_468, 468, 0, 1)
            AddFunction(new PushButton(this, ENGPANEL, "3467", "467", "Engine Start Panel", "Engine Shutdown Button"));    // elements["PTN_467"] = default_button(_("Engine Shutdown Button"), devices.ENGPANEL, device_commands.Button_467, 467)
            AddFunction(new PushButton(this, ENGPANEL, "3645", "645", "Engine Start Panel", "Engine Start Switch Cover"));    // elements["PTN_645"] = default_2_position_tumb(_("Engine Start Switch Cover"), devices.ENGPANEL, device_commands.Button_645, 645)
            AddFunction(new PushButton(this, ENGPANEL, "3646", "646", "Engine Start Panel", "Starter Fuel Pump Switch"));    // elements["PTN_646"] = default_2_position_tumb(_("Starter Fuel Pump Switch"), devices.ENGPANEL, device_commands.Button_646, 646)
            AddFunction(new PushButton(this, INSTPANEL, "3647", "647", "Engine Start Panel", "Engine Fuel Left Boost Pump Switch"));    // elements["PTN_647"] = default_2_position_tumb(_("Engine Fuel Left Boost Pump Switch"), devices.INSTPANEL, device_commands.Button_647, 647)
            AddFunction(new PushButton(this, INSTPANEL, "3648", "648", "Engine Start Panel", "Engine Fuel Right Boost Pump Switch"));    // elements["PTN_648"] = default_2_position_tumb(_("Engine Fuel Right Boost Pump Switch"), devices.INSTPANEL, device_commands.Button_648, 648)
            AddFunction(new PushButton(this, ENGPANEL, "3649", "649", "Engine Start Panel", "Engine Start Switch"));    // elements["PTN_649"] = default_button(_("Engine Start Switch"), devices.ENGPANEL, device_commands.Button_649, 649, 0, 1)
            AddFunction(new Switch(this, ENGPANEL, "650", new SwitchPosition[] { }, "Engine Start Panel", "Ventilation Selector Switch", "%0.1f"));    // elements["PTN_650"] = multiposition_switch_limited(_("Ventilation Selector Switch"), devices.ENGPANEL, device_commands.Button_650, 650, 3, 0.50, false, 0)
            AddFunction(new PushButton(this, ENGPANEL, "3651", "651", "Engine Start Panel", "Fuel Cutoff Switch Cover"));    // elements["PTN_651"] = default_2_position_tumb(_("Fuel Cutoff Switch Cover"), devices.ENGPANEL, device_commands.Button_651, 651)
            AddFunction(new PushButton(this, ENGPANEL, "3652", "652", "Engine Start Panel", "Fuel Cutoff Switch"));    // elements["PTN_652"] = default_2_position_tumb(_("Fuel Cutoff Switch"), devices.ENGPANEL, device_commands.Button_652, 652)
            AddFunction(new PushButton(this, INSTPANEL, "3477", "477", "Engine Start Panel", "Fuel Dump Switch Cover"));    // elements["PTN_477"] = default_2_position_tumb(_("Fuel Dump Switch Cover"), devices.INSTPANEL, device_commands.Button_477, 477)
            AddFunction(new PushButton(this, INSTPANEL, "3478", "478", "Engine Start Panel", "Fuel Dump Switch"));    // elements["PTN_478"] = default_2_position_tumb(_("Fuel Dump Switch"), devices.INSTPANEL, device_commands.Button_478, 478)
            AddFunction(new PushButton(this, INSTPANEL, "3471", "471", "Engine Start Panel", "A/B Emergency Cutoff Switch Cover"));    // elements["PTN_471"] = default_2_position_tumb(_("A/B Emergency Cutoff Switch Cover"), devices.INSTPANEL, device_commands.Button_471, 471)
            AddFunction(new PushButton(this, INSTPANEL, "3472", "472", "Engine Start Panel", "A/B Emergency Cutoff Switch"));    // elements["PTN_472"] = default_2_position_tumb(_("A/B Emergency Cutoff Switch"), devices.INSTPANEL, device_commands.Button_472, 472)
            AddFunction(new PushButton(this, ENGPANEL, "3464", "464", "Engine Start Panel", "Emergency Throttle Cover"));    // elements["PTN_464"] = default_2_position_tumb(_("Emergency Throttle Cover"), devices.ENGPANEL, device_commands.Button_464, 464)
            AddFunction(new Axis(this, ENGPANEL, "3465", "465", 0.15d, 0d, 1d, "Engine Start Panel", "Emergency Throttle Handle"));    // elements["PTN_465"] = default_axis_limited(_("Emergency Throttle Handle"),devices.ENGPANEL,device_commands.Button_465,465, 0.8, 0.5, true, false, {0.0, 1.0})
            AddFunction(new PushButton(this, ENGPANEL, "3473", "473", "Engine Start Panel", "Secondary Oil Control Cover"));    // elements["PTN_473"] = default_2_position_tumb(_("Secondary Oil Control Cover"), devices.ENGPANEL, device_commands.Button_473, 473)
            AddFunction(new PushButton(this, ENGPANEL, "3474", "474", "Engine Start Panel", "Secondary Oil Control Switch"));    // elements["PTN_474"] = default_2_position_tumb(_("Secondary Oil Control Switch"), devices.ENGPANEL, device_commands.Button_474, 474)
            AddFunction(new PushButton(this, ENGPANEL, "3475", "475", "Engine Start Panel", "Engine Emergency Control Cover"));    // elements["PTN_475"] = default_2_position_tumb(_("Engine Emergency Control Cover"), devices.ENGPANEL, device_commands.Button_475, 475)
            AddFunction(new Switch(this, ENGPANEL, "476", new SwitchPosition[] { }, "Engine Start Panel", "Engine Emergency Control Switch", "%0.1f"));    // elements["PTN_476"] = multiposition_switch_limited(_("Engine Emergency Control Switch"), devices.ENGPANEL, device_commands.Button_476, 476,3, 0.5, true, 0)
            #endregion  
            #region  Fuel Panel"
            AddFunction(new Switch(this, ENGPANEL, "193", new SwitchPosition[] { }, "Fuel Panel", "Refuel Transfer Switch", "%0.1f"));    // elements["PTN_193"] = multiposition_switch_limited(_("Refuel Transfer Switch"), devices.ENGPANEL, device_commands.Button_193, 193, 3, 0.5, false, 0)
            AddFunction(new Switch(this, INSTPANEL, "360", new SwitchPosition[] { }, "Fuel Panel", "Bingo Fuel 1 000 kg Selector", "%0.1f"));    // elements["PTN_360"] = default_multiposition_knob(_("Bingo Fuel 1,000 kg Selector"), devices.INSTPANEL, device_commands.Button_360, 360,  10, 0.1, false, 0)
            AddFunction(new Switch(this, INSTPANEL, "361", new SwitchPosition[] { }, "Fuel Panel", "Bingo Fuel 100 kg Selector", "%0.1f"));    // elements["PTN_361"] = default_multiposition_knob(_("Bingo Fuel 100 kg Selector"), devices.INSTPANEL, device_commands.Button_361, 361,  10, 0.1, false, 0)
            AddFunction(new PushButton(this, INSTPANEL, "3357", "357", "Fuel Panel", "Fuel Crossfeeed Switch"));    // elements["PTN_357"] = default_2_position_tumb(_("Fuel Crossfeeed Switch"), devices.INSTPANEL, device_commands.Button_357, 357)
            #endregion  
            #region  Radio Panel
            AddFunction(new PushButton(this, UHF, "3429", "429", "Radio Panel", "UHF Power 5W/25W Switch"));    // elements["PTN_429"] = default_2_position_tumb(_("UHF Power 5W/25W Switch"), devices.UHF, device_commands.Button_429, 429)
            AddFunction(new PushButton(this, UHF, "3430", "430", "Radio Panel", "UHF SIL Switch"));    // elements["PTN_430"] = default_2_position_tumb(_("UHF SIL Switch"), devices.UHF, device_commands.Button_430, 430)
            //AddFunction(Switch.CreateToggleSwitch(this, UHF, "3431", "431", "Radio Panel", "UHF E+A2 Switch", "%0.1f"));    // elements["PTN_431"] = default_2_way_spring_switch(_("UHF E+A2 Switch"), devices.UHF, device_commands.Button_431, 431, true)
            AddFunction(new PushButton(this, UHF, "3432", "432", "Radio Panel", "UHF CDE Switch"));    // elements["PTN_432"] = default_button(_("UHF CDE Switch"), devices.UHF, device_commands.Button_432, 432)
            AddFunction(new Switch(this, UHF, "433", new SwitchPosition[] { }, "Radio Panel", "UHF Mode Switch", "%0.1f"));    // elements["PTN_433"] = multiposition_switch_limited(_("UHF Mode Switch"), devices.UHF, device_commands.Button_433, 433, 4, 0.25, false, 0)
            AddFunction(new PushButton(this, UHF, "3434", "434", "Radio Panel", "UHF TEST Switch"));    // elements["PTN_434"] = default_button(_("UHF TEST Switch"), devices.UHF, device_commands.Button_434, 434)
            AddFunction(new Switch(this, UHF, "435", new SwitchPosition[] { }, "Radio Panel", "UHF Knob", "%0.1f"));    // elements["PTN_435"] = default_multiposition_knob(_("UHF Knob"), devices.UHF, device_commands.Button_435, 435, 20, 0.05,false,0.05)
            AddFunction(new PushButton(this, UVHF, "3437", "437", "Radio Panel", "U/VHF TEST Switch"));    // elements["PTN_437"] = default_button(_("U/VHF TEST Switch"), devices.UVHF, device_commands.Button_437, 437)
            //AddFunction(Switch.CreateToggleSwitch(this, UVHF, "3438", "438", "Radio Panel", "U/VHF E+A2 Switch", "%0.1f"));    // elements["PTN_438"] = default_2_way_spring_switch(_("U/VHF E+A2 Switch"), devices.UVHF, device_commands.Button_438, 438, true)
            AddFunction(new PushButton(this, UVHF, "3439", "439", "Radio Panel", "U/VHF SIL Switch"));    // elements["PTN_439"] = default_2_position_tumb(_("U/VHF SIL Switch"), devices.UVHF, device_commands.Button_439, 439)
            AddFunction(new Switch(this, UVHF, "440", new SwitchPosition[] { }, "Radio Panel", "U/VHF 100 MHz Selector", "%0.1f"));    // elements["PTN_440"] = default_multiposition_knob(_("U/VHF 100 MHz Selector"), devices.UVHF, device_commands.Button_440, 440, 4, 0.1, false, 0)
            AddFunction(new Switch(this, UVHF, "441", new SwitchPosition[] { }, "Radio Panel", "U/VHF 10 MHz Selector", "%0.1f"));    // elements["PTN_441"] = default_multiposition_knob(_("U/VHF 10 MHz Selector"), devices.UVHF, device_commands.Button_441, 441, 10, 0.1, false, 0)
            AddFunction(new Switch(this, UVHF, "442", new SwitchPosition[] { }, "Radio Panel", "U/VHF 1 MHz Selector", "%0.1f"));    // elements["PTN_442"] = default_multiposition_knob(_("U/VHF 1 MHz Selector"), devices.UVHF, device_commands.Button_442, 442, 10, 0.1, false, 0)
            AddFunction(new Switch(this, UVHF, "443", new SwitchPosition[] { }, "Radio Panel", "U/VHF 100 KHz Selector", "%0.1f"));    // elements["PTN_443"] = default_multiposition_knob(_("U/VHF 100 KHz Selector"), devices.UVHF, device_commands.Button_443, 443, 10, 0.1, false, 0)
            AddFunction(new Switch(this, UVHF, "444", new SwitchPosition[] { }, "Radio Panel", "U/VHF 25 KHz Selector", "%0.1f"));    // elements["PTN_444"] = default_multiposition_knob(_("U/VHF 25 KHz Selector"), devices.UVHF, device_commands.Button_444, 444, 4, 0.25, false, 0)
            AddFunction(new Switch(this, UVHF, "445", new SwitchPosition[] { }, "Radio Panel", "U/VHF Knob", "%0.1f"));    // elements["PTN_445"] = default_multiposition_knob(_("U/VHF Knob"), devices.UVHF, device_commands.Button_445, 445, 20, 0.05,false,0.05)
            AddFunction(new Switch(this, UVHF, "446", new SwitchPosition[] { }, "Radio Panel", "U/VHF Mode Switch 1", "%0.1f"));    // elements["PTN_446"] = multiposition_switch_limited(_("U/VHF Mode Switch 1"), devices.UVHF, device_commands.Button_446, 446, 5, 0.25, false, 0)
            AddFunction(new PushButton(this, UVHF, "3447", "447", "Radio Panel", "U/VHF Power 5W/25W Switch"));    // elements["PTN_447"] = default_2_position_tumb(_("U/VHF Power 5W/25W Switch"), devices.UVHF, device_commands.Button_447, 447)
            AddFunction(new Switch(this, UVHF, "448", new SwitchPosition[] { }, "Radio Panel", "U/VHF Manual/Preset Mode Selector", "%0.1f"));    // elements["PTN_448"] = multiposition_switch_limited(_("U/VHF Manual/Preset Mode Selector"), devices.UVHF, device_commands.Button_448, 448, 3, 0.50, false, 0)
            #endregion  
            #region  Right MDI
            #endregion
            #region  Navigational Antennas
            AddFunction(new Switch(this, VORILS, "616", new SwitchPosition[] { }, "Navigational Antennas", "VOR/ILS Frequency Change Whole", "%0.1f"));    // elements["PTN_616"] = default_multiposition_knob(_("VOR/ILS Frequency Change Whole"),devices.VORILS,device_commands.Button_616,616, 11, 0.1, false, 0)
            AddFunction(new PushButton(this, VORILS, "3617", "617", "Navigational Antennas", "VOR/ILS Power Dial"));    // elements["PTN_617"] = default_2_position_tumb(_("VOR/ILS Power Dial"), devices.VORILS,device_commands.Button_617,617)
            AddFunction(new Switch(this, VORILS, "618", new SwitchPosition[] { }, "Navigational Antennas", "VOR/ILS Frequency Change Decimal", "%0.1f"));    // elements["PTN_618"] = default_multiposition_knob(_("VOR/ILS Frequency Change Decimal"),devices.VORILS,device_commands.Button_618,618, 20, 0.05,false,0.0)
            AddFunction(new PushButton(this, VORILS, "3619", "619", "Navigational Antennas", "VOR/ILS Test Dial"));    // elements["PTN_619"] = default_2_position_tumb(_("VOR/ILS Test Dial"), devices.VORILS,device_commands.Button_619,619)
            AddFunction(new Switch(this, TACAN, "623", new SwitchPosition[] { }, "Navigational Antennas", "TACAN Channel 10 Selector", "%0.1f"));    // elements["PTN_623"] = default_multiposition_knob(_("TACAN Channel 10 Selector"),devices.TACAN,device_commands.Button_623,623, 13, 0.076923, false, 0.0)
            AddFunction(new PushButton(this, TACAN, "3624", "624", "Navigational Antennas", "TACAN X/Y Select"));    // elements["PTN_624"] = default_2_position_tumb(_("TACAN X/Y Select"),devices.TACAN,device_commands.Button_624,624)
            AddFunction(new Switch(this, TACAN, "625", new SwitchPosition[] { }, "Navigational Antennas", "TACAN Channel 1 Selector", "%0.1f"));    // elements["PTN_625"] = default_multiposition_knob(_("TACAN Channel 1 Selector"),devices.TACAN,device_commands.Button_625,625, 10, 0.1, false, 0)
            AddFunction(new Switch(this, TACAN, "626", new SwitchPosition[] { }, "Navigational Antennas", "TACAN Mode Select", "%0.1f"));    // elements["PTN_626"] = multiposition_switch_limited(_("TACAN Mode Select"), devices.TACAN, device_commands.Button_626, 626, 4, 0.33, false, 0)
            #endregion  
            #region  Miscellaneous Left Panel
            AddFunction(new PushButton(this, MISCPANELS, "3400", "400", "Miscellaneous Left Panel", "Cockpit Clock"));    // elements["PTN_400"] = default_2_position_tumb(_("Cockpit Clock"), devices.MISCPANELS, device_commands.Button_400, 400)
            AddFunction(new PushButton(this, SYSLIGHTS, "", "191", "Miscellaneous Left Panel", "Audio Warning Reset"));    // elements["PTN_191"] = default_button(_("Audio Warning Reset"), devices.SYSLIGHTS, Keys.PlaneResetMasterWarning, 191, 0, 1)
            AddFunction(new PushButton(this, MISCPANELS, "3458", "458", "Miscellaneous Left Panel", "Anti-Skid Switch Cover"));    // elements["PTN_458"] = default_2_position_tumb(_("Anti-Skid Switch Cover"), devices.MISCPANELS, device_commands.Button_458, 458)
            AddFunction(new PushButton(this, MISCPANELS, "3459", "459", "Miscellaneous Left Panel", "Anti-Skid Switch"));    // elements["PTN_459"] = default_2_position_tumb(_("Anti-Skid Switch"), devices.MISCPANELS, device_commands.Button_459, 459)
            //AddFunction(Switch.CreateToggleSwitch(this, MISCPANELS, "3666", "666", "Miscellaneous Left Panel", "Parking Brake Lever", "%0.1f"));    // elements["PTN_666"] = default_animated_lever(_("Parking Brake Lever"), devices.MISCPANELS, device_commands.Button_666, 666,5.0)
            //AddFunction(Switch.CreateToggleSwitch(this, SUBSYSTEMS, "3456", "456", "Miscellaneous Left Panel", "Canopy Jettison", "%0.1f"));    // elements["PTN_456"] = default_animated_lever(_("Canopy Jettison"),devices.SUBSYSTEMS, device_commands.Button_456, 456,5.0)
            //AddFunction(Switch.CreateToggleSwitch(this, MISCPANELS, "3457", "457", "Miscellaneous Left Panel", "Drag Chute Lever", "%0.1f"));    // elements["PTN_457"] = default_animated_lever(_("Drag Chute Lever"), devices.MISCPANELS, device_commands.Button_457, 457,5.0)
            //AddFunction(Switch.CreateToggleSwitch(this, MISCPANELS, "3807", "807", "Miscellaneous Left Panel", "Nose Wheel Steering / IFF Interrogation Button", "%0.1f"));    // elements["PTN_807"] = default_2_way_spring_switch(_("Nose Wheel Steering / IFF Interrogation Button"), devices.MISCPANELS, device_commands.Button_807, 807)
            AddFunction(new PushButton(this, SUBSYSTEMS, "3655", "655", "Miscellaneous Left Panel", "Canopy Rest"));    // elements["PTN_655"] = default_2_position_tumb(_("Canopy Rest"),devices.SUBSYSTEMS, device_commands.Button_655, 655)
            //AddFunction((this, SUBSYSTEMS, "3656", "656", "Miscellaneous Left Panel", "Canopy Lock/Neutral/Lower Lever"));    // elements["PTN_656"] = default_multiposition_animated_lever(_("Canopy Lock/Neutral/Lower Lever"), devices.SUBSYSTEMS, device_commands.Button_656, 656, 3, 0.5, false, 0,2.5)
            AddFunction(new PushButton(this, PCN_NAV, "3905", "905", "Miscellaneous Left Panel", "Emergency Compass"));    // elements["PTN_905"] = default_2_position_tumb(_("Emergency Compass"),devices.PCN_NAV, device_commands.Button_905, 905)
            AddFunction(new PushButton(this, SUBSYSTEMS, "3907", "907", "Miscellaneous Left Panel", "Canopy Handle"));    // elements["PTN_907"] = default_2_position_tumb(_("Canopy Handle"),devices.SUBSYSTEMS, device_commands.Button_907, 907)
            AddFunction(new PushButton(this, SUBSYSTEMS, "3908", "908", "Miscellaneous Left Panel", "Canopy Handle (alt?)"));    // elements["PTN_908"] = default_2_position_tumb(_("Canopy Handle"),devices.SUBSYSTEMS, device_commands.Button_908, 908)
            AddFunction(new PushButton(this, MISCPANELS, "3909", "909", "Miscellaneous Left Panel", "Mirror Rendering Toggle"));    // elements["PTN_909"] = default_2_position_tumb(_("Mirror Rendering Toggle"), devices.MISCPANELS, device_commands.Button_909, 909)
            #endregion  
            #region  Miscellaneous Right Panel
            AddFunction(new Switch(this, ENGINE, "657", new SwitchPosition[] { }, "Miscellaneous Right Panel", "Emergency Hydraulic Pump Switch", "%0.1f"));    // elements["PTN_657"] = multiposition_switch_spring(_("Emergency Hydraulic Pump Switch"), devices.ENGINE, device_commands.Button_657, device_commands.Button_657, 657)
            AddFunction(new PushButton(this, SYSLIGHTS, "3658", "658", "Miscellaneous Right Panel", "Audio Warning Switch"));    // elements["PTN_658"] = default_2_position_tumb(_("Audio Warning Switch"), devices.SYSLIGHTS, device_commands.Button_658, 658)
            AddFunction(new PushButton(this, MISCPANELS, "3659", "659", "Miscellaneous Right Panel", "Pitot Heat Cover"));    // elements["PTN_659"] = default_2_position_tumb(_("Pitot Heat Cover"), devices.MISCPANELS, device_commands.Button_659, 659)
            AddFunction(new PushButton(this, MISCPANELS, "3660", "660", "Miscellaneous Right Panel", "Pitot Heat Switch"));    // elements["PTN_660"] = default_2_position_tumb(_("Pitot Heat Switch"), devices.MISCPANELS, device_commands.Button_660, 660)
            #endregion  
            #region  Miscellaneous Seat
            //AddFunction(Switch.CreateToggleSwitch(this, MISCPANELS, "3900", "900", "Miscellaneous Seat", "Seat Adjustment Switch", "%0.1f"));    // elements["PTN_900"] = default_2_way_spring_switch(_("Seat Adjustment Switch"), devices.MISCPANELS, device_commands.Button_900, 900,true)
            AddFunction(new PushButton(this, ECS, "3910", "910", "Miscellaneous Seat", "LOX Dilution Lever"));    // elements["PTN_910"] = default_2_position_tumb(_("LOX Dilution Lever"),devices.ECS, device_commands.Button_910,  910)
            AddFunction(new PushButton(this, ECS, "3912", "912", "Miscellaneous Seat", "LOX Emergency Supply"));    // elements["PTN_912"] = default_2_position_tumb(_("LOX Emergency Supply"), devices.ECS, device_commands.Button_912, 912)
            #endregion  
            #region  Sound Panel
            AddFunction(new PushButton(this, SYSLIGHTS, "3700", "700", "Sound Panel", "AMPLIS Selector Knob"));    // elements["PTN_700"] = default_2_position_tumb(_("AMPLIS Selector Knob"), devices.SYSLIGHTS, device_commands.Button_700, 700)
            AddFunction(new Axis(this, SYSLIGHTS, "3701", "701", 0.15d, 0d, 1d, "Sound Panel", "VOR/ILS Volume Knob"));    // elements["PTN_701"] = default_axis_limited(_("VOR/ILS Volume Knob"), devices.SYSLIGHTS, device_commands.Button_701, 701, 0.8, 0.5, true, false, {0.0, 1.0})
            AddFunction(new Axis(this, SYSLIGHTS, "3702", "702", 0.15d, 0d, 1d, "Sound Panel", "TACAN Volume Knob"));    // elements["PTN_702"] = default_axis_limited(_("TACAN Volume Knob"), devices.SYSLIGHTS, device_commands.Button_702, 702, 0.8, 0.5, true, false, {0.0, 1.0})
            AddFunction(new Axis(this, SYSLIGHTS, "3703", "703", 0.15d, 0d, 1d, "Sound Panel", "MAGIC Tone Volume Knob"));    // elements["PTN_703"] = default_axis_limited(_("MAGIC Tone Volume Knob"), devices.SYSLIGHTS, device_commands.Button_703, 703, 0.8, 0.5, true, false, {0.0, 1.0})
            AddFunction(new Axis(this, SYSLIGHTS, "3704", "704", 0.15d, 0d, 1d, "Sound Panel", "TB APP Volume Knob"));    // elements["PTN_704"] = default_axis_limited(_("TB APP Volume Knob"), devices.SYSLIGHTS, device_commands.Button_704, 704, 0.8, 0.5, true, false, {0.0, 1.0})
            AddFunction(new Axis(this, SYSLIGHTS, "3705", "705", 0.15d, 0d, 1d, "Sound Panel", "Marker Signal Volume Knob"));    // elements["PTN_705"] = default_axis_limited(_("Marker Signal Volume Knob"), devices.SYSLIGHTS, device_commands.Button_705, 705, 0.8, 0.5, true, false, {0.0, 1.0})
            AddFunction(new Axis(this, SYSLIGHTS, "3706", "706", 0.15d, 0d, 1d, "Sound Panel", "UHF Radio Volume Knob"));    // elements["PTN_706"] = default_axis_limited(_("UHF Radio Volume Knob"), devices.SYSLIGHTS, device_commands.Button_706, 706, 0.8, 0.5, true, false, {0.0, 1.0})
            AddFunction(new Axis(this, SYSLIGHTS, "3707", "707", 0.15d, 0d, 1d, "Sound Panel", "V/UHF Radio Volume Knob"));    // elements["PTN_707"] = default_axis_limited(_("V/UHF Radio Volume Knob"), devices.SYSLIGHTS, device_commands.Button_707, 707, 0.8, 0.5, true, false, {0.0, 1.0})
            #endregion  
            #region  Flight Instruments
            AddFunction(new Axis(this, FLIGHTINST, "3309", "309", 0.15d, 0d, 1d, "Flight Instruments", "Barometric Pressure Calibration"));    // elements["PTN_309"] = default_axis(_("Barometric Pressure Calibration"),devices.FLIGHTINST,device_commands.Button_309,309)
            AddFunction(new PushButton(this, FLIGHTINST, "3314", "314", "Flight Instruments", "ADI Cage Lever"));    // elements["PTN_314"] = default_2_position_tumb(_("ADI Cage Lever"),devices.FLIGHTINST, device_commands.Button_314, 314)
            AddFunction(new PushButton(this, FLIGHTINST, "3315", "315", "Flight Instruments", "ADI Backlight Switch"));    // elements["PTN_315"] = default_2_position_tumb(_("ADI Backlight Switch"),devices.FLIGHTINST,device_commands.Button_315,315)
            #endregion  
            #region  ECS Panel
            AddFunction(new PushButton(this, ECS, "3630", "630", "ECS Panel", "ECS Main Mode Switch"));    // elements["PTN_630"] = default_2_position_tumb(_("ECS Main Mode Switch"),devices.ECS, device_commands.Button_630, 630)
            AddFunction(new PushButton(this, ECS, "3631", "631", "ECS Panel", "ECS C Button"));    // elements["PTN_631"] = default_button(_("ECS C Button"), devices.ECS, device_commands.Button_631, 631, 1, 1)
            AddFunction(new PushButton(this, ECS, "3633", "633", "ECS Panel", "ECS F Button"));    // elements["PTN_633"] = default_button(_("ECS F Button"), devices.ECS, device_commands.Button_633, 633, 1, 1)
            AddFunction(new PushButton(this, ECS, "3635", "635", "ECS Panel", "ECS Cond Switch"));    // elements["PTN_635"] = default_2_position_tumb(_("ECS Cond Switch"),devices.ECS, device_commands.Button_635, 635)
            AddFunction(new PushButton(this, ECS, "3636", "636", "ECS Panel", "ECS Air Exchange Switch"));    // elements["PTN_636"] = default_2_position_tumb(_("ECS Air Exchange Switch"),devices.ECS, device_commands.Button_636, 636)
            AddFunction(new Axis(this, ECS, "3637", "637", 0.15d, 0d, 1d, "ECS Panel", "ECS Temperature Select Knob"));    // elements["PTN_637"] = default_axis_limited_cycle(_("ECS Temperature Select Knob"), devices.ECS, device_commands.Button_637, 637, 0.8, 0.5, true, false, {-1.0, 1.0})
            AddFunction(new PushButton(this, ECS, "3638", "638", "ECS Panel", "ECS Defog Switch"));    // elements["PTN_638"] = default_2_position_tumb(_("ECS Defog Switch"),devices.ECS, device_commands.Button_638, 638)
                                                                                                       // 
            #endregion
            #region  IFF
            AddFunction(new Switch(this, INSTPANEL, "377", new SwitchPosition[] { }, "IFF", "Mode-1 Tens Selector", "%0.1f"));    // elements["PTN_377"] = default_multiposition_knob(_("Mode-1 Tens Selector"), devices.INSTPANEL, device_commands.Button_377, 377,  10, 0.1, false, 0)
            AddFunction(new Switch(this, INSTPANEL, "378", new SwitchPosition[] { }, "IFF", "Mode-1 Ones Selector", "%0.1f"));    // elements["PTN_378"] = default_multiposition_knob(_("Mode-1 Ones Selector"), devices.INSTPANEL, device_commands.Button_378, 378,  10, 0.1, false, 0)
            AddFunction(new Switch(this, INSTPANEL, "379", new SwitchPosition[] { }, "IFF", "Mode-3A Thousands Selector", "%0.1f"));    // elements["PTN_379"] = default_multiposition_knob(_("Mode-3A Thousands Selector"), devices.INSTPANEL, device_commands.Button_379, 379,  10, 0.1, false, 0)
            AddFunction(new Switch(this, INSTPANEL, "380", new SwitchPosition[] { }, "IFF", "Mode-3A Hundreds Selector", "%0.1f"));    // elements["PTN_380"] = default_multiposition_knob(_("Mode-3A Hundreds Selector"), devices.INSTPANEL, device_commands.Button_380, 380,  10, 0.1, false, 0)
            AddFunction(new Switch(this, INSTPANEL, "381", new SwitchPosition[] { }, "IFF", "Mode-3A Tens Selector", "%0.1f"));    // elements["PTN_381"] = default_multiposition_knob(_("Mode-3A Tens Selector"), devices.INSTPANEL, device_commands.Button_381, 381,  10, 0.1, false, 0)
            AddFunction(new Switch(this, INSTPANEL, "382", new SwitchPosition[] { }, "IFF", "Mode-3A Ones Selector", "%0.1f"));    // elements["PTN_382"] = default_multiposition_knob(_("Mode-3A Ones Selector"), devices.INSTPANEL, device_commands.Button_382, 382,  10, 0.1, false, 0)
            AddFunction(new Switch(this, INSTPANEL, "383", new SwitchPosition[] { }, "IFF", "Ident Power Switch", "%0.1f"));    // elements["PTN_383"] = default_3_position_tumb(_("Ident Power Switch"), devices.INSTPANEL, device_commands.Button_383, 383, false, false)
            AddFunction(new PushButton(this, INSTPANEL, "3384", "384", "IFF", "Mode-1 Switch"));    // elements["PTN_384"] = default_2_position_tumb(_("Mode-1 Switch"), devices.INSTPANEL, device_commands.Button_384, 384)
            AddFunction(new PushButton(this, INSTPANEL, "3385", "385", "IFF", "Mode-2 Switch"));    // elements["PTN_385"] = default_2_position_tumb(_("Mode-2 Switch"), devices.INSTPANEL, device_commands.Button_385, 385)
            AddFunction(new PushButton(this, INSTPANEL, "3386", "386", "IFF", "Mode-3A Switch"));    // elements["PTN_386"] = default_2_position_tumb(_("Mode-3A Switch"), devices.INSTPANEL, device_commands.Button_386, 386)
            AddFunction(new PushButton(this, INSTPANEL, "3387", "387", "IFF", "Mode-C Switch"));    // elements["PTN_387"] = default_2_position_tumb(_("Mode-C Switch"), devices.INSTPANEL, device_commands.Button_387, 387)
            #endregion  
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
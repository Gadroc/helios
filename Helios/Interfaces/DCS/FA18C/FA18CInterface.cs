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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.FA18C
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Interfaces.DCS.AV8B.Functions;
    //using GadrocsWorkshop.Helios.Interfaces.DCS.FA18C.Functions;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using Microsoft.Win32;
    using System;

    [HeliosInterface("Helios.FA18C", "DCS FA-18C", typeof(FA18CInterfaceEditor), typeof(UniqueHeliosInterfaceFactory))]
    public class FA18CInterface : BaseUDPInterface
    {
        private string _dcsPath;

        private bool _phantomFix;
        private int _phantomLeft;
        private int _phantomTop;

        private long _nextCheck = 0;

        #region Devices
        //  From devices.lua - DCS seem to want this to remain constant which is great 
        private const string FM_PROXY = "1";
        private const string CONTROL_INTERFACE = "2";
        private const string ELEC_INTERFACE = "3";
        private const string HYDRAULIC_INTERFACE = "4";
        private const string GEAR_INTERFACE = "5";
        private const string FUEL_INTERFACE = "6";
        private const string CPT_MECHANICS = "7";               // Integral DEVICE for various mechanical systems like canopy, etc
        private const string EXT_LIGHTS = "8";                  // External Lighting
        private const string CPT_LIGTHS = "9";                  // Cockpit Lighting
        private const string OXYGEN_INTERFACE = "10";           // Cockpit interface to OBOGS
        private const string ECS_INTERFACE = "11";              //  Environmental Control System
        private const string ENGINES_INTERFACE = "12";          //  Cockpit interface to aircraft engines
        //  HOTAS interface
        private const string HOTAS = "13";                      //  Is this a duplicate of 38 
        // Computers
        private const string MUX = "14";                        // Multiplex manager, holds channels and manages remote terminals addition/remove
        private const string SDC = "15";                        // Signal Data Computer (part of FIRAMS)
        private const string MISSION_COMPUTER_NO_1 = "16";      // AN/AYK-14, Mission Computer No 1
        private const string MISSION_COMPUTER_NO_2 = "17";      // AN/AYK-14, Mission Computer No 2
        private const string FCCA = "18";                       //  Flight Control Computer A
        private const string FCCB = "19";                       //  Flight Control Computer B
        private const string ADC = "20";                        //  Air Data Computer - CPO-1334/A
        private const string ARMAMENT_COMPUTER = "21";          //  Armament Computer - AYK-22
        private const string CONTROL_CONVERTER = "22";          // Control-Converter - C-10382/A
        private const string SMS = "23";                        // Stores Management System, Armament Computer - CP-2218/AYK-22(V)
        private const string DIGITAL_MAP_COMPUTER = "24";       //  Digital Map Computer - CP-1802/ASQ-196
        private const string UFC = "25";                        // Electronic Equipment Control (UFC) - C-10380/ASQ
        // Instruments
        private const string AAU52    = "26";                   //  Standby Pressure Altimeter - AAU-52/A
        private const string AVU35    = "27";                   //  Indicated Airspeed Indicator - AVU-35/A
        private const string AVU53    = "28";                   //  Vertical Speed Indicator - AVU-53/A
        private const string STANDBY_COMPASS = "29";            // Standby Compass - AQU-3/A
        private const string ID2163A = "30";                    // Radar Altimeter Height Indicator - ID-2163/A
        private const string RADAR_ALTIMETER = "31";            // Radar Altimeter - APN-194(V)
        private const string SAI = "32";                        // Standby Attitude Indicator - ARU-48
        // FIRAMS
        private const string IFEI = "33";                       // Integrated Fuel/Engine Indicator (IFEI)
        //  Multipurpose Display Group
        private const string HUD = "34";                        // Head-Up Display - AVQ-32
        private const string MDI_LEFT = "35";                   // Left Multipurpose Display Indicator (DDI) - IP-1556/A
        private const string MDI_RIGHT = "36";                  // Right Multipurpose Display Indicator (DDI) - IP-1556/A
        private const string AMPCD = "37";                      // Advanced Multipurpose Color Display - ???
        //  Stick and throttle grips
        private const string HOTASA = "38";                     // Stick and throttle grips //  Is this a duplicate of 13
        //  Radio & Comm
        private const string UHF1 = "39";                       // VHF/UHF Receiver-Transmitter - ARC 210
        private const string UHF2 = "40";                       // VHF/UHF Receiver-Transmitter - ARC 210 DCS
        private const string INTERCOM = "41";                   // Intercommunication Amplifier-Control - AM-7360/A
        private const string KY58 = "42";                       // KY-58 Secure Speech System
        //  Sensors
        private const string RADAR = "43";                      // Radar - AN/APG-73, interfaced to the rest of avionic system via the Radar Data Processor CP-2062/APG-73
        private const string FLIR = "44";                       // Forward Looking Infrared Pod interface
        //  INS/GPS
        private const string INS = "45";                        // INS, AN/ASN-139
        private const string GPS = "46";                        // GPS, AN/ASN-163
        private const string MAD = "47";                        // Magnetic Azimuth Detector, DT-604/A
        //  Armament
        private const string SIDEWINDER_INTERFACE = "48";
        private const string MAVERICK_INTERFACE = "49";
        //  RNAV
        private const string ADF = "50";                        //  Direction Finder OA-8697/ARD
        private const string ANTENNA_SELECTOR = "51";
        private const string MIDS = "52";                       // MIDS-LVT (implements Link 16 and TACAN)
        private const string ILS = "53";                        // AN/ARA-63D, airborne segment of US NAVY ACLS, and US Marines MRAALS
        //  TEWS
        private const string RWR = "54";                        // AN/ALR-67(V)
        private const string CMDS = "55";                       //  Countermeasures dispenser System

        private const string MACROS = "56";
        #endregion

        public FA18CInterface()
            : base("DCS FA18C")
        {
            DCSConfigurator config = new DCSConfigurator("DCSFA18C", DCSPath);
            config.ExportConfigPath = "Config\\Export";
            config.ExportFunctionsPath = "pack://application:,,,/Helios;component/Interfaces/DCS/FA18C/ExportFunctions.lua";
            Port = config.Port;
            _phantomFix = config.PhantomFix;
            _phantomLeft = config.PhantomFixLeft;
            _phantomTop = config.PhantomFixTop;

            #region MFD

            #region Left MFCD MFD

            //Left MDI

            AddFunction(new PushButton(this, MDI_LEFT, "3054", "3054", "Left MFCD", "OSB01", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3055", "3055", "Left MFCD", "OSB02", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3056", "3056", "Left MFCD", "OSB03", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3057", "3057", "Left MFCD", "OSB04", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3058", "3058", "Left MFCD", "OSB05", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3059", "3059", "Left MFCD", "OSB06", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3060", "3060", "Left MFCD", "OSB07", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3061", "3061", "Left MFCD", "OSB08", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3062", "3062", "Left MFCD", "OSB09", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3063", "3063", "Left MFCD", "OSB10", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3064", "3064", "Left MFCD", "OSB11", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3065", "3065", "Left MFCD", "OSB12", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3066", "3066", "Left MFCD", "OSB13", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3067", "3067", "Left MFCD", "OSB14", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3068", "3068", "Left MFCD", "OSB15", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3069", "3069", "Left MFCD", "OSB16", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3070", "3070", "Left MFCD", "OSB17", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3071", "3071", "Left MFCD", "OSB18", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3072", "3072", "Left MFCD", "OSB19", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3073", "3073", "Left MFCD", "OSB20", "1", "0", "%1d"));
            //elements["pnt_51"] = default_3_position_tumb(_("Left MDI Brightness Selector Knob, OFF/NIGHT/DAY"), devices.MDI_LEFT, MDI_commands.MDI_off_night_day, 51, false, anim_speed_default, false, 0.1, { 0, 0.2})
            //elements["pnt_52"] = default_axis_limited(_("Left MDI Brightness Control Knob"), devices.MDI_LEFT, MDI_commands.MDI_brightness, 52, 0.0, 0.1, false, false, { 0, 1})
            //elements["pnt_53"] = default_axis_limited(_("Left MDI Contrast Control Knob"), devices.MDI_LEFT, MDI_commands.MDI_contrast, 53, 0.0, 0.1, false, false, { 0, 1})
            AddFunction(new Axis(this, MDI_LEFT, "3194", "3194", 0.1d, 0d, 1d, "Left MFCD", "Off/Brightness Control"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_LEFT, "3220", "3220", "-1", "Day", "0", "Off", "1", "Night", "Left MFCD", "DAY/NIGHT Mode", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_LEFT, "3221", "3221", "-1", "More", "0", "Off", "1", "Less", "Left MFCD", "Symbology", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_LEFT, "3222", "3222", "-1", "Up", "0", "Off", "1", "Down", "Left MFCD", "Gain", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_LEFT, "3223", "3223", "-1", "Up", "0", "Off", "1", "Down", "Left MFCD", "Contrast", "%1d"));
            #endregion

            #region Centre MFCD AMPCD
            //elements["pnt_312"] = springloaded_3_pos_tumb2(_("Heading Set Switch"), devices.MDI_LEFT, MDI_commands.MDI_Left_HDG_Negative, MDI_commands.MDI_Left_HDG_Positive, 312)
            //elements["pnt_313"] = springloaded_3_pos_tumb(_("Course Set Switch"), devices.MDI_LEFT, MDI_commands.MDI_Left_CRS_Negative, MDI_commands.MDI_Left_CRS_Positive, 313)

            AddFunction(new PushButton(this, AMPCD, "3183", "3183", "Centre MFD", "OSB01"));
            AddFunction(new PushButton(this, AMPCD, "3184", "3184", "Centre MFD", "OSB02"));
            AddFunction(new PushButton(this, AMPCD, "3185", "3185", "Centre MFD", "OSB03"));
            AddFunction(new PushButton(this, AMPCD, "3186", "3186", "Centre MFD", "OSB04"));
            AddFunction(new PushButton(this, AMPCD, "3187", "3187", "Centre MFD", "OSB05"));
            AddFunction(new PushButton(this, AMPCD, "3188", "3188", "Centre MFD", "OSB06"));
            AddFunction(new PushButton(this, AMPCD, "3189", "3189", "Centre MFD", "OSB07"));
            AddFunction(new PushButton(this, AMPCD, "3190", "3190", "Centre MFD", "OSB08"));
            AddFunction(new PushButton(this, AMPCD, "3191", "3191", "Centre MFD", "OSB09"));
            AddFunction(new PushButton(this, AMPCD, "3192", "3192", "Centre MFD", "OSB10"));
            AddFunction(new PushButton(this, AMPCD, "3193", "3193", "Centre MFD", "OSB11"));
            AddFunction(new PushButton(this, AMPCD, "3194", "3194", "Centre MFD", "OSB12"));
            AddFunction(new PushButton(this, AMPCD, "3195", "3195", "Centre MFD", "OSB13"));
            AddFunction(new PushButton(this, AMPCD, "3196", "3196", "Centre MFD", "OSB14"));
            AddFunction(new PushButton(this, AMPCD, "3197", "3197", "Centre MFD", "OSB15"));
            AddFunction(new PushButton(this, AMPCD, "3198", "3198", "Centre MFD", "OSB16"));
            AddFunction(new PushButton(this, AMPCD, "3199", "3199", "Centre MFD", "OSB17"));
            AddFunction(new PushButton(this, AMPCD, "3200", "3200", "Centre MFD", "OSB18"));
            AddFunction(new PushButton(this, AMPCD, "3201", "3201", "Centre MFD", "OSB19"));
            AddFunction(new PushButton(this, AMPCD, "3202", "3202", "Centre MFD", "OSB20"));
            AddFunction(new Axis(this, AMPCD, "3203", "3203", 0.1d, 0d, 1d, "Centre MFD", "Off/Brightness Control"));

            //elements["pnt_177_2"] = AMPCD_switch_positive(_("AMPCD Night/Day Brightness Selector, DAY"), devices.AMPCD, AMPCD_commands.AMPCD_nite_day_DAY, 177)
            //elements["pnt_177_1"] = AMPCD_switch_negative(_("AMPCD Night/Day Brightness Selector, NGT"), devices.AMPCD, AMPCD_commands.AMPCD_nite_day_NGT, 177)
            //elements["pnt_179_2"] = AMPCD_switch_positive(_("AMPCD Symbology Control Switch, UP"), devices.AMPCD, AMPCD_commands.AMPCD_symbology_UP, 179)
            //elements["pnt_179_1"] = AMPCD_switch_negative(_("AMPCD Symbology Control Switch, DOWN"), devices.AMPCD, AMPCD_commands.AMPCD_symbology_DOWN, 179)
            //elements["pnt_182_2"] = AMPCD_switch_positive(_("AMPCD Contrast Control Switch, UP"), devices.AMPCD, AMPCD_commands.AMPCD_contrast_UP, 182)
            //elements["pnt_182_1"] = AMPCD_switch_negative(_("AMPCD Contrast Control Switch, DOWN"), devices.AMPCD, AMPCD_commands.AMPCD_contrast_DOWN, 182)
            //elements["pnt_180_2"] = AMPCD_switch_positive(_("AMPCD Gain Control Switch, UP"), devices.AMPCD, AMPCD_commands.AMPCD_gain_UP, 180)
            //elements["pnt_180_1"] = AMPCD_switch_negative(_("AMPCD Gain Control Switch, DOWN"), devices.AMPCD, AMPCD_commands.AMPCD_gain_DOWN, 180)

            AddFunction(Switch.CreateThreeWaySwitch(this, AMPCD, "3244", "3244", "-1", "Day", "0", "Off", "1", "Night", "Centre MFD", "DAY/NIGHT Mode", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, AMPCD, "3245", "3245", "-1", "More", "0", "Off", "1", "Less", "Centre MFD", "Symbology", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, AMPCD, "3246", "3246", "-1", "Up", "0", "Off", "1", "Down", "Centre MFD", "Gain", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, AMPCD, "3247", "3247", "-1", "Up", "0", "Off", "1", "Down", "Centre MFD", "Contrast", "%1d"));
            #endregion

            #region Right MFCD MFD

            AddFunction(new PushButton(this, MDI_RIGHT, "3079", "3079", "Right MFCD", "OSB01"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3080", "3080", "Right MFCD", "OSB02"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3081", "3081", "Right MFCD", "OSB03"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3082", "3082", "Right MFCD", "OSB04"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3083", "3083", "Right MFCD", "OSB05"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3084", "3084", "Right MFCD", "OSB06"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3085", "3085", "Right MFCD", "OSB07"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3086", "3086", "Right MFCD", "OSB08"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3087", "3087", "Right MFCD", "OSB09"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3088", "3088", "Right MFCD", "OSB10"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3089", "3089", "Right MFCD", "OSB11"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3090", "3090", "Right MFCD", "OSB12"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3091", "3091", "Right MFCD", "OSB13"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3092", "3092", "Right MFCD", "OSB14"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3093", "3093", "Right MFCD", "OSB15"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3094", "3094", "Right MFCD", "OSB16"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3095", "3095", "Right MFCD", "OSB17"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3096", "3096", "Right MFCD", "OSB18"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3097", "3097", "Right MFCD", "OSB19"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3098", "3098", "Right MFCD", "OSB20"));
            //elements["pnt_76"] = default_3_position_tumb(_("Right MDI Brightness Selector Knob, OFF/NIGHT/DAY"), devices.MDI_RIGHT, MDI_commands.MDI_off_night_day, 76, false, anim_speed_default, false, 0.1, { 0, 0.2})
            //elements["pnt_77"] = default_axis_limited(_("Right MDI Brightness Control Knob"), devices.MDI_RIGHT, MDI_commands.MDI_brightness, 77, 0.0, 0.1, false, false, { 0, 1})
            //elements["pnt_78"] = default_axis_limited(_("Right MDI Contrast Control Knob"), devices.MDI_RIGHT, MDI_commands.MDI_contrast, 78, 0.0, 0.1, false, false, { 0, 1})
            AddFunction(new Axis(this, MDI_RIGHT, "3195", "3195", 0.1d, 0d, 1d, "Right MFCD", "Off/Brightness Control"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_RIGHT, "3244", "3244", "-1", "Day", "0", "Off", "1", "Night", "Right MFCD", "DAY/NIGHT Mode", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_RIGHT, "3245", "3245", "-1", "More", "0", "Off", "1", "Less", "Right MFCD", "Symbology", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_RIGHT, "3246", "3246", "-1", "Up", "0", "Off", "1", "Down", "Right MFCD", "Gain", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_RIGHT, "3247", "3247", "-1", "Up", "0", "Off", "1", "Down", "Right MFCD", "Contrast", "%1d"));
            #endregion
            #endregion

            #region Caution Indicators
            // Caution Light Indicator Panel
            AddFunction(new FlagValue(this, "298", "Caution Indicators", "CK_SEAT", ""));       // create_caution_lamp(298,CautionLights.CPT_LTS_CK_SEAT)
            AddFunction(new FlagValue(this, "299", "Caution Indicators", "APU_ACC", ""));       // create_caution_lamp(299,CautionLights.CPT_LTS_APU_ACC)
            AddFunction(new FlagValue(this, "300", "Caution Indicators", "BATT_SW", ""));       // create_caution_lamp(300,CautionLights.CPT_LTS_BATT_SW)
            AddFunction(new FlagValue(this, "301", "Caution Indicators", "FCS_HOT", ""));       // create_caution_lamp(301,CautionLights.CPT_LTS_FCS_HOT)
            AddFunction(new FlagValue(this, "302", "Caution Indicators", "GEN_TIE", ""));       // create_caution_lamp(302,CautionLights.CPT_LTS_GEN_TIE)
            AddFunction(new FlagValue(this, "303", "Caution Indicators", "SPARE_CTN1", ""));    // create_caution_lamp(303,CautionLights.CPT_LTS_SPARE_CTN1)
            AddFunction(new FlagValue(this, "304", "Caution Indicators", "FUEL_LO", ""));       // create_caution_lamp(304,CautionLights.CPT_LTS_FUEL_LO)
            AddFunction(new FlagValue(this, "305", "Caution Indicators", "FCES", ""));          // create_caution_lamp(305,CautionLights.CPT_LTS_FCES)
            AddFunction(new FlagValue(this, "306", "Caution Indicators", "SPARE_CTN2", ""));    // create_caution_lamp(306,CautionLights.CPT_LTS_SPARE_CTN2)
            AddFunction(new FlagValue(this, "307", "Caution Indicators", "L_GEN", ""));         // create_caution_lamp(307,CautionLights.CPT_LTS_L_GEN)
            AddFunction(new FlagValue(this, "308", "Caution Indicators", "R_GEN", ""));         // create_caution_lamp(308,CautionLights.CPT_LTS_R_GEN)
            AddFunction(new FlagValue(this, "309", "Caution Indicators", "SPARE_CTN3", ""));    // create_caution_lamp(309,CautionLights.CPT_LTS_SPARE_CTN3)
            // LH Advisory and Threat Warning Indicator Panel
            AddFunction(new FlagValue(this, "13", "Caution Indicators", "MASTER_CAUTION", "")); // create_caution_lamp(13,CautionLights.CPT_LTS_MASTER_CAUTION)
            AddFunction(new FlagValue(this, "10", "Caution Indicators", "FIRE_LEFT", ""));      // create_caution_lamp(10,CautionLights.CPT_LTS_FIRE_LEFT)
            AddFunction(new FlagValue(this, "15", "Caution Indicators", "GO", ""));             // create_caution_lamp(15,CautionLights.CPT_LTS_GO)
            AddFunction(new FlagValue(this, "16", "Caution Indicators", "NO_GO", ""));          // create_caution_lamp(16,CautionLights.CPT_LTS_NO_GO)
            AddFunction(new FlagValue(this, "17", "Caution Indicators", "L_BLEED", ""));        // create_caution_lamp(17,CautionLights.CPT_LTS_L_BLEED)
            AddFunction(new FlagValue(this, "18", "Caution Indicators", "R_BLEED", ""));        // create_caution_lamp(18,CautionLights.CPT_LTS_R_BLEED)
            AddFunction(new FlagValue(this, "19", "Caution Indicators", "SPD_BRK", ""));        // create_caution_lamp(19,CautionLights.CPT_LTS_SPD_BRK)
            AddFunction(new FlagValue(this, "20", "Caution Indicators", "STBY", ""));           // create_caution_lamp(20,CautionLights.CPT_LTS_STBY)
            AddFunction(new FlagValue(this, "21", "Caution Indicators", "L_BAR_RED", ""));      // create_caution_lamp(21,CautionLights.CPT_LTS_L_BAR_RED)
            AddFunction(new FlagValue(this, "22", "Caution Indicators", "REC", ""));            // create_caution_lamp(22,CautionLights.CPT_LTS_REC)
            AddFunction(new FlagValue(this, "23", "Caution Indicators", "L_BAR_GREEN", ""));    // create_caution_lamp(23,CautionLights.CPT_LTS_L_BAR_GREEN)
            AddFunction(new FlagValue(this, "24", "Caution Indicators", "XMIT", ""));           // create_caution_lamp(24,CautionLights.CPT_LTS_XMIT)
            AddFunction(new FlagValue(this, "25", "Caution Indicators", "ASPJ_OH", ""));        // create_caution_lamp(25,CautionLights.CPT_LTS_ASPJ_OH)
            // RH Advisory and Threat Warning Indicator Panel
            AddFunction(new FlagValue(this, "29", "Caution Indicators", "FIRE_APU", ""));       // create_caution_lamp(29,CautionLights.CPT_LTS_FIRE_APU)
            AddFunction(new FlagValue(this, "26", "Caution Indicators", "FIRE_RIGHT", ""));     // create_caution_lamp(26,CautionLights.CPT_LTS_FIRE_RIGHT)
            AddFunction(new FlagValue(this, "31", "Caution Indicators", "RCDR_ON", ""));        // create_caution_lamp(31,CautionLights.CPT_LTS_RCDR_ON)
            AddFunction(new FlagValue(this, "32", "Caution Indicators", "DISP", ""));           // create_caution_lamp(32,CautionLights.CPT_LTS_DISP)
            AddFunction(new FlagValue(this, "38", "Caution Indicators", "SAM", ""));            // create_caution_lamp(38,CautionLights.CPT_LTS_SAM)
            AddFunction(new FlagValue(this, "39", "Caution Indicators", "AI", ""));             // create_caution_lamp(39,CautionLights.CPT_LTS_AI)
            AddFunction(new FlagValue(this, "40", "Caution Indicators", "AAA", ""));            // create_caution_lamp(40,CautionLights.CPT_LTS_AAA)
            AddFunction(new FlagValue(this, "41", "Caution Indicators", "CW", ""));             // create_caution_lamp(41,CautionLights.CPT_LTS_CW)
            AddFunction(new FlagValue(this, "33", "Caution Indicators", "SPARE_RH1", ""));      // create_caution_lamp(33,CautionLights.CPT_LTS_SPARE_RH1)
            AddFunction(new FlagValue(this, "34", "Caution Indicators", "SPARE_RH2", ""));      // create_caution_lamp(34,CautionLights.CPT_LTS_SPARE_RH2)
            AddFunction(new FlagValue(this, "35", "Caution Indicators", "SPARE_RH3", ""));      // create_caution_lamp(35,CautionLights.CPT_LTS_SPARE_RH3)
            AddFunction(new FlagValue(this, "36", "Caution Indicators", "SPARE_RH4", ""));      // create_caution_lamp(36,CautionLights.CPT_LTS_SPARE_RH4)
            AddFunction(new FlagValue(this, "37", "Caution Indicators", "SPARE_RH5", ""));      // create_caution_lamp(37,CautionLights.CPT_LTS_SPARE_RH5)
            // Flaps Landing Gear and Stores Indicator Panel
            AddFunction(new FlagValue(this, "152", "Caution Indicators", "CTR", ""));           // create_caution_lamp(152,CautionLights.CPT_LTS_CTR)
            AddFunction(new FlagValue(this, "154", "Caution Indicators", "LI", ""));            // create_caution_lamp(154,CautionLights.CPT_LTS_LI)
            AddFunction(new FlagValue(this, "156", "Caution Indicators", "LO", ""));            // create_caution_lamp(156,CautionLights.CPT_LTS_LO)
            AddFunction(new FlagValue(this, "158", "Caution Indicators", "RI", ""));            // create_caution_lamp(158,CautionLights.CPT_LTS_RI)
            AddFunction(new FlagValue(this, "160", "Caution Indicators", "RO", ""));            // create_caution_lamp(160,CautionLights.CPT_LTS_RO)
            AddFunction(new FlagValue(this, "166", "Caution Indicators", "NOSE_GEAR", ""));     // create_caution_lamp(166,CautionLights.CPT_LTS_NOSE_GEAR)
            AddFunction(new FlagValue(this, "165", "Caution Indicators", "LEFT_GEAR", ""));     // create_caution_lamp(165,CautionLights.CPT_LTS_LEFT_GEAR)
            AddFunction(new FlagValue(this, "167", "Caution Indicators", "RIGHT_GEAR", ""));    // create_caution_lamp(167,CautionLights.CPT_LTS_RIGHT_GEAR)
            AddFunction(new FlagValue(this, "163", "Caution Indicators", "HALF_FLAPS", ""));    // create_caution_lamp(163,CautionLights.CPT_LTS_HALF_FLAPS)
            AddFunction(new FlagValue(this, "164", "Caution Indicators", "FULL_FLAPS", ""));    // create_caution_lamp(164,CautionLights.CPT_LTS_FULL_FLAPS)
            AddFunction(new FlagValue(this, "162", "Caution Indicators", "FLAPS", ""));         // create_caution_lamp(162,CautionLights.CPT_LTS_FLAPS)
            // Lock/Shoot Light Assy
            AddFunction(new FlagValue(this, "1", "Caution Indicators", "LOCK", ""));            // create_caution_lamp(1,CautionLights.CPT_LTS_LOCK)
            AddFunction(new FlagValue(this, "2", "Caution Indicators", "SHOOT", ""));           // create_caution_lamp(2,CautionLights.CPT_LTS_SHOOT)
            AddFunction(new FlagValue(this, "3", "Caution Indicators", "SHOOT_STROBE", ""));    // create_caution_lamp(3,CautionLights.CPT_LTS_SHOOT_STROBE)
            // Master Arm Control
            AddFunction(new FlagValue(this, "47", "Caution Indicators", "AA", ""));             // create_caution_lamp(47,CautionLights.CPT_LTS_AA)
            AddFunction(new FlagValue(this, "48", "Caution Indicators", "AG", ""));             // create_caution_lamp(48,CautionLights.CPT_LTS_AG)
            AddFunction(new FlagValue(this, "45", "Caution Indicators", "DISCH", ""));          // create_caution_lamp(45,CautionLights.CPT_LTS_DISCH)
            AddFunction(new FlagValue(this, "44", "Caution Indicators", "READY", ""));          // create_caution_lamp(44,CautionLights.CPT_LTS_READY)
            // Arresting Hook Control Handle
            AddFunction(new FlagValue(this, "294", "Caution Indicators", "HOOK", ""));          // create_caution_lamp(294,CautionLights.CPT_LTS_HOOK)
            // Landing Gear 
                //create_caution_lamp(CautionLights.CPT_LTS_LDG_GEAR_HANDLE)
            // APU Control Panel
            AddFunction(new FlagValue(this, "376", "Caution Indicators", "APU_READY", ""));    // create_caution_lamp(376,CautionLights.CPT_LTS_APU_READY)
            // ECM Control Panel Assy
                       //create_caution_lamp(CautionLights.CPT_LTS_SEL)
            // Map Gain Control Panel Assy
            AddFunction(new FlagValue(this, "137", "Caution Indicators", "SPN", ""));           // create_caution_lamp(137,CautionLights.CPT_LTS_SPN)
            // Height Indicator
            AddFunction(new FlagValue(this, "290", "Caution Indicators", "LOW_ALT_WARN", ""));  // create_caution_lamp(290,CautionLights.CPT_LTS_LOW_ALT_WARN)
            // AoA Indexer Lights
            AddFunction(new FlagValue(this, "4", "Caution Indicators", "AOA_HIGH", ""));        // create_caution_lamp(4,CautionLights.CPT_LTS_AOA_HIGH)
            AddFunction(new FlagValue(this, "5", "Caution Indicators", "AOA_CENTER", ""));      // create_caution_lamp(5,CautionLights.CPT_LTS_AOA_CENTER)
            AddFunction(new FlagValue(this, "6", "Caution Indicators", "AOA_LOW", ""));         // create_caution_lamp(6,CautionLights.CPT_LTS_AOA_LOW)
            #region Declarations for Caution Lights numbers from Lamps.lua
            // Caution Light Indicator Panel
            // CPT_LTS_CK_SEAT = 0
            // CPT_LTS_APU_ACC = 1
            // CPT_LTS_BATT_SW = 2
            // CPT_LTS_FCS_HOT = 3
            // CPT_LTS_GEN_TIE = 4
            // CPT_LTS_SPARE_CTN1 = 5
            // CPT_LTS_FUEL_LO = 6
            // CPT_LTS_FCES = 7
            // CPT_LTS_SPARE_CTN2 = 8
            // CPT_LTS_L_GEN = 9
            // CPT_LTS_R_GEN = 10
            // CPT_LTS_SPARE_CTN3 = 11
            // LH Advisory and Threat Warning Indicator Panel
            // CPT_LTS_MASTER_CAUTION = 12
            // CPT_LTS_FIRE_LEFT = 13
            // CPT_LTS_GO = 14
            // CPT_LTS_NO_GO = 15
            // CPT_LTS_L_BLEED = 16
            // CPT_LTS_R_BLEED = 17
            // CPT_LTS_SPD_BRK = 18
            // CPT_LTS_STBY = 19
            // CPT_LTS_L_BAR_RED = 20
            // CPT_LTS_REC = 21
            // CPT_LTS_L_BAR_GREEN = 22
            // CPT_LTS_XMIT = 23
            // CPT_LTS_ASPJ_OH = 24
            // RH Advisory and Threat Warning Indicator Panel
            // CPT_LTS_FIRE_APU = 25
            // CPT_LTS_FIRE_RIGHT = 26
            // CPT_LTS_RCDR_ON = 27
            // CPT_LTS_DISP = 28
            // CPT_LTS_SAM = 29
            // CPT_LTS_AI = 30
            // CPT_LTS_AAA = 31
            // CPT_LTS_CW = 32
            // CPT_LTS_SPARE_RH1 = 33
            // CPT_LTS_SPARE_RH2 = 34
            // CPT_LTS_SPARE_RH3 = 35
            // CPT_LTS_SPARE_RH4 = 36
            // CPT_LTS_SPARE_RH5 = 37
            // Flaps Landing Gear and Stores Indicator Panel
            // CPT_LTS_CTR = 38
            // CPT_LTS_LI = 39
            // CPT_LTS_LO = 40
            // CPT_LTS_RI = 41
            // CPT_LTS_RO = 42
            // CPT_LTS_NOSE_GEAR = 43
            // CPT_LTS_LEFT_GEAR = 44
            // CPT_LTS_RIGHT_GEAR = 45
            // CPT_LTS_HALF_FLAPS = 46
            // CPT_LTS_FULL_FLAPS = 47
            // CPT_LTS_FLAPS = 48
            // Lock/Shoot Light Assy
            // CPT_LTS_LOCK = 49
            // CPT_LTS_SHOOT = 50
            // CPT_LTS_SHOOT_STROBE = 51
            // Master Arm Control
            // CPT_LTS_AA = 52
            // CPT_LTS_AG = 53
            // CPT_LTS_DISCH = 54
            // CPT_LTS_READY = 55
            // Arresting Hook Control Handle
            // CPT_LTS_HOOK = 56
            // Landing Gear 
            // CPT_LTS_LDG_GEAR_HANDLE = 57
            // APU Control Panel
            // CPT_LTS_APU_READY = 58
            // ECM Control Panel Assy
            // CPT_LTS_SEL = 59
            // Map Gain Control Panel Assy
            // CPT_LTS_SPN = 60
            // Height Indicator
            // CPT_LTS_LOW_ALT_WARN = 61
            // AMAC Control
            // CautionLightsNumber = 62
            // CPT_LTS_AOA_HIGH = 63
            // CPT_LTS_AOA_CENTER = 64
            // CPT_LTS_AOA_LOW = 65
            #endregion
            #endregion

            #region Internal Lights
            AddFunction(new FlagValue(this, "460", "Internal Lights", "Console", ""));    //  create_int_lights(460 InternalLights.Console_lt)
            AddFunction(new FlagValue(this, "461", "Internal Lights", "Flood", ""));    //  create_int_lights(461 InternalLights.Flood_lt)
            AddFunction(new FlagValue(this, "462", "Internal Lights", "NVG Flood", ""));    //  create_int_lights(462 InternalLights.NvgFlood_lt)
            AddFunction(new FlagValue(this, "464", "Internal Lights", "Emergancy Instruments", ""));    //  create_int_lights(464 InternalLights.EmerInstr_lt)
            AddFunction(new FlagValue(this, "465", "Internal Lights", "Engine Instrument Flood", ""));    //  create_int_lights(465 InternalLights.EngInstFlood_lt)
            AddFunction(new FlagValue(this, "466", "Internal Lights", "Instrument", ""));    //  create_int_lights(466 InternalLights.Instrument_lt)
            AddFunction(new FlagValue(this, "467", "Internal Lights", "Standby Compass", ""));    //  create_int_lights(467 InternalLights.StbyCompass_lt)
            AddFunction(new FlagValue(this, "810", "Internal Lights", "Utility", ""));    //  create_int_lights(810 InternalLights.Utility_lt)
            AddFunction(new FlagValue(this, "463", "Internal Lights", "Chart", ""));    //  create_int_lights(463 InternalLights.Chart_lt)

            #endregion

            #region IFEI
            AddFunction(new FlagValue(this, "468", "IFEI", "IFEI", ""));    //   IFEI_lt, create_simple_lamp(468 controllers.IfeiLights 0)
            AddFunction(new FlagValue(this, "469", "IFEI", "IFEI buttons", ""));    //   IFEI buttons_lt, create_simple_lamp(469 controllers.IfeiLights 1)

            #endregion

            #region RWR
            AddFunction(new FlagValue(this, "273", "RWR", "Limit", ""));    //   Limit_lt, create_simple_lamp(273 controllers.RWR_LimitLt)
            AddFunction(new FlagValue(this, "274", "RWR", "Display", ""));    //   Display_lt, create_simple_lamp(274 controllers.RWR_LowerLt)
            AddFunction(new FlagValue(this, "270", "RWR", "Special Enable", ""));    //   Special Enable, create_simple_lamp(270 controllers.RWR_LowerLt)
            AddFunction(new FlagValue(this, "271", "RWR", "Special", ""));    //   Special_lt, create_simple_lamp(271 controllers.RWR_LowerLt)
            AddFunction(new FlagValue(this, "267", "RWR", "Enable", ""));    //   Enable_lt, create_simple_lamp(267 controllers.RWR_EnableLt)
            AddFunction(new FlagValue(this, "268", "RWR", "Offset", ""));    //   Offset_lt, create_simple_lamp(268 controllers.RWR_LowerLt)
            AddFunction(new FlagValue(this, "264", "RWR", "Fail", ""));    //   Fail_lt, create_simple_lamp(264 controllers.RWR_FailLt)
            AddFunction(new FlagValue(this, "265", "RWR", "BIT", ""));    //   BIT_lt, create_simple_lamp(265 controllers.RWR_LowerLt)
            AddFunction(new FlagValue(this, "520", "RWR", "RWR Lights Brightness ", ""));    //   RWR Lights Brightness , create_simple_lamp(520 controllers.RWR_LowerLt)

            #endregion

            #region CMDS
            AddFunction(new FlagValue(this, "516", "CMDS", "ECM Jettison", ""));    //   ECM Jettison, create_simple_lamp(516 controllers.CMDS_JettLt)

            #endregion

        }

        private string DCSPath
        {
            get
            {
                if (_dcsPath == null)
                {
                    RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS FA-18C");
                    if (pathKey != null)
                    {
                        _dcsPath = (string)pathKey.GetValue("Path");
                        pathKey.Close();
                        ConfigManager.LogManager.LogDebug("DCS FA-18C Interface Editor - Found DCS Path (Path=\"" + _dcsPath + "\")");
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

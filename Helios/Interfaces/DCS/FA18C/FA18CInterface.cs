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

            AddFunction(new PushButton(this, MDI_LEFT, "3054", "3054", "Left MDI", "OSB01", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3055", "3055", "Left MDI", "OSB02", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3056", "3056", "Left MDI", "OSB03", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3057", "3057", "Left MDI", "OSB04", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3058", "3058", "Left MDI", "OSB05", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3059", "3059", "Left MDI", "OSB06", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3060", "3060", "Left MDI", "OSB07", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3061", "3061", "Left MDI", "OSB08", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3062", "3062", "Left MDI", "OSB09", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3063", "3063", "Left MDI", "OSB10", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3064", "3064", "Left MDI", "OSB11", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3065", "3065", "Left MDI", "OSB12", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3066", "3066", "Left MDI", "OSB13", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3067", "3067", "Left MDI", "OSB14", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3068", "3068", "Left MDI", "OSB15", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3069", "3069", "Left MDI", "OSB16", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3070", "3070", "Left MDI", "OSB17", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3071", "3071", "Left MDI", "OSB18", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3072", "3072", "Left MDI", "OSB19", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MDI_LEFT, "3073", "3073", "Left MDI", "OSB20", "1", "0", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_LEFT, "3051", "3051", "-1", "Off", "0", "Night", "1", "Day", "Left MDI", "Left MDI Brightness Selector Knob, OFF/NIGHT/DAY", "%1d"));
            AddFunction(new Axis(this, MDI_LEFT, "3052", "3052", 0.1d, 0d, 1d, "Left MDI", "Brightness Control Knob"));
            AddFunction(new Axis(this, MDI_LEFT, "3053", "3053", 0.1d, 0d, 1d, "Left MDI", "Contrast Control Knob"));
            #endregion

            #region Centre MFCD AMPCD
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_LEFT, "3312", "3312", "-1", "Left", "0", "Centre", "1", "Right", "AMPCD", "Heading Set Switch", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_LEFT, "3313", "3313", "-1", "Left", "0", "Centre", "1", "Right", "AMPCD", "Course Set Switch", "%1d"));

            AddFunction(new PushButton(this, AMPCD, "3183", "3183", "AMPCD", "OSB01"));
            AddFunction(new PushButton(this, AMPCD, "3184", "3184", "AMPCD", "OSB02"));
            AddFunction(new PushButton(this, AMPCD, "3185", "3185", "AMPCD", "OSB03"));
            AddFunction(new PushButton(this, AMPCD, "3186", "3186", "AMPCD", "OSB04"));
            AddFunction(new PushButton(this, AMPCD, "3187", "3187", "AMPCD", "OSB05"));
            AddFunction(new PushButton(this, AMPCD, "3188", "3188", "AMPCD", "OSB06"));
            AddFunction(new PushButton(this, AMPCD, "3189", "3189", "AMPCD", "OSB07"));
            AddFunction(new PushButton(this, AMPCD, "3190", "3190", "AMPCD", "OSB08"));
            AddFunction(new PushButton(this, AMPCD, "3191", "3191", "AMPCD", "OSB09"));
            AddFunction(new PushButton(this, AMPCD, "3192", "3192", "AMPCD", "OSB10"));
            AddFunction(new PushButton(this, AMPCD, "3193", "3193", "AMPCD", "OSB11"));
            AddFunction(new PushButton(this, AMPCD, "3194", "3194", "AMPCD", "OSB12"));
            AddFunction(new PushButton(this, AMPCD, "3195", "3195", "AMPCD", "OSB13"));
            AddFunction(new PushButton(this, AMPCD, "3196", "3196", "AMPCD", "OSB14"));
            AddFunction(new PushButton(this, AMPCD, "3197", "3197", "AMPCD", "OSB15"));
            AddFunction(new PushButton(this, AMPCD, "3198", "3198", "AMPCD", "OSB16"));
            AddFunction(new PushButton(this, AMPCD, "3199", "3199", "AMPCD", "OSB17"));
            AddFunction(new PushButton(this, AMPCD, "3200", "3200", "AMPCD", "OSB18"));
            AddFunction(new PushButton(this, AMPCD, "3201", "3201", "AMPCD", "OSB19"));
            AddFunction(new PushButton(this, AMPCD, "3202", "3202", "AMPCD", "OSB20"));
            AddFunction(new Axis(this, AMPCD, "3203", "3203", 0.1d, 0d, 1d, "AMPCD", "Off/Brightness Control"));
            AddFunction(Switch.CreateThreeWaySwitch(this, AMPCD, "3177", "3177", "-1", "Day", "0", "Off", "1", "Night", "AMPCD", "DAY/NIGHT Mode", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, AMPCD, "3179", "3179", "-1", "More", "0", "Off", "1", "Less", "AMPCD", "Symbology", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, AMPCD, "3180", "3180", "-1", "Up", "0", "Off", "1", "Down", "AMPCD", "Gain", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, AMPCD, "3182", "3182", "-1", "Up", "0", "Off", "1", "Down", "AMPCD", "Contrast", "%1d"));
            #endregion

            #region Right MFCD MFD

            AddFunction(new PushButton(this, MDI_RIGHT, "3079", "3079", "Right MDI", "OSB01"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3080", "3080", "Right MDI", "OSB02"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3081", "3081", "Right MDI", "OSB03"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3082", "3082", "Right MDI", "OSB04"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3083", "3083", "Right MDI", "OSB05"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3084", "3084", "Right MDI", "OSB06"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3085", "3085", "Right MDI", "OSB07"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3086", "3086", "Right MDI", "OSB08"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3087", "3087", "Right MDI", "OSB09"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3088", "3088", "Right MDI", "OSB10"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3089", "3089", "Right MDI", "OSB11"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3090", "3090", "Right MDI", "OSB12"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3091", "3091", "Right MDI", "OSB13"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3092", "3092", "Right MDI", "OSB14"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3093", "3093", "Right MDI", "OSB15"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3094", "3094", "Right MDI", "OSB16"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3095", "3095", "Right MDI", "OSB17"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3096", "3096", "Right MDI", "OSB18"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3097", "3097", "Right MDI", "OSB19"));
            AddFunction(new PushButton(this, MDI_RIGHT, "3098", "3098", "Right MDI", "OSB20"));
            AddFunction(Switch.CreateThreeWaySwitch(this, MDI_RIGHT, "3076", "3076", "-1", "Off", "0", "Night", "1", "Day", "Right MDI", "Right MDI Brightness Selector Knob, OFF/NIGHT/DAY", "%1d"));
            AddFunction(new Axis(this, MDI_RIGHT, "3077", "3077", 0.1d, 0d, 1d, "Right MDI", "Brightness Control Knob"));
            AddFunction(new Axis(this, MDI_RIGHT, "3078", "3078", 0.1d, 0d, 1d, "Right MDI", "Contrast Control Knob"));
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
            AddFunction(new FlagValue(this, "468", "IFEI", "IFEI", ""));    //   IFEI_lt, create_simple_lamp(468 controllers.IFEILights 0)
            AddFunction(new FlagValue(this, "469", "IFEI", "IFEI buttons", ""));    //   IFEI buttons_lt, create_simple_lamp(469 controllers.IFEILights 1)

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
            #region Control System
            AddFunction(new Axis(this, CONTROL_INTERFACE, "3345", "3345", 0.1d, 0d, 1d, "Controls", "RUD TRIM Control"));
            AddFunction(new PushButton(this, CONTROL_INTERFACE, "3346", "3346", "Controls", "T/O TRIM Button"));
            AddFunction(new PushButton(this, CONTROL_INTERFACE, "3349", "3349", "Controls", "FCS RESET Button"));
            AddFunction(Switch.CreateToggleSwitch(this, CONTROL_INTERFACE, "3347", "3347", "1", "NORM", "0", "ORIDE", "Controls", "GAIN Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, CONTROL_INTERFACE, "3234", "3234", "1", "AUTO", "0", "HALF", "-1", "FULL", "Controls", "FLAP Switch", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, CONTROL_INTERFACE, "3138", "3138", "1", "RCVY", "0", "NORM", "Controls", "Spin Recovery Switch", "%0.1f"));
            AddFunction(new PushButton(this, CONTROL_INTERFACE, "3470", "3470", "Controls", "FCS BIT Switch"));
            AddFunction(new Axis(this, CONTROL_INTERFACE, "3504", "3504", 0.1d, 0d, 1d, "Controls", "Throttles Friction Adjusting Lever"));
            #endregion
            #region Electric system
            AddFunction(Switch.CreateThreeWaySwitch(this, ELEC_INTERFACE, "3404", "3404", "1", "ON", "0", "OFF", "-1", "ORIDE", "Electrical", "Battery Switch", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3402", "3402", "1", "NORM", "0", "OFF", "Electrical", "Left Generator Control Switch", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3403", "3403", "1", "NORM", "0", "OFF", "Electrical", "Right Generator Control Switch", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3378", "3378", "1", "NORM", "0", "RESET", "Electrical", "Generator TIE Control Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ELEC_INTERFACE, "3336", "3336", "1", "RESET", "0", "NORM", "-1", "OFF", "Electrical", "External Power Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ELEC_INTERFACE, "3332", "3332", "1", "A ON", "0", "AUTO", "-1", "B ON", "Electrical", "Ground Power Switch 1", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ELEC_INTERFACE, "3333", "3333", "1", "A ON", "0", "AUTO", "-1", "B ON", "Electrical", "Ground Power Switch 2", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ELEC_INTERFACE, "3334", "3334", "1", "A ON", "0", "AUTO", "-1", "B ON", "Electrical", "Ground Power Switch 3", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ELEC_INTERFACE, "3335", "3335", "1", "A ON", "0", "AUTO", "-1", "B ON", "Electrical", "Ground Power Switch 4", "%0.1f"));
            #endregion
            #region Anti-Ice
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3409", "3409", "1", "ON", "0", "AUTO", "Electrical", "Pitot Heater Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ENGINES_INTERFACE, "3410", "3410", "1", "ON", "0", "OFF", "-1", "TEST", "Engine", "Engine Anti-Ice Switch", "%0.1f"));
            #endregion
            #region CB
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3381", "3381", "1", "ON", "0", "OFF", "Electrical", "CB FCS CHAN 1", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3382", "3382", "1", "ON", "0", "OFF", "Electrical", "CB FCS CHAN 2", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3383", "3383", "1", "ON", "0", "OFF", "Electrical", "CB SPD BRK", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3384", "3384", "1", "ON", "0", "OFF", "Electrical", "CB LAUNCH BAR", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3454", "3454", "1", "ON", "0", "OFF", "Electrical", "CB FCS CHAN 3", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3455", "3455", "1", "ON", "0", "OFF", "Electrical", "CB FCS CHAN 4", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3456", "3456", "1", "ON", "0", "OFF", "Electrical", "CB HOOK", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ELEC_INTERFACE, "3457", "3457", "1", "ON", "0", "OFF", "Electrical", "CB LG", "%0.1f"));
            #endregion
            #region 
            AddFunction(Switch.CreateThreeWaySwitch(this, ELEC_INTERFACE, "3368", "3368", "1", "1 OFF", "0", "NORM", "-1", "2 OFF", "Electrical", "MC Switch", "%0.1f"));
            #endregion
            #region Power Plant
            AddFunction(new PushButton(this, ENGINES_INTERFACE, "3375", "3375", "Engine", "APU Control Switch"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ENGINES_INTERFACE, "3377", "3377", "1", "LEFT", "0", "OFF", "-1", "RIGHT", "Engine", "Engine Crank Switch", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINES_INTERFACE, "3331", "3331", "1", "(RMB) TEST A", "0", "(LMB) TEST B", "Engine", "Fire and Bleed Air Test Switch", "%0.1f"));
            #endregion
            #region Hydraulic system
            AddFunction(Switch.CreateToggleSwitch(this, HYDRAULIC_INTERFACE, "3369", "3369", "1", "NORM", "0", "ORIDE", "Hydraulics", "Hydraulic Isolate Override Switch", "%0.1f"));
            #endregion
            #region Gear system
            AddFunction(Switch.CreateThreeWaySwitch(this, GEAR_INTERFACE, "3226", "3226", "1", "UP", "0", "DOWN", "-1", "(MW)EMERGENCY DOWN", "Gear", "Landing Gear Control Handle", "%0.1f"));
            AddFunction(new PushButton(this, GEAR_INTERFACE, "3229", "3229", "Gear", "Down Lock Override Button - Push to unlock"));
            AddFunction(Switch.CreateToggleSwitch(this, GEAR_INTERFACE, "3238", "3238", "1", "ON", "0", "OFF", "Gear", "Anti Skid Switch", "%0.1f"));
            AddFunction(new PushButton(this, GEAR_INTERFACE, "3233", "3233", "Gear", "Launch Bar Control Switch"));
            AddFunction(Switch.CreateToggleSwitch(this, GEAR_INTERFACE, "3293", "3293", "1", "UP", "0", "DOWN", "Gear", "Arresting Hook Handle", "%0.1f"));
            #endregion
            #region Fuel system
            AddFunction(Switch.CreateToggleSwitch(this, FUEL_INTERFACE, "3340", "3340", "1", "INHIBIT", "0", "NORM", "Fuel System", "Internal Wing Tank Fuel Control Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, FUEL_INTERFACE, "3341", "3341", "1", "EXTEND", "0", "RETRACT", "-1", "EMERG EXTD", "Fuel System", "Probe Control Switch", "%0.1f"));
            AddFunction(new PushButton(this, FUEL_INTERFACE, "3344", "3344", "Fuel System", "Fuel Dump Switch"));
            AddFunction(Switch.CreateThreeWaySwitch(this, FUEL_INTERFACE, "3343", "3343", "1", "STOP", "0", "NORM", "-1", "ORIDE", "Fuel System", "External Centerline Tank Fuel Control Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, FUEL_INTERFACE, "3342", "3342", "1", "STOP", "0", "NORM", "-1", "ORIDE", "Fuel System", "External Wing Tanks Fuel Control Switch", "%0.1f"));
            #endregion
            #region Cockpit Mechanics
            AddFunction(Switch.CreateThreeWaySwitch(this, CPT_MECHANICS, "3453", "3453", "1", "OPEN", "0", "HOLD", "-1", "CLOSE", "Cockpit", "Canopy Control Switch", "%0.1f"));
            AddFunction(new PushButton(this, CPT_MECHANICS, "3043", "3043", "Cockpit", "Canopy Jettison Handle Unlock Button - Press to unlock"));
            AddFunction(new PushButton(this, CPT_MECHANICS, "3042", "3042", "Cockpit", "Canopy Jettison Handle - Pull to jettison"));
            AddFunction(new PushButton(this, CPT_MECHANICS, "3510", "3510", "Cockpit", "Ejection Control Handle (3 times)"));
            AddFunction(Switch.CreateToggleSwitch(this, CPT_MECHANICS, "3511", "3511", "1", "SAFE", "0", "ARMED", "Cockpit", "Ejection Seat SAFE/ARMED Handle", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, CPT_MECHANICS, "3512", "3512", "1", "PULL", "0", "PUSH", "Cockpit", "Ejection Seat Manual Override Handle", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, CPT_MECHANICS, "3513", "3513", "1", "LOCK", "0", "UNLOCK", "Cockpit", "Shoulder Harness Control Handle", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, CPT_MECHANICS, "3514", "3514", "1", "UP", "0", "HOLD", "-1", "DOWN", "Cockpit", "Seat Height Adjustment Switch", "%0.1f"));
            AddFunction(new PushButton(this, CPT_MECHANICS, "3260", "3260", "Cockpit", "Rudder Pedal Adjust Lever"));
            AddFunction(new PushButton(this, CPT_MECHANICS, "3575", "3575", "Cockpit", "Hide Stick toggle"));
            #endregion
            #region Mirrors
            //            --Mirrors
            //497 = 2_position_tumb | Toggle Mirrors, 0, 3002, 0)
            //498 = 2_position_tumb | Toggle Mirrors, 0, 3003, 0)
            //499 = 2_position_tumb | Toggle Mirrors, 0, 3004, 0)

            //AddFunction(Switch.CreateToggleSwitch(this, 3002, "3497", "3497", "3002", "Toggle Mirrors", "%0.1f"));
            //AddFunction(Switch.CreateToggleSwitch(this, 3003, "3498", "3498", "3003", "Toggle Mirrors", "%0.1f"));
            //AddFunction(Switch.CreateToggleSwitch(this, 3004, "3499", "3499", "3004", "Toggle Mirrors", "%0.1f"));
            #endregion
            #region Exterior Lights
            AddFunction(new Axis(this, EXT_LIGHTS, "3338", "3338", 0.15d, 0d, 1d, "External Lights", "POSITION Lights Dimmer Control"));
            AddFunction(new Axis(this, EXT_LIGHTS, "3337", "3337", 0.15d, 0d, 1d, "External Lights", "FORMATION Lights Dimmer Control"));
            AddFunction(Switch.CreateThreeWaySwitch(this, EXT_LIGHTS, "3339", "3339", "1", "BRT", "0", "OFF", "-1", "DIM", "External Lights", "STROBE Lights Switch", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, EXT_LIGHTS, "3237", "3237", "1", "ON", "0", "OFF", "External Lights", "LDG/TAXI LIGHT Switch", "%0.1f"));
            #endregion
            #region Cockpit Lights
            AddFunction(new Axis(this, CPT_LIGTHS, "3413", "3413", 0.15d, 0d, 1d, "Cockpit Lights", "CONSOLES Lights Dimmer Control"));
            AddFunction(new Axis(this, CPT_LIGTHS, "3414", "3414", 0.15d, 0d, 1d, "Cockpit Lights", "INST PNL Dimmer Control"));
            AddFunction(new Axis(this, CPT_LIGTHS, "3415", "3415", 0.15d, 0d, 1d, "Cockpit Lights", "FLOOD Light Dimmer Control"));
            AddFunction(Switch.CreateThreeWaySwitch(this, CPT_LIGTHS, "3419", "3419", "1", "NVG", "0", "NITE", "-1", "DAY", "Cockpit Lights", "MODE Switch", "%0.1f"));
            AddFunction(new Axis(this, CPT_LIGTHS, "3418", "3418", 0.15d, 0d, 1d, "Cockpit Lights", "CHART Light Dimmer Control"));
            AddFunction(new Axis(this, CPT_LIGTHS, "3417", "3417", 0.15d, 0d, 1d, "Cockpit Lights", "WARN/CAUTION Dimmer Control"));
            AddFunction(Switch.CreateToggleSwitch(this, CPT_LIGTHS, "3416", "3416", "1", "TEST", "0", "OFF", "Cockpit Lights", "Lights Test Switch", "%0.1f"));
            AddFunction(new PushButton(this, CPT_LIGTHS, "3014", "3014", "Cockpit Lights", "MASTER CAUTION Reset Button - Press to reset"));
            AddFunction(Switch.CreateToggleSwitch(this, CPT_LIGTHS, "3239", "3239", "1", "FIELD", "0", "CARRIER", "Cockpit Lights", "HOOK BYPASS Switch", "%0.1f"));
            #endregion
            #region Oxygen System
            AddFunction(Switch.CreateToggleSwitch(this, OXYGEN_INTERFACE, "3365", "3365", "1", "ON", "0", "OFF", "Oxygen System", "OBOGS Control Switch", "%0.1f"));
            AddFunction(new Axis(this, OXYGEN_INTERFACE, "3366", "3366", 0.5d, 0d, 1d, "Oxygen System", "OXY Flow Knob"));
            #endregion
            #region ECS
            AddFunction(new Switch(this, ECS_INTERFACE, "3411", new SwitchPosition[] { new SwitchPosition("1.0", "R OFF", "3411"), new SwitchPosition("0.0", "Norm", "3411"), new SwitchPosition("-1.0", "L Off", "3411"), new SwitchPosition("-2.0", "Off", "3411") }, "ECS", "Bleed Air Knob", "%0.1f"));
            AddFunction(new PushButton(this, ECS_INTERFACE, "3412", "3412", "ECS", "Bleed Air Knob (Pull)"));
            AddFunction(new Switch(this, ECS_INTERFACE, "3405", new SwitchPosition[] { new SwitchPosition("1.0", "Auto", "3405"), new SwitchPosition("0.0", "Man", "3405"), new SwitchPosition("-1.0", "Off", "3405"), new SwitchPosition("-2.0", "Ram", "3405") }, "ECS", "ECS Mode Switch", "%0.1f"));
            AddFunction(new Switch(this, ECS_INTERFACE, "3408", new SwitchPosition[] { new SwitchPosition("1.0", "Norm", "3408"), new SwitchPosition("0.0", "Dump", "3408"), new SwitchPosition("-1.0", "Ram", "3408"), new SwitchPosition("-2.0", "Dump", "3408") }, "ECS", "Cabin Pressure Switch", "%0.1f"));
            AddFunction(new Axis(this, ECS_INTERFACE, "3451", "3451", 0.1d, 0d, 1d, "ECS", "Defog Handle"));
            AddFunction(new Axis(this, ECS_INTERFACE, "3407", "3407", 0.1d, 0d, 1d, "ECS", "Cabin Temperature Knob"));
            AddFunction(new Axis(this, ECS_INTERFACE, "3406", "3406", 0.1d, 0d, 1d, "ECS", "Suit Temperature Knob"));
            AddFunction(Switch.CreateToggleSwitch(this, ECS_INTERFACE, "3297", "3297", "1", "NORM", "0", "EMERG", "ECS", "AV COOL Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ECS_INTERFACE, "3452", "3452", "1", "ANTI ICE", "0", "OFF", "-1", "RAIN", "ECS", "Windshield Anti-Ice/Rain Switch", "%0.1f"));
            AddFunction(new Axis(this, ECS_INTERFACE, "3505", "3505", 0.1d, 0d, 1d, "ECS", "Left Louver"));
            AddFunction(new Axis(this, ECS_INTERFACE, "3506", "3506", 0.1d, 0d, 1d, "ECS", "Right Louver"));
            #endregion
            #region HOTAS
            AddFunction(Switch.CreateToggleSwitch(this, HOTAS, "3494", "3494", "1", "ON", "0", "OFF", "HOTAS", "Exterior Lights Switch", "%0.1f"));
            #endregion
            #region Master Arm Panel
            AddFunction(new PushButton(this, SMS, "3458", "3458","SMS", "A/A Master Mode Button"));
            AddFunction(new PushButton(this, SMS, "3459", "3459","SMS", "A/G Master Mode Button"));
            AddFunction(Switch.CreateToggleSwitch(this, SMS, "3049", "3049", "1", "ARM", "0", "SAFE","SMS", "Master Arm Switch", "%0.1f"));
            AddFunction(new PushButton(this, SMS, "3050", "3050","SMS", "Emergency Jettison Button"));
            AddFunction(Switch.CreateToggleSwitch(this, SMS, "3258", "3258", "1", "ENABLE", "0", "NORM","SMS", "Auxiliary Release Switch", "%0.1f"));
            AddFunction(new PushButton(this, SMS, "3153", "3153","SMS", "Station Jettison Centre Select Button"));
            AddFunction(new PushButton(this, SMS, "3155", "3155","SMS", "Station Jettison Left In Select Button"));
            AddFunction(new PushButton(this, SMS, "3157", "3157","SMS", "Station Jettison Left Out Select Button"));
            AddFunction(new PushButton(this, SMS, "3159", "3159","SMS", "Station Jettison Right In Select Button"));
            AddFunction(new PushButton(this, SMS, "3161", "3161","SMS", "Station Jettison Right Out Select Button"));
            AddFunction(new PushButton(this, SMS, "3235", "3235","SMS", "Selective Jettison Pushbutton"));
            AddFunction(new Switch(this, SMS, "3236", new SwitchPosition[] { new SwitchPosition("1.0", "L FUS MSL", "3236"), new SwitchPosition("0.0", "Safe", "3236"), new SwitchPosition("-1.0", "R FUS MSL", "3236"), new SwitchPosition("-2.0", "Rack", "3236"), new SwitchPosition("-3.0", "LCHR", "3236"), new SwitchPosition("-4.0", "Stores", "3236") }, "SMS", "Selective Jettison Knob", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SMS, "3135", "3135", "1", "ORIDE", "0", "NORM", "-1", "OFF","SMS", "IR Cooling Switch", "%0.1f"));
            #endregion
            #region Fire Systems
            AddFunction(new PushButton(this, ENGINES_INTERFACE, "3046", "3046", "Engine", "Fire Extinguisher Pushbutton"));
            AddFunction(new PushButton(this, ENGINES_INTERFACE, "3030", "3030", "Engine", "APU Fire Warning/Extinguisher Light"));
            #endregion
            #region Multipurpose Display Group -----------
            #endregion
            #region Head-Up Display
            AddFunction(Switch.CreateThreeWaySwitch(this, HUD, "3140", "3140", "1", "NORM", "0", "REJ 1", "-1", "REJ 2", "HUD", "HUD Symbology Reject Switch", "%0.1f"));
            AddFunction(new Axis(this, HUD, "3141", "3141", 0.1d, 0d, 1d, "HUD", "HUD Symbology Brightness Control Knob"));
            AddFunction(Switch.CreateToggleSwitch(this, HUD, "3142", "3142", "1", "DAY", "0", "NIGHT", "HUD", "HUD Symbology Brightness Selector Knob", "%0.1f"));
            AddFunction(new Axis(this, HUD, "3143", "3143", 0.1d, 0d, 1d, "HUD", "Black Level Control Knob"));
            AddFunction(new Switch(this, HUD, "3411", new SwitchPosition[] { new SwitchPosition("1.0", "W", "3411"), new SwitchPosition("0.0", "B", "3411"), new SwitchPosition("-1.0", "Vid", "3411"), new SwitchPosition("-2.0", "Off", "3411") }, "HUD", "HUD Video Control Switch", "%0.1f"));
            AddFunction(new Axis(this, HUD, "3145", "3145", 0.1d, 0d, 1d, "HUD", "Balance Control Knob"));
            AddFunction(new Axis(this, HUD, "3146", "3146", 0.1d, 0d, 1d, "HUD", "AOA Indexer Control Knob"));
            AddFunction(Switch.CreateToggleSwitch(this, HUD, "3147", "3147", "1", "BARO", "0", "RDR", "HUD", "Altitude Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, HUD, "3148", "3148", "1", "INS", "0", "AUTO", "-1", "STBY", "HUD", "Attitude Selector Switch", "%0.1f"));
            #endregion
             #region Instruments
            #endregion
            #region Standby Pressure Altimeter AAU-52/A
            AddFunction(new Axis(this, AAU52, "3224", "3224", 0.1d, 0.04d, 1d, "AAU52", "AAU-52 Altimeter Pressure Setting Knob"));
            #endregion
            #region Radar Altimeter Height Indicator
            #endregion
            #region tandby Attitude Indicator
            AddFunction(new PushButton(this, SAI, "3215", "3215", "SAI", "SAI Test Button - Push to test"));
            #endregion
            #region Integrated Fuel/Engine Indicator (IFEI)
            AddFunction(new PushButton(this, IFEI, "3168", "3168", "IFEI", "IFEI Mode Button"));
            AddFunction(new PushButton(this, IFEI, "3169", "3169", "IFEI", "IFEI QTY Button"));
            AddFunction(new PushButton(this, IFEI, "3170", "3170", "IFEI", "IFEI Up Arrow Button"));
            AddFunction(new PushButton(this, IFEI, "3171", "3171", "IFEI", "IFEI Down Arrow Button"));
            AddFunction(new PushButton(this, IFEI, "3172", "3172", "IFEI", "IFEI ZONE Button"));
            AddFunction(new PushButton(this, IFEI, "3173", "3173", "IFEI", "IFEI ET Button"));
            AddFunction(new Axis(this, IFEI, "3174", "3174", 0.1d, 0d, 1d, "IFEI", "IFEI Brightness Control Knob"));
            #endregion
            #region Sensor panel
            AddFunction(new Switch(this, RADAR, "3440", new SwitchPosition[] { new SwitchPosition("1.0", "Off", "3440"), new SwitchPosition("0.0", "Standby", "3440"), new SwitchPosition("-1.0", "Opr", "3440"), new SwitchPosition("-2.0", "EMERG(PULL)", "3440")}, "RADAR", "RADAR Switch (MW to pull)", "%0.1f"));

            #endregion
            #region OLD
            AddFunction(new Switch(this, INS, "3443", new SwitchPosition[] { new SwitchPosition("1.0", "Off", "3443"), new SwitchPosition("0.0", "CV", "3443"), new SwitchPosition("-1.0", "Gnd", "3443"), new SwitchPosition("-2.0", "Nav", "3443"), new SwitchPosition("-3.0", "IFA", "3443"), new SwitchPosition("-4.0", "Gyro", "3443"), new SwitchPosition("-5.0", "GB", "3443"), new SwitchPosition("-6.0", "Test", "3443") }, "INS", "INS Mode Switch", "%0.1f"));

            #endregion
            #region intercom
            AddFunction(new Axis(this, INTERCOM, "3357", "3357", 0.1d, 0d, 1d, "Intercom", "VOX Volume Control Knob"));
            AddFunction(new Axis(this, INTERCOM, "3358", "3358", 0.1d, 0d, 1d, "Intercom", "ICS Volume Control Knob"));
            AddFunction(new Axis(this, INTERCOM, "3359", "3359", 0.1d, 0d, 1d, "Intercom", "RWR Volume Control Knob"));
            AddFunction(new Axis(this, INTERCOM, "3360", "3360", 0.1d, 0d, 1d, "Intercom", "WPN Volume Control Knob"));
            AddFunction(new Axis(this, INTERCOM, "3361", "3361", 0.1d, 0d, 1d, "Intercom", "MIDS B Volume Control Knob"));
            AddFunction(new Axis(this, INTERCOM, "3362", "3362", 0.1d, 0d, 1d, "Intercom", "MIDS A Volume Control Knob"));
            AddFunction(new Axis(this, INTERCOM, "3363", "3363", 0.1d, 0d, 1d, "Intercom", "TACAN Volume Control Knob"));
            AddFunction(new Axis(this, INTERCOM, "3364", "3364", 0.1d, 0d, 1d, "Intercom", "AUX Volume Control Knob"));
            AddFunction(Switch.CreateThreeWaySwitch(this, INTERCOM, "3350", "3350", "1", "CIPHER", "0", "OFF", "-1", "PLAIN", "Intercom", "Comm Relay Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, INTERCOM, "3351", "3351", "1", "COMM 1", "0", "OFF", "-1", "COMM 2", "Intercom", "COMM G XMT Switch", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, INTERCOM, "3356", "3356", "1", "EMER", "0", "NORM", "Intercom", "IFF Master Switch", "%0.1f"));
            AddFunction(new Switch(this, INTERCOM, "3355", new SwitchPosition[] { new SwitchPosition("1.0", "Dis", "3355"), new SwitchPosition("0.0", "Aud", "3355"), new SwitchPosition("-1.0", "Dis", "3355"), new SwitchPosition("-2.0", "Off", "3355") }, "Intercom", "IFF Mode 4 Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, INTERCOM, "3354", "3354", "1", "HOLD", "0", "NORM", "-1", "ZERO", "Intercom", "CRYPTO Switch", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, INTERCOM, "3353", "3353", "1", "UFC", "0", "MAN", "Intercom", "ILS UFC/MAN Switch", "%0.1f"));
            AddFunction(new Axis(this, INTERCOM, "3352", "3352", 0.05d, 0d, 1d, "Intercom", "ILS Channel Selector Switch"));
            AddFunction(new Switch(this, KY58, "3444", new SwitchPosition[] { new SwitchPosition("1.0", "P", "3444"), new SwitchPosition("0.0", "C", "3444"), new SwitchPosition("-1.0", "LD", "3444"), new SwitchPosition("-2.0", "HV", "3444") }, "KY58", "KY-58 Mode Select Knob", "%0.1f"));
            AddFunction(new Axis(this, KY58, "3445", "3445", 0.1d, 0d, 1d, "KY58", "KY-58 Volume Control Knob"));
            AddFunction(new Switch(this, KY58, "3446", new SwitchPosition[] { new SwitchPosition("1.0", "Z 1-5", "3446"), new SwitchPosition("0.0", "1", "3446"), new SwitchPosition("-1.0", "2", "3446"), new SwitchPosition("-2.0", "3", "3446"), new SwitchPosition("-3.0", "4", "3446"), new SwitchPosition("-4.0", "5", "3446"), new SwitchPosition("-5.0", "6", "3446"), new SwitchPosition("-6.0", "Z All", "3446") }, "KY58", "KY-58 Fill Select Knob", "%0.1f"));
            AddFunction(new Switch(this, KY58, "3447", new SwitchPosition[] { new SwitchPosition("1.0", "Off", "3447"), new SwitchPosition("0.0", "On", "3447"), new SwitchPosition("-1.0", "TD", "3447") }, "KY58", "KY-58 Power Select Knob", "%0.1f"));
            AddFunction(new PushButton(this, INTERCOM, "3230", "3230", "Intercom", "Warning Tone Silence Button - Push to silence"));
            #endregion
            #region antenna selector
            AddFunction(Switch.CreateThreeWaySwitch(this, ANTENNA_SELECTOR, "3373", "3373", "1", "UPPER", "0", "AUTO", "-1", "LOWER", "Antenna", "COMM 1 Antenna Selector Switch", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, ANTENNA_SELECTOR, "3374", "3374", "1", "UPPER", "0", "BOTH", "-1", "LOWER", "Antenna", "IFF Antenna Selector Switch", "%0.1f"));
            #endregion
            #region RWR
            AddFunction(new PushButton(this, RWR, "3277", "3277", "RWR", "ALR-67 POWER Pushbutton"));
            AddFunction(new PushButton(this, RWR, "3275", "3275", "RWR", "ALR-67 DISPLAY Pushbutton"));
            AddFunction(new PushButton(this, RWR, "3272", "3272", "RWR", "ALR-67 SPECIAL Pushbutton"));
            AddFunction(new PushButton(this, RWR, "3269", "3269", "RWR", "ALR-67 OFFSET Pushbutton"));
            AddFunction(new PushButton(this, RWR, "3266", "3266", "RWR", "ALR-67 BIT Pushbutton"));
            AddFunction(new Axis(this, RWR, "3263", "3263", 0.1d, 0d, 1d, "RWR", "ALR-67 DMR Control Knob"));
            AddFunction(new Axis(this, RWR, "3262", "3262", 0.1d, 0d, 1d, "RWR", "ALR-67 AUDIO Control Knob (no function)"));
            AddFunction(new Switch(this, RWR, "3261", new SwitchPosition[] { new SwitchPosition("1.0", "N", "3261"), new SwitchPosition("0.0", "I", "3261"), new SwitchPosition("-1.0", "A", "3261"), new SwitchPosition("-2.0", "U", "3261"), new SwitchPosition("-3.0", "F", "3261") }, "RWR", "ALR-67 DIS TYPE Switch", "%0.1f"));
            AddFunction(new Axis(this, RWR, "3216", "3216", 0.1d, 0d, 1d, "RWR", "RWR Intensity Knob"));
            #endregion
            #region CMDS
            AddFunction(new PushButton(this, CMDS, "3380", "3380", "CMDS", "Dispense Button - Push to dispense flares and chaff"));
            AddFunction(Switch.CreateThreeWaySwitch(this, CMDS, "3517", "3517", "1", "BYPASS", "0", "ON", "-1", "OFF", "CMDS", "DISPENSER Switch", "%0.1f"));
            AddFunction(new PushButton(this, CMDS, "3515", "3515", "CMDS", "ECM - Push to jettison"));
            #endregion
            #region ICMCP
            AddFunction(new Switch(this, CMDS, "3248", new SwitchPosition[] { new SwitchPosition("1.0", "Xmit", "3248"), new SwitchPosition("0.0", "Rcv", "3248"), new SwitchPosition("-1.0", "BIT", "3248"), new SwitchPosition("-2.0", "Stby", "3248"), new SwitchPosition("-3.0", "Off", "3248") }, "CMDS", "ECM Mode Switch", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, CMDS, "3507", "3507", "1", "ENABLE", "0", "DISABLE (no function)", "CMDS", "NUC WPN Switch", "%0.1f"));
            #endregion
            #region TODO list
            //AddFunction(Switch.CreateThreeWaySwitch(this, TODO, "3175", "3175", "1", "HMD", "0", "LDDI", "-1", "RDDI", "Work In Progress", "Selector Switch", "%0.1f"));
            //AddFunction(Switch.CreateThreeWaySwitch(this, TODO, "3176", "3176", "1", "HUD", "0", "LDIR", "-1", "RDDI", "Work In Progress", "Selector Switch", "%0.1f"));
            //AddFunction(Switch.CreateThreeWaySwitch(this, TODO, "3314", "3314", "1", "MAN", "0", "OFF", "-1", "AUTO", "Work In Progress", "Mode Selector Switch", "%0.1f"));
            //AddFunction(new PushButton(this, TODO, "3007", "3007", "Work In Progress", "Video BIT Initiate Pushbutton - Push to initiate BIT"));
            //AddFunction(new Axis(this, TODO, "3136", "3136", 0.1d, 0d, 1d, "Work In Progress", "HMD OFF/BRT Knob"));
            //AddFunction(Switch.CreateThreeWaySwitch(this, TODO, "3439", "3439", "1", "ON", "0", "STBY", "-1", "OFF", "Work In Progress", "FLIR Switch", "%0.1f"));
            //AddFunction(Switch.CreateThreeWaySwitch(this, TODO, "3441", "3441", "1", "ARM", "0", "SAFE", "-1", "AFT", "Work In Progress", "LTD/R Switch", "%0.1f"));
            //AddFunction(Switch.CreateToggleSwitch(this, TODO, "3442", "3442", "1", "ON", "0", "OFF", "Work In Progress", "LST/NFLR Switch", "%0.1f"));
            //AddFunction(new PushButton(this, TODO, "3315", "3315", "Work In Progress", "Left Video Sensor BIT Initiate Pushbutton - Push to initiate BIT"));
            //AddFunction(new PushButton(this, TODO, "3318", "3318", "Work In Progress", "Right Video Sensor BIT Initiate Pushbutton - Push to initiate BIT"));
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

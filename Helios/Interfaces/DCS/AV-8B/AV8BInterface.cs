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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.AV8B
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Interfaces.DCS.AV8B.Functions;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using Microsoft.Win32;
    using System;

    [HeliosInterface("Helios.AV8B", "DCS AV-8B", typeof(AV8BInterfaceEditor), typeof(UniqueHeliosInterfaceFactory))]
    public class AV8BInterface : BaseUDPInterface
    {
        private string _dcsPath;

        private bool _phantomFix;
        private int _phantomLeft;
        private int _phantomTop;

        private long _nextCheck = 0;

        #region Devices
        //  The device list seems to be a moving target at this moment and 2.5.1 saw some changes which altered numbers. 
        //  Need to keep an eye on devices.lua
        private const string ELECTRIC = "1";
        private const string COMM1 = "2";
        private const string COMM2 = "3";
        private const string INTERCOM = "4";
        private const string TACAN = "5";
        private const string AWLS = "6";
        private const string RSC = "7";
        private const string ACNIP = "8";
        private const string DECS = "9";
        private const string ADC = "10";
        private const string MSC = "11";
        private const string NAV_INS = "12";
        private const string VREST = "13";
        private const string NAVFLIR = "14";
        private const string TGTPOD = "15";
        private const string DMT = "16";
        private const string RWR = "17";
        private const string RWRCONTROL = "18";
        private const string FLIGHTINSTRUMENTS = "19";
        private const string EDP = "20";
        private const string FQIS = "21";
        private const string HUDCONTROL = "22";
        private const string UFCCONTROL = "23";
        private const string ODUCONTROL = "24";
        private const string EHSD_MAP = "25";
        private const string MPCD_LEFT = "26";
        private const string MPCD_RIGHT = "27";
        private const string FLIGHTCONTROLS = "28";
        private const string SMC = "29";
        private const string EWS = "30";
        private const string CBTSYST = "31";
        private const string TVWPNS = "32";
        private const string LTEXT = "33";
        private const string LTINT = "34";
        private const string LTWCA = "35";
        private const string ECS = "36";
        private const string HYDRAULIC = "37";
        private const string NETWORKINTERFACE = "38";
        private const string ANVIS9 = "39";
        private const string AUTOSTART = "40";
        #endregion

        //#region Buttons
        //private const string BUTTON_1 = "3001";
        //private const string BUTTON_2 = "3002";
        //private const string BUTTON_3 = "3003";
        //private const string BUTTON_4 = "3004";
        //private const string BUTTON_5 = "3005";
        //private const string BUTTON_6 = "3006";
        //private const string BUTTON_7 = "3007";
        //private const string BUTTON_8 = "3008";
        //private const string BUTTON_9 = "3009";
        //private const string BUTTON_10 = "3010";
        //private const string BUTTON_11 = "3011";
        //private const string BUTTON_12 = "3012";
        //private const string BUTTON_13 = "3013";
        //private const string BUTTON_14 = "3014";
        //private const string BUTTON_15 = "3015";
        //private const string BUTTON_16 = "3016";
        //private const string BUTTON_17 = "3017";
        //private const string BUTTON_18 = "3018";
        //private const string BUTTON_19 = "3019";
        //private const string BUTTON_20 = "3020";
        //private const string BUTTON_21 = "3021";
        //private const string BUTTON_22 = "3022";
        //private const string BUTTON_23 = "3023";
        //private const string BUTTON_24 = "3024";
        //private const string BUTTON_25 = "3025";
        //private const string BUTTON_26 = "3026";
        //private const string BUTTON_27 = "3027";
        //private const string BUTTON_28 = "3028";
        //private const string BUTTON_29 = "3029";
        //private const string BUTTON_30 = "3030";
        //private const string BUTTON_31 = "3031";
        //private const string BUTTON_32 = "3032";
        //private const string BUTTON_33 = "3033";
        //private const string BUTTON_34 = "3034";
        //private const string BUTTON_35 = "3035";
        //private const string BUTTON_36 = "3036";
        //private const string BUTTON_37 = "3037";
        //private const string BUTTON_38 = "3038";
        //private const string BUTTON_39 = "3039";
        //private const string BUTTON_40 = "3040";
        //private const string BUTTON_41 = "3041";
        //private const string BUTTON_42 = "3042";
        //private const string BUTTON_43 = "3043";
        //private const string BUTTON_44 = "3044";
        //private const string BUTTON_45 = "3045";
        //private const string BUTTON_46 = "3046";
        //private const string BUTTON_47 = "3047";
        //private const string BUTTON_48 = "3048";
        //private const string BUTTON_49 = "3049";
        //private const string BUTTON_50 = "3050";
        //private const string BUTTON_51 = "3051";
        //private const string BUTTON_52 = "3052";
        //private const string BUTTON_53 = "3053";
        //private const string BUTTON_54 = "3054";
        //private const string BUTTON_55 = "3055";
        //private const string BUTTON_56 = "3056";
        //private const string BUTTON_57 = "3057";
        //private const string BUTTON_58 = "3058";
        //private const string BUTTON_59 = "3059";
        //private const string BUTTON_60 = "3060";
        //private const string BUTTON_61 = "3061";
        //private const string BUTTON_62 = "3062";
        //private const string BUTTON_63 = "3063";
        //private const string BUTTON_64 = "3064";
        //private const string BUTTON_65 = "3065";
        //private const string BUTTON_66 = "3066";
        //private const string BUTTON_67 = "3067";
        //private const string BUTTON_68 = "3068";
        //private const string BUTTON_69 = "3069";
        //#endregion

        public AV8BInterface()
            : base("DCS AV8B")
        {
            DCSConfigurator config = new DCSConfigurator("DCSAV8B", DCSPath);
            config.ExportConfigPath = "Config\\Export";
            config.ExportFunctionsPath = "pack://application:,,,/Helios;component/Interfaces/DCS/AV-8B/ExportFunctions.lua";
            Port = config.Port;
            _phantomFix = config.PhantomFix;
            _phantomLeft = config.PhantomFixLeft;
            _phantomTop = config.PhantomFixTop;

            #region Left MCFD
            AddFunction(new PushButton(this, MPCD_LEFT, "3200", "3200", "Left MFCD", "OSB01", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3201", "3201", "Left MFCD", "OSB02", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3202", "3202", "Left MFCD", "OSB03", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3203", "3203", "Left MFCD", "OSB04", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3204", "3204", "Left MFCD", "OSB05", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3205", "3205", "Left MFCD", "OSB06", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3206", "3206", "Left MFCD", "OSB07", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3207", "3207", "Left MFCD", "OSB08", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3208", "3208", "Left MFCD", "OSB09", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3209", "3209", "Left MFCD", "OSB10", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3210", "3210", "Left MFCD", "OSB11", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3211", "3211", "Left MFCD", "OSB12", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3212", "3212", "Left MFCD", "OSB13", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3213", "3213", "Left MFCD", "OSB14", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3214", "3214", "Left MFCD", "OSB15", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3215", "3215", "Left MFCD", "OSB16", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3216", "3216", "Left MFCD", "OSB17", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3217", "3217", "Left MFCD", "OSB18", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3218", "3218", "Left MFCD", "OSB19", "1", "0", "%1d"));
            AddFunction(new PushButton(this, MPCD_LEFT, "3219", "3219", "Left MFCD", "OSB20", "1", "0", "%1d"));
            AddFunction(new Axis(this, MPCD_LEFT, "3194", "3194", 0.1d, 0d, 1d, "Left MFCD", "Off/Brightness Control"));
            #endregion

            #region Right MCFD
            AddFunction(new PushButton(this, MPCD_RIGHT, "3224", "3224", "Right MFCD", "OSB01"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3225", "3225", "Right MFCD", "OSB02"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3226", "3226", "Right MFCD", "OSB03"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3227", "3227", "Right MFCD", "OSB04"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3228", "3228", "Right MFCD", "OSB05"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3229", "3229", "Right MFCD", "OSB06"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3230", "3230", "Right MFCD", "OSB07"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3231", "3231", "Right MFCD", "OSB08"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3232", "3232", "Right MFCD", "OSB09"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3233", "3233", "Right MFCD", "OSB10"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3234", "3234", "Right MFCD", "OSB11"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3235", "3235", "Right MFCD", "OSB12"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3236", "3236", "Right MFCD", "OSB13"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3237", "3237", "Right MFCD", "OSB14"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3238", "3238", "Right MFCD", "OSB15"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3239", "3239", "Right MFCD", "OSB16"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3240", "3240", "Right MFCD", "OSB17"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3241", "3241", "Right MFCD", "OSB18"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3242", "3242", "Right MFCD", "OSB19"));
            AddFunction(new PushButton(this, MPCD_RIGHT, "3243", "3243", "Right MFCD", "OSB20"));
            AddFunction(new Axis(this, MPCD_RIGHT, "3195", "3195", 0.1d, 0d, 1d, "Right MFCD", "Off/Brightness Control"));

            #endregion

            #region UFC
            AddFunction(new PushButton(this, UFCCONTROL, "3302", "3302", "UFC", "1"));
            AddFunction(new PushButton(this, UFCCONTROL, "3303", "3303", "UFC", "2/N"));
            AddFunction(new PushButton(this, UFCCONTROL, "3304", "3304", "UFC", "3"));
            AddFunction(new PushButton(this, UFCCONTROL, "3306", "3306", "UFC", "4/W"));
            AddFunction(new PushButton(this, UFCCONTROL, "3307", "3307", "UFC", "5"));
            AddFunction(new PushButton(this, UFCCONTROL, "3308", "3308", "UFC", "6/E"));
            AddFunction(new PushButton(this, UFCCONTROL, "3310", "3310", "UFC", "7"));
            AddFunction(new PushButton(this, UFCCONTROL, "3311", "3311", "UFC", "8/S"));
            AddFunction(new PushButton(this, UFCCONTROL, "3312", "3312", "UFC", "9"));
            AddFunction(new PushButton(this, UFCCONTROL, "3315", "3315", "UFC", "0"));
            AddFunction(new PushButton(this, UFCCONTROL, "3316", "3316", "UFC", "."));
            AddFunction(new PushButton(this, UFCCONTROL, "3313", "3313", "UFC", "-"));
            AddFunction(new PushButton(this, UFCCONTROL, "3314", "3314", "UFC", "Enter"));
            AddFunction(new PushButton(this, UFCCONTROL, "3305", "3305", "UFC", "Clear"));
            AddFunction(new PushButton(this, UFCCONTROL, "3294", "3294", "UFC", "Timer"));
            AddFunction(new PushButton(this, UFCCONTROL, "3324", "3324", "UFC", "Altitude"));
            AddFunction(new PushButton(this, UFCCONTROL, "3318", "3318", "UFC", "IFF"));
            AddFunction(new PushButton(this, UFCCONTROL, "3319", "3319", "UFC", "TACAN"));
            AddFunction(new PushButton(this, UFCCONTROL, "3320", "3320", "UFC", "All Weather Landing"));
            AddFunction(new PushButton(this, UFCCONTROL, "3317", "3317", "UFC", "On/Off"));
            AddFunction(new PushButton(this, UFCCONTROL, "3325", "3325", "UFC", "Emmission Control"));
            AddFunction(new PushButton(this, UFCCONTROL, "3296", "3296", "UFC", "Target of Opportunity"));
            AddFunction(new PushButton(this, UFCCONTROL, "3322", "3322", "UFC", "Waypoint Overfly"));
            AddFunction(new PushButton(this, UFCCONTROL, "3321", "3321", "UFC", "Weapons"));
            AddFunction(new PushButton(this, UFCCONTROL, "3323", "3323", "UFC", "RADAR Beacon"));
            AddFunction(new PushButton(this, UFCCONTROL, "3297", "3297", "UFC", "I/P"));
            AddFunction(new PushButton(this, UFCCONTROL, "3309", "3309", "UFC", "Save"));
            AddFunction(new PushButton(this, LTWCA, "3198", "3198", "Caution/Warning", "Master Caution Button"));
            AddFunction(new FlagValue(this, "196", "Caution/Warning", "Master Caution", "Master Caution indicator"));
            AddFunction(new PushButton(this, LTWCA, "3199", "3199", "Caution/Warning", "Master Warning Button"));
            AddFunction(new FlagValue(this, "197", "Caution/Warning", "Master Warning", "Master warning indicator"));
            AddFunction(new Axis(this, UFCCONTROL, "3295", "3295", 0.1d, 0d, 1d, "UFC", "Display Brightness"));
            AddFunction(new Axis(this, UFCCONTROL, "3298", "3298", 0.1d, 0d, 1d, "UFC", "Comm 1 Volume"));
            AddFunction(new Axis(this, UFCCONTROL, "3299", "3299", 0.1d, 0d, 1d, "UFC", "Comm 2 Volume"));
            AddFunction(new Axis(this, UFCCONTROL, "3300", "3300", 0.1d, 0d, 1d, "UFC", "Comm 1 Channel selector"));
            AddFunction(new Axis(this, UFCCONTROL, "3301", "3301", 0.1d, 0d, 1d, "UFC", "Comm 2 Channel selector"));

            //       public IndicatorPushButton(BaseUDPInterface sourceInterface, string deviceId, string buttonId, string argId, string device, string name)
            //       public FlagValue(BaseUDPInterface sourceInterface, string id, string device, string name, string description)
            //       public PushButton(BaseUDPInterface sourceInterface, string deviceId, string buttonId, string argId, string device, string name)
            AddFunction(Switch.CreateThreeWaySwitch(this, HUDCONTROL, "3288", "3288", "1", "Norm", "0", "Reject 1", "-1", "Reject 2", "HUD Control", "Declutter switch", "%1d"));
            AddFunction(new Axis(this, HUDCONTROL, "3289", "3289", 0.1d, 0d, 1d, "HUD Control", "Off/Brightness control"));
            AddFunction(Switch.CreateThreeWaySwitch(this, HUDCONTROL, "3290", "3290", "1", "Day", "0", "Auto", "-1", "Night", "HUD Control", "Display Mode switch", "%1d"));
            AddFunction(new Axis(this, HUDCONTROL, "3291", "3291", 0.1d, 0d, 1d, "HUD Control", "Video Brightness"));
            AddFunction(new Axis(this, HUDCONTROL, "3292", "3292", 0.1d, 0d, 1d, "HUD Control", "Video Contrast"));
            AddFunction(Switch.CreateToggleSwitch(this, HUDCONTROL, "3293", "3293", "1", "Baro", "0", "Rdr", "HUD Control", "Altimeter Mode Switch", "%1d"));

            #endregion

            #region Master Modes
            AddFunction(new PushButton(this, MSC, "3282", "3282", "Master Modes", "NAV Button"));
            AddFunction(new FlagValue(this, "283", "Master Modes", "Nav Indicator", "Nav Master Mode Indicator"));
            AddFunction(new PushButton(this, MSC, "3284", "3284", "Master Modes", "VSTOL Button"));
            AddFunction(new FlagValue(this, "285", "Master Modes", "VSTOL Indicator", "VSTOL Master Mode Indicator"));
            AddFunction(new PushButton(this, MSC, "3280", "3280", "Master Modes", "A/G Button"));
            AddFunction(new FlagValue(this, "281", "Master Modes", "A/G Indicator", "A/G Master Mode Indicator"));
            #endregion

            #region ODU
            AddFunction(new PushButton(this, ODUCONTROL, "3250", "3250", "ODU", "1"));
            AddFunction(new PushButton(this, ODUCONTROL, "3251", "3251", "ODU", "2"));
            AddFunction(new PushButton(this, ODUCONTROL, "3252", "3252", "ODU", "3"));
            AddFunction(new PushButton(this, ODUCONTROL, "3248", "3248", "ODU", "4"));
            AddFunction(new PushButton(this, ODUCONTROL, "3249", "3249", "ODU", "5"));
            #endregion

            #region Threat Indicators
            AddFunction(new FlagValue(this, "276", "Threat Indicators", "SAM", "SAM launch detected"));
            AddFunction(new FlagValue(this, "277", "Threat Indicators", "CW", "CW Ground Tracking (Continuous Wave) radar is locked on aircraft"));
            AddFunction(new FlagValue(this, "278", "Threat Indicators", "AI", "Air Intercept radar is locked on aircraft. Flashes if launch detected"));
            AddFunction(new FlagValue(this, "279", "Threat Indicators", "AAA", "AAA gun radar is locked on aircraft"));
            #endregion

            #region Caution Indicators
            AddFunction(new FlagValue(this, "326", "Caution Indicators", "L Fuel Indicator", ""));
            AddFunction(new FlagValue(this, "327", "Caution Indicators", "R fuel Indicator", ""));
            AddFunction(new FlagValue(this, "328", "Caution Indicators", "15 Sec Indicator", "JPT above normal lift rating"));
            AddFunction(new FlagValue(this, "329", "Caution Indicators", "MFS", "Manual Fuel System on"));
            AddFunction(new FlagValue(this, "330", "Caution Indicators", "Bingo", "Bingo Warning Light"));
            AddFunction(new FlagValue(this, "331", "Caution Indicators", "H2O", "Less than 15 secs water left"));
            #endregion

            #region Warning Indicators
            AddFunction(new FlagValue(this, "334", "Warning Indicators", "Fire", "Fire in engine"));
            AddFunction(new FlagValue(this, "335", "Warning Indicators", "LAW", "Low alt warning"));
            AddFunction(new FlagValue(this, "336", "Warning Indicators", "Flaps Warning Light", "Flap System Failure"));
            AddFunction(new FlagValue(this, "337", "Warning Indicators", "L Tank", "Left tank over pressure"));
            AddFunction(new FlagValue(this, "338", "Warning Indicators", "R Tank", "Right Tank Over pressure"));
            AddFunction(new FlagValue(this, "339", "Warning Indicators", "HYD", "Both hydraulics failed"));
            AddFunction(new FlagValue(this, "340", "Warning Indicators", "Gear", "Gear Warning Light"));
            AddFunction(new FlagValue(this, "341", "Warning Indicators", "OT", "Over temp JPT"));
            AddFunction(new FlagValue(this, "342", "Warning Indicators", "JPTL", "JPTL Control Fail"));
            AddFunction(new FlagValue(this, "343", "Warning Indicators", "EFC", "All Digital engine controls failed"));
            AddFunction(new FlagValue(this, "344", "Warning Indicators", "GEN", "AC Generator offline"));
            #endregion

            #region Refueling Indicators
            AddFunction(new FlagValue(this, "750", "Refueling Indicators", "Canopy Left", "Refueling Left Tank"));
            AddFunction(new FlagValue(this, "751", "Refueling Indicators", "Canopy Ready", "Refueling Ready light"));
            AddFunction(new FlagValue(this, "752", "Refueling Indicators", "Canopy Right", "Refuelng Right Light"));
            #endregion

            #region Fuel Quantity Indicator System
            AddFunction(new Axis(this, FQIS, "3380", "3380", 0.05d, 0.15d, 0.85d, "Fuel Quantity", "Bingo Fuel Set Knob"));
            AddFunction(new Switch(this, FQIS, "3379", new SwitchPosition[] { new SwitchPosition("0.99", "OUTBD", "3395"), new SwitchPosition( "0.66", "INBD", "3395"), new SwitchPosition("0.33", "WING", "3395"), new SwitchPosition("0.0", "INT", "3395"), new SwitchPosition("-0.33", "TOTAL", "3395"), new SwitchPosition("-0.66", "FEED", "3395"), new SwitchPosition("-0.99", "BIT", "3395") }, "Fuel Quantity", "Fuel Totaliser Selector", "%0.1f"));

            #endregion

            #region Advisory indicators
            AddFunction(new FlagValue(this, "560", "Advisory Indicators", "OXY", "OBOGS malfunction"));
            AddFunction(new FlagValue(this, "561", "Advisory Indicators", "WSHLD", "Windshield hot"));
            AddFunction(new FlagValue(this, "562", "Advisory Indicators", "HYD 1", "HYD 1 pressure ≤ 1400 psi"));
            AddFunction(new FlagValue(this, "563", "Advisory Indicators", "HYD 2", "HYD 2 pressure ≤ 1400 psi"));
            AddFunction(new FlagValue(this, "564", "Advisory Indicators", "L PUMP", "Left fuel boost pump pressure low"));
            AddFunction(new FlagValue(this, "565", "Advisory Indicators", "R PUMP", "Right fuel boost pump pressure low"));
            AddFunction(new FlagValue(this, "566", "Advisory Indicators", "L TRANS", "Low air pressure to the left feeder tank"));
            AddFunction(new FlagValue(this, "567", "Advisory Indicators", "R TRANS", "Low air pressure to the right feeder tank"));
            AddFunction(new FlagValue(this, "568", "Advisory Indicators", "FLAPS 1", "Flaps 1 channel failed"));
            AddFunction(new FlagValue(this, "569", "Advisory Indicators", "FLAPS 2", "Flaps 2 channel failed"));
            AddFunction(new FlagValue(this, "570", "Advisory Indicators", "AUT FLP", "Auto flap mode or ADC failed"));
            AddFunction(new FlagValue(this, "571", "Advisory Indicators", "PROP", "Fuel proportioner off or failed"));
            AddFunction(new FlagValue(this, "572", "Advisory Indicators", "LIDS", "LIDS not in correct position"));
            AddFunction(new FlagValue(this, "573", "Advisory Indicators", "OIL", "Oil pressure low"));
            AddFunction(new FlagValue(this, "574", "Advisory Indicators", "APU GEN", "APU selected and emergency generator failed"));
            AddFunction(new FlagValue(this, "575", "Advisory Indicators", "Blank 1", "Unused"));
            AddFunction(new FlagValue(this, "576", "Advisory Indicators", "GPS", "GPS not valid"));
            AddFunction(new FlagValue(this, "577", "Advisory Indicators", "DEP RES", "Departure resistance reduced"));
            AddFunction(new FlagValue(this, "578", "Advisory Indicators", "DC", "Main transformer-rectifier failed"));
            AddFunction(new FlagValue(this, "579", "Advisory Indicators", "STBY TRU", "Standby TRU inoperative or off line"));
            AddFunction(new FlagValue(this, "580", "Advisory Indicators", "CS COOL", "Cockpit avionics cooling fan failed"));
            AddFunction(new FlagValue(this, "581", "Advisory Indicators", "LOAD", "Fuel asymmetry over VL limit"));
            AddFunction(new FlagValue(this, "582", "Advisory Indicators", "CANOPY", "Canopy not closed and locked"));
            AddFunction(new FlagValue(this, "583", "Advisory Indicators", "INS", "INS aligning or failed"));
            AddFunction(new FlagValue(this, "584", "Advisory Indicators", "SKID", "Anti-Skid System Malfunction"));
            AddFunction(new FlagValue(this, "585", "Advisory Indicators", "EFC", "DECU 1 or DECU 2 has failed"));
            AddFunction(new FlagValue(this, "586", "Advisory Indicators", "NWS", "Nosewheel Steering malfunction"));
            AddFunction(new FlagValue(this, "587", "Advisory Indicators", "AFC", "AFC malfunction or AFC deselected"));
            AddFunction(new FlagValue(this, "588", "Advisory Indicators", "C*AUT", "Computed delivery mode (AUTO and CCIP) not available"));
            AddFunction(new FlagValue(this, "589", "Advisory Indicators", "H2O SEL", "Over 250 knots and water switch not OFF"));
            AddFunction(new FlagValue(this, "590", "Advisory Indicators", "APU", "APU operating"));
            AddFunction(new FlagValue(this, "591", "Advisory Indicators", "PITCH", "Pitch stab aug off or failed"));
            AddFunction(new FlagValue(this, "592", "Advisory Indicators", "IFF", "Mode 4 off, not zeroized or not responding"));
            AddFunction(new FlagValue(this, "593", "Advisory Indicators", "SPD BRK", "Gear up and speed brake extended. Gear down and speed brake not 250"));
            AddFunction(new FlagValue(this, "594", "Advisory Indicators", "DROOP", "Ailerons dropped"));
            AddFunction(new FlagValue(this, "595", "Advisory Indicators", "ROLL", "Roll stab aug off or failed"));
            AddFunction(new FlagValue(this, "596", "Advisory Indicators", "AFT BAY", "Aft avionics bay ECS failed"));
            AddFunction(new FlagValue(this, "597", "Advisory Indicators", "AV BIT", "Refuelng Right Light"));
            AddFunction(new FlagValue(this, "598", "Advisory Indicators", "Blank 2", "Unused"));
            AddFunction(new FlagValue(this, "599", "Advisory Indicators", "YAW", "Yaw stab aug off or failed"));
            AddFunction(new FlagValue(this, "600", "Advisory Indicators", "CW NOGO", "Jammer Failure. Cannot jam CW radars"));
            AddFunction(new FlagValue(this, "601", "Advisory Indicators", "P JAM", "Jammer Pod Active: Jamming Pulse-Doppler radar signals"));
            AddFunction(new FlagValue(this, "602", "Advisory Indicators", "JMR HOT", "Jammer pod overtemp"));
            AddFunction(new FlagValue(this, "603", "Advisory Indicators", "ENG EXC", "Engine overspeed, overtemperature or over g was detected"));
            AddFunction(new FlagValue(this, "604", "Advisory Indicators", "P NOGO", "Jammer Failure. Cannot jam Pulse-Doppler radars"));
            AddFunction(new FlagValue(this, "605", "Advisory Indicators", "CW JAM", "Jammer Pod Active: Jamming CW radar signals"));
            AddFunction(new FlagValue(this, "606", "Advisory Indicators", "REPLY", "IFF responding to Mode 4 interrogation"));
            #endregion

            #region Landing Gear Indicators
            AddFunction(new FlagValue(this, "446", "Landing Gear Indicators", "Gear - Lever", "Red Gear Lever"));
            AddFunction(new FlagValue(this, "462", "Landing Gear Indicators", "Nose Wrn", "Nose Gear Warning"));
            AddFunction(new FlagValue(this, "463", "Landing Gear Indicators", "Nose", "Nose Gear Ready"));
            AddFunction(new FlagValue(this, "464", "Landing Gear Indicators", "Left Wrn", "Left Wheel Warning"));
            AddFunction(new FlagValue(this, "465", "Landing Gear Indicators", "Left", "Left Wheel Ready"));
            AddFunction(new FlagValue(this, "466", "Landing Gear Indicators", "Right Wrn", "Right Wheel Warning"));
            AddFunction(new FlagValue(this, "467", "Landing Gear Indicators", "Right", "Right Wheel Ready"));
            AddFunction(new FlagValue(this, "468", "Landing Gear Indicators", "Main Wrn", "Main Gear Warning"));
            AddFunction(new FlagValue(this, "469", "Landing Gear Indicators", "Main", "Main Gear Ready"));
            #endregion

            #region Left Hand Advisory Indicators
            AddFunction(new FlagValue(this, "451", "LH Flaps & Water", "SEL indicator", "Combat thrust limiter selected"));
            AddFunction(new FlagValue(this, "452", "LH Flaps & Water", "CMBT indicator", "Combat thrust activated. Flashes after 2 ½ minutes"));
            AddFunction(new FlagValue(this, "453", "LH Flaps & Water", "STO indicator", "Flap switch in STOL"));
            AddFunction(Switch.CreateThreeWaySwitch(this, DECS, "3449", "3449", "-1", "TO", "0", "Off", "1", "VSTOL", "LH Flaps & Water", "H2O Mode Switch", "%1d"));
            AddFunction(new PushButton(this, DECS, "3450", "3450", "LH Flaps & Water", "Combat Thrust Button"));
            AddFunction(Switch.CreateThreeWaySwitch(this, VREST, "3454", "3454", "-1", "Cruise", "0", "Auto", "1", "VSTOL", "LH Flaps & Water", "Flaps Mode Switch", "%1d"));
            #endregion

            #region Stores Management Controller
            AddFunction(Switch.CreateToggleSwitch(this, SMC, "3287", "3287", "1", "Armed", "0", "Safe", "Stores Management", "Master Arm Switch", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SMC, "3396", "3396", "1", "Up", "0", "Off", "-1", "Dn", "Stores Management", "Armament Mode control", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SMC, "3397", "3397", "1", "Increase", "0", "Off", "-1", "decrease", "Stores Management", "Armament Fuzing control", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SMC, "3398", "3398", "1", "Increase", "0", "Off", "-1", "decrease", "Stores Management", "Armament Quantity Tens", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SMC, "3399", "3399", "1", "Increase", "0", "Off", "-1", "decrease", "Stores Management", "Armament Quantity Units", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SMC, "3400", "3400", "1", "Increase", "0", "Off", "-1", "decrease", "Stores Management", "Armament Multiple Release", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SMC, "3401", "3401", "1", "Increase", "0", "Off", "-1", "decrease", "Stores Management", "Armament Release interval hundreds", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SMC, "3402", "3402", "1", "Increase", "0", "Off", "-1", "decrease", "Stores Management", "Armament Release interval tens", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SMC, "3403", "3403", "1", "Increase", "0", "Off", "-1", "decrease", "Stores Management", "Armament Release interval units", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, SMC, "3420", "3420", "1", "Cooling", "0", "Off", "Stores Management", "IR Cool Switch", "%1d"));
            AddFunction(new PushButton(this, SMC, "3407", "3407", "Stores Management", "Select station 1"));
            AddFunction(new FlagValue(this, "406", "Stores Management", "Station 1 Selected", "Station 1 selected indicator"));
            AddFunction(new PushButton(this, SMC, "3409", "3409", "Stores Management", "Select station 2"));
            AddFunction(new FlagValue(this, "408", "Stores Management", "Station 2 Selected", "station 2 selected indicator"));
            AddFunction(new PushButton(this, SMC, "3411", "3411", "Stores Management", "Select station 3"));
            AddFunction(new FlagValue(this, "410", "Stores Management", "Station 3 Selected", "station 3 selected indicator"));
            AddFunction(new PushButton(this, SMC, "3413", "3413", "Stores Management", "Select station 4"));
            AddFunction(new FlagValue(this, "412", "Stores Management", "Station 4 Selected", "station 4 selected indicator"));
            AddFunction(new PushButton(this, SMC, "3415", "3415", "Stores Management", "Select station 5"));
            AddFunction(new FlagValue(this, "414", "Stores Management", "Station 5 Selected", "station 5 selected indicator"));
            AddFunction(new PushButton(this, SMC, "3417", "3417", "Stores Management", "Select station 6"));
            AddFunction(new FlagValue(this, "416", "Stores Management", "Station 6 Selected", "station 6 selected indicator"));
            AddFunction(new PushButton(this, SMC, "3419", "3419", "Stores Management", "Select station 7"));
            AddFunction(new FlagValue(this, "418", "Stores Management", "Station 7 Selected", "station 7 selected indicator"));
            AddFunction(new PushButton(this, SMC, "3286", "3286", "Stores Management", "Launch Flare Salvo"));
            AddFunction(new Switch(this, SMC, "3404", new SwitchPosition[] { new SwitchPosition("1.0", "FUEL", "3404"), new SwitchPosition("0.5", "CMBT", "3404"), new SwitchPosition("0.0", "SAFE", "3404"), new SwitchPosition("-0.5", "STOR", "3404"),new SwitchPosition("-1.0", "STA", "3404") },  "Stores Management", "Jettison Mode Selector", "%0.1f"));
            AddFunction(new PushButton(this, SMC, "3405", "3405", "Stores Management", "Jettison Stores"));
            AddFunction(new Switch(this, SMC, "3395", new SwitchPosition[] { new SwitchPosition("1.0", "T", "3395"), new SwitchPosition("0.66", "N", "3395"), new SwitchPosition("0.33", "N/T", "3395"), new SwitchPosition("0.0", "Norm", "3395")}, "Stores Management", "Manual Fuzing Release Control", "%0.1f"));

            #endregion

            #region Engine Display Panel
            AddFunction(new ScaledNetworkValue(this, "271", 0.94d, "EDP", "Nozzle Position", "Current Nozzle position.", "", BindingValueUnits.Degrees));
            #endregion

            #region Flight Instruments
            AddFunction(new Altimeter(this));
            //AddFunction(new RotaryEncoder(this, FM_PROXY, BUTTON_1, "62", 0.04d, "Altimeter", "Pressure"));

            CalibrationPointCollectionDouble vviScale = new CalibrationPointCollectionDouble(-0.6d, -6000d, 0.6d, 6000d);
            vviScale.Add(new CalibrationPointDouble(0d, 0d));
            AddFunction(new ScaledNetworkValue(this, "362", vviScale, "Flight Instruments", "VVI", "Vertical velocity indicator -6000 to +6000.", "", BindingValueUnits.FeetPerMinute));

            CalibrationPointCollectionDouble AoAScale = new CalibrationPointCollectionDouble(-0.05d, -5d, 0.20d, 20d);
            AoAScale.Add(new CalibrationPointDouble(0d, 0d));
            AddFunction(new ScaledNetworkValue(this, "361", AoAScale, "AOA", "Angle of Attack", "Current angle of attack of the aircraft.", "", BindingValueUnits.Degrees));
            AddFunction(new FlagValue(this, "360", "AOA", "Off Flag", ""));

            CalibrationPointCollectionDouble airspeedScale = new CalibrationPointCollectionDouble(0.0d, 0.0d, 1.0d, 1000d);
            AddFunction(new ScaledNetworkValue(this, "346", airspeedScale, "IAS", "Airspeed", "Current indicated air speed of the aircraft.", "", BindingValueUnits.Knots));

            #endregion

        }

        private string DCSPath
        {
            get
            {
                if (_dcsPath == null)
                {
                    RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS AV-8B");
                    if (pathKey != null)
                    {
                        _dcsPath = (string)pathKey.GetValue("Path");
                        pathKey.Close();
                        ConfigManager.LogManager.LogDebug("DCS AV-8B Interface Editor - Found DCS Path (Path=\"" + _dcsPath + "\")");
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

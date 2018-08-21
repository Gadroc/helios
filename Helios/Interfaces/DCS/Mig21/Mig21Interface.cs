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


namespace GadrocsWorkshop.Helios.Interfaces.DCS.MiG21
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;

    [HeliosInterface("Helios.MiG21", "DCS MiG21", typeof(MiG21InterfaceEditor), typeof(UniqueHeliosInterfaceFactory))]
    public class MiG21Interface : BaseUDPInterface
    {
        #region Devices
        private const string DC_BUS = "1";
        private const string AC_BUS = "2";
        private const string ENGINE_START_DEVICE = "3";
        private const string FUEL_PUMPS = "4";
        private const string FUEL_SYSTEM = "5";
        private const string ENGINE = "6";
        private const string CONTROL_SYSTEM = "7";
        private const string SAU = "8";
        private const string TRIMER = "9";
        private const string SPS = "10";
        private const string ARU = "11";
        private const string AIRBRAKE = "12";
        private const string GEAR_BRAKES = "13";
        private const string GEARS = "14";
        private const string FLAPS = "15";
        private const string CHUTE = "16";  //comment number was 17 but its 16th in devices.lua and now works
        private const string KONUS = "17";
        private const string SOPLO = "18";
        private const string OXYGENE_SYSTEM = "19";
        private const string COMPRESSED_AIR_SYSTEM = "20";
        private const string GYRO_DEVICES = "21";
        private const string RADIO = "22";
        private const string KSI = "23";
        private const string ARK = "24";
        private const string RSBN = "25";
        private const string avAChS = "26";
        private const string PITOT_TUBES = "27";
        private const string KPP = "28";
        private const string IAS_INDICATOR = "29";
        private const string TAS_INDICATOR = "30";
        private const string M_INDICATOR = "31";
        private const string ALTIMETER = "32";
        private const string RADIO_ALTIMETER = "33";
        private const string DA_200 = "34";
        private const string ACCELEROMETER = "35";
        private const string UUA = "36";
        private const string SPO = "37";
        private const string SRZO = "37";
        private const string SOD = "39";
        private const string RADAR = "40";
        private const string ASP = "41";
        private const string WEAPON_CONTROL = "42";
        private const string CANOPY = "43";
        private const string MAIN_HYDRO = "44";
        private const string HELMET_VISOR = "45";
        private const string LIGHTS = "46";
        private const string LIGHTS_WARNING = "47";
        private const string SPRD = "48";
        private const string SARPP = "49";
        private const string AIR_CONDITIONING = "50";
        private const string UNCLASSIFIED = "51";
        private const string MARKER = "52";
        private const string FIRE_EXTINGUISHER = "53";
        private const string MACROS = "54";
        private const string INTERCOM = "55";
        private const string NUKE_CONTROL = "56";
        private const string SPS_CONTROL = "57";
        private const string ASO = "58";
        private const string ARCADE = "59";
        private const string ARCADE2 = "60";
        private const string KNEEBOARD = "61";
        private const string PADLOCK = "62";
        private const string DEBUG = "63";
        private const string DEBUG_AVIONICS_MODULE = "62";
        #endregion

        #region Buttons
        private const string BATTERY_ON = "3001";
        private const string BATTERY_HEAT = "3002";
        private const string DC_GENERATOR = "3003";
        private const string AC_GENERATOR = "3004";
        private const string PO7501 = "3005";
        private const string PO7502 = "3006";
        private const string EMERGENCY_INVERTER = "3007";
        private const string GIRO_1 = "3008";
        private const string GIRO_2 = "3009";
        private const string PUMP_3 = "3010";
        private const string PUMP_1 = "3011";
        private const string PUMP_RASHOD = "3012";
        private const string FUEL_QT = "3013";
        private const string ZAZIG = "3014";
        private const string HOTSTART = "3015";
        private const string START_BUTTON = "3016";
        private const string AIRSTART = "3017";
        private const string ACCEL_RESET = "3018";
        private const string PITO_SELECT = "3019";
        private const string PITO_HEAT_MAIN = "3020";
        private const string PITOT_HEAT_AUX = "3021";
        private const string SURGE_DOORS = "3022";
        private const string FORS_MAX = "3023";
        private const string CHR = "3024";
        private const string POZAR_OBORUD = "3025";
        private const string POZAR_SAFETY_COVER = "3026";
        private const string OGNETUSHITEL = "3027";
        private const string TEXT_BACKLIGHT_KB_UP = "3028";
        private const string INSTRUMENTS_BACKLIGHT_KB_UP = "3029";
        private const string RED_LIGHTS_MAIN_KB_UP = "3030";
        private const string WHITE_LIGHTS_MAIN_KB_UP = "3031";
        private const string TEXT_BACKLIGHT_KB_DOWN = "3328";
        private const string INSTRUMENTS_BACKLIGHT_KB_DOWN = "3329";
        private const string RED_LIGHTS_MAIN_KB_DOWN = "3330";
        private const string WHITE_LIGHTS_MAIN_KB_DOWN = "3331";
        private const string NAV_LIGHTS = "3032";
        private const string LANDING_LIGHTS = "3033";
        private const string CHECK_WARNING_LIGHTS10 = "3034";
        private const string CHECK_WARNING_LIGHTS11 = "3074";
        private const string CHECK_WARNING_LIGHTS20 = "3035";
        private const string CHECK_WARNING_LIGHTS21 = "3075";
        private const string CHECK_WARNING_LIGHTS30 = "3036";
        private const string CHECK_WARNING_LIGHTS31 = "3076";
        private const string CHECK_WARNING_LIGHTS40 = "3037";
        private const string CHECK_WARNING_LIGHTS41 = "3077";
        private const string CHECK_WARNING_LIGHTS50 = "3038";
        private const string CHECK_WARNING_LIGHTS51 = "3078";
        private const string CHECK_WARNING_LIGHTS60 = "3039";
        private const string CHECK_WARNING_LIGHTS61 = "3079";
        private const string SORC = "3040";
        private const string RADIO_ON = "3041";
        private const string RADIO_COMPASS_SOUND = "3042";
        private const string SQUELCH = "3043";
        private const string RADIO_VOLUME = "3044";
        private const string RADIO_CHANNEL = "3045";
        private const string RADIO_INTERCOM = "3046";
        private const string ARK_ON = "3047";
        private const string ARK_SOUND = "3048";
        private const string ARK_PEREK_LUCENIE = "3049";
        private const string ARK_CHANNEL = "3050";
        private const string ARK_ZONE = "3051";
        private const string ARK_ANTENA_COMPASS = "3052";
        private const string ARK_FAR_NEAR = "3053";
        private const string RSBN_ON = "3054";
        private const string RSBN_MODE = "3055";
        private const string RSBN_ARK = "3056";
        private const string RSBN_IDENT = "3057";
        private const string RSBN_SOUND = "3058";
        private const string RSBN_NAV = "3059";
        private const string RSBN_LAND = "3060";
        private const string RSBN_RESET = "3061";
        private const string RSBN_BEARING = "3062";
        private const string RSBN_FAR = "3063";
        private const string RSBN_TEST = "3080";
        private const string SAU_ON = "3064";
        private const string SAU_PITCH_ON = "3065";
        private const string SAU_STABIL = "3066";
        private const string SAU_CANCEL = "3067";
        private const string SAU_PRIVEDENIE = "3068";
        private const string SAU_LOW_ALT_ON = "3069";
        private const string SAU_LANDING_CONTROL_COMMAND = "3070";
        private const string SAU_LANDING_CONTROL_AUTO = "3071";
        private const string SAU_RESET_OFF = "3072";
        private const string ALTIMETER_PRESSURE = "3073";
        private const string SPO_ON = "3083";
        private const string SPO_TEST = "3084";
        private const string SPO_DAY_NIGHT = "3085";
        private const string SPO_VOLUME = "3086";
        private const string SRZO_VOPROS = "3087";
        private const string SRZO_CODES = "3088";
        private const string SRZO_ON = "3089";
        private const string SOD_ON = "3090";
        private const string SOD_IDENT = "3091";
        private const string SOD_VOLNI = "3092";
        private const string SOD_MODE = "3093";
        private const string RADAR_ON = "3094";
        private const string RADAR_LOW_ALT = "3095";
        private const string RADAR_FIX_BEAM = "3096";
        private const string RADAR_MGN_STIR = "3097";
        private const string RADAR_JAM_CONT = "3098";
        private const string RADAR_JAM_TMP = "3099";
        private const string RADAR_JAM_PASS = "3100";
        private const string RADAR_JAM_METEO = "3101";
        private const string RADAR_VOPROS = "3102";
        private const string RADAR_JAM_LOW_SPEED = "3103";
        private const string RADAR_KONTROL = "3104";
        private const string RADAR_RESET = "3105";
        private const string SPRD_START_ON = "3106";
        private const string SPRD_DROP_ON = "3107";
        private const string SPRD_START_SAFETY_COVER = "3108";
        private const string SPRD_DROP_SAFETY_COVER = "3109";
        private const string SPRD_START = "3110";
        private const string SPRD_DROP = "3111";
        private const string SPS_ON = "3112";
        private const string ARU_MAN_AUTO = "3113";
        private const string ARU_LOW_SPEED = "3114";
        private const string ARU_HIGH_SPEED = "3115";
        private const string AIRBRAKES = "3116";
        private const string ABS_ON = "3117";
        private const string NOSEGEAR_BRAKE = "3118";
        private const string EMERG_BRAKES = "3119";
        private const string GEAR_HANDLE_FIXATOR = "3120";
        private const string GEAR_LEVER = "3121";
        private const string EMERG_GEARS_MAIN = "3122";
        private const string EMERG_GEARS_NOSE = "3123";
        private const string FLAPS_0 = "3124";
        private const string FLAPS_25 = "3125";
        private const string FLAPS_45 = "3126";
        private const string FLAPS_RESET = "3127";
        private const string DRAGCHUTE = "3128";
        private const string DRAGCHUTE_SAFTEY_COVER = "3129";
        private const string DRAGCHUTE_DISCONNECT = "3130";
        private const string TRIMMER_ON = "3131";
        private const string TRIMMER_BTN_UP = "3132";
        private const string KONUS_ON = "3133";
        private const string KONUS_MAN_AUTO = "3134";
        private const string KONUS_BUTTON = "3135";
        private const string SOPLO2X_POZ = "3136";
        private const string NR27_ON = "3137";
        private const string AILERON_BOOSTERS = "3138";
        private const string KPP_ON = "3139";
        private const string KPP_ARRETIR = "3140";
        private const string KPP_SET_PITCH = "3141";
        private const string NPP_ON = "3142";
        private const string NPP_NASTROJKA = "3143";
        private const string NPP_SET_COURSE = "3144";
        private const string RADIO_ALT_ON = "3145";
        private const string DANGER_ALT_SELECT = "3146";
        private const string HELMET_AIR_CONDITION = "3147";
        private const string EMERG_OXYGENE = "3148";
        private const string MIXTURE_OXYGENE_SELECT = "3149";
        private const string CANOPY_HERMET_HANDLE = "3150";
        private const string CANOPY_LOCK_HANDLE = "3151";
        private const string CANOPY_OPEN = "3152";
        private const string CANOPY_ANTI_ICE = "3153";
        private const string CANOPY_EMERG_RELEASE = "3154";
        private const string ASP_ON = "3155";
        private const string ASP_MAN_AUTO_SELECT = "3156";
        private const string ASP_BOMB_STRELB_SELECT = "3157";
        private const string ASP_MISSILE_GUN_SELECT = "3158";
        private const string ASP_SS_GIRO_SELECT = "3159";
        private const string ASP_PIPPER_ON = "3160";
        private const string ASP_NET_ON = "3161";
        private const string ASP_TARGET_SIZE = "3162";
        private const string ASP_INTERCEPT_ANGLE = "3163";
        private const string ASP_SCALE_LIGHT = "3164";
        private const string ASP_PIPPER_LIGHT = "3165";
        private const string ASP_NET_LIGHT = "3166";
        private const string OBOGREV = "3167";
        private const string PUSK = "3168";
        private const string PITANIE_12 = "3169";
        private const string PITANIE_34 = "3170";
        private const string GS23 = "3171";
        private const string FKP = "3172";
        private const string TAKT_SBROS_SAFETY_COVER = "3173";
        private const string TAKT_SBROS = "3174";
        private const string AVAR_PUSK_SAFETY_COVER = "3175";
        private const string AVAR_PUSK = "3176";
        private const string SBROS_KRIL_BAKOV_SAFETY_COVER = "3177";
        private const string SBROS_KRIL_BAKOV = "3178";
        private const string SBROS_VNESN_SAFETY_COVER = "3179";
        private const string SBROS_VNESN = "3180";
        private const string SBROS_VNUTR_SAFETY_COVER = "3181";
        private const string SbROS_VNUTR = "3182";
        private const string ASP_VOZDUH_ZEMLJA = "3183";
        private const string ASP_SS_NEUTR_RNS = "3184";
        private const string ASP_GUN_RELOAD_1 = "3185";
        private const string ASP_GUN_RELOAD_2 = "3186";
        private const string ASP_GUN_RELOAD_3 = "3187";
        private const string ASP_LAUNCHER_SELECT = "3188";
        private const string MISSILE_SOUND = "3189";
        private const string ZAHVAT = "3190";
        private const string GUN_FIRE_BTN = "3191";
        private const string PUSK_BTN_SAFETY_COVER = "3192";
        private const string PUSK_BTN = "3193";
        private const string CANOPY_CLOSE = "3194";
        private const string SEAT_HEIGHT_UP = "3195";
        private const string SBROS_PODV_BAKOV = "3196";
        //Nuke Box
        private const string IAB_AVAR_SBROS = "3197";
        private const string IAB_AVAR_SBROS_VZR = "3198";
        private const string IAB_BOEVOJ_SBROS = "3199";
        private const string IAB_SPEC_AB = "3200";
        private const string IAB_TORMOZ = "3201";
        private const string IAB_VOZDUH = "3202";
        //=
        private const string DA200_SET = "3203";
        private const string RU_DOBLOG_KB_DOWN = "3204";
        private const string RU_DOBLOG_KB_UP = "3704";
        private const string HELMET_HEAT_MAN_AUT = "3205";
        private const string HELMET_QUICK_HEAT = "3206";
        private const string HELMET_VISOR_EXTRA = "3207";
        private const string AIR_CONDITION_SELECT = "3208";
        private const string SARPP_EXTRA = "3209";
        private const string EMERG_TRANSMIT_SAFETY_COVER = "3210";
        private const string EMERG_TRANSMIT_ON = "3211";
        private const string SEAT_HEIGHT_DOWN = "3212";
        private const string RU_DOBLOG_AXIS = "3213";
        private const string RU_DOBLOG_AXIS_STICK = "3713";
        //SPS Box
        private const string SPS_ON_OFF = "3214";
        private const string SPS_TRANSMIT = "3215";
        private const string SPS_PROGRAM = "3216";
        private const string SPS_CONTINOUS = "3217";
        private const string SPS_TEST = "3218";
        private const string SPS_DISPENSE = "3219";
        private const string SPS_OFF_PARAL_FULL = "3220";
        private const string SPS_COVER = "3221";
        private const string SPS_PUSH = "3222";
        //GUV Box
        private const string GUV_ON_OFF = "3223";
        private const string GUV_POD_MAIN = "3224";
        private const string GUV_ARM_1 = "3225";
        private const string GUV_ARM_2 = "3226";
        private const string GUV_ARM_3 = "3227";
        private const string GEAR_BRAKE_HANDLE = "3228";
        private const string SRZO_DESTR_COVER = "3229";
        private const string SRZO_DESTR = "3230";
        private const string TEXT_BACKLIGHT_AXIS = "3231";
        private const string INSTRUMENTS_BACKLIGHT_AXIS = "3232";
        private const string RED_LIGHTS_MAIN_AXIS = "3233";
        private const string WHITE_LIGHTS_MAIN_AXIS = "3234";
        private const string MIG21_AUTOSTART = "3235";
        private const string MIG21_AUTOSTOP = "3236";
        //237 empty
        private const string RUD_STOP_LOCK = "3238";
        private const string RADAR_FILTER = "3239";
        private const string TRIMMER_BTN_DOWN = "3240";
        //New KB control inputs
        private const string THROTTLE_INC = "3241";
        private const string THROTTLE_DEC = "3242";
        private const string RUD_LEFT = "3243";
        private const string RUD_RIGHT = "3244";
        private const string STICK_UP = "3245";
        private const string STICK_DOWN = "3246";
        private const string STICK_LEFT = "3247";
        private const string STICK_RIGHT = "3248";
        //Clock
        private const string ACHS_LEFT_PUSH = "3249";
        private const string ACHS_LEFT_PULL = "3250";
        private const string ACHS_LEFT_ROTATE = "3251";
        private const string ACHS_RIGHT_PUSH = "3252";
        private const string ACHS_RIGHT_ROTATE = "3253";
        //Dummy
        private const string MISL_MODE_COVER = "3254";
        private const string MISL_MODE_ACT_TRAIN = "3255";
        private const string GSUIT_MAX_MIN = "3256";
        private const string SINA_NR_1_COVER = "3257";
        private const string SINA_NR_1 = "3258";
        private const string SINA_NR_2 = "3259";
        private const string AIRDUCT_TEST_COVER = "3260";
        private const string AIRDUCT_TEST = "3261";
        private const string BU45_BUSTER = "3262";
        private const string SODPBU_1 = "3263";
        private const string SODPBU_2 = "3264";
        private const string EJECT_EJECT_EJECT = "3265";
        private const string EMER_OXY = "3266";
        private const string UK_2_MML = "3267";
        private const string UK_2_MGs_KM = "3268";
        private const string SUIT_VENT = "3269";
        private const string HARNESS = "3270";
        private const string CANOPY_VENT_SYSTEM = "3272";
        private const string HARNESS_LOOSE_TIGHT = "3273";
        private const string RUD_FIXATOR = "3274";
        private const string ALTIMETER_PRESSURE_RESET = "3275";
        private const string FORCE_FEEDBACK = "3276";
        private const string SORC_NIGHT_DAY = "3277";
        #endregion

        public MiG21Interface()
            : base("DCS MiG21")
        {
            #region DragChute
            AddFunction(new PushButton(this, CHUTE, DRAGCHUTE, "298", "Chute", "Release Drop Chute"));
            AddFunction(new PushButton(this, CHUTE, DRAGCHUTE_DISCONNECT, "305", "Chute", "Disconnect Drop Chute"));
            AddFunction(Switch.CreateToggleSwitch(this, CHUTE, DRAGCHUTE_SAFTEY_COVER, "304", "1", "Open", "0", "Closed", "Chute", "Saftey Cover", "%1d"));
            #endregion

            #region Radio
            AddFunction(Switch.CreateToggleSwitch(this, RADIO, RADIO_ON, "173", "1", "On", "0", "Off", "Radio", "Radio On", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, RADIO, RADIO_COMPASS_SOUND, "208", "1", "Radio", "0", "Compass", "Radio", "Radio/Compass Sound", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, RADIO, SQUELCH, "209", "1", "On", "0", "Off", "Radio", "Squelch On/Off", "%1d"));
            //works great with potentiometer
            AddFunction(new Axis(this, RADIO, RADIO_VOLUME, "210", 0.1d, 0d, 1d, "Radio", "Radio Volume"));
            //works better this way instead of axis to get the output
            AddFunction(new Switch(this, RADIO, RADIO_CHANNEL, new SwitchPosition[] { new SwitchPosition("0", "Channel 0", RADIO_CHANNEL), 
                new SwitchPosition("0.05", "Channel 1", RADIO_CHANNEL), new SwitchPosition("0.1", "Channel 2", RADIO_CHANNEL), new SwitchPosition("0.15", "Channel 3", RADIO_CHANNEL), 
                new SwitchPosition("0.2", "Channel 4", RADIO_CHANNEL), new SwitchPosition("0.25", "Channel 5", RADIO_CHANNEL), new SwitchPosition("0.3", "Channel 6", RADIO_CHANNEL), 
                new SwitchPosition("0.35", "Channel 7", RADIO_CHANNEL), new SwitchPosition("0.4", "Channel 8", RADIO_CHANNEL) , new SwitchPosition("0.45", "Channel 9", RADIO_CHANNEL),
                new SwitchPosition("0.5", "Channel 10", RADIO_CHANNEL), new SwitchPosition("0.55", "Channel 11", RADIO_CHANNEL) , new SwitchPosition("0.66", "Channel 12", RADIO_CHANNEL),
                new SwitchPosition("0.65", "Channel 13", RADIO_CHANNEL), new SwitchPosition("0.7", "Channel 14", RADIO_CHANNEL) , new SwitchPosition("0.75", "Channel 15", RADIO_CHANNEL),
                new SwitchPosition("0.8", "Channel 16", RADIO_CHANNEL), new SwitchPosition("0.85", "Channel 17", RADIO_CHANNEL) , new SwitchPosition("0.9", "Channel 18", RADIO_CHANNEL),
                new SwitchPosition("0.95", "Channel 19", RADIO_CHANNEL), new SwitchPosition("1", "Channel 20", RADIO_CHANNEL)}, "Radio", "Radio Channel Selector", "%0.1f"));
            AddFunction(new PushButton(this, INTERCOM, RADIO_INTERCOM, "315", "Radio", "Intercom pushbutton"));
            // potentiometer with initial 0, min0, max 20, step 1 works perfect with it.
            AddFunction(new ScaledNetworkValue(this, "211", 20d, "Radio Channel Display", "Radio Channel output for display", "Current channel", "use potentiometer with initial 0, min0, max 20, step 1", BindingValueUnits.Numeric, 0d, "%.4f"));          
            #endregion

            #region DC & AC buses & giro devices
            AddFunction(Switch.CreateToggleSwitch(this, DC_BUS, BATTERY_ON, "165", "1", "On", "0", "Off", "AC DC Buses", "Battery On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, DC_BUS, BATTERY_HEAT, "155", "1", "On", "0", "Off", "AC DC Buses", "Battery Heat On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, DC_BUS, DC_GENERATOR, "166", "1", "On", "0", "Off", "AC DC Buses", "DC Generator On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AC_BUS, AC_GENERATOR, "169", "1", "On", "0", "Off", "AC DC Buses", "AC Generator On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AC_BUS, PO7501, "153", "1", "On", "0", "Off", "AC DC Buses", "PO-750 Inverter #1 On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AC_BUS, PO7502, "154", "1", "On", "0", "Off", "AC DC Buses", "PO-750 Inverter #2 On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, AC_BUS, EMERGENCY_INVERTER, "164", "1", "On", "0", "Off", "AC DC Buses", "Emergency Inverter On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, GYRO_DEVICES, GIRO_1, "162", "1", "On", "0", "Off", "Giro Devices", "Giro, NPP, SAU, RLS Signal, KPP Power On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, GYRO_DEVICES, GIRO_2, "163", "1", "On", "0", "Off", "Giro Devices", "DA-200 Signal, Giro, NPP, RLS, SAU Power On/Off", "%1d"));
            #endregion

            #region FUEL_PUMPS & FUEL_SYSTEM
            AddFunction(Switch.CreateToggleSwitch(this, FUEL_PUMPS, PUMP_3, "159", "1", "Open", "0", "Closed", "Fuel Tanks 3rd Group", "Fuel Pump On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, FUEL_PUMPS, PUMP_1, "160", "1", "Open", "0", "Closed", "Fuel Tanks 1st Group", "Fuel Pump On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, FUEL_PUMPS, PUMP_RASHOD, "161", "1", "Open", "0", "Closed", "Drain Fuel Tank", "Fuel Pump On/Off", "%1d"));
            //used potentiometer with step value of 0.2 and lower sensitivity (acts same in cockpit its either up or down fully practically)
            //elements["PNT_274"] = default_axis(LOCALIZE("Fuel Quantity Set"),devices.FUEL_SYSTEM, device_commands.FuelQt,274, 0.0, 0.02, true, false)
            AddFunction(new Axis(this, FUEL_SYSTEM, FUEL_QT, "274", 0.1d, 0d, 0.2d, "Fuel System", "Fuel Qt"));
            #endregion

            #region Engine Start Device
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE_START_DEVICE, ZAZIG, "302", "1", "Open", "0", "Closed", "Engine Start Device", "APU On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE_START_DEVICE, HOTSTART, "288", "1", "Open", "0", "Closed", "Engine Start Device", "Engine Cold / Normal Start", "%1d"));
            AddFunction(new PushButton(this, ENGINE_START_DEVICE, START_BUTTON, "289", "Engine Start Device", "Start Engine Button"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE_START_DEVICE, AIRSTART, "301", "1", "Open", "0", "Closed", "Engine Start Device", "Engine Emergency Air Start on/off", "%1d"));
            AddFunction(new PushButton(this, ENGINE_START_DEVICE, RUD_STOP_LOCK, "616", "Engine Start Device", "Engine Stop/Lock"));
            #endregion

            #region Pitot Tubes
            AddFunction(Switch.CreateToggleSwitch(this, PITOT_TUBES, PITO_SELECT, "229", "1", "Open", "0", "Closed", "Pitot Tubes", "Pitot tube Selector Main/Emergency", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, PITOT_TUBES, PITO_HEAT_MAIN, "279", "1", "Open", "0", "Closed", "Pitot Tubes", "Pitot tube/Periscope/Clock Heat", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, PITOT_TUBES, PITOT_HEAT_AUX, "280", "1", "Open", "0", "Closed", "Pitot Tubes", "Secondary Pitot Tube Heat", "%1d"));
            #endregion

            #region DA200 (Variometer)
            AddFunction(new Axis(this, DA_200, DA200_SET, "261", 0.1d, 0d, 1d, "DA200", "DA200 Set")); //perfect with potentiometer
            #endregion

            #region Engine
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, SURGE_DOORS, "308", "0", "Open", "1", "Closed", "Engine", "Anti surge doors - Auto/Manual", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, FORS_MAX, "300", "1", "Open", "0", "Closed", "Engine", "Afterburner/Maximum Off/On", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ENGINE, CHR, "320", "1", "Open", "0", "Closed", "Engine", "Emergency Afterburner Off/On", "%1d"));
            #endregion

            #region Fire Extinguisher
            AddFunction(Switch.CreateToggleSwitch(this, FIRE_EXTINGUISHER, POZAR_OBORUD, "303", "1", "Open", "0", "Closed", "Fire Extinguisher", "Fire Extinguisher Off/On", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, FIRE_EXTINGUISHER, POZAR_SAFETY_COVER, "324", "1", "Open", "0", "Closed", "Fire Extinguisher", "Fire Extinguisher Cover", "%1d"));
            AddFunction(new PushButton(this, FIRE_EXTINGUISHER, OGNETUSHITEL, "325", "Fire Extinguisher", "Fire Extinguisher Button"));
            #endregion

            #region Lights
            AddFunction(new Axis(this, LIGHTS, TEXT_BACKLIGHT_AXIS, "612", 0.1d, 0d, 1d, "Lights", "Cockpit Texts Back-light"));
            AddFunction(new Axis(this, LIGHTS, INSTRUMENTS_BACKLIGHT_AXIS, "156", 0.1d, 0d, 1d, "Lights", "Instruments Back-light"));
            AddFunction(new Axis(this, LIGHTS, RED_LIGHTS_MAIN_AXIS, "157", 0.1d, 0d, 1d, "Lights", "Main Red Lights"));
            AddFunction(new Axis(this, LIGHTS, WHITE_LIGHTS_MAIN_AXIS, "222", 0.1d, 0d, 1d, "Lights", "Main White Lights"));
            AddFunction(new Switch(this, LIGHTS, NAV_LIGHTS, new SwitchPosition[] { new SwitchPosition("0.00", "Off ", NAV_LIGHTS),
                new SwitchPosition("0.33", "Min", NAV_LIGHTS), new SwitchPosition("0.66", "Med", NAV_LIGHTS),new SwitchPosition("0.99", "Max", NAV_LIGHTS)},
                "Lights", "Navlights", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, LIGHTS, LANDING_LIGHTS, "323", "1.0", "Off", "0.5", "Taxi", "0.0", "Land", "Lights", "Off/Taxi/Land Lights", "%0.1f"));
            #endregion

            #region Lights Warning
            //both push button and axis work perfect. Its a rotary with a pushbutton in game.
            AddFunction(new PushButton(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS10, "369", "Lights Warning", "Check Warning Lights 195"));
            AddFunction(new Axis(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS11, "195", 0.1d, 0d, 1d, "Lights Warning", "Set  Warning Lights 195"));
            AddFunction(new PushButton(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS20, "370", "Lights Warning", "Check Warning Lights 196"));
            AddFunction(new Axis(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS21, "196", 0.1d, 0d, 1d, "Lights Warning", "Set Warning Lights 196"));
            AddFunction(new PushButton(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS30, "371", "Lights Warning", "Check Warning Lights 273"));
            AddFunction(new Axis(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS31, "273", 0.1d, 0d, 1d, "Lights Warning", "Set Warning Lights 273"));
            AddFunction(new PushButton(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS40, "372", "Lights Warning", "Check Warning Lights 282"));
            AddFunction(new Axis(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS41, "282", 0.1d, 0d, 1d, "Lights Warning", "Set Warning Lights 282"));
            AddFunction(new PushButton(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS50, "373", "Lights Warning", "Check Warning Lights 283"));
            AddFunction(new Axis(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS51, "283", 0.1d, 0d, 1d, "Lights Warning", "Set Warning Lights 283"));
            AddFunction(new PushButton(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS60, "374", "Lights Warning", "Check Warning Lights 322"));
            AddFunction(new Axis(this, LIGHTS_WARNING, CHECK_WARNING_LIGHTS61, "322", 0.1d, 0d, 1d, "Lights Warning", "Set Warning Lights 322"));
            AddFunction(new PushButton(this, LIGHTS_WARNING, SORC, "255", "Lights Warning", "Sorc Check Warning Lights "));
            AddFunction(new Axis(this, LIGHTS_WARNING, SORC_NIGHT_DAY, "657", 0.1d, 0d, 1d, "Sorc", "Sorc Set Night Day Warning Lights "));
            #endregion

            #region ARK
            AddFunction(Switch.CreateToggleSwitch(this, ARK, ARK_ON, "174", "1", "Open", "0", "Closed", "ARK", "ARK On/Off", "%1d"));
            AddFunction(new Axis(this, ARK, ARK_SOUND, "198", 0.1d, 0d, 1d, "ARK", "ARK Sound"));
            AddFunction(new PushButton(this, ARK, ARK_PEREK_LUCENIE, "212", "ARK", "ARK Change"));
            AddFunction(new PushButton(this, ARK, ARK_CHANNEL, "213", "ARK", "ARK 1", "0.1", "0.1", "0.1d"));
            AddFunction(new PushButton(this, ARK, ARK_CHANNEL, "214", "ARK", "ARK 2", "0.2", "0.2", "0.1d"));
            AddFunction(new PushButton(this, ARK, ARK_CHANNEL, "215", "ARK", "ARK 3", "0.3", "0.3", "0.1d"));
            AddFunction(new PushButton(this, ARK, ARK_CHANNEL, "216", "ARK", "ARK 4", "0.4", "0.4", "0.1d"));
            AddFunction(new PushButton(this, ARK, ARK_CHANNEL, "217", "ARK", "ARK 5", "0.5", "0.5", "0.1d"));
            AddFunction(new PushButton(this, ARK, ARK_CHANNEL, "218", "ARK", "ARK 6", "0.6", "0.6", "0.1d"));
            AddFunction(new PushButton(this, ARK, ARK_CHANNEL, "219", "ARK", "ARK 7", "0.7", "0.7", "0.1d"));
            AddFunction(new PushButton(this, ARK, ARK_CHANNEL, "220", "ARK", "ARK 8", "0.8", "0.8", "0.1d"));
            AddFunction(new PushButton(this, ARK, ARK_CHANNEL, "221", "ARK", "ARK 9", "0.9", "0.9", "0.1d"));
            //perfect with rotary switch. could also use a potentiometer as long as in profile you set min ie 0 max 8 and step 1.. might be handy for 100 position later
            AddFunction(new Switch(this, ARK, ARK_ZONE, new SwitchPosition[] { new SwitchPosition("0.0", "1 1", ARK_ZONE), new SwitchPosition("0.14", "1 11", ARK_ZONE), 
                new SwitchPosition("0.28", "2 1", ARK_ZONE), new SwitchPosition("0.42", "2 11", ARK_ZONE), new SwitchPosition("0.56", "3 1", ARK_ZONE), 
                new SwitchPosition("0.70", "3 11", ARK_ZONE), new SwitchPosition("0.84", "4 1", ARK_ZONE), new SwitchPosition("0.98", "4 11", ARK_ZONE) 
                }, "ARK", "Ark Zone Selector", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, ARK, ARK_ANTENA_COMPASS, "197", "1", "Open", "0", "Closed", "ARK", "ARK Mode - Antenna / Compass", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ARK, ARK_FAR_NEAR, "254", "1", "Open", "0", "Closed", "ARK", "Marker Far/Near", "%1d"));
            #endregion

            #region RSBN
            AddFunction(Switch.CreateToggleSwitch(this, RSBN, RSBN_ON, "176", "1", "Open", "0", "Closed", "RSBN", "RSBN On/Off", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, RSBN, RSBN_MODE, "240", "1.0", "Land", "0.5", "Navigation", "0.0", "Descend", "RSBN", "Land/Navigation/Descend", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, RSBN, RSBN_ARK, "340", "1", "Open", "0", "Closed", "RSBN", "RSBN / ARK", "%1d"));
            AddFunction(new PushButton(this, RSBN, RSBN_IDENT, "294", "RSBN", "RSBN Identify"));
            AddFunction(new PushButton(this, RSBN, RSBN_TEST, "347", "RSBN", "RSBN Test"));
            #endregion

            #region RSBN Panel
            AddFunction(new Axis(this, RSBN, RSBN_SOUND, "345", 0.1d, 0d, 1d, "RSBN", "RSBN Sound"));
            //use potentiometer with 100 position min1 max99 step1. 0-100 below. Works a treat for both below
            AddFunction(new Switch(this, RSBN, RSBN_NAV, new SwitchPosition[] { new SwitchPosition("0.00", "0", RSBN_NAV)
                ,new SwitchPosition("0.01", "1", RSBN_NAV), new SwitchPosition("0.02", "2", RSBN_NAV), new SwitchPosition("0.03", "3", RSBN_NAV)
                ,new SwitchPosition("0.04", "4", RSBN_NAV), new SwitchPosition("0.05", "5", RSBN_NAV), new SwitchPosition("0.06", "6", RSBN_NAV)
                ,new SwitchPosition("0.07", "7", RSBN_NAV), new SwitchPosition("0.08", "8", RSBN_NAV), new SwitchPosition("0.09", "9", RSBN_NAV)
                ,new SwitchPosition("0.10", "10", RSBN_NAV), new SwitchPosition("0.11", "11", RSBN_NAV), new SwitchPosition("0.12", "12", RSBN_NAV)
                ,new SwitchPosition("0.13", "13", RSBN_NAV), new SwitchPosition("0.14", "14", RSBN_NAV), new SwitchPosition("0.15", "15", RSBN_NAV)            
                ,new SwitchPosition("0.16", "16", RSBN_NAV), new SwitchPosition("0.17", "17", RSBN_NAV), new SwitchPosition("0.18", "18", RSBN_NAV)            
                ,new SwitchPosition("0.19", "19", RSBN_NAV), new SwitchPosition("0.20", "20", RSBN_NAV), new SwitchPosition("0.21", "21", RSBN_NAV)            
                ,new SwitchPosition("0.22", "22", RSBN_NAV), new SwitchPosition("0.23", "23", RSBN_NAV), new SwitchPosition("0.24", "24", RSBN_NAV)             
                ,new SwitchPosition("0.25", "25", RSBN_NAV), new SwitchPosition("0.26", "26", RSBN_NAV), new SwitchPosition("0.27", "27", RSBN_NAV)
                ,new SwitchPosition("0.28", "28", RSBN_NAV), new SwitchPosition("0.29", "29", RSBN_NAV), new SwitchPosition("0.30", "30", RSBN_NAV)
                ,new SwitchPosition("0.31", "31", RSBN_NAV), new SwitchPosition("0.32", "32", RSBN_NAV), new SwitchPosition("0.33", "33", RSBN_NAV)
                ,new SwitchPosition("0.34", "34", RSBN_NAV), new SwitchPosition("0.35", "35", RSBN_NAV), new SwitchPosition("0.36", "36", RSBN_NAV)
                ,new SwitchPosition("0.37", "37", RSBN_NAV), new SwitchPosition("0.38", "38", RSBN_NAV), new SwitchPosition("0.39", "39", RSBN_NAV)
                ,new SwitchPosition("0.40", "40", RSBN_NAV), new SwitchPosition("0.41", "41", RSBN_NAV), new SwitchPosition("0.42", "42", RSBN_NAV)
                ,new SwitchPosition("0.43", "43", RSBN_NAV), new SwitchPosition("0.44", "44", RSBN_NAV), new SwitchPosition("0.45", "45", RSBN_NAV)
                ,new SwitchPosition("0.46", "46", RSBN_NAV), new SwitchPosition("0.47", "47", RSBN_NAV), new SwitchPosition("0.48", "48", RSBN_NAV)
                ,new SwitchPosition("0.49", "49", RSBN_NAV), new SwitchPosition("0.50", "50", RSBN_NAV), new SwitchPosition("0.51", "51", RSBN_NAV)
                ,new SwitchPosition("0.52", "52", RSBN_NAV), new SwitchPosition("0.53", "53", RSBN_NAV), new SwitchPosition("0.54", "54", RSBN_NAV)
                ,new SwitchPosition("0.55", "55", RSBN_NAV), new SwitchPosition("0.56", "56", RSBN_NAV), new SwitchPosition("0.57", "57", RSBN_NAV)
                ,new SwitchPosition("0.58", "58", RSBN_NAV), new SwitchPosition("0.59", "59", RSBN_NAV), new SwitchPosition("0.60", "60", RSBN_NAV)
                ,new SwitchPosition("0.61", "61", RSBN_NAV), new SwitchPosition("0.62", "62", RSBN_NAV), new SwitchPosition("0.63", "63", RSBN_NAV)
                ,new SwitchPosition("0.64", "64", RSBN_NAV), new SwitchPosition("0.65", "65", RSBN_NAV), new SwitchPosition("0.66", "66", RSBN_NAV)
                ,new SwitchPosition("0.67", "67", RSBN_NAV), new SwitchPosition("0.68", "68", RSBN_NAV), new SwitchPosition("0.69", "69", RSBN_NAV)
                ,new SwitchPosition("0.70", "70", RSBN_NAV), new SwitchPosition("0.71", "71", RSBN_NAV), new SwitchPosition("0.72", "72", RSBN_NAV)
                ,new SwitchPosition("0.73", "73", RSBN_NAV), new SwitchPosition("0.74", "74", RSBN_NAV), new SwitchPosition("0.75", "75", RSBN_NAV)
                ,new SwitchPosition("0.76", "76", RSBN_NAV), new SwitchPosition("0.77", "77", RSBN_NAV), new SwitchPosition("0.78", "78", RSBN_NAV)
                ,new SwitchPosition("0.79", "79", RSBN_NAV), new SwitchPosition("0.80", "80", RSBN_NAV), new SwitchPosition("0.81", "81", RSBN_NAV)
                ,new SwitchPosition("0.82", "82", RSBN_NAV), new SwitchPosition("0.83", "83", RSBN_NAV), new SwitchPosition("0.84", "84", RSBN_NAV)
                ,new SwitchPosition("0.85", "85", RSBN_NAV), new SwitchPosition("0.86", "86", RSBN_NAV), new SwitchPosition("0.87", "87", RSBN_NAV)
                ,new SwitchPosition("0.88", "88", RSBN_NAV), new SwitchPosition("0.89", "89", RSBN_NAV), new SwitchPosition("0.90", "90", RSBN_NAV)
                ,new SwitchPosition("0.91", "91", RSBN_NAV), new SwitchPosition("0.92", "92", RSBN_NAV), new SwitchPosition("0.93", "93", RSBN_NAV)
                ,new SwitchPosition("0.94", "94", RSBN_NAV), new SwitchPosition("0.95", "95", RSBN_NAV), new SwitchPosition("0.96", "96", RSBN_NAV)
                ,new SwitchPosition("0.97", "97", RSBN_NAV), new SwitchPosition("0.98", "98", RSBN_NAV), new SwitchPosition("0.99", "99", RSBN_NAV)
                }, "RSBN", "RSBN NAV", "%0.01f"));
            //The following Axis is now crashing the game... need to investigate, remove from pull until fixed
            //RSBN_LAND value, as soon as you use it game crashes now... need to investigate
            //tried axis but RSBN_LAND value still used and does the same thing.
            //AddFunction(new Switch(this, RSBN, RSBN_LAND, new SwitchPosition[] { new SwitchPosition("0.00", "0", RSBN_LAND)
            //    ,new SwitchPosition("0.01", "1", RSBN_LAND), new SwitchPosition("0.02", "2", RSBN_LAND), new SwitchPosition("0.03", "3", RSBN_LAND)
            //    ,new SwitchPosition("0.04", "4", RSBN_LAND), new SwitchPosition("0.05", "5", RSBN_LAND), new SwitchPosition("0.06", "6", RSBN_LAND)
            //    ,new SwitchPosition("0.07", "7", RSBN_LAND), new SwitchPosition("0.08", "8", RSBN_LAND), new SwitchPosition("0.09", "9", RSBN_LAND)
            //    ,new SwitchPosition("0.10", "10", RSBN_LAND), new SwitchPosition("0.11", "11", RSBN_LAND), new SwitchPosition("0.12", "12", RSBN_LAND)
            //    ,new SwitchPosition("0.13", "13", RSBN_LAND), new SwitchPosition("0.14", "14", RSBN_LAND), new SwitchPosition("0.15", "15", RSBN_LAND)            
            //    ,new SwitchPosition("0.16", "16", RSBN_LAND), new SwitchPosition("0.17", "17", RSBN_LAND), new SwitchPosition("0.18", "18", RSBN_LAND)            
            //    ,new SwitchPosition("0.19", "19", RSBN_LAND), new SwitchPosition("0.20", "20", RSBN_LAND), new SwitchPosition("0.21", "21", RSBN_LAND)            
            //    ,new SwitchPosition("0.22", "22", RSBN_LAND), new SwitchPosition("0.23", "23", RSBN_LAND), new SwitchPosition("0.24", "24", RSBN_LAND)             
            //    ,new SwitchPosition("0.25", "25", RSBN_LAND), new SwitchPosition("0.26", "26", RSBN_LAND), new SwitchPosition("0.27", "27", RSBN_LAND)
            //    ,new SwitchPosition("0.28", "28", RSBN_LAND), new SwitchPosition("0.29", "29", RSBN_LAND), new SwitchPosition("0.30", "30", RSBN_LAND)
            //    ,new SwitchPosition("0.31", "31", RSBN_LAND), new SwitchPosition("0.32", "32", RSBN_LAND), new SwitchPosition("0.33", "33", RSBN_LAND)
            //    ,new SwitchPosition("0.34", "34", RSBN_LAND), new SwitchPosition("0.35", "35", RSBN_LAND), new SwitchPosition("0.36", "36", RSBN_LAND)
            //    ,new SwitchPosition("0.37", "37", RSBN_LAND), new SwitchPosition("0.38", "38", RSBN_LAND), new SwitchPosition("0.39", "39", RSBN_LAND)
            //    ,new SwitchPosition("0.40", "40", RSBN_LAND), new SwitchPosition("0.41", "41", RSBN_LAND), new SwitchPosition("0.42", "42", RSBN_LAND)
            //    ,new SwitchPosition("0.43", "43", RSBN_LAND), new SwitchPosition("0.44", "44", RSBN_LAND), new SwitchPosition("0.45", "45", RSBN_LAND)
            //    ,new SwitchPosition("0.46", "46", RSBN_LAND), new SwitchPosition("0.47", "47", RSBN_LAND), new SwitchPosition("0.48", "48", RSBN_LAND)
            //    ,new SwitchPosition("0.49", "49", RSBN_LAND), new SwitchPosition("0.50", "50", RSBN_LAND), new SwitchPosition("0.51", "51", RSBN_LAND)
            //    ,new SwitchPosition("0.52", "52", RSBN_LAND), new SwitchPosition("0.53", "53", RSBN_LAND), new SwitchPosition("0.54", "54", RSBN_LAND)
            //    ,new SwitchPosition("0.55", "55", RSBN_LAND), new SwitchPosition("0.56", "56", RSBN_LAND), new SwitchPosition("0.57", "57", RSBN_LAND)
            //    ,new SwitchPosition("0.58", "58", RSBN_LAND), new SwitchPosition("0.59", "59", RSBN_LAND), new SwitchPosition("0.60", "60", RSBN_LAND)
            //    ,new SwitchPosition("0.61", "61", RSBN_LAND), new SwitchPosition("0.62", "62", RSBN_LAND), new SwitchPosition("0.63", "63", RSBN_LAND)
            //    ,new SwitchPosition("0.64", "64", RSBN_LAND), new SwitchPosition("0.65", "65", RSBN_LAND), new SwitchPosition("0.66", "66", RSBN_LAND)
            //    ,new SwitchPosition("0.67", "67", RSBN_LAND), new SwitchPosition("0.68", "68", RSBN_LAND), new SwitchPosition("0.69", "69", RSBN_LAND)
            //    ,new SwitchPosition("0.70", "70", RSBN_LAND), new SwitchPosition("0.71", "71", RSBN_LAND), new SwitchPosition("0.72", "72", RSBN_LAND)
            //    ,new SwitchPosition("0.73", "73", RSBN_LAND), new SwitchPosition("0.74", "74", RSBN_LAND), new SwitchPosition("0.75", "75", RSBN_LAND)
            //    ,new SwitchPosition("0.76", "76", RSBN_LAND), new SwitchPosition("0.77", "77", RSBN_LAND), new SwitchPosition("0.78", "78", RSBN_LAND)
            //    ,new SwitchPosition("0.79", "79", RSBN_LAND), new SwitchPosition("0.80", "80", RSBN_LAND), new SwitchPosition("0.81", "81", RSBN_LAND)
            //    ,new SwitchPosition("0.82", "82", RSBN_LAND), new SwitchPosition("0.83", "83", RSBN_LAND), new SwitchPosition("0.84", "84", RSBN_LAND)
            //    ,new SwitchPosition("0.85", "85", RSBN_LAND), new SwitchPosition("0.86", "86", RSBN_LAND), new SwitchPosition("0.87", "87", RSBN_LAND)
            //    ,new SwitchPosition("0.88", "88", RSBN_LAND), new SwitchPosition("0.89", "89", RSBN_LAND), new SwitchPosition("0.90", "90", RSBN_LAND)
            //    ,new SwitchPosition("0.91", "91", RSBN_LAND), new SwitchPosition("0.92", "92", RSBN_LAND), new SwitchPosition("0.93", "93", RSBN_LAND)
            //    ,new SwitchPosition("0.94", "94", RSBN_LAND), new SwitchPosition("0.95", "95", RSBN_LAND), new SwitchPosition("0.96", "96", RSBN_LAND)
            //    ,new SwitchPosition("0.97", "97", RSBN_LAND), new SwitchPosition("0.98", "98", RSBN_LAND), new SwitchPosition("0.99", "99", RSBN_LAND)
            //    }, "RSBN", "RSBN PRMG LAND", "%0.01f"));
            AddFunction(new PushButton(this, RSBN, RSBN_RESET, "366", "RSBN", "RSBN Reset"));
            AddFunction(Switch.CreateToggleSwitch(this, RSBN, RSBN_BEARING, "367", "1", "Open", "0", "Closed", "RSBN", "RSBN Bearing", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, RSBN, RSBN_FAR, "368", "1", "Open", "0", "Closed", "RSBN", "RSBN Distance", "%1d"));
            #endregion

            #region SAU
            AddFunction(Switch.CreateToggleSwitch(this, SAU, SAU_ON, "179", "1", "Open", "0", "Closed", "SAU", "SAU on/off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, SAU, SAU_PITCH_ON, "180", "1", "Open", "0", "Closed", "SAU", "SAU Pitch on/off", "%1d"));
            AddFunction(new PushButton(this, SAU, SAU_STABIL, "343", "SAU", "SAU - Stabilize"));
            AddFunction(new PushButton(this, SAU, SAU_CANCEL, "376", "SAU", "SAU - cancel current mode"));
            AddFunction(new PushButton(this, SAU, SAU_PRIVEDENIE, "377", "SAU", "SAU - Recovery - stick"));
            AddFunction(Switch.CreateToggleSwitch(this, SAU, SAU_LOW_ALT_ON, "344", "1", "Open", "0", "Closed", "SAU", "SAU Preset - Limit Altitude", "%1d"));
            AddFunction(new PushButton(this, SAU, SAU_LANDING_CONTROL_COMMAND, "341", "SAU", "SAU - Landing - Command"));
            AddFunction(new PushButton(this, SAU, SAU_LANDING_CONTROL_AUTO, "342", "SAU", "SAU - Landing - Auto"));
            AddFunction(new PushButton(this, SAU, SAU_RESET_OFF, "348", "SAU", "SAU Reset/Off"));
            #endregion

            #region SPO
            AddFunction(Switch.CreateToggleSwitch(this, SPO, SPO_ON, "202", "1", "Open", "0", "Closed", "SPO", "SPO-10 RWR On/Off", "%1d"));
            AddFunction(new PushButton(this, SPO, SPO_TEST, "226", "SPO", "SPO-10 Test"));
            AddFunction(Switch.CreateToggleSwitch(this, SPO, SPO_DAY_NIGHT, "227", "1", "Open", "0", "Closed", "SPO", "SPO-10 Night / Day", "%1d"));
            AddFunction(new Axis(this, SPO, SPO_VOLUME, "225", 0.1d, 0d, 1d, "SPO", "SPO-10 Volume"));
            #endregion

            #region SRZO IFF (not implemented in game)
            //all of these recieve but dont send into cockpit.... though not implemented in game i did hope it would switch them..
            //will look into more closely if Leatherneck implement it in game.
            //AddFunction(Switch.CreateToggleSwitch(this, SRZO, SRZO_VOPROS, "188", "1", "Open", "0", "Closed", "SRZO", "SRZO IFF Coder/Decoder On/Off", "%1d"));
            //AddFunction(new Switch(this, SRZO, SRZO_CODES, new SwitchPosition[] { new SwitchPosition("0.00", "0", SRZO_CODES)
            //    ,new SwitchPosition("0.08", "1", SRZO_CODES), new SwitchPosition("0.16", "2", SRZO_CODES), new SwitchPosition("0.24", "3", SRZO_CODES)
            //    ,new SwitchPosition("0.32", "4", SRZO_CODES), new SwitchPosition("0.4", "5", SRZO_CODES), new SwitchPosition("0.48", "6", SRZO_CODES)
            //    ,new SwitchPosition("0.56", "7", SRZO_CODES), new SwitchPosition("0.64", "8", SRZO_CODES), new SwitchPosition("0.72", "9", SRZO_CODES)
            //    ,new SwitchPosition("0.8", "10", SRZO_CODES), new SwitchPosition("0.88", "11", SRZO_CODES)
            //     }, "SRZO", "SRZO Codes", "%0.01f"));        
            //AddFunction(Switch.CreateToggleSwitch(this, SRZO, SRZO_ON, "346", "1", "Open", "0", "Closed", "SRZO", "IFF System 'Type 81' On/Off", "%1d"));
            //AddFunction(Switch.CreateToggleSwitch(this, SRZO, EMERG_TRANSMIT_SAFETY_COVER, "190", "1", "Open", "0", "Closed", "SRZO", "Emergency Transmitter Cover", "%1d"));
            //AddFunction(Switch.CreateToggleSwitch(this, SRZO, EMERG_TRANSMIT_ON, "191", "1", "Open", "0", "Closed", "SRZO", "Emergency Transmitter On/Off", "%1d"));
            //AddFunction(Switch.CreateToggleSwitch(this, SRZO, SRZO_DESTR_COVER, "427", "1", "Open", "0", "Closed", "SRZO", "SRZO Self Destruct Cover", "%1d"));
            //AddFunction(new PushButton(this, SRZO, SRZO_DESTR, "428", "SRZO", "SRZO Self Destruct"));
            #endregion

            #region SOD
            AddFunction(Switch.CreateToggleSwitch(this, SOD, SOD_ON, "200", "1", "Open", "0", "Closed", "SOD", "SOD IFF On/Off", "%1d"));
            AddFunction(new PushButton(this, SOD, SOD_IDENT, "199", "SOD", "SOD Identify"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SOD, SOD_VOLNI, "201", "1.0", "3", "0.5", "1", "0.0", "2", "SOD", "Wave Selector 3/1/2", "%0.1f"));
            AddFunction(new Switch(this, SOD, SOD_MODE, new SwitchPosition[] { new SwitchPosition("0.00", "Fine", SOD_MODE)
                ,new SwitchPosition("0.25", "Course", SOD_MODE), new SwitchPosition("0.50", "Off", SOD_MODE), new SwitchPosition("0.75", "Single", SOD_MODE)
                , new SwitchPosition("1.0", "Group", SOD_MODE)   
                }, "SOD", "SOD Modes", "%0.01f"));
            #endregion

            #region RADAR
            AddFunction(Switch.CreateThreeWaySwitch(this, RADAR, RADAR_ON, "205", "1.0", "Off", "0.5", "Prep", "0.0", "On", "RADAR", "Radar Off/Prep/On", "%0.1f"));
            AddFunction(Switch.CreateThreeWaySwitch(this, RADAR, RADAR_LOW_ALT, "206", "1.0", "Off", "0.5", "Comp", "0.0", "On", "RADAR", "Radar Off/Comp/on", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, RADAR, RADAR_FIX_BEAM, "207", "1", "Open", "0", "Closed", "RADAR", "Locked Beam On/Off", "%1d"));
            AddFunction(new PushButton(this, RADAR, RADAR_MGN_STIR, "266", "RADAR", "Radar Screen Magnetic Reset"));
            AddFunction(new PushButton(this, RADAR, RADAR_JAM_CONT, "330", "RADAR", "Radar Interferes - Continues"));
            AddFunction(new PushButton(this, RADAR, RADAR_JAM_TMP, "331", "RADAR", "Radar Interferes - Temporary"));
            AddFunction(new PushButton(this, RADAR, RADAR_JAM_PASS, "332", "RADAR", "Radar Interferes - Passive"));
            AddFunction(new PushButton(this, RADAR, RADAR_JAM_METEO, "333", "RADAR", "Radar Interferes - Weather"));
            AddFunction(new PushButton(this, RADAR, RADAR_VOPROS, "334", "RADAR", "Radar Interferes - IFF"));
            AddFunction(new PushButton(this, RADAR, RADAR_JAM_LOW_SPEED, "335", "RADAR", "Radar Interferes - Low Speed"));
            AddFunction(new PushButton(this, RADAR, RADAR_KONTROL, "336", "RADAR", "Radar Interferes - Self-test"));
            AddFunction(new PushButton(this, RADAR, RADAR_RESET, "337", "RADAR", "Radar Interferes - Reset"));
            AddFunction(new PushButton(this, RADAR, ZAHVAT, "378", "RADAR", "Lock Target - stick"));
            AddFunction(new Axis(this, RADAR, RADAR_FILTER, "623", 0.1d, 0d, 1d, "RADAR", "Radar Filter"));
            #endregion

            #region SPRD
            AddFunction(Switch.CreateToggleSwitch(this, SPRD, SPRD_START_ON, "167", "1", "Open", "0", "Closed", "SPRD", "SPRD (RATO) System On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, SPRD, SPRD_DROP_ON, "168", "1", "Open", "0", "Closed", "SPRD", "SPRD (RATO) Drop System On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, SPRD, SPRD_START_SAFETY_COVER, "252", "1", "Open", "0", "Closed", "SPRD", "SPRD (RATO) Start Cover", "%1d"));
            AddFunction(new PushButton(this, SPRD, SPRD_START, "253", "SPRD", "SPRD (RATO) Start"));
            AddFunction(Switch.CreateToggleSwitch(this, SPRD, SPRD_DROP_SAFETY_COVER, "317", "1", "Open", "0", "Closed", "SPRD", "SPRD (RATO)t Drop Cover", "%1d"));
            AddFunction(new PushButton(this, SPRD, SPRD_DROP, "318", "SPRD", "SPRD (RATO) Drop"));
            #endregion

            #region SPS
            AddFunction(Switch.CreateToggleSwitch(this, SPS, SPS_ON, "293", "1", "Open", "0", "Closed", "SPS", "SPS System Off/On", "%1d"));
            #endregion

            #region ARU
            AddFunction(Switch.CreateToggleSwitch(this, ARU, ARU_MAN_AUTO, "295", "1", "Open", "0", "Closed", "ARU", "ARU System - Manual/Auto", "%1d"));
            //the spring loaded part is taken care of in the profile editor.2 device commands...
            //just using high speed works perfect, dont need the low speed part
            //elements["PNT_296"]	= default_springloaded_3pos_switch(LOCALIZE("ARU System - Low Speed/Neutral/High Speed"), devices.ARU, device_commands.ARUhighSpeed, device_commands.ARUlowSpeed, 1, 0, -1, 296)				 
            AddFunction(Switch.CreateThreeWaySwitch(this, ARU, ARU_HIGH_SPEED, "296", "-1.0", "Low", "0.0", "Neutral", "1.0", "High", "ARU", "Low/Neutral/High hs", "%0.1f"));
            #endregion

            #region Airbrake
            AddFunction(Switch.CreateToggleSwitch(this, AIRBRAKE, AIRBRAKES, "316", "1", "Open", "0", "Closed", "Airbrake", "Airbrakes", "%1d"));
            #endregion

            #region GearBrakes
            AddFunction(Switch.CreateToggleSwitch(this, GEAR_BRAKES, ABS_ON, "299", "1", "Open", "0", "Closed", "Gear Brakes", "ABS off/on", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, GEAR_BRAKES, NOSEGEAR_BRAKE, "238", "1", "Open", "0", "Closed", "Gear Brakes", "Nosegear Brake Off/On", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, GEAR_BRAKES, EMERG_BRAKES, "237", "0", "Closed", "1", "Open", "Gear Brakes", "Emergency brake", "%1d"));
            #endregion

            #region Gears
            AddFunction(Switch.CreateToggleSwitch(this, GEARS, GEAR_HANDLE_FIXATOR, "326", "1", "Open", "0", "Closed", "Gears", "Gear Handle Fixator", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, GEARS, GEAR_LEVER, "327", "1.0", "Up", "0.0", "Neutral", "-1.0", "Down", "Gears", "Up/Neutral/Down", "%0.1f"));
            AddFunction(new Axis(this, GEARS, EMERG_GEARS_MAIN, "223", 0.1d, 0d, 1d, "Gears", "Emergency Gears Main"));
            AddFunction(Switch.CreateToggleSwitch(this, GEARS, EMERG_GEARS_NOSE, "281", "0", "Open", "1", "Closed", "Gears", "Nose Gear Emergency Release Handle", "%1d"));
            #endregion

            #region Flaps
            AddFunction(Switch.CreateToggleSwitch(this, FLAPS, FLAPS_0, "311", "0", "Open", "1", "Closed", "Flaps", "Flaps Neutral", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, FLAPS, FLAPS_25, "312", "0", "Open", "1", "Closed", "Flaps", "Flaps Take-Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, FLAPS, FLAPS_45, "313", "0", "Open", "1", "Closed", "Flaps", "Flaps Landing", "%1d"));
            AddFunction(new PushButton(this, FLAPS, FLAPS_RESET, "314", "Flaps", "Flaps Reset"));
            #endregion

            #region TRIMER
            AddFunction(Switch.CreateToggleSwitch(this, TRIMER, TRIMMER_ON, "172", "1", "On", "0", "Off", "Trimmer", "Trimmer On/Off", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, TRIMER, TRIMMER_BTN_UP, "379", "1.0", "Up", "0.0", "Neutral", "-1.0", "Down", "Trimmer", "Trimmer Up/Neutral/Down", "%0.1f"));
            #endregion

            #region SOPLO aka engine nozzle
            AddFunction(Switch.CreateToggleSwitch(this, SOPLO, SOPLO2X_POZ, "291", "1", "Open", "0", "Closed", "Engine Nozzle", "2 Position Emergency Control", "%1d"));
            #endregion

            #region MAIN_HYDRO and BUSTER_HYDRO
            AddFunction(Switch.CreateToggleSwitch(this, MAIN_HYDRO, NR27_ON, "171", "1", "On", "0", "Off", "Hydro", "Emergency Hydraulic Pump On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, MAIN_HYDRO, AILERON_BOOSTERS, "319", "1", "On", "0", "Off", "Hydro", "Aileron Booster - Off/On", "%1d"));
            #endregion

            #region KPP
            AddFunction(Switch.CreateToggleSwitch(this, KPP, KPP_ON, "177", "1", "On", "0", "Off", "KPP", "KPP Main/Emergency", "%1d"));
            AddFunction(new PushButton(this, KPP, KPP_ARRETIR, "259", "KPP", "KPP Cage"));
            AddFunction(new Axis(this, KPP, KPP_SET_PITCH, "260", 0.01d, -1d, 1d, "KPP", "KPP set")); //works
            #endregion

            #region IAS / TAS / KSI (NPP)
            AddFunction(Switch.CreateToggleSwitch(this, KSI, NPP_ON, "178", "1", "On", "0", "Off", "NPP", "NPP On/Off", "%1d"));
            AddFunction(new PushButton(this, KSI, NPP_NASTROJKA, "258", "NPP", "NPP Adjust"));
            //use rotary encoder in profile
            AddFunction(new Axis(this, KSI, NPP_SET_COURSE, "263", 0.1d, -1d, 1d, "NPP", "NPP Course set"));
            #endregion

            #region RADIO ALTIMETER
            AddFunction(Switch.CreateToggleSwitch(this, RADIO_ALTIMETER, RADIO_ALT_ON, "175", "1", "On", "0", "Off", "Radio Altimeter", "Radio Altimeter/Marker On/Off", "%1d"));
            AddFunction(new Switch(this, RADIO_ALTIMETER, DANGER_ALT_SELECT, new SwitchPosition[] { new SwitchPosition("0.00", "Off", DANGER_ALT_SELECT)
                ,new SwitchPosition("0.14", "50", DANGER_ALT_SELECT), new SwitchPosition("0.28", "100", DANGER_ALT_SELECT), new SwitchPosition("0.42", "150", DANGER_ALT_SELECT)
                ,new SwitchPosition("0.56", "200", DANGER_ALT_SELECT), new SwitchPosition("0.70", "250", DANGER_ALT_SELECT), new SwitchPosition("0.84", "300", DANGER_ALT_SELECT)
                , new SwitchPosition("0.98", "400", DANGER_ALT_SELECT)   
                }, "Radio Altimeter", "Danger Alt Select", "%0.01f"));
           #endregion

            #region OXYGENE_SYSTEM
            AddFunction(Switch.CreateToggleSwitch(this, OXYGENE_SYSTEM, HELMET_AIR_CONDITION, "285", "1", "On", "0", "Off", "Oxygen System", "Helmet Air Condition Off/On", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, OXYGENE_SYSTEM, EMERG_OXYGENE, "286", "1", "On", "0", "Off", "Oxygen System", "Emergency Oxygen Off/On", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, OXYGENE_SYSTEM, MIXTURE_OXYGENE_SELECT, "287", "1", "On", "0", "Off", "Oxygen System", "Mixture/Oxygen Select", "%1d"));
            #endregion

            #region CANOPY
            AddFunction(Switch.CreateToggleSwitch(this, CANOPY, CANOPY_HERMET_HANDLE, "328", "1", "On", "0", "Off", "Canopy", "Hermetize Canopy", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, CANOPY, CANOPY_LOCK_HANDLE, "329", "1", "On", "0", "Off", "Canopy", "Secure Canopy - Canopy Lock Handle", "%1d"));
            AddFunction(new PushButton(this, CANOPY, CANOPY_OPEN, "375", "Canopy", "Canopy Open"));
            AddFunction(new PushButton(this, CANOPY, CANOPY_CLOSE, "385", "Canopy", "Canopy Close"));
            AddFunction(new PushButton(this, CANOPY, CANOPY_ANTI_ICE, "239", "Canopy", "Canopy Anti Ice"));
            AddFunction(Switch.CreateToggleSwitch(this, CANOPY, CANOPY_EMERG_RELEASE, "224", "1", "On", "0", "Off", "Canopy", "Canopy Emergency Release Handle", "%1d"));
            AddFunction(new Axis(this, CANOPY, CANOPY_VENT_SYSTEM, "649", 0.1d, 0d, 1d, "Canopy", "Canopy Vent System"));
            #endregion

            #region ASP Gunsight
            AddFunction(Switch.CreateToggleSwitch(this, ASP, ASP_ON, "186", "1", "On", "0", "Off", "ASP", "ASP Optical sight On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ASP, ASP_MAN_AUTO_SELECT, "241", "1", "On", "0", "Off", "ASP", "ASP Main Mode - Manual/Auto", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ASP, ASP_BOMB_STRELB_SELECT, "242", "1", "On", "0", "Off", "ASP", "ASP Mode - Bombardment/Shooting", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ASP, ASP_MISSILE_GUN_SELECT, "243", "1", "On", "0", "Off", "ASP", "ASP Mode - Missiles-Rockets/Gun", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ASP, ASP_SS_GIRO_SELECT, "244", "1", "On", "0", "Off", "ASP", "ASP Mode - Giro/Missile", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ASP, ASP_PIPPER_ON, "249", "1", "On", "0", "Off", "ASP", "ASP Pipper On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, ASP, ASP_NET_ON, "250", "1", "On", "0", "Off", "ASP", "ASP Fix net On/Off", "%1d"));
            //rem reduce increments in profile for these two targe size and intercept angle
            AddFunction(new Axis(this, ASP, ASP_TARGET_SIZE, "245", 0.01d, 0d, 1d, "ASP", "ASP Target Size"));
            AddFunction(new Axis(this, ASP, ASP_INTERCEPT_ANGLE, "246", 0.01d, 0d, 1d, "ASP", "ASP Intercept Angle"));
            AddFunction(new Axis(this, ASP, ASP_SCALE_LIGHT, "247", 0.1d, 0d, 1d, "ASP", "ASP Scale Light"));
            AddFunction(new Axis(this, ASP, ASP_PIPPER_LIGHT, "248", 0.1d, 0d, 1d, "ASP", "ASP Pipper Light Control"));
            AddFunction(new Axis(this, ASP, ASP_NET_LIGHT, "251", 0.1d, 0d, 1d, "ASP", "ASP Fix Net light control"));
            AddFunction(new Axis(this, ASP, RU_DOBLOG_AXIS, "384", 0.1d, 0d, 1d, "ASP", "TDC Range / Pipper Span control"));
            #endregion

            #region WEAPON_CONTROL
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, OBOGREV, "181", "1", "On", "0", "Off", "Weapon Control", "Missiles - Rockets Heat On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, PUSK, "182", "1", "On", "0", "Off", "Weapon Control", "Missiles - Rockets Launch On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, PITANIE_12, "183", "1", "On", "0", "Off", "Weapon Control", "Pylon 1-2 Power On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, PITANIE_34, "184", "1", "On", "0", "Off", "Weapon Control", "Pylon 3-4 Power On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, GS23, "185", "1", "On", "0", "Off", "Weapon Control", "GS-23 Gun On/Off", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, FKP, "187", "1", "On", "0", "Off", "Weapon Control", "Guncam On/Off", "%1d"));
            // disabling due to duplicate function definition.  
            //AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, TAKT_SBROS_SAFETY_COVER, "227", "1", "On", "0", "Off", "Weapon Control", "Tactical Drop Cover", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, TAKT_SBROS, "278", "1", "On", "0", "Off", "Weapon Control", "Tactical Drop Switch", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, AVAR_PUSK_SAFETY_COVER, "275", "1", "On", "0", "Off", "Weapon Control", "Emergency Missile/Rocket Launcher Cover", "%1d"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, AVAR_PUSK, "276", "Weapon Control", "Emergency Missile/Rocket Launcher"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, SBROS_KRIL_BAKOV_SAFETY_COVER, "256", "1", "On", "0", "Off", "Weapon Control", "Drop Wing Fuel Tanks Cover", "%1d"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, SBROS_KRIL_BAKOV, "257", "Weapon Control", "Drop Wing Fuel Tanks"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, SBROS_PODV_BAKOV, "386", "Weapon Control", "Drop Center Fuel Tank"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, SBROS_VNESN_SAFETY_COVER, "269", "1", "On", "0", "Off", "Weapon Control", "Drop Payload - Outer Pylons Cover", "%1d"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, SBROS_VNESN, "270", "Weapon Control", "Drop Payload - Outer Pylon"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, SBROS_VNUTR_SAFETY_COVER, "271", "1", "On", "0", "Off", "Weapon Control", "Drop Payload - Inner Pylons Cover", "%1d"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, SbROS_VNUTR, "272", "Weapon Control", "Drop Payload - Inner Pylons"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, ASP_VOZDUH_ZEMLJA, "230", "1", "Air", "0", "Ground", "Weapon Control", "Weapon Mode - Air/Ground", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, WEAPON_CONTROL, ASP_SS_NEUTR_RNS, "231", "1.0", "IR Missile", "0.5", "Neutral", "0.0", "SAR Missile", "Weapon Control", "IR/Neutral/SAR", "%0.1f"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, ASP_GUN_RELOAD_1, "232", "Weapon Control", "Activate Gun Loading Pyro - 1"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, ASP_GUN_RELOAD_2, "233", "Weapon Control", "Activate Gun Loading Pyro - 2"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, ASP_GUN_RELOAD_3, "234", "Weapon Control", "Activate Gun Loading Pyro - 3"));
            AddFunction(new Switch(this, WEAPON_CONTROL, ASP_LAUNCHER_SELECT, new SwitchPosition[] { new SwitchPosition("0.0", "0", ASP_LAUNCHER_SELECT), new SwitchPosition("0.1", "1", ASP_LAUNCHER_SELECT), 
                new SwitchPosition("0.2", "2", ASP_LAUNCHER_SELECT), new SwitchPosition("0.3", "3", ASP_LAUNCHER_SELECT), new SwitchPosition("0.4", "4", ASP_LAUNCHER_SELECT), 
                new SwitchPosition("0.5", "5", ASP_LAUNCHER_SELECT), new SwitchPosition("0.6", "6", ASP_LAUNCHER_SELECT), new SwitchPosition("0.7", "7", ASP_LAUNCHER_SELECT), 
                new SwitchPosition("0.8", "8", ASP_LAUNCHER_SELECT), new SwitchPosition("0.9", "9", ASP_LAUNCHER_SELECT), new SwitchPosition("1.0", "10", ASP_LAUNCHER_SELECT) 
                }, "Weapon Control", "ASP Launch Selector", "%0.1f"));
            AddFunction(new Axis(this, WEAPON_CONTROL, MISSILE_SOUND, "297", 0.1d, 0d, 1d, "Weapon Control", "Missile Seeker Sound"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, GUN_FIRE_BTN, "381", "Weapon Control", "Fire Gun"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, PUSK_BTN, "382", "Weapon Control", "Release Weapon"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, PUSK_BTN_SAFETY_COVER, "383", "1", "On", "0", "Off", "Weapon Control", "Release Weapon Cover", "%1d"));
            #endregion

            #region HELMET_VISOR
            AddFunction(Switch.CreateToggleSwitch(this, HELMET_VISOR, HELMET_HEAT_MAN_AUT, "306", "1", "Manual", "0", "Auto", "Helmet Visor", "Helmet Heat - Manual/Auto", "%1d"));
            AddFunction(new PushButton(this, HELMET_VISOR, HELMET_QUICK_HEAT, "310", "Helmet Visor", "Helmet Quick Heat"));
            // commenting until this duplicate can be resolved.
            //AddFunction(Switch.CreateToggleSwitch(this, HELMET_VISOR, HELMET_VISOR_EXTRA, "369", "1", "On", "0", "Off", "Helmet Visor", "Helmet visor - off/on", "%1d"));
            #endregion

            #region AIR CONDITIONING
            AddFunction(new Switch(this, AIR_CONDITIONING, AIR_CONDITION_SELECT, new SwitchPosition[] { 
                new SwitchPosition("0.00", "Off", AIR_CONDITION_SELECT), new SwitchPosition("0.33", "Cold", AIR_CONDITION_SELECT), 
                new SwitchPosition("0.66", "Auto", AIR_CONDITION_SELECT),new SwitchPosition("0.99", "Warm", AIR_CONDITION_SELECT)},
                "Air Conditioning", "Off/Cold/Auto/Warm", "%0.1f"));
            #endregion

            #region SARPP
            AddFunction(Switch.CreateToggleSwitch(this, SARPP, SARPP_EXTRA, "193", "1", "On", "0", "Off", "SARPP-12", "Flight Data Recorder On/Off", "%1d"));
            #endregion

            #region avAChS
            AddFunction(Switch.CreateThreeWaySwitch(this, avAChS, ACHS_LEFT_PULL, "265", "1.0", "In", "0.0", "Neutral", "-1.0", "Out", "avAChS", "Clock left push/pull springloaded", "%0.1f"));
            AddFunction(new Axis(this, avAChS, ACHS_LEFT_ROTATE, "264", 0.1d, -1d, 1d, "avAChS", "Clock left rotate"));
            AddFunction(Switch.CreateThreeWaySwitch(this, avAChS, ACHS_RIGHT_PUSH, "268", "-1.0", "Up", "0.0", "Neutral", "1.0", "Down", "avAChS", "Clock right push 3way", "%0.1f"));
            // commenting until this duplicate can be resolved.
            //AddFunction(new PushButton(this, avAChS, ACHS_RIGHT_PUSH, "268", "avAChS", "Clock right push button"));
            AddFunction(new Axis(this, avAChS, ACHS_RIGHT_ROTATE, "267", 0.1d, 0d, 1d, "avAChS", "Clock right rotate"));
            #endregion

            #region Dummy buttons/switches
            //these 4 are now implemented by leatherneck
            AddFunction(Switch.CreateToggleSwitch(this, RADAR, MISL_MODE_COVER, "632", "1", "Open", "0", "Closed", "Radar", "Radar emission - Cover", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, RADAR, MISL_MODE_ACT_TRAIN, "633", "1", "Combat", "0", "Training", "Radar", "Radar emission - Combat/Training", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, KONUS, AIRDUCT_TEST_COVER, "638", "1", "Open", "0", "Closed", "Mach", "1.7 Mach Test Button - Cover", "%1d"));
            AddFunction(new PushButton(this, KONUS, AIRDUCT_TEST, "639", "Mach", "1.7 Mach Test Button"));
            //not implemented in game
            //AddFunction(new Axis(this, UNCLASSIFIED, GSUIT_MAX_MIN, "634", 0.1d, 0d, 1d, "Unclassified", "G-Suit Max/Min valve"));
            //AddFunction(Switch.CreateToggleSwitch(this, UNCLASSIFIED, SINA_NR_1_COVER, "635", "1", "Open", "0", "Closed", "Unclassified", "Electric Bus Nr.1 - Cover", "%1d"));
            //AddFunction(Switch.CreateToggleSwitch(this, UNCLASSIFIED, SINA_NR_1, "636", "1", "On", "0", "Off", "Unclassified", "Electric Bus Nr.1", "%1d"));
            //AddFunction(Switch.CreateToggleSwitch(this, UNCLASSIFIED, SINA_NR_2, "637", "1", "On", "0", "Off", "Unclassified", "Electric Bus Nr.2", "%1d"));
            //AddFunction(new PushButton(this, UNCLASSIFIED, BU45_BUSTER, "640", "Unclassified", "BU-45 Buster System Separation"));
            //AddFunction(new PushButton(this, UNCLASSIFIED, SODPBU_1, "642", "Unclassified", "SOD Control PBU-1"));
            //AddFunction(new PushButton(this, UNCLASSIFIED, SODPBU_2, "641", "Unclassified", "SOD Control PBU-2"));
            //AddFunction(new PushButton(this, UNCLASSIFIED, EJECT_EJECT_EJECT, "643", "Unclassified", "Eject"));
            //AddFunction(Switch.CreateToggleSwitch(this, UNCLASSIFIED, EMER_OXY, "644", "1", "On", "0", "Off", "Unclassified", "Ejection Seat Emergency Oxygen", "%1d"));
            //AddFunction(Switch.CreateToggleSwitch(this, UNCLASSIFIED, UK_2_MML, "645", "1", "On", "0", "Off", "Unclassified", "UK-2M Mic Amplifier M/L", "%1d"));
            //AddFunction(Switch.CreateToggleSwitch(this, UNCLASSIFIED, UK_2_MGs_KM, "646", "1", "On", "0", "Off", "Unclassified", "UK-2M Mic Amplifier GS/KM", "%1d"));
            //AddFunction(new Axis(this, UNCLASSIFIED, SUIT_VENT, "647", 0.1d, 0d, 1d, "Unclassified", "Suit Ventilation"));
            //AddFunction(Switch.CreateToggleSwitch(this, UNCLASSIFIED, HARNESS, "648", "1", "Open", "0", "Closed", "Unclassified", "Harness Separation", "%1d"));
            //elements["PNT_448"] = default_axis_limited(LOCALIZE("Harness Loose/Tight"),devices.UNCLASSIFIED, device_commands.HarnessLooseTight,650, 0.2, false, false, {0.0, 1.0})
            //elements["PNT_449"] = default_axis_limited(LOCALIZE("Throttle Fixation"),devices.UNCLASSIFIED, device_commands.RUDFixator,651, 0.2, false, false, {0.0, 1.0})
            #endregion

            #region IAB PBK-3 (Nuke control)
            AddFunction(Switch.CreateToggleSwitch(this, NUKE_CONTROL, IAB_AVAR_SBROS, "387", "1", "On", "0", "Off", "Nuke Control", "Emergency Jettison", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, NUKE_CONTROL, IAB_AVAR_SBROS_VZR, "388", "1", "Armed", "0", "Not Armed", "Nuke Control", "Emergency Jettison Armed / Not Armed", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, NUKE_CONTROL, IAB_BOEVOJ_SBROS, "389", "1", "On", "0", "Off", "Nuke Control", "Tactical Jettison", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, NUKE_CONTROL, IAB_SPEC_AB, "390", "1", "Special AB", "0", "Missile-Rocket-Bombs-Cannon", "Nuke Control", "Special AB / Missile-Rocket-Bombs-Cannon", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, NUKE_CONTROL, IAB_TORMOZ, "391", "1", "On", "0", "Off", "Nuke Control", "Brake Chute", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, NUKE_CONTROL, IAB_VOZDUH, "392", "1", "Air", "0", "Ground", "Nuke Control", "Detonation Air / Ground", "%1d"));
            #endregion

            #region SPS 141-100
            AddFunction(Switch.CreateToggleSwitch(this, SPS_CONTROL, SPS_ON_OFF, "393", "1", "On", "0", "Off", "SPS Control", "Detonation Air / Ground", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, SPS_CONTROL, SPS_TRANSMIT, "394", "1", "Transmit", "0", "Receive", "SPS Control", "Transmit / Receive", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, SPS_CONTROL, SPS_PROGRAM, "395", "1", "Prog 1", "0", "Prog 2", "SPS Control", "Program I / II", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, SPS_CONTROL, SPS_CONTINOUS, "396", "1", "Continuous", "0", "Continuous", "SPS Control", "Continuous / Continuous", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, SPS_CONTROL, SPS_TEST, "397", "1", "On", "0", "Off", "SPS Control", "Test", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, SPS_CONTROL, SPS_DISPENSE, "398", "1", "Auto", "0", "Manual", "SPS Control", "Dispenser Auto / Manual", "%1d"));
            AddFunction(Switch.CreateThreeWaySwitch(this, SPS_CONTROL, SPS_OFF_PARAL_FULL, "399", "0.0", "Off", "0.5", "Parallel", "1.0", "Full", "SPS", "Off/Parallel/Full", "%0.1f"));
            AddFunction(Switch.CreateToggleSwitch(this, SPS_CONTROL, SPS_COVER, "400", "1", "Open", "0", "Closed", "SPS Control", "Manual Activation button - Cover", "%1d"));
            AddFunction(new PushButton(this, SPS_CONTROL, SPS_PUSH, "401", "SPS Control", "Manual Activation button"));
            #endregion

            #region GUV Control Box -/N/ GUV is useless, it's mostly anti-infantry weapon
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, GUV_ON_OFF, "420", "1", "On", "0", "Off", "Weapon Control GUV", "Manual Activation button - Cover", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, WEAPON_CONTROL, GUV_POD_MAIN, "421", "1", "Main", "0", "UPK", "Weapon Control GUV", "MAIN GUN / UPK Guns", "%1d"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, GUV_ARM_1, "422", "Weapon Control GUV", "LOAD 1"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, GUV_ARM_2, "425", "Weapon Control GUV", "LOAD 2"));
            AddFunction(new PushButton(this, WEAPON_CONTROL, GUV_ARM_3, "424", "Weapon Control GUV", "LOAD 3"));
            #endregion

            #region Warning Lights
            AddFunction(new FlagValue(this, "541", "Warning Lights", "Canopy Warning light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "542", "Warning Lights", "SORC light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "407", "Warning Lights", "Check state", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "516", "Warning Lights", "Marker light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "518", "Warning Lights", "Stabilizator light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "520", "Warning Lights", "Check gear light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "521", "Warning Lights", "Flaps light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "522", "Warning Lights", "Airbrake light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "523", "Warning Lights", "Central Pylon light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "524", "Warning Lights", "Rato L light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "525", "Warning Lights", "Rato R light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "526", "Warning Lights", "Pylon 1 On light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "527", "Warning Lights", "Pylon 2 On light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "528", "Warning Lights", "Pylon 3 On light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "529", "Warning Lights", "Pylon 4 On light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "530", "Warning Lights", "Pylon 1 Off light", "Indicator lit when off"));
            AddFunction(new FlagValue(this, "531", "Warning Lights", "Pylon 2 Off light", "Indicator lit when off"));
            AddFunction(new FlagValue(this, "532", "Warning Lights", "Pylon 3 Off light", "Indicator lit when off"));
            AddFunction(new FlagValue(this, "533", "Warning Lights", "Pylon 4 Off light", "Indicator lit when off"));
            #endregion

            #region in cockpit indicators
            AddFunction(new FlagValue(this, "9", "Cockpit Indicators", "Gear Nose Up Light", "Indicator lit when up"));
            AddFunction(new FlagValue(this, "12", "Cockpit Indicators", "Gear Nose Down Light", "Indicator lit when down"));
            AddFunction(new FlagValue(this, "10", "Cockpit Indicators", "Gear Left Up Light", "Indicator lit when up"));
            AddFunction(new FlagValue(this, "13", "Cockpit Indicators", "Gear Left Down Light", "Indicator lit when down"));
            AddFunction(new FlagValue(this, "11", "Cockpit Indicators", "Gear Right Up Light", "Indicator lit when up"));
            AddFunction(new FlagValue(this, "14", "Cockpit Indicators", "Gear Right Down Light", "Indicator lit when down"));

            AddFunction(new FlagValue(this, "548", "Cockpit Indicators", "RSBN azimuth correction light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "549", "Cockpit Indicators", "RSBN range correction light", "Indicator lit when on"));

            AddFunction(new FlagValue(this, "500", "Cockpit Indicators", "Low alt light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "537", "Cockpit Indicators", "AOA warning light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "535", "Cockpit Indicators", "KPP Arrested light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "519", "Cockpit Indicators", "Trimmer light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "510", "Cockpit Indicators", "DC Gen light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "511", "Cockpit Indicators", "AC Gen light", "Indicator lit when on"));

            AddFunction(new FlagValue(this, "501", "Cockpit Indicators", "Fuel PODC light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "502", "Cockpit Indicators", "Fuel 1GR light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "503", "Cockpit Indicators", "Fuel 450 light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "504", "Cockpit Indicators", "Fuel 3GR light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "505", "Cockpit Indicators", "Fuel PODW light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "506", "Cockpit Indicators", "Fuel Expence light", "Indicator lit when on"));

            AddFunction(new FlagValue(this, "509", "Cockpit Indicators", "Start Device Doused light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "507", "Cockpit Indicators", "Afterburner 1 light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "508", "Cockpit Indicators", "Afterburner 2 light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "512", "Cockpit Indicators", "Nozzle light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "517", "Cockpit Indicators", "Cone light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "513", "Cockpit Indicators", "Oil light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "534", "Cockpit Indicators", "Fire light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "515", "Cockpit Indicators", "Check Hydraulic pressure light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "514", "Cockpit Indicators", "Check Buster pressure light", "Indicator lit when on"));

            AddFunction(new FlagValue(this, "539", "Cockpit Indicators", "ASP Target acquired light", "Indicator lit when acquired"));
            AddFunction(new FlagValue(this, "581", "Cockpit Indicators", "IAB light 1", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "582", "Cockpit Indicators", "IAB light 2", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "583", "Cockpit Indicators", "IAB light 3", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "593", "Cockpit Indicators", "SPS Illumination", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "546", "Cockpit Indicators", "SAU Stabilisation light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "547", "Cockpit Indicators", "SAU feeding light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "544", "Cockpit Indicators", "SAU landing command light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "545", "Cockpit Indicators", "SAU landing auto light", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "550", "Cockpit Indicators", "GUN gotovn LIGHT", "Indicator lit when on"));

            AddFunction(new FlagValue(this, "601", "Cockpit Indicators", "SPO LF", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "602", "Cockpit Indicators", "SPO RF", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "603", "Cockpit Indicators", "SPO RB", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "604", "Cockpit Indicators", "SPO LB", "Indicator lit when on"));
            AddFunction(new FlagValue(this, "605", "Cockpit Indicators", "SPO Muted", "Indicator lit when up"));

            #endregion


            /******************************************************
             * ******************GAUGES BELOW**********************
             * ****************************************************/

            #region Accelerometer
            CalibrationPointCollectionDouble accelerometerScale = new CalibrationPointCollectionDouble(-0.41d, -5, 1.0d, 10d);
            accelerometerScale.Add(new CalibrationPointDouble(0.096d, 1d));
            accelerometerScale.Add(new CalibrationPointDouble(0.5d, 5d));
            accelerometerScale.Add(new CalibrationPointDouble(0.81d, 8d));
            AddFunction(new ScaledNetworkValue(this, "110", accelerometerScale, "Accelerometer", "Acceleration", "Current gs", "", BindingValueUnits.Numeric, "%0.2f"));
            AddFunction(new ScaledNetworkValue(this, "113", 9d, "Accelerometer", "Maxium acceleration", "Max Gs attained.", "", BindingValueUnits.Numeric, 1d, "%0.2f"));
            AddFunction(new ScaledNetworkValue(this, "114", 6d, "Accelerometer", "Minimum acceleration", "Min Gs attained.", "", BindingValueUnits.Numeric, -5d, "%0.2f"));
            AddFunction(new PushButton(this, ACCELEROMETER, ACCEL_RESET, "228", "Accelerometer", "Accelerometer Reset buton"));
            #endregion

            #region IAS
            AddFunction(new ScaledNetworkValue(this, "100", 555.55d, "IAS", "IAS", "Current ias", "", BindingValueUnits.Numeric, 0d, "%0.4f"));
            #endregion

            #region TAS
            CalibrationPointCollectionDouble tasScale = new CalibrationPointCollectionDouble(0.20d, 167d, 1.0d, 833d);
            tasScale.Add(new CalibrationPointDouble(0.309d, 278d));
            tasScale.Add(new CalibrationPointDouble(0.49d, 417d));
            tasScale.Add(new CalibrationPointDouble(0.67d, 555d));
            AddFunction(new ScaledNetworkValue(this, "101", tasScale, "TAS", "TAS", "Current tas", "", BindingValueUnits.Numeric, "%0.4f"));

            CalibrationPointCollectionDouble mScale = new CalibrationPointCollectionDouble(0d, 0d, 1.0d, 3.0d);
            mScale.Add(new CalibrationPointDouble(0.202d, 0.6d));
            mScale.Add(new CalibrationPointDouble(0.312d, 1.0d));
            mScale.Add(new CalibrationPointDouble(0.6d, 1.8d));
            mScale.Add(new CalibrationPointDouble(0.66d, 2.0d));
            AddFunction(new ScaledNetworkValue(this, "102", 3.00d, "TAS", "M", "Current mach", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            #endregion

            #region Fuel Gauge
            AddFunction(new ScaledNetworkValue(this, "52", 6000d, "Fuel", "Fuel Gauge", "Current fuel", "", BindingValueUnits.Numeric, 0, "%0.4f"));
            #endregion

            #region UUA (aoa)
            AddFunction(new ScaledNetworkValue(this, "105", 0.6108d, "UUA", "UUA (AOA)", "Current Angle", "", BindingValueUnits.Numeric, 0d, "%0.4f"));
            #endregion

            #region DA200
            CalibrationPointCollectionDouble vviScale = new CalibrationPointCollectionDouble(-1d, -400d, 1.0d, 400d);
            vviScale.Add(new CalibrationPointDouble(-0.878d, -200d));
            vviScale.Add(new CalibrationPointDouble(-0.754d, -100d));
            vviScale.Add(new CalibrationPointDouble(-0.575d, -50d));
            vviScale.Add(new CalibrationPointDouble(-0.504d, -20d));
            vviScale.Add(new CalibrationPointDouble(-0.256d, -10d));
            vviScale.Add(new CalibrationPointDouble(0d, 0d));
            vviScale.Add(new CalibrationPointDouble(0.256d, 10d));
            vviScale.Add(new CalibrationPointDouble(0.505d, 20d));
            vviScale.Add(new CalibrationPointDouble(0.571d, 50d));
            vviScale.Add(new CalibrationPointDouble(0.751d, 100d));
            vviScale.Add(new CalibrationPointDouble(0.871d, 200d));
            AddFunction(new ScaledNetworkValue(this, "106", vviScale, "DA200", "DA200 VVI", "Current vvi", "", BindingValueUnits.Numeric, "%.4f"));

            AddFunction(new ScaledNetworkValue(this, "31", 1d, "DA200", "DA200 Slip", "Current slip", "", BindingValueUnits.Numeric, 0d, "%.4f"));

            AddFunction(new ScaledNetworkValue(this, "107", 0.0443d, "DA200", "DA200 Turn", "Current turn", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            #endregion

            #region KPP aka ADI
            AddFunction(new ScaledNetworkValue(this, "108", 180d, "KPP", "Bank", "Current bank", "(-180 to 180)", BindingValueUnits.Degrees));
            AddFunction(new ScaledNetworkValue(this, "109", -90d, "KPP", "Pitch", "Current pitch", "(-90 to 90)", BindingValueUnits.Degrees));
            AddFunction(new NetworkValue(this, "565", "KPP", "bank steering offset", "Current amount of bank steering displayed on the KPP.", "(-1 to 1)", BindingValueUnits.Numeric));
            AddFunction(new NetworkValue(this, "566", "KPP", "pitch steering offset", "Current amount of pitch steering displayed on the KPP.", "(-1 to 1)", BindingValueUnits.Numeric));
            AddFunction(new FlagValue(this, "567", "KPP", "K flag", "Indicates whether the K flag is displayed on the KPP."));
            AddFunction(new FlagValue(this, "568", "KPP", "T flag", "Indicates whether the T flag is displayed on the KPP."));
            //For slip uses the same input as da200
            //for the 2 PRMG aux uses the same input as NPP main course & glideslope needles
            #endregion

            #region NPP aka HSI
            //required for KPP aux input too==
            AddFunction(new NetworkValue(this, "590", "NPP", "Course Needle", "Current course required on NPP & KPP.", "(-1 to 1)", BindingValueUnits.Numeric));
            AddFunction(new NetworkValue(this, "589", "NPP", "Glideslope Needle", "Current glideslope required on NPP & KPP.", "(-1 to 1)", BindingValueUnits.Numeric));
            //================================
            AddFunction(new ScaledNetworkValue(this, "111", 360d, "NPP", "Heading", "Current heading displayed on the HSI", "(0-360)", BindingValueUnits.Degrees));
            AddFunction(new ScaledNetworkValue(this, "68", 360d, "NPP", "Commaned Course", "Current commanded course on the HSI", "(0-360)", BindingValueUnits.Degrees));
            AddFunction(new ScaledNetworkValue(this, "36", 360d, "NPP", "Bearing", "Current bearing displayed on the HSI", "(0-360)", BindingValueUnits.Degrees));
            AddFunction(new FlagValue(this, "587", "NPP", "K flag", "Indicates whether the K flag is displayed on the NPP."));
            AddFunction(new FlagValue(this, "588", "NPP", "G flag", "Indicates whether the G flag is displayed on the NPP."));
            #endregion

            #region Engine Exhaust Temp
            CalibrationPointCollectionDouble engtempScale = new CalibrationPointCollectionDouble(0d, 300d, 1.0d, 900d);
            engtempScale.Add(new CalibrationPointDouble(0.11d, 400d));
            engtempScale.Add(new CalibrationPointDouble(0.25d, 500d));
            engtempScale.Add(new CalibrationPointDouble(0.39d, 600d));
            engtempScale.Add(new CalibrationPointDouble(0.51d, 650d));
            engtempScale.Add(new CalibrationPointDouble(0.635d, 700d));
            engtempScale.Add(new CalibrationPointDouble(0.75d, 750d));
            engtempScale.Add(new CalibrationPointDouble(0.87d, 800d));
            engtempScale.Add(new CalibrationPointDouble(0.95d, 850d));
            AddFunction(new ScaledNetworkValue(this, "51", engtempScale, "Engine", "Exhaust Temp", "Current temp", "", BindingValueUnits.Numeric, "%0.4f"));
            #endregion

            #region Engine RPM
            CalibrationPointCollectionDouble enginerpmScale = new CalibrationPointCollectionDouble(0.0d, 0.0, 1.0d, 110d);
            AddFunction(new ScaledNetworkValue(this, "50", enginerpmScale, "EngineRpm", "EngineRpm1", "Current rpm", "", BindingValueUnits.Numeric, "%0.4f"));
            AddFunction(new ScaledNetworkValue(this, "670", enginerpmScale, "EngineRpm", "EngineRpm2", "Current rpm", "", BindingValueUnits.Numeric, "%0.4f"));
            #endregion

            #region Radio Altimeter
            CalibrationPointCollectionDouble radaltScale = new CalibrationPointCollectionDouble(0d, 0d, 1.0d, 1000d);
            radaltScale.Add(new CalibrationPointDouble(0.041d, 10d));
            radaltScale.Add(new CalibrationPointDouble(0.07d, 20d));
            radaltScale.Add(new CalibrationPointDouble(0.103d, 30d));
            radaltScale.Add(new CalibrationPointDouble(0.13d, 40d));
            radaltScale.Add(new CalibrationPointDouble(0.181d, 50d));
            radaltScale.Add(new CalibrationPointDouble(0.21d, 60d));
            radaltScale.Add(new CalibrationPointDouble(0.245d, 70d));
            radaltScale.Add(new CalibrationPointDouble(0.260d, 80d));
            radaltScale.Add(new CalibrationPointDouble(0.298d, 90d));
            radaltScale.Add(new CalibrationPointDouble(0.325d, 100d));
            radaltScale.Add(new CalibrationPointDouble(0.472d, 150d));
            radaltScale.Add(new CalibrationPointDouble(0.58d, 200d));
            radaltScale.Add(new CalibrationPointDouble(0.680d, 250d));
            radaltScale.Add(new CalibrationPointDouble(0.732d, 300d));
            radaltScale.Add(new CalibrationPointDouble(0.807d, 400d));
            radaltScale.Add(new CalibrationPointDouble(0.867d, 500d));
            radaltScale.Add(new CalibrationPointDouble(0.909d, 600d));
            AddFunction(new ScaledNetworkValue(this, "103", radaltScale, "Radio Altimeter", "Radio Altimeter", "Current AGL", "", BindingValueUnits.Numeric, "%0.4f"));
            #endregion

            #region Baro Altimeter
            //uses baro alt pressure knob axis as input for qfe card
            AddFunction(new Axis(this, ALTIMETER, ALTIMETER_PRESSURE, "262", 0.01d, -1d, 1d, "Baro Altimeter", "Altimeter pressure knob axis"));
            AddFunction(new PushButton(this, ALTIMETER, ALTIMETER_PRESSURE_RESET, "653", "Baro Altimeter", "Altimeter Pressure Reset"));
            CalibrationPointCollectionDouble baroAltimeterMScale = new CalibrationPointCollectionDouble(0.0d, 0d, 1.0d, 1000d);
            baroAltimeterMScale.Add(new CalibrationPointDouble(0.211d, 200d));
            baroAltimeterMScale.Add(new CalibrationPointDouble(0.416d, 400d));
            baroAltimeterMScale.Add(new CalibrationPointDouble(0.61d, 600d));
            baroAltimeterMScale.Add(new CalibrationPointDouble(0.815d, 800d));
            AddFunction(new ScaledNetworkValue(this, "104", baroAltimeterMScale, "Baro Altimeter", "Altimeter M", "Current Meters", "", BindingValueUnits.Meters, "%0.4f"));
            AddFunction(new ScaledNetworkValue(this, "112", 30000d, "Baro Altimeter", "Altimeter Km", "Current Km's", "", BindingValueUnits.Meters, 0d, "%0.4f"));
            AddFunction(new ScaledNetworkValue(this, "658", 1000d, "Baro Altimeter", "Altimeter Triangle M", "Current Triangle Meters", "", BindingValueUnits.Meters, 0d, "%0.4f"));
            AddFunction(new ScaledNetworkValue(this, "652", 30000d, "Baro Altimeter", "Altimeter Triangle Km", "Current Triangle Km's", "", BindingValueUnits.Meters, 0d, "%0.4f"));
            #endregion

            #region ARU-3VM
            AddFunction(new ScaledNetworkValue(this, "64", 1d, "ARU", "ARU-3VM", "Current ratio", "", BindingValueUnits.Numeric, 0d, "%0.4f"));
            #endregion

            #region Nosecone Position UPES3
            //Nosecone manual needle uses position controller output
            CalibrationPointCollectionDouble upesScale = new CalibrationPointCollectionDouble(0d, 0d, 1.0d, 1.0d);
            AddFunction(new ScaledNetworkValue(this, "66", 1.0d, "Nose Cone", "Nosecone position", "Nose position", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            AddFunction(Switch.CreateToggleSwitch(this, KONUS, KONUS_ON, "170", "1", "Open", "0", "Closed", "Nose Cone", "Nosecone On/Of", "%1d"));
            AddFunction(Switch.CreateToggleSwitch(this, KONUS, KONUS_MAN_AUTO, "309", "1", "Open", "0", "Closed", "Nose Cone", "Nosecone Control - Manual/Auto", "%1d"));
            AddFunction(new Axis(this, KONUS, KONUS_BUTTON, "236", 0.1d, 0d, 1d, "Nose Cone", "Nosecone manual position controller"));
            #endregion

            #region Hydraulic Pressure
            AddFunction(new ScaledNetworkValue(this, "126", 300d, "Hydraulic Pressure Gauge", "Hydraulic Pressure Main", "Current pressure", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            AddFunction(new ScaledNetworkValue(this, "125", 300d, "Hydraulic Pressure Gauge", "Hydraulic Pressure Secondary", "Current pressure", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            #endregion

            #region Voltmeter
            AddFunction(new ScaledNetworkValue(this, "124", 30d, "Voltmeter Gauge", "Voltmeter", "Current Volts", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            #endregion

            #region Oil Pressure Gauge
            AddFunction(new ScaledNetworkValue(this, "627", 4d, "Oil Pressure Gauge", "Oil Pressure", "Current Pressure", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            #endregion

            #region RSBN Distance
            AddFunction(new ScaledNetworkValue(this, "357", 10d, "RSBN Distance Gauge", "RSBN Distance Single Meters", "Current Singles Distance", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            AddFunction(new ScaledNetworkValue(this, "356", 10d, "RSBN Distance Gauge", "RSBN Distance Tens Meters", "Current Tens Distance", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            AddFunction(new ScaledNetworkValue(this, "355", 10d, "RSBN Distance Gauge", "RSBN Distance Hundreds Meters", "Current Hundreds Distance", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            #endregion

            #region Engine Oxygen Manometer (engine O2 feed)
            AddFunction(new ScaledNetworkValue(this, "61", 40d, "Engine O2 Gauge", "Engine O2", "Current O2", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            #endregion

            #region O2 Level Gauge
            AddFunction(new ScaledNetworkValue(this, "59", 150d, "O2 Level Gauge", "O2 Level", "Current level", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            AddFunction(new FlagValue(this, "60", "O2 Level Gauge", "Lung Blinkers", "Indicates whether the O2 is flowing."));
            #endregion

            #region O2 Pressure Gauge
            AddFunction(new ScaledNetworkValue(this, "58", 20d, "O2 Pressure Gauge", "O2 Pressure", "Current pressure", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            #endregion

            #region Battery capacity meter gauge
            AddFunction(new ScaledNetworkValue(this, "55", 100d, "Battery Capacity Gauge", "Battery Capacity", "Current capacity", "", BindingValueUnits.Numeric, 0d, "%.4f"));
            #endregion

        }

    }
}

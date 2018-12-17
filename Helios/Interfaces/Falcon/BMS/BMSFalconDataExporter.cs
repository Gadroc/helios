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

namespace GadrocsWorkshop.Helios.Interfaces.Falcon.BMS
{
    using GadrocsWorkshop.Helios.Util;
    using System;

    class BMSFalconDataExporter : FalconDataExporter
    {
        private const int PHASE_LENGTH = 1;
        private const int MAX_SECONDS = 60 * 60 * 24;

        private SharedMemory _sharedMemory = null;
        private SharedMemory _sharedMemory2 = null;

        private RadarContact[] _contacts = new RadarContact[40];

        private FlightData _lastFlightData;
        private FlightData2 _lastFlightData2;

        protected enum LightState
        {
            OFF,
            ON,
            TempOFF,
        }

        private int _outerMarkerLastTick;
        private LightState _outerMarkerState;

        private int _middleMarkerLastTick;
        private LightState _middleMarkerState;

        private int _probeheatLastTick;
        private LightState _probeheatState;

        private int _auxsrchLastTick;
        private LightState _auxsrchState;

        private int _launchLastTick;
        private LightState _launchState;

        private int _primodeLastTick;
        private LightState _primodeState;

        private int _unkLastTick;
        private LightState _unkState;

        private int _elecFaultLastTick;
        private LightState _elecFaultState;

        private int _oxyBrowLastTick;
        private LightState _oxyBrowState;

        private int _epuOnLastTick;
        private LightState _epuOnState;

        private int _jfsOnLastTick;
        private LightState _jfsOnState;

        public BMSFalconDataExporter(FalconInterface falconInterface)
            : base(falconInterface)
        {
            AddValue("Right Eyebrow", "oxy low indicator", "OXY LOW indicator on right eyebrow", "True if lit", BindingValueUnits.Boolean);
            AddValue("Caution", "equip hot indicator", "Equip hot indicator on caution panel", "True if lit", BindingValueUnits.Boolean);
            AddValue("Test Panel", "FLCS channel lamps", "FLCS channel lamps on test panel (abcd)", "True if lit", BindingValueUnits.Boolean);
            AddValue("Right Eyebrow", "flcs indicator", "FLCS Indicator", "True if lit", BindingValueUnits.Boolean);

            AddValue("Autopilot", "on indicator", "Indicates whether the autopilot is on.", "True if on", BindingValueUnits.Boolean);

            AddValue("General", "on ground", "Indicates weight on wheels.", "True if wheight is on wheels.", BindingValueUnits.Boolean);
            AddValue("Flight Control", "run light", "Run light on the flight control panel indicating bit is running.", "True if lit", BindingValueUnits.Boolean);
            AddValue("Flight Control", "fail light", "Fail light on the flight control panel indicating bit failure.", "True if lit", BindingValueUnits.Boolean);
            AddValue("Right Eyebrow", "dbu on indicator", "DBU Warning light on the right eyebrow.", "True if lit", BindingValueUnits.Boolean);
            AddValue("General", "parking brake engaged", "Indicates if the parking brake is engaged.", "True if engaged", BindingValueUnits.Boolean);
            AddValue("Caution", "cadc indicator", "CADC indicator lamp on the caution panel.", "True if lit", BindingValueUnits.Boolean);
            AddValue("General", "speed barke", "Indicates if the speed brake is deployed.", "True if speed breake is in any other position than stowed.", BindingValueUnits.Boolean);

            AddValue("HSI", "Outer marker indicator", "Outer marker indicator on HSI", "True if lit", BindingValueUnits.Boolean);
            AddValue("HSI", "Middle marker indicator", "Middle marker indicator on HSI", "True if lit", BindingValueUnits.Boolean);

            AddValue("HSI", "nav mode", "Nav mode currently selected for the HSI/eHSI", "", BindingValueUnits.Numeric);
            AddValue("Tacan", "ufc tacan band", "Tacan band set with the UFC.", "1 = X, 2 = Y", BindingValueUnits.Numeric);
            AddValue("Tacan", "aux tacan band", "Tacan band set with the AUX COM panel.", "1 = X, 2 = Y", BindingValueUnits.Numeric);
            AddValue("Tacan", "ufc tacan mode", "Tacan mode set with the UFC.", "1 = TR, 2 = AA", BindingValueUnits.Numeric);
            AddValue("Tacan", "aux tacan mode", "Tacan mode set with the AUX COM panel.", "1 = TR, 2 = AA", BindingValueUnits.Numeric);

            //BMS 4.33 addition
            AddValue("Engine", "nozzle 2 position", "Current engine nozzle2.", "Percent open (0-100)", BindingValueUnits.Numeric);
            AddValue("Engine", "rpm2", "Current engine rpm2.", "Percent (0-103)", BindingValueUnits.Numeric);
            AddValue("Engine", "ftit2", "Current forward turbine inlet temp2", "Degrees C", BindingValueUnits.Numeric);
            AddValue("Engine", "oil pressure 2", "Current oil pressure 2 in the engine.", "Percent (0-100)", BindingValueUnits.Numeric);
            AddValue("CMDS", "CMDS Mode", "Current CMDS mode", "(0 off, 1 stby, 2 Man, 3 Semi, 4 Auto, 5 BYP)", BindingValueUnits.Numeric);
            AddValue("UHF", "Backup channel", "Current Backup UHF channel", "", BindingValueUnits.Numeric);
            AddValue("UHF", "Backup frequency", "Current Backup UHF frequency", "", BindingValueUnits.Numeric);
            AddValue("UHF", "Backup frequency digit 1", "Current Backup UHF frequency digit 1", "", BindingValueUnits.Numeric);
            AddValue("UHF", "Backup frequency digit 2", "Current Backup UHF frequency digit 2", "", BindingValueUnits.Numeric);
            AddValue("UHF", "Backup frequency digit 3", "Current Backup UHF frequency digit 3", "", BindingValueUnits.Numeric);
            AddValue("UHF", "Backup frequency digit 4", "Current Backup UHF frequency digit 4", "", BindingValueUnits.Numeric);
            AddValue("UHF", "Backup frequency digit 5,6", "Current Backup UHF frequency digit 5,6", "", BindingValueUnits.Numeric);
            AddValue("Altitude", "Cabin Altitude", "Current cabin altitude", "", BindingValueUnits.Numeric);
            AddValue("HYD", "Pressure A", "Current hydraulic pressure a", "", BindingValueUnits.Numeric);
            AddValue("HYD", "Pressure B", "Current hydraulic pressure b", "", BindingValueUnits.Numeric);
            AddValue("Time", "Time", "Current tine in seconds", "(max 60 * 60 * 24)", BindingValueUnits.Numeric);
            AddValue("Engine", "fuel flow 2", "Current fuel flow to the engine 2.", "", BindingValueUnits.PoundsPerHour);

            //AltBits
            AddValue("AltBits", "altimeter calibration type", "", "True if hg otherwise hpa.", BindingValueUnits.Boolean);
            AddValue("AltBits", "altimeter pneu flag", "", "True if visible", BindingValueUnits.Boolean);

            //PowerBits
            AddValue("POWER", "bus power battery", "at least the battery bus is powered", "True if powered", BindingValueUnits.Boolean);
            AddValue("POWER", "bus power emergency", "at least the emergency bus is powered", "True if powered", BindingValueUnits.Boolean);
            AddValue("POWER", "bus power essential", "at least the essential bus is powered", "True if powered", BindingValueUnits.Boolean);
            AddValue("POWER", "bus power non essential", "at least the non-essential bus is powered", "True if powered", BindingValueUnits.Boolean);
            AddValue("POWER", "main generator", "main generator is online", "True if online", BindingValueUnits.Boolean);
            AddValue("POWER", "standby generator", "standby generator is online", "True if online", BindingValueUnits.Boolean);
            AddValue("POWER", "Jetfuel starter", "JFS is running, can be used for magswitch", "True if running", BindingValueUnits.Boolean);

            //BlinkBits
            //AddValue("Blink", "Outer marker", "slow flashing for outer marker", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "Middle marker", "fast flashing for middle marker", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "probeheat", "probeheat system is tested", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "aux search", "search function in NOT activated and a search radar is painting ownship", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "launch", "missile is fired at ownship", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "primary mode", "priority mode is enabled but more than 5 threat emitters are detected", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "unknown", "unknown is not active but EWS detects unknown radar", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "elec fault", "non-resetting fault", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "oxy brow", "monitor fault during Obogs", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "epu on", "abnormal EPU operation", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "JFS on_slow", "slow blinking: non-critical failure", "True if blinking", BindingValueUnits.Boolean);
            //AddValue("Blink", "JFS on_fast", "fast blinking: critical failure", "True if blinking", BindingValueUnits.Boolean);
        }

        internal override void InitData()
        {
            _sharedMemory = new SharedMemory("FalconSharedMemoryArea");
            _sharedMemory.Open();

            _sharedMemory2 = new SharedMemory("FalconSharedMemoryArea2");
            _sharedMemory2.Open();
        }

        internal override void PollData()
        {
            if (_sharedMemory != null && _sharedMemory.IsDataAvailable)
            {
                _lastFlightData = (FlightData)_sharedMemory.MarshalTo(typeof(FlightData));

                float altitidue = _lastFlightData.z;
                if (_lastFlightData.z < 0)
                {
                    altitidue = 99999.99f - _lastFlightData.z;
                }
                SetValue("Altimeter", "altitidue", new BindingValue(altitidue));
                SetValue("Altimeter", "barometric pressure", new BindingValue(29.92));
                SetValue("ADI", "pitch", new BindingValue(_lastFlightData.pitch));
                SetValue("ADI", "roll", new BindingValue(_lastFlightData.roll));
                SetValue("ADI", "ils horizontal", new BindingValue((_lastFlightData.AdiIlsHorPos / 2.5f) - 1f));
                SetValue("ADI", "ils vertical", new BindingValue((_lastFlightData.AdiIlsVerPos * 2f) - 1f));
                SetValue("HSI", "bearing to beacon", new BindingValue(_lastFlightData.bearingToBeacon));
                SetValue("HSI", "current heading", new BindingValue(_lastFlightData.currentHeading));
                SetValue("HSI", "desired course", new BindingValue(_lastFlightData.desiredCourse));
                SetValue("HSI", "desired heading", new BindingValue(_lastFlightData.desiredHeading));

                float deviation = _lastFlightData.courseDeviation % 180;
                SetValue("HSI", "course deviation", new BindingValue(deviation / _lastFlightData.deviationLimit));

                SetValue("HSI", "distance to beacon", new BindingValue(_lastFlightData.distanceToBeacon));
                SetValue("VVI", "vertical velocity", new BindingValue(_lastFlightData.zDot));
                SetValue("AOA", "angle of attack", new BindingValue(_lastFlightData.alpha));
                SetValue("IAS", "mach", new BindingValue(_lastFlightData.mach));
                SetValue("IAS", "indicated air speed", new BindingValue(_lastFlightData.kias));
                SetValue("IAS", "true air speed", new BindingValue(_lastFlightData.vt));

                SetValue("General", "Gs", new BindingValue(_lastFlightData.gs));
                SetValue("Engine", "nozzle position", new BindingValue(_lastFlightData.nozzlePos * 100));
                SetValue("Fuel", "internal fuel", new BindingValue(_lastFlightData.internalFuel));
                SetValue("Fuel", "external fuel", new BindingValue(_lastFlightData.externalFuel));
                SetValue("Engine", "fuel flow", new BindingValue(_lastFlightData.fuelFlow));
                SetValue("Engine", "rpm", new BindingValue(_lastFlightData.rpm));
                SetValue("Engine", "ftit", new BindingValue(_lastFlightData.ftit * 100));
                SetValue("Landging Gear", "position", new BindingValue(_lastFlightData.gearPos != 0d));
                SetValue("General", "speed brake position", new BindingValue(_lastFlightData.speedBrake));
                SetValue("General", "speed brake indicator", new BindingValue(_lastFlightData.speedBrake > 0d));
                SetValue("EPU", "fuel", new BindingValue(_lastFlightData.epuFuel));
                SetValue("Engine", "oil pressure", new BindingValue(_lastFlightData.oilPressure));

                SetValue("CMDS", "chaff remaining", new BindingValue(_lastFlightData.ChaffCount));
                SetValue("CMDS", "flares remaining", new BindingValue(_lastFlightData.FlareCount));

                SetValue("Trim", "roll trim", new BindingValue(_lastFlightData.TrimRoll));
                SetValue("Trim", "pitch trim", new BindingValue(_lastFlightData.TrimPitch));
                SetValue("Trim", "yaw trim", new BindingValue(_lastFlightData.TrimYaw));


                SetValue("Tacan", "ufc tacan chan", new BindingValue(_lastFlightData.UFCTChan));
                SetValue("Tacan", "aux tacan chan", new BindingValue(_lastFlightData.AUXTChan));

                ProcessContacts(_lastFlightData);
            }
            if(_sharedMemory2 != null & _sharedMemory2.IsDataAvailable)
            {
                _lastFlightData2 = (FlightData2)_sharedMemory2.MarshalTo(typeof(FlightData2));
                SetValue("Altimeter", "indicated altitude", new BindingValue(-_lastFlightData2.aauz));
                SetValue("HSI", "nav mode", new BindingValue((int)_lastFlightData2.navMode));
                SetValue("Tacan", "ufc tacan band", new BindingValue(_lastFlightData2.tacanInfo[(int)TacanSources.UFC].HasFlag(TacanBits.band) ? 1 : 2));
                SetValue("Tacan", "aux tacan band", new BindingValue(_lastFlightData2.tacanInfo[(int)TacanSources.AUX].HasFlag(TacanBits.mode) ? 2 : 1));
                SetValue("Tacan", "ufc tacan mode", new BindingValue(_lastFlightData2.tacanInfo[(int)TacanSources.UFC].HasFlag(TacanBits.band) ? 1 : 2));
                SetValue("Tacan", "aux tacan mode", new BindingValue(_lastFlightData2.tacanInfo[(int)TacanSources.AUX].HasFlag(TacanBits.mode) ? 2 : 1));

                //BMS 4.33 addition
                SetValue("Engine", "nozzle 2 position", new BindingValue(_lastFlightData2.nozzlePos2));
                SetValue("Engine", "rpm 2 position", new BindingValue(_lastFlightData2.rpm2));
                SetValue("Engine", "ftit 2 position", new BindingValue(_lastFlightData2.ftit2));
                SetValue("Engine", "oil pressure 2", new BindingValue(_lastFlightData2.oilPressure2));
                SetValue("Engine", "fuel flow 2", new BindingValue(_lastFlightData2.fuelFlow2));
                SetValue("Altimeter", "barometric pressure", new BindingValue(_lastFlightData2.AltCalReading));

                ProcessAltBits(_lastFlightData2.altBits);
                ProcessPowerBits(_lastFlightData2.powerBits);
                //ProcessBlinkBits(_lastFlightData2.blinkBits);

                SetValue("CMDS", "CMDS Mode", new BindingValue((int)_lastFlightData2.cmdsMode));
                SetValue("UHF", "Backup channel", new BindingValue(_lastFlightData2.BupUhfPreset));
                SetValue("UHF", "Backup frequency", new BindingValue(_lastFlightData2.BupUhfFreq));
                SetValue("UHF", "Backup frequency digit 1", new BindingValue(_lastFlightData2.BupUhfFreq / 100000 % 10));
                SetValue("UHF", "Backup frequency digit 2", new BindingValue(_lastFlightData2.BupUhfFreq / 10000 % 10));
                SetValue("UHF", "Backup frequency digit 3", new BindingValue(_lastFlightData2.BupUhfFreq / 1000 % 10));
                SetValue("UHF", "Backup frequency digit 4", new BindingValue(_lastFlightData2.BupUhfFreq / 100 % 10));
                SetValue("UHF", "Backup frequency digit 5,6", new BindingValue(_lastFlightData2.BupUhfFreq % 100));
                SetValue("Altitude", "Cabin Altitude", new BindingValue(_lastFlightData2.cabinAlt));
                SetValue("HYD", "Pressure A", new BindingValue(_lastFlightData2.hydPressureA));
                SetValue("HYD", "Pressure B", new BindingValue(_lastFlightData2.hydPressureB));
                SetValue("Time", "Time", new BindingValue(_lastFlightData2.currentTime));

                ProcessHsiBits(_lastFlightData.hsiBits, _lastFlightData.desiredCourse, _lastFlightData.bearingToBeacon, _lastFlightData2.blinkBits, _lastFlightData2.currentTime);
                ProcessLightBits(_lastFlightData.lightBits);
                ProcessLightBits2(_lastFlightData.lightBits2, _lastFlightData2.blinkBits, _lastFlightData2.currentTime);
                ProcessLightBits3(_lastFlightData.lightBits3);
            }
        }

        protected void ProcessLightBits(BMSLightBits bits)
        {
            SetValue("Left Eyebrow", "master caution indicator", new BindingValue(bits.HasFlag(BMSLightBits.MasterCaution)));
            SetValue("Left Eyebrow", "tf-fail indicator", new BindingValue(bits.HasFlag(BMSLightBits.TF)));
            SetValue("Right Eyebrow", "oxy low indicator", new BindingValue(bits.HasFlag(BMSLightBits.OXY_BROW)));
            SetValue("Right Eyebrow", "engine fire indicator", new BindingValue(bits.HasFlag(BMSLightBits.ENG_FIRE)));
            SetValue("Right Eyebrow", "hydraulic/oil indicator", new BindingValue(bits.HasFlag(BMSLightBits.HYD)));
            SetValue("Right Eyebrow", "canopy indicator", new BindingValue(bits.HasFlag(BMSLightBits.CAN)));
            SetValue("Right Eyebrow", "takeoff landing config indicator", new BindingValue(bits.HasFlag(BMSLightBits.T_L_CFG)));
            SetValue("Right Eyebrow", "flcs indicator", new BindingValue(bits.HasFlag(BMSLightBits.FLCS)));
            SetValue("Caution", "stores config indicator", new BindingValue(bits.HasFlag(BMSLightBits.CONFIG)));
            SetValue("AOA Indexer", "above indicator", new BindingValue(bits.HasFlag(BMSLightBits.AOAAbove)));
            SetValue("AOA Indexer", "on indicator", new BindingValue(bits.HasFlag(BMSLightBits.AOAOn)));
            SetValue("AOA Indexer", "below indicator", new BindingValue(bits.HasFlag(BMSLightBits.AOABelow)));
            SetValue("Refuel Indexer", "ready indicator", new BindingValue(bits.HasFlag(BMSLightBits.RefuelRDY)));
            SetValue("Refuel Indexer", "air/nws indicator", new BindingValue(bits.HasFlag(BMSLightBits.RefuelAR)));
            SetValue("Refuel Indexer", "disconnect indicator", new BindingValue(bits.HasFlag(BMSLightBits.RefuelDSC)));
            SetValue("Caution", "flight control system indicator", new BindingValue(bits.HasFlag(BMSLightBits.FltControlSys)));
            SetValue("Caution", "leading edge flaps indicator", new BindingValue(bits.HasFlag(BMSLightBits.LEFlaps)));
            SetValue("Caution", "engine fault indticator", new BindingValue(bits.HasFlag(BMSLightBits.EngineFault)));
            SetValue("Caution", "equip hot indicator", new BindingValue(bits.HasFlag(BMSLightBits.EQUIP_HOT)));
            SetValue("Caution", "overheat indicator", new BindingValue(bits.HasFlag(BMSLightBits.Overheat)));
            SetValue("Caution", "low fuel indicator", new BindingValue(bits.HasFlag(BMSLightBits.FuelLow)));
            SetValue("Caution", "avionics indicator", new BindingValue(bits.HasFlag(BMSLightBits.Avionics)));
            SetValue("Caution", "radar altimeter indicator", new BindingValue(bits.HasFlag(BMSLightBits.RadarAlt)));
            SetValue("Caution", "iff indicator", new BindingValue(bits.HasFlag(BMSLightBits.IFF)));
            SetValue("Caution", "ecm indicator", new BindingValue(bits.HasFlag(BMSLightBits.ECM)));
            SetValue("Caution", "hook indicator", new BindingValue(bits.HasFlag(BMSLightBits.Hook)));
            SetValue("Caution", "nws fail indicator", new BindingValue(bits.HasFlag(BMSLightBits.NWSFail)));
            SetValue("Caution", "cabin pressure indicator", new BindingValue(bits.HasFlag(BMSLightBits.CabinPress)));
            SetValue("Autopilot", "on indicator", new BindingValue(bits.HasFlag(BMSLightBits.AutoPilotOn)));
            SetValue("Misc", "tfs stanby indicator", new BindingValue(bits.HasFlag(BMSLightBits.TFR_STBY)));
            SetValue("Test Panel", "FLCS channel lamps", new BindingValue(bits.HasFlag(BMSLightBits.Flcs_ABCD)));

            //UpdateLightState(time, bits.HasFlag(BMSLightBits.OXY_BROW), blinkBits.HasFlag(BlinkBits.OXY_BROW), ref _oxyBrowLastTick, ref _oxyBrowState);
            //SetValue("Right Eyebrow", "oxy low indicator", new BindingValue(_oxyBrowState == LightState.ON));
        }

        protected void ProcessLightBits2(LightBits2 bits, BlinkBits blinkBits, int time)
        {
            bool rwrPower = bits.HasFlag(LightBits2.AuxPwr);

            SetValue("Threat Warning Prime", "handoff indicator", new BindingValue(bits.HasFlag(LightBits2.HandOff)));
            //SetValue("Threat Warning Prime", "launch indicator", new BindingValue(bits.HasFlag(LightBits2.Launch)));
            //SetValue("Threat Warning Prime", "prioirty mode indicator", new BindingValue(bits.HasFlag(LightBits2.PriMode)));
            SetValue("Threat Warning Prime", "open mode indicator", new BindingValue(bits.HasFlag(LightBits2.AuxPwr) && !bits.HasFlag(LightBits2.PriMode)));
            SetValue("Threat Warning Prime", "naval indicator", new BindingValue(bits.HasFlag(LightBits2.Naval)));
            //SetValue("Threat Warning Prime", "unknown mode indicator", new BindingValue(bits.HasFlag(LightBits2.Unk)));
            SetValue("Threat Warning Prime", "target step indicator", new BindingValue(bits.HasFlag(LightBits2.TgtSep)));
            //SetValue("Aux Threat Warning", "search indicator", new BindingValue(bits.HasFlag(LightBits2.AuxSrch)));
            SetValue("Aux Threat Warning", "activity indicator", new BindingValue(bits.HasFlag(LightBits2.AuxAct)));
            SetValue("Aux Threat Warning", "low altitude indicator", new BindingValue(bits.HasFlag(LightBits2.AuxLow)));
            SetValue("Aux Threat Warning", "power indicator", new BindingValue(bits.HasFlag(LightBits2.AuxPwr)));

            SetValue("CMDS", "Go", new BindingValue(bits.HasFlag(LightBits2.Go)));
            SetValue("CMDS", "NoGo", new BindingValue(bits.HasFlag(LightBits2.NoGo)));
            SetValue("CMDS", "Degr", new BindingValue(bits.HasFlag(LightBits2.Degr)));
            SetValue("CMDS", "Rdy", new BindingValue(bits.HasFlag(LightBits2.Rdy)));
            SetValue("CMDS", "ChaffLo", new BindingValue(bits.HasFlag(LightBits2.ChaffLo)));
            SetValue("CMDS", "FlareLo", new BindingValue(bits.HasFlag(LightBits2.FlareLo)));

            SetValue("ECM", "power indicator", new BindingValue(bits.HasFlag(LightBits2.EcmPwr)));
            SetValue("ECM", "fail indicator", new BindingValue(bits.HasFlag(LightBits2.EcmFail)));
            SetValue("Caution", "forward fuel low indicator", new BindingValue(bits.HasFlag(LightBits2.FwdFuelLow)));
            SetValue("Caution", "aft fuel low indicator", new BindingValue(bits.HasFlag(LightBits2.AftFuelLow)));
            SetValue("EPU", "on indicator", new BindingValue(bits.HasFlag(LightBits2.EPUOn)));
            SetValue("JFS", "run indicator", new BindingValue(bits.HasFlag(LightBits2.JFSOn)));
            SetValue("Caution", "second engine compressor indicator", new BindingValue(bits.HasFlag(LightBits2.SEC)));
            SetValue("Caution", "oxygen low indicator", new BindingValue(bits.HasFlag(LightBits2.OXY_LOW)));
            //SetValue("Caution", "probe heat indicator", new BindingValue(bits.HasFlag(LightBits2.PROBEHEAT)));
            SetValue("Caution", "seat arm indicator", new BindingValue(bits.HasFlag(LightBits2.SEAT_ARM)));
            SetValue("Caution", "backup fuel control indicator", new BindingValue(bits.HasFlag(LightBits2.BUC)));
            SetValue("Caution", "fuel oil hot indicator", new BindingValue(bits.HasFlag(LightBits2.FUEL_OIL_HOT)));
            SetValue("Caution", "anti skid indicator", new BindingValue(bits.HasFlag(LightBits2.ANTI_SKID)));
            SetValue("Misc", "tfs engaged indicator", new BindingValue(bits.HasFlag(LightBits2.TFR_ENGAGED)));
            SetValue("Gear Handle", "handle indicator", new BindingValue(bits.HasFlag(LightBits2.GEARHANDLE)));
            SetValue("Right Eyebrow", "engine indicator", new BindingValue(bits.HasFlag(LightBits2.ENGINE)));

            UpdateBlinkingLightState(time, bits.HasFlag(LightBits2.PROBEHEAT), blinkBits.HasFlag(BlinkBits.PROBEHEAT), ref _probeheatLastTick, ref _probeheatState);
            SetValue("Caution", "probe heat indicator", new BindingValue(_probeheatState == LightState.ON));

            UpdateBlinkingLightState(time, bits.HasFlag(LightBits2.AuxSrch), blinkBits.HasFlag(BlinkBits.AuxSrch), ref _auxsrchLastTick, ref _auxsrchState);
            SetValue("Aux Threat Warning", "search indicator", new BindingValue(_auxsrchState == LightState.ON));

            UpdateBlinkingLightState(time, bits.HasFlag(LightBits2.Launch), blinkBits.HasFlag(BlinkBits.Launch), ref _launchLastTick, ref _launchState);
            SetValue("Threat Warning Prime", "launch indicator", new BindingValue(_launchState == LightState.ON));

            UpdateBlinkingLightState(time, bits.HasFlag(LightBits2.PriMode), blinkBits.HasFlag(BlinkBits.PriMode), ref _primodeLastTick, ref _primodeState);
            SetValue("Threat Warning Prime", "open mode indicator", new BindingValue(_primodeState == LightState.ON));

            UpdateBlinkingLightState(time, bits.HasFlag(LightBits2.Unk), blinkBits.HasFlag(BlinkBits.Unk), ref _unkLastTick, ref _unkState);
            SetValue("Threat Warning Prime", "unknown mode indicator", new BindingValue(_unkState == LightState.ON));
        }

        protected void ProcessLightBits3(BMSLightBits3 bits)
        {
            SetValue("Electronic", "flcs pmg indicator", new BindingValue(bits.HasFlag(BMSLightBits3.FlcsPmg)));
            SetValue("Electronic", "main gen indicator", new BindingValue(bits.HasFlag(BMSLightBits3.MainGen)));
            SetValue("Electronic", "standby generator indicator", new BindingValue(bits.HasFlag(BMSLightBits3.StbyGen)));
            SetValue("Electronic", "epu gen indicator", new BindingValue(bits.HasFlag(BMSLightBits3.EpuGen)));
            SetValue("Electronic", "epu pmg indicator", new BindingValue(bits.HasFlag(BMSLightBits3.EpuPmg)));
            SetValue("Electronic", "to flcs indicator", new BindingValue(bits.HasFlag(BMSLightBits3.ToFlcs)));
            SetValue("Electronic", "flcs rly indicator", new BindingValue(bits.HasFlag(BMSLightBits3.FlcsRly)));
            SetValue("Electronic", "bat fail indicator", new BindingValue(bits.HasFlag(BMSLightBits3.BatFail)));
            SetValue("EPU", "hydrazine indicator", new BindingValue(bits.HasFlag(BMSLightBits3.Hydrazine)));
            SetValue("EPU", "air indicator", new BindingValue(bits.HasFlag(BMSLightBits3.Air)));
            SetValue("Caution", "electric bus fail indicator", new BindingValue(bits.HasFlag(BMSLightBits3.Elec_Fault)));
            SetValue("Caution", "lef fault indicator", new BindingValue(bits.HasFlag(BMSLightBits3.Lef_Fault)));

            SetValue("General", "on ground", new BindingValue(bits.HasFlag(BMSLightBits3.OnGround)));
            SetValue("Flight Control", "run light", new BindingValue(bits.HasFlag(BMSLightBits3.FlcsBitRun)));
            SetValue("Flight Control", "fail light", new BindingValue(bits.HasFlag(BMSLightBits3.FlcsBitFail)));
            SetValue("Right Eyebrow", "dbu on indicator", new BindingValue(bits.HasFlag(BMSLightBits3.DbuWarn)));
            SetValue("General", "parking brake engaged", new BindingValue(bits.HasFlag(BMSLightBits3.ParkBrakeOn)));
            SetValue("Caution", "cadc indicator", new BindingValue(bits.HasFlag(BMSLightBits3.cadc)));
            SetValue("General", "speed barke", new BindingValue(bits.HasFlag(BMSLightBits3.SpeedBrake)));

            SetValue("Landing Gear", "nose gear indicator", new BindingValue(bits.HasFlag(BMSLightBits3.NoseGearDown)));
            SetValue("Landing Gear", "left gear indicator", new BindingValue(bits.HasFlag(BMSLightBits3.LeftGearDown)));
            SetValue("Landing Gear", "right gear indicator", new BindingValue(bits.HasFlag(BMSLightBits3.RightGearDown)));
            SetValue("General", "power off", new BindingValue(bits.HasFlag(BMSLightBits3.Power_Off)));

            //UpdateLightState(time, bits.HasFlag(BMSLightBits3.Elec_Fault), blinkBits.HasFlag(BlinkBits.Elec_Fault), ref _elecFaultLastTick, ref _elecFaultState);
            //SetValue("Blinklight State", "elec fault", new BindingValue(_elecFaultState == LightState.ON));

            //UpdateLightState(time, bits.HasFlag(LightBits2.EPUOn), blinkBits.HasFlag(BlinkBits.EPUOn), ref _epuOnLastTick, ref _epuOnState);
            //SetValue("Blinklight State", "epu on", new BindingValue(_epuOnState == LightState.ON));

            //UpdateLightState(time, bits.HasFlag(LightBits2.JFSOn), blinkBits.HasFlag(BlinkBits.JFSOn_Slow) | blinkBits.HasFlag(BlinkBits.JFSOn_Fast), ref _jfsOnLastTick, ref _jfsOnState);
            //SetValue("Blinklight State", "JFS on", new BindingValue(_jfsOnState == LightState.ON));
        }

        protected void ProcessHsiBits(HsiBits bits, float desiredCourse, float bearingToBeacon, BlinkBits blinkBits, int time)
        {
            SetValue("HSI", "to flag", new BindingValue(bits.HasFlag(HsiBits.ToTrue)));
            SetValue("HSI", "from flag", new BindingValue(bits.HasFlag(HsiBits.FromTrue)));

            SetValue("HSI", "ils warning flag", new BindingValue(bits.HasFlag(HsiBits.IlsWarning)));
            SetValue("HSI", "course warning flag", new BindingValue(bits.HasFlag(HsiBits.CourseWarning)));
            SetValue("HSI", "off flag", new BindingValue(bits.HasFlag(HsiBits.HSI_OFF)));
            SetValue("HSI", "init flag", new BindingValue(bits.HasFlag(HsiBits.Init)));
            SetValue("ADI", "off flag", new BindingValue(bits.HasFlag(HsiBits.ADI_OFF)));
            SetValue("ADI", "aux flag", new BindingValue(bits.HasFlag(HsiBits.ADI_AUX)));
            SetValue("ADI", "gs flag", new BindingValue(bits.HasFlag(HsiBits.ADI_GS)));
            SetValue("ADI", "loc flag", new BindingValue(bits.HasFlag(HsiBits.ADI_LOC)));
            SetValue("Backup ADI", "off flag", new BindingValue(bits.HasFlag(HsiBits.BUP_ADI_OFF)));
            SetValue("VVI", "off flag", new BindingValue(bits.HasFlag(HsiBits.VVI)));
            SetValue("AOA", "off flag", new BindingValue(bits.HasFlag(HsiBits.AOA)));

            UpdateBlinkingLightState(time, bits.HasFlag(HsiBits.OuterMarker), blinkBits.HasFlag(BlinkBits.OuterMarker), ref _outerMarkerLastTick, ref _outerMarkerState);
            SetValue("HSI", "Outer marker indicator", new BindingValue(_outerMarkerState == LightState.ON));

            UpdateBlinkingLightState(time, bits.HasFlag(HsiBits.MiddleMarker), blinkBits.HasFlag(BlinkBits.MiddleMarker), ref _middleMarkerLastTick, ref _middleMarkerState);
            SetValue("HSI", "Middle marker indicator", new BindingValue(_middleMarkerState == LightState.ON));
        }

        protected void ProcessAltBits(AltBits bits)
        {
            SetValue("AltBits", "calibration in inches of mercury (hg) otherwise hector pascal (hPa) ", new BindingValue(bits.HasFlag(AltBits.CalType)));
            SetValue("AltBits", "pneu flag is visible", new BindingValue(bits.HasFlag(AltBits.PneuFlag)));
        }

        protected void ProcessPowerBits(PowerBits bits)
        {
            SetValue("POWER", "bus power battery", new BindingValue(bits.HasFlag(PowerBits.BusPowerBattery)));
            SetValue("POWER", "bus power emergency", new BindingValue(bits.HasFlag(PowerBits.BusPowerEmergency)));
            SetValue("POWER", "bus power essential", new BindingValue(bits.HasFlag(PowerBits.BusPowerEssential)));
            SetValue("POWER", "bus power non essential", new BindingValue(bits.HasFlag(PowerBits.BusPowerNonEssential)));
            SetValue("POWER", "main generator", new BindingValue(bits.HasFlag(PowerBits.MainGenerator)));
            SetValue("POWER", "standby generator", new BindingValue(bits.HasFlag(PowerBits.StandbyGenerator)));
            SetValue("POWER", "Jetfuel starter", new BindingValue(bits.HasFlag(PowerBits.JetFuelStarter)));
        }

        //protected void ProcessBlinkBits(BlinkBits bits)
        //{
        //    SetValue("Blink", "Outer marker", new BindingValue(bits.HasFlag(BlinkBits.OuterMarker)));
        //    SetValue("Blink", "Middle marker", new BindingValue(bits.HasFlag(BlinkBits.MiddleMarker)));
        //    SetValue("Blink", "probeheat", new BindingValue(bits.HasFlag(BlinkBits.PROBEHEAT)));
        //    SetValue("Blink", "aux search", new BindingValue(bits.HasFlag(BlinkBits.AuxSrch)));
        //    SetValue("Blink", "launch", new BindingValue(bits.HasFlag(BlinkBits.Launch)));
        //    SetValue("Blink", "primary mode", new BindingValue(bits.HasFlag(BlinkBits.PriMode)));
        //    SetValue("Blink", "unknown", new BindingValue(bits.HasFlag(BlinkBits.Unk)));
        //    SetValue("Blink", "elec fault", new BindingValue(bits.HasFlag(BlinkBits.Elec_Fault)));
        //    SetValue("Blink", "oxy brow", new BindingValue(bits.HasFlag(BlinkBits.OXY_BROW)));
        //    SetValue("Blink", "epu on", new BindingValue(bits.HasFlag(BlinkBits.EPUOn)));
        //    SetValue("Blink", "JFS on_slow", new BindingValue(bits.HasFlag(BlinkBits.JFSOn_Slow)));
        //    SetValue("Blink", "JFS on_fast", new BindingValue(bits.HasFlag(BlinkBits.JFSOn_Fast)));
        //}

        protected void UpdateBlinkingLightState(int time, bool on, bool blinking, ref int lastTick, ref LightState state)
        {
            switch (state)
            {
                case LightState.OFF:
                    if(on)
                    {
                        state = LightState.ON;
                        lastTick = time;
                    }
                    break;
                case LightState.ON:
                    if(!on)
                        state = LightState.OFF;
                    else if (blinking)
                    {
                        int delta = negMod(time - lastTick, MAX_SECONDS);

                        if (delta > PHASE_LENGTH)
                        {
                            state = LightState.TempOFF;
                            lastTick = time;
                        }
                    }
                    break;
                case LightState.TempOFF:
                    if (!on)
                        state = LightState.OFF;
                    else if (blinking)
                    {
                        int delta = negMod(time - lastTick, MAX_SECONDS);

                        if (delta > PHASE_LENGTH)
                        {
                            state = LightState.ON;
                            lastTick = time;
                        }
                    }
                    else
                    {
                        state = LightState.ON;
                        lastTick = time;
                    }
                    break;
            }
        }

        private float ClampValue(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        internal override void CloseData()
        {
            _sharedMemory.Close();
            _sharedMemory.Dispose();
            _sharedMemory = null;

            _sharedMemory2.Close();
            _sharedMemory2.Dispose();
            _sharedMemory2 = null;
        }

        override internal RadarContact[] RadarContacts
        {
            get
            {
                return _contacts;
            }
        }

        private void ProcessContacts(FlightData flightData)
        {
            for(int i = 0; i < flightData.RWRsymbol.Length; i++)
            {
                _contacts[i].Symbol = (RadarSymbols)flightData.RWRsymbol[i];
                _contacts[i].Selected = flightData.selected[i] > 0;
                _contacts[i].Bearing = flightData.bearing[i] * 57.3f;
                _contacts[i].RelativeBearing = (-flightData.currentHeading + _contacts[i].Bearing) % 360d;
                _contacts[i].Lethality = flightData.lethality[i];
                _contacts[i].MissileActivity = flightData.missileActivity[i] > 0;
                _contacts[i].MissileLaunch = flightData.missileLaunch[i] > 0;
                _contacts[i].NewDetection = flightData.newDetection[i] > 0;
                _contacts[i].Visible = i < flightData.RwrObjectCount;
            }
        }

        //https://stackoverflow.com/questions/1878907/the-smallest-difference-between-2-angles
        private int negMod(int a, int n)
        {
            return (a % n + n) % n;
        }
    }
}

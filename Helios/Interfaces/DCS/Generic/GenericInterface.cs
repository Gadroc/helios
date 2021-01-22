﻿//  Copyright 2014 Craig Courtney
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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.Generic
{
    using GadrocsWorkshop.Helios.ComponentModel;
    //using GadrocsWorkshop.Helios.Interfaces.DCS.Generic.Functions;
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using Microsoft.Win32;
    using System;

    [HeliosInterface("Helios.DCS", "DCS Generic Interface", typeof(GenericInterfaceEditor), typeof(UniqueHeliosInterfaceFactory))]
    public class GenericInterface : BaseUDPInterface
    {
        private string _dcsPath;

        private bool _phantomFix;
        private int _phantomLeft;
        private int _phantomTop;

        private long _nextCheck = 0;

        #region Devices
        private const string PUSH_BUTTONS = "1";
        private const string TOGGLE_SWITCH = "2";
        private const string THREEWAYSWITCH = "3";
        private const string AXIS = "4";
        private const string MULTI_POS_SWITCH = "5";
        private const string ROCKER_ABCC = "7";
        private const string ROCKER_ABAB = "8";
        private const string ROTARY_ENCODER = "9";
        private const string ROCKER_AABB = "10";
        private const string INDICATOR_PUSHBUTTON = "11";
		private const string TOGGLE_SWITCH_B = "12";

		#endregion

		public GenericInterface()
            : base("DCS Generic")
        {
            DCSConfigurator config = new DCSConfigurator("DCSGeneric", DCSPath);
            config.ExportConfigPath = "Config\\Export";
            config.ExportFunctionsPath = "pack://application:,,,/Helios;component/Interfaces/DCS/Generic/ExportFunctions.lua";
            Port = config.Port;
            _phantomFix = config.PhantomFix;
            _phantomLeft = config.PhantomFixLeft;
            _phantomTop = config.PhantomFixTop;

            int indice = 0;
            int contador = 0;

            for (int i = 1; i < 200; i++)   // 200 Network values
            {
                Functions.Add(new NetworkValue(this, i.ToString(), "NetWork Values", "NetWorkValue_" + i.ToString(), "Float values from DCS", "", BindingValueUnits.Numeric, null));
            }

            indice = 1000;
            for (int i = 1; i < 200; i++)   // 200 flag values
            {
                AddFunction(new FlagValue(this, (indice + i).ToString(), "Flag Values", "FlagValue_" + i.ToString(), "Used for lights, usually, 1=ON 0=OFF"));
            }

            indice = 2000;
            for (int i = 1; i < 200; i++)   // 200 push buttons
            {
                AddFunction(new PushButton(this, PUSH_BUTTONS, (3000 + i).ToString(), (indice + i).ToString(), "Push Buttons", "PB_" + i.ToString()));
            }

            indice = 3000;
            for (int i = 1; i < 250; i++)       // 250 Toggle Switch 1, 0
            {
                AddFunction(Switch.CreateToggleSwitch(this, TOGGLE_SWITCH, (3000 + i).ToString(), (indice + i).ToString(), "1", "UP", "0", "DOWN", "Toggle Switch 1,0", "TSwitch_" + i.ToString(), "%1d"));
            }


            indice = 4000;
            for (int i = 1; i < 250; i++)       // 250 Toggle Switch 1, -1
            {
                AddFunction(Switch.CreateToggleSwitch(this, TOGGLE_SWITCH_B, (3000 + i).ToString(), (indice + i).ToString(), "1", "UP", "-1", "DOWN", "Toggle Switch 1,-1", "TSwitch_B_" + i.ToString(), "%1d"));
            }



            indice = 5000;
            for (int i = 1; i < 100; i++)    // 100 Threeway Switch 1,0,-1
            {
                AddFunction(Switch.CreateThreeWaySwitch(this, THREEWAYSWITCH, (3000 + i).ToString(), (indice + i).ToString(), "1", "UP", "0", "CENTER", "-1", "DOWN", "Threeway Switch 1,0,-1", "3WSwitch_A_" + i.ToString(), "%1d"));
            }

            indice = 5000;
            for (int i = 101; i < 200; i++)    // 100 Threeway Switch  0  0.1  0.2
            {
                AddFunction(Switch.CreateThreeWaySwitch(this, THREEWAYSWITCH, (3000 + i).ToString(), (indice + i).ToString(), "0.0", "UP", "0.1", "CENTER", "0.2", "DOWN", "Threeway Switch 0,0.1,0.2", "3WSwitch_B_" + i.ToString(), "%0.2f"));
            }

            indice = 5000;
            for (int i = 201; i < 300; i++)    // 100 Threeway Switch  0   0.5   1
            {
                AddFunction(Switch.CreateThreeWaySwitch(this, THREEWAYSWITCH, (3000 + i).ToString(), (indice + i).ToString(), "0.0", "UP", "0.5", "CENTER", "1.0", "DOWN", "Threeway Switch 0,0.5,1", "3WSwitch_C_" + i.ToString(), "%0.2f"));
            }

            indice = 6000;
            for (int i = 1; i < 100; i++)  // 100 axis 0.1
            {
                AddFunction(new Axis(this, AXIS, (3000 + i).ToString(), (indice + i).ToString(), 0.1d, 0.0d, 1.0d, "Axis 0.1", "Axis_A_" + i.ToString()));
            }

            indice = 6000;
            for (int i = 101; i < 200; i++)  // 100 axis 0.05
            {
                AddFunction(new Axis(this, AXIS, (3000 + i).ToString(), (indice + i).ToString(), 0.05d, 0.0d, 1.0d, "Axis 0.05", "Axis_B_" + i.ToString()));
            }


            indice = 7000;
            for (int i = 1; i < 50; i++)   // 50 multiposition 6 pos
            {
                AddFunction(new Switch(this, MULTI_POS_SWITCH, (indice + i).ToString(), new SwitchPosition[] { new SwitchPosition("0.0", "POS_01", (3000 + i).ToString()),
                    new SwitchPosition("0.1", "POS_02", (3000 + i).ToString()),
                    new SwitchPosition("0.2", "POS_03", (3000 + i).ToString()),
                    new SwitchPosition("0.3", "POS_04", (3000 + i).ToString()),
                    new SwitchPosition("0.4", "POS_05", (3000 + i).ToString()),
                    new SwitchPosition("0.5", "POS_06", (3000 + i).ToString()) },
                    "Multi 6 pos Switches", "Multi6PosSwitch_" + i.ToString(), "%0.2f"));
            }

            for (int i = 51; i < 100; i++)   // 50 multiposition 11 pos
            {
                AddFunction(new Switch(this, MULTI_POS_SWITCH, (indice + i).ToString(), new SwitchPosition[] { new SwitchPosition("0.0", "POS_01", (3000 + i).ToString()),
                    new SwitchPosition("0.1", "POS_02", (3000 + i).ToString()),
                    new SwitchPosition("0.2", "POS_03", (3000 + i).ToString()),
                    new SwitchPosition("0.3", "POS_04", (3000 + i).ToString()),
                    new SwitchPosition("0.4", "POS_05", (3000 + i).ToString()),
                    new SwitchPosition("0.5", "POS_06", (3000 + i).ToString()),
                    new SwitchPosition("0.6", "POS_07", (3000 + i).ToString()),
                    new SwitchPosition("0.7", "POS_08", (3000 + i).ToString()),
                    new SwitchPosition("0.8", "POS_09", (3000 + i).ToString()),
                    new SwitchPosition("0.9", "POS_10", (3000 + i).ToString()),
                    new SwitchPosition("1,0", "POS_11", (3000 + i).ToString()), }, "Multi 11 pos Switches", "Multi11PosSwitch_" + i.ToString(), "%0.2f"));
            }

            for (int i = 101; i < 121; i++)   // 20 multiposition 21 pos
            {
                AddFunction(new Switch(this, MULTI_POS_SWITCH, (indice + i).ToString(), new SwitchPosition[] { new SwitchPosition("0.0", "POS_01", (3000 + i).ToString()),
                    new SwitchPosition("0.05", "POS_02", (3000 + i).ToString()),
                    new SwitchPosition("0.1", "POS_03", (3000 + i).ToString()),
                    new SwitchPosition("0.15", "POS_04", (3000 + i).ToString()),
                    new SwitchPosition("0.2", "POS_05", (3000 + i).ToString()),
                    new SwitchPosition("0.25", "POS_06", (3000 + i).ToString()),
                    new SwitchPosition("0.3", "POS_07", (3000 + i).ToString()),
                    new SwitchPosition("0.35", "POS_08", (3000 + i).ToString()),
                    new SwitchPosition("0.4", "POS_09", (3000 + i).ToString()),
                    new SwitchPosition("0.45", "POS_10", (3000 + i).ToString()),
                    new SwitchPosition("0.5", "POS_11", (3000 + i).ToString()),
                    new SwitchPosition("0.55", "POS_12", (3000 + i).ToString()),
                    new SwitchPosition("0.6", "POS_13", (3000 + i).ToString()),
                    new SwitchPosition("0.65", "POS_14", (3000 + i).ToString()),
                    new SwitchPosition("0.7", "POS_15", (3000 + i).ToString()),
                    new SwitchPosition("0.75", "POS_16", (3000 + i).ToString()),
                    new SwitchPosition("0.8", "POS_17", (3000 + i).ToString()),
                    new SwitchPosition("0.85", "POS_18", (3000 + i).ToString()),
                    new SwitchPosition("0.9", "POS_19", (3000 + i).ToString()),
                    new SwitchPosition("0.95", "POS_20", (3000 + i).ToString()),
                    new SwitchPosition("1,0", "POS_21", (3000 + i).ToString()), }, "Multi 21 pos Switches", "Multi21PosSwitch_" + i.ToString(), "%0.2f"));
            }

            indice = 8000;
            contador = 1;
            for (int i = 1; i < 150; i = i + 3)   // 50 rockers ABCC
            {
                AddFunction(new Rocker(this, ROCKER_ABCC, (3000 + i).ToString(), (3001 + i).ToString(), (3002 + i).ToString(), (3002 + i).ToString(), (indice + contador).ToString(), "Rocker type ABCC", "Rocker_A_" + contador.ToString(), true));
                contador++;
            }


            for (int i = 1; i < 100; i = i + 2)   // 50 rockers ABAB
            {
                AddFunction(new Rocker(this, ROCKER_ABAB, (3000 + i).ToString(), (3001 + i).ToString(), (3000 + i).ToString(), (3001 + i).ToString(), (indice + contador).ToString(), "Rocker type ABAB", "Rocker_B_" + contador.ToString(), true));
                contador++;
            }

            for (int i = 1; i < 100; i = i + 2)   // 50 rockers AABB
            {
                AddFunction(new Rocker(this, ROCKER_AABB, (3000 + i).ToString(), (3000 + i).ToString(), (3001 + i).ToString(), (3001 + i).ToString(), (indice + contador).ToString(), "Rocker type AABB", "Rocker_C_" + contador.ToString(), true));
                contador++;
            }



            indice = 9000;
            for (int i = 1; i < 50; i++)   // 50 rotary encoders 0.1
            {
                AddFunction(new RotaryEncoder(this, ROTARY_ENCODER, (3000 + i).ToString(), (indice + i).ToString(), 0.1d, "Rotary Encoder 0.1", "RotEncode_A_" + i.ToString()));
            }

            for (int i = 51; i < 100; i++)   // 50 rotary encoders 0.05
            {
                AddFunction(new RotaryEncoder(this, ROTARY_ENCODER, (3000 + i).ToString(), (indice + i).ToString(), 0.05d, "Rotary Encoder 0.05", "RotEncoder_B_" + i.ToString()));
            }

            indice = 10000;
            for (int i = 1; i < 100; i++)   // 100 indicator pushbuttons
            {
                AddFunction(new IndicatorPushButton(this, INDICATOR_PUSHBUTTON, (3000 + i).ToString(), (indice + i).ToString(), "Indicator pushbutton", "Ind_PButton_" + i.ToString()));
            }

        }

        private string DCSPath
        {
            get
            {
                if (_dcsPath == null)
                {
                    RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS World");
                    if (pathKey != null)
                    {
                        _dcsPath = (string)pathKey.GetValue("Path");
                        pathKey.Close();
                        ConfigManager.LogManager.LogDebug("DCS Generic Interface Editor - Found DCS Path (Path=\"" + _dcsPath + "\")");
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

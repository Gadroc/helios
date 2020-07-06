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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.MiG21
{
    using GadrocsWorkshop.Helios.Interfaces.DCS.Common;
    using GadrocsWorkshop.Helios.UDPInterface;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using Microsoft.Win32;
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MiG21InterfaceEditor.xaml
    /// </summary>
    public partial class MiG21InterfaceEditor : HeliosInterfaceEditor
    {
        static MiG21InterfaceEditor()
        {
            Type ownerType = typeof(MiG21InterfaceEditor);

            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(DCSConfigurator.AddDoFile, AddDoFile_Executed));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(DCSConfigurator.RemoveDoFile, RemoveDoFile_Executed));
        }

        private static void AddDoFile_Executed(object target, ExecutedRoutedEventArgs e)
        {
            MiG21InterfaceEditor editor = target as MiG21InterfaceEditor;
            string file = e.Parameter as string;
            if (editor != null && !string.IsNullOrWhiteSpace(file) && !editor.Configuration.DoFiles.Contains(file))
            {
                editor.Configuration.DoFiles.Add((string)e.Parameter);
                editor.NewDoFile.Text = "";
            }
        }

        private static void RemoveDoFile_Executed(object target, ExecutedRoutedEventArgs e)
        {
            MiG21InterfaceEditor editor = target as MiG21InterfaceEditor;
            string file = e.Parameter as string;
            if (editor != null && !string.IsNullOrWhiteSpace(file) && editor.Configuration.DoFiles.Contains(file))
            {
                editor.Configuration.DoFiles.Remove(file);
            }
        }

        private string _dcsPath = null;

        public MiG21InterfaceEditor()
        {
            InitializeComponent();
            Configuration = new DCSConfigurator("MiG21", "pack://application:,,,/Helios;component/Interfaces/DCS/MiG21/Export.lua", DCSPath, true);
            Configuration.ExportConfigPath = "Config\\Export";
            Configuration.ExportFunctionsPath = "pack://application:,,,/Helios;component/Interfaces/DCS/MiG21/ExportFunctions.lua";
        }

        #region Properties

        public DCSConfigurator Configuration
        {
            get { return (DCSConfigurator)GetValue(ConfigurationProperty); }
            set { SetValue(ConfigurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Configuration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfigurationProperty =
            DependencyProperty.Register("Configuration", typeof(DCSConfigurator), typeof(MiG21InterfaceEditor), new PropertyMetadata(null));

        public string DCSPath
        {
            get
            {
                if (_dcsPath == null)
                {
                    RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS World");
                    if (pathKey == null)
                    {
                        pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS World");
                    }
                    if (pathKey == null)
                    {
                        pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS World OpenBeta");
                    }
                    if (pathKey == null)
                    {
                        pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS World OpenAlpha");
                    }
                    if (pathKey != null)
                    {
                        _dcsPath = (string)pathKey.GetValue("Path");
                        pathKey.Close();
                        ConfigManager.LogManager.LogDebug("DCS MiG21 Interface Editor - Found DCS Path (Path=\"" + _dcsPath + "\")");
                    }
                    else
                    {
                        _dcsPath = "";
                    }
                }
                return _dcsPath;
            }
        }

        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == InterfaceProperty)
            {
                Configuration.UDPInterface = Interface as BaseUDPInterface;
            }

            base.OnPropertyChanged(e);
        }

        private void Configure_Click(object sender, RoutedEventArgs e)
        {
            if (Configuration.UpdateExportConfig())
            {
                MessageBox.Show(Window.GetWindow(this), "DCS MiG21 has been configured.");
            }
            else
            {
                MessageBox.Show(Window.GetWindow(this), "Error updating DCS MiG21 configuration.  Please do one of the following and try again:\n\nOption 1) Run Helios as Administrator\nOption 2) Install DCS outside the Program Files Directory\nOption 3) Disable UAC.");
            }
        }

        private void ResetPath(object sender, RoutedEventArgs e)
        {
            if (Configuration != null)
            {
                Configuration.AppPath = DCSPath;
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Configuration.RestoreConfig();
        }
    }
}

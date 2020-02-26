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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.Common
{
    using GadrocsWorkshop.Helios.UDPInterface;
    using GadrocsWorkshop.Helios.Util;
    using Microsoft.Win32;
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Input;

    public class DCSConfigurator : NotificationObject
    {
        private Guid FolderSavedGames = new Guid("4C5C32FF-BB9D-43b0-B5B4-2D72E54EAAA4");

        private static RoutedUICommand AddDoFileCommand = new RoutedUICommand("Adds a do file to a DCS config.", "AddDoFile", typeof(DCSConfigurator));
        private static RoutedUICommand RemoveDoFileCommand = new RoutedUICommand("Removes a do file to a DCS config.", "RemoveDoFile", typeof(DCSConfigurator));

        public static RoutedUICommand AddDoFile { get { return AddDoFileCommand; } }
        public static RoutedUICommand RemoveDoFile { get { return RemoveDoFileCommand; } }

        private bool _allowDCSWorld = true;

        private string _defaultAppPath;
        private string _appPath;
        private string _savedgamesPath = "";

        private bool _phantomFix;
        private int _phantomFixLeft;
        private int _phantomFixTop;

        private string _prefPrefix = "";
        private string _mainExportLUA = "";

        private string _heliosExportLUA = "";
        private string _heliosExportLUAMD5 = "";

        private BaseUDPInterface _udpInterface;

        private string _exportConfigPath = "";
        private string _exportFunctionsPath = "";

        private string _ipAddress = "127.0.0.1";
        private int _port = 9089;

        private int _exportFrequency = 15;

        private bool _isConfigUpToDate = false;
        private bool _isPathValid = false;

        private string _dcsInstallType = "GA";

        private bool _dcsInstallTypeGA = true;
        private bool _dcsInstallTypeAlpha = false;
        private bool _dcsInstallTypeBeta = false;

        private bool _dcsUseNewExport = false;

        private ObservableCollection<string> _doFiles = new ObservableCollection<string>();

        public DCSConfigurator(string preferencesPrefix, string defaultAppPath) : this(preferencesPrefix, defaultAppPath, true)
        {
        }

        public DCSConfigurator(string preferencesPrefix, string defaultAppPath, bool allowDCSWorld)
            : this(preferencesPrefix, "pack://application:,,,/Helios;component/Interfaces/DCS/Common/Export.lua", defaultAppPath, allowDCSWorld)
        {
        }

        public DCSConfigurator(string preferencesPrefix, string exportLuaPath, string defaultAppPath) : this(preferencesPrefix, exportLuaPath, defaultAppPath, true)
        {
        }

        public DCSConfigurator(string preferencesPrefix, string exportLuaPath, string defaultAppPath, bool allowDCSWorld) 
        {
            _allowDCSWorld = allowDCSWorld;
            _defaultAppPath = defaultAppPath;
            _dcsInstallType = "";

            _prefPrefix = preferencesPrefix;
            _mainExportLUA = exportLuaPath;
            _ipAddress = ConfigManager.SettingsManager.LoadSetting(_prefPrefix, "IPAddress", "127.0.0.1");
            _exportFrequency = ConfigManager.SettingsManager.LoadSetting(_prefPrefix, "ExportFrequency", 15);
            _port = int.Parse(ConfigManager.SettingsManager.LoadSetting(_prefPrefix, "Port", "9089"), CultureInfo.InvariantCulture);
            _appPath = ConfigManager.SettingsManager.LoadSetting(_prefPrefix, "Path", _defaultAppPath);

            _phantomFix = ConfigManager.SettingsManager.LoadSetting(_prefPrefix, "PhantomMonitorFix", false);
            _phantomFixLeft = int.Parse(ConfigManager.SettingsManager.LoadSetting(_prefPrefix, "PhantomMonitorLeft", "0"), CultureInfo.InvariantCulture);
            _phantomFixTop = int.Parse(ConfigManager.SettingsManager.LoadSetting(_prefPrefix, "PhantomMonitorTop", "0"), CultureInfo.InvariantCulture);

            string savedDoFiles = ConfigManager.SettingsManager.LoadSetting(_prefPrefix, "DoFiles", "");
            foreach (string file in savedDoFiles.Split(','))
            {
                if (!string.IsNullOrWhiteSpace(file))
                {
                    _doFiles.Add(file);
                }
            }

            UpdateHeliosProperties();

            _doFiles.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(DoFiles_CollectionChanged);
        }

        void DoFiles_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Save Config
            ConfigManager.SettingsManager.SaveSetting(_prefPrefix, "DoFiles", string.Join(",", DoFiles));
            UpdateHeliosProperties();
        }

        #region Properties

        public bool IsPathValid
        {
            get
            {
                return _isPathValid;
            }
            private set
            {
                if (!_isPathValid.Equals(value))
                {
                    bool oldValue = _isPathValid;
                    _isPathValid = value;
                    OnPropertyChanged("IsPathValid", oldValue, value, false);
                }
            }
        }

        public string AppPath
        {
            get
            {
                return _appPath;
            }
            set
            {
                if (!string.Equals(_appPath, value))
                {
                    string oldValue = _appPath;
                    _appPath = value;
                    OnPropertyChanged("AppPath", oldValue, value, false);
                    ConfigManager.SettingsManager.SaveSetting(_prefPrefix, "Path", _appPath);
                    IsPathValid = CheckPath();
                    IsUpToDate = CheckConfig();
                }
            }
        }

        public bool PhantomFix
        {
            get
            {
                return _phantomFix;
            }
            set
            {
                if (!_phantomFix.Equals(value))
                {
                    bool oldValue = _phantomFix;
                    _phantomFix = value;
                    OnPropertyChanged("PhantomFix", oldValue, value, false);
                    ConfigManager.SettingsManager.SaveSetting(_prefPrefix, "PhantomMonitorFix", _phantomFix);
                }
            }
        }

        public int PhantomFixLeft
        {
            get
            {
                return _phantomFixLeft;
            }
            set
            {
                if (!_phantomFixLeft.Equals(value))
                {
                    int oldValue = _phantomFixLeft;
                    _phantomFixLeft = value;
                    OnPropertyChanged("PhantomFixLeft", oldValue, value, false);
                    ConfigManager.SettingsManager.SaveSetting(_prefPrefix, "PhantomMonitorLeft", _phantomFixLeft.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        public int PhantomFixTop
        {
            get
            {
                return _phantomFixTop;
            }
            set
            {
                if (!_phantomFixTop.Equals(value))
                {
                    int oldValue = _phantomFixTop;
                    _phantomFixTop = value;
                    OnPropertyChanged("PhantomFixTop", oldValue, value, false);
                    ConfigManager.SettingsManager.SaveSetting(_prefPrefix, "PhantomMonitorTop", _phantomFixTop.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        public int ExportFrequency
        {
            get
            {
                return _exportFrequency;
            }
            set
            {
                if (!_exportFrequency.Equals(value))
                {
                    int oldValue = _exportFrequency;
                    _exportFrequency = value;
                    OnPropertyChanged("ExportFrequency", oldValue, value, true);
                    UpdateHeliosProperties();
                    ConfigManager.SettingsManager.SaveSetting(_prefPrefix, "ExportFrequency", _exportFrequency);
                }
            }
        }

        public ObservableCollection<string> DoFiles
        {
            get { return _doFiles; }
        }

        public BaseUDPInterface UDPInterface
        {
            get
            {
                return _udpInterface;
            }
            set
            {
                if ((_udpInterface == null && value != null)
                    || (_udpInterface != null && !_udpInterface.Equals(value)))
                {

                    BaseUDPInterface oldValue = _udpInterface;
                    if (oldValue != null)
                    {
                        oldValue.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Interface_PropertyChanged);
                        oldValue.Functions.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Functions_CollectionChanged);
                    }

                    _udpInterface = value;
                    UpdateHeliosProperties();
                    _udpInterface.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Interface_PropertyChanged);
                    _udpInterface.Functions.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Functions_CollectionChanged);

                    OnPropertyChanged("UDPInterface", oldValue, value, false);
                }
            }
        }

        public string ExportAppPath
        {
            get
            {
                if (_allowDCSWorld)
                {
                    if (InstallTypeGA)
                    {
                        return System.IO.Path.Combine(SavedGamesFolder, "DCS");
                    }
                    else
                    {
                        if (InstallTypeBeta)
                        {
                            return System.IO.Path.Combine(SavedGamesFolder, "DCS.openbeta");
                        }
                        else
                        {
                            if (InstallTypeAlpha)
                            {
                                return System.IO.Path.Combine(SavedGamesFolder, "DCS.openalpha");
                            }
                        }
                    }
                }
                return AppPath;
            }
        }

        public string ExportConfigPath
        {
            get
            {
                if (_allowDCSWorld)
                {
                    RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS World");
                    if (pathKey != null)
                    {
                        pathKey.Close();
                        return "Scripts";
                    }
                    pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS World OpenBeta");
                    if (pathKey != null)
                    {
                        pathKey.Close();
                        return "Scripts";
                    }
                    pathKey = Registry.CurrentUser.OpenSubKey(@"Software\Eagle Dynamics\DCS World OpenAlpha");
                    if (pathKey != null)
                    {
                        pathKey.Close();
                        return "Scripts";
                    }


                }
                return _exportConfigPath;
            }
            set
            {
                if ((_exportConfigPath == null && value != null)
                    || (_exportConfigPath != null && !_exportConfigPath.Equals(value)))
                {
                    string oldValue = _exportConfigPath;
                    _exportConfigPath = value;
                    OnPropertyChanged("ExportConfigPath", oldValue, value, false);
                    IsPathValid = CheckPath();
                    IsUpToDate = CheckConfig();
                }
            }
        }

        public string IPAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                if ((_ipAddress == null && value != null)
                    || (_ipAddress != null && !_ipAddress.Equals(value)))
                {
                    string oldValue = _ipAddress;
                    _ipAddress = value;
                    OnPropertyChanged("IPAddress", oldValue, value, false);
                    UpdateHeliosProperties();
                    ConfigManager.SettingsManager.SaveSetting(_prefPrefix, "IPAddress", _ipAddress);
                }
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
            set
            {
                if (!_port.Equals(value))
                {
                    int oldValue = _port;
                    _port = value;
                    OnPropertyChanged("Port", oldValue, value, false);
                    UpdateHeliosProperties();
                    ConfigManager.SettingsManager.SaveSetting(_prefPrefix, "Port", _port.ToString(CultureInfo.InvariantCulture));
                    if (UDPInterface != null)
                    {
                        UDPInterface.Port = Port;
                    }

                }
            }
        }

        public string ExportFunctionsPath
        {
            get
            {
                return _exportFunctionsPath;
            }
            set
            {
                if ((_exportFunctionsPath == null && value != null)
                    || (_exportFunctionsPath != null && !_exportFunctionsPath.Equals(value)))
                {
                    string oldValue = _exportFunctionsPath;
                    _exportFunctionsPath = value;
                    OnPropertyChanged("ExportFunctionsPath", oldValue, value, false);
                    UpdateHeliosProperties();
                }
            }
        }

        public bool IsUpToDate
        {
            get
            {
                return _isConfigUpToDate;
            }
            private set
            {
                if (!_isConfigUpToDate.Equals(value))
                {
                    bool oldValue = _isConfigUpToDate;
                    _isConfigUpToDate = value;
                    OnPropertyChanged("IsUpToDate", oldValue, value, false);
                }
            }
        }

        #endregion
        public bool UpdateExportConfig()
        {
            return UpdateExportConfig("Export.lua");
        }

        public bool UpdateExportConfig(String exportFile)
        {
            string exportLuaPath = System.IO.Path.Combine(ExportAppPath, ExportConfigPath, exportFile);

            try
            {
                if (File.Exists(exportLuaPath))
                {
                    string currentHash = Hash.GetMD5HashFromFile(exportLuaPath);
                    if (!currentHash.Equals(_heliosExportLUAMD5))
                    {
                        string backupFile = exportLuaPath + ".back";
                        if (!File.Exists(backupFile))
                        {
                            File.Move(exportLuaPath, backupFile);
                        }
                    }
                    File.Delete(exportLuaPath);
                }

                StreamWriter propertiesFile = File.CreateText(exportLuaPath);
                propertiesFile.Write(_heliosExportLUA);
                propertiesFile.Close();
               
            }
            catch (Exception e)
            {
                ConfigManager.LogManager.LogError("DCS Configuration - Error updating " + exportFile + " (Filename=\"" + exportLuaPath + "\")", e);
                return false;
            }

            IsUpToDate = CheckConfig();

            return true;
        }

        public string DCSInstallType
        {
            get
            {
                return _dcsInstallType;
             }
            set
            {
                if ((_dcsInstallType == null && value != null)
                    || (_dcsInstallType != null && !_dcsInstallType.Equals(value)))
                {
                    string oldValue = _dcsInstallType;
                    _dcsInstallType = value;
                    OnPropertyChanged("DCSInstallType", oldValue, value, false);
                    UpdateHeliosProperties();
                    ConfigManager.SettingsManager.SaveSetting(_prefPrefix, "DCSInstallType", _dcsInstallType);
                }
            }
        }

        public string SavedGamesFolder
        {
            get
            {
                if (_savedgamesPath == "")
                {
                    // We attempt to get the Saved Games known folder from the native method to cater for situations
                    // when the locale of the installation has the folder name in non-English.
                    IntPtr pathPtr;
                    int hr = NativeMethods.SHGetKnownFolderPath(ref FolderSavedGames, 0, IntPtr.Zero, out pathPtr);
                    if (hr == 0)
                    {
                        _savedgamesPath = Marshal.PtrToStringUni(pathPtr);
                        Marshal.FreeCoTaskMem(pathPtr);
                    }
                    else
                    {
                        _savedgamesPath = Environment.GetEnvironmentVariable("userprofile") + "Saved Games";
                    }
                }
                return _savedgamesPath;
            }
        }

        public bool RestoreConfig()
        {
            return RestoreConfig("Export.lua");
        }

        public bool RestoreConfig(String exportFile)
        {
            string exportLuaPath = System.IO.Path.Combine(ExportAppPath, ExportConfigPath, exportFile);
            string backupFile = exportLuaPath + ".back";

            if (File.Exists(exportLuaPath))
            {
                File.Delete(exportLuaPath);
            }

            if (File.Exists(backupFile))
            {
                File.Move(backupFile, exportLuaPath);
                File.Delete(backupFile);
            }

            IsUpToDate = CheckConfig();

            return true;
        }

        private bool CheckPath()
        {
            try
            {
                if (Directory.Exists(ExportAppPath))
                {
                    string exportLuaPath = System.IO.Path.Combine(ExportAppPath, ExportConfigPath);
                    if (!Directory.Exists(exportLuaPath))
                    {
                        Directory.CreateDirectory(exportLuaPath);
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                ConfigManager.LogManager.LogError("Error checking DCS Path", e);
            }

            return false;
        }

        private bool CheckConfig()
        {
            return CheckConfig("Export.lua");
        }
        private bool CheckConfig(String exportFile)
        {
            try
            {
                string exportLuaPath = System.IO.Path.Combine(ExportAppPath, ExportConfigPath, exportFile);
                return CheckFile(exportLuaPath, _heliosExportLUAMD5);
            }
            catch (Exception e)
            {
                ConfigManager.LogManager.LogError("Error checking status of DCS Export configuration", e);
            }

            return false;
        }

        private bool CheckFile(string path, string expectedHash)
        {
            if (File.Exists(path))
            {
                string currentExportHash = Hash.GetMD5HashFromFile(path);

                if (currentExportHash.Equals(expectedHash))
                {
                    return true;
                }
            }

            return false;
        }

        private string AircraftType
        {
            get
            {
                return UDPInterface.AlternateName;           
            }
        }


        private void UpdateHeliosProperties()
        {
            string _luaVarScope; 

            if (_dcsUseNewExport) _luaVarScope = "l"; else _luaVarScope = "g";   // the new structure for export.lua code attempts to avoid the use of globals.
            if (_udpInterface != null)
            {
                double exportInterval = Math.Round(1d / Math.Max(4, _exportFrequency), 3);
                int lowExportTickInterval = Math.Min(1, (int)Math.Floor(0.250d / exportInterval));

                StringWriter configFile = new StringWriter();
                configFile.WriteLine("local l" + "Aircraft = \"" + AircraftType + "\"");
                configFile.WriteLine(_luaVarScope + "Host = \"" + IPAddress + "\"");
                configFile.WriteLine(_luaVarScope + "Port = " + Port.ToString(CultureInfo.InvariantCulture));
                configFile.WriteLine(_luaVarScope + "ExportInterval = " + exportInterval.ToString(CultureInfo.InvariantCulture));
                configFile.WriteLine(_luaVarScope + "ExportLowTickInterval = " + lowExportTickInterval.ToString(CultureInfo.InvariantCulture));
                bool addEveryFrameComma = false;
                bool addComma = false;
                StringBuilder everyFrameArguments = new StringBuilder();
                StringBuilder arguments = new StringBuilder();

                foreach (NetworkFunction function in _udpInterface.Functions)
                {
                    foreach (ExportDataElement element in function.GetDataElements())
                    {
                        DCSDataElement dcsElement = element as DCSDataElement;
                        if (dcsElement != null && dcsElement.Format != null)
                        {
                            if (dcsElement.IsExportedEveryFrame)
                            {
                                if (addEveryFrameComma)
                                {
                                    everyFrameArguments.Append(", ");
                                }
                                everyFrameArguments.Append("[");
                                everyFrameArguments.Append(dcsElement.ID);
                                everyFrameArguments.Append("]=\"");
                                everyFrameArguments.Append(dcsElement.Format);
                                everyFrameArguments.Append("\"");
                                addEveryFrameComma = true;
                            }
                            else
                            {
                                if (addComma)
                                {
                                    arguments.Append(", ");
                                }
                                arguments.Append("[");
                                arguments.Append(dcsElement.ID);
                                arguments.Append("]=\"");
                                arguments.Append(dcsElement.Format);
                                arguments.Append("\"");
                                addComma = true;
                            }
                        }
                    }
                }

                configFile.Write(_luaVarScope + "EveryFrameArguments = {");
                configFile.Write(everyFrameArguments.ToString());
                configFile.WriteLine("}");

                configFile.Write(_luaVarScope + "Arguments = {");
                configFile.Write(arguments.ToString());
                configFile.WriteLine("}");

                configFile.WriteLine("");

                if (!string.IsNullOrWhiteSpace(_exportFunctionsPath))
                {
                    Resources.CopyResourceFile(_exportFunctionsPath, configFile);
                }

                configFile.WriteLine("");

                if (!string.IsNullOrWhiteSpace(_mainExportLUA))
                {
                    Resources.CopyResourceFile(_mainExportLUA, configFile);
                }

                configFile.WriteLine("");

                foreach (string file in DoFiles)
                {
                    configFile.Write("dofile(\"");
                    configFile.Write(file);
                    configFile.WriteLine("\")");
                }

                configFile.Close();

                _heliosExportLUA = configFile.ToString();
                _heliosExportLUAMD5 = Hash.GetMD5HashFromString(_heliosExportLUA);

                IsUpToDate = CheckConfig();
            }
        }

        void Functions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateHeliosProperties();
        }

        void Interface_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateHeliosProperties();
        }

        public bool InstallTypeGA
        {
            get
            {
                return _dcsInstallTypeGA;
            }
            set
            {
                if (!_dcsInstallTypeGA.Equals(value))
                {
                    bool oldValue = _dcsInstallTypeGA;
                    _dcsInstallTypeGA = value;
                    OnPropertyChanged("InstallTypeGA", oldValue, value, false);
                }
            }
        }
        public bool InstallTypeBeta
        {
            get
            {
                return _dcsInstallTypeBeta;
            }
            set
            {
                if (!_dcsInstallTypeBeta.Equals(value))
                {
                    bool oldValue = _dcsInstallTypeBeta;
                    _dcsInstallTypeBeta = value;
                    OnPropertyChanged("InstallTypeBeta", oldValue, value, false);
                }
            }
        }
        public bool InstallTypeAlpha
        {
            get
            {
                return _dcsInstallTypeAlpha;
            }
            set
            {
                if (!_dcsInstallTypeAlpha.Equals(value))
                {
                    bool oldValue = _dcsInstallTypeAlpha;
                    _dcsInstallTypeAlpha = value;
                    OnPropertyChanged("InstallTypeAlpha", oldValue, value, false);
                }
            }
        }
        public bool UseNewExport
        {
            get
            {
                return _dcsUseNewExport;
            }
            set
            {
                if (!_dcsUseNewExport.Equals(value))
                {
                    bool oldValue = _dcsUseNewExport;
                    _dcsUseNewExport = value;
                    OnPropertyChanged("UseNewExport", oldValue, value, false);
                }
            }
        }
        
    }
}

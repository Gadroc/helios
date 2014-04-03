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

namespace GadrocsWorkshop.Helios
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Xml;

    public static class VersionChecker
    {
        private static Version _runningVersion;
        private static string _currentVersion = "";
        private static string _downloadUrl = "";

        private const string VERSION_URL = "http://www.gadrocsworkshop.com/helios/version.xml";

        static VersionChecker()
        {
            GetRunningVersion();
            GetCurrentVersion();
        }

        public static void CheckVersion()
        {
            if (!string.IsNullOrWhiteSpace(_currentVersion) && _runningVersion != null)
            {
                try
                {
                    string[] parts = _currentVersion.Split('.');

                    if (parts.Length == 3)
                    {
                        int major = int.Parse(parts[0]);
                        int minor = int.Parse(parts[1]);
                        int build = int.Parse(parts[2]);

                        if (major > _runningVersion.Major || 
                            (major == _runningVersion.Major && minor > _runningVersion.Minor) ||
                            (major == _runningVersion.Major && minor == _runningVersion.Minor && build > _runningVersion.Build))
                        {
                            MessageBoxResult result = MessageBox.Show("A newer version of Helios is available. Would you like to download it now?", "Version Check", MessageBoxButton.YesNo, MessageBoxImage.Information);
                            if (result == MessageBoxResult.Yes)
                            {
                                System.Diagnostics.Process.Start(_downloadUrl);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ConfigManager.LogManager.LogError("Error comapring versions", e);
                }
            }
        }

        private static void GetRunningVersion()
        {
            try
            {
                _runningVersion = Assembly.GetEntryAssembly().GetName().Version;
            }
            catch (Exception e)
            {
                ConfigManager.LogManager.LogError("Error reading running version.", e);
            }
        }

        private static void GetCurrentVersion()
        {
            try
            {
                using (XmlTextReader xmlReader = new XmlTextReader(VERSION_URL))
                {
                    xmlReader.ReadStartElement("HeliosVersion");
                    _currentVersion = xmlReader.ReadElementString("CurrentVersion");
                    _downloadUrl = xmlReader.ReadElementString("DownloadUrl");
                    while (!xmlReader.Name.Equals("HeliosVersion"))
                    {
                        xmlReader.Read();
                    }
                    xmlReader.ReadEndElement();
                }
            }
            catch (Exception e)
            {
                ConfigManager.LogManager.LogError("Error retrieving current version.", e);
            }
        }
    }
}

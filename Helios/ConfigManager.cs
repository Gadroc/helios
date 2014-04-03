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
    using System.Collections.Generic;
    using System.IO;


    public class ConfigManager
    {
        private static readonly string _applicationPath;
        private static string _documentPath;
        private static string _documentImagePath;
        private static string _documentProfilePath;
        private static string _documentTemplatesPath;
        private static string _documentPanelTemplatesPath;

        private static ITemplateManager _templateManager;
        private static IImageManager _imageManager;
        private static IModuleManager _moduleManager;
        private static IProfileManager _profileManager;
        private static DisplayManager _displayManager;
        private static UndoManager _undoManager;
        private static LogManager _logManager;
        private static ISettingsManager _settingsManager;

        /// <summary>
        /// Private constructor to prevent instances.  This class is a Singleton which should be accessed
        /// </summary>
        static ConfigManager()
        {
            string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            _applicationPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        #region Properties

        public static string DocumentPath
        {
            get
            {
                return _documentPath;
            }
            set
            {
                _documentPath = value;
                _documentImagePath = Path.Combine(_documentPath, "Images");
                _documentProfilePath = Path.Combine(_documentPath, "Profiles");
                _documentTemplatesPath = Path.Combine(_documentPath, "Templates");
                _documentPanelTemplatesPath = Path.Combine(_documentPath, "Panel Templates");

                List<string> paths = new List<string> { _documentPath, _documentImagePath, _documentProfilePath, _documentTemplatesPath, _documentPanelTemplatesPath };
                foreach (string path in paths)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
            }
        }

        public static string ApplicationPath { get { return _applicationPath; } }
        public static string ProfilePath { get { return _documentProfilePath; } }
        public static string ImagePath { get { return _documentImagePath; } }
        public static string TemplatePath { get { return _documentTemplatesPath; } }
        public static string PanelTemplatePath { get { return _documentPanelTemplatesPath; } }

        public static IImageManager ImageManager
        {
            get
            {
                return _imageManager;
            }
            set
            {
                _imageManager = value;
            }
        }
        public static ITemplateManager TemplateManager
        {
            get
            {
                return _templateManager;
            }
            set
            {
                _templateManager = value;
            }
        }
        public static IModuleManager ModuleManager
        {
            get
            {
                return _moduleManager;
            }
            set
            {
                _moduleManager = value;
            }
        }
        public static IProfileManager ProfileManager
        {
            get
            {
                return _profileManager;
            }
            set
            {
                _profileManager = value;
            }
        }
        public static DisplayManager DisplayManager
        {
            get
            {
                return _displayManager;
            }
            set
            {
                _displayManager = value;
            }
        }
        public static UndoManager UndoManager
        {
            get
            {
                return _undoManager;
            }
            set
            {
                _undoManager = value;
            }
        }
        public static LogManager LogManager
        {
            get
            {
                return _logManager;
            }
            set
            {
                _logManager = value;
            }
        }
        public static ISettingsManager SettingsManager
        {
            get
            {
                return _settingsManager;
            }
            set
            {
                _settingsManager = value;
            }
        }

        #endregion
    }
}

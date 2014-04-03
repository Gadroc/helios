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
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Threading;
    using System.Xml;

    internal class ProfileManager : IProfileManager
    {
        internal ProfileManager()
        {
        }

        public bool SaveProfile(HeliosProfile profile)
        {            
            try
            {                
                string tempPath = Path.ChangeExtension(profile.Path, "tmp");
                string backupPath = Path.ChangeExtension(profile.Path, "bak");

                // Delete tmp file if exists
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }

                TextWriter writer = new StreamWriter(tempPath, false);
                TypeConverter boolConverter = TypeDescriptor.GetConverter(typeof(bool));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;

                XmlWriter xmlWriter = XmlWriter.Create(writer, settings);
                HeliosBindingCollection bindings = new HeliosBindingCollection();

                HeliosSerializer serializer = new HeliosSerializer(null);
                serializer.SerializeProfile(profile, xmlWriter);

                profile.IsDirty = false;
                xmlWriter.Close();
                writer.Close();

                // Delete existing backup
                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }

                // backup existing file
                if (File.Exists(profile.Path))
                {
                    File.Move(profile.Path, backupPath);
                }

                // Rename .tmp to actual
                File.Move(tempPath, profile.Path);

                profile.LoadTime = Directory.GetLastWriteTime(profile.Path);

                return true;
            }
            catch (Exception e)
            {
                ConfigManager.LogManager.LogError("Error saving profile", e);
                return false;
            }
        }

        public HeliosProfile LoadProfile(string path)
        {
            return LoadProfile(path, null);
        }

        public HeliosProfile LoadProfile(string path, Dispatcher dispatcher)
        {
            HeliosProfile profile = null;

            try
            {

                if (File.Exists(path))
                {
                    bool rewrite = false;

                    profile = new HeliosProfile(false);
                    profile.Path = path;
                    profile.Name = Path.GetFileNameWithoutExtension(path);

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreComments = true;
                    settings.IgnoreWhitespace = true;

                    TextReader reader = new StreamReader(path);
                    XmlReader xmlReader = XmlReader.Create(reader, settings);

                    HeliosSerializer deserializer = new HeliosSerializer(dispatcher);
                    int profileVersion = deserializer.GetProfileVersion(xmlReader);

                    if (profileVersion != 3)
                    {
                        profile.IsInvalidVersion = true;
                    }
                    else
                    {
                        deserializer.DeserializeProfile(profile, xmlReader);
                    }

                    xmlReader.Close();
                    reader.Close();

                    profile.IsDirty = false;
                    profile.LoadTime = Directory.GetLastWriteTime(path);

                    if (rewrite)
                    {
                        SaveProfile(profile);
                    }
                }
            }
            catch (Exception e)
            {
                ConfigManager.LogManager.LogError("Error loading profile " + path, e);
                profile = null;
            }

            return profile;
        }
    }
}

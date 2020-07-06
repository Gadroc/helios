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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// TemplateManager gives access to saving and loading control templates.
    /// </summary>
    public class TemplateManager : ITemplateManager
    {
        private HeliosTemplateCollection _userTemplates = new HeliosTemplateCollection();
        private List<HeliosTemplate> _moduleTemplates = new List<HeliosTemplate>();

        private string _userTemplateDirectory = "";

        internal TemplateManager(string userTemplateDirectory, string userPanelTemplateDirectory)
        {
            ConfigManager.LogManager.Log("TemplateManager Intialisation: Templates = " + userTemplateDirectory + ". Panel Templates = " + userPanelTemplateDirectory);

            _userTemplateDirectory = userTemplateDirectory;

            PopulateUserTemplatesCollection();

            _userTemplates.CollectionChanged += new NotifyCollectionChangedEventHandler(UserTemplates_CollectionChanged);
        }

        #region Properties

        public HeliosTemplateCollection UserTemplates
        {
            get
            {
                return _userTemplates;
            }
        }

        public IList<HeliosTemplate> ModuleTemplates
        {
            get
            {
                return _moduleTemplates.AsReadOnly();
            }
        }

        #endregion

        internal void LoadModuleTemplates(string moduleName)
        {
            string templateDirectory = Path.Combine(ConfigManager.ApplicationPath, "Templates", moduleName);
            ConfigManager.LogManager.LogDebug("TemplateManager Loading Module Templates: " + templateDirectory);
            if (Directory.Exists(templateDirectory))
            {
                LoadTemplateDirectory(_moduleTemplates, templateDirectory, false);
            }
        }

        private void PopulateUserTemplatesCollection()
        {
            ConfigManager.LogManager.Log("TemplateManager Loading User Templates.");
            LoadTemplateDirectory(_userTemplates, _userTemplateDirectory, true);
        }

        private void LoadTemplateDirectory(IList<HeliosTemplate> templates, string directory, bool userTemplates)
        {
            ConfigManager.LogManager.LogDebug("TemplateManager Loading Template Directory: " + directory);
            foreach (string templateFile in Directory.GetFiles(directory, "*.htpl"))
            {
                ConfigManager.LogManager.LogDebug("TemplateManager Loading Template: " + templateFile);
                try
                {
                    templates.Add(LoadTemplate(templateFile, userTemplates));
                }
                catch (Exception e)
                {
                    ConfigManager.LogManager.LogError("TemplateManager Template Not Added due to Failure - (probable duplicate name): " + templateFile, e);
                    continue;
                }
            }

            foreach (string subDirectory in Directory.GetDirectories(directory))
            {
                ConfigManager.LogManager.LogDebug("TemplateManager Loading Template Subdirectory: " + subDirectory);
                LoadTemplateDirectory(templates, subDirectory, userTemplates);
            }
        }

        private HeliosTemplate LoadTemplate(string path, bool isUserTemplate)
        {
            HeliosTemplate template = new HeliosTemplate(isUserTemplate);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            settings.CloseInput = true;
            ConfigManager.LogManager.LogDebug("TemplateManager Reading Template XML: " + path);
            XmlReader reader = XmlReader.Create(path, settings);
            template.ReadXml(reader);
            reader.Close();
            ConfigManager.LogManager.LogDebug("TemplateManager XML Reader Closed: " + path);

            return template;
        }


        private string GetTemplatePath(HeliosTemplate template)
        {
            return Path.Combine(_userTemplateDirectory, template.Category, template.Name + ".htpl");
        }

        void UserTemplates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (HeliosTemplate template in e.OldItems)
                {
                    if (template != null)
                    {
                        string path = GetTemplatePath(template);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (HeliosTemplate template in e.NewItems)
                {
                    if (template != null)
                    {
                        string path = GetTemplatePath(template);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }

                        string directory = Path.GetDirectoryName(path);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        XmlWriter writer = new XmlTextWriter(path, Encoding.UTF8);
                        template.WriteXml(writer);
                        writer.Close();
                    }
                }
            }
        }
    }
}

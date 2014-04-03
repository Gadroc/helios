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

namespace GadrocsWorkshop.Helios.ProfileEditor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string _startupProfile = null;

        #region Properties

        public string StartupProfile
        {
            get { return _startupProfile; }
            private set { _startupProfile = value; }
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CommandLineOptions options = new CommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(e.Args, options))
            {
                if (options.Profiles != null && options.Profiles.Count > 0)
                {
                    StartupProfile = options.Profiles.Last();
                }
            }

            string documentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), options.DocumentPath);
            HeliosInit.Initialize(documentPath, "ProfileEditor.log", options.LogLevel);

            ConfigManager.LogManager.LogInfo("Starting Editor");
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(string.Format("An error occured: {0}", e.Exception.Message), "Error");
            e.Handled = true;
        }
    }
}

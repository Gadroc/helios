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

namespace GadrocsWorkshop.Helios.ControlCenter
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Microsoft.Shell;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string InstanceUniqueName = "HeliosApplicationInstanceMutex";
        private string _startupProfile = null;
        private bool _disableTouchKit;

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(InstanceUniqueName))
            {
                //SplashScreen splashScreen = null;

                //splashScreen = new SplashScreen("splash_logo.png");
                //splashScreen.Show(false);
                GadrocsWorkshop.Helios.ControlCenter.App app = new GadrocsWorkshop.Helios.ControlCenter.App();
                app.InitializeComponent();
                //Thread.Sleep(1000);
                //splashScreen.Close(TimeSpan.FromMilliseconds(500));
                app.Run();
                SingleInstance<App>.Cleanup();
            }
        }

        #region Properties

        public string StartupProfile
        {
            get { return _startupProfile; }
            private set { _startupProfile = value; }
        }

        public bool DisableTouchKit
        {
            get { return _disableTouchKit; }
            private set { _disableTouchKit = value; }
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CommandLineOptions options = new CommandLineOptions();

            if (CommandLine.Parser.Default.ParseArguments(e.Args, options))
            {
                DisableTouchKit = options.DisableTouchKit;

                if (options.Profiles != null && options.Profiles.Count > 0)
                {
                    StartupProfile = options.Profiles.Last();
                }
            }

            string documentPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), options.DocumentPath);
            HeliosInit.Initialize(documentPath, "ControlCenter.log", options.LogLevel);
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {

            CommandLineOptions options = new CommandLineOptions();

            CommandLine.Parser.Default.ParseArguments(args.ToArray(), options);

            if (options.Exit)
            {
                ApplicationCommands.Close.Execute(null, Application.Current.MainWindow);
            }
            else if (options.Profiles != null && options.Profiles.Count > 0 && File.Exists(options.Profiles.Last()))
            {
                ControlCenterCommands.RunProfile.Execute(options.Profiles.Last(), Application.Current.MainWindow);
            }

            return true;
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            System.Windows.MessageBox.Show(string.Format("An error occured: {0}", e.Exception.Message), "Error");
            e.Handled = true;
            //ConfigManager.LogManager.LogError("Unhandled Exception", e.Exception);
            //e.Handled = false;
        }
    }
}

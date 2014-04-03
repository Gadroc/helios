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
    using GadrocsWorkshop.Helios.ProfileEditor.ViewModel;
    using System;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ResetMonitors.xaml
    /// </summary>
    public partial class ResetMonitors : Window
    {
        private HeliosProfile _profile;
        private List<MonitorResetItem> _monitors;
        private List<String> _newMonitors;

        public ResetMonitors(HeliosProfile profile)
        {
            _profile = profile;
            HeliosProfile newProfile = new HeliosProfile();

            _monitors = new List<MonitorResetItem>(profile.Monitors.Count);
            for (int i = 0; i < profile.Monitors.Count; i++)
            {
                _monitors.Add(new MonitorResetItem(profile.Monitors[i], i, i < newProfile.Monitors.Count ? i : 0));
            }

            _newMonitors = new List<String>(newProfile.Monitors.Count);
            foreach (Monitor monitor in newProfile.Monitors)
            {
                _newMonitors.Add(monitor.Name);
            }

            InitializeComponent();

            OldLayout.Profile = profile;
            NewLayout.Profile = newProfile;
        }

        public List<MonitorResetItem> MonitorResets
        {
            get { return _monitors; }
        }

        public List<String> NewMonitors
        {
            get { return _newMonitors; }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Ok(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

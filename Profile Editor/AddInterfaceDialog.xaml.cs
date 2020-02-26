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
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Interaction logic for AddInterfaceDialog.xaml
    /// </summary>
    public partial class AddInterfaceDialog : Window
    {
        private HeliosProfile _profile = null;
        private List<HeliosInterface> _availableInterfaces = new List<HeliosInterface>();

        public AddInterfaceDialog(HeliosProfile profile)
        {
            InitializeComponent();
            _profile = profile;
            UpdateAvailableInterfaces();
        }

        #region Properties

        public HeliosInterface SelectedInterface
        {
            get { return (HeliosInterface)GetValue(SelectedInterfaceProperty); }
            set { SetValue(SelectedInterfaceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedInterface.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedInterfaceProperty =
            DependencyProperty.Register("SelectedInterface", typeof(HeliosInterface), typeof(AddInterfaceDialog), new PropertyMetadata(null, SelectedInterfaceChanged));

        public List<HeliosInterface> AvailableInterfaces
        {
            get
            {
                return _availableInterfaces;
            }
        }

        #endregion

        private static void SelectedInterfaceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            AddInterfaceDialog dialog = obj as AddInterfaceDialog;
            if (dialog != null)
            {
                dialog.AddButton.IsEnabled = (args.NewValue != null);
            }
        }

        private void AddInterface(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void UpdateAvailableInterfaces()
        {
            AvailableInterfaces.Clear();

            if (_profile != null)
            {
                foreach (HeliosInterfaceDescriptor descriptor in ConfigManager.ModuleManager.InterfaceDescriptors)
                {
                    ConfigManager.LogManager.LogInfo("Checking for available instances of " + descriptor.Name + " interface.");
                    try
                    {
                        foreach (HeliosInterface newInterface in descriptor.GetNewInstances(_profile))
                        {
                            ConfigManager.LogManager.LogInfo("Adding " + newInterface.Name + " Type: " + descriptor.InterfaceType.BaseType.Name + " to add interface list.");
                            AvailableInterfaces.Add(newInterface);
                        }
                    }
                    catch (Exception e)
                    {
                        ConfigManager.LogManager.LogError("Error trying to get available instances for " + descriptor.Name + " interface.", e);
                    }
                }
            }
        }
    }
}

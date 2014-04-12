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

namespace GadrocsWorkshop.Helios.Interfaces.Falcon
{
    using GadrocsWorkshop.Helios.Windows.Controls;
    using Microsoft.Win32;
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for FalconIntefaceEditor.xaml
    /// </summary>
    public partial class FalconIntefaceEditor : HeliosInterfaceEditor
    {
        public FalconIntefaceEditor()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.DereferenceLinks = true;
            ofd.Multiselect = false;
            ofd.ValidateNames = true;
            ofd.Filter = "Key Files (*.key)|*.key";
            ofd.Title = "Select Key File";

            ofd.FileName = ((FalconInterface)Interface).KeyFileName;
            Nullable<bool> result = ofd.ShowDialog(Window.GetWindow(this));

            if (result == true)
            {
                ((FalconInterface)Interface).KeyFileName = ConfigManager.ImageManager.MakeImagePathRelative(ofd.FileName);
            }
        }
    }
}

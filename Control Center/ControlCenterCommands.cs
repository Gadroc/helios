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
    using System.Windows.Input;
    
    static class ControlCenterCommands
    {
        private static RoutedUICommand NextProfileCommand = new RoutedUICommand("Next Profile", "NextProfile", typeof(ControlCenterCommands));
        private static RoutedUICommand PreviousProfileCommand = new RoutedUICommand("Previous Profile", "PreviousProfile", typeof(ControlCenterCommands));
        private static RoutedUICommand StartProfileCommand = new RoutedUICommand("Start Profile", "StartProfile", typeof(ControlCenterCommands));
        private static RoutedUICommand StopProfileCommand = new RoutedUICommand("Stop Profile", "StopProfile", typeof(ControlCenterCommands));
        private static RoutedUICommand ResetProfileCommand = new RoutedUICommand("Reset Profile", "ResetProfile", typeof(ControlCenterCommands));
        private static RoutedUICommand DeleteProfileCommand = new RoutedUICommand("Delete Profile", "DeleteProfile", typeof(ControlCenterCommands));
        private static RoutedUICommand OpenControlCenterCommand = new RoutedUICommand("Open Control Center", "OpenControlCenter", typeof(ControlCenterCommands));
        private static RoutedUICommand TogglePreferencesCommand = new RoutedUICommand("Toggle Display of Control Center Preferences", "TogglePreferences", typeof(ControlCenterCommands));
        private static RoutedUICommand RunProfileCommand = new RoutedUICommand("Opens and Runs a Profile", "RunProfile", typeof(ControlCenterCommands));

        public static RoutedUICommand NextProfile { get { return NextProfileCommand; } }
        public static RoutedUICommand PreviousProfile { get { return PreviousProfileCommand; } }
        public static RoutedUICommand StartProfile { get { return StartProfileCommand; } }
        public static RoutedUICommand StopProfile { get { return StopProfileCommand; } }
        public static RoutedUICommand ResetProfile { get { return ResetProfileCommand; } }
        public static RoutedUICommand DeleteProfile { get { return DeleteProfileCommand; } }
        public static RoutedUICommand OpenControlCenter { get { return OpenControlCenterCommand; } }
        public static RoutedUICommand TogglePreferences { get { return TogglePreferencesCommand; } }
        public static RoutedUICommand RunProfile { get { return RunProfileCommand; } }
    }
}

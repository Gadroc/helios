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

namespace GadrocsWorkshop.Helios.Windows.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public class StaticValueEditor : UserControl
    {
        public string StaticValue
        {
            get { return (string)GetValue(StaticValueProperty); }
            set { SetValue(StaticValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StaticValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StaticValueProperty =
            DependencyProperty.Register("StaticValue", typeof(string), typeof(StaticValueEditor), new PropertyMetadata(""));

        public HeliosProfile Profile
        {
            get { return (HeliosProfile)GetValue(ProfileProperty); }
            set { SetValue(ProfileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Profile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfileProperty =
            DependencyProperty.Register("Profile", typeof(HeliosProfile), typeof(StaticValueEditor), new PropertyMetadata(null));
    }
}

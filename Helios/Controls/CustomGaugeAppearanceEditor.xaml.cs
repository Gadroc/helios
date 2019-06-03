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

namespace GadrocsWorkshop.Helios.Controls
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System.Windows;


    /// <summary>
    /// Interaction logic for CustomGaugeAppearanceEditor.xaml
    /// </summary>
    [HeliosPropertyEditor("Helios.Base.CustomGauge", "Appearance")]
    public partial class CustomGaugeAppearanceEditor : HeliosPropertyEditor
    {
        public CustomGaugeAppearanceEditor()
        {
            InitializeComponent();
        }

        private void MinPosition_GotFocus(object sender, RoutedEventArgs e)
        {
            CustomGauge pot = Control as CustomGauge;
            if (pot != null)
            {
                pot.Value = pot.MinValue;
            }
        }

        private void MaxPosition_GotFocus(object sender, RoutedEventArgs e)
        {
            CustomGauge pot = Control as CustomGauge;
            if (pot != null)
            {
                pot.Value = pot.MaxValue;
            }
        }
        private void Scale_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
       
        private void PivotX_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void PivotY_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }
        private void Needle_posX_GotFocus(object sender, RoutedEventArgs e)
        {
          
        }
        private void Needle_posY_GotFocus(object sender, RoutedEventArgs e)
        {
          
        }
    }
}

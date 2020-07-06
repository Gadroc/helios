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
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for TextDisplayAppearancePropertyEditor.xaml
    /// </summary>
    [HeliosPropertyEditor("Helios.Base.TextDisplay", "Appearance")]
    public partial class TextDisplayAppearancePropertyEditor : HeliosPropertyEditor
    {
        public TextDisplayAppearancePropertyEditor()
        {
            InitializeComponent();
        }

        private void TurnTextDisplayOn(object sender, RoutedEventArgs e)
        {
            //TextDisplay textDisplay = Control as TextDisplay;
            //if (textDisplay != null)
            //{
            //    textDisplay.TextValue = "NA";
            //}
        }

        private void TurnTextDisplayOff(object sender, RoutedEventArgs e)
        {
            TextDisplay textDisplay = Control as TextDisplay;
            if (textDisplay != null)
            {
                textDisplay.TextValue = "0";
            }
        }

        private void LeftPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextDisplay textDisplay = Control as TextDisplay;
                if (textDisplay != null)
                {
                    textDisplay.TextFormat.PaddingRight = textDisplay.TextFormat.PaddingLeft;
                }
            }
        }

        private void RightPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextDisplay textDisplay = Control as TextDisplay;
                if (textDisplay != null)
                {
                    textDisplay.TextFormat.PaddingLeft = textDisplay.TextFormat.PaddingRight;
                }
            }
        }

        private void TopPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextDisplay textDisplay = Control as TextDisplay;
                if (textDisplay != null)
                {
                    textDisplay.TextFormat.PaddingBottom = textDisplay.TextFormat.PaddingTop;
                }
            }
        }

        private void BottomPaddingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            if (!System.Windows.Input.Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) && slider != null && slider.IsFocused)
            {
                TextDisplay textDisplay = Control as TextDisplay;
                if (textDisplay != null)
                {
                    textDisplay.TextFormat.PaddingTop = textDisplay.TextFormat.PaddingBottom;
                }
            }
        }

        private void HeliosTestTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            HeliosTextBox textBox = sender as HeliosTextBox;
            TextDisplay textDisplay = Control as TextDisplay;
            if (textDisplay != null)
            {
                textDisplay.TextValue = textBox.Text;
            }
        }

        private void HeliosTextParserBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}

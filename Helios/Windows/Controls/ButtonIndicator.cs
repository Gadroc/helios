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
    using System.Windows.Media;

    public class ButtonIndicator : Control
    {
        static ButtonIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonIndicator), new FrameworkPropertyMetadata(typeof(ButtonIndicator)));
        }

        #region Properties

        public bool IsPushed
        {
            get { return (bool)GetValue(IsPushedProperty); }
            set { SetValue(IsPushedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPushed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPushedProperty =
            DependencyProperty.Register("IsPushed", typeof(bool), typeof(ButtonIndicator), new UIPropertyMetadata(false));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(ButtonIndicator), new UIPropertyMetadata(""));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Fill.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(ButtonIndicator), new UIPropertyMetadata(Brushes.DarkRed));

        #endregion
    }
}

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
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

    public class HeliosInterfaceEditor : HeliosEditorDocument
    {
        public HeliosInterfaceEditor()
        {
        }

        #region Properties

        public HeliosInterface Interface
        {
            get { return (HeliosInterface)GetValue(InterfaceProperty); }
            set { SetValue(InterfaceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Interface.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InterfaceProperty =
            DependencyProperty.Register("Interface", typeof(HeliosInterface), typeof(HeliosInterfaceEditor), new PropertyMetadata(null, new PropertyChangedCallback(InterfaceChanged)));

        public override bool HandlesScroll
        {
            get { return false; }
        }

        public override string Title
        {
            get { return Interface.Name; }
        }

        #endregion

        private static void InterfaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeliosInterfaceEditor p = d as HeliosInterfaceEditor;
            p.BindingFocus = p.Interface;
            p.OnInterfaceChanged(e.OldValue as HeliosInterface, e.NewValue as HeliosInterface);
        }

        protected virtual void OnInterfaceChanged(HeliosInterface oldInterface, HeliosInterface newInterface)
        {
        }
    }
}

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
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Interaction logic for FalconCallbackValueEditor.xaml
    /// </summary>
    public partial class FalconCallbackValueEditor : StaticValueEditor
    {
        private FalconInterface _falcon;

        public FalconCallbackValueEditor()
        {
            InitializeComponent();
        }

        #region Properties

        public List<FalconKeyCallback> Callbacks
        {
            get { return (List<FalconKeyCallback>)GetValue(CallbacksProperty); }
            set { SetValue(CallbacksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Callbacks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CallbacksProperty =
            DependencyProperty.Register("Callbacks", typeof(List<FalconKeyCallback>), typeof(FalconCallbackValueEditor), new PropertyMetadata(null));

        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ProfileProperty)
            {
                if (_falcon != null)
                {
                    _falcon.PropertyChanged -= Falcon_PropertyChanged;
                }

                if (Profile != null && Profile.Interfaces.ContainsKey("Falcon"))
                {
                    _falcon = Profile.Interfaces["Falcon"] as FalconInterface;
                    PopulateCallbacks();
                    _falcon.PropertyChanged += Falcon_PropertyChanged;
                }
            }
            base.OnPropertyChanged(e);
        }

        void Falcon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PopulateCallbacks();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CallbackList.ScrollIntoView(CallbackList.SelectedItem);
        }

        private void PopulateCallbacks()
        {
            if (_falcon != null && _falcon.KeyFile != null && _falcon.KeyFile.Callbacks != null)
            {
                Callbacks = new List<FalconKeyCallback>(_falcon.KeyFile.Callbacks);
            }
            else
            {
                Callbacks = new List<FalconKeyCallback>();
            }
            ICollectionView view = CollectionViewSource.GetDefaultView(Callbacks);
            new FalconKeyCallbackFilter(view, TextFilter);
        }
    }
}

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

namespace GadrocsWorkshop.Helios.Interfaces.DirectX
{
    using GadrocsWorkshop.Helios.Windows.Controls;
    using SharpDX.DirectInput;
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for DirectXControllerInterfaceEditor.xaml
    /// </summary>
    public partial class DirectXControllerInterfaceEditor : HeliosInterfaceEditor
    {
        private Dictionary<DirectXControllerAxis, AxisBar> _axis = new Dictionary<DirectXControllerAxis, AxisBar>();
        private Dictionary<DirectXControllerButton, ButtonIndicator> _buttons = new Dictionary<DirectXControllerButton, ButtonIndicator>();
        private Dictionary<DirectXControllerPOVHat, PointOfViewIndicator> _povs = new Dictionary<DirectXControllerPOVHat, PointOfViewIndicator>();

        private DispatcherTimer _timer = new DispatcherTimer();

        public DirectXControllerInterfaceEditor()
        {
            InitializeComponent();

            _timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
            _timer.Tick += new EventHandler(Timer_Tick);
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(DirectXControllerInterfaceEditor_IsVisibleChanged);
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            DirectXControllerInterface dxInterface = Interface as DirectXControllerInterface;

            if (dxInterface != null && dxInterface.IsValid)
            {
                JoystickState state = dxInterface.GetState();
                foreach (KeyValuePair<DirectXControllerButton, ButtonIndicator> pair in _buttons)
                {
                    pair.Value.IsPushed = pair.Key.GetValue(state);
                }

                foreach (KeyValuePair<DirectXControllerAxis, AxisBar> pair in _axis)
                {
                    pair.Value.Value = pair.Key.GetValue(state);
                }

                foreach (KeyValuePair<DirectXControllerPOVHat, PointOfViewIndicator> pair in _povs)
                {
                    pair.Value.Direction = pair.Key.GetValue(state);
                }
            }
        }

        void DirectXControllerInterfaceEditor_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == InterfaceProperty)
            {
                DirectXControllerInterface oldInterface = e.OldValue as DirectXControllerInterface;
                if (oldInterface != null)
                {
                    oldInterface.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Interface_PropertyChanged);
                }

                DirectXControllerInterface newInterface = e.NewValue as DirectXControllerInterface;
                if (newInterface != null)
                {
                    newInterface.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Interface_PropertyChanged);
                    newInterface.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Interface_PropertyChanged);
                }

                UpdateControllerDisplay();
            }
            base.OnPropertyChanged(e);
        }

        void Interface_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ControllerId"))
            {
                UpdateControllerDisplay();
            }
        }

        private void UpdateControllerDisplay()
        {
            AxisPanel.Children.Clear();
            ButtonPanel.Children.Clear();
            _buttons.Clear();

            DirectXControllerInterface controller = Interface as DirectXControllerInterface;
            if (controller != null)
            {
                foreach (DirectXControllerFunction function in controller.Functions)
                {
                    DirectXControllerButton button = function as DirectXControllerButton;
                    if (button != null)
                    {
                        ButtonIndicator indicator = new ButtonIndicator();
                        indicator.Width = 30;
                        indicator.Height = 30;
                        indicator.Label = (button.ObjectNumber + 1).ToString();
                        indicator.Margin = new Thickness(2);
                        _buttons.Add(button, indicator);
                        ButtonPanel.Children.Add(indicator);
                    }

                    DirectXControllerAxis axis = function as DirectXControllerAxis;
                    if (axis != null)
                    {
                        AxisBar bar = new AxisBar();
                        bar.BarWidth = 30;
                        bar.Height = 110;
                        bar.Label = axis.Name;
                        bar.BorderThickness = new Thickness(1d);
                        bar.BorderBrush = Brushes.DarkGray;
                        bar.Margin = new Thickness(10, 2, 0, 2);
                        _axis.Add(axis, bar);
                        AxisPanel.Children.Add(bar);
                    }

                    DirectXControllerPOVHat hat = function as DirectXControllerPOVHat;
                    if (hat != null)
                    {
                        PointOfViewIndicator pov = new PointOfViewIndicator();
                        pov.Width = 60;
                        pov.Height = 60;
                        pov.Direction = POVDirection.Center;

                        TextBlock label = new TextBlock();
                        label.Width = 60;
                        label.Text = hat.Name;

                        StackPanel panel = new StackPanel();
                        panel.Margin = new Thickness(10, 2, 0, 2);
                        panel.Children.Add(pov);
                        panel.Children.Add(label);

                        _povs.Add(hat, pov);
                        POVPanel.Children.Add(panel);
                    }
                }

                if (_axis.Count > 0)
                {
                    AxisGroup.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    AxisGroup.Visibility = System.Windows.Visibility.Collapsed;
                }

                if (_buttons.Count > 0)
                {
                    ButtonGroup.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    ButtonGroup.Visibility = System.Windows.Visibility.Collapsed;
                }

                if (_povs.Count > 0)
                {
                    POVGroup.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    POVGroup.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
    }
}

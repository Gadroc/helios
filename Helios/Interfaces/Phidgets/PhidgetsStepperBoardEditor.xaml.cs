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

namespace GadrocsWorkshop.Helios.Interfaces.Phidgets
{
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for PhidgetsStepperBoardEditor.xaml
    /// </summary>
    public partial class PhidgetsStepperBoardEditor : HeliosInterfaceEditor
    {
        private ObservableCollection<String> _motorList = new ObservableCollection<String>();

        public PhidgetsStepperBoardEditor()
        {
            InitializeComponent();
        }

        #region Properites

        public ObservableCollection<String> MotorList
        {
            get { return _motorList; }
        }

        public PhidgetsStepper SelectedStepper
        {
            get { return (PhidgetsStepper)GetValue(SelectedStepperProperty); }
            set { SetValue(SelectedStepperProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedStepper.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedStepperProperty =
            DependencyProperty.Register("SelectedStepper", typeof(PhidgetsStepper), typeof(PhidgetsStepperBoardEditor), new UIPropertyMetadata(null));

        #endregion

        protected override void OnInterfaceChanged(HeliosInterface oldInterface, HeliosInterface newInterface)
        {
            PhidgetStepperBoard oldBoard = oldInterface as PhidgetStepperBoard;
            if (oldBoard != null)
            {
                oldBoard.Detach();
            }

            PhidgetStepperBoard newBoard = newInterface as PhidgetStepperBoard;
            if (newBoard != null)
            {
                newBoard.Attach();
            }

            _motorList.Clear();
            for (int i = 0; i < newBoard.MotorCount; i++)
            {
                _motorList.Add("Motor " + i);
            }
        }

        public override void Closed()
        {
            base.Closed();
            PhidgetStepperBoard board = Interface as PhidgetStepperBoard;
            if (board != null)
            {
                board.Detach();
            }
        }

        private void SelectedGroupChanged(object sender, SelectionChangedEventArgs e)
        {
            PhidgetStepperBoard board = Interface as PhidgetStepperBoard;
            PhidgetsStepper stepper = board.Steppers[MotorListBox.SelectedIndex];
            CalibrationEditor.MaxOutput = long.MaxValue;
            CalibrationEditor.MinOutput = long.MinValue;
            CalibrationEditor.Calibration = stepper.Calibration;

            SelectedStepper = stepper;
        }

        private void TextValueChanged(object sender, TextChangedEventArgs e)
        {
            SelectedStepper.StepperValue.ExecuteAction(new BindingValue(TextValueTextBox.Text), true);
        }

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedStepper != null)
            {
                if (TabControl.GetIsSelected(CalibrationTab))
                {
                    SelectedStepper.StepperValue.ExecuteAction(new BindingValue(TextValueTextBox.Text), true);
                }
                else
                {
                    SelectedStepper.TargetPosition.ExecuteAction(new BindingValue(0), true);
                }
            }
        }

        private void Increment_Click(object sender, RoutedEventArgs e)
        {
            SelectedStepper.Increment.ExecuteAction(BindingValue.Empty, true);
            SelectedStepper.Zero.ExecuteAction(BindingValue.Empty, true);
        }

        private void Decrement_Click(object sender, RoutedEventArgs e)
        {
            SelectedStepper.Decrement.ExecuteAction(BindingValue.Empty, true);
            SelectedStepper.Zero.ExecuteAction(BindingValue.Empty, true);
        }

    }
}

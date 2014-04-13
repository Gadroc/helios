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
    /// Interaction logic for PhidgetsServoBoardEditor.xaml
    /// </summary>
    public partial class PhidgetsServoBoardEditor : HeliosInterfaceEditor
    {
        private ObservableCollection<String> _motorList = new ObservableCollection<String>();

        public PhidgetsServoBoardEditor()
        {
            InitializeComponent();
        }

        #region Properties

        public ObservableCollection<String> MotorList
        {
            get { return _motorList; }
        }

        public PhidgetsServo SelectedServo
        {
            get { return (PhidgetsServo)GetValue(SelectedServoProperty); }
            set { SetValue(SelectedServoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedServo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedServoProperty =
            DependencyProperty.Register("SelectedServo", typeof(PhidgetsServo), typeof(PhidgetsServoBoardEditor), new UIPropertyMetadata(null));

        #endregion

        protected override void OnInterfaceChanged(HeliosInterface oldInterface, HeliosInterface newInterface)
        {
            PhidgetsServoBoard oldBoard = oldInterface as PhidgetsServoBoard;
            if (oldBoard != null)
            {
                oldBoard.Detach();
            }

            PhidgetsServoBoard newBoard = newInterface as PhidgetsServoBoard;
            if (newBoard != null)
            {
                newBoard.Attach();
            }

            _motorList.Clear();
            for (int i = 0; i < newBoard.ServoCount; i++)
            {
                _motorList.Add("Servo " + i);
            }
        }

        public override void Closed()
        {
            base.Closed();
            PhidgetsServoBoard board = Interface as PhidgetsServoBoard;
            if (board != null)
            {
                board.Detach();
            }
        }

        private void SelectedGroupChanged(object sender, SelectionChangedEventArgs e)
        {
            PhidgetsServoBoard board = Interface as PhidgetsServoBoard;
            PhidgetsServo servo = board.Servos[MotorListBox.SelectedIndex];
            CalibrationEditor.Calibration = servo.Calibration;

            SelectedServo = servo;
        }

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextValueChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedServo != null)
            {
                SelectedServo.ServoValue.ExecuteAction(new BindingValue(TextValueTextBox.Text), true);
            }
        }
    }
}

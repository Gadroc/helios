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
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for CalibrationEditorDouble.xaml
    /// </summary>
    public partial class CalibrationEditorDouble : UserControl
    {
        public CalibrationEditorDouble()
        {
            InitializeComponent();
        }

        public CalibrationPointCollectionDouble Calibration
        {
            get { return (CalibrationPointCollectionDouble)GetValue(CalibrationProperty); }
            set { SetValue(CalibrationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Calibration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CalibrationProperty =
            DependencyProperty.Register("Calibration", typeof(CalibrationPointCollectionDouble), typeof(CalibrationEditorDouble), new UIPropertyMetadata(null));

        public double MaxOutput
        {
            get { return (double)GetValue(MaxOutputProperty); }
            set { SetValue(MaxOutputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxOutput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxOutputProperty =
            DependencyProperty.Register("MaxOutput", typeof(double), typeof(CalibrationEditorDouble), new UIPropertyMetadata(double.MaxValue));

        public double MinOutput
        {
            get { return (double)GetValue(MinOutputProperty); }
            set { SetValue(MinOutputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinOutput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinOutputProperty =
            DependencyProperty.Register("MinOutput", typeof(double), typeof(CalibrationEditorDouble), new UIPropertyMetadata(double.MinValue));

        private void AddCalibrationPoint(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Calibration.AddPointAfter((CalibrationPointDouble)button.Tag);
        }

        private void RemoveCalibrationPoint(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Calibration.Remove((CalibrationPointDouble)button.Tag);
        }


        private void OutputChange(object sender, TextChangedEventArgs e)
        {
            HeliosTextBox box = sender as HeliosTextBox;
            if (box != null)
            {
                double value = 0;
                double.TryParse(box.Text, out value);
                if (value < MinOutput)
                {
                    box.Text = MinOutput.ToString();
                }
                else if (value > MaxOutput)
                {
                    box.Text = MaxOutput.ToString();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GadrocsWorkshop.Helios.Windows.Controls
{
    /// <summary>
    /// Interaction logic for CalibrationEditorLong.xaml
    /// </summary>
    public partial class CalibrationEditorLong : UserControl
    {
        public CalibrationEditorLong()
        {
            InitializeComponent();
        }

        public long MaxOutput
        {
            get { return (long)GetValue(MaxOutputProperty); }
            set { SetValue(MaxOutputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxOutput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxOutputProperty =
            DependencyProperty.Register("MaxOutput", typeof(long), typeof(CalibrationEditorLong), new UIPropertyMetadata(long.MaxValue));

        public long MinOutput
        {
            get { return (long)GetValue(MinOutputProperty); }
            set { SetValue(MinOutputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinOutput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinOutputProperty =
            DependencyProperty.Register("MinOutput", typeof(long), typeof(CalibrationEditorLong), new UIPropertyMetadata(long.MinValue));

        public CalibrationPointCollectionLong Calibration
        {
            get { return (CalibrationPointCollectionLong)GetValue(CalibrationProperty); }
            set { SetValue(CalibrationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Calibration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CalibrationProperty =
            DependencyProperty.Register("Calibration", typeof(CalibrationPointCollectionLong), typeof(CalibrationEditorLong), new UIPropertyMetadata(null));

        private void AddCalibrationPoint(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Calibration.AddPointAfter((CalibrationPointLong)button.Tag);
        }

        private void RemoveCalibrationPoint(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Calibration.Remove((CalibrationPointLong)button.Tag);
        }

        private void OutputChange(object sender, TextChangedEventArgs e)
        {
            HeliosTextBox box = sender as HeliosTextBox;
            if (box != null)
            {
                long value = 0;
                long.TryParse(box.Text, out value);
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

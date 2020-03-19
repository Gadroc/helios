namespace GadrocsWorkshop.Helios
{
    using System;
    using System.Reflection;
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

    /// <summary>
    /// Interaction logic for VersionReminderForm.xaml
    /// </summary>
    public partial class VersionReminderForm : Window
    {
        public VersionReminderForm()
        {
            InitializeComponent();
        }
        protected override void OnActivated(EventArgs e)
        {
            this.Topmost = true; 
            Version _runningVersion = Assembly.GetEntryAssembly().GetName().Version;
            VersionBlock.Text = _runningVersion.Major.ToString() + "." + _runningVersion.Minor.ToString() + "." + _runningVersion.Build.ToString() + "." + _runningVersion.Revision.ToString("0000");
            //StatusBlock.Text = "Released";
            base.OnActivated(e);
        }
        private void ComboBoxReminder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Boolean remindNextRelease = false;
            string cbTag = ((sender as ComboBox).SelectedItem as ComboBoxItem).Tag.ToString();
            if (cbTag == "Next")
            {
                cbTag = "7";
                remindNextRelease = true;
            }
            
            Int32 cbTagVal = Convert.ToInt32(cbTag);
            VersionChecker.SetNextCheckDate(DateTime.Today.AddDays(cbTagVal),remindNextRelease);
            this.Close();
        }

        private void ButtonDownload_Click(object sender, RoutedEventArgs e)
        {
            VersionChecker.SetDownloadNeeded();
            this.Close();
        }
    }
}

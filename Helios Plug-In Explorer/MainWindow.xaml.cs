using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using GadrocsWorkshop.Helios.Runtime;

namespace GadrocsWorkshop.Helios.PlugInExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HeliosRuntime _runtime;

        public MainWindow()
        {
            InitializeComponent();

            PlugIns = new ObservableCollection<PluginTreeItem>();

            _runtime = new HeliosRuntime();
            _runtime.InitializeAll();

            foreach (Lazy<IPlugIn, IPlugInMetaData> plugIn in _runtime.PlugIns)
            {
                PlugIns.Add(new PluginTreeItem(plugIn.Value, plugIn.Metadata));
            }
        }

        ~MainWindow()
        {
            _runtime.Shutdown();
        }

        public ObservableCollection<PluginTreeItem> PlugIns
        {
            get { return (ObservableCollection<PluginTreeItem>)GetValue(PlugInsProperty); }
            set { SetValue(PlugInsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlugIns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlugInsProperty =
            DependencyProperty.Register("PlugIns", typeof(ObservableCollection<PluginTreeItem>), typeof(MainWindow), new PropertyMetadata(null));


    }
}

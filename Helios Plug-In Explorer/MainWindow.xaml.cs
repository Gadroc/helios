//  Copyright 2013 Craig Courtney
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

namespace GadrocsWorkshop.Helios.PlugInExplorer
{
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

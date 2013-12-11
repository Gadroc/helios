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

namespace GadrocsWorkshop.Helios.ControlEditor.View
{
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
   
    using GadrocsWorkshop.Helios.ControlEditor.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserControlViewModel _userControl = null;
        private UserControl _editControl = null;
        private int _currentMode = -1;

        public MainWindow()
        {
            InitializeComponent();
            _userControl = new UserControlViewModel();
            setEditMode(0);
        }

        public void setEditMode(int mode)
        {
            if (_currentMode != mode)
            {
                if (_editControl != null)
                {
                    EditorPresenter.Content = null;
                    _editControl = null;
                }

                switch (mode)
                {
                    case 0: // Edit Properties
                        ControlPropertiesView controlView = new ControlPropertiesView();
                        controlView.DataContext = _userControl;
                        _editControl = controlView;
                        break;

                    case 1: // Edit Visuals
                        VisualsView visualView = new VisualsView();
                        visualView.DataContext = _userControl;
                        _editControl = visualView;
                        break;
                }

                EditorPresenter.Content = _editControl;
                if (_editControl != null) { _editControl.Focus(); }
                _currentMode = mode;
            }
        }

        private void EditModeClicked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;
            setEditMode(int.Parse(button.Tag.ToString()));
            e.Handled = true;
        }
    }
}

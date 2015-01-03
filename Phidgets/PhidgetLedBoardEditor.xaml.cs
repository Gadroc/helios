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
    using GadrocsWorkshop.Helios.Interfaces.Phidgets.UndoEvents;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for PhidgetLedBoardEditor.xaml
    /// </summary>
    public partial class PhidgetLedBoardEditor : HeliosInterfaceEditor
    {
        public PhidgetLedBoardEditor()
        {
            InitializeComponent();
        }

        #region Properties

        public LedGroupsViewModel GroupsViewModel
        {
            get { return (LedGroupsViewModel)GetValue(GroupsViewModelProperty); }
            set { SetValue(GroupsViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GroupsViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GroupsViewModelProperty =
            DependencyProperty.Register("GroupsViewModel", typeof(LedGroupsViewModel), typeof(PhidgetLedBoardEditor), new PropertyMetadata(null));

        #endregion

        protected override void OnInterfaceChanged(HeliosInterface oldInterface, HeliosInterface newInterface)
        {
            PhidgetLEDBoard oldBoard = oldInterface as PhidgetLEDBoard;
            if (oldBoard != null)
            {
                oldBoard.Detach();
            }

            PhidgetLEDBoard newBoard = newInterface as PhidgetLEDBoard;
            if (newBoard != null)
            {
                newBoard.Attach();
            }

            GroupsViewModel = new LedGroupsViewModel(newInterface as PhidgetLEDBoard);
        }

        private void AddLedGroup(object sender, RoutedEventArgs e)
        {
            PhidgetLEDBoard board = Interface as PhidgetLEDBoard;

            int i = 0;
            string name = "Led Group " + i++;
            while (board.LedGroups.ContainsKey(name))
            {
                name = "Led Group " + i++;
            }

            LEDGroup group = new LEDGroup(board, name);
            board.LedGroups.Add(group);

            LedGroupListBox.SelectedItem = GroupsViewModel.Last();

            ConfigManager.UndoManager.AddUndoItem(new AddLedGroupUndoEvent(board.LedGroups, group));
        }

        private void RemoveLedGroup(object sender, RoutedEventArgs e)
        {
            PhidgetLEDBoard board = Interface as PhidgetLEDBoard;
            LedGroupViewModel model = LedGroupListBox.SelectedItem as LedGroupViewModel;
            if (model != null && board.LedGroups.Contains(model.Group))
            {
                ConfigManager.UndoManager.AddUndoItem(new RemoveLedGroupUndoEvent(board.LedGroups, model.Group, board.LedGroups.IndexOf(model.Group)));
                board.LedGroups.Remove(model.Group);
            }
        }

        public override void Closed()
        {
            base.Closed();
            PhidgetLEDBoard board = Interface as PhidgetLEDBoard;
            if (board != null)
            {
                foreach (LEDGroup group in board.LedGroups)
                {
                    board.SetGroupPower(group, false);
                }
                board.Detach();
            }
        }

        private void SelectedGroupChanged(object sender, SelectionChangedEventArgs e)
        {
            PhidgetLEDBoard board = Interface as PhidgetLEDBoard;
            LedGroupViewModel model = LedGroupListBox.SelectedItem as LedGroupViewModel;
            if (model != null)
            {
                model.Group.Level = model.Group.DefaultLevel;
            }
            SetLeds();
        }

        private void DefaultLevelChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PhidgetLEDBoard board = Interface as PhidgetLEDBoard;
            LedGroupViewModel model = LedGroupListBox.SelectedItem as LedGroupViewModel;
            if (model != null)
            {
                model.Group.Level = model.Group.DefaultLevel;
                SetLeds();
            }
        }

        private void LEDClicked(object sender, RoutedEventArgs e)
        {
            PhidgetLEDBoard board = Interface as PhidgetLEDBoard;
            LedGroupViewModel model = LedGroupListBox.SelectedItem as LedGroupViewModel;
            if (model != null)
            {
                model.Group.Level = model.Group.DefaultLevel;
            }
            SetLeds();
        }

        private void SetLeds()
        {
            PhidgetLEDBoard board = Interface as PhidgetLEDBoard;
            LedGroupViewModel model = LedGroupListBox.SelectedItem as LedGroupViewModel;
            for (int i = 0; i < 64; i++)
            {
                int level = 0;
                if (model != null && model.Group.Leds.Contains(i))
                {
                    level = model.Group.DefaultLevel;
                }
                board.SetLedPower(i, level);
            }
        }

        private void EditorLostFocus(object sender, RoutedEventArgs e)
        {
            PhidgetLEDBoard board = Interface as PhidgetLEDBoard;
            for (int i = 0; i < 64; i++)
            {
                board.SetLedPower(i, 0);
            }
        }

        private void EditorGotFocus(object sender, RoutedEventArgs e)
        {
            PhidgetLEDBoard board = Interface as PhidgetLEDBoard;
            LedGroupViewModel model = LedGroupListBox.SelectedItem as LedGroupViewModel;
            foreach (LEDGroup group in board.LedGroups)
            {
                board.SetGroupPower(group, false);
            }
            if (model != null)
            {
                model.Group.Level = model.Group.DefaultLevel;
            }
            SetLeds();
        }
    }
}

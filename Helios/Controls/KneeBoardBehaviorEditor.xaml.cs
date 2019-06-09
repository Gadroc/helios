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


namespace GadrocsWorkshop.Helios.Controls
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for KneeBoardBehaviorEditor.xaml
    /// </summary>
    [HeliosPropertyEditor("Helios.Base.KneeBoard", "Behavior")]
    public partial class KneeBoardBehaviorEditor : HeliosPropertyEditor
    {
        public KneeBoardBehaviorEditor()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ControlProperty)
            {
                KneeBoard page = Control as KneeBoard;
                if (page != null && page.DefaultPosition > 0)
                {
                
                }
            }
            base.OnPropertyChanged(e);
        }

        private void Position_GotFocus(object sender, RoutedEventArgs e)
        {
            KneeBoard page = Control as KneeBoard;
            FrameworkElement senderControl = sender as FrameworkElement;
            if (senderControl != null && page != null)
            {
                int index = page.Positions.IndexOf((KneeBoardPosition)senderControl.Tag);
                page.CurrentPosition = index+1;
            }
        }

        private void Add_Position_Click(object sender, RoutedEventArgs e)
        {
            KneeBoard page = Control as KneeBoard;
            if (page != null)
            {
                KneeBoardPosition position = new KneeBoardPosition(page, page.Positions.Count + 1, "{Helios}/Images/KneeBoards/default_kneeboard_image.png");
                page.Positions.Add(position);
                ConfigManager.UndoManager.AddUndoItem(new KneeBoardAddPositionUndoEvent(page, position));
            }
        }

        private void DeletePosition_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            KneeBoard page = Control as KneeBoard;
            if (page != null)
            {
                e.CanExecute = (page.Positions.Count > 2);
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void DeletePosition_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            KneeBoard page = Control as KneeBoard;
            if (page != null && page.Positions.Contains((KneeBoardPosition)PositionList.SelectedItem))
            {
                KneeBoardPosition removedPosition = (KneeBoardPosition)PositionList.SelectedItem;
                int index = page.Positions.IndexOf(removedPosition);
                page.Positions.Remove(removedPosition);
                ConfigManager.UndoManager.AddUndoItem(new KneeBoardDeletePositionUndoEvent(page, removedPosition, index));
            }        
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            KneeBoard page = Control as KneeBoard;
            if (page != null)
            {
                page.CurrentPosition = PositionList.SelectedIndex  ;  //+1
            }
        }

      

        private void Delete_Position_Click(object sender, RoutedEventArgs e)
        {
            KneeBoard page = Control as KneeBoard;
            FrameworkElement senderControl = sender as FrameworkElement;
            if (senderControl != null && page != null)
            {
                KneeBoardPosition position = senderControl.Tag as KneeBoardPosition;
                if (position != null && page.Positions.Contains(position))
                {
                    page.Positions.Remove(position);
                }
            }
        }
    }
}

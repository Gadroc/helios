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

namespace GadrocsWorkshop.Helios.ProfileEditor.PropertyEditors
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Interaction logic for HeliosPanelNodePropertyEditor.xaml
    /// </summary>
    [HeliosPropertyEditor("*", "hply")]
    public partial class LayoutPropertyEditor : HeliosPropertyEditor, IDataErrorInfo
    {
        public LayoutPropertyEditor()
        {
            InitializeComponent();
        }

        #region Properties

        public override string Category
        {
            get
            {
                return "Layout";
            }
        }

        public string VisualName
        {
            get { return (string)GetValue(VisualNameProperty); }
            set { SetValue(VisualNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisualName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisualNameProperty =
            DependencyProperty.Register("VisualName", typeof(string), typeof(LayoutPropertyEditor), new PropertyMetadata(""));

        #endregion

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ControlProperty)
            {
                VisualName = Control.Name;
            }
            if (e.Property == VisualNameProperty)
            {
                if ((this as IDataErrorInfo)["VisualName"] == null)
                {
                    Control.Name = VisualName;
                }
            }

            base.OnPropertyChanged(e);
        }

        string IDataErrorInfo.Error
        {
            get { return null; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                if (columnName.Equals("VisualName"))
                {
                    if (string.IsNullOrWhiteSpace(VisualName))
                    {
                        return "Name can not be blank.";
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(VisualName, "^[a-zA-Z0-9_ ]*$"))
                    {
                        return "Name must not contain special characters.";
                    }
                    if (Control != null && Control.Parent != null && !VisualName.Equals(Control.Name) && Control.Parent.Children.ContainsKey(VisualName))
                    {
                        return "Name must be unique.";
                    }
                }
                return null;
            }
        }
    }
}

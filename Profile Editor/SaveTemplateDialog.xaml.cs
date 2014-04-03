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

namespace GadrocsWorkshop.Helios.ProfileEditor
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for SaveTemplateDialog.xaml
    /// </summary>
    public partial class SaveTemplateDialog : Window
    {
        public SaveTemplateDialog()
        {
            InitializeComponent();
        }

        #region Properties

        public string TemplateName
        {
            get { return (string)GetValue(TemplateNameProperty); }
            set { SetValue(TemplateNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TemplateName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplateNameProperty =
            DependencyProperty.Register("TemplateName", typeof(string), typeof(SaveTemplateDialog), new PropertyMetadata(""));

        public string TemplateCategory
        {
            get { return (string)GetValue(TemplateCategoryProperty); }
            set { SetValue(TemplateCategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TemplateCategory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplateCategoryProperty =
            DependencyProperty.Register("TemplateCategory", typeof(string), typeof(SaveTemplateDialog), new PropertyMetadata(""));

        public Visibility CateogryVisibility
        {
            get { return (Visibility)GetValue(CateogryVisibilityProperty); }
            set { SetValue(CateogryVisibilityProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowCateogry.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CateogryVisibilityProperty =
            DependencyProperty.Register("CateogryVisibility", typeof(Visibility), typeof(SaveTemplateDialog), new PropertyMetadata(Visibility.Visible));


        #endregion

        private void OK_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

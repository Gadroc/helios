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
    using GadrocsWorkshop.Helios.ProfileEditor.ViewModel;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for TemplateManager.xaml
    /// </summary>
    public partial class TemplateManagerWindow : Window
    {
        TemplateManagerGroupCollection _templateGroups = new TemplateManagerGroupCollection();

        public TemplateManagerWindow()
        {
            InitializeComponent();

            PopulateTemplates();
        }

        #region Properties

        public TemplateManagerGroupCollection TemplateGroups
        {
            get { return _templateGroups; }
        }

        public HeliosVisual TemplatePreview
        {
            get { return (HeliosVisual)GetValue(TemplatePreviewProperty); }
            set { SetValue(TemplatePreviewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TemplatePreview.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplatePreviewProperty =
            DependencyProperty.Register("TemplatePreview", typeof(HeliosVisual), typeof(TemplateManagerWindow), new PropertyMetadata(null));

        public string TemplateName
        {
            get { return (string)GetValue(TemplateNameProperty); }
            set { SetValue(TemplateNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TemplateName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplateNameProperty =
            DependencyProperty.Register("TemplateName", typeof(string), typeof(TemplateManagerWindow), new PropertyMetadata(null));

        public string TemplateCategory
        {
            get { return (string)GetValue(TemplateCategoryProperty); }
            set { SetValue(TemplateCategoryProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TemplateCategory.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemplateCategoryProperty =
            DependencyProperty.Register("TemplateCategory", typeof(string), typeof(TemplateManagerWindow), new PropertyMetadata(null));

        #endregion

        private void PopulateTemplates()
        {
            _templateGroups.Clear();
            foreach (HeliosTemplate template in ConfigManager.TemplateManager.UserTemplates)
            {
                TemplateManagerGroup group;
                if (_templateGroups.ContainsKey(template.Category))
                {
                    group = _templateGroups[template.Category];
                }
                else
                {
                    group = new TemplateManagerGroup(template.Category);
                    _templateGroups.Add(group);
                }
                group.Children.Add(template);
            }
        }

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {
            HeliosTemplate selectedTemplate = TemplateTreeView.SelectedItem as HeliosTemplate;
            if (selectedTemplate != null)
            {
                if (MessageBox.Show(this, "This action cannot be undone.  Are you sure you want to delete this template?", "Delete Template", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    ConfigManager.TemplateManager.UserTemplates.Remove(selectedTemplate);
                    PopulateTemplates();
                }
            }
        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            HeliosTemplate selectedTemplate = TemplateTreeView.SelectedItem as HeliosTemplate;
            if (selectedTemplate != null)
            {
                string templateKey = TemplateCategory + "." + TemplateName;

                if (ConfigManager.TemplateManager.UserTemplates.ContainsKey(templateKey))
                {
                    if (MessageBox.Show(this, "A template already exists with that name.  Do you want to overwrite the existing template?", "Overwrite Template", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        TemplateName = selectedTemplate.Name;
                        TemplateCategory = selectedTemplate.Category;
                        return;
                    }
                    ConfigManager.TemplateManager.UserTemplates.RemoveKey(templateKey);
                }

                ConfigManager.TemplateManager.UserTemplates.Remove(selectedTemplate);
                selectedTemplate.Name = TemplateName;
                selectedTemplate.Category = TemplateCategory;
                ConfigManager.TemplateManager.UserTemplates.Add(selectedTemplate);

                PopulateTemplates();
            }
        }

        private void SelectedItem_Changed(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            HeliosTemplate selectedTemplate = TemplateTreeView.SelectedItem as HeliosTemplate;
            if (selectedTemplate == null)
            {
                TemplateCategory = "";
                TemplateName = "";
                TemplatePreview = null;
            }
            else
            {
                TemplateName = selectedTemplate.Name;
                TemplateCategory = selectedTemplate.Category;
                TemplatePreview = selectedTemplate.CreateInstance();
                TemplatePreview.DesignMode = true;
            }
        }

        private void Preview_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (TemplatePreview != null)
            {
                TemplatePreview.MouseDown(e.GetPosition(PreviewView));
            }
        }

        private void Preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (TemplatePreview != null)
            {
                TemplatePreview.MouseDrag(e.GetPosition(PreviewView));
            }
        }

        private void Preview_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (TemplatePreview != null)
            {
                TemplatePreview.MouseUp(e.GetPosition(PreviewView));
            }
        }
    }

}

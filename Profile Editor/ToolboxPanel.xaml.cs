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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Interaction logic for ToolboxPanel.xaml
    /// </summary>
    public partial class ToolboxPanel : UserControl
    {
        private ToolboxGroupCollection _toolboxGroups;

        public ToolboxPanel()
        {
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                _toolboxGroups = new ToolboxGroupCollection();

                CollectionView toolboxCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(_toolboxGroups);
                toolboxCollectionView.SortDescriptions.Add(new System.ComponentModel.SortDescription("Name", System.ComponentModel.ListSortDirection.Ascending));

                foreach (HeliosDescriptor descriptor in ConfigManager.ModuleManager.ControlDescriptors)
                {
                    AddTool(new DescriptorToolboxItem(descriptor));
                }
                PopulateTemplates(ConfigManager.TemplateManager.ModuleTemplates);
                PopulateTemplates(ConfigManager.TemplateManager.UserTemplates);

                ConfigManager.TemplateManager.UserTemplates.CollectionChanged += new NotifyCollectionChangedEventHandler(UserTemplates_CollectionChanged);
            }
            InitializeComponent();
        }

        void UserTemplates_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (HeliosTemplate template in e.NewItems)
                {
                    if (template != null)
                    {
                        AddTool(new TemplateToolboxItem(template));
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (HeliosTemplate template in e.OldItems)
                {
                    if (template != null)
                    {
                        RemoveTemplateTool(template);
                    }
                }
            }
        }

        private void PopulateTemplates(IList<HeliosTemplate> templates)
        {
            foreach (HeliosTemplate template in templates)
            {
                AddTool(new TemplateToolboxItem(template));
            }
        }

        private void AddTool(ToolboxItem tool)
        {
            if (tool.Category != "_Hidden Parts")
            {
                ToolboxGroup group;
                if (_toolboxGroups.ContainsKey(tool.Category))
                {
                    group = _toolboxGroups[tool.Category];
                }
                else
                {
                    group = new ToolboxGroup(tool.Category);
                    group.DragAdvisor = new ToolboxDragAdvisor();
                    _toolboxGroups.Add(group);
                }

                group.Add(tool);
            }
        }

        private void RemoveTemplateTool(HeliosTemplate template)
        {

            if (_toolboxGroups.ContainsKey(template.Category))
            {
                ToolboxGroup group = _toolboxGroups[template.Category];

                if (group != null)
                {
                    ToolboxItem removeItem = null;

                    foreach (ToolboxItem item in group)
                    {
                        TemplateToolboxItem templateItem = item as TemplateToolboxItem;
                        if (templateItem != null)
                        {
                            if (templateItem.Template.Equals(template))
                            {
                                removeItem = item;
                                break;
                            }
                        }
                    }

                    if (removeItem != null)
                    {
                        group.Remove(removeItem);
                    }
                }
            }
        }

        #region Properties

        public HeliosProfile Profile
        {
            get { return (HeliosProfile)GetValue(ProfileProperty); }
            set { SetValue(ProfileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Profile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfileProperty =
            DependencyProperty.Register("Profile", typeof(HeliosProfile), typeof(ToolboxPanel), new PropertyMetadata(null));

        public ToolboxGroupCollection ToolboxGroups
        {
            get
            {
                return _toolboxGroups;
            }
        }

        #endregion
    }
}

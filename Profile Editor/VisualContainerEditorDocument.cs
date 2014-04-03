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
    using GadrocsWorkshop.Helios.ProfileEditor.PropertyEditors;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System.Windows;

    public class VisualContainerEditorDocument : HeliosEditorDocument
    {
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == BindingFocusProperty)
            {
                SetPropertyEditors();
            }

            base.OnPropertyChanged(e);
        }

        private void SetPropertyEditors()
        {
            PropertyEditors.Clear();

            HeliosVisual visual = BindingFocus as HeliosVisual;
            if (visual != null)
            {
                // Setup Layout Panel
                HeliosPropertyEditor layoutEditor;
                if (visual is Monitor)
                {
                    layoutEditor = new MonitorPropertyEditor();
                }
                else
                {
                    layoutEditor = new LayoutPropertyEditor();
                }
                layoutEditor.Control = visual;
                PropertyEditors.Add(layoutEditor);

                foreach (HeliosPropertyEditorDescriptor descriptor in ConfigManager.ModuleManager.GetPropertyEditors(visual.TypeIdentifier))
                {
                    HeliosPropertyEditor editor = descriptor.CreateInstance();
                    editor.Control = visual;
                    PropertyEditors.Add(editor);
                }
            }
        }

        public override bool HandlesScroll
        {
            get { return true; }
        }

        public override string Title
        {
            get { return "should never execute!"; }
        }
    }
}

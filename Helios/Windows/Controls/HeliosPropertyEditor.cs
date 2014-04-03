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

namespace GadrocsWorkshop.Helios.Windows.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using GadrocsWorkshop.Helios.ComponentModel;

    public class HeliosPropertyEditor : UserControl
    {
        public HeliosPropertyEditor()
        {
            Background = Brushes.Transparent;
        }

        #region Properties

        public HeliosVisual Control
        {
            get { return (HeliosVisual)GetValue(ControlProperty); }
            set { SetValue(ControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Control.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControlProperty =
            DependencyProperty.Register("Control", typeof(HeliosVisual), typeof(HeliosPropertyEditor), new UIPropertyMetadata(null));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsExpaneded.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpaneded", typeof(bool), typeof(HeliosPropertyEditor), new UIPropertyMetadata(true));

        public virtual string Category
        {
            get
            {
                object[] attrs = GetType().GetCustomAttributes(typeof(HeliosPropertyEditorAttribute), false);
                foreach (object attribute in attrs)
                {
                    HeliosPropertyEditorAttribute editorAttribute = attribute as HeliosPropertyEditorAttribute;
                    if (editorAttribute != null)
                    {
                        return editorAttribute.Category;
                    }
                }

                return "";
            }
        }

        #endregion
    }
}

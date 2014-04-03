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
	using System.Windows.Input;

    public abstract class HeliosEditorDocument : UserControl
    {
        static HeliosEditorDocument()
        {
            Type ownerType = typeof(HeliosEditorDocument);

            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ApplicationCommands.Undo, OnExecuteUndo, OnCanExecuteUndo));
            CommandManager.RegisterClassCommandBinding(ownerType, new CommandBinding(ApplicationCommands.Redo, OnExecuteRedo, OnCanExecuteRedo));
        }

        public HeliosEditorDocument()
        {
            PropertyEditors = new HeliosPropertyEditorCollection();
        }

        #region Properties

        public HeliosProfile Profile
        {
            get { return (HeliosProfile)GetValue(ProfileProperty); }
            set { SetValue(ProfileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Profile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfileProperty =
            DependencyProperty.Register("Profile", typeof(HeliosProfile), typeof(HeliosEditorDocument), new PropertyMetadata(null));

        public HeliosObject BindingFocus
        {
            get { return (HeliosObject)GetValue(BindingFocusProperty); }
            set { SetValue(BindingFocusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BindingFocus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingFocusProperty =
            DependencyProperty.Register("BindingFocus", typeof(HeliosObject), typeof(HeliosEditorDocument), new PropertyMetadata(null));

        public HeliosPropertyEditorCollection PropertyEditors
        {
            get { return (HeliosPropertyEditorCollection)GetValue(PropertyEditorsProperty); }
            set { SetValue(PropertyEditorsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PropertyEditors.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyEditorsProperty =
            DependencyProperty.Register("PropertyEditors", typeof(HeliosPropertyEditorCollection), typeof(HeliosEditorDocument), new PropertyMetadata(null));

        /// <summary>
        /// Flag indicating this editor internally handles scrolling.  Returning false will wrap this editor in a scrollerview before displaying.  Otherwise 
        /// editor will be displayed directly in layout.
        /// </summary>
        public abstract bool HandlesScroll { get; }

        /// <summary>
        /// Returns the title for this document.
        /// </summary>
        public abstract string Title { get; }

        #endregion

        public virtual void Closed()
        {
        }

        public virtual void SetBindingFocus(HeliosObject bindingFoucsObject)
        {
        }

        private static void OnExecuteUndo(object sender, ExecutedRoutedEventArgs e)
        {
            ConfigManager.UndoManager.Undo();
        }

        private static void OnCanExecuteUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ConfigManager.UndoManager.CanUndo;
        }

        private static void OnExecuteRedo(object sender, ExecutedRoutedEventArgs e)
        {
            ConfigManager.UndoManager.Redo();
        }

        private static void OnCanExecuteRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ConfigManager.UndoManager.CanRedo;
        }

    }
}

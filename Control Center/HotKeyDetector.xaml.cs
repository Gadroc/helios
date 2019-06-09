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

namespace GadrocsWorkshop.Helios.ControlCenter
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
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    
    /// <summary>
    /// Interaction logic for HotKeyDetector.xaml
    /// </summary>
    public partial class HotKeyDetector : Window
    {
        private delegate void CloseDelegate();

        public HotKeyDetector()
        {
            InitializeComponent();
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        public string HotKeyDescription
        {
            get { return (string)GetValue(HotKeyDescriptionProperty); }
            set { SetValue(HotKeyDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HotKeyDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HotKeyDescriptionProperty =
            DependencyProperty.Register("HotKeyDescription", typeof(string), typeof(HotKeyDetector), new PropertyMetadata(""));

        public ModifierKeys Modifiers
        {
            get { return (ModifierKeys)GetValue(ModifiersProperty); }
            set { SetValue(ModifiersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Modifiers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModifiersProperty =
            DependencyProperty.Register("Modifiers", typeof(ModifierKeys), typeof(HotKeyDetector), new PropertyMetadata(ModifierKeys.None));

        public Keys Key
        {
            get { return (Keys)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Key.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(Keys), typeof(HotKeyDetector), new UIPropertyMetadata(Keys.None));

        void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            if (msg.message == NativeMethods.WM_KEYDOWN)
            {
                Modifiers = Modifiers | ModifierForVK((Keys)msg.wParam);
            }
            if (msg.message == NativeMethods.WM_KEYUP)
            {
                Keys upKey = (Keys)msg.wParam;
                ModifierKeys mods = ModifierForVK(upKey);
                if (mods != ModifierKeys.None)
                {
                    Modifiers = Modifiers & ~ModifierForVK(upKey);
                }
                else if (upKey != Keys.None)
                {
                    Key = upKey;
                    DispatcherTimer minimizeTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 250), DispatcherPriority.Normal, TimedClose, Dispatcher);
                    minimizeTimer.Start();
                }
            }

            HotKeyDescription = KeyboardEmulator.ModifierKeysToString(Modifiers) + Key.ToString();
        }

        public ModifierKeys ModifierForVK(Keys key)
        {
            switch (key)
            {
                case Keys.Shift:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.ShiftKey:
                    return ModifierKeys.Shift;
                case Keys.Control:
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.ControlKey:
                    return ModifierKeys.Control;
                case Keys.Alt:
                    return ModifierKeys.Alt;
                case Keys.LWin:
                case Keys.RWin:
                    return ModifierKeys.Windows;
                default:
                    return ModifierKeys.None;
            }
        }

        void TimedClose(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            Dispatcher.Invoke(new CloseDelegate(Close), null);
        }
    }
}

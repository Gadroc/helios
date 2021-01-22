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

/// <summary>
/// This has been deprecated in favour of ODU_1 which uses text displays and a higher res background image
/// </summary>


namespace GadrocsWorkshop.Helios.Gauges.AV8B
{
    using GadrocsWorkshop.Helios.Gauges.AV8B;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Windows.Media;
    using System.Windows;
    using System.Windows.Threading;


    [HeliosControl("Helios.AV8B.ODU", "Option Display Unit", "_Hidden Parts", typeof(AV8BDeviceRenderer))]
    class ODU_AV8B : AV8BDevice
    {
        // these three sections are the dead space in the ODU image.
        private static readonly Rect SCREEN_RECT_L = new Rect(0,168, 398,68);
        private Rect _scaledScreenRectL = SCREEN_RECT_L;
        private static readonly Rect SCREEN_RECT_LB = new Rect(0, 0, 194, 65);
        private Rect _scaledScreenRectLB = SCREEN_RECT_LB;
        private static readonly Rect SCREEN_RECT_R = new Rect(79, 0, 115, 244);
        private Rect _scaledScreenRectR = SCREEN_RECT_R;

        public ODU_AV8B()
            : base("Option Display Unit", new Size(398, 236))
        {
            AddButton("ODU 1", 199, 28, new Size(36, 36));
            AddButton("ODU 2", 199, 78, new Size(36, 36));
            AddButton("ODU 3", 199, 128, new Size(36, 36));
            AddButton("ODU 4", 39, 78, new Size(36, 36));
            AddButton("ODU 5", 39, 128, new Size(36, 36));
        }

        public override string BezelImage
        {
            get { return "{AV-8B}/Images/ODU.png"; }
        }

        private new void AddTrigger(IBindingTrigger trigger, string device)
        {
            trigger.Device = device;
            Triggers.Add(trigger);
        }

        private new void AddAction(IBindingAction action, string device)
        {
            action.Device = device;
            Actions.Add(action);
        }
        private void AddButton(string name, double x, double y) { AddButton(name, x, y, false); }
        private void AddButton(string name, double x, double y, Size size) { AddButton(name, x, y, size, false); }
        private void AddButton(string name, double x, double y, bool horizontal) { AddButton(name, x, y, new Size(40, 40), false); }
        private void AddButton(string name, double x, double y, Size size, bool horizontal) { AddButton(name, x, y, size, horizontal, false); }
        private void AddButton(string name, double x, double y, Size size, bool horizontal, bool altImage)
        {
            Helios.Controls.PushButton button = new Helios.Controls.PushButton();
            button.Top = y;
            button.Left = x;
            button.Width = size.Width;
            button.Height = size.Height;
            button.Image = "{AV-8B}/Images/ODU Button Up.png";
            button.PushedImage = "{AV-8B}/Images/ODU Button Dn.png";
            button.Text = "";
            button.Name = "ODU Key " + name;

            Children.Add(button);

            AddTrigger(button.Triggers["pushed"], "ODU Key " + name);
            AddTrigger(button.Triggers["released"], "ODU Key " + name);

            AddAction(button.Actions["push"], "ODU Key " + name);
            AddAction(button.Actions["release"], "ODU Key " + name);
            AddAction(button.Actions["set.physical state"], "ODU Key " + name);
        }


        public override bool HitTest(Point location)
        {
            if (_scaledScreenRectL.Contains(location) || _scaledScreenRectLB.Contains(location) || _scaledScreenRectR.Contains(location))
            {
                return false;
            }

            return true;
        }
        public override void MouseDown(Point location)
        {
            // No-Op
        }

        public override void MouseDrag(Point location)
        {
            // No-Op
        }

        public override void MouseUp(Point location)
        {
            // No-Op
        }

    }
}
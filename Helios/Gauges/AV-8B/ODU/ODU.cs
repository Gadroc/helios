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

namespace GadrocsWorkshop.Helios.Gauges.AV8B
{
    using GadrocsWorkshop.Helios.Gauges.AV8B;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Windows.Media;
    using System.Windows;
    using System.Windows.Threading;


    [HeliosControl("Helios.AV8B.ODU", "Option Display Unit", "AV-8B", typeof(AV8BDeviceRenderer))]
    class UFC_AV8B : AV8BDevice
    {
        // these three sections are the dead space in the UFC image.
        private static readonly Rect SCREEN_RECT_L = new Rect(0, 135, 38, 415);
        private Rect _scaledScreenRectL = SCREEN_RECT_L;
        private static readonly Rect SCREEN_RECT_LB = new Rect(38, 476, 103, 74);
        private Rect _scaledScreenRectLB = SCREEN_RECT_LB;
        private static readonly Rect SCREEN_RECT_R = new Rect(743, 102, 65, 448);
        private Rect _scaledScreenRectR = SCREEN_RECT_R;
        private String _font = "MS 33558";
        //private String _font = "Franklin Gothic";

        public UFC_AV8B()
            : base("Option Display Unit", new Size(808, 550))
        {

            AddButton("1", 266, 126, new Size(60, 60));
            AddButton("2", 337, 126, new Size(60, 60));
            AddButton("3", 410, 126, new Size(60, 60));
            AddButton("4", 410, 196, new Size(60, 60));
            AddButton("5", 337, 196, new Size(60, 60));


        }

        public override string BezelImage
        {
            get { return "{Helios}/Images/AV-8B/ODU.png"; }
        }

        private void AddTrigger(IBindingTrigger trigger, string device)
        {
            trigger.Device = device;
            Triggers.Add(trigger);
        }

        private void AddAction(IBindingAction action, string device)
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
            button.Image = "{Helios}/Images/AV-8B/UFC Button Up " + name + ".png";
            button.PushedImage = "{Helios}/Images/AV-8B/UFC Button Dn " + name + ".png";
            button.Text = "";
            button.Name = "UFC Key " + name;

            Children.Add(button);

            AddTrigger(button.Triggers["pushed"], "UFC Key " + name);
            AddTrigger(button.Triggers["released"], "UFC Key " + name);

            AddAction(button.Actions["push"], "UFC Key " + name);
            AddAction(button.Actions["release"], "UFC Key " + name);
            AddAction(button.Actions["set.physical state"], "UFC Key " + name);
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
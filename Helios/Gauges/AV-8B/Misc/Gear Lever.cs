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
    using System.Windows;

    [HeliosControl("Helios.AV8B.GearLever", "Landing Gear Lever", "AV-8B (Deprecated)", typeof(AV8BDeviceRenderer))]
    class Gear: AV8BDevice
    {
 
        public Gear()
            : base("Landing Gear Lever", new Size(85, 220))
        {
            Helios.Controls.ToggleSwitch toggle = new Helios.Controls.ToggleSwitch();
            toggle.Top = 0;
            toggle.Left = 0;
            toggle.Width = 85;
            toggle.Height = 220;
            toggle.DefaultPosition = ToggleSwitchPosition.Two;
            toggle.PositionOneImage = "{Helios}/Images/AV-8B/Gear Lever Up.png";
            toggle.PositionTwoImage = "{Helios}/Images/AV-8B/Gear Lever Dn.png";
            toggle.PositionOneIndicatorOnImage = "{Helios}/Images/AV-8B/Gear Lever Up Red.png";
            toggle.PositionTwoIndicatorOnImage = "{Helios}/Images/AV-8B/Gear Lever Dn Red.png";
            toggle.Name = "Landing Gear Lever";

            Children.Add(toggle);
            foreach (IBindingTrigger trigger in toggle.Triggers)
            {
                AddTrigger(trigger, "Landing Gear Lever");
            }
            foreach (IBindingAction action in toggle.Actions)
            {
                AddAction(action, "Landing Gear Lever");
            }

            //AddTrigger(toggle.Triggers["pushed"], name);
            //AddTrigger(toggle.Triggers["released"], name);

            //AddAction(toggle.Actions["push"], name);
            //AddAction(toggle.Actions["release"], name);
            //AddAction(toggle.Actions["set.physical state"], name);

        }
        public override string BezelImage
        {
            get { return ""; }
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

        public override bool HitTest(Point location)
        {
            //if (_scaledScreenRect.Contains(location))
            //{
            //    return false;
            //}

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

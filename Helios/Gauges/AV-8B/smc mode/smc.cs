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
    using System;
    using System.Windows;

    [HeliosControl("Helios.AV8B.smc", "Stores Management Panel", "AV-8B", typeof(GaugeRenderer))]
    public class SMC: BaseGauge
    {
 
        public SMC()
            : base("Stores Management Panel", new Size(703, 254))
        {

            AddDisplay("Stores Mode", new Helios.Gauges.AV8B.smcModeDisplay(), new Point(48, 34), new Size(32, 32));
            AddDisplay("Fuze Mode", new Helios.Gauges.AV8B.fuzeDisplay(), new Point(120, 34), new Size(60, 32));
            AddDisplay("Quantity", new Helios.Gauges.AV8B.TwoDigitDisplay(), new Point(226, 34), new Size(64, 32));
            AddDisplay("Multiple", new Helios.Gauges.AV8B.OneDigitDisplay(), new Point(340, 34), new Size(32, 32));
            AddDisplay("Interval", new Helios.Gauges.AV8B.ThreeDigitDisplay(), new Point(428, 34), new Size(96, 32));

            Components.Add(new GaugeImage("{Helios}/Images/AV-8B/AV-8B SMC faceplate.png", new Rect(0d, 0d, 703d, 254d)));
        }
        private void AddDisplay(string name, BaseGauge _gauge, Point posn, Size displaySize)
        {
            _gauge.Name = name;
            _gauge.Width = displaySize.Width;
            _gauge.Height = displaySize.Height;
            _gauge.Left = posn.X;
            _gauge.Top = posn.Y;
            Children.Add(_gauge);
            foreach (IBindingTrigger trigger in _gauge.Triggers)
            {
                AddTrigger(trigger, name);
            }
            foreach (IBindingAction action in _gauge.Actions)
            {
                AddAction(action, name);
            }
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
        protected override void OnPropertyChanged(PropertyNotificationEventArgs args)
        {
            OnDisplayUpdate();
            base.OnPropertyChanged(args);
        }


    }
}

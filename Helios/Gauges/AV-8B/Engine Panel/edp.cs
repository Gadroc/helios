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

    [HeliosControl("Helios.AV8B.edp", "AV-8B Engine Panel", "AV-8B", typeof(AV8BDeviceRenderer))]
    class edp: AV8BDevice
    {
        public edp()
            : base("Engine Panel", new Size(528,302))
        {
            AddDisplay("Nozzle Position", new Helios.Gauges.AV8B.edpNoz(), new Point(405d, 140d), new Size(18d, 72d));  //nozzle needle
            AddDisplay("Engine RPM Indicator", new Helios.Gauges.AV8B.FourDigitDisplay(), new Point(186, 45), new Size(120, 42));
            AddDisplay("Engine Duct Indicator", new Helios.Gauges.AV8B.ThreeDigitDisplay(), new Point(44, 45), new Size(90, 42));
            AddDisplay("Engine FF Indicator", new Helios.Gauges.AV8B.ThreeDigitDisplay(), new Point(44, 137), new Size(90, 42));
            AddDisplay("Jet Pipe Temp Indicator", new Helios.Gauges.AV8B.ThreeDigitDisplay(), new Point(214, 137), new Size(90, 42));
            AddDisplay("Stabilizer Direction Indicator", new Helios.Gauges.AV8B.stabilizerDisplay(), new Point(44, 232), new Size(30, 42));
            AddDisplay("Stabilizer Angle Indicator", new Helios.Gauges.AV8B.TwoDigitDisplay(), new Point(73, 232), new Size(60, 42));
            AddDisplay("H2O Amount Indicator", new Helios.Gauges.AV8B.TwoDigitDisplay(), new Point(214, 232), new Size(60, 42));
            AddIndicator("H2O Flow Indicator",new Point(158,234),new Size(32,32));

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
            if(name == "Nozzle Position")
            {
                AddAction(_gauge.Actions["set.nozzle angle"], name);
            }
            else
            {
                AddAction(_gauge.Actions["set.value"], name);
            }

        }
        private void AddIndicator(string name, Point posn , Size size)
        {
            Helios.Controls.Indicator indicator = new Helios.Controls.Indicator();
            indicator.Top = posn.Y;
            indicator.Left = posn.X;
            indicator.Width = size.Width;
            indicator.Height = size.Height;
            indicator.OnImage = "{Helios}/Gauges/AV-8B/Engine Panel/edp_water_light.xaml";
            indicator.OffImage = "{Helios}/Images/AV-8B/_transparant.png";
            indicator.Text = "";
            indicator.Name = name;

            Children.Add(indicator);
            foreach (IBindingTrigger trigger in indicator.Triggers)
            {
                AddTrigger(trigger, name);
            }
            AddAction(indicator.Actions["set.indicator"], name);


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

            return false;  // nothing to press on the EDP so return false.
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
        public override string BezelImage
        {
            get { return "{Helios}/Gauges/AV-8B/Engine Panel/edp_faceplate.xaml"; }
        }
    }
}

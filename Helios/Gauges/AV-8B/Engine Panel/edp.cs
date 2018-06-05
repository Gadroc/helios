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

    [HeliosControl("Helios.AV8B.edp", "AV-8B Engine Panel", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class edp: BaseGauge
    {
        private HeliosValue _angle;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;

        public edp()
            : base("Engine Panel", new Size(528,302))
        {


            Helios.Gauges.AV8B.FourDigitDisplay _rpm = new Helios.Gauges.AV8B.FourDigitDisplay();
            _rpm.Name = "Engine RPM Indicator";
            _rpm.Width = 120;
            _rpm.Height = 42;
            _rpm.Left = 186;
            _rpm.Top = 45;
            Children.Add(_rpm);
            foreach (IBindingTrigger trigger in _rpm.Triggers)
            {
                AddTrigger(trigger, "Engine RPM Indicator");
            }
            foreach (IBindingAction action in _rpm.Actions)
            {
                AddAction(action, "Engine RPM Indicator");
            }

            Helios.Gauges.AV8B.ThreeDigitDisplay _duct = new Helios.Gauges.AV8B.ThreeDigitDisplay();
            _duct.Name = "Engine Duct Indicator";
            _duct.Width = 90;
            _duct.Height = 42;
            _duct.Left = 44;
            _duct.Top = 45;
            Children.Add(_duct);
            foreach (IBindingTrigger trigger in _duct.Triggers)
            {
                AddTrigger(trigger, "Engine Duct Indicator");
            }
            foreach (IBindingAction action in _duct.Actions)
            {
                AddAction(action, "Engine Duct Indicator");
            }
            Helios.Gauges.AV8B.ThreeDigitDisplay _jpt = new Helios.Gauges.AV8B.ThreeDigitDisplay();
            _jpt.Name = "Jet Pipe Temp Indicator";
            _jpt.Width = 90;
            _jpt.Height = 42;
            _jpt.Left = 214;
            _jpt.Top = 137;
            Children.Add(_jpt);
            foreach (IBindingTrigger trigger in _jpt.Triggers)
            {
                AddTrigger(trigger, "Jet Pipe Temp Indicator");
            }
            foreach (IBindingAction action in _jpt.Actions)
            {
                AddAction(action, "Jet Pipe Temp Indicator");
            }
            Helios.Gauges.AV8B.ThreeDigitDisplay _ff = new Helios.Gauges.AV8B.ThreeDigitDisplay();
            _ff.Name = "Engine FF Indicator";
            _ff.Width = 90;
            _ff.Height = 42;
            _ff.Left = 44;
            _ff.Top = 137;
            Children.Add(_ff);
            foreach (IBindingTrigger trigger in _ff.Triggers)
            {
                AddTrigger(trigger, "Engine FF Indicator");
            }
            foreach (IBindingAction action in _ff.Actions)
            {
                AddAction(action, "Engine FF Indicator");
            }
            Helios.Gauges.AV8B.TwoDigitDisplay _water = new Helios.Gauges.AV8B.TwoDigitDisplay();
            _water.Name = "Water Amount Indicator";
            _water.Width = 60;
            _water.Height = 42;
            _water.Left = 214;
            _water.Top = 233;
            Children.Add(_water);
            foreach (IBindingTrigger trigger in _water.Triggers)
            {
                AddTrigger(trigger, "Water Amount Indicator");
            }
            foreach (IBindingAction action in _water.Actions)
            {
                AddAction(action, "Water Amount Indicator");
            }
            Helios.Gauges.AV8B.TwoDigitDisplay _stabAngle = new Helios.Gauges.AV8B.TwoDigitDisplay();
            _stabAngle.Name = "Stabilizer Angle Indicator";
            _stabAngle.Width = 60;
            _stabAngle.Height = 42;
            _stabAngle.Left = 73;
            _stabAngle.Top = 232;
            Children.Add(_stabAngle);
            foreach (IBindingTrigger trigger in _stabAngle.Triggers)
            {
                AddTrigger(trigger, "Stabilizer Angle Indicator");
            }
            foreach (IBindingAction action in _stabAngle.Actions)
            {
                AddAction(action, "Stabilizer Angle Indicator");
            }
            Helios.Gauges.AV8B.stabilizerDisplay _stabDirection = new Helios.Gauges.AV8B.stabilizerDisplay();
            _stabDirection.Name = "Stabilizer Direction Indicator";
            _stabDirection.Width = 30;
            _stabDirection.Height = 42;
            _stabDirection.Left = 44;
            _stabDirection.Top = 232;
            Children.Add(_stabDirection);
            foreach (IBindingTrigger trigger in _stabDirection.Triggers)
            {
                AddTrigger(trigger, "Stabilizer Direction Indicator");
            }
            foreach (IBindingAction action in _stabDirection.Actions)
            {
                AddAction(action, "Stabilizer Direction Indicator");
            }



            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/Engine Panel/edp_faceplate.xaml", new Rect(0d, 0d, 528d, 302d)));
            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 0.94d, 150d);
            _needle = new GaugeNeedle("{Helios}/Gauges/AV-8B/Common/nozzle_needle.xaml", new Point(440d, 138d), new Size(18d, 72d), new Point(7.6d, 28.1d), 90d);
            Components.Add(_needle);

            _angle = new HeliosValue(this, new BindingValue(0d), "", "nozzle angle", "Current position of Nozzles.", "(0 - 120)", BindingValueUnits.Degrees);
            _angle.Execute += new HeliosActionHandler(Angle_Execute);
            Actions.Add(_angle);

        }

        void Angle_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
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

    }
}

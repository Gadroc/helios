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

namespace GadrocsWorkshop.Helios.Interfaces.DCS.Common
{
    using GadrocsWorkshop.Helios.UDPInterface;
    using System;
    using System.Globalization;

    public class Axis : NetworkFunction
    {
        private string _id;
        private string _format;

        private string _actionData;

        private double _argValue;
        private double _argMin;
        private double _argMax;

        private bool _loop;

        private HeliosValue _value;

        private HeliosAction _incrementAction;
        private HeliosAction _decrementAction;

        public Axis(BaseUDPInterface sourceInterface, string deviceId, string buttonId, string argId, double argValue, double argMin, double argMax, string device, string name)
            : this(sourceInterface, deviceId, buttonId, argId, argValue, argMin, argMax, device, name, false, "%.3f")
        {
        }

        public Axis(BaseUDPInterface sourceInterface, string deviceId, string buttonId, string argId, double argValue, double argMin, double argMax, string device, string name, bool loop, string exportFormat)
            : base(sourceInterface)
        {
            _id = argId;
            _format = exportFormat;
            _loop = loop;

            _argValue = argValue;
            _argMin = argMin;
            _argMax = argMax;

            _actionData = "C" + deviceId + "," + buttonId + ",";

            _value = new HeliosValue(sourceInterface, new BindingValue(0.0d), device, name, "Current value of this axis.", argMin.ToString() + "-" + argMax.ToString(), BindingValueUnits.Numeric);
            _value.Execute += new HeliosActionHandler(Value_Execute);
            Values.Add(_value);
            Actions.Add(_value);
            Triggers.Add(_value);

            _incrementAction = new HeliosAction(sourceInterface, device, name, "increment", "Increments this axis value.", "Amount to increment by (Default:"+ argValue + ")", BindingValueUnits.Numeric);
            _incrementAction.Execute += new HeliosActionHandler(IncrementAction_Execute);
            Actions.Add(_incrementAction);

            _decrementAction = new HeliosAction(sourceInterface, device, name, "decrement", "Decrement this axis value.", "Amount to decrement by (Default:" + -argValue + ")", BindingValueUnits.Numeric);
            _decrementAction.Execute += new HeliosActionHandler(DecrementAction_Execute);
            Actions.Add(_decrementAction);
        }

        void DecrementAction_Execute(object action, HeliosActionEventArgs e)
        {
            double newValue = _value.Value.DoubleValue - _argValue;
            if (_loop)
            {
                while (newValue < _argMin)
                {
                    newValue += (_argMax - _argMin);
                }
            }
            else
            {
                newValue = Math.Max(_argMin, newValue);
            }
            _value.SetValue(new BindingValue(newValue), e.BypassCascadingTriggers);
            SourceInterface.SendData(_actionData + _value.Value.DoubleValue.ToString(CultureInfo.InvariantCulture));
        }

        void IncrementAction_Execute(object action, HeliosActionEventArgs e)
        {
            double newValue = _value.Value.DoubleValue + _argValue;
            if (_loop)
            {
                while (newValue > _argMax)
                {
                    newValue -= (_argMax - _argMin);
                }
            }
            else
            {
                newValue = Math.Min(_argMax, newValue);
            }
            _value.SetValue(new BindingValue(newValue), e.BypassCascadingTriggers);
            SourceInterface.SendData(_actionData + _value.Value.DoubleValue.ToString(CultureInfo.InvariantCulture));
        }

        void Value_Execute(object action, HeliosActionEventArgs e)
        {
            _value.SetValue(e.Value, e.BypassCascadingTriggers);
            SourceInterface.SendData(_actionData + _value.Value.DoubleValue.ToString(CultureInfo.InvariantCulture));
        }

        public override void ProcessNetworkData(string id, string value)
        {
            double newValue;
            if (double.TryParse(value, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out newValue))
            {
                if (newValue > _argMax) newValue = _argMax;
                if (newValue < _argMin) newValue = _argMin;
                _value.SetValue(new BindingValue(newValue), false);
            }          
        }

        public override ExportDataElement[] GetDataElements()
        {
            return new ExportDataElement[] { new DCSDataElement(_id, _format) };
        }

        public override void Reset()
        {
            _value.SetValue(BindingValue.Empty, true);
        }
    }
}

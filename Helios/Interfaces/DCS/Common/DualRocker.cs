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

    public class DualRocker : NetworkFunction
    {
        private string _id;
        private string _format;
		private string _arg_name;

		private string _position1Name;
        private string _position2Name;

        private string _pushed1ActionData;
        private string _pushed2ActionData;
        private string _release1ActionData;
        private string _release2ActionData;

        private string _position1Value;
        private string _releaseValue;
        private string _position2Value;

        private HeliosAction _push1Action;
        private HeliosAction _push2Action;
        private HeliosAction _releaseAction;

        private HeliosTrigger _pushed1Trigger;
        private HeliosTrigger _pushed2Trigger;
        private HeliosTrigger _releasedTrigger;

        private HeliosValue _value, _arg_value;

        private bool _release2 = false;

        public DualRocker(BaseUDPInterface sourceInterface, string deviceId, string button1Id, string button2Id, string releaseButtonId, string releaseButton2Id, string argId, string device, string name, bool vertical)
            : this(sourceInterface, deviceId, button1Id, button2Id, releaseButtonId, releaseButton2Id, argId, device, name, vertical, "1", "-1", "0", "%1d")
        {
        }

        public DualRocker(BaseUDPInterface sourceInterface, string deviceId, string button1Id, string button2Id, string releaseButtonId, string releaseButton2Id, string argId, string device, string name, bool vertical, string push1Value, string push2Value, string releaseValue, string exportFormat)
            : base(sourceInterface)
        {
            _id = argId;
			
			_arg_name = "Argument Value of " + name;
			_format = exportFormat;

            _position1Value = push1Value;
            _position2Value = push2Value;
            _releaseValue = releaseValue;

            _pushed1ActionData = "C" + deviceId + "," + button1Id + "," + push1Value;
            _pushed2ActionData = "C" + deviceId + "," + button2Id + "," + push2Value;
            _release1ActionData = "C" + deviceId + "," + releaseButtonId + "," + releaseValue;
            _release2ActionData = "C" + deviceId + "," + releaseButton2Id + "," + releaseValue;

            if (vertical)
            {
                _position1Name = "up";
                _position2Name = "down";
            }
            else
            {
                _position1Name = "left";
                _position2Name = "right";
            }


            _value = new HeliosValue(sourceInterface, new BindingValue(false), device, name, "Current position of this rocker.", "1=" + _position1Name + ", 2=released" + ", 3=" + _position2Name, BindingValueUnits.Numeric);
            Values.Add(_value);
            Triggers.Add(_value);

            _pushed1Trigger = new HeliosTrigger(sourceInterface, device, name, "pushed " + _position1Name, "Fired when this rocker is pushed " + _position1Name + " in the simulator.");
            Triggers.Add(_pushed1Trigger);
            _pushed2Trigger = new HeliosTrigger(sourceInterface, device, name, "pushed " + _position2Name, "Fired when this rocker is pushed " + _position2Name + " in the simulator.");
            Triggers.Add(_pushed2Trigger);

            _releasedTrigger = new HeliosTrigger(sourceInterface, device, name, "released", "Fired when this rocker is released in the simulator.");
            Triggers.Add(_releasedTrigger);

            _push1Action = new HeliosAction(sourceInterface, device, name, "push " + _position1Name, "Pushes this rocker " + _position1Name + " in the simulator");
            _push1Action.Execute += new HeliosActionHandler(Push1Action_Execute);
            Actions.Add(_push1Action);

            _push2Action = new HeliosAction(sourceInterface, device, name, "push " + _position2Name, "Pushes this rocker " + _position2Name + " in the simulator");
            _push2Action.Execute += new HeliosActionHandler(Push2Action_Execute);
            Actions.Add(_push2Action);

            _releaseAction = new HeliosAction(sourceInterface, device, name, "release", "Releases the rocker in the simulator.");
            _releaseAction.Execute += new HeliosActionHandler(ReleaseAction_Execute);
            Actions.Add(_releaseAction);

			_arg_value = new HeliosValue(sourceInterface, BindingValue.Empty, device, _arg_name, "Argument value in DCS", "argument value", BindingValueUnits.Numeric);

			Values.Add(_arg_value);
			Triggers.Add(_arg_value);
		}

        void ReleaseAction_Execute(object action, HeliosActionEventArgs e)
        {
            if (_release2)
            {
                SourceInterface.SendData(_release2ActionData);
            }
            else
            {
                SourceInterface.SendData(_release1ActionData);
            }
        }

        void Push1Action_Execute(object action, HeliosActionEventArgs e)
        {
            _release2 = false;
            SourceInterface.SendData(_pushed1ActionData);
        }

        void Push2Action_Execute(object action, HeliosActionEventArgs e)
        {
            _release2 = true;
            SourceInterface.SendData(_pushed2ActionData);
        }

        public override void ProcessNetworkData(string id, string value)
        {
            if (value.Equals(_position1Value))
            {
                _value.SetValue(new BindingValue(1), false);
                _pushed1Trigger.FireTrigger(BindingValue.Empty);
            }

            else if (value.Equals(_position2Value))
            {
                _value.SetValue(new BindingValue(3), false);
                _pushed1Trigger.FireTrigger(BindingValue.Empty);
            }

            else if (value.Equals(_releaseValue))
            {
                _value.SetValue(new BindingValue(2), false);
                _releasedTrigger.FireTrigger(BindingValue.Empty);
            }
			_arg_value.SetValue(new BindingValue(value), false);
		}

        public override ExportDataElement[] GetDataElements()
        {
            return new ExportDataElement[] { new DCSDataElement(_id, _format) };
        }

        public override void Reset()
        {
            _value.SetValue(BindingValue.Empty, true);
			_arg_value.SetValue(BindingValue.Empty, true);
		}

    }
}

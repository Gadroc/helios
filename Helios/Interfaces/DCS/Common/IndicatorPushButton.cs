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

    public class IndicatorPushButton : NetworkFunction
    {
        private string _id;
        private string _format;

        private string _pushActionData;
        private string _releaseActionData;
        private string _pushValue;
        private string _releaseValue;

        private HeliosAction _pushAction;
        private HeliosAction _releaseAction;
        private HeliosTrigger _pushedTrigger;
        private HeliosTrigger _releasedTrigger;
        private HeliosValue _value;
        private HeliosValue _indicatorValue;

        public IndicatorPushButton(BaseUDPInterface sourceInterface, string deviceId, string buttonId, string argId, string device, string name)
            : this(sourceInterface, deviceId, buttonId, argId, device, name, "0.2", "0.0")
        {
        }

        public IndicatorPushButton(BaseUDPInterface sourceInterface, string deviceId, string buttonId, string argId, string device, string name, string pushValue, string releaseValue)
            : base(sourceInterface)
        {
            _id = argId;
            _format = "%0.1f";

            _pushValue = pushValue;
            _releaseValue = releaseValue;

            _pushActionData = "C" + deviceId + "," + buttonId + "," + pushValue;
            _releaseActionData = "C" + deviceId + "," + buttonId + "," + releaseValue;

            _value = new HeliosValue(sourceInterface, BindingValue.Empty, device, name, "Current state of this button.", "True if the button is currently pushed(either via pressure or toggle), otherwise false.", BindingValueUnits.Boolean);
            Values.Add(_value);
            Triggers.Add(_value);

            _indicatorValue = new HeliosValue(sourceInterface, BindingValue.Empty, device, name + " indicator", "Current state of the indicator lamp on this button.", "True if the indicator is on, otherwise false.", BindingValueUnits.Boolean);
            Values.Add(_indicatorValue);
            Triggers.Add(_indicatorValue);

            _pushedTrigger = new HeliosTrigger(sourceInterface, device, name, "pushed", "Fired when this button is pushed in the simulator.");
            Triggers.Add(_pushedTrigger);
            _releasedTrigger = new HeliosTrigger(sourceInterface, device, name, "released", "Fired when this button is released in the simulator.");
            Triggers.Add(_releasedTrigger);

            _pushAction = new HeliosAction(sourceInterface, device, name, "push", "Pushes this button in the simulator");
            _pushAction.Execute += new HeliosActionHandler(PushAction_Execute);
            Actions.Add(_pushAction);
            _releaseAction = new HeliosAction(sourceInterface, device, name, "release", "Releases the button in the simulator.");
            _releaseAction.Execute += new HeliosActionHandler(ReleaseAction_Execute);
            Actions.Add(_releaseAction);
        }

        void ReleaseAction_Execute(object action, HeliosActionEventArgs e)
        {
            SourceInterface.SendData(_releaseActionData);
        }

        void PushAction_Execute(object action, HeliosActionEventArgs e)
        {
            SourceInterface.SendData(_pushActionData);
        }

        public override void ProcessNetworkData(string id, string value)
        {
            switch (value)
            {
                case "0.0":
                    _value.SetValue(new BindingValue(false), false);
                    _indicatorValue.SetValue(new BindingValue(false), false);
                    break;
                case "0.1":
                    _value.SetValue(new BindingValue(false), false);
                    _indicatorValue.SetValue(new BindingValue(true), false);
                    break;
                case "0.2":
                    _value.SetValue(new BindingValue(true), false);
                    _indicatorValue.SetValue(new BindingValue(false), false);
                    break;
                case "0.3":
                    _value.SetValue(new BindingValue(true), false);
                    _indicatorValue.SetValue(new BindingValue(true), false);
                    break;
            }
        }

        public override ExportDataElement[] GetDataElements()
        {
            return new ExportDataElement[] { new DCSDataElement(_id, _format, true) };
        }

        public override void Reset()
        {
            _value.SetValue(BindingValue.Empty, true);
            _indicatorValue.SetValue(BindingValue.Empty, true);
        }

    }
}

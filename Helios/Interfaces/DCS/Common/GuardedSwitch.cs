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
    using System.Globalization;

    class GuardedSwitch : Switch
    {
        private int _guardPosition = 2;

        private string _guardArgId;
        private string[] _actionData = new string[2];
        private string _guardUpValue;
        private string _guardDownValue;

        private HeliosValue _guardValue;
        private HeliosAction _autoguardPositionAction;

        #region Static Factories

        public static GuardedSwitch CreateToggleSwitch(BaseUDPInterface sourceInterface, string deviceId, string action, string argId,
                                                    string guardAction, string guardArgId, string guardUpValue, string guardDownValue,
                                                    string position1Value, string position1Name,
                                                    string position2Value, string position2Name,
                                                    string device, string name, string exportFormat)
        {
            return new GuardedSwitch(sourceInterface, deviceId, argId, guardAction, guardArgId, guardUpValue, guardDownValue, new SwitchPosition[] { new SwitchPosition(position1Value, position1Name, action), new SwitchPosition(position2Value, position2Name, action) }, device, name, exportFormat);
        }

        public static GuardedSwitch CreateThreeWaySwitch(BaseUDPInterface sourceInterface, string deviceId, string action, string argId,
                                                    string guardAction, string guardArgId, string guardUpValue, string guardDownValue,
                                                    string position1Value, string position1Name,
                                                    string position2Value, string position2Name,
                                                    string position3Value, string position3Name,
                                                    string device, string name, string exportFormat)
        {
            return new GuardedSwitch(sourceInterface, deviceId, argId, guardAction, guardArgId, guardUpValue, guardDownValue,
                    new SwitchPosition[] { new SwitchPosition(position1Value, position1Name, action), new SwitchPosition(position2Value, position2Name, action), new SwitchPosition(position3Value, position3Name, action) },
                    device, name, exportFormat);
        }

        #endregion

        public GuardedSwitch(BaseUDPInterface sourceInterface, string deviceId, string argId, string guardAction, string guardArgId, string guardUpValue, string guardDownValue, SwitchPosition[] positions, string device, string name, string exportFormat)
            : this(sourceInterface, deviceId, argId, guardAction, guardArgId, guardUpValue, guardDownValue, positions, device, name, exportFormat, false)
        {
        }

        public GuardedSwitch(BaseUDPInterface sourceInterface, string deviceId, string argId, string guardAction, string guardArgId, string guardUpValue, string guardDownValue, SwitchPosition[] positions, string device, string name, string exportFormat, bool everyFrame)
            : base(sourceInterface, deviceId, argId, positions, device, name, exportFormat, everyFrame)
        {
            _guardArgId = guardArgId;
            _guardUpValue = guardUpValue;
            _guardDownValue = guardDownValue;

            _actionData[0] = "C" + deviceId + "," + guardAction + "," + guardUpValue;
            _actionData[1] = "C" + deviceId + "," + guardAction + "," + guardDownValue;

            _guardValue = new HeliosValue(sourceInterface, BindingValue.Empty, device, name + " guard", "Current position of the guard for this switch.", "1 = Up, 2 = Down", BindingValueUnits.Numeric);
            _guardValue.Execute += new HeliosActionHandler(GuardValue_Execute);
            Actions.Add(_guardValue);
            Triggers.Add(_guardValue);
            Values.Add(_guardValue);

            _autoguardPositionAction = new HeliosAction(sourceInterface, device, name, "autoguard set", "Sets the position of this switch, and automatically switches the guard up if necessary.", ValueDescriptions, BindingValueUnits.Numeric);
            _autoguardPositionAction.Execute += new HeliosActionHandler(AutoguardPositionAction_Execute);
            Actions.Add(_autoguardPositionAction);
        }

        public int GuardPosition
        {
            get
            {
                return _guardPosition;
            }
            private set
            {
                if (value >= 1 && value <= 2 && value != _guardPosition)
                {
                    switch (value)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                    }

                    _guardPosition = value;
                    _guardValue.SetValue(new BindingValue(_guardPosition.ToString(CultureInfo.InvariantCulture)), SourceInterface.BypassTriggers);
                }
            }
        }

        void AutoguardPositionAction_Execute(object action, HeliosActionEventArgs e)
        {
            if (GuardPosition == 2)
            {
                SourceInterface.BeginTriggerBypass(e.BypassCascadingTriggers);
                GuardPosition = 1;
                SourceInterface.SendData(_actionData[GuardPosition - 1]);
                SourceInterface.EndTriggerBypass(e.BypassCascadingTriggers);
            }
        }

        protected void GuardValue_Execute(object action, HeliosActionEventArgs e)
        {
            SourceInterface.BeginTriggerBypass(e.BypassCascadingTriggers);
            GuardPosition = (int)e.Value.DoubleValue;
            SourceInterface.SendData(_actionData[GuardPosition - 1]);
            SourceInterface.EndTriggerBypass(e.BypassCascadingTriggers);
        }

        public override void ProcessNetworkData(string id, string value)
        {
            if (_guardArgId.Equals(id))
            {
                int newGuardPosition = _guardPosition;
                if (_guardUpValue.Equals(value))
                {
                    newGuardPosition = 1;
                }
                else if (_guardDownValue.Equals(value))
                {
                    newGuardPosition = 2;
                }

                GuardPosition = newGuardPosition;
            }
            else
            {
                base.ProcessNetworkData(id, value);
            }
        }

        public override ExportDataElement[] GetDataElements()
        {
            return new ExportDataElement[] { new DCSDataElement(_id, _format, _everyframe), new DCSDataElement(_guardArgId, _format, _everyframe)  };
        }
    }
}

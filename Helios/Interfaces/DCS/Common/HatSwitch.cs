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
    using System.Collections.Generic;
    using System.Globalization;

    class HatSwitch : NetworkFunction
    {
        public enum HatPosition : int
        {
            Center,
            Up,
            Down,
            Left,
            Right
        }

        protected readonly string _id;
        protected readonly string _format;
        protected readonly bool _everyframe;

        private HatPosition _currentPosition = HatPosition.Center;

        private HeliosValue _positionValue;

        private Dictionary<string, HatPosition> _positionByValue = new Dictionary<string, HatPosition>();
        private Dictionary<HatPosition, string> _sendData = new Dictionary<HatPosition, string>();

        public HatSwitch(BaseUDPInterface sourceInterface, string deviceId, string argId,
                    string leftAction, string leftValue,
                    string upAction, string upValue,
                    string rightAction, string rightValue,
                    string downAction, string downValue,
                    string releaseAction, string releaseValue,
                    string device, string name, string exportFormat)
            : this(sourceInterface, deviceId, argId, leftAction, leftValue, upAction, upValue, rightAction, rightValue, downAction, downValue, releaseAction, releaseValue, device, name, exportFormat, false)
        {
        }

        public HatSwitch(BaseUDPInterface sourceInterface, string deviceId, string argId, 
                            string leftAction, string leftValue, 
                            string upAction, string upValue, 
                            string rightAction, string rightValue, 
                            string downAction, string downValue, 
                            string releaseAction, string releaseValue,
                            string device, string name, string exportFormat, bool everyFrame)
            : base(sourceInterface)
        {
            _id = argId;
            _format = exportFormat;
            _everyframe = everyFrame;

            _sendData.Add(HatPosition.Center, "C" + deviceId + "," + releaseAction + "," + releaseValue);
            _sendData.Add(HatPosition.Left, "C" + deviceId + "," + leftAction + "," + leftValue);
            _sendData.Add(HatPosition.Up, "C" + deviceId + "," + upAction + "," + upValue);
            _sendData.Add(HatPosition.Right, "C" + deviceId + "," + rightAction + "," + rightValue);
            _sendData.Add(HatPosition.Down, "C" + deviceId + "," + downAction + "," + downValue);

            _positionByValue.Add(releaseValue, HatPosition.Center);
            _positionByValue.Add(leftValue, HatPosition.Left);
            _positionByValue.Add(upValue, HatPosition.Up);
            _positionByValue.Add(rightValue, HatPosition.Right);
            _positionByValue.Add(downValue, HatPosition.Down);            

            _positionValue = new HeliosValue(sourceInterface, new BindingValue((double)_currentPosition), device, name, "Current position of the hat switch.", "Position 0 = center, 1 = up, 2 = down, 3 = left,  or 4 = right.", BindingValueUnits.Numeric);
            _positionValue.Execute += new HeliosActionHandler(SetPositionAction_Execute);
            Values.Add(_positionValue);
            Actions.Add(_positionValue);
            Triggers.Add(_positionValue);
        }

        public HatPosition SwitchPosition
        {
            get
            {
                return _currentPosition;
            }
            set
            {
                if (!_currentPosition.Equals(value))
                {
                    _currentPosition = value;
                    _positionValue.SetValue(new BindingValue(((int)_currentPosition).ToString(CultureInfo.InvariantCulture)), SourceInterface.BypassTriggers);
                }
            }
        }

        void SetPositionAction_Execute(object action, HeliosActionEventArgs e)
        {
            SourceInterface.BeginTriggerBypass(e.BypassCascadingTriggers);
            try
            {
                SwitchPosition = (HatPosition)Enum.Parse(typeof(HatPosition), e.Value.StringValue);
                SourceInterface.SendData(_sendData[SwitchPosition]);
            }
            catch
            {
                // No-op if the parse fails we won't set the position.
            }
            SourceInterface.EndTriggerBypass(e.BypassCascadingTriggers);
        }

        public override void ProcessNetworkData(string id, string value)
        {
            if (_id.Equals(id) && _positionByValue.ContainsKey(value))
            {
                SwitchPosition = _positionByValue[value];
            }
        }

        public override ExportDataElement[] GetDataElements()
        {
            return new ExportDataElement[] { new DCSDataElement(_id, _format, _everyframe) };
        }

        public override void Reset()
        {
            SourceInterface.BeginTriggerBypass(true);
            SwitchPosition = HatPosition.Center;
            SourceInterface.EndTriggerBypass(true);
        }
    }
}

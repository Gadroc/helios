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

    class LockedEncoder : NetworkFunction
    {
        private string _incrementData;
        private string _decrementData;

        private string _incrementPrefix;
        private string _decrementPrefix;

        private HeliosAction _incrementAction;
        private HeliosAction _decrementAction;

        public LockedEncoder(BaseUDPInterface sourceInterface, string deviceId, string buttonId, string button2Id, string argId, double argValue, string device, string name)
            : base(sourceInterface)
        {
            _incrementPrefix = "C" + deviceId + "," + buttonId + ",";
            _decrementPrefix = "C" + deviceId + "," + button2Id + ",";
            _incrementData = _incrementPrefix + argValue.ToString(CultureInfo.InvariantCulture);
            _decrementData = _decrementPrefix + (-argValue).ToString(CultureInfo.InvariantCulture);

            _incrementAction = new HeliosAction(sourceInterface, device, name, "increment", "Increments this setting.");
            _incrementAction.Execute += new HeliosActionHandler(IncrementAction_Execute);
            Actions.Add(_incrementAction);

            _decrementAction = new HeliosAction(sourceInterface, device, name, "decrement", "Decrement this setting.");
            _decrementAction.Execute += new HeliosActionHandler(DecrementAction_Execute);
            Actions.Add(_decrementAction);
        }

        void DecrementAction_Execute(object action, HeliosActionEventArgs e)
        {
            SourceInterface.SendData(_decrementData);
        }

        void IncrementAction_Execute(object action, HeliosActionEventArgs e)
        {
            SourceInterface.SendData(_incrementData);
        }

        public override void ProcessNetworkData(string id, string value)
        {
            // No-Op
        }

        public override ExportDataElement[] GetDataElements()
        {
            return new ExportDataElement[0];
        }

        public override void Reset()
        {
            // No-Op
        }
    }
}

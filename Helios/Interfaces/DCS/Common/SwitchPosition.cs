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
    public class SwitchPosition
    {
        private string _argValue;
        private string _name;

        private string _action;
        private string _stopAction;
        private string _stopActionValue;
        private string _exitValue;

        public SwitchPosition(string argValue, string name, string action)
            : this(argValue, name, action, null, null)
        {
        }

        public SwitchPosition(string argValue, string name, string action, string stopAction, string stopActionValue)
            : this(argValue, name, action, stopAction, stopActionValue, null)
        {
        }

        public SwitchPosition(string argValue, string name, string action, string stopAction, string stopActionValue, string exitValue)
        {
            _argValue = argValue;
            _name = name;
            _action = action;
            _stopAction = stopAction;
            _stopActionValue = stopActionValue;
            _exitValue = exitValue;
        }

        #region Properties

        public string ExitValue
        {
            get { return _exitValue; }
        }

        public string ArgValue 
        {
            get { return _argValue; }
        }

        public string Name 
        {
            get { return _name; }
        }

        public string Action
        {
            get { return _action; }
        }

        public string StopAction
        {
            get { return _stopAction; }
        }

        public string StopActionValue
        {
            get { return _stopActionValue; }
        }

        #endregion

        public override string ToString()
        {            
            return ArgValue + "=" + Name;
        }
    }
}

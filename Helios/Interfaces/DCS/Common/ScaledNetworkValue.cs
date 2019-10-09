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

    public class ScaledNetworkValue : NetworkFunction
    {
        private string _id;
        private string _format;

        private double _baseValue;

        private CalibrationPointCollectionDouble _calibratedScale;
        private double _scale;

        private HeliosValue _value;

        public ScaledNetworkValue(BaseUDPInterface sourceInterface, string id, CalibrationPointCollectionDouble scale, string device, string name, string description, string valueDescription, BindingValueUnit unit)
            : this(sourceInterface, id, scale, device, name, description, valueDescription, unit, "%.4f")
        {
        }

        public ScaledNetworkValue(BaseUDPInterface sourceInterface, string id, CalibrationPointCollectionDouble scale, string device, string name, string description, string valueDescription, BindingValueUnit unit, string exportFormat)
            : this(sourceInterface, id, device, name, description, valueDescription, unit, 0d, exportFormat)
        {
            _calibratedScale = scale;
            _scale = 0d;
        }

        public ScaledNetworkValue(BaseUDPInterface sourceInterface, string id, double scale, string device, string name, string description, string valueDescription, BindingValueUnit unit)
            : this(sourceInterface, id, scale, device, name, description, valueDescription, unit, 0d, "%.4f")
        {
        }

        public ScaledNetworkValue(BaseUDPInterface sourceInterface, string id, double scale, string device, string name, string description, string valueDescription, BindingValueUnit unit, double baseValue, string exportFormat)
            : this(sourceInterface, id, device, name, description, valueDescription, unit, baseValue, exportFormat)
        {
            _calibratedScale = null;
            _scale = scale;
        }

        private ScaledNetworkValue(BaseUDPInterface sourceInterface, string id, string device, string name, string description, string valueDescription, BindingValueUnit unit, double baseValue, string exportFormat)
            : base(sourceInterface)
        {
            _id = id;
            _format = exportFormat;
            _baseValue = baseValue;
            _value = new HeliosValue(sourceInterface, BindingValue.Empty, device, name, description, valueDescription, unit);
            Values.Add(_value);
            Triggers.Add(_value);
        }


        public override void ProcessNetworkData(string id, string value)
        {
            double scaledValue;
            if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out scaledValue))
            {
                if (_calibratedScale != null)
                {
                    _value.SetValue(new BindingValue(_calibratedScale.Interpolate(scaledValue)), false);
                }
                else
                {
                    _value.SetValue(new BindingValue((scaledValue * _scale) + _baseValue), false);
                }
            }
        }

        public override ExportDataElement[] GetDataElements()
        {
            return new ExportDataElement[] { new DCSDataElement(_id, _format, true) };
        }

        public override void Reset()
        {
            _value.SetValue(BindingValue.Empty, true);
        }

    }
}

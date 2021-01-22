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

    public class DualNetworkValue : NetworkFunction
    {
        private string _id;
        private string _format, _pure_name;

        private double _baseValue;

        private CalibrationPointCollectionDouble _calibratedScale;
        private double _scale;

        private HeliosValue _value, _pure_value;

        public DualNetworkValue(BaseUDPInterface sourceInterface, string id, CalibrationPointCollectionDouble scale, string device, string name, string description, string valueDescription, BindingValueUnit unit)
            : this(sourceInterface, id, scale, device, name, description, valueDescription, unit, "%.4f")
        {
        }

        public DualNetworkValue(BaseUDPInterface sourceInterface, string id, CalibrationPointCollectionDouble scale, string device, string name, string description, string valueDescription, BindingValueUnit unit, string exportFormat)
            : this(sourceInterface, id, device, name, description, valueDescription, unit, 0d, exportFormat)
        {
            _calibratedScale = scale;
            _scale = 0d;
        }

        public DualNetworkValue(BaseUDPInterface sourceInterface, string id, double scale, string device, string name, string description, string valueDescription, BindingValueUnit unit)
            : this(sourceInterface, id, scale, device, name, description, valueDescription, unit, 0d, "%.4f")
        {
        }

        public DualNetworkValue(BaseUDPInterface sourceInterface, string id, double scale, string device, string name, string description, string valueDescription, BindingValueUnit unit, double baseValue, string exportFormat)
            : this(sourceInterface, id, device, name, description, valueDescription, unit, baseValue, exportFormat)
        {
            _calibratedScale = null;
            _scale = scale;
        }

        private DualNetworkValue(BaseUDPInterface sourceInterface, string id, string device, string name, string description, string valueDescription, BindingValueUnit unit, double baseValue, string exportFormat)
            : base(sourceInterface)
        {
			_pure_name = "Pure " + name;
            _id = id;
            _format = exportFormat;
            _baseValue = baseValue;
            _value = new HeliosValue(sourceInterface, BindingValue.Empty, device, name, description, valueDescription, unit);
			_pure_value = new HeliosValue(sourceInterface, BindingValue.Empty, device, _pure_name, "Pure value in DCS", "pure value", unit);
			Values.Add(_value);
            Triggers.Add(_value);
			Values.Add(_pure_value);
			Triggers.Add(_pure_value);
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

				_pure_value.SetValue(new BindingValue(scaledValue), false);
			}
        }

        public override ExportDataElement[] GetDataElements()
        {
            return new ExportDataElement[] { new DCSDataElement(_id, _format, true) };
        }

        public override void Reset()
        {
            _value.SetValue(BindingValue.Empty, true);
			_pure_value.SetValue(BindingValue.Empty, true);
		}

    }
}

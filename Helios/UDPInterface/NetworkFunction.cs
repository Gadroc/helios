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

namespace GadrocsWorkshop.Helios.UDPInterface
{
    public abstract class NetworkFunction
    {
        private bool _debug;
        private BaseUDPInterface _sourceInterface;
        private HeliosTriggerCollection _triggers = new HeliosTriggerCollection();
        private HeliosActionCollection _actions = new HeliosActionCollection();
        private HeliosValueCollection _values = new HeliosValueCollection();

        protected NetworkFunction(BaseUDPInterface sourceInterface)
        {
            _sourceInterface = sourceInterface;
        }

        public BaseUDPInterface SourceInterface
        {
            get
            {
                return _sourceInterface;
            }
        }

        public HeliosTriggerCollection Triggers
        {
            get
            {
                return _triggers;
            }
        }

        public HeliosActionCollection Actions
        {
            get
            {
                return _actions;
            }
        }

        public HeliosValueCollection Values
        {
            get
            {
                return _values;
            }
        }

        public bool IsDebugMode
        {
            get
            {
                return _debug;
            }
            set
            {
                _debug = value;
            }
        }

        public abstract void Reset();

        public abstract ExportDataElement[] GetDataElements();

        public abstract void ProcessNetworkData(string id, string value);
    }
}

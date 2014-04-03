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

namespace GadrocsWorkshop.Helios
{
    using System.Windows.Threading;

    public class BaseDeserializer
    {
        private delegate object CreateObjectDelegate(string type, string typeId);
        private CreateObjectDelegate _objectCreator;
        private Dispatcher _dispatcher;

        public BaseDeserializer(Dispatcher dispatcher)
        {
            _objectCreator = new CreateObjectDelegate(DispCreateNewObject);
            _dispatcher = dispatcher;
        }

        protected Dispatcher Dispatcher
        { get { return _dispatcher; } }

        #region Object Creation Methods

        protected object CreateNewObject(string type, string typeId)
        {
            return Dispatcher.Invoke(_objectCreator, type, typeId);
        }

        private object DispCreateNewObject(string type, string typeId)
        {
            switch (type)
            {
                case "Monitor":
                    return new Monitor();

                case "Visual":
                    HeliosVisual visual = ConfigManager.ModuleManager.CreateControl(typeId);
                    visual.Dispatcher = _dispatcher;
                    return visual;

                case "Interface":
                    HeliosInterfaceDescriptor descriptor = ConfigManager.ModuleManager.InterfaceDescriptors[typeId];
                    HeliosInterface heliosInterface = descriptor != null ? descriptor.CreateInstance() : null;
                    heliosInterface.Dispatcher = _dispatcher;
                    return heliosInterface;

                case "Binding":
                    return new HeliosBinding();

            }
            return null;
        }

        #endregion
    }
}

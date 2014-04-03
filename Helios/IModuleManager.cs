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
    using GadrocsWorkshop.Helios.Windows.Controls;

    public interface IModuleManager
    {
        #region Properties

        HeliosDescriptorCollection ControlDescriptors { get; }
        HeliosInterfaceDescriptorCollection InterfaceDescriptors { get; }

        #endregion

        HeliosVisual CreateControl(string typeIdentifier);
        HeliosInterfaceEditor CreateInterfaceEditor(HeliosInterface item, HeliosProfile profile);
        HeliosVisualRenderer CreaterRenderer(HeliosVisual visual);
        HeliosPropertyEditorDescriptorCollection GetPropertyEditors(string typeIdentifier);

        BindingValueUnitConverter GetUnitConverter(BindingValueUnit from, BindingValueUnit to);
        bool CanConvertUnit(BindingValueUnit from, BindingValueUnit to);
        BindingValue ConvertUnit(BindingValue value, BindingValueUnit from, BindingValueUnit to);
    }
}

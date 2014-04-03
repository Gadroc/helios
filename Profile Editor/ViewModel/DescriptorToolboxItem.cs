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

namespace GadrocsWorkshop.Helios.ProfileEditor.ViewModel
{
    class DescriptorToolboxItem : ToolboxItem
    {
        private HeliosDescriptor _descriptor;

        public DescriptorToolboxItem(HeliosDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public override bool IsRemoveable
        {
            get { return false; }
        }

        public override string Name
        {
            get { return _descriptor.Name; }
        }

        public override string Category
        {
            get { return _descriptor.Category; }
        }

        public override HeliosVisual CreateControl()
        {
            return ConfigManager.ModuleManager.CreateControl(_descriptor.TypeIdentifier);
        }

        public override void ConfigureIcon(HeliosVisual control)
        {
            control.ConfigureIconInstance();
        }
    }
}

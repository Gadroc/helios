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
    class TemplateToolboxItem : ToolboxItem
    {
        private HeliosTemplate _template;

        public TemplateToolboxItem(HeliosTemplate template)
        {
            _template = template;
        }

        public override bool IsRemoveable
        {
            get { return _template.IsUserTemplate; }
        }

        public HeliosTemplate Template
        {
            get { return _template; }
        }

        public override string Name
        {
            get { return _template.Name; }
        }

        public override string Category
        {
            get { return _template.Category; }
        }

        public override HeliosVisual CreateControl()
        {
            return _template.CreateInstance();
        }

        public override void ConfigureIcon(HeliosVisual control)
        {
            // No-Op
        }
    }
}

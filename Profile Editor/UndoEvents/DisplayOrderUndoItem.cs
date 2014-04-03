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

namespace GadrocsWorkshop.Helios.ProfileEditor.UndoEvents
{
    class DisplayOrderUndoItem : IUndoItem
    {
        private bool _up;
        private HeliosVisualContainer _container;
        private HeliosVisual _control;

        public DisplayOrderUndoItem(HeliosVisualContainer container, HeliosVisual control, bool up)
        {
            _container = container;
            _control = control;
            _up = up;
        }

        public void Undo()
        {
            if (_up)
            {
                _container.Children.MoveDown(_control);
            }
            else
            {
                _container.Children.MoveUp(_control);
            }
        }

        public void Do()
        {
            if (_up)
            {
                _container.Children.MoveUp(_control);
            }
            else
            {
                _container.Children.MoveDown(_control);
            }
        }
    }
}

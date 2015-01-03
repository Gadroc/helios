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

namespace GadrocsWorkshop.Helios.Interfaces.Eos
{
    using System;

    public class EosOutput : NotificationObject
    {
        private string _name;
        private HeliosActionCollection _actions = new HeliosActionCollection();
        private WeakReference _board = new WeakReference(null);

        public EosOutput(EosBoard board)
        {
            _board = new WeakReference(board);
        }

        #region Properties

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != null && !value.Equals(_name))
                {
                    string oldValue = value;
                    _name = value;
                    OnPropertyChanged("Name", oldValue, value, true);

                    foreach (IBindingAction action in _actions)
                    {
                        action.Device = _name;
                    }
                }
            }
        }

        public HeliosActionCollection Actions
        {
            get { return _actions; }
        }

        protected EosBoard Board
        {
            get { return (EosBoard)_board.Target; }
        }

        #endregion
    }
}

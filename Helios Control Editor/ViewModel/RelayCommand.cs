//  Copyright 2013 Craig Courtney
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

namespace GadrocsWorkshop.Helios.ControlEditor.ViewModel
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;

    public class RelayCommand : ICommand 
    { 
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields 

        #region Constructors 

        public RelayCommand(Action<object> execute) : this(execute, null) 
        { 
        } 
        
        public RelayCommand(Action<object> execute, Predicate<object> canExecute) 
        { 
            if (execute == null) throw new ArgumentNullException("execute");
            _execute = execute; _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough] 
        public bool CanExecute(object parameter) 
        {
            return _canExecute == null ? true : _canExecute(parameter); 
        }
        
        public event EventHandler CanExecuteChanged 
        {
            add { CommandManager.RequerySuggested += value; } 
            remove { CommandManager.RequerySuggested -= value; } 
        }
        
        public void Execute(object parameter)
        {
            _execute(parameter);
        } 
        #endregion // ICommand Members 
    }
}

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
    using System;

    public interface IBindingAction : IBindingElement
    {
        /// <summary>
        /// Unique ID for this action.
        /// </summary>
        string ActionID { get; }

        /// <summary>
        /// Display name for this action;
        /// </summary>
        string ActionName { get; }

        /// <summary>
        /// Verb used to describe this action.
        /// </summary>
        string ActionVerb { get; }

        /// <summary>
        /// Target object which this action acts on.
        /// </summary>
        HeliosObject Target { get; }

        /// <summary>
        /// Short description of what this action does.
        /// </summary>
        string ActionDescription { get; }

        /// <summary>
        /// Description of the valid values that this action accepts.
        /// </summary>
        string ActionValueDescription { get; }

        /// <summary>
        /// Returns true if this action requires an input value.
        /// </summary>
        bool ActionRequiresValue { get; }

        /// <summary>
        /// Description to be displayed in binding use %value% to subsititue the appropriate
        /// value from the binding configuration.
        /// </summary>
        string ActionBindingDescription { get; }

        /// <summary>
        /// Description to be displayed in binding when context of action is known use %value% to subsititue the appropriate
        /// value from the binding configuration.
        /// </summary>
        string ActionInputBindingDescription { get; }

        /// <summary>
        /// Returns user interface control to be used for editing static values for
        /// to this action.
        /// </summary>
        Type ValueEditorType { get; }

        /// <summary>
        /// Executes this action.
        /// </summary>
        /// <param name="value">Value to be processed by this action.</param>
        /// <param name="bypassCascadingTriggers">If true this action will not fire additional triggers.</param>
        void ExecuteAction(BindingValue value, bool bypassCascadingTriggers);
    }
}

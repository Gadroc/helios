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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="trigger">Sending trigger object.</param>
    /// <param name="e"></param>
    public delegate void HeliosTriggerHandler(object trigger, HeliosTriggerEventArgs e);

    public interface IBindingTrigger : IBindingElement
    {
        /// <summary>
        /// Event which is fired when ever the trigger is activated.
        /// </summary>
        event HeliosTriggerHandler TriggerFired;

        /// <summary>
        /// Unique ID for this trigger.
        /// </summary>
        string TriggerID { get; }

        /// <summary>
        /// Display name for this trigger.
        /// </summary>
        string TriggerName { get; }

        /// <summary>
        /// Verb used to describe the trigger.
        /// </summary>
        string TriggerVerb { get; }

        /// <summary>
        /// Gets the description of when this trigger is fired.
        /// </summary>
        string TriggerDescription { get; }

        /// <summary>
        /// Gets the description of the contents of the supplied value when this trigger is fired.
        /// </summary>
        string TriggerValueDescription { get; }

        /// <summary>
        /// Description to be displayed in binding use %value% to subsititue the appropriate
        /// value from the binding configuration.
        /// </summary>
        string TriggerBindingDescription { get; }

        /// <summary>
        /// Returns true if this trigger supplies a value.
        /// </summary>
        bool TriggerSuppliesValue { get; }

        /// <summary>
        /// Source object which fires this trigger.
        /// </summary>
        HeliosObject Source { get; }
    }
}

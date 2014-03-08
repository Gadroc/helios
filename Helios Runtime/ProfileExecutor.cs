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

using System;
using System.Collections.Concurrent;

using GadrocsWorkshop.Helios.Binding;

namespace GadrocsWorkshop.Helios.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class ProfileExecutor
    {

        public ProfileExecutor(Profile profile)
        {
            // TODO: Create blocking event queue for incoming triggers            
        }

        /// <summary>
        /// Adds a trigger event to the event processing queue.  This method is thread safe.
        /// </summary>
        /// <param name="e">TriggerEvent which needs to be processed.</param>
        public void TriggerFired(TriggerEvent e)
        {
            // TODO: Add trigger event to the queue
        }

        /// <summary>
        /// Starts this profile
        /// </summary>
        public void Start()
        {
            // TODO: Create thread for executing events
            // TODO: Construct and register for trigger events on all devices and displays
            // TODO: 
        }

        public void Stop()
        {
            // TODO: Deconstruct devices and deisplasy used in the profile.
        }
    }
}

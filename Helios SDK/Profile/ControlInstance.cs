using System;
using System.Collections.Generic;

using GadrocsWorkshop.Helios.ComponentModel;

namespace GadrocsWorkshop.Helios.Profile
{
    public class ControlInstance : PropertyObject
    {
        private IControl _control;
        private Dictionary<string, IVisualState> _states;

        /// <summary>
        /// Creates a new instance for a given control.
        /// </summary>
        /// <param name="control"></param>
        public ControlInstance(IControl control)
        {
            _control = control;
            _states = new Dictionary<string,IVisualState>();
        }

        /// <summary>
        /// Returns control which this instance displays.
        /// </summary>
        public IControl Control
        {
            get
            {
                return _control;
            }
        }

        /// <summary>
        /// Returns a dictionary of visual states for this instance.
        /// </summary>
        public Dictionary<string, IVisualState> States
        {
            get
            {
                return _states;
            }
        }

    }
}

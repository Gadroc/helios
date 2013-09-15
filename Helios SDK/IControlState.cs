using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadrocsWorkshop.Helios
{
    /// <summary>
    /// Contains state data for an invididual instance of a control.
    /// </summary>
    public interface IControlState
    {
        /// <summary>
        /// Top of the Control.
        /// </summary>
        int Top { get; }

        /// <summary>
        /// Left of the Control.
        /// </summary>
        int Left { get; }

        /// <summary>
        /// Width of this Control.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Height of this Control.
        /// </summary>
        int Height { get; }
    }
}

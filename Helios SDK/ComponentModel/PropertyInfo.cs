using System;
using System.ComponentModel;

namespace GadrocsWorkshop.Helios.ComponentModel
{
    /// <summary>
    /// Meta data flags for special actions which should be taken on property changes.
    /// </summary>
    [Flags]
    public enum PropertyInfo
    {
        [Description("Changes to this property are recorded in the undo buffer.")]
        Undoable,
        [Description("When this property changes it forces a clear of the undo buffer.")]
        UndoClear
    }
}

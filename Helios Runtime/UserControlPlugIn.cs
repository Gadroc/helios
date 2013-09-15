using System;
using System.Linq;

using GadrocsWorkshop.Helios;
using System.ComponentModel.Composition;

namespace GadrocsWorkshop.Helios.Runtime
{
    [Export(typeof(IPlugIn))]
    [ExportMetadata("Id", "GadrocsWorkshop.UserControls")]
    [ExportMetadata("Name", "User Control")]
    [ExportMetadata("Description", "Allows the creation and use of user defined controls. Controls are created and edited with the Helios Control Editor.")]
    public class UserControlPlugIn : IPlugIn
    {
        public System.Collections.Generic.IEnumerable<IDisplay> GetDisplays()
        {
            return Enumerable.Empty<IDisplay>();
        }

        public System.Collections.Generic.IEnumerable<IDevice> GetDevices()
        {
            return Enumerable.Empty<IDevice>();
        }

        public System.Collections.Generic.IEnumerable<IControlType> GetControlTypes()
        {
            return Enumerable.Empty<IControlType>();
        }

        public IControl CreateControl(string typeId)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            // TODO: Loop through existing controls in the system and catalog them
        }

        public void Destroy()
        {
            // No-Op
        }
    }
}

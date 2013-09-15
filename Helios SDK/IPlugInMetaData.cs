using System;

namespace GadrocsWorkshop.Helios
{
    public interface IPlugInMetaData
    {
        string Id { get; }
        string Name { get; }
        string Description { get; }
    }
}

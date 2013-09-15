using System;

namespace GadrocsWorkshop.Helios.Runtime
{
    /// <summary>
    /// Meta-data class for user controls.
    /// </summary>
    public class UserControlType : IControlType
    {
        public string Id
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsRemoteable
        {
            get
            {
                return true;
            }
        }
    }
}

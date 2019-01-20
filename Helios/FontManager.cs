using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Drawing.Text;
using System.Drawing;

/// <summary>
/// Singleton class to allow for dynamic loading of fonts
/// </summary>

namespace GadrocsWorkshop.Helios
{
    public class FontManager
    {
        private static FontManager _instance = null;
        private static readonly object _padlock = new object();
        private PrivateFontCollection _privateFontCollection = new PrivateFontCollection();

        FontManager()
        {
            AddFont("SFDigitalReadout-Medium.ttf");
        }

        public static FontManager Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new FontManager();
                    }
                    return _instance;
                }
            }
        }

        public FontFamily GetFontFamilyByName(string name)
        {
            return _privateFontCollection.Families.FirstOrDefault(x => x.Name == name);
        }

        public void AddFont(string fullFileName)
        {
            AddFont(File.ReadAllBytes(fullFileName));
        }

        public void AddFont(byte[] fontBytes)
        {
            var handle = GCHandle.Alloc(fontBytes, GCHandleType.Pinned);
            IntPtr pointer = handle.AddrOfPinnedObject();
            try
            {
                _privateFontCollection.AddMemoryFont(pointer, fontBytes.Length);
            }
            finally
            {
                handle.Free();
            }
        }

    }
}

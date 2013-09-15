using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

using GadrocsWorkshop.Helios;

namespace GadrocsWorkshop.Helios.Runtime
{
    /// <summary>
    /// Runtime which manages all plug-ins and configuration.
    /// </summary>
    public class HeliosRuntime
    {
        private CompositionContainer _container;

        [ImportMany]
        private IEnumerable<Lazy<IPlugIn, IPlugInMetaData>> _plugins;

        private ISet<IPlugIn> _activePlugins;
        private IList<IDisplay> _displays;

        public HeliosRuntime()
        {
            _activePlugins = new HashSet<IPlugIn>();

            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(HeliosRuntime).Assembly));
            // TODO:: Should we iterate sub-directories?
            catalog.Catalogs.Add(new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PlugIns")));

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (Exception f in e.LoaderExceptions)
                {
                    Console.WriteLine(f.ToString());
                }
                Console.WriteLine(e.ToString());
            }

            _displays = new List<IDisplay>();
        }

        public IEnumerable<Lazy<IPlugIn, IPlugInMetaData>> PlugIns
        {
            get
            {
                return _plugins;
            }
        }

        /// <summary>
        /// Initializes the runtime plug-ins so they are ready for use.
        /// </summary>
        public void InitializeAll()
        {
            foreach (Lazy<IPlugIn, IPlugInMetaData> i in _plugins)
            {
                Initialize(i.Value);
            }
        }

        /// <summary>
        /// Initializes a plug-in by it's unique id.
        /// </summary>
        /// <param name="plugInId">Id of the plugin which should be initialized.</param>
        public void Initialize(string plugInId)
        {
            foreach (Lazy<IPlugIn, IPlugInMetaData> i in _plugins)
            {
                if (i.Metadata.Id.Equals(plugInId))
                {
                    Initialize(i.Value);
                    return;
                }
            }
            throw new ArgumentException("PlugIn (" + plugInId + ") not found", "plugInId");
        }

        /// <summary>
        /// Initializes a given plug-in
        /// </summary>
        /// <param name="plugin">PlugIn which should be initialized</param>
        public void Initialize(IPlugIn plugin)
        {
            if (!_activePlugins.Contains(plugin))
            {
                plugin.Initialize();

                // Iterate displays and add them to our collection
                foreach (IDisplay display in plugin.GetDisplays())
                {
                    _displays.Add(display);
                }

                _activePlugins.Add(plugin);
            }
        }

        /// <summary>
        /// Cleans up the runtime and all plug-ins.
        /// </summary>
        public void Shutdown()
        {
            foreach (IPlugIn plugin in _activePlugins)
            {
                plugin.Destroy();
            }
            _activePlugins.Clear();
        }
    }
}

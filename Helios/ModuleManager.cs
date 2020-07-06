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
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// ModuleManager gives access to all plugin component (Controls, Interfaces, Converters and Property Editors).
    /// </summary>
    internal class ModuleManager : IModuleManager
    {
        private string _applicationPath;
        private byte[] _publicKey;

        private List<BindingValueUnitConverter> _converters = new List<BindingValueUnitConverter>();
        private HeliosDescriptorCollection _controlDescriptors = new HeliosDescriptorCollection();
        private HeliosInterfaceDescriptorCollection _interfaceDescriptors = new HeliosInterfaceDescriptorCollection();

        private Dictionary<string, HeliosPropertyEditorDescriptorCollection> _propertyEditors = new Dictionary<string, HeliosPropertyEditorDescriptorCollection>();

        internal ModuleManager(string applicationPath)
        {
            ConfigManager.LogManager.Log("ModuleManager Intialisation: " + applicationPath);
            _applicationPath = applicationPath;

            Assembly appAssembly = Assembly.GetEntryAssembly();
            AssemblyName appName = appAssembly.GetName();
            _publicKey = appName.GetPublicKey();
        }

        public HeliosDescriptorCollection ControlDescriptors
        {
            get
            {
                return _controlDescriptors;
            }
        }

        public HeliosInterfaceDescriptorCollection InterfaceDescriptors
        {
            get
            {
                return _interfaceDescriptors;
            }
        }

        public HeliosVisual CreateControl(string typeIdentifier)
        {
            HeliosVisual control = null;
            HeliosDescriptor descriptor = _controlDescriptors[typeIdentifier];
            if (descriptor != null)
            {
                control = (HeliosVisual)Activator.CreateInstance(descriptor.ControlType);
            }
            return control;
        }

        public HeliosInterfaceEditor CreateInterfaceEditor(HeliosInterface item, HeliosProfile profile)
        {
            HeliosInterfaceEditor editor = null;
            if (item != null)
            {
                HeliosInterfaceDescriptor descriptor = _interfaceDescriptors[item.GetType()];
                if (descriptor != null && descriptor.InterfaceEditorType != null)
                {
                    editor = (HeliosInterfaceEditor)Activator.CreateInstance(descriptor.InterfaceEditorType);
                    editor.Interface = item;
                    editor.Profile = profile;
                }
            }
            return editor;
        }

        public HeliosVisualRenderer CreaterRenderer(HeliosVisual visual)
        {
            HeliosVisualRenderer renderer = null;
            Type visualType = visual.GetType();

            HeliosDescriptor descriptor = _controlDescriptors[visualType];
            if (descriptor != null)
            {
                renderer = (HeliosVisualRenderer)Activator.CreateInstance(descriptor.Renderer);
                renderer.Visual = visual;
            }

            return renderer;
        }

        public HeliosPropertyEditorDescriptorCollection GetPropertyEditors(string typeIdentifier)
        {
            if (_propertyEditors.ContainsKey(typeIdentifier))
            {
                return _propertyEditors[typeIdentifier];
            }

            return new HeliosPropertyEditorDescriptorCollection();
        }

        public BindingValueUnitConverter GetUnitConverter(BindingValueUnit from, BindingValueUnit to)
        {
            foreach (BindingValueUnitConverter converter in _converters)
            {
                if (converter.CanConvert(from, to))
                {
                    return converter;
                }
            }
            return null;
        }

        public bool CanConvertUnit(BindingValueUnit from, BindingValueUnit to)
        {
            if (from.Equals(to)) return true;

            return (GetUnitConverter(from, to) != null);
        }

        public BindingValue ConvertUnit(BindingValue value, BindingValueUnit from, BindingValueUnit to)
        {
            BindingValueUnitConverter converter = GetUnitConverter(from, to);
            if (converter != null)
            {
                return converter.Convert(value, from, to);
            }
            return BindingValue.Empty;
        }

        internal void RegisterModule(Assembly asm)
        {
            if (asm != null)
            {
                try
                {
                    string moduleName = asm.GetName().Name;

                    foreach (Type type in asm.GetTypes())
                    {
                        if (type.IsAbstract) continue;
                        object[] attrs = type.GetCustomAttributes(false);
                        foreach (object attribute in attrs)
                        {
                            HeliosControlAttribute controlAttribute = attribute as HeliosControlAttribute;
                            if (controlAttribute != null)
                            {
                                ConfigManager.LogManager.LogDebug("Control found " + type.Name);
                                _controlDescriptors.Add(new HeliosDescriptor(type, controlAttribute));
                            }

                            HeliosInterfaceAttribute interfaceAttribute = attribute as HeliosInterfaceAttribute;
                            if (interfaceAttribute != null)
                            {
                                ConfigManager.LogManager.LogDebug("Interface found " + type.Name);
                                _interfaceDescriptors.Add(new HeliosInterfaceDescriptor(type, interfaceAttribute));
                            }

                            HeliosUnitConverterAttribute converterAttribute = attribute as HeliosUnitConverterAttribute;
                            if (converterAttribute != null)
                            {
                                ConfigManager.LogManager.LogDebug("Converter found " + type.Name);
                                _converters.Add((BindingValueUnitConverter)Activator.CreateInstance(type));
                            }

                            HeliosPropertyEditorAttribute editorAttribute = attribute as HeliosPropertyEditorAttribute;
                            if (editorAttribute != null)
                            {
                                ConfigManager.LogManager.LogDebug("Property editor found " + type.Name);
                                HeliosPropertyEditorDescriptorCollection editors;
                                if (_propertyEditors.ContainsKey(editorAttribute.TypeIdentifier))
                                {
                                    editors = _propertyEditors[editorAttribute.TypeIdentifier];
                                }
                                else
                                {
                                    editors = new HeliosPropertyEditorDescriptorCollection();
                                    _propertyEditors.Add(editorAttribute.TypeIdentifier, editors);
                                }

                                editors.Add(new HeliosPropertyEditorDescriptor(type, editorAttribute));
                            }
                        }

                    }
                }
                catch (ReflectionTypeLoadException e)
                {
                    ConfigManager.LogManager.LogError("Failed reflecting assembly " + asm.FullName, e);
                }
                catch (Exception e)
                {
                    ConfigManager.LogManager.LogError("Failed adding module " + asm.FullName, e);
                }
            }
        }

        private bool CompareKeys(byte[] key)
        {
            if (key.Length != _publicKey.Length)
            {
                return false;
            }

            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] != _publicKey[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

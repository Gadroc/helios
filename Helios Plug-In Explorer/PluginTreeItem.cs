using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GadrocsWorkshop.Helios.PlugInExplorer
{
    public class PluginTreeItem : INotifyPropertyChanged
    {
        public enum TreeItemType
        {
            Plugin,
            Folder,
            Display,
            Device,
            Control
        }

        private TreeItemType _type;
        private PluginTreeItem _parent;
        private ObservableCollection<PluginTreeItem> _children;
        private bool _expanded;
        private bool _selected;
        private object _item;
        private string _name;

        private PluginTreeItem(string name, object item, PluginTreeItem parent, TreeItemType type)
        {
            _name = name;
            _item = item;
            _parent = parent;
            _type = type;
            _children = new ObservableCollection<PluginTreeItem>();
        }

        public PluginTreeItem(IPlugIn plugIn, IPlugInMetaData plugInMetaData) : this(plugInMetaData.Name, plugInMetaData, null, TreeItemType.Plugin)
        {
            PluginTreeItem displays = new PluginTreeItem("Displays", null, this, TreeItemType.Folder);
            foreach (IDisplay display in plugIn.GetDisplays())
            {
                displays.Children.Add(new PluginTreeItem(display.Name, display, displays, TreeItemType.Display));
            }
            if (displays.Children.Count > 0)
            {
                Children.Add(displays);
            }

            PluginTreeItem devices = new PluginTreeItem("Devices", null, this, TreeItemType.Folder);
            foreach (IDevice device in plugIn.GetDevices())
            {
                devices.Children.Add(new PluginTreeItem(device.Name, device, devices, TreeItemType.Device));
            }
            if (devices.Children.Count > 0)
            {
                Children.Add(devices);
            }

            PluginTreeItem controls = new PluginTreeItem("Controls", null, this, TreeItemType.Folder);
            foreach (IControlType controlType in plugIn.GetControlTypes())
            {
                controls.Children.Add(new PluginTreeItem(controlType.Name, controlType, controls, TreeItemType.Control));
            }
            if (controls.Children.Count > 0)
            {
                Children.Add(controls);
            }
        }

        public ObservableCollection<PluginTreeItem> Children
        {
            get { return _children; }
        }

        public bool IsSelected
        {
            get { return _selected; }
            set
            {
                if (value != _selected)
                {
                    _selected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public bool IsExpanded
        {
            get { return _expanded; }
            set
            {
                if (value != _expanded)
                {
                    _expanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_expanded && _parent != null)
                    _parent.IsExpanded = true;
            }
        }

        public object Item
        {
            get { return _item; }
        }

        public TreeItemType ItemType
        {
            get { return _type; }
        }

        public string Name
        {
            get { return _name; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, args);
            }
        }
    }
}

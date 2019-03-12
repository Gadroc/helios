//  Copyright 2014 Craig Courtney
//    
//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later versionCannot find interface trigger
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
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;

    // for inputs, a trigger on the interface creates an action on the device
    public struct DefaultInputBinding
    {
        public string ChildName, InterfaceTriggerName, DeviceActionName;

        public DefaultInputBinding(string childName, string interfaceTriggerName, string deviceActionName)
        {
            ChildName = childName;
            InterfaceTriggerName = interfaceTriggerName;
            DeviceActionName = deviceActionName;
            ConfigManager.LogManager.LogInfo("Default Input Binding: Trigger " + interfaceTriggerName + " to action " + deviceActionName + " for child " + childName);
        }
    }

    // for output, a triggeer on the device leads to an action on the interface
    public struct DefaultOutputBinding
    {
        public string ChildName, DeviceTriggerName, InterfaceActionName;

        public DefaultOutputBinding(string childName, string deviceTriggerName, string interfaceActionName)
        {
            ChildName = childName;
            DeviceTriggerName = deviceTriggerName;
            InterfaceActionName = interfaceActionName;
            ConfigManager.LogManager.LogInfo("Default Output Binding: Trigger " + deviceTriggerName + " to action " + interfaceActionName  + " for child " + childName);
        }
    }

    public abstract class CompositeVisual : HeliosVisual
    {
        private Dictionary<HeliosVisual, Rect> _nativeSizes = new Dictionary<HeliosVisual, Rect>();
        protected List<DefaultOutputBinding> _defaultOutputBindings;
        protected List<DefaultInputBinding> _defaultInputBindings;
        protected string _defaultInterfaceName; // default name of the interface to be used
        protected string _defaultBindingName;   // the name of the default binding in the interface
        protected HeliosInterface _defaultInterface;


        public CompositeVisual(string name, Size nativeSize)
            : base(name, nativeSize)
        {
            PersistChildren = false;
            Children.CollectionChanged += Children_CollectionChanged;
            _defaultInterfaceName = "";
            _defaultBindingName = "";
            _defaultInterface = null;
            _defaultInputBindings = new List<DefaultInputBinding>();
            _defaultOutputBindings = new List<DefaultOutputBinding>();
        }

        void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if ((e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add) ||
                (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace))
            {
                foreach (HeliosVisual control in e.NewItems)
                {
                    if (!_nativeSizes.ContainsKey(control))
                    {
                        _nativeSizes.Add(control, new Rect(control.Left, control.Top, control.Width, control.Height));
                    }
                }
            }

            if ((e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove) ||
                (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace))
            {
                foreach (HeliosVisual control in e.OldItems)
                {
                    if (_nativeSizes.ContainsKey(control))
                    {
                        _nativeSizes.Remove(control);
                    }
                }
            }
        }

        #region Properties
        public string DefaultInterfaceName
        {
            set
            {
                _defaultInterfaceName = value;
            }
        }

        public string DefaultBindingName
        {
            set
            {
                _defaultBindingName = value;
            }
        }

        public List<DefaultInputBinding> DefaultInputBindings
        {
            get {
                return _defaultInputBindings;
            }
        }
        public List<DefaultOutputBinding> DefaultOutputBindings
        {
            get
            {
                return _defaultOutputBindings;
            }
        }

        #endregion

        public override void Reset()
        {
            base.Reset();
            foreach (HeliosVisual child in Children)
            {
                child.Reset();
            }
        }

        protected override void OnPropertyChanged(PropertyNotificationEventArgs args)
        {
            if (args.PropertyName.Equals("Width") || args.PropertyName.Equals("Height"))
            {
                double scaleX = Width / NativeSize.Width;
                double scaleY = Height / NativeSize.Height;
                ScaleChildrenInt(scaleX, scaleY);
            }
            base.OnPropertyChanged(args);
        }

        private void ScaleChildrenInt(double scaleX, double scaleY)
        {
            foreach (KeyValuePair<HeliosVisual, Rect> item in _nativeSizes)
            {
                if (item.Value.Left > 0)
                {
                    double locXDif = item.Value.Left;
                    item.Key.Left = item.Value.Left + (locXDif * scaleX) - locXDif;
                }
                item.Key.Width = Math.Max(item.Value.Width * scaleX, 1d);
                if (item.Value.Top > 0)
                {
                    double locYDif = item.Value.Top;
                    item.Key.Top = item.Value.Top + (locYDif * scaleY) - locYDif;
                }
                item.Key.Height = Math.Max(item.Value.Height * scaleY, 1d);
            }
        }

        protected void AddDefaultInputBinding(string childName, string interfaceTriggerName, string deviceActionName)
        {
            DefaultInputBindings.Add(new DefaultInputBinding(
                childName: childName,
                interfaceTriggerName: interfaceTriggerName,
                deviceActionName: deviceActionName
                ));
        }

        protected void AddDefaultOutputBinding(string childName, string deviceTriggerName, string interfaceActionName)
        {
            DefaultOutputBindings.Add(new DefaultOutputBinding(
                childName: childName,
                deviceTriggerName: deviceTriggerName,
                interfaceActionName: interfaceActionName
                ));
        }

        private HeliosBinding CreateNewBinding(IBindingTrigger trigger, IBindingAction action)
        {
            HeliosBinding binding = new HeliosBinding(trigger, action);

            if (action.ActionRequiresValue && (ConfigManager.ModuleManager.CanConvertUnit(trigger.Unit, action.Unit)))
            {
                binding.ValueSource = BindingValueSources.TriggerValue;
            }
            else
            {
                binding.ValueSource = BindingValueSources.StaticValue;
            }
            return binding;
        }

        protected override void OnProfileChanged(HeliosProfile oldProfile)
        {
            base.OnProfileChanged(oldProfile);
            if (!DesignMode)
                return;

            /// grab the default interface, if it exists
            if (_defaultInterfaceName == "") {
                return; 
            }
            if (!Profile.Interfaces.ContainsKey(_defaultInterfaceName))
            {
                ConfigManager.LogManager.LogError("Cannot find default interface " + _defaultInterfaceName);
                return;
            }
            _defaultInterface = Profile.Interfaces[_defaultInterfaceName];

            /// looping for all default input bindings to assign the value
            foreach (DefaultInputBinding defaultBinding in _defaultInputBindings)
            {
                if (!Children.ContainsKey(defaultBinding.ChildName))
                {
                    ConfigManager.LogManager.LogError("Cannot find child " + defaultBinding.ChildName);
                    continue;
                }
                ConfigManager.LogManager.LogDebug("Auto binding child " + defaultBinding.ChildName);
                HeliosVisual child = Children[defaultBinding.ChildName];
                if (!child.Actions.ContainsKey(defaultBinding.DeviceActionName))
                {
                    ConfigManager.LogManager.LogError("Cannot find action " + defaultBinding.DeviceActionName);
                    continue;
                }
                if (!_defaultInterface.Triggers.ContainsKey(defaultBinding.InterfaceTriggerName))
                {
                    ConfigManager.LogManager.LogError("Cannot find interface trigger " + defaultBinding.InterfaceTriggerName);
                    continue;
                }
                ConfigManager.LogManager.LogDebug("Auto binding trigger " + defaultBinding.InterfaceTriggerName + " to " + defaultBinding.DeviceActionName);
                child.OutputBindings.Add(CreateNewBinding(_defaultInterface.Triggers[defaultBinding.InterfaceTriggerName], 
                    child.Actions[defaultBinding.DeviceActionName]));

                //child.OutputBindings.Add(
                //    new HeliosBinding(_defaultInterface.Triggers[defaultBinding.InterfaceTriggerName],
                //        child.Actions[defaultBinding.DeviceActionName]));
            }

            /// now looping for all default output bindings to assign the value
            foreach (DefaultOutputBinding defaultBinding in _defaultOutputBindings) {
                if (!Children.ContainsKey(defaultBinding.ChildName)) {
                    ConfigManager.LogManager.LogError("Cannot find child " + defaultBinding.ChildName);
                    continue;
                }
                HeliosVisual child = Children[defaultBinding.ChildName];
                if (!child.Triggers.ContainsKey(defaultBinding.DeviceTriggerName)) {
                    ConfigManager.LogManager.LogError("Cannot find trigger " + defaultBinding.DeviceTriggerName);
                    continue;
                }
                if (!_defaultInterface.Actions.ContainsKey(defaultBinding.InterfaceActionName))
                {
                    ConfigManager.LogManager.LogError("Cannot find action " + defaultBinding.InterfaceActionName);
                    continue;
                }
                ConfigManager.LogManager.LogDebug("Child Output binding trigger " + defaultBinding.DeviceTriggerName + " to " + defaultBinding.InterfaceActionName);
                child.OutputBindings.Add(CreateNewBinding(child.Triggers[defaultBinding.DeviceTriggerName],
                                      _defaultInterface.Actions[defaultBinding.InterfaceActionName]));
        //            child.OutputBindings.Add(
        //new HeliosBinding(child.Triggers[defaultBinding.DeviceTriggerName],
        //                  _defaultInterface.Actions[defaultBinding.InterfaceActionName]));
            }
        }

        private Point FromCenter(Point pos, Size size) {
            return new Point(pos.X - size.Width / 2.0, pos.Y - size.Height / 2.0);
        }

        protected void AddTrigger(IBindingTrigger trigger, string device)
        {
            trigger.Device = device;
            Triggers.Add(trigger);
        }

        protected void AddAction(IBindingAction action, string device)
        {
            action.Device = device;
            Actions.Add(action);
        }

        private string GetComponentName(string name) {
            return Name + "_" + name;
        }

        protected Potentiometer AddPot(string name, Point posn, Size size, string knobImage,
            double initialRotation, double rotationTravel, double minValue, double maxValue, 
            double initialValue, double stepValue,
            string interfaceDeviceName, string interfaceElementName, bool fromCenter)
        {
            string componentName = GetComponentName(name);
            if (fromCenter)
                posn = FromCenter(posn, size);
            Potentiometer _knob = new Potentiometer
            {
                Name = componentName,
                KnobImage = knobImage,
                InitialRotation = initialRotation,
                RotationTravel = rotationTravel,
                MinValue = minValue,
                MaxValue = maxValue,
                InitialValue = initialValue,
                StepValue = stepValue,
                Top = posn.Y,
                Left = posn.X,
                Width = size.Width,
                Height = size.Height
            };

            Children.Add(_knob);
            foreach (IBindingTrigger trigger in _knob.Triggers)
            {
                AddTrigger(trigger, componentName);
            }
            AddAction(_knob.Actions["set.value"], componentName);

            AddDefaultOutputBinding(
                childName: componentName,
                deviceTriggerName: "value.changed",
                interfaceActionName: interfaceDeviceName + ".set." + interfaceElementName
           );



            return _knob;
        }

        protected RotaryEncoder AddEncoder(string name, Point posn, Size size, 
            string knobImage, double stepValue, double rotationStep, 
            string interfaceDeviceName, string interfaceElementName, bool fromCenter)
        {
            if (fromCenter)
                posn = FromCenter(posn, size);
            string componentName = GetComponentName(name);
            RotaryEncoder _knob = new RotaryEncoder
            {
                Name = componentName,
                KnobImage = knobImage,
                StepValue = stepValue,
                RotationStep = rotationStep,
                Top = posn.Y,
                Left = posn.X,
                Width = size.Width,
                Height = size.Height
            };

            Children.Add(_knob);
            foreach (IBindingTrigger trigger in _knob.Triggers)
            {
                AddTrigger(trigger, componentName);
            }
            foreach (IBindingAction action in _knob.Actions)
            {
                AddAction(action, componentName);
            }
            AddDefaultOutputBinding(
                childName: componentName,
                deviceTriggerName: "encoder.incremented",
                interfaceActionName: interfaceDeviceName + ".increment." + interfaceElementName
            );
            AddDefaultOutputBinding(
                childName: componentName,
                deviceTriggerName: "encoder.decremented",
                interfaceActionName: interfaceDeviceName + ".decrement." + interfaceElementName
                );

            return _knob;
        }


        protected PushButton AddButton(string name, Point posn, Size size, string image, string pushedImage,
            string buttonText, string interfaceDeviceName, string interfaceElementName, bool fromCenter)
        {
            if (fromCenter)
                posn = FromCenter(posn, size);
            string componentName = GetComponentName(name);
            PushButton button = new PushButton
            {
                Top = posn.Y,
                Left = posn.X,
                Width = size.Width,
                Height = size.Height,
                Image = image,
                PushedImage = pushedImage,
                Text = buttonText,
                Name = componentName
            };

            Children.Add(button);

            AddTrigger(button.Triggers["pushed"], componentName);
            AddTrigger(button.Triggers["released"], componentName);

            AddAction(button.Actions["push"], componentName);
            AddAction(button.Actions["release"], componentName);
            AddAction(button.Actions["set.physical state"], componentName);

            // add the default actions
            AddDefaultOutputBinding(
                childName: componentName,
                deviceTriggerName: "pushed",
                interfaceActionName: interfaceDeviceName + ".push." + interfaceElementName 
                );
            AddDefaultOutputBinding(
                childName: componentName,
                deviceTriggerName: "released",
                interfaceActionName: interfaceDeviceName + ".release." + interfaceElementName
                );
            AddDefaultInputBinding(
                childName: componentName,
                interfaceTriggerName: interfaceDeviceName + "." + interfaceElementName + ".changed",
                deviceActionName: "set.physical state");

            return button;
        }

        protected Indicator AddIndicator(string name, Point posn, Size size,
            string onImage, string offImage, Color onTextColor, Color offTextColor, string font,
            bool vertical, string interfaceDeviceName, string interfaceElementName, bool fromCenter)
        {
            if (fromCenter)
                posn = FromCenter(posn, size);
            string componentName = GetComponentName("Annunciator " + name);
            Indicator indicator = new Helios.Controls.Indicator
            {
                Top = posn.Y,
                Left = posn.X,
                Width = size.Width,
                Height = size.Height,
                OnImage = onImage,
                OffImage = offImage
            };

            indicator.Text = name;
            indicator.Name = componentName;
            indicator.OnTextColor = onTextColor;
            indicator.OffTextColor = offTextColor;
            indicator.TextFormat.FontStyle = FontStyles.Normal;
            indicator.TextFormat.FontWeight = FontWeights.Normal;
            if (vertical)
            {
                indicator.TextFormat.FontSize = 8;
            }
            else
            {
                indicator.TextFormat.FontSize = 12;
            }
            indicator.TextFormat.FontFamily = new FontFamily(font);
            indicator.TextFormat.PaddingLeft = 0;
            indicator.TextFormat.PaddingRight = 0;
            indicator.TextFormat.PaddingTop = 0;
            indicator.TextFormat.PaddingBottom = 0;
            indicator.TextFormat.VerticalAlignment = TextVerticalAlignment.Center;
            indicator.TextFormat.HorizontalAlignment = TextHorizontalAlignment.Center;

            Children.Add(indicator);
            foreach (IBindingTrigger trigger in indicator.Triggers)
            {
                AddTrigger(trigger, componentName);
            }
            AddAction(indicator.Actions["set.indicator"], componentName);

            return indicator;
        }

        protected IndicatorPushButton AddIndicatorPushButton(string name, Point pos, Size size,
            string image, string pushedImage, Color textColor, Color onTextColor, string font)
        {
            string componentName = GetComponentName(name);
            IndicatorPushButton indicator = new Helios.Controls.IndicatorPushButton
            {
                Top = pos.Y,
                Left = pos.X,
                Width = size.Width,
                Height = size.Height,
                Image = image,
                PushedImage = pushedImage,
                Text = name,
                Name = componentName,
                OnTextColor = onTextColor,
                TextColor = textColor
            };
            indicator.TextFormat.FontStyle = FontStyles.Normal;
            indicator.TextFormat.FontWeight = FontWeights.Normal;
            indicator.TextFormat.FontSize = 18;
            indicator.TextFormat.FontFamily = new FontFamily(font);
            indicator.TextFormat.PaddingLeft = 0;
            indicator.TextFormat.PaddingRight = 0;
            indicator.TextFormat.PaddingTop = 0;
            indicator.TextFormat.PaddingBottom = 0;
            indicator.TextFormat.VerticalAlignment = TextVerticalAlignment.Center;
            indicator.TextFormat.HorizontalAlignment = TextHorizontalAlignment.Center;

            Children.Add(indicator);
            AddTrigger(indicator.Triggers["pushed"], componentName);
            AddTrigger(indicator.Triggers["released"], componentName);

            AddAction(indicator.Actions["push"], componentName);
            AddAction(indicator.Actions["release"], componentName);
            AddAction(indicator.Actions["set.indicator"], componentName);
            return indicator;
        }

        protected ThreeWayToggleSwitch AddThreeWayToggle(string name, Point pos, Size size,
            ThreeWayToggleSwitchPosition defaultPosition, ThreeWayToggleSwitchType switchType,
            string interfaceDeviceName, string interfaceElementName, bool fromCenter, 
            string positionOneImage = "{Helios}/Images/Toggles/round-up.png", 
            string positionTwoImage = "{Helios}/Images/Toggles/round-norm.png", 
            string positionThreeImage = "{Helios}/Images/Toggles/round-down.png")
        {
            string componentName = GetComponentName(name);
            ThreeWayToggleSwitch toggle = new ThreeWayToggleSwitch
            {
                Top = pos.Y,
                Left =  pos.X,
                Width = size.Width,
                Height = size.Height,
                DefaultPosition = defaultPosition,
                PositionOneImage = positionOneImage,
                PositionTwoImage = positionTwoImage,
                PositionThreeImage = positionThreeImage,
                SwitchType = switchType,
                Name = componentName
            };

            Children.Add(toggle);
            foreach (IBindingTrigger trigger in toggle.Triggers)
            {
                AddTrigger(trigger, componentName);
            }
            AddAction(toggle.Actions["set.position"], componentName);

            AddDefaultOutputBinding(
                childName: componentName,
                deviceTriggerName: "position.changed",
                interfaceActionName: interfaceDeviceName + ".set." + interfaceElementName
            );
            AddDefaultInputBinding(
                childName: componentName,
                interfaceTriggerName: interfaceDeviceName + "." + interfaceElementName + ".changed",
                deviceActionName: "set.position");

            return toggle;
        }

        protected TextDisplay AddTextDisplay(
            string name, 
            Point pos, 
            Size size, 
            string font, 
            TextHorizontalAlignment horizontalAlignment,
            TextVerticalAlignment verticalAligment,
            double baseFontsize, 
            string testTextDisplay,
            Color textColor,
            Color backgroundColor,
            bool useBackground,
            string interfaceDeviceName, 
            string interfaceElementName
            )
        {
            string componentName = GetComponentName(name);
            TextDisplay display = new TextDisplay
            {
                Top = pos.Y,
                Left = pos.X,
                Width = size.Width,
                Height = size.Height,
                Name = componentName
            };
            TextFormat textFormat = new TextFormat
            {
                FontFamily = new FontFamily(font),
                HorizontalAlignment = horizontalAlignment,
                VerticalAlignment = verticalAligment,
                FontSize = baseFontsize,
                PaddingRight = 0,
                PaddingLeft = 0,
                PaddingTop = 0,
                PaddingBottom = 0
            };

            display.TextFormat = textFormat;
            display.OnTextColor = textColor; // Color.FromArgb(0xff, 0x40, 0xb3, 0x29);
            display.BackgroundColor = backgroundColor; // Color.FromArgb(0xff, 0x00, 0x00, 0x00);
            display.UseBackground = useBackground;
            display.ParserDictionary = "";
            display.TextTestValue = testTextDisplay;
            Children.Add(display);
            AddAction(display.Actions["set.TextDisplay"], componentName);

            AddDefaultInputBinding(
                childName: componentName,
                interfaceTriggerName: interfaceDeviceName + "." + interfaceElementName + ".changed",
                deviceActionName: "set.TextDisplay");

            return display;
        }

    }

}

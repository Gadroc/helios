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


    public abstract partial class CompositeVisualAV8B : CompositeVisual
    {
        protected new HeliosInterface _defaultInterface;

        public CompositeVisualAV8B(string name, Size nativeSize)
            : base(name, nativeSize)
        {
            _defaultInterfaceName = "";
            _defaultBindingName = "";
            _defaultInterface = null;
        }

        private string GetComponentName(string name)
        {
            return Name + "_" + name;
        }

//
// For gauges with more than one input function, we need this override to map the 
// different interface element names to the individual functions.  This carries with it
// a requirement for the gauge function name to equal the interface element name
//

        protected new Gauges.BaseGauge AddGauge(
            string name,
            Gauges.BaseGauge gauge,
            Point posn,
            Size size,
            string interfaceDeviceName,
            string interfaceElementName
            )
        {
            if (DefaultInterfaceName == "DCS AV-8B" && (name == "Altimeter Gauge" || name == "ADI Gauge" ))
            {
                gauge.Name = name;
                gauge.Top = posn.Y;
                gauge.Left = posn.X;
                gauge.Width = size.Width;
                gauge.Height = size.Height;

                string componentName = GetComponentName(name);
                gauge.Name = componentName;


                Children.Add(gauge);
                foreach (IBindingTrigger trigger in gauge.Triggers)
                {
                    AddTrigger(trigger, trigger.Device);
                }
                foreach (IBindingAction action in gauge.Actions)
                {
                    if (action.Name != "hidden")
                    {
                        AddAction(action, action.Device);
                        // SAI Pitch adjustment offset
                        // SAI Cage/Pitch Adjust Knob
                        if (name == "ADI Gauge" && action.Name == "SAI Pitch adjustment offset")
                        {
                            interfaceElementName = "SAI Cage/Pitch Adjust Knob";
                        }
                        else
                        {
                            interfaceElementName = action.Name;
                        }
                        AddDefaultInputBinding(
                        childName: componentName,
                        interfaceTriggerName: interfaceDeviceName + "." + interfaceElementName + ".changed",
                        deviceActionName: action.Device + "." + action.ActionVerb + "." + action.Name
                        );
                    }
                }
                return gauge;
            }
            else
            {
                return base.AddGauge(
                    name,
                    gauge,
                    posn,
                    size,
                    interfaceDeviceName,
                    interfaceElementName
                    );
            }
        }
        private Point FromCenter(Point posn, Size size)
        {
            return new Point(posn.X - size.Width / 2.0, posn.Y - size.Height / 2.0);
        }
        protected new RotaryEncoder AddEncoder(string name, Point posn, Size size,
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
                if (action.Name != "hidden")
                {
                    AddAction(action, componentName);
                }
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

    }
}

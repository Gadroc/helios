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
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;

    public abstract class CompositeVisual : HeliosVisual
    {
        private Dictionary<HeliosVisual, Rect> _nativeSizes = new Dictionary<HeliosVisual, Rect>();

        public CompositeVisual(string name, Size nativeSize)
            : base(name, nativeSize)
        {
            PersistChildren = false;
            Children.CollectionChanged += Children_CollectionChanged;
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

        /// <summary>
        ///  method to add a Pot to the Composite visual
        /// </summary>
        /// <param name="name"></param>
        /// <param name="posn"></param>
        /// <param name="size"></param>
        /// <param name="knobImage"></param>
        /// <param name="initialRotation"></param>
        /// <param name="rotationTravel"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="initialValue"></param>
        /// <param name="stepValue"></param>
        protected Helios.Controls.Potentiometer AddPot(string name, Point posn, Size size, string knobImage,
            double initialRotation, double rotationTravel, double minValue, double maxValue, double initialValue, double stepValue)
        {
            string componentName = GetComponentName(name);
            Potentiometer _knob = new Helios.Controls.Potentiometer
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

            return _knob;
        }

        /// <summary>
        /// Method to add an encoder
        /// </summary>
        /// <param name="name"></param>
        /// <param name="posn"></param>
        /// <param name="size"></param>
        /// <param name="knobImage"></param>
        /// <param name="stepValue"></param>
        /// <param name="rotationStep"></param>
        protected Helios.Controls.RotaryEncoder AddEncoder(string name, Point posn, Size size, string knobImage, double stepValue, double rotationStep)
        {
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
            return _knob;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="posn"></param>
        /// <param name="size"></param>
        /// <param name="image"></param>
        /// <param name="pushedImage"></param>
        /// <param name="buttonText"></param>
        /// <returns></returns>
        protected PushButton AddButton(string name, Point posn, Size size, string image, string pushedImage,
            string buttonText)
        {
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

            return button;
        }

        /// <summary>
        /// Adding an Indicator to the composite image
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="onImage"></param>
        /// <param name="offImage"></param>
        /// <param name="onTextColor"></param>
        /// <param name="offTextColor"></param>
        /// <param name="font"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        protected Indicator AddIndicator(string name, Point pos, Size size,
            string onImage, string offImage, Color onTextColor, Color offTextColor, string font,
            bool vertical)
        {
            string componentName = GetComponentName("Annunciator " + name);
            Helios.Controls.Indicator indicator = new Helios.Controls.Indicator
            {
                Top = pos.Y,
                Left = pos.X,
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="image"></param>
        /// <param name="pushedImage"></param>
        /// <param name="textColor"></param>
        /// <param name="onTextColor"></param>
        /// <param name="font"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="defaultPosition"></param>
        /// <param name="switchType"></param>
        /// <param name="positionOneImage"></param>
        /// <param name="positionTwoImage"></param>
        /// <param name="positionThreeImage"></param>
        /// <returns></returns>
        protected ThreeWayToggleSwitch AddThreeWayToggle(string name, Point pos, Size size,
            ThreeWayToggleSwitchPosition defaultPosition, ThreeWayToggleSwitchType switchType,
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
                PositionOneImage = "{Helios}/Images/Toggles/round-up.png",
                PositionTwoImage = "{Helios}/Images/Toggles/round-norm.png",
                PositionThreeImage = "{Helios}/Images/Toggles/round-down.png",
                SwitchType = ThreeWayToggleSwitchType.OnOnOn,
                Name = componentName
            };

            Children.Add(toggle);
            foreach (IBindingTrigger trigger in toggle.Triggers)
            {
                AddTrigger(trigger, componentName);
            }
            AddAction(toggle.Actions["set.position"], componentName);
            return toggle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="size"></param>
        /// <param name="font"></param>
        /// <param name="horizontalAlignment"></param>
        /// <param name="verticalAligment"></param>
        /// <param name="baseFontsize"></param>
        /// <param name="testTextDisplay"></param>
        /// <param name="textColor"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="useBackground"></param>
        /// <returns></returns>
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
            bool useBackground
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
                FontSize = baseFontsize
            };

            display.TextFormat = textFormat;
            display.OnTextColor = Color.FromArgb(0xff, 0x40, 0xb3, 0x29);
            display.BackgroundColor = Color.FromArgb(0xff, 0x00, 0x00, 0x00);
            display.UseBackground = useBackground;
            display.ParserDictionary = "A=A";
            display.TextTestValue = testTextDisplay;
            Children.Add(display);
            AddAction(display.Actions["set.TextDisplay"], componentName);

            return display;
        }

    }

}

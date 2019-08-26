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

namespace GadrocsWorkshop.Helios.Gauges.M2000C
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Globalization;
    using System.Windows;

    public class Indicator_Desc
    {
        private string _name;
        private double _posx;
        private double _posy;
        private string _color;

        public Indicator_Desc(string name, double posx, double posy, string color)
        {
            _name = name;
            _posx = posx;
            _posy = posy;
            _color = color;
        }
        public string getName() { return _name; }
        public double getPosX() { return _posx; }
        public double getPosY() { return _posy; }
        public string getColor() { return _color; }
    }

    [HeliosControl("M2000C_CAUTION_PANEL", "Caution Panel", "M2000C Gauges", typeof(MFDRenderer))]
    class M2000C_CautionPanel : MFD
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 256, 280);
        private Rect _scaledScreenRect = SCREEN_RECT;

        public M2000C_CautionPanel()
            : base("Caution Panel", new Size(256, 280))
        {
            int row0 = 30, row1 = 92, row2 = 112, row3 = 132, row4 = 153, row5 = 173, row6 = 193, row7 = 214, row8 = 234;
            int column1 = 14, column2 = 57, column3 = 100, column4 = 144, column5 = 185;
            //First row
            AddIndicator("BATT", column1, row1);
            AddIndicator("TR", column2, row1);
            AddIndicator("ALT1", column3, row1);
            AddIndicator("ALT2", column4, row1);
            //Second row
            AddIndicator("HUILE", column1, row2);
            AddIndicator("T7", column2, row2);
            AddIndicator("CALC", column3, row2);
            AddIndicator("SOURIS", column4, row2);
            AddIndicator("PELLE", column5, row2);
            //Third row
            AddIndicator("BP", column1, row3);
            AddIndicator("BPG", column2, row3);
            AddIndicator("BPD", column3, row3);
            AddIndicator("TRANSF", column4, row3);
            AddIndicator("NIVEAU", column5, row3);
            //Forth row
            AddIndicator("HYD1", column1, row4);
            AddIndicator("HYD2", column2, row4);
            AddIndicator("HYDS", column3, row4);
            AddIndicator("EP", column4, row4);
            AddIndicator("BINGO", column5, row4);
            //Fifth row
            AddIndicator("PCAB", column1, row5);
            AddIndicator("TEMP", column2, row5);
            AddIndicator("REGO2", column3, row5);
            AddIndicator("5mnO2", column4, row5);
            AddIndicator("O2HA", column5, row5);
            //Sixth row
            AddIndicator("ANEMO", column1, row6);
            AddIndicator("CC", column2, row6);
            AddIndicator("DSV", column3, row6);
            AddIndicator("CONDIT", column4, row6);
            AddIndicator("CONF", column5, row6);
            //Seventh row
            AddIndicator("PA", column1, row7);
            AddIndicator("MAN", column2, row7);
            AddIndicator("DOM", column3, row7);
            AddIndicator("BECS", column4, row7);
            AddIndicator("U.S.EL", column5, row7);
            //Eighth row
            AddIndicator("ZEICHEN", column1, row8);
            AddIndicator("GAIN", column2, row8);
            AddIndicator("RPM", column3, row8);
            AddIndicator("DECOL", column4, row8);
            AddIndicator("PARK", column5, row8);

            AddSwitch("Main Battery Switch", "red", 15, row0, ToggleSwitchPosition.Two, ToggleSwitchType.OnOn, false);
            AddSwitch("Electric Power Transfer Switch", "long-black", 61, row0, ToggleSwitchPosition.One, ToggleSwitchType.OnOn, false);
            AddSwitch("Alternator 1 Switch", "long-black", 105, row0, ToggleSwitchPosition.One, ToggleSwitchType.OnOn, false);
            AddSwitch("Alternator 2 Switch", "long-black", 147, row0, ToggleSwitchPosition.One, ToggleSwitchType.OnOn, false);
            AddSwitch("Test", "long-black", 190, 76, ToggleSwitchPosition.Two, ToggleSwitchType.MomOn, true);
        }

        #region Properties

        public override string BezelImage
        {
            get { return "{Helios}/Images/M2000C/CautionPanel/caution-panel.png"; }
        }

        #endregion

        protected override void OnPropertyChanged(PropertyNotificationEventArgs args)
        {
            if (args.PropertyName.Equals("Width") || args.PropertyName.Equals("Height"))
            {
                double scaleX = Width / NativeSize.Width;
                double scaleY = Height / NativeSize.Height;
                _scaledScreenRect.Scale(scaleX, scaleY);
            }
            base.OnPropertyChanged(args);
        }

        private void AddIndicator(string name, double x, double y)
        {
            
            Indicator newIndicator = new Indicator();
            newIndicator.Top = y;
            newIndicator.Left = x;
            newIndicator.Width = 32;
            newIndicator.Height = 15;
            newIndicator.Text = "";

            newIndicator.OffImage = "{Helios}/Images/M2000C/CautionPanel/" + name + "-off.png";
            newIndicator.OnImage = "{Helios}/Images/M2000C/CautionPanel/" + name + "-on.png";

            newIndicator.Name = name;

            Children.Add(newIndicator);

            foreach (IBindingTrigger trigger in newIndicator.Triggers)
            {
                AddTrigger(trigger, name);
            }
            foreach (IBindingAction action in newIndicator.Actions)
            {
                AddAction(action, name);
            }
//            AddAction(newIndicator.Actions["set.indicator"], name);
        }

        private new void AddTrigger(IBindingTrigger trigger, string device)
        {
            trigger.Device = device;
            Triggers.Add(trigger);
        }

        private new void AddAction(IBindingAction action, string device)
        {
            action.Device = device;
            Actions.Add(action);
        }

        private void AddSwitch(string name, string imagePrefix, double x, double y, ToggleSwitchPosition defaultPosition, ToggleSwitchType defaultType, bool horizontal)
        {
            ToggleSwitch newSwitch = new ToggleSwitch();
            newSwitch.Name = name;
            newSwitch.SwitchType = defaultType;
            newSwitch.ClickType = ClickType.Touch;
            newSwitch.DefaultPosition = defaultPosition;

            newSwitch.Top = y;
            newSwitch.Left = x;
            if (horizontal)
            {
                newSwitch.Orientation = ToggleSwitchOrientation.Horizontal;
                newSwitch.PositionOneImage = "{Helios}/Images/M2000C/Switches/" + imagePrefix + "-left.png";
                newSwitch.PositionTwoImage = "{Helios}/Images/M2000C/Switches/" + imagePrefix + "-right.png";
                newSwitch.Width = 50;
                newSwitch.Height = 25;
            }
            else
            {
                newSwitch.Orientation = ToggleSwitchOrientation.Vertical;
                newSwitch.PositionOneImage = "{Helios}/Images/M2000C/Switches/" + imagePrefix + "-up.png";
                newSwitch.PositionTwoImage = "{Helios}/Images/M2000C/SWitches/" + imagePrefix + "-down.png";
                newSwitch.Width = 25;
                newSwitch.Height = 50;
            }

            Children.Add(newSwitch);

            foreach (IBindingTrigger trigger in newSwitch.Triggers)
            {
                AddTrigger(trigger, name);
            }
            foreach (IBindingAction action in newSwitch.Actions)
            {
                AddAction(action, name);
            }
//            AddAction(newSwitch.Actions["set.position"], name);
        }

        public override bool HitTest(Point location)
        {
            if (_scaledScreenRect.Contains(location))
            {
                return false;
            }

            return true;
        }

        public override void MouseDown(Point location)
        {
            // No-Op
        }

        public override void MouseDrag(Point location)
        {
            // No-Op
        }

        public override void MouseUp(Point location)
        {
            // No-Op
        }
    }
}

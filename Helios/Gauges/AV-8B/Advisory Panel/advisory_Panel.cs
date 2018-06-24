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

namespace GadrocsWorkshop.Helios.Gauges.AV8B
{
    using GadrocsWorkshop.Helios.Gauges.AV8B;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Windows.Media;
    using System.Windows;
    using System.Windows.Threading;


    [HeliosControl("Helios.AV8B.Advisory", "AV-8B Advisory Panel", "AV-8B", typeof(AV8BDeviceRenderer))]
    class advisoryPanel : AV8BDevice
    {
        // these three sections are the dead space in the UFC image.
        private static readonly Rect SCREEN_RECT1 = new Rect(265, 0, 290, 320);
        private static readonly Rect SCREEN_RECT2 = new Rect(401, 290, 154, 320);
        private Rect _scaledScreenRect1 = SCREEN_RECT1;
        private Rect _scaledScreenRect2 = SCREEN_RECT2;

        //private String _font = "MS 33558";
        private String _font = "Franklin Gothic";

        public advisoryPanel()
            : base("AV-8B Advisory Panel", new Size(555, 1024))
        {
            AddIndicator("OXY", new Point(22,86), new Size(104,40));
            AddIndicator("HYD 1", new Point(22,146), new Size(104,40));
            AddIndicator("L PUMP", new Point(22,204), new Size(104,40));
            AddIndicator("L TRANS", new Point(22,262), new Size(104,40));
            AddIndicator("FLAPS 1", new Point(22,323), new Size(104,40));
            AddIndicator("PROP", new Point(22,382), new Size(104,40));
            AddIndicator("APU GEN", new Point(22,441), new Size(104,40));
            AddIndicator("DEP RES", new Point(22,499), new Size(104,40));
            AddIndicator("CS COOL", new Point(22,557), new Size(104,40));
            AddIndicator("INS", new Point(22,616), new Size(104,40));
            AddIndicator("AFC", new Point(22,676), new Size(104,40));
            AddIndicator("PITCH", new Point(22,734), new Size(104,40));
            AddIndicator("ROLL", new Point(22,792), new Size(104,40));
            AddIndicator("YAW", new Point(22,850), new Size(104,40));
            AddIndicator("ENG EXC", new Point(22,909), new Size(104,40));
            AddIndicator("WSHLD", new Point(154,86), new Size(104,40));
            AddIndicator("HYD 2", new Point(154,146), new Size(104,40));
            AddIndicator("R PUMP", new Point(154,204), new Size(104,40));
            AddIndicator("R TRANS", new Point(154,262), new Size(104,40));
            AddIndicator("FLAPS 2", new Point(154,323), new Size(104,40));
            AddIndicator("LIDS", new Point(154,382), new Size(104,40));
            AddIndicator("Blank 1", new Point(154,441), new Size(104,40));
            AddIndicator("DC", new Point(154,499), new Size(104,40));
            AddIndicator("LOAD", new Point(154,557), new Size(104,40));
            AddIndicator("SKID", new Point(154,616), new Size(104,40));
            AddIndicator("C*AUT", new Point(154,676), new Size(104,40));
            AddIndicator("IFF", new Point(154,734), new Size(104,40));
            AddIndicator("AFT BAY", new Point(154,792), new Size(104,40));
            AddIndicator("CW NOGO", new Point(154,850), new Size(104,40));
            AddIndicator("P NOGO", new Point(154,909), new Size(104,40));
            AddIndicator("AUT FLP", new Point(294,323), new Size(104,40));
            AddIndicator("OIL", new Point(294,382), new Size(104,40));
            AddIndicator("GPS", new Point(294,441), new Size(104,40));
            AddIndicator("STBY TR", new Point(294,499), new Size(104,40));
            AddIndicator("CANOPY", new Point(294,557), new Size(104,40));
            AddIndicator("EFC", new Point(294,616), new Size(104,40));
            AddIndicator("H2O SEL", new Point(294,676), new Size(104,40));
            AddIndicator("SPD BRK", new Point(294,734), new Size(104,40));
            AddIndicator("AV BIT", new Point(294,792), new Size(104,40));
            AddIndicator("P JAM", new Point(294,850), new Size(104,40));
            AddIndicator("CW JAM", new Point(294,909), new Size(104,40));
            AddIndicator("NWS", new Point(426,616), new Size(104,40));
            AddIndicator("APU", new Point(426,676), new Size(104,40));
            AddIndicator("DROOP", new Point(426,734), new Size(104,40));
            AddIndicator("Blank 2", new Point(426,792), new Size(104,40));
            AddIndicator("JMR HOT", new Point(426,850), new Size(104,40));
            AddIndicator("REPLY", new Point(426,909), new Size(104,40));
        }

        public override string BezelImage
        {
            get { return "{Helios}/Images/AV-8B/Advisory Panel Frame.png"; }
        }

        private void AddTrigger(IBindingTrigger trigger, string device)
        {
            trigger.Device = device;
            Triggers.Add(trigger);
        }

        private void AddAction(IBindingAction action, string device)
        {
            action.Device = device;
            Actions.Add(action);
        }
        private void AddIndicator(string name, Point point, Size size) { AddIndicator(name, point.X, point.Y, size, false); }
        private void AddIndicator(string name, double x, double y, Size size) { AddIndicator(name, x, y, size, false); }
        private void AddIndicator(string name, double x, double y, Size size, bool _vertical)
        {
            Helios.Controls.Indicator indicator = new Helios.Controls.Indicator();
            indicator.Top = y;
            indicator.Left = x;
            indicator.Width = size.Width;
            indicator.Height = size.Height;
            indicator.OnImage = "{Helios}/Images/Indicators/anunciator.png";
            indicator.OffImage = "{Helios}/Images/Indicators/anunciator.png";
            if (name == "Blank 1" || name == "Blank 2")
            {
                indicator.Text = "-";
            }
            else
            {
                indicator.Text = name;
            }
            indicator.Name = "Annunciator " + name;
            indicator.OnTextColor = Color.FromArgb(0xff, 0x94, 0xEB, 0xA6);
            indicator.OffTextColor = Color.FromArgb(0xff, 0x10, 0x10, 0x10);
            indicator.TextFormat.FontStyle = FontStyles.Normal;
            indicator.TextFormat.FontWeight = FontWeights.Normal;
            indicator.TextFormat.FontSize = 16;
            indicator.TextFormat.FontFamily = new FontFamily(_font);  // this probably needs to change before release
            indicator.TextFormat.PaddingLeft = 0;
            indicator.TextFormat.PaddingRight = 0;
            indicator.TextFormat.PaddingTop = 0;
            indicator.TextFormat.PaddingBottom = 0;
            indicator.TextFormat.VerticalAlignment = TextVerticalAlignment.Center;
            indicator.TextFormat.HorizontalAlignment = TextHorizontalAlignment.Center;

            Children.Add(indicator);
            foreach (IBindingTrigger trigger in indicator.Triggers)
            {
                AddTrigger(trigger, name);
            }
            foreach (IBindingAction action in indicator.Actions)
            {
                AddAction(action, name);
            }
        }

        public override bool HitTest(Point location)
        {
            if (_scaledScreenRect1.Contains(location) || _scaledScreenRect2.Contains(location))
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
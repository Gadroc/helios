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

namespace GadrocsWorkshop.Helios.ProfileEditor
{
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Media;

    class MonitorAdorner : Adorner
    {
        private Monitor _monitor;
        private string _label;
        private TextFormat _textFormat = new TextFormat();
        private Pen _borderPen = new Pen(Brushes.Black, 1d);

        public MonitorAdorner(HeliosVisualView adornedElement, string monitorLabel, Monitor monitor)
            : base(adornedElement)
        {
            _textFormat.VerticalAlignment = TextVerticalAlignment.Center;
            _label = monitorLabel;
            _monitor = monitor;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            drawingContext.DrawRectangle(null, _borderPen, adornedElementRect);
            drawingContext.DrawRoundedRectangle(Brushes.White, _borderPen,
                    new Rect(adornedElementRect.X + (adornedElementRect.Width / 2d) - 7d, adornedElementRect.Y + (adornedElementRect.Height / 2d) - 7d, 14d, 14d), 2d, 2d);
            _textFormat.RenderText(drawingContext, Brushes.Black, _label, adornedElementRect);
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ProfileEditorCommands.OpenProfileItem.Execute(_monitor, this);
                e.Handled = true;
            }
        }
    }
}

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

namespace GadrocsWorkshop.Helios.Windows.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Custom control used to represent a input axis.
    /// </summary>
    public class AxisBar : Control
    {
        public enum AxisBarOrientation
        {
            Horizontal,
            Vertical
        }

        static AxisBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AxisBar), new FrameworkPropertyMetadata(typeof(AxisBar)));
        }

        private static readonly PropertyChangedCallback _callback = new PropertyChangedCallback(FillPropertyChanged);

        private static void FillPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            AxisBar bar = source as AxisBar;
            
            if (bar != null)
            {
                bar.UpdateFill();
            }
        }

        #region Properties

        /// <summary>
        /// Brush used to fill in the bar.
        /// </summary>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FillColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(AxisBar), new UIPropertyMetadata(new SolidColorBrush(Colors.DarkRed)));

        /// <summary>
        /// Height of the filled area.
        /// </summary>
        public double FillHeight
        {
            get { return (double)GetValue(FillHeightProperty); }
        }

        // Using a DependencyProperty as the backing store for FillHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillHeightProperty =
            DependencyProperty.Register("FillHeight", typeof(double), typeof(AxisBar), new UIPropertyMetadata(0d));

        /// <summary>
        /// Width of the filled area.
        /// </summary>
        public double FillWidth
        {
            get { return (double)GetValue(FillWidthProperty); }
        }

        // Using a DependencyProperty as the backing store for FillWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillWidthProperty =
            DependencyProperty.Register("FillWidth", typeof(double), typeof(AxisBar), new UIPropertyMetadata(0d));

        /// <summary>
        /// Minimum possible input value.
        /// </summary>
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(AxisBar), new UIPropertyMetadata(0, _callback));

        /// <summary>
        /// Maximum possible input value.
        /// </summary>
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(AxisBar), new UIPropertyMetadata(65536, _callback));

        /// <summary>
        /// Input value used to render the bar.
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(AxisBar), new UIPropertyMetadata(0, _callback));

        /// <summary>
        /// Orientation used to fill the bar.
        /// </summary>
        public AxisBarOrientation Orientation
        {
            get { return (AxisBarOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(AxisBarOrientation), typeof(AxisBar), new UIPropertyMetadata(AxisBarOrientation.Vertical));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(AxisBar), new UIPropertyMetadata(""));

        public double BarWidth
        {
            get { return (double)GetValue(BarWidthProperty); }
            set { SetValue(BarWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BarWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BarWidthProperty =
            DependencyProperty.Register("BarWidth", typeof(double), typeof(AxisBar), new UIPropertyMetadata(30d));

        #endregion

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateFill();
        }

        private void UpdateFill()
        {
            if (Value < Minimum)
            {
                Value = Minimum;
            }
            else if (Value > Maximum)
            {
                Value = Maximum;
            }

            switch (Orientation)
            {
                case AxisBarOrientation.Vertical:
                    SetValue(FillHeightProperty, GetFillSize(ActualHeight));
                    SetValue(FillWidthProperty, ActualWidth);
                    break;

                case AxisBarOrientation.Horizontal:
                default:
                    SetValue(FillHeightProperty, ActualHeight);
                    SetValue(FillWidthProperty, GetFillSize(ActualWidth));
                    break;
            }
        }

        private double GetFillSize(double maximumSize)
        {
            double multiplier = ((double)Value - (double)Minimum) / ((double)Maximum - (double)Minimum);
            double fillSize = maximumSize * multiplier;
            return fillSize;
        }
    }
}

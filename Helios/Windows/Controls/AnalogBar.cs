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
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Custom control used to represent a input axis.
    /// </summary>
    public class AnalogBar : Control
    {
        public enum AnalogBarOrientation
        {
            Horizontal,
            Vertical
        }

        static AnalogBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalogBar), new FrameworkPropertyMetadata(typeof(AnalogBar)));
        }

        private static readonly PropertyChangedCallback _callback = new PropertyChangedCallback(FillPropertyChanged);

        private static void FillPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            AnalogBar bar = source as AnalogBar;

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
            DependencyProperty.Register("Fill", typeof(Brush), typeof(AnalogBar), new UIPropertyMetadata(new SolidColorBrush(Colors.DarkRed)));

        /// <summary>
        /// Height of the filled area.
        /// </summary>
        public double FillHeight
        {
            get { return (double)GetValue(FillHeightProperty); }
        }

        // Using a DependencyProperty as the backing store for FillHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillHeightProperty =
            DependencyProperty.Register("FillHeight", typeof(double), typeof(AnalogBar), new UIPropertyMetadata(0d));

        /// <summary>
        /// Width of the filled area.
        /// </summary>
        public double FillWidth
        {
            get { return (double)GetValue(FillWidthProperty); }
        }

        // Using a DependencyProperty as the backing store for FillWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FillWidthProperty =
            DependencyProperty.Register("FillWidth", typeof(double), typeof(AnalogBar), new UIPropertyMetadata(0d));

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
            DependencyProperty.Register("Minimum", typeof(int), typeof(AnalogBar), new UIPropertyMetadata(0, _callback));

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
            DependencyProperty.Register("Maximum", typeof(int), typeof(AnalogBar), new UIPropertyMetadata(65536, _callback));

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
            DependencyProperty.Register("Value", typeof(int), typeof(AnalogBar), new UIPropertyMetadata(65536, _callback));

        /// <summary>
        /// Orientation which the bar will be filled.
        /// </summary>
        public AnalogBarOrientation Orientation
        {
            get { return (AnalogBarOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(AnalogBar.AnalogBarOrientation), typeof(AnalogBar), new UIPropertyMetadata((object)AnalogBar.AnalogBarOrientation.Vertical));



        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(AnalogBar), new PropertyMetadata(""));


        public int BarWidth
        {
            get { return (int)GetValue(BarWidthProperty); }
            set { SetValue(BarWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BarWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BarWidthProperty =
            DependencyProperty.Register("BarWidth", typeof(int), typeof(AnalogBar), new PropertyMetadata(30));

       
        #endregion

        private void UpdateFill()
        {
            if (this.Value < this.Minimum)
                this.Value = this.Minimum;
            else if (this.Value > this.Maximum)
                this.Value = this.Maximum;
            switch (this.Orientation)
            {
                case AnalogBar.AnalogBarOrientation.Vertical:
                    this.SetValue(AnalogBar.FillHeightProperty, (object)this.GetFillSize(this.ActualHeight));
                    this.SetValue(AnalogBar.FillWidthProperty, (object)this.ActualWidth);
                    break;
                default:
                    this.SetValue(AnalogBar.FillHeightProperty, (object)this.ActualHeight);
                    this.SetValue(AnalogBar.FillWidthProperty, (object)this.GetFillSize(this.ActualWidth));
                    break;
            }
        }

        private double GetFillSize(double maximumSize)
        {
            double num = ((double)this.Value - (double)this.Minimum) / ((double)this.Maximum - (double)this.Minimum);
            return maximumSize * num;
        }
    }
}

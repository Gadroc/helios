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
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ProfilePreview : FrameworkElement
    {
        private class MonitorRectangle
        {
            public Rect DisplayRectangle;
            public Monitor Monitor;
            public HeliosVisualView View;
            public MonitorAdorner Adorner;
        }

        private VisualCollection _children;
        private bool _setAdorners = false;

        List<MonitorRectangle> _monitorRectangles = new List<MonitorRectangle>();

        private DrawingBrush _checkeredBrush;
        private Pen _borderPen;

        static ProfilePreview()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProfilePreview), new FrameworkPropertyMetadata(typeof(ProfilePreview)));
        }

        public ProfilePreview()
        {
            _children = new VisualCollection(this);

            DrawingGroup checkerGroup = new DrawingGroup();
            checkerGroup.Children.Add(new GeometryDrawing(Brushes.White, null, new RectangleGeometry(new Rect(0, 0, 100, 100))));

            DrawingGroup grayGroup = new DrawingGroup();
            grayGroup.Children.Add(new GeometryDrawing(Brushes.LightGray, null, new RectangleGeometry(new Rect(0, 0, 50, 50))));
            grayGroup.Children.Add(new GeometryDrawing(Brushes.LightGray, null, new RectangleGeometry(new Rect(50, 50, 50, 50))));

            checkerGroup.Children.Add(grayGroup);
            checkerGroup.Freeze();

            _checkeredBrush = new DrawingBrush(checkerGroup);
            _checkeredBrush.Viewport = new Rect(0, 0, 10, 10);
            _checkeredBrush.ViewportUnits = BrushMappingMode.Absolute;
            _checkeredBrush.TileMode = TileMode.Tile;
            _checkeredBrush.Freeze();

            _borderPen = new Pen(Brushes.Black, 1d);

            Focusable = false;
        }

        #region Properties

        public HeliosProfile Profile
        {
            get { return (HeliosProfile)GetValue(ProfileProperty); }
            set { SetValue(ProfileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Profile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProfileProperty =
            DependencyProperty.Register("Profile", typeof(HeliosProfile), typeof(ProfilePreview), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public bool ShowPanels
        {
            get { return (bool)GetValue(ShowPanelsProperty); }
            set { SetValue(ShowPanelsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowPanels.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowPanelsProperty =
            DependencyProperty.Register("ShowPanels", typeof(bool), typeof(ProfilePreview), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.None));

        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ZoomFactor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double), typeof(ProfilePreview), new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region Visual Methods

        protected override int VisualChildrenCount
        {
            get
            {
                return _children.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _children[index];
        }

        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            Size resultSize = new Size(0, 0);

            if (Profile != null)
            {
                resultSize.Width = double.IsPositiveInfinity(availableSize.Width) ? Math.Max(1d, Profile.Monitors.VirtualScreenWidth * ZoomFactor) : availableSize.Width;
                resultSize.Height = double.IsPositiveInfinity(availableSize.Height) ? Math.Max(1d, Profile.Monitors.VirtualScreenHeight * ZoomFactor) : availableSize.Height;

                double scale = Math.Min(resultSize.Height / Profile.Monitors.VirtualScreenHeight, resultSize.Width / Profile.Monitors.VirtualScreenWidth);

                resultSize.Width = Profile.Monitors.VirtualScreenWidth * scale;
                resultSize.Height = Profile.Monitors.VirtualScreenHeight * scale;

                foreach (HeliosVisualView child in _children)
                {
                    Size childSize = new Size(child.Visual.Width * scale,
                                             child.Visual.Height * scale);
                    child.Measure(childSize);
                }
            }

            return resultSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Profile != null)
            {
                Profile.Monitors.UpdateVirtualScreen();

                double scale = Math.Min(finalSize.Width / Profile.Monitors.VirtualScreenWidth, finalSize.Height / Profile.Monitors.VirtualScreenHeight);

                for(int i=0; i<_monitorRectangles.Count; i++)
                {
                    MonitorRectangle monitorRect = _monitorRectangles[i];
                    monitorRect.DisplayRectangle = new Rect(((monitorRect.Monitor.Left - Profile.Monitors.VirtualScreenLeft) * scale),
                                                            ((monitorRect.Monitor.Top - Profile.Monitors.VirtualScreenTop) * scale),
                                                            monitorRect.Monitor.Width * scale,
                                                            monitorRect.Monitor.Height * scale);
                    monitorRect.View.Arrange(monitorRect.DisplayRectangle);
                }
            }

            return finalSize;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ProfileProperty)
            {
                HeliosProfile oldProfile = e.OldValue as HeliosProfile;
                if (oldProfile != null)
                {
                    oldProfile.Monitors.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Monitors_CollectionChanged);
                }

                if (Profile != null)
                {
                    Profile.Monitors.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Monitors_CollectionChanged);
                }

                Dispatcher.Invoke(new Action(UpdateMonitors));
            }
            else if (e.Property == ShowPanelsProperty)
            {
                foreach (HeliosVisualView panelView in _children)
                {
                    panelView.Visibility = ShowPanels ? Visibility.Visible : Visibility.Hidden;
                }
            }

            base.OnPropertyChanged(e);
        }

        void Monitors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove || e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                foreach (Monitor monitor in e.OldItems)
                {
                    monitor.Resized -= Monitor_MoveResize;
                    monitor.Moved -= Monitor_MoveResize;
                }
            }

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add || e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                foreach (Monitor monitor in e.NewItems)
                {
                    monitor.Resized += Monitor_MoveResize;
                    monitor.Moved += Monitor_MoveResize;
                }
            }
            Dispatcher.BeginInvoke(new Action(UpdateMonitors));
        }

        private void Monitor_MoveResize(object sender, EventArgs e)
        {
            if (CheckAccess())
            {
                InvalidateAll();
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(InvalidateAll));
            }
        }

        private void InvalidateAll()
        {
            InvalidateArrange();
            InvalidateMeasure();
            InvalidateVisual();
        }

        private void UpdateMonitors()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
            foreach (MonitorRectangle monitorRect in _monitorRectangles)
            {
                if (layer != null)
                {
                    layer.Remove(monitorRect.Adorner);
                }
                _children.Remove(monitorRect.View);
            }

            _monitorRectangles.Clear();

            if (Profile != null)
            {
                _setAdorners = true;

                int i = 1;
                foreach (Monitor monitor in Profile.Monitors)
                {
                    HeliosVisualView monitorView = new HeliosVisualView();
                    monitorView.Visual = monitor;
                    monitorView.Visibility = ShowPanels ? Visibility.Visible : Visibility.Hidden;
                    _children.Add(monitorView);
                    
                    MonitorAdorner adorner = new MonitorAdorner(monitorView, i++.ToString(), monitor);                    

                    MonitorRectangle monitorRect = new MonitorRectangle();
                    monitorRect.Monitor = monitor;
                    monitorRect.View = monitorView;
                    monitorRect.Adorner = adorner;
                    _monitorRectangles.Add(monitorRect);
                }
            }
            InvalidateMeasure();
            InvalidateArrange();
            InvalidateVisual();
        }

        private void SetAdorners()
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this);
            if (layer != null)
            {
                foreach (MonitorRectangle monitorRect in _monitorRectangles)
                {
                    layer.Add(monitorRect.Adorner);
                }
            }
            _setAdorners = false;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (_setAdorners)
            {
                SetAdorners();
            }

            for (int index = 0; index < _monitorRectangles.Count; index++)
            {
                drawingContext.DrawRectangle(_checkeredBrush, _borderPen, _monitorRectangles[index].DisplayRectangle);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                Point position = e.GetPosition(this);
                for (int index = 0; index < _monitorRectangles.Count; index++)
                {
                    if (_monitorRectangles[index].DisplayRectangle.Contains(position))
                    {
                        if (ProfileEditorCommands.OpenProfileItem.CanExecute(_monitorRectangles[index].Monitor, this))
                        {
                            ProfileEditorCommands.OpenProfileItem.Execute(_monitorRectangles[index].Monitor, this);
                        }
                        break;
                    }
                }
                e.Handled = true;
            }
        }
        
    }
}

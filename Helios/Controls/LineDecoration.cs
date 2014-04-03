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

namespace GadrocsWorkshop.Helios.Controls
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Media;
    using System.Xml;

    [HeliosControl("Helios.Base.Line", "Line", "Panel Decorations", typeof(LineDecorationRenderer))]
    public class LineDecoration : HeliosVisual
    {
        private Point _start = new Point(0, 0);
        private Point _end = new Point(100, 0);
        private Color _lineColor = Colors.White;
        private double _thickness = 2d;
        private bool _curve = false;
        private double _curveDepth = 0.5d;
        private double _curveStart = 0.49d;

        private bool _recalcing = false;

        public LineDecoration()
            : base("Line", new System.Windows.Size(100, 1))
        {
        }

        #region Properties

        public Point Start
        {
            get
            {
                return _start;
            }
            set
            {
                if (!_start.Equals(value))
                {
                    Point oldValue = _start;
                    _start = value;
                    OnPropertyChanged("Start", oldValue, value, !_recalcing);

                    if (!_recalcing)
                    {
                        _recalcing = true;
                        UpdateSizeLocation();
                        _recalcing = false;
                    }

                    Refresh();
                }
            }
        }

        public Point End
        {
            get
            {
                return _end;
            }
            set
            {
                if (!_end.Equals(value))
                {
                    Point oldValue = _end;
                    _end = value;
                    OnPropertyChanged("End", oldValue, value, !_recalcing);

                    if (!_recalcing)
                    {
                        _recalcing = true;
                        UpdateSizeLocation();
                        _recalcing = false;
                    }

                    Refresh();
                }
            }
        }


        public double Thickness
        {
            get
            {
                return _thickness;
            }
            set
            {
                if (!_thickness.Equals(value))
                {
                    double oldValue = _thickness;
                    _thickness = value;
                    OnPropertyChanged("Thickness", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public Color LineColor
        {
            get
            {
                return _lineColor;
            }
            set
            {
                if (!_lineColor.Equals(value))
                {
                    Color oldValue = _lineColor;
                    _lineColor = value;
                    OnPropertyChanged("LineColor", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public bool Curve
        {
            get
            {
                return _curve;
            }
            set
            {
                if (!_curve.Equals(value))
                {
                    bool oldValue = _curve;
                    _curve = value;
                    OnPropertyChanged("Curve", oldValue, value, true);

                    if (!_recalcing)
                    {
                        _recalcing = true;
                        UpdateSizeLocation();
                        _recalcing = false;
                    }

                    Refresh();
                }
            }
        }

        public double CurveStart
        {
            get
            {
                return _curveStart;
            }
            set
            {
                if (!_curveStart.Equals(value))
                {
                    double oldValue = _curveStart;
                    _curveStart = value;
                    OnPropertyChanged("CurveStart", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public double CurveDepth
        {
            get
            {
                return _curveDepth;
            }
            set
            {
                if (!_curveDepth.Equals(value))
                {
                    double oldValue = _curveDepth;
                    _curveDepth = value;
                    OnPropertyChanged("CurveDepth", oldValue, value, true);
                    Refresh();
                }
            }
        }

        #endregion

        /// <summary>
        /// Calculate size and location based on start and end.
        /// </summary>
        private void UpdateSizeLocation()
        {
            Left = Math.Min(Start.X, End.X);
            Top = Math.Min(Start.Y, End.Y);
            Width = Math.Max(1, Math.Abs(Start.X - End.X));
            Height = Math.Max(1, Math.Abs(Start.Y - End.Y));
        }

        protected override void OnPropertyChanged(PropertyNotificationEventArgs args)
        {
            if (args.PropertyName.Equals("Left"))
            {
                if (!_recalcing)
                {
                    _recalcing = true;

                    double oldLocation = (double)args.OldValue;
                    double newLocation = (double)args.NewValue;

                    Start = new Point(Start.X + (newLocation - oldLocation),
                                       Start.Y);

                    End = new Point(End.X + (newLocation - oldLocation),
                                       End.Y);

                    _recalcing = false;
                }
            }

            else if (args.PropertyName.Equals("Top"))
            {
                if (!_recalcing)
                {
                    _recalcing = true;

                    double oldLocation = (double)args.OldValue;
                    double newLocation = (double)args.NewValue;

                    Start = new Point(Start.X,
                                       Start.Y + (newLocation - oldLocation));

                    End = new Point(End.X,
                                       End.Y + (newLocation - oldLocation));

                    _recalcing = false;
                }
            }

            else if (args.PropertyName.Equals("Width"))
            {
                if (!_recalcing)
                {
                    _recalcing = true;

                    double oldSize = (double)args.OldValue;
                    double newSize = (double)args.NewValue;

                    Point newStart = Start;
                    Point newEnd = End;

                    if (Start.X > End.X)
                    {
                        newStart.X += newSize - oldSize;
                    }
                    else if (Start.X < End.X)
                    {
                        newEnd.X += newSize - oldSize;
                    }

                    Start = newStart;
                    End = newEnd;

                    _recalcing = false;
                }
            }

            else if (args.PropertyName.Equals("Height"))
            {
                if (!_recalcing)
                {
                    _recalcing = true;

                    double oldSize = (double)args.OldValue;
                    double newSize = (double)args.NewValue;

                    Point newStart = Start;
                    Point newEnd = End;

                    if (Start.Y > End.Y)
                    {
                        newStart.Y += newSize - oldSize;
                    }
                    else if (Start.Y < End.Y)
                    {
                        newEnd.Y += newSize - oldSize;
                    }

                    Start = newStart;
                    End = newEnd;

                    _recalcing = false;
                }
            }

            base.OnPropertyChanged(args);
        }

        public override void ConfigureIconInstance()
        {
            Start = new Point(0,0);
            End = new Point(50, 0);
            LineColor = Color.FromRgb(30, 30, 30);
        }

        public override bool HitTest(Point location)
        {
            return false;
        }

        public override void MouseDown(System.Windows.Point location)
        {
            // No-Op
        }

        public override void MouseDrag(System.Windows.Point location)
        {
            // No-Op
        }

        public override void MouseUp(System.Windows.Point location)
        {
            // No-Op
        }

        public void Clone(LineDecoration item)
        {
            LineColor = item.LineColor;
            Thickness = item.Thickness;

            Start = item.Start;
            End = item.End;

            Curve = item.Curve;
            CurveDepth = item.CurveDepth;
            CurveStart = item.CurveStart;
        }

        public override void ReadXml(XmlReader reader)
        {
            TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));
            TypeConverter pointConverter = TypeDescriptor.GetConverter(typeof(Point));

            base.ReadXml(reader);

            LineColor = (Color)colorConverter.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("Color"));
            Thickness = Double.Parse(reader.ReadElementString("Thickness"), CultureInfo.InvariantCulture);
            Start = (Point)pointConverter.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("Start"));
            End = (Point)pointConverter.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("End"));

            if (reader.Name.Equals("Curve"))
            {
                Curve = true;
                reader.ReadStartElement("Curve");
                CurveStart = Double.Parse(reader.ReadElementString("Start"), CultureInfo.InvariantCulture);
                CurveDepth = Double.Parse(reader.ReadElementString("Depth"), CultureInfo.InvariantCulture);
                reader.ReadEndElement();
            }
            else
            {
                Curve = false;
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));
            TypeConverter pointConverter = TypeDescriptor.GetConverter(typeof(Point));

            base.WriteXml(writer);

            writer.WriteElementString("Color", colorConverter.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, LineColor));
            writer.WriteElementString("Thickness", Thickness.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Start", pointConverter.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, Start));
            writer.WriteElementString("End", pointConverter.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, End));
            
            if (Curve)
            {
                writer.WriteStartElement("Curve");
                writer.WriteElementString("Start", CurveStart.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Depth", CurveDepth.ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();
            }
        }
    }
}

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
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Xml;

    /// <summary>
    /// The struct that contains the display information
    /// </summary>
    public class Monitor : HeliosVisualContainer
    {
        private MonitorRenderer _renderer;

        private bool _fillBackground = true;
        private Color _backgroundColor = Colors.DarkGray;
        private string _backgroundImageFile = "";
        private ImageAlignment _backgroundAlignment = ImageAlignment.Stretched;
        private DisplayOrientation _orientation;
        private bool _alwaysOnTop = true;
        private int _suppressMouseAfterTouchDuration = 0;  

        public Monitor()
            : this(0, 0, 1024, 768, DisplayOrientation.DMDO_DEFAULT)
        {
        }

        public Monitor(double left, double top, double width, double height, DisplayOrientation orientation)
            : base("Monitor", new Size(width, height))
        {
            Top = top;
            Left = left;
            Width = width;
            Height = height;
            _orientation = orientation;
            if (Top == 0 && Left == 0)
            {
                _fillBackground = false;
            }
        }

        public Monitor(Monitor display)
            : this(display.Left, display.Top, display.Width, display.Height, display.Orientation)
        {
        }

        #region Properties

        public override string TypeIdentifier
        {
            get
            {
                return "Helios.Monitor";
            }
        }

        public double Right { get { return Left + Width; } }

        public double Bottom { get { return Top + Height; } }

        public DisplayOrientation Orientation { get { return _orientation; } set { _orientation = value; } }

        public override HeliosVisualRenderer Renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = new MonitorRenderer();
                    _renderer.Visual = this;
                }
                return _renderer;
            }
        }

        public bool FillBackground
        {
            get
            {
                return _fillBackground;
            }
            set
            {
                if (!_fillBackground.Equals(value))
                {
                    bool oldValue = _fillBackground;
                    _fillBackground = value;
                    OnPropertyChanged("FillBackground", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                if ((_backgroundColor == null && value != null)
                    || (_backgroundColor != null && !_backgroundColor.Equals(value)))
                {
                    Color oldValue = _backgroundColor;
                    _backgroundColor = value;
                    OnPropertyChanged("BackgroundColor", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public string BackgroundImage
        {
            get
            {
                return _backgroundImageFile;
            }
            set
            {
                if ((_backgroundImageFile == null && value != null)
                    || (_backgroundImageFile != null && !_backgroundImageFile.Equals(value)))
                {
                    string oldValue = _backgroundImageFile;
                    _backgroundImageFile = value;
                    OnPropertyChanged("BackgroundImage", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public ImageAlignment BackgroundAlignment
        {
            get
            {
                return _backgroundAlignment;
            }
            set
            {
                if (!_backgroundAlignment.Equals(value))
                {
                    ImageAlignment oldValue = _backgroundAlignment;
                    _backgroundAlignment = value;
                    OnPropertyChanged("BackgroundAlignment", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public bool AlwaysOnTop
        {
            get
            {
                return _alwaysOnTop;
            }
            set
            {
                if (!_alwaysOnTop.Equals(value))
                {
                    _alwaysOnTop = value;
                    OnPropertyChanged("AlwaysOnTop", !value, value, true);
                }
            }
        }

        public int SuppressMouseAfterTouchDuration { get => _suppressMouseAfterTouchDuration; set => _suppressMouseAfterTouchDuration = value; }

        #endregion

        public override void ReadXml(XmlReader reader)
        {
            TypeConverter cc = TypeDescriptor.GetConverter(typeof(Color));
            TypeConverter bc = TypeDescriptor.GetConverter(typeof(bool));
            TypeConverter ic = TypeDescriptor.GetConverter(typeof(int));

            base.ReadXml(reader);

            _orientation = (DisplayOrientation)Enum.Parse(typeof(DisplayOrientation), reader.ReadElementString("Orientation"));

            // REVISIT: this assumes the order of XML elements and also assumes that there are no foreign elements present
            // and that all properties are located just ahead of the "Children" element

            if (reader.Name.Equals("AlwaysOnTop"))
            {
                _alwaysOnTop = (bool)bc.ConvertFromInvariantString(reader.ReadElementString("AlwaysOnTop"));
            }

            if (reader.Name.Equals("SuppressMouseAfterTouchDuration"))
            {
                _suppressMouseAfterTouchDuration = (int)ic.ConvertFromInvariantString(reader.ReadElementString("SuppressMouseAfterTouchDuration"));
            }

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("Background");
                if (reader.Name.Equals("Image"))
                {
                    BackgroundImage = reader.ReadElementString("Image");
                    BackgroundAlignment = (ImageAlignment)Enum.Parse(typeof(ImageAlignment), reader.ReadElementString("ImageAlignment"));
                }
                else
                {
                    BackgroundImage = "";
                }
                if (reader.Name.Equals("Color"))
                {
                    BackgroundColor = (Color)cc.ConvertFromInvariantString(reader.ReadElementString("Color"));
                    FillBackground = true;
                }
                else
                {
                    FillBackground = false;
                }
                reader.ReadEndElement();
            }
            else
            {
                FillBackground = false;
                reader.Read();
            }
        }

        public override void WriteXml(XmlWriter writer)
        {
            TypeConverter cc = TypeDescriptor.GetConverter(typeof(Color));
            TypeConverter bc = TypeDescriptor.GetConverter(typeof(bool));
            TypeConverter ic = TypeDescriptor.GetConverter(typeof(int));

            base.WriteXml(writer);

            writer.WriteElementString("Orientation", Orientation.ToString());
            writer.WriteElementString("AlwaysOnTop", bc.ConvertToInvariantString(AlwaysOnTop));
            writer.WriteElementString("SuppressMouseAfterTouchDuration", ic.ConvertToInvariantString(_suppressMouseAfterTouchDuration));

            writer.WriteStartElement("Background");
            if (!string.IsNullOrWhiteSpace(BackgroundImage))
            {
                writer.WriteElementString("Image", BackgroundImage);
                writer.WriteElementString("ImageAlignment", BackgroundAlignment.ToString());
            }

            if (FillBackground)
            {
                writer.WriteElementString("Color", cc.ConvertToInvariantString(BackgroundColor));
            }
            writer.WriteEndElement();
        }

        public override bool HitTest(Point location)
        {
            if (FillBackground || !String.IsNullOrWhiteSpace(BackgroundImage))
            {
                return true;
            }
            else
            {
                return false;
            }
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

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
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Media;
    using System.Xml;

    [HeliosControl("Helios.Base.Text", "Label", "Panel Decorations", typeof(TextDecorationRenderer))]
    public class TextDecoration : HeliosVisual
    {
        HeliosValue _textValue;

        private TextFormat _format = new TextFormat();
        private string _text = "Label";
        private Color _fontColor = Colors.White;
        private Color _backgroundColor = Color.FromRgb(30,30,30);
        private bool _fillBackground = false;

        public TextDecoration()
            : base("Label", new Size(60, 20))
        {
            _format.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Format_PropertyChanged);

            _textValue = new HeliosValue(this, new BindingValue(_text), "", "Text", "Text for this label.", "", BindingValueUnits.Text);
            _textValue.Execute += new HeliosActionHandler(TextValue_Execute);
            Actions.Add(_textValue);
            Values.Add(_textValue);
        }

        #region Properties

        public TextFormat Format
        {
            get
            {
                return _format;
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if ((_text == null && value != null)
                    || (_text != null && !_text.Equals(value)))
                {
                    string oldValue = _text;
                    _text = value;
                    OnPropertyChanged("Text", oldValue, value, true);
                    OnDisplayUpdate();
                }
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
                    OnDisplayUpdate();
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

        public Color FontColor
        {
            get
            {
                return _fontColor;
            }
            set
            {
                if ((_fontColor == null && value != null)
                    || (_fontColor != null && !_fontColor.Equals(value)))
                {
                    Color oldValue = _fontColor;
                    _fontColor = value;
                    OnPropertyChanged("FontColor", oldValue, value, true);
                    Refresh();
                }
            }
        }

        #endregion

        void Format_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyNotificationEventArgs origArgs = e as PropertyNotificationEventArgs;
            if (origArgs != null)
            {
                OnPropertyChanged("Format", origArgs);
            }
            OnDisplayUpdate();
        }

        void TextValue_Execute(object action, HeliosActionEventArgs e)
        {
            _textValue.SetValue(e.Value, e.BypassCascadingTriggers);
            Text = e.Value.StringValue;
        }

        public override void ConfigureIconInstance()
        {
            FontColor = Color.FromRgb(30, 30, 30);
        }

        public override void ScaleChildren(double scaleX, double scaleY)
        {
            double scale = scaleX > scaleY ? scaleX : scaleY;
            Format.FontSize *= scale;
        }

        public override bool HitTest(Point location)
        {
            return FillBackground;
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

        public override void WriteXml(XmlWriter writer)
        {
            TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));
            TypeConverter boolConverter = TypeDescriptor.GetConverter(typeof(bool));

            writer.WriteElementString("FillBackground", boolConverter.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, FillBackground));
            writer.WriteElementString("BackgroundColor", colorConverter.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, BackgroundColor));
            writer.WriteElementString("FontColor", colorConverter.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, FontColor));

            writer.WriteStartElement("Font");
            _format.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteElementString("Text", Text);

            base.WriteXml(writer);
        }

        public override void ReadXml(XmlReader reader)
        {
            TypeConverter colorConverter = TypeDescriptor.GetConverter(typeof(Color));
            TypeConverter boolConverter = TypeDescriptor.GetConverter(typeof(bool));

            FillBackground = (bool)boolConverter.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("FillBackground"));
            BackgroundColor = (Color)colorConverter.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("BackgroundColor"));
            FontColor = (Color)colorConverter.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("FontColor"));

            reader.ReadStartElement("Font");
            _format.ReadXml(reader);
            reader.ReadEndElement();

            Text = reader.ReadElementString("Text");

            base.ReadXml(reader);
        }
    }
}

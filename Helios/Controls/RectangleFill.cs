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

    [HeliosControl("Helios.Base.RectangleFill", "Rectangle Fill", "Panel Decorations", typeof(RectangleFillRenderer))]
    public class RectangleFill : HeliosVisual
    {
        private double _value = 1d;
        private double _borderThickness = 0d;
        private double _cornerRadius = 0d;
        private bool _inverse_direction = false;
        private Color _borderColor = Colors.Black;
        private Color _fillColor = Colors.Red;
        private TypeConverter _colorConverter = TypeDescriptor.GetConverter(typeof(Color));
        private HeliosValue _fillValue;

       




        public RectangleFill()
            : base("Rectangle Fill", new Size(25, 100))
        {
            IsSnapTarget = false;
            HeliosAction fillColorAction = new HeliosAction(this, "", "fill color", "set", "Sets the fill color for the rectangle", "Color in the hex format #AARRGGBB.", BindingValueUnits.Text);
            fillColorAction.Execute += new HeliosActionHandler(FillColorAction_Execute);
            Actions.Add(fillColorAction);
            _fillValue = new HeliosValue(this, new BindingValue(0d), "", "Height", "Current value of the Fill Height. Value from 0 min to 1 max", "", BindingValueUnits.Numeric);
            _fillValue.Execute += new HeliosActionHandler(SetValue_Execute);
            Values.Add(_fillValue);
            Actions.Add(_fillValue);
        }

        void FillColorAction_Execute(object action, HeliosActionEventArgs e)
        {
            try 
            {
                FillColor = (Color)_colorConverter.ConvertFromInvariantString(e.Value.StringValue);
            }
            catch
            {
                ConfigManager.LogManager.LogWarning("Rectangle error converting color value. (Name=\"" + Name + "\", Value=\"" + e.Value.StringValue + "\")");
            }            
        }


        void SetValue_Execute(object action, HeliosActionEventArgs e)
        {
            try
            {
                BeginTriggerBypass(e.BypassCascadingTriggers);
                FillHeight = e.Value.DoubleValue;
                EndTriggerBypass(e.BypassCascadingTriggers);
            }
            catch
            {
                // No-op if the parse fails we won't set the position.
            }
        }



        #region Properties

        public Color FillColor
        {
            get
            {
                return _fillColor;
            }
            set
            {
                if ((_fillColor == null && value != null)
                    || (_fillColor != null && !_fillColor.Equals(value)))
                {
                    Color oldValue = _fillColor;
                    _fillColor = value;
                    OnPropertyChanged("FillColor", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                if ((_borderColor == null && value != null)
                    || (_borderColor != null && !_borderColor.Equals(value)))
                {
                    Color oldValue = _borderColor;
                    _borderColor = value;
                    OnPropertyChanged("BorderColor", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public double BorderThickness
        {
            get
            {
                return _borderThickness;
            }
            set
            {
                if (!_borderThickness.Equals(value))
                {
                    double oldValue = _borderThickness;
                    _borderThickness = value;
                    OnPropertyChanged("BorderThickness", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public double CornerRadius
        {
            get
            {
                return _cornerRadius;
            }
            set
            {
                if (!_cornerRadius.Equals(value))
                {
                    double oldValue = _cornerRadius;
                    _cornerRadius = value;
                    OnPropertyChanged("CornerRadius", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public bool Inverse_direction
        {
            get
            {
                return _inverse_direction;
            }
            set
            {
                if (!_inverse_direction.Equals(value))
                {
                    bool oldValue = _inverse_direction;
                    _inverse_direction = value;
                    OnPropertyChanged("Inverse_direction", oldValue, value, true);
                    Refresh();
                }
            }
        }
        #endregion

        public double FillHeight
        {
            get
            {
                return _value;
            }
            set
            {
                if (!_value.Equals(value) & value<=1)
                {
                    double oldValue = _value;
                    _value = value;
                    _fillValue.SetValue(new BindingValue(_value), BypassTriggers);
                    OnPropertyChanged("FillHeight", oldValue, value, true);
                    Refresh();
                }
            }
        }



        public override bool HitTest(Point location)
        {
            return (FillColor.A == 255);
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

        public override void ReadXml(XmlReader reader)
        {
            TypeConverter bc = TypeDescriptor.GetConverter(typeof(bool));
            FillColor = (Color)_colorConverter.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("Color"));
            if (reader.Name.Equals("CornerRadius"))
            {
                CornerRadius = Double.Parse(reader.ReadElementString("CornerRadius"), CultureInfo.InvariantCulture);
            }
            if (reader.Name.Equals("Border"))
            {
                reader.ReadStartElement("Border");
                BorderThickness = Double.Parse(reader.ReadElementString("Thickness"), CultureInfo.InvariantCulture);
                BorderColor = (Color)_colorConverter.ConvertFromString(null, System.Globalization.CultureInfo.InvariantCulture, reader.ReadElementString("Color"));
                reader.ReadEndElement();
            }
            else
            {
                BorderThickness = 0d;
            }
            if (reader.Name.Equals("Inverse_direction"))
            {
                _inverse_direction = (bool)bc.ConvertFromInvariantString(reader.ReadElementString("Inverse_direction"));
            }

            else
            {
                Inverse_direction = false;
            }
            base.ReadXml(reader);
        }

        public override void WriteXml(XmlWriter writer)
        {
            TypeConverter bc = TypeDescriptor.GetConverter(typeof(bool));
            writer.WriteElementString("Color", _colorConverter.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, FillColor));
            writer.WriteElementString("CornerRadius", CornerRadius.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Inverse_direction", bc.ConvertToInvariantString(Inverse_direction));
            if (BorderThickness != 0d)
            {
                writer.WriteStartElement("Border");
                writer.WriteElementString("Thickness", BorderThickness.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Color", _colorConverter.ConvertToString(null, System.Globalization.CultureInfo.InvariantCulture, BorderColor));
                writer.WriteEndElement();
            }
          
            base.WriteXml(writer);
        }
    }
}

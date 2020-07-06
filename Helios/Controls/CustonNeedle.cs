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
    using System;
    using System.Windows;

    public abstract class CustomNeedle : HeliosVisual
    {
        private string _knobImage;
        private double _rotation;

       

        private const double SWIPE_SENSITIVY_BASE = 45d;

      
        private CalibrationPointCollectionDouble _swipeCalibration;
        

        protected CustomNeedle(string name, Size defaultSize)
            : base(name, defaultSize)
        {
            _swipeCalibration = new CalibrationPointCollectionDouble(-1d, 2d, 1d, 0.5d);
            _swipeCalibration.Add(new CalibrationPointDouble(0.0d, 1d));
        }

        #region Properties

       

        public string KnobImage
        {
            get
            {
                return _knobImage;
            }
            set
            {
                if ((_knobImage == null && value != null)
                    || (_knobImage != null && !_knobImage.Equals(value)))
                {
                    string oldValue = _knobImage;
                    _knobImage = value;
                    OnPropertyChanged("KnobImage", oldValue, value, true);
                    Refresh();
                }
            }
        }

        public double KnobRotation
        {
            get
            {
                return _rotation;
            }
            protected set
            {
                if (!_rotation.Equals(value))
                {
                    double oldValue = _rotation;
                    _rotation = value % 360d;
                    OnPropertyChanged("KnobRotation", oldValue, value, false);
                    OnDisplayUpdate();
                }
            }
        }

     

      

        #endregion

  //      protected abstract void Pulse(bool increment);

        private Vector VectorFromCenter(Point devicePosition)
        {
            return devicePosition - new Point(DisplayRectangle.Width / 2, DisplayRectangle.Height / 2);
        }

        private double GetAngle(Point startPoint, Point endPoint)
        {
            return Vector.AngleBetween(VectorFromCenter(startPoint), VectorFromCenter(endPoint));
        }

        public override void MouseDown(Point location)
        {
           
        }

      void Profile_ProfileTick(object sender, EventArgs e)
      {
        
      }

       public override void MouseDrag(Point location)
       {
        
       }

       public override void MouseUp(Point location)
       {
         
       }   
    }
}

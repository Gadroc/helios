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

namespace GadrocsWorkshop.Helios.Gauges
{
    using System;
    using System.Windows.Media;

    public abstract class GaugeComponent
    {
        private Geometry _clip;
        private Geometry _renderClip;
        private bool _hidden = false;

        public event EventHandler DisplayUpdate;

        #region Properties

        public Geometry Clip
        {
            get
            {
                return _clip;
            }
            set
            {
                _clip = value;
                _clip.Freeze();
            }
        }


        public bool IsHidden
        {
            get
            {
                return _hidden;
            }
            set
            {
                if (!_hidden.Equals(value))
                {
                    _hidden = value;
                    OnDisplayUpdate();
                }
            }
        }

        #endregion

        protected void OnDisplayUpdate()
        {
            EventHandler handler = DisplayUpdate;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Render(DrawingContext drawingContext)
        {
            if (!_hidden)
            {
                if (_renderClip != null)
                {
                    drawingContext.PushClip(_renderClip);
                }
                OnRender(drawingContext);
                if (_renderClip != null)
                {
                    drawingContext.Pop();
                }
            }
        }

        protected abstract void OnRender(DrawingContext drawingContext);

        public void Refresh(double xScale, double yScale)
        {
            if (Clip != null)
            {
                _renderClip = Clip.CloneCurrentValue();
                _renderClip.Transform = new ScaleTransform(xScale, yScale);
            }
            else
            {
                _renderClip = null;
            }
            OnRefresh(xScale, yScale);
        }

        protected abstract void OnRefresh(double xScale, double yScale);
    }
}

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

namespace GadrocsWorkshop.Helios.Gauges.AV8B.SlipBall
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.AV8B.SlipBall", "SlipBall", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class SlipBall : BaseGauge
    {
        private HeliosValue _slipBall;

        private GaugeNeedle _slipBallNeedle;
        private GaugeNeedle _slipBallMarker;

        private CalibrationPointCollectionDouble _slipBallCalibration;

        public SlipBall()
            : base("SlipBall", new Size(225, 114))
        {
            Point center = new Point(112d, 57d);

            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/ADI/adi_slip_bezel.png", new Rect(0d, 0d, 225d, 114d)));
            //Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/ADI/adi_inner_ring.xaml", new Rect(0d, 0d, 350d, 350d)));

            _slipBallCalibration = new CalibrationPointCollectionDouble(-1d, -50d, 1d, 50d);
            _slipBallNeedle = new GaugeNeedle("{Helios}/Gauges/AV-8B/ADI/adi_slip_ball.xaml", new Point(112d, 75d), new Size(24d, 24d), new Point(12d, 12d));
            _slipBallMarker = new GaugeNeedle("{Helios}/Gauges/AV-8B/ADI/adi_slip_marker.xaml", new Point(112d, 25d), new Size(14d, 18d), new Point(7d, 9d));
            Components.Add(_slipBallNeedle);
            Components.Add(_slipBallMarker);

            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/ADI/adi_slip_guides.xaml", new Rect(0d, 0d, 225d, 114d)));

           // Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/ADI/adi_outer_ring.xaml", new Rect(0d, 0d, 350d, 350d)));



            _slipBall = new HeliosValue(this, new BindingValue(0d), "", "Slip Ball Offset", "Side slip indicator offset from the center of the tube.", "(-1 to 1) -1 full left and 1 is full right.", BindingValueUnits.Numeric);
            _slipBall.Execute += new HeliosActionHandler(SlipBall_Execute);
            Actions.Add(_slipBall);

        }

         void SlipBall_Execute(object action, HeliosActionEventArgs e)
        {
            _slipBall.SetValue(e.Value, e.BypassCascadingTriggers);
            _slipBallNeedle.HorizontalOffset = _slipBallCalibration.Interpolate(e.Value.DoubleValue);
            _slipBallMarker.HorizontalOffset = _slipBallCalibration.Interpolate(e.Value.DoubleValue);
        }

    }
}

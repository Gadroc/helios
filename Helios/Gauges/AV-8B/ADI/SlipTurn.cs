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

    [HeliosControl("Helios.AV8B.SlipTurn", "AV-8B Slip Turn Indicator", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class SlipTurn : BaseGauge
    {
        private HeliosValue _slipBall;
        private HeliosValue _turnIndicator;

        private GaugeNeedle _slipBallNeedle;
        private GaugeNeedle _TurnMarker;

        private HeliosValue _warningFlag;
        private GaugeNeedle _warningFlagNeedle;

        private CalibrationPointCollectionDouble _slipBallCalibration;

        public SlipTurn()
            : base("SlipTurn", new Size(225, 114))
        {
            Point center = new Point(112d, 57d);

            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/ADI/adi_slip_bezel.png", new Rect(0d, 0d, 225d, 114d)));

            _slipBallCalibration = new CalibrationPointCollectionDouble(-1d, -50d, 1d, 50d);
            _slipBallNeedle = new GaugeNeedle("{Helios}/Gauges/AV-8B/ADI/adi_slip_ball.xaml", new Point(112d, 75d), new Size(24d, 24d), new Point(12d, 12d));
            _TurnMarker = new GaugeNeedle("{Helios}/Gauges/AV-8B/ADI/adi_turn_marker.xaml", new Point(112d, 38d), new Size(14d, 18d), new Point(7d, 9d));
            Components.Add(_slipBallNeedle);
            Components.Add(_TurnMarker);

            _warningFlagNeedle = new GaugeNeedle("{Helios}/Gauges/AV-8B/AOA/aoa_off_flag.xaml", new Point(80d, 20d), new Size(28d, 36d), new Point(0d, 0d), 0d);
            Components.Add(_warningFlagNeedle);


            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/ADI/adi_slip_guides.xaml", new Rect(0d, 0d, 225d, 114d)));

            _slipBall = new HeliosValue(this, new BindingValue(0d), "Flight Instruments", "Slip Ball Offset", "Side slip indicator offset from the center of the tube.", "(-1 to 1) -1 full left and 1 is full right.", BindingValueUnits.Numeric);
            _slipBall.Execute += new HeliosActionHandler(SlipBall_Execute);
            Actions.Add(_slipBall);

            _turnIndicator = new HeliosValue(this, new BindingValue(0d), "Flight Instruments", "Turn Indicator Offset", "Turn indicator offset from the center of the gauge.", "(-1 to 1) -1 full left and 1 is full right.", BindingValueUnits.Numeric);
            _turnIndicator.Execute += new HeliosActionHandler(turnIndicator_Execute);
            Actions.Add(_turnIndicator);
        
            _warningFlag = new HeliosValue(this, new BindingValue(false), "Flight Instruments", "Slip Turn Warning Flag", "Indicates whether the warning flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _warningFlag.Execute += new HeliosActionHandler(OffFlag_Execute);
            Actions.Add(_warningFlag);
        }

        void SlipBall_Execute(object action, HeliosActionEventArgs e)
        {
            _slipBall.SetValue(e.Value, e.BypassCascadingTriggers);
            _slipBallNeedle.HorizontalOffset = _slipBallCalibration.Interpolate(e.Value.DoubleValue);
        }
        void turnIndicator_Execute(object action, HeliosActionEventArgs e)
        {
            _turnIndicator.SetValue(e.Value, e.BypassCascadingTriggers);
            _TurnMarker.HorizontalOffset = _slipBallCalibration.Interpolate(e.Value.DoubleValue);
        }
        void OffFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _warningFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _warningFlagNeedle.Rotation = e.Value.BoolValue ? -90 : 0;
            _warningFlagNeedle.IsHidden = e.Value.BoolValue;
        }

    }
}

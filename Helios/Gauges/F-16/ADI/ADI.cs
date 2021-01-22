﻿//  Copyright 2014 Craig Courtney
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

namespace GadrocsWorkshop.Helios.Gauges.F_16.ADI
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.F16.ADI", "ADI", "F-16", typeof(GaugeRenderer))]
    public class ADI : BaseGauge
    {
        private HeliosValue _pitch;
        private HeliosValue _roll;
        private HeliosValue _slipBall;
        private HeliosValue _ilsHorizontal;
        private HeliosValue _ilsVertical;

        private HeliosValue _auxFlag;
        private HeliosValue _offFlag;
        private HeliosValue _gsFlag;
        private HeliosValue _locFlag;

        private GaugeImage _auxFlagImage;
        private GaugeImage _offFlagImage;
        private GaugeImage _gsFlagImage;
        private GaugeImage _locFlagImage;

        private GaugeNeedle _ball;
        private GaugeNeedle _slipBallNeedle;
        private GaugeNeedle _ilsHorizontalNeedle;
        private GaugeNeedle _ilsVerticalNeedle;
        private GaugeNeedle _ilsScaleNeedle;

        private CalibrationPointCollectionDouble _pitchCalibration;
        private CalibrationPointCollectionDouble _ilsCalibration;
        private CalibrationPointCollectionDouble _slipBallCalibration;

        public ADI()
            : base("ADI", new Size(350, 350))
        {

            _pitchCalibration = new CalibrationPointCollectionDouble(-360d, -1066d, 360d, 1066d);
            _ball = new GaugeNeedle("{Helios}/Gauges/F-16/ADI/adi_ball.xaml", new Point(175d, 175d), new Size(225d, 1350d), new Point(112.5d, 677d));
            _ball.Clip = new EllipseGeometry(new Point(175d, 175d), 113d, 113d);
            Components.Add(_ball);

            _ilsCalibration = new CalibrationPointCollectionDouble(-1d, -116d, 1d, 116d);

            _ilsHorizontalNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/ADI/adi_ils_bar.xaml", new Point(175d, 175d), new Size(189d, 6d), new Point(95d, 4d), 90d);
            _ilsHorizontalNeedle.VerticalOffset = _ilsCalibration.Interpolate(1d);
            Components.Add(_ilsHorizontalNeedle);

            _ilsVerticalNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/ADI/adi_ils_bar.xaml", new Point(175d, 175d), new Size(189d, 6d), new Point(95d, 4d));
            _ilsVerticalNeedle.VerticalOffset = _ilsCalibration.Interpolate(1d);
            Components.Add(_ilsVerticalNeedle);

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/ADI/adi_inner_ring.xaml", new Rect(0d, 0d, 350d, 350d)));

            _auxFlagImage = new GaugeImage("{Helios}/Gauges/F-16/ADI/adi_aux_flag.xaml", new Rect(0d, 0d, 350d, 350d));
            _auxFlagImage.IsHidden = true;
            Components.Add(_auxFlagImage);

            _offFlagImage = new GaugeImage("{Helios}/Gauges/F-16/ADI/adi_off_flag.xaml", new Rect(0d, 0d, 350d, 350d));
            _offFlagImage.IsHidden = true;
            Components.Add(_offFlagImage);

            _gsFlagImage = new GaugeImage("{Helios}/Gauges/F-16/ADI/adi_gs_flag.xaml", new Rect(0d, 0d, 350d, 350d));
            _gsFlagImage.IsHidden = true;
            Components.Add(_gsFlagImage);

            _locFlagImage = new GaugeImage("{Helios}/Gauges/F-16/ADI/adi_loc_flag.xaml", new Rect(0d, 0d, 350d, 350d));
            _locFlagImage.IsHidden = true;
            Components.Add(_locFlagImage);

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/ADI/adi_outer_ring.xaml", new Rect(0d, 0d, 350d, 350d)));

            _ilsScaleNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/ADI/adi_ils_scale_needle.xaml", new Point(30d, 174d), new Size(14d, 12d), new Point(1d, 6d));
            Components.Add(_ilsScaleNeedle);

            _slipBallCalibration = new CalibrationPointCollectionDouble(-1d, -26d, 1d, 26d);

            _slipBallNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/ADI/adi_slip_ball.xaml", new Point(175d, 313d), new Size(10d, 10d), new Point(5d, 5d));
            Components.Add(_slipBallNeedle);

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/ADI/adi_guides.xaml", new Rect(0d, 0d, 350d, 350d)));

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/ADI/adi_bezel.png", new Rect(0d, 0d, 350d, 350d)));

            _slipBall = new HeliosValue(this, new BindingValue(0d), "", "side slip", "Side slip indicator offset.", "-1 to 1", BindingValueUnits.Numeric);
            _slipBall.Execute += new HeliosActionHandler(SlipBall_Execute);
            Actions.Add(_slipBall);

            _auxFlag = new HeliosValue(this, new BindingValue(false), "", "aux flag", "Indicates whether the aux flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _auxFlag.Execute += new HeliosActionHandler(AuxFlag_Execute);
            Actions.Add(_auxFlag);

            _offFlag = new HeliosValue(this, new BindingValue(false), "", "off flag", "Indicates whether the off flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _offFlag.Execute += new HeliosActionHandler(OffFlag_Execute);
            Actions.Add(_offFlag);

            _gsFlag = new HeliosValue(this, new BindingValue(false), "", "loc flag", "Indicates whether the loc flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _gsFlag.Execute += new HeliosActionHandler(GsFlag_Execute);
            Actions.Add(_gsFlag);

            _locFlag = new HeliosValue(this, new BindingValue(false), "", "gs flag", "Indicates whether the gs flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _locFlag.Execute += new HeliosActionHandler(LocFlag_Execute);
            Actions.Add(_locFlag);

            _pitch = new HeliosValue(this, new BindingValue(0d), "", "pitch", "Current ptich of the aircraft.", "(0 - 360)", BindingValueUnits.Degrees);
            _pitch.Execute += new HeliosActionHandler(Pitch_Execute);
            Actions.Add(_pitch);

            _roll = new HeliosValue(this, new BindingValue(0d), "", "roll", "Current roll of the aircraft.", "(0 - 360)", BindingValueUnits.Degrees);
            _roll.Execute += new HeliosActionHandler(Roll_Execute);
            Actions.Add(_roll);

            _ilsHorizontal = new HeliosValue(this, new BindingValue(1d), "", "ils horizontal deviation", "Current deviation from glide scope.", "-1 to 1", BindingValueUnits.Numeric);
            _ilsHorizontal.Execute += new HeliosActionHandler(ILSHorizontal_Execute);
            Actions.Add(_ilsHorizontal);

            _ilsVertical = new HeliosValue(this, new BindingValue(1d), "", "ils vertical deviation", "Current deviation from ILS side scope.", "-1 to 1", BindingValueUnits.Numeric);
            _ilsVertical.Execute += new HeliosActionHandler(ILSVertical_Execute);
            Actions.Add(_ilsVertical);
        }

        void SlipBall_Execute(object action, HeliosActionEventArgs e)
        {
            _slipBall.SetValue(e.Value, e.BypassCascadingTriggers);
            _slipBallNeedle.HorizontalOffset = _slipBallCalibration.Interpolate(e.Value.DoubleValue);
        }

        void AuxFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _auxFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _auxFlagImage.IsHidden = !e.Value.BoolValue;
        }

        void GsFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _gsFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _gsFlagImage.IsHidden = !e.Value.BoolValue;
        }

        void LocFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _locFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _locFlagImage.IsHidden = !e.Value.BoolValue;
        }

        void OffFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _offFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _offFlagImage.IsHidden = !e.Value.BoolValue;
        }

        void ILSVertical_Execute(object action, HeliosActionEventArgs e)
        {
            _ilsVertical.SetValue(e.Value, e.BypassCascadingTriggers);
            _ilsVerticalNeedle.VerticalOffset = _ilsCalibration.Interpolate(e.Value.DoubleValue);
        }

        void ILSHorizontal_Execute(object action, HeliosActionEventArgs e)
        {
            _ilsHorizontal.SetValue(e.Value, e.BypassCascadingTriggers);
            _ilsHorizontalNeedle.VerticalOffset = _ilsCalibration.Interpolate(e.Value.DoubleValue);
        }

        void Pitch_Execute(object action, HeliosActionEventArgs e)
        {
            _ball.VerticalOffset = _pitchCalibration.Interpolate(e.Value.DoubleValue);
        }

        void Roll_Execute(object action, HeliosActionEventArgs e)
        {
            _ball.Rotation = -e.Value.DoubleValue;
        }
    }
}

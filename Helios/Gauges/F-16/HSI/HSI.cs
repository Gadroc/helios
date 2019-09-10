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

namespace GadrocsWorkshop.Helios.Gauges.F_16.HSI
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.F16.HSI", "HSI", "F-16", typeof(GaugeRenderer))]
    public class HSI : BaseGauge
    {
        private HeliosValue _bearing;
        private HeliosValue _desiredHeading;
        private HeliosValue _desiredCourse;
        private HeliosValue _currentHeading;
        private HeliosValue _distance;
        private HeliosValue _courseDeviation;
        private HeliosValue _offFlag;
        private HeliosValue _courseFlag;
        private HeliosValue _dmeFlag;
        private HeliosValue _toFlag;
        private HeliosValue _fromFlag;

        private GaugeImage _offFlagImage;
        private GaugeNeedle _courseFlagNeedle;
        private GaugeNeedle _toFlagNeedle;
        private GaugeNeedle _fromFlagNeedle;
        private GaugeImage _dmeFlagImage;
        private GaugeNeedle _deviationNeedle;
        private GaugeNeedle _deviationCard;
        private GaugeNeedle _desiredCourseNeedle;
        private GaugeNeedle _headingBug;
        private GaugeNeedle _compassNeedle;
        private GaugeDrumCounter _milesDrum;
        private GaugeDrumCounter _courseDrum;
        private GaugeNeedle _bearingNeedle;

        private CalibrationPointCollectionDouble _deviationCallibration;

        public HSI()
            : base("HSI", new Size(350, 350))
        {

            Components.Add(new GaugeRectangle(Colors.Black, new Rect(5, 5, 340, 340)));

            _milesDrum = new GaugeDrumCounter("{Helios}/Gauges/F-16/Common/drum_tape.xaml", new Point(46, 60), "##%", new Size(10d, 15d), new Size(10d, 15d));
            _milesDrum.Clip = new RectangleGeometry(new Rect(46d, 60d, 30d, 15d));
            Components.Add(_milesDrum);

            _courseDrum = new GaugeDrumCounter("{Helios}/Gauges/F-16/Common/drum_tape.xaml", new Point(279, 60), "##%", new Size(10d, 15d), new Size(10d, 15d));
            _courseDrum.Clip = new RectangleGeometry(new Rect(279d, 60d, 30d, 15d));
            Components.Add(_courseDrum);

            _compassNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/HSI/hsi_compass.xaml", new Point(175, 184), new Size(243, 243), new Point(121.5, 121.5));
            Components.Add(_compassNeedle);

            _dmeFlagImage = new GaugeImage("{Helios}/Gauges/F-16/HSI/hsi_dme_flag.xaml", new Rect(35, 64, 37, 10));
            _dmeFlagImage.IsHidden = true;
            Components.Add(_dmeFlagImage);

            _courseFlagNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/HSI/hsi_course_flag.xaml", new Point(175, 184), new Size(350, 350), new Point(175, 184));
            _courseFlagNeedle.IsHidden = true;
            Components.Add(_courseFlagNeedle);

            _toFlagNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/HSI/hsi_to_flag.xaml", new Point(175, 184), new Size(350, 350), new Point(175, 184));
            _toFlagNeedle.IsHidden = true;
            Components.Add(_toFlagNeedle);

            _fromFlagNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/HSI/hsi_from_flag.xaml", new Point(175, 184), new Size(350, 350), new Point(175, 184));
            _fromFlagNeedle.IsHidden = true;
            Components.Add(_fromFlagNeedle);

            _deviationCard = new GaugeNeedle("{Helios}/Gauges/F-16/HSI/hsi_deviation_card.xaml", new Point(175, 184), new Size(183, 183), new Point(91.5, 91.5));
            Components.Add(_deviationCard);

            _deviationNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/HSI/hsi_deviation_needle.xaml", new Point(175, 184), new Size(4, 149), new Point(1.5, 74.5));
            Components.Add(_deviationNeedle);

            _desiredCourseNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/HSI/hsi_course_needle_f16.xaml", new Point(175, 184), new Size(12, 207), new Point(6, 103.5));
            Components.Add(_desiredCourseNeedle);

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/HSI/hsi_faceplate.xaml", new Rect(0, 0, 350, 350)));

            _headingBug = new GaugeNeedle("{Helios}/Gauges/F-16/HSI/hsi_heading_bug.xaml", new Point(175, 184), new Size(22, 9), new Point(11, 130.5));
            Components.Add(_headingBug);

            _bearingNeedle = new GaugeNeedle("{Helios}/Gauges/F-16/HSI/hsi_bearing_needle_f16.xaml", new Point(175, 184), new Size(11, 256), new Point(6, 128));
            Components.Add(_bearingNeedle);

            _offFlagImage = new GaugeImage("{Helios}/Gauges/F-16/HSI/hsi_off_flag.xaml", new Rect(246, 116, 77, 131));
            _offFlagImage.IsHidden = true;
            Components.Add(_offFlagImage);

            Components.Add(new GaugeImage("{Helios}/Gauges/F-16/HSI/hsi_bezel.png", new Rect(0, 0, 350, 350)));

            _offFlag = new HeliosValue(this, new BindingValue(false), "", "off flag", "Indicates whether the off flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _offFlag.Execute += new HeliosActionHandler(OffFlag_Execute);
            Actions.Add(_offFlag);

            _courseFlag = new HeliosValue(this, new BindingValue(false), "", "course flag", "Indicates whether the course flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _courseFlag.Execute += new HeliosActionHandler(CourseFlag_Execute);
            Actions.Add(_courseFlag);

            _dmeFlag = new HeliosValue(this, new BindingValue(false), "", "dme flag", "Indicates whether the dme flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _dmeFlag.Execute += new HeliosActionHandler(DMEFlag_Execute);
            Actions.Add(_dmeFlag);

            _toFlag = new HeliosValue(this, new BindingValue(false), "", "to flag", "Indicates whether the to flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _toFlag.Execute += new HeliosActionHandler(ToFlag_Execute);
            Actions.Add(_toFlag);

            _fromFlag = new HeliosValue(this, new BindingValue(false), "", "from flag", "Indicates whether the from flag is displayed.", "True if displayed.", BindingValueUnits.Boolean);
            _fromFlag.Execute += new HeliosActionHandler(FromFlag_Execute);
            Actions.Add(_fromFlag);

            _distance = new HeliosValue(this, new BindingValue(0d), "", "distance to beacon", "Miles to the beacon.", "", BindingValueUnits.NauticalMiles);
            _distance.Execute += new HeliosActionHandler(Distance_Execute);
            Actions.Add(_distance);

            _bearing = new HeliosValue(this, new BindingValue(0d), "", "bearing", "Bearing to the beacon.", "(0 - 360)", BindingValueUnits.Degrees);
            _bearing.Execute += new HeliosActionHandler(Bearing_Execute);
            Actions.Add(_bearing);

            _desiredHeading = new HeliosValue(this, new BindingValue(0d), "", "desired heading", "Current desired heading.", "(0 - 360)", BindingValueUnits.Degrees);
            _desiredHeading.Execute += new HeliosActionHandler(DesiredHeading_Execute);
            Actions.Add(_desiredHeading);

            _desiredCourse = new HeliosValue(this, new BindingValue(0d), "", "desired course", "Current desired course.", "(0 - 360)", BindingValueUnits.Degrees);
            _desiredCourse.Execute += new HeliosActionHandler(DesiredCourse_Execute);
            Actions.Add(_desiredCourse);

            _courseDeviation = new HeliosValue(this, new BindingValue(0d), "", "course deviation", "Deviation from course.", "(-1 - 1)", BindingValueUnits.Numeric);
            _courseDeviation.Execute += new HeliosActionHandler(Deviation_Execute);
            Actions.Add(_courseDeviation);

            _currentHeading = new HeliosValue(this, new BindingValue(0d), "", "heading", "Current magnetic heading.", "(0 - 360)", BindingValueUnits.Degrees);
            _currentHeading.Execute += new HeliosActionHandler(CurrentHeading_Execute);
            Actions.Add(_currentHeading);

            _deviationCallibration = new CalibrationPointCollectionDouble(-1d, -70d, 1d, 70d);
        }

        void OffFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _offFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _offFlagImage.IsHidden = !e.Value.BoolValue;
        }

        void DMEFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _dmeFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _dmeFlagImage.IsHidden = !e.Value.BoolValue;
        }

        void CourseFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _courseFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _courseFlagNeedle.IsHidden = !e.Value.BoolValue;
        }

        void ToFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _toFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _toFlagNeedle.IsHidden = !e.Value.BoolValue;
        }

        void FromFlag_Execute(object action, HeliosActionEventArgs e)
        {
            _fromFlag.SetValue(e.Value, e.BypassCascadingTriggers);
            _fromFlagNeedle.IsHidden = !e.Value.BoolValue;
        }

        void Deviation_Execute(object action, HeliosActionEventArgs e)
        {
            _courseDeviation.SetValue(e.Value, e.BypassCascadingTriggers);
            _deviationNeedle.HorizontalOffset = -_deviationCallibration.Interpolate(_courseDeviation.Value.DoubleValue);
        }

        void Bearing_Execute(object action, HeliosActionEventArgs e)
        {
            _bearing.SetValue(new BindingValue(ClampDegrees(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            _bearingNeedle.Rotation = _bearing.Value.DoubleValue + _compassNeedle.Rotation;
        }

        void Distance_Execute(object action, HeliosActionEventArgs e)
        {
            _distance.SetValue(e.Value, e.BypassCascadingTriggers);
            _milesDrum.Value = e.Value.DoubleValue;
        }

        void DesiredHeading_Execute(object action, HeliosActionEventArgs e)
        {
            _desiredHeading.SetValue(new BindingValue(ClampDegrees(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            _headingBug.Rotation = _desiredHeading.Value.DoubleValue + _compassNeedle.Rotation;
        }

        void DesiredCourse_Execute(object action, HeliosActionEventArgs e)
        {
            _desiredCourse.SetValue(new BindingValue(ClampDegrees(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            _desiredCourseNeedle.Rotation = _desiredCourse.Value.DoubleValue + _compassNeedle.Rotation;
            _deviationNeedle.Rotation = _desiredCourseNeedle.Rotation;
            _deviationCard.Rotation = _desiredCourseNeedle.Rotation;
            _courseDrum.Value = _desiredCourse.Value.DoubleValue;
            _courseFlagNeedle.Rotation = _deviationCard.Rotation;
            _toFlagNeedle.Rotation = _deviationCard.Rotation;
            _fromFlagNeedle.Rotation = _deviationCard.Rotation;
        }

        void CurrentHeading_Execute(object action, HeliosActionEventArgs e)
        {
            _currentHeading.SetValue(new BindingValue(ClampDegrees(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            _compassNeedle.Rotation = -_currentHeading.Value.DoubleValue;
            _headingBug.Rotation = _desiredHeading.Value.DoubleValue + _compassNeedle.Rotation;
            _bearingNeedle.Rotation = _bearing.Value.DoubleValue + _compassNeedle.Rotation;
            _desiredCourseNeedle.Rotation = _desiredCourse.Value.DoubleValue + _compassNeedle.Rotation;
            _deviationNeedle.Rotation = _desiredCourseNeedle.Rotation;
            _deviationCard.Rotation = _desiredCourseNeedle.Rotation;
            _courseFlagNeedle.Rotation = _deviationCard.Rotation;
            _toFlagNeedle.Rotation = _deviationCard.Rotation;
            _fromFlagNeedle.Rotation = _deviationCard.Rotation;
        }

        private double ClampDegrees(double input)
        {
            while (input < 0d)
            {
                input += 360d;
            }
            while (input > 360d)
            {
                input -= 360d;
            }
            return input;
        }
    }
}

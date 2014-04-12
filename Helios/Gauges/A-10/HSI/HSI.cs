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

namespace GadrocsWorkshop.Helios.Gauges.A_10.HSI
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.A10.HSI", "HSI", "A-10 Gauges", typeof(GaugeRenderer))]
    public class HSI : BaseGauge
    {
        private HeliosValue _bearing1;
        private HeliosValue _bearing2;
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
        private GaugeNeedle _bearingNeedle2;

        private CalibrationPointCollectionDouble _deviationCallibration;

        private bool _relativeDesiredHeading = true;
        private bool _relativeBearing = true;
        private bool _relativeCourse = true;

        public HSI()
            : base("HSI", new Size(350, 350))
        {
            Point center = new Point(175, 170);

            Components.Add(new GaugeRectangle(Colors.Black, new Rect(10, 10, 330, 330)));

            _milesDrum = new GaugeDrumCounter("{Helios}/Gauges/A-10/Common/drum_tape.xaml", new Point(28, 68), "##%", new Size(10d, 15d), new Size(13d, 18d));
            _milesDrum.Clip = new RectangleGeometry(new Rect(28d, 60d, 39d, 30d));
            Components.Add(_milesDrum);

            _courseDrum = new GaugeDrumCounter("{Helios}/Gauges/A-10/Common/drum_tape.xaml", new Point(282, 68), "##%", new Size(10d, 15d), new Size(13d, 18d));
            _courseDrum.Clip = new RectangleGeometry(new Rect(282d, 68d, 39d, 18d));
            Components.Add(_courseDrum);

            _compassNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_compass.xaml", center, new Size(243, 243), new Point(121.5, 121.5));
            Components.Add(_compassNeedle);

            _dmeFlagImage = new GaugeImage("{Helios}/Gauges/A-10/HSI/hsi_range_flag.xaml", new Rect(27, 74, 37, 10));
            _dmeFlagImage.IsHidden = true;
            Components.Add(_dmeFlagImage);

            _courseFlagNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_bearing_flag.xaml", center, new Size(350, 350), center);
            _courseFlagNeedle.IsHidden = true;
            Components.Add(_courseFlagNeedle);

            _toFlagNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_to_flag.xaml", center, new Size(350, 350), center);
            _toFlagNeedle.IsHidden = true;
            Components.Add(_toFlagNeedle);

            _fromFlagNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_from_flag.xaml", center, new Size(350, 350), center);
            _fromFlagNeedle.IsHidden = true;
            Components.Add(_fromFlagNeedle);

            _deviationCard = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_deviation_card.xaml", center, new Size(183, 183), new Point(91.5, 91.5));
            Components.Add(_deviationCard);

            _deviationNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_deviation_needle.xaml", center, new Size(6, 149), new Point(3, 74.5));
            Components.Add(_deviationNeedle);

            _desiredCourseNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_course_needle.xaml", center, new Size(16, 217), new Point(8, 108.5));
            Components.Add(_desiredCourseNeedle);

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/HSI/hsi_faceplate.xaml", new Rect(0, 0, 350, 350)));

            _headingBug = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_heading_bug.xaml", center, new Size(22, 9), new Point(11, 130.5));
            Components.Add(_headingBug);

            _bearingNeedle = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_bearing_needle_1.xaml", center, new Size(9, 288), new Point(4.5, 148));
            Components.Add(_bearingNeedle);

            _bearingNeedle2 = new GaugeNeedle("{Helios}/Gauges/A-10/HSI/hsi_bearing_needle_2.xaml", center, new Size(12, 267), new Point(6, 133.5));
            Components.Add(_bearingNeedle2);

            _offFlagImage = new GaugeImage("{Helios}/Gauges/A-10/HSI/hsi_off_flag.xaml", new Rect(266, 116, 77, 131));
            _offFlagImage.IsHidden = true;
            Components.Add(_offFlagImage);

            Components.Add(new GaugeImage("{Helios}/Gauges/A-10/HSI/hsi_bezel.png", new Rect(0, 0, 350, 350)));

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

            _bearing1 = new HeliosValue(this, new BindingValue(0d), "", "bearing 1", "Bearing to the beacon.", "(0 - 360)", BindingValueUnits.Degrees);
            _bearing1.Execute += new HeliosActionHandler(Bearing1_Execute);
            Actions.Add(_bearing1);

            _bearing2 = new HeliosValue(this, new BindingValue(0d), "", "bearing 2", "Bearing to the beacon.", "(0 - 360)", BindingValueUnits.Degrees);
            _bearing2.Execute += new HeliosActionHandler(Bearing2_Execute);
            Actions.Add(_bearing2);

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

        #region Properties

        public bool IsDesiredHeadingRelative
        {
            get
            {
                return _relativeDesiredHeading;
            }
            set
            {
                if (!_relativeDesiredHeading.Equals(value))
                {
                    bool oldValue = _relativeDesiredHeading;
                    _relativeDesiredHeading = value;
                    OnPropertyChanged("IsDesiredHeadingRelative", oldValue, value, true);
                }
            }
        }

        public bool IsBearingRelative
        {
            get
            {
                return _relativeBearing;
            }
            set
            {
                if (!_relativeBearing.Equals(value))
                {
                    bool oldValue = _relativeBearing;
                    _relativeBearing = value;
                    OnPropertyChanged("IsBearingRelative", oldValue, value, true);
                }
            }
        }

        public bool IsDesiredCourseRelative
        {
            get
            {
                return _relativeCourse;
            }
            set
            {
                if (!_relativeCourse.Equals(value))
                {
                    bool oldValue = _relativeCourse;
                    _relativeCourse = value;
                    OnPropertyChanged("IsDesiredCourseRelative", oldValue, value, true);
                }
            }
        }

        #endregion

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

        void Bearing1_Execute(object action, HeliosActionEventArgs e)
        {
            _bearing1.SetValue(new BindingValue(ClampDegrees(e.Value.DoubleValue)), e.BypassCascadingTriggers);

            if (_relativeBearing)
            {
                _bearingNeedle.Rotation = _bearing1.Value.DoubleValue;
            }
            else
            {
                _bearingNeedle.Rotation = _bearing1.Value.DoubleValue + _compassNeedle.Rotation;
            }
        }

        void Bearing2_Execute(object action, HeliosActionEventArgs e)
        {
            _bearing2.SetValue(new BindingValue(ClampDegrees(e.Value.DoubleValue)), e.BypassCascadingTriggers);

            if (_relativeBearing)
            {
                _bearingNeedle2.Rotation = _bearing2.Value.DoubleValue;
            }
            else
            {
                _bearingNeedle2.Rotation = _bearing2.Value.DoubleValue + _compassNeedle.Rotation;
            }
        }

        void Distance_Execute(object action, HeliosActionEventArgs e)
        {
            _distance.SetValue(e.Value, e.BypassCascadingTriggers);
            _milesDrum.Value = e.Value.DoubleValue;
        }

        void DesiredHeading_Execute(object action, HeliosActionEventArgs e)
        {
            _desiredHeading.SetValue(new BindingValue(ClampDegrees(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            if (_relativeDesiredHeading)
            {
                _headingBug.Rotation = _desiredHeading.Value.DoubleValue;
            }
            else
            {
                _headingBug.Rotation = _desiredHeading.Value.DoubleValue + _compassNeedle.Rotation;
            }
        }

        void DesiredCourse_Execute(object action, HeliosActionEventArgs e)
        {
            _desiredCourse.SetValue(new BindingValue(ClampDegrees(e.Value.DoubleValue)), e.BypassCascadingTriggers);

            if (_relativeCourse)
            {
                _desiredCourseNeedle.Rotation = _desiredCourse.Value.DoubleValue;
                _courseDrum.Value = ClampDegrees(_desiredCourse.Value.DoubleValue + _currentHeading.Value.DoubleValue);
            }
            else
            {
                _desiredCourseNeedle.Rotation = _desiredCourse.Value.DoubleValue + _compassNeedle.Rotation;
                _courseDrum.Value = ClampDegrees(_desiredCourse.Value.DoubleValue);
            }

            _deviationNeedle.Rotation = _desiredCourseNeedle.Rotation;
            _deviationCard.Rotation = _desiredCourseNeedle.Rotation;
            _courseFlagNeedle.Rotation = _deviationCard.Rotation;
            _toFlagNeedle.Rotation = _deviationCard.Rotation;
            _fromFlagNeedle.Rotation = _deviationCard.Rotation;
        }

        void CurrentHeading_Execute(object action, HeliosActionEventArgs e)
        {
            _currentHeading.SetValue(new BindingValue(ClampDegrees(e.Value.DoubleValue)), e.BypassCascadingTriggers);
            _compassNeedle.Rotation = -_currentHeading.Value.DoubleValue;

            if (!_relativeDesiredHeading)
            {
                _headingBug.Rotation = _desiredHeading.Value.DoubleValue + _compassNeedle.Rotation;
            }

            if (!_relativeBearing)
            {
                _bearingNeedle.Rotation = _bearing1.Value.DoubleValue + _compassNeedle.Rotation;
                _bearingNeedle2.Rotation = _bearing2.Value.DoubleValue + _compassNeedle.Rotation;
            }

            if (!_relativeCourse)
            {
                _desiredCourseNeedle.Rotation = _desiredCourse.Value.DoubleValue + _compassNeedle.Rotation;
                _deviationNeedle.Rotation = _desiredCourseNeedle.Rotation;
                _deviationCard.Rotation = _desiredCourseNeedle.Rotation;
                _courseFlagNeedle.Rotation = _deviationCard.Rotation;
                _toFlagNeedle.Rotation = _deviationCard.Rotation;
                _fromFlagNeedle.Rotation = _deviationCard.Rotation;
            }
            else
            {
                _courseDrum.Value = ClampDegrees(_desiredCourse.Value.DoubleValue + _currentHeading.Value.DoubleValue);
            }
        }

        private double ClampDegrees(double input)
        {
            while (input < 0d)
            {
                input += 360d;
            }
            while (input >= 360d)
            {
                input -= 360d;
            }
            return input;
        }

        #region Persistance

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("IsDesiredHeadingRelative", IsDesiredHeadingRelative.ToString());
            writer.WriteElementString("IsBearingRelative", IsBearingRelative.ToString());
            writer.WriteElementString("IsDesiredCourseRelative", IsDesiredCourseRelative.ToString());
            base.WriteXml(writer);
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.Name.Equals("IsDesiredHeadingRelative"))
            {
                IsDesiredHeadingRelative = bool.Parse(reader.ReadElementString("IsDesiredHeadingRelative"));
            }

            if (reader.Name.Equals("IsBearingRelative"))
            {
                IsBearingRelative = bool.Parse(reader.ReadElementString("IsBearingRelative"));
            }

            if (reader.Name.Equals("IsDesiredCourseRelative"))
            {
                IsDesiredCourseRelative = bool.Parse(reader.ReadElementString("IsDesiredCourseRelative"));
            }
            base.ReadXml(reader);
        }

        #endregion
    }
}

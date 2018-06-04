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

namespace GadrocsWorkshop.Helios.Gauges.AV8B.EDP
{
    using GadrocsWorkshop.Helios.Gauges.AV8B.TwoDigitDisplay;
    using GadrocsWorkshop.Helios.Gauges.AV8B.ThreeDigitDisplay;
    using GadrocsWorkshop.Helios.Gauges.AV8B.FourDigitDisplay;
    using GadrocsWorkshop.Helios.Gauges.AV8B.stabilizerDisplay;
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;

    [HeliosControl("Helios.AV8B.edp", "AV-8B Engine Panel", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class edp: BaseGauge
    {
        private HeliosValue _angle;
        private HeliosValue _stabDir;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;
        private TwoDigitDisplay _stab;
        private stabilizerDisplay _stabArrow;
        private TwoDigitDisplay _water;
        private ThreeDigitDisplay _ff;
        private ThreeDigitDisplay _jpt;
        private ThreeDigitDisplay _duct;
        private FourDigitDisplay _rpm;


        public edp()
            : base("Engine Panel", new Size(528,302))
        {

            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/Engine Panel/edp_faceplate.xaml", new Rect(0d, 0d, 528d, 302d)));

            _stab = new TwoDigitDisplay();
            _stabArrow = new stabilizerDisplay();
            _water = new TwoDigitDisplay();
            _ff = new ThreeDigitDisplay();
            _jpt = new ThreeDigitDisplay();
            _duct = new ThreeDigitDisplay();
            _rpm = new FourDigitDisplay();

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 0.94d, 150d);
            _needle = new GaugeNeedle("{Helios}/Gauges/AV-8B/Common/nozzle_needle.xaml", new Point(440d, 138d), new Size(18d, 72d), new Point(7.6d, 28.1d), 90d);
            Components.Add(_needle);

            _angle = new HeliosValue(this, new BindingValue(0d), "", "angle", "Current position of Nozzles.", "(0 - 120)", BindingValueUnits.Degrees);
            _angle.Execute += new HeliosActionHandler(Angle_Execute);
            Actions.Add(_angle);

            _stabDir = new HeliosValue(this, new BindingValue(0d), "", "stabilizer arrow", "Stabilizer direction arrow.", "(Up, Neutral, Down)", BindingValueUnits.Numeric);
            _stabDir.Execute += new HeliosActionHandler(_stabArrow.DigitDisplay_Execute);
            Actions.Add(_stabDir);
        }

        void Angle_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }
     }
}

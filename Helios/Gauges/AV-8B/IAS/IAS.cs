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

namespace GadrocsWorkshop.Helios.Gauges.AV8B.IAS
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Windows;
    using System.Windows.Media;

    [HeliosControl("Helios.AV8B.IAS", "IAS", "AV-8B Gauges", typeof(GaugeRenderer))]
    public class IAS : BaseGauge
    {
        private HeliosValue _indicatedAirSpeed;
        private GaugeNeedle _needle;
        private CalibrationPointCollectionDouble _needleCalibration;
        
        public IAS()
            : base("IAS", new Size(364, 376))
        {

            //Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/IAS/ias_faceplate.xaml", new Rect(32d, 38d, 300, 300)));

            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 950d, 350d);
            _needleCalibration.Add(new CalibrationPointDouble(100d, 18d));
            _needleCalibration.Add(new CalibrationPointDouble(500d, 180d));
            _needle = new GaugeNeedle("{Helios}/Gauges/AV-8B/Common/needle_a.xaml", new Point(182d, 188d), new Size(20, 175), new Point(10, 140), 0d);
            Components.Add(_needle);

            //Components.Add(new GaugeImage("{Helios}/Gauges/A-10/Common/gauge_bezel.png", new Rect(0d, 0d, 364d, 376d)));
            Components.Add(new GaugeImage("{Helios}/Gauges/AV-8B/IAS/needle_mask.xaml", new Rect(170d, 60d, 100d, 110d)));

            _indicatedAirSpeed = new HeliosValue(this, new BindingValue(0d), "", "indicated airspeed", "Current indicated airspeed of the aircraft.", "(0 - 950)", BindingValueUnits.Knots);
            _indicatedAirSpeed.Execute += new HeliosActionHandler(IndicatedAirSpeed_Execute);
            Actions.Add(_indicatedAirSpeed);
        }

        void IndicatedAirSpeed_Execute(object action, HeliosActionEventArgs e)
        {
            _needle.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue);
        }

    }
}

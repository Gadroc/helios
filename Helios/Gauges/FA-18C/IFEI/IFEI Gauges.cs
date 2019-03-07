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

namespace GadrocsWorkshop.Helios.Gauges.FA18C
{
    using GadrocsWorkshop.Helios.Gauges;
    using GadrocsWorkshop.Helios.ComponentModel;
    using GadrocsWorkshop.Helios.Controls;
    using System;
    using System.Windows.Media;
    using System.Windows;

    [HeliosControl("Helios.FA18C.IFEIGauges", "IFEI Needles & Flags", "F/A-18C", typeof(GaugeRenderer))]
    public class IFEI_Gauges : BaseGauge
    {
        private static readonly Rect SCREEN_RECT = new Rect(0, 0, 1, 1);
        private Rect _scaledScreenRect = SCREEN_RECT;

        //private String _font = "Hornet IFEI Mono"; // "Segment7 Standard"; //"Seven Segment";
        private Color _textColor = Color.FromArgb(0xff,220, 220, 220);
        private string _imageLocation = "{Helios}/Gauges/FA-18C/IFEI/";
        private HeliosValue _leftNozzle;
        private HeliosValue _rightNozzle;
        private GaugeNeedle _gnleftnoz;
        private GaugeNeedle _gnrightnoz;
        private CalibrationPointCollectionDouble _needleCalibration;
        private GaugeImage _gibackground;
        private GaugeImage _gireflection;
        private GaugeImage _giL;
        private HeliosValue _indicatorLeft;
        private GaugeImage _giR;
        private HeliosValue _indicatorRight;
        private GaugeImage _giZ;
        private HeliosValue _indicatorZulu;
        private GaugeImage _giBingo;
        private HeliosValue _indicatorBingo;
        private GaugeImage _giFF;
        private HeliosValue _indicatorFuelFlow;
        private GaugeImage _giTemp;
        private HeliosValue _indicatorTemp;
        private GaugeImage _giRPM;
        private HeliosValue _indicatorRPM;
        private GaugeImage _giOil;
        private HeliosValue _indicatorOil;
        private GaugeImage _giNoz;
        private HeliosValue _indicator_Noz;
        private GaugeImage _giGaugeMarksL;
        private HeliosValue _indicatorMarksLeft;
        private GaugeImage _giGaugeMarksR;
        private HeliosValue _indicatorMarksRight;
        private GaugeImage _giGaugeMarksL000;
        private HeliosValue _indicatorMarksLeft000;
        private GaugeImage _giGaugeMarksR000;
        private HeliosValue _indicatorMarksRight000;
        private GaugeImage _giGaugeMarksL050;
        private HeliosValue _indicatorMarksLeft050;
        private GaugeImage _giGaugeMarksR050;
        private HeliosValue _indicatorMarksRight050;
        private GaugeImage _giGaugeMarksL100;
        private HeliosValue _indicatorMarksLeft100;
        private GaugeImage _giGaugeMarksR100;
        private HeliosValue _indicatorMarksRight100;

        public IFEI_Gauges()
            : base("IFEI_Gauges", new Size(779, 702))
        {
            // adding the control buttons
            // Fuel info

            // Add various image components to the gauge
            _gibackground = new GaugeImage(_imageLocation + "IFEI.png", new Rect(0d, 0d, 779d, 702d));
            Components.Add(_gibackground);
            _gibackground.IsHidden = true;  // This is to make sure that we do not mask anything while developing

            _giGaugeMarksL = new GaugeImage(_imageLocation + "IFEI Left Nozzle Gauge Marks.xaml", new Rect(80d, 270d, 277d, 137d));
            Components.Add(_giGaugeMarksL);
            _giGaugeMarksR = new GaugeImage(_imageLocation + "IFEI Right Nozzle Gauge Marks.xaml", new Rect(80d, 270d, 277d, 137d));
            Components.Add(_giGaugeMarksR);
            _giGaugeMarksL000 = new GaugeImage(_imageLocation + "IFEI Left 0 Nozzle Gauge Marks.xaml", new Rect(80d, 270d, 277d, 137d));
            Components.Add(_giGaugeMarksL000);
            _giGaugeMarksR000 = new GaugeImage(_imageLocation + "IFEI Right 0 Nozzle Gauge Marks.xaml", new Rect(80d, 270d, 277d, 137d));
            Components.Add(_giGaugeMarksR000);
            _giGaugeMarksL050 = new GaugeImage(_imageLocation + "IFEI Left 50 Nozzle Gauge Marks.xaml", new Rect(80d, 270d, 277d, 137d));
            Components.Add(_giGaugeMarksL050);
            _giGaugeMarksR050 = new GaugeImage(_imageLocation + "IFEI Right 50 Nozzle Gauge Marks.xaml", new Rect(80d, 270d, 277d, 137d));
            Components.Add(_giGaugeMarksR050);
            _giGaugeMarksL100 = new GaugeImage(_imageLocation + "IFEI Left 100 Nozzle Gauge Marks.xaml", new Rect(80d, 270d, 277d, 137d));
            Components.Add(_giGaugeMarksL100);
            _giGaugeMarksR100 = new GaugeImage(_imageLocation + "IFEI Right 100 Nozzle Gauge Marks.xaml", new Rect(80d, 270d, 277d, 137d));
            Components.Add(_giGaugeMarksR100);
            _needleCalibration = new CalibrationPointCollectionDouble(0d, 0d, 90d, 90d);
            _gnleftnoz = new GaugeNeedle(_imageLocation + "IFEI Left Needle.xaml", new Point(83d, 273d), new Size(92d, 6d), new Point(3d, 3d));
            Components.Add(_gnleftnoz);
            _gnleftnoz.IsHidden = false;
            _gnrightnoz = new GaugeNeedle(_imageLocation + "IFEI Right Needle.xaml", new Point(354d, 273d), new Size(92d, 6d), new Point(89d, 3d));
            Components.Add(_gnrightnoz);
            _gnrightnoz.IsHidden = false;
            _leftNozzle = new HeliosValue(this, new BindingValue(0d), "", "Left Nozzle", "Left Nozzle Position in %.", "", BindingValueUnits.NozzlePositionPercent);
            _leftNozzle.Execute += new HeliosActionHandler(LeftNozzlePosition_Execute);
            Actions.Add(_leftNozzle);
            _rightNozzle = new HeliosValue(this, new BindingValue(0d), "", "Right Nozzle", "Right Nozzle Position in %.", "", BindingValueUnits.NozzlePositionPercent);
            _rightNozzle.Execute += new HeliosActionHandler(RightNozzlePosition_Execute);
            Actions.Add(_rightNozzle);

            _giNoz = new GaugeImage(_imageLocation + "IFEI Legends Noz.xaml", new Rect(202d, 336d, 28d, 12d));
            Components.Add(_giNoz);
            _giRPM = new GaugeImage(_imageLocation + "IFEI Legends RPM.xaml", new Rect(202d, 102d, 28d, 12d));
            Components.Add(_giRPM);
            _giFF = new GaugeImage(_imageLocation + "IFEI Legends FF.xaml", new Rect(202d, 214d, 30d, 24d));
            Components.Add(_giFF);
            _giTemp = new GaugeImage(_imageLocation + "IFEI Legends Temp.xaml", new Rect(198d, 157d, 36d, 12d));
            Components.Add(_giTemp);
            _giBingo = new GaugeImage(_imageLocation + "IFEI Legends Bingo.xaml", new Rect(596d, 236d, 40d, 12d));
            Components.Add(_giBingo);
            _giL = new GaugeImage(_imageLocation + "IFEI Legends L.xaml", new Rect(689d, 114d, 8d, 12d));
            Components.Add(_giL);
            _giR = new GaugeImage(_imageLocation + "IFEI Legends R.xaml", new Rect(689d, 180d,  8d, 12d));
            Components.Add(_giR);
            _giZ = new GaugeImage(_imageLocation + "IFEI Legends Z.xaml", new Rect(688d, 376d, 8d, 12d));
            Components.Add(_giZ);
            _giOil = new GaugeImage(_imageLocation + "IFEI Legends Oil.xaml", new Rect(206d, 450d, 22d, 12d));
            Components.Add(_giOil);
            _gireflection = new GaugeImage(_imageLocation + "IFEI Reflections.png", new Rect(0d, 0d, 779d, 702d));
            Components.Add(_gireflection);
            _gireflection.IsHidden = false;
            _giOil.IsHidden = false;
            _giZ.IsHidden = true;
            _giR.IsHidden = true;
            _giL.IsHidden = true;
            _giBingo.IsHidden = false;
            _giTemp.IsHidden = false;
            _giFF.IsHidden = false;
            _giRPM.IsHidden = false;
            _giNoz.IsHidden = false;
            _giGaugeMarksL.IsHidden = false;
            _giGaugeMarksR.IsHidden = false;
            _giGaugeMarksL000.IsHidden = false;
            _giGaugeMarksR000.IsHidden = false;
            _giGaugeMarksL050.IsHidden = false;
            _giGaugeMarksR050.IsHidden = false;
            _giGaugeMarksL100.IsHidden = false;
            _giGaugeMarksR100.IsHidden = false;

            _indicatorLeft = new HeliosValue(this, new BindingValue(0d), "", "Left flag", "Left Indicator flag.", "", BindingValueUnits.Boolean);
            _indicatorLeft.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorLeft);
            _indicatorRight = new HeliosValue(this, new BindingValue(0d), "", "Right flag", "Right Indicator flag.", "", BindingValueUnits.Boolean);
            _indicatorRight.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorRight);
            _indicatorZulu = new HeliosValue(this, new BindingValue(0d), "", "Zulu flag", "Zulu Indicator flag.", "", BindingValueUnits.Boolean);
            _indicatorZulu.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorZulu);
            _indicatorBingo = new HeliosValue(this, new BindingValue(0d), "", "Bingo flag", "Bingo Indicator flag.", "", BindingValueUnits.Boolean);
            _indicatorBingo.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorBingo);
            _indicatorFuelFlow = new HeliosValue(this, new BindingValue(0d), "", "Fuel Flow flag", "Fuel Flow Indicator flag.", "", BindingValueUnits.Boolean);
            _indicatorFuelFlow.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorFuelFlow);
            _indicatorTemp = new HeliosValue(this, new BindingValue(0d), "", "Temperature flag", "Temp Indicator flag.", "", BindingValueUnits.Boolean);
            _indicatorTemp.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorTemp);
            _indicatorRPM = new HeliosValue(this, new BindingValue(0d), "", "RPM flag", "RPM Indicator flag.", "", BindingValueUnits.Boolean);
            _indicatorRPM.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorRPM);
            _indicatorOil = new HeliosValue(this, new BindingValue(0d), "", "Oil Flag", "Oil Indicator flag.", "", BindingValueUnits.Boolean);
            _indicatorOil.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorOil);
            _indicator_Noz = new HeliosValue(this, new BindingValue(0d), "", "Noz Flag", "Noz Indicator flag.", "", BindingValueUnits.Boolean);
            _indicator_Noz.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicator_Noz);
            _indicatorMarksLeft = new HeliosValue(this, new BindingValue(0d), "", "Gauge Marks Left", "Left side Gauge Indicator Marks.", "", BindingValueUnits.Boolean);
            _indicatorMarksLeft.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorMarksLeft);
            _indicatorMarksRight = new HeliosValue(this, new BindingValue(0d), "", "Gauge Marks Right", "Right side Gauge Indicator Marks.", "", BindingValueUnits.Boolean);
            _indicatorMarksRight.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorMarksRight);
            _indicatorMarksLeft000 = new HeliosValue(this, new BindingValue(0d), "", "0 Gauge Mark Left", "Left side Gauge Indicator Mark 0.", "", BindingValueUnits.Boolean);
            _indicatorMarksLeft000.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorMarksLeft000);
            _indicatorMarksRight000 = new HeliosValue(this, new BindingValue(0d), "", "0 Gauge Mark Right", "Right side Gauge Indicator Mark 0.", "", BindingValueUnits.Boolean);
            _indicatorMarksRight000.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorMarksRight000);
            _indicatorMarksLeft050 = new HeliosValue(this, new BindingValue(0d), "", "50 Gauge Mark Left", "Left side Gauge Indicator Mark 50.", "", BindingValueUnits.Boolean);
            _indicatorMarksLeft050.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorMarksLeft050);
            _indicatorMarksRight050 = new HeliosValue(this, new BindingValue(0d), "", "50 Gauge Mark Right", "Right side Gauge Indicator Mark 50.", "", BindingValueUnits.Boolean);
            _indicatorMarksRight050.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorMarksRight050);
            _indicatorMarksLeft100 = new HeliosValue(this, new BindingValue(0d), "", "100 Gauge Mark Left", "Left side Gauge Indicator Mark 100.", "", BindingValueUnits.Boolean);
            _indicatorMarksLeft100.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorMarksLeft100);
            _indicatorMarksRight100 = new HeliosValue(this, new BindingValue(0d), "", "100 Gauge Mark Right", "Right side Gauge Indicator Mark 100.", "", BindingValueUnits.Boolean);
            _indicatorMarksRight100.Execute += new HeliosActionHandler(Indicator_Execute);
            Actions.Add(_indicatorMarksRight100);
        }
        protected override void OnProfileChanged(HeliosProfile oldProfile) {
            base.OnProfileChanged(oldProfile);
        }

        void LeftNozzlePosition_Execute(object action, HeliosActionEventArgs e)
        {          
            _gnleftnoz.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue % 1000d);
        }

        void RightNozzlePosition_Execute(object action, HeliosActionEventArgs e)
        {
            _gnrightnoz.Rotation = _needleCalibration.Interpolate(e.Value.DoubleValue % 1000d);
        }

        void Indicator_Execute(object action,HeliosActionEventArgs e)
        {
            ConfigManager.LogManager.LogInfo("Indicator Execute: Action - " + action.ToString() + " Event: " + e.ToString());
        }
        public override bool HitTest(Point location)
        {
            if (_scaledScreenRect.Contains(location))
            {
                return false;
            }

            return true;
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

    }
}

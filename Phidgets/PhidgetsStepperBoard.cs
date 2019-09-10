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

namespace GadrocsWorkshop.Helios.Interfaces.Phidgets
{
    using GadrocsWorkshop.Helios.ComponentModel;
    using global::Phidgets;
    using global::Phidgets.Events;
    using System;
    using System.Globalization;

    [HeliosInterface("Helios.Phidgets.UnipolarStepperBoard", "Phidgets Unipolar Stepper Controller", typeof(PhidgetsStepperBoardEditor), Factory = typeof(PhidgetsInterfaceFactory))]
    public class PhidgetStepperBoard : PhidgetInterface
    {
        private Stepper _stepperBoard;
        private Int32 _serialNumber = 0;
        private PhidgetsStepper[] _steppers = new PhidgetsStepper[4];
        private int _motorCount = 0;

        public PhidgetStepperBoard()
            : base("Phidgets Stepper Board")
        {
            for (int i = 0; i < 4; i++)
            {
                PhidgetsStepper stepper = new PhidgetsStepper(this, i);
                _steppers[i] = stepper;
            }
        }

        public PhidgetStepperBoard(Int32 serialNumber)
            : this()
        {
            SerialNumber = serialNumber;
            ConfigManager.LogManager.LogDebug("Attaching phidget stepper board for config (SerialNumber=\"" + _serialNumber + "\")"); 
            Attach();
        }

        ~PhidgetStepperBoard()
        {
            Detach();
        }

        #region Properties

        public PhidgetsStepper[] Steppers
        {
            get { return _steppers; }
        }

        protected override void OnProfileChanged(HeliosProfile oldProfile)
        {
            base.OnProfileChanged(oldProfile);

            if (oldProfile != null)
            {
                oldProfile.ProfileStarted -= Profile_ProfileStarted;
                oldProfile.ProfileStopped -= Profile_ProfileStopped;
            }

            if (Profile != null)
            {
                Profile.ProfileStarted += Profile_ProfileStarted;
                Profile.ProfileStopped += Profile_ProfileStopped;
            }
        }

        private void Profile_ProfileStarted(object sender, EventArgs e)
        {
            ConfigManager.LogManager.LogDebug("Attaching phidget stepper board for profile start. (SerialNumber=\"" + _serialNumber + "\")");
            Attach();
        }

        private void Profile_ProfileStopped(object sender, EventArgs e)
        {
            ConfigManager.LogManager.LogDebug("Detaching phidget stepper board for profile stop. (SerialNumber=\"" + _serialNumber + "\")");
            Detach();
        }

        public override Int32 SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                if (!_serialNumber.Equals(value))
                {
                    Int32 oldValue = _serialNumber;

                    if (_stepperBoard != null)
                    {
                        Detach();
                    }

                    _serialNumber = value;

                    OnPropertyChanged("SerialNumber", oldValue, value, false);

                    Name = "Phidgets Stepper Board (" + _serialNumber + ")";
                }
            }
        }

        public int MotorCount
        {
            get
            {
                return _motorCount;
            }
            private set
            {
                _motorCount = value;
            }
        }

        public void Attach()
        {
            if (_serialNumber > 0 && _stepperBoard == null)
            {
                _stepperBoard = new Stepper();
                _stepperBoard.Attach += new AttachEventHandler(StepperBoard_Attach);
                _stepperBoard.open(_serialNumber);
            }

            if (Profile != null)
            {
                Profile.ProfileTick += Profile_ProfileTick;
            }
        }

        void Profile_ProfileTick(object sender, EventArgs e)
        {
            if (_stepperBoard != null && _stepperBoard.Attached)
            {
                foreach (StepperStepper stepper in _stepperBoard.steppers)
                {
                    if (stepper.Stopped && stepper.Engaged)
                    {
                        stepper.Engaged = false;
                    }
                }
            }
        }

        private void StepperBoard_Attach(object sender, AttachEventArgs e)
        {
            for (int i = 0; i < _stepperBoard.steppers.Count; i++)
            {
                if (_motorCount == 0)
                {
                    _steppers[i].MinAcceleration = _stepperBoard.steppers[i].AccelerationMin;
                    _steppers[i].MaxAcceleration = _stepperBoard.steppers[i].AccelerationMax;
                    _steppers[i].Acceleration = _steppers[i].MaxAcceleration;

                    _steppers[i].MinVelocity = _stepperBoard.steppers[i].VelocityMin;
                    _steppers[i].MaxVelocity = _stepperBoard.steppers[i].VelocityMax;
                    _steppers[i].VelocityLimit = _steppers[i].MaxVelocity;

                    RegisterBindings(i);                    
                }
                if (_steppers[i].VelocityLimit > _stepperBoard.steppers[i].VelocityMax)
                    _steppers[i].VelocityLimit = _stepperBoard.steppers[i].VelocityMax;
                if (_steppers[i].VelocityLimit < _stepperBoard.steppers[i].VelocityMin)
                    _steppers[i].VelocityLimit = _stepperBoard.steppers[i].VelocityMin;

                _stepperBoard.steppers[i].VelocityLimit = _steppers[i].VelocityLimit;

                if (_steppers[i].Acceleration > _stepperBoard.steppers[i].AccelerationMax)
                    _steppers[i].Acceleration = _stepperBoard.steppers[i].AccelerationMax;
                if (_steppers[i].Acceleration < _stepperBoard.steppers[i].AccelerationMin)
                    _steppers[i].Acceleration = _stepperBoard.steppers[i].AccelerationMin;
                _stepperBoard.steppers[i].Acceleration = _steppers[i].Acceleration;
            }

            if (_motorCount == 0)
            {
                _motorCount = _stepperBoard.steppers.Count;
                ConfigManager.LogManager.LogDebug("Detaching phidget stepper board after config. (SerialNumber=\"" + _serialNumber + "\")");
                _stepperBoard.close();
                _stepperBoard = null;
            }
        }

        private void RegisterBindings(int motorNum)
        {
            Values.Add(_steppers[motorNum].StepperValue);
            Values.Add(_steppers[motorNum].TargetPosition);

            Actions.Add(_steppers[motorNum].Zero);
            Actions.Add(_steppers[motorNum].StepperValue);
            Actions.Add(_steppers[motorNum].TargetPosition);
            Actions.Add(_steppers[motorNum].Increment);
            Actions.Add(_steppers[motorNum].Decrement);

            Triggers.Add(_steppers[motorNum].StepperValue);
            Triggers.Add(_steppers[motorNum].TargetPosition);
        }

        public void Detach()
        {
            ConfigManager.LogManager.LogDebug("Detaching phidget stepper board. (SerialNumber=\"" + _serialNumber + "\")");
            if (_stepperBoard != null && _stepperBoard.Attached)
            {
                try
                {
                    /*
                    SetTargetPosition(0, 0);
                    SetTargetPosition(1, 0);
                    SetTargetPosition(2, 0);
                    SetTargetPosition(3, 0);
                    */

                    try
                    {
                        foreach (StepperStepper stepper in _stepperBoard.steppers)
                        {
                            while (!stepper.Stopped)
                    {
                        // Wait for all steppers to reset to their zero position.
                                System.Threading.Thread.Sleep(50);
                    }
                        }
                    }
                    catch { }


                    foreach (StepperStepper stepper in _stepperBoard.steppers)
                    {
                        stepper.Engaged = false;
                    }

                    _stepperBoard.close();
                    _stepperBoard = null;
                }
                catch (PhidgetException e)
                {
                    ConfigManager.LogManager.LogError("Error closing stepper board. (SerialNumber=\"" + _serialNumber + "\")", e);
                }
            }
        }

        #endregion

        internal void ZeroCurrentPosition(int motorNumber)
        {
            if (_stepperBoard != null && _stepperBoard.Attached)
            {
                _stepperBoard.steppers[motorNumber].CurrentPosition = 0;
                _stepperBoard.steppers[motorNumber].TargetPosition = 0;
            }
        }

        internal void SetTargetPosition(int motorNumber, long position)
        {
            if (_stepperBoard != null && _stepperBoard.Attached)
            {
                try
                {

                _stepperBoard.steppers[motorNumber].TargetPosition = position;
                _stepperBoard.steppers[motorNumber].Engaged = true;
            }
                catch
                { }
            }
        }

        internal void SetAcceleration(int motorNumber, double acceleration)
        {
            if (_stepperBoard != null && _stepperBoard.Attached)
            {
                if (acceleration > _stepperBoard.steppers[motorNumber].AccelerationMax)
                    acceleration = _stepperBoard.steppers[motorNumber].AccelerationMax;
                if (acceleration < _stepperBoard.steppers[motorNumber].AccelerationMin)
                    acceleration = _stepperBoard.steppers[motorNumber].AccelerationMin;

                _stepperBoard.steppers[motorNumber].Acceleration = acceleration;
            }
        }

        internal void SetVelocityLimit(int motorNumber, double velocityLimit)
        {
            if (_stepperBoard != null && _stepperBoard.Attached)
            {
                _stepperBoard.steppers[motorNumber].VelocityLimit = velocityLimit;
            }
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            SerialNumber = Int32.Parse(reader.ReadElementString("SerialNumber"), CultureInfo.InvariantCulture);
            _motorCount = int.Parse(reader.ReadElementString("MotorCount"), CultureInfo.InvariantCulture);
            reader.ReadStartElement("Motors");
            for (int i = 0; i < _motorCount; i++)
            {
                RegisterBindings(i);
                reader.ReadStartElement("Motor");
                _steppers[i].VelocityLimit = double.Parse(reader.ReadElementString("VelocityLimit"), CultureInfo.InvariantCulture);
                _steppers[i].Acceleration = double.Parse(reader.ReadElementString("Acceleration"), CultureInfo.InvariantCulture);
                _steppers[i].Calibration.Read(reader);
                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("SerialNumber", SerialNumber.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("MotorCount", _motorCount.ToString(CultureInfo.InvariantCulture));
            writer.WriteStartElement("Motors");
            for (int i = 0; i < _motorCount; i++)
            {
                writer.WriteStartElement("Motor");
                writer.WriteElementString("VelocityLimit", _steppers[i].VelocityLimit.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Acceleration", _steppers[i].Acceleration.ToString(CultureInfo.InvariantCulture));
                _steppers[i].Calibration.Write(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}

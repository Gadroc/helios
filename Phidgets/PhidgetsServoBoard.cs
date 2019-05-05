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
    using System.ComponentModel;
    using System.Globalization;

    [HeliosInterface("Helios.Phidgets.AdvancedServoBoard", "Phidgets Servo Controller", typeof(PhidgetsServoBoardEditor), Factory = typeof(PhidgetsInterfaceFactory))]
    public class PhidgetsServoBoard : PhidgetInterface
    {
        private AdvancedServo _servoBoard;
        private Int32 _serialNumber = 0;
        private PhidgetsServo[] _servos = new PhidgetsServo[8];
        private int _servoCount;

        public PhidgetsServoBoard()
            : base("Phidget Servo Board")
        {
            for (int i = 0; i < 8; i++)
            {
                PhidgetsServo servo = new PhidgetsServo(this, i);
                _servos[i] = servo;
            }
            _servoCount = 0;
        }

        public PhidgetsServoBoard(Int32 serialNumber)
            : this()
        {
            SerialNumber = serialNumber;
            ConfigManager.LogManager.LogDebug("Attaching phidget servo board for config (SerialNumber=\"" + _serialNumber + "\")");
            Attach();
        }

        ~PhidgetsServoBoard()
        {
            Detach();
        }

        public PhidgetsServo[] Servos
        {
            get { return _servos; }
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
            ConfigManager.LogManager.LogDebug("Attaching phidget servo board for profile start. (SerialNumber=\"" + _serialNumber + "\")");
            Attach();
        }

        private void Profile_ProfileStopped(object sender, EventArgs e)
        {
            ConfigManager.LogManager.LogDebug("Detaching phidget servo board for profile stop. (SerialNumber=\"" + _serialNumber + "\")");
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

                    if (_servoBoard != null)
                    {
                        Detach();
                    }

                    _serialNumber = value;

                    OnPropertyChanged("SerialNumber", oldValue, value, false);

                    Name = "Phidgets Servo Board (" + _serialNumber + ")";
                }
            }
        }

        public int ServoCount
        {
            get
            {
                return _servoCount;
            }
            private set
            {
                _servoCount = value;
            }
        }

        public void Attach()
        {
            if (_serialNumber > 0 && _servoBoard == null)
            {
                _servoBoard = new AdvancedServo();
                _servoBoard.Attach += new AttachEventHandler(ServoBoard_Attach);
                _servoBoard.open(_serialNumber);
            }
        }

        private void ServoBoard_Attach(object sender, AttachEventArgs e)
        {
            for (int i = 0; i < _servoBoard.servos.Count; i++)
            {
                if (_servoCount == 0)
                {
                    SetServoProperties(i, _servos[i].ServoType, _servos[i].ServoMinPulseWidth, _servos[i].ServoMaxPulseWidth, _servos[i].ServoMaxVelocity, _servos[i].ServoDegrees);

                    _servos[i].MinAcceleration = _servoBoard.servos[i].AccelerationMin;
                    _servos[i].MaxAcceleration = _servoBoard.servos[i].AccelerationMax;
                    _servos[i].Acceleration = _servos[i].MaxAcceleration;

                    _servos[i].MinVelocity = _servoBoard.servos[i].VelocityMin;
                    _servos[i].MaxVelocity = _servoBoard.servos[i].VelocityMax;
                    _servos[i].VelocityLimit = _servos[i].MaxVelocity;

                    RegisterBindings(i); 
                }
                _servoBoard.servos[i].VelocityLimit = _servos[i].VelocityLimit;
                _servoBoard.servos[i].Acceleration = _servos[i].Acceleration < _servoBoard.servos[i].AccelerationMax ? _servos[i].Acceleration : _servoBoard.servos[i].AccelerationMax;;

                _servos[i].Calibration.OutputLimitMax = _servoBoard.servos[i].PositionMax;
                _servos[i].Calibration.OutputLimitMin = _servoBoard.servos[i].PositionMin;
            }

            if (_servoCount == 0)
            {
                _servoCount = _servoBoard.servos.Count;
                ConfigManager.LogManager.LogDebug("Detaching phidget servo board after config. (SerialNumber=\"" + _serialNumber + "\")");
                _servoBoard.close();
                _servoBoard = null;
            }
        }

        private void RegisterBindings(int motorNum)
        {
            Values.Add(_servos[motorNum].ServoValue);
            Values.Add(_servos[motorNum].TargetPosition);

            Actions.Add(_servos[motorNum].ServoValue);
            Actions.Add(_servos[motorNum].TargetPosition);

            Triggers.Add(_servos[motorNum].ServoValue);
            Triggers.Add(_servos[motorNum].TargetPosition);
        }

        public void Detach()
        {
            ConfigManager.LogManager.LogDebug("Detaching phidget servo board. (SerialNumber=\"" + _serialNumber + "\")");
            try
            {
            if (_servoBoard != null && _servoBoard.Attached)
            {

                    /*
                    for (int i = 0; i < _servoBoard.servos.Count; i++)
                {
                        while (!_servoBoard.servos[i].Stopped)
                        {
                            // Wait for all steppers to reset to their zero position.
                            System.Threading.Thread.Sleep(50);
                        }
                    }
                    */


                    for (int i = 0; i < _servoBoard.servos.Count; i++)
                    {
                        _servoBoard.servos[i].Engaged = false;
                    }

                    _servoBoard.close();
                    _servoBoard = null;
                }

            }
                catch (PhidgetException e)
                {
                    ConfigManager.LogManager.LogError("Error closing servo board", e);
                }
            }

        internal void SetTargetPosition(int servoNumber, double position)
        {
            if (_servoBoard != null && _servoBoard.Attached)
            {
                double newPosition = position;
                if (newPosition < _servoBoard.servos[servoNumber].PositionMin)
                {
                    newPosition = _servoBoard.servos[servoNumber].PositionMin;
                }
                else if (newPosition > _servoBoard.servos[servoNumber].PositionMax)
                {
                    newPosition = _servoBoard.servos[servoNumber].PositionMax;
                }

                _servoBoard.servos[servoNumber].Position = newPosition;
                _servoBoard.servos[servoNumber].Engaged = true;
            }
        }

        internal void SetAcceleration(int servoNumber, double acceleration)
        {
            if (_servoBoard != null && _servoBoard.Attached)
            {
                _servoBoard.servos[servoNumber].Acceleration = acceleration;
            }
        }

        internal void SetVelocityLimit(int servoNumber, double velocityLimit)
        {
            if (_servoBoard != null && _servoBoard.Attached)
            {
                _servoBoard.servos[servoNumber].VelocityLimit = velocityLimit;
            }
        }

        internal void SetSpeedRamping(int servoNumber, bool speedRamping)
        {
            if (_servoBoard != null && _servoBoard.Attached)
            {
                _servoBoard.servos[servoNumber].SpeedRamping = speedRamping;
            }
        }

        internal void SetServoProperties(int servoNumber, ServoServo.ServoType type, double minPulse, double maxPulse, double maxVelocity, double travel)
        {
            if (_servoBoard != null && _servoBoard.Attached)
            {
                double degrees = (maxPulse-minPulse)/travel;
                if (type == ServoServo.ServoType.USER_DEFINED)
                {
                    _servoBoard.servos[servoNumber].setServoParameters(minPulse, maxPulse, degrees, degrees * maxVelocity);
                }
                else
                {
                    _servoBoard.servos[servoNumber].Type = type;
                }
                _servos[servoNumber].Calibration.OutputLimitMax = travel > 0 ? travel : 0.001;
                _servos[servoNumber].Calibration.OutputLimitMin = 0.0;
            }
        }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            TypeConverter bc = TypeDescriptor.GetConverter(typeof(bool));
            SerialNumber = Int32.Parse(reader.ReadElementString("SerialNumber"), CultureInfo.InvariantCulture);
            _servoCount = int.Parse(reader.ReadElementString("ServoCount"), CultureInfo.InvariantCulture);
            reader.ReadStartElement("Servos");
            for (int i = 0; i < _servoCount; i++)
            {
                RegisterBindings(i);
                reader.ReadStartElement("Servo");
                _servos[i].SpeedRamping = (bool)bc.ConvertFromInvariantString(reader.ReadElementString("SpeedRamping"));
                _servos[i].VelocityLimit = double.Parse(reader.ReadElementString("VelocityLimit"), CultureInfo.InvariantCulture);
                _servos[i].Acceleration = double.Parse(reader.ReadElementString("Acceleration"), CultureInfo.InvariantCulture);
                _servos[i].Calibration.Read(reader);

                if (reader.Name.Equals("Type"))
                {
                    _servos[i].ServoType = (ServoServo.ServoType)Enum.Parse(typeof(ServoServo.ServoType), reader.ReadElementString("Type"));
                    _servos[i].ServoMinPulseWidth = double.Parse(reader.ReadElementString("MinPulse"), CultureInfo.InvariantCulture);
                    _servos[i].ServoMaxPulseWidth = double.Parse(reader.ReadElementString("MaxPulse"), CultureInfo.InvariantCulture);
                    _servos[i].ServoMaxVelocity = double.Parse(reader.ReadElementString("MaxVelocity"), CultureInfo.InvariantCulture);
                    _servos[i].ServoDegrees = double.Parse(reader.ReadElementString("Travel"), CultureInfo.InvariantCulture);

                }

                reader.ReadEndElement();
            }
            reader.ReadEndElement();
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            TypeConverter bc = TypeDescriptor.GetConverter(typeof(bool));
            writer.WriteElementString("SerialNumber", SerialNumber.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("ServoCount", _servoCount.ToString(CultureInfo.InvariantCulture));
            writer.WriteStartElement("Servos");
            for (int i = 0; i < _servoCount; i++)
            {
                writer.WriteStartElement("Servo");
                writer.WriteElementString("SpeedRamping", bc.ConvertToString(_servos[i].SpeedRamping));
                writer.WriteElementString("VelocityLimit", _servos[i].VelocityLimit.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Acceleration", _servos[i].Acceleration.ToString(CultureInfo.InvariantCulture));
                _servos[i].Calibration.Write(writer);

                writer.WriteElementString("Type", _servos[i].ServoType.ToString());
                writer.WriteElementString("MinPulse", _servos[i].ServoMinPulseWidth.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("MaxPulse", _servos[i].ServoMaxPulseWidth.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("MaxVelocity", _servos[i].ServoMaxVelocity.ToString(CultureInfo.InvariantCulture));
                writer.WriteElementString("Travel", _servos[i].ServoDegrees.ToString(CultureInfo.InvariantCulture));

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}

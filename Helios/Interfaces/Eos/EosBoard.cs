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

namespace GadrocsWorkshop.Helios.Interfaces.Eos
{
    using GadrocsWorkshop.Eos;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class EosBoard : NotificationObject
    {
        private WeakReference _interface = new WeakReference(null);

        private EosDevice _device;

        private byte _address;
        private bool _active;
        private string _firmware = "";
        private string _name;
        private byte _digitalInputCount;
        private byte _analogInputCount;
        private byte _ledOutputCount;
        private byte _textOutputCount;
        private byte _servoCount;
        private byte _stepperCount;
        private byte _coilCount;

        private EosInputCollection _inputs = new EosInputCollection();
        private EosOutputCollection _outputs = new EosOutputCollection();
        private List<EosDigitalInput> _digitalInputs = new List<EosDigitalInput>();
        private List<EosAnalogInput> _analogInputs = new List<EosAnalogInput>();
        private List<EosLedOutput> _ledOutputs = new List<EosLedOutput>();
        private List<EosTextOutput> _textOutputs = new List<EosTextOutput>();
        private List<EosServo> _servoOutputs = new List<EosServo>();
        private List<EosStepper> _stepperOutputs = new List<EosStepper>();
        private List<EosCoilOutput> _coilOutputs = new List<EosCoilOutput>();

        public EosBoard(EosDirectSerial eosInterface, byte address, string name)
        {
            _interface = new WeakReference(eosInterface);
            _address = address;
            _name = name;

            // Send out packet to get info on this board
            if (eosInterface.Bus != null)
            {
                eosInterface.Bus.GetInfo(address);
            }

            _outputs.Add(new EosBacklight(this));
        }

        public EosBoard(EosDirectSerial eosInterface, EosDevice device)
            : this(eosInterface, device.Address, device.Name)
        {
            Device = device;
        }

        #region Properties

        public EosDevice Device
        {
            get { return _device; }
            set
            {
                _device = value;
                Name = _device.Name;
                Firmware = _device.Firmware;
                DigitalInputCount = _device.DigitalInputs;
                AnalogInputCount = _device.AnalogInputs;
                LedOutputCount = _device.LedOutputs;
                TextOutputCount = _device.AlpahNumbericDisplays;
                ServoOutputCount = _device.ServoMotors;
                StepperOutputCount = _device.StepperMotors;
                CoilOutputCount = _device.CoilOutputs;
            }
        }

        public EosInputCollection Inputs
        {
            get
            {
                return _inputs;
            }
        }

        public EosOutputCollection Outputs
        {
            get
            {
                return _outputs;
            }
        }

        public byte Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (_device != null && _address != value)
                {
                    byte oldValue = _address;
                    _address = value;
                    _device.SetNodeAddress(_address);
                    OnPropertyChanged("Address", oldValue, value, false);
                }
            }
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    OnPropertyChanged("Active", !value, value, false);
                }
            }
        }

        public string Firmware
        {
            get { return _firmware; }
            private set
            {
                if (value != null && !_firmware.Equals(value))
                {
                    string oldValue = _firmware;
                    _firmware = value;
                    OnPropertyChanged("Firmware", oldValue, value, false);
                }
            }
        }

        public EosDirectSerial EOSInterface
        {
            get
            {
                return (EosDirectSerial)_interface.Target;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_device != null)
                {
                    if ((_name == null && value != null)
                        || (_name != null && !_name.Equals(value)))
                    {
                        string oldValue = _name;
                        _name = value;
                        _device.SetName(_name);
                        OnPropertyChanged("Name", oldValue, value, true);
                        foreach (EosInput input in Inputs)
                        {
                            foreach (IBindingTrigger trigger in input.Triggers)
                            {
                                trigger.Device = value;
                            }
                        }
                        foreach (EosOutput output in Outputs)
                        {
                            foreach (HeliosAction action in output.Actions)
                            {
                                action.Device = value;
                            }
                        }
                    }
                }
            }
        }

        public byte DigitalInputCount
        {
            get
            {
                return _digitalInputCount;
            }
            private set
            {
                if (_digitalInputCount != value)
                {
                    byte oldValue = _digitalInputCount;
                    _digitalInputCount = value;

                    for (byte i = oldValue; i < _digitalInputCount; i++)
                    {
                        EosDigitalInput input = new EosDigitalInput(this, i);
                        _digitalInputs.Add(input);
                        _inputs.Add(input);
                    }

                    while (_digitalInputs.Count > _digitalInputCount)
                    {
                        EosDigitalInput input = _digitalInputs.Last();
                        _digitalInputs.Remove(input);
                        _inputs.Remove(input);
                    }

                    OnPropertyChanged("DigitalInputCount", oldValue, value, false);
                }
            }
        }

        public byte LedOutputCount
        {
            get
            {
                return _ledOutputCount;
            }
            private set
            {
                if (_ledOutputCount != value)
                {
                    byte oldValue = _ledOutputCount;
                    _ledOutputCount = value;

                    for (byte i = oldValue; i < _ledOutputCount; i++)
                    {
                        EosLedOutput output = new EosLedOutput(this, i);
                        _ledOutputs.Add(output);
                        _outputs.Add(output);
                    }

                    while (_ledOutputs.Count > _ledOutputCount)
                    {
                        EosLedOutput output = _ledOutputs.Last();
                        _ledOutputs.Remove(output);
                        _outputs.Remove(output);
                    }

                    OnPropertyChanged("LedOutputCount", oldValue, value, false);
                }
            }
        }

        public byte StepperOutputCount
        {
            get
            {
                return _stepperCount;
            }
            private set
            {
                if (_stepperCount != value)
                {
                    byte oldValue = _stepperCount;
                    _stepperCount = value;

                    for (byte i = oldValue; i < _stepperCount; i++)
                    {
                        EosStepper output = new EosStepper(this, i);
                        _stepperOutputs.Add(output);
                        _outputs.Add(output);
                    }

                    while (_stepperOutputs.Count > _stepperCount)
                    {
                        EosStepper output = _stepperOutputs.Last();
                        _stepperOutputs.Remove(output);
                        _outputs.Remove(output);
                    }

                    OnPropertyChanged("StepperOutputCount", oldValue, value, false);
                }
            }
        }

        public byte ServoOutputCount
        {
            get
            {
                return _servoCount;
            }
            private set
            {
                if (_servoCount != value)
                {
                    byte oldValue = _servoCount;
                    _servoCount = value;

                    for (byte i = oldValue; i < _servoCount; i++)
                    {
                        EosServo output = new EosServo(this, i);
                        _servoOutputs.Add(output);
                        _outputs.Add(output);
                    }

                    while (_servoOutputs.Count > _servoCount)
                    {
                        EosServo output = _servoOutputs.Last();
                        _servoOutputs.Remove(output);
                        _outputs.Remove(output);
                    }

                    OnPropertyChanged("ServoOutputCount", oldValue, value, false);
                }
            }
        }

        public byte CoilOutputCount
        {
            get
            {
                return _coilCount;
            }
            private set
            {
                if (_coilCount != value)
                {
                    byte oldValue = _coilCount;
                    _coilCount = value;

                    for (byte i = oldValue; i < _coilCount; i++)
                    {
                        EosCoilOutput output = new EosCoilOutput(this, i);
                        _coilOutputs.Add(output);
                        _outputs.Add(output);
                    }

                    while (_coilOutputs.Count > _coilCount)
                    {
                        EosCoilOutput output = _coilOutputs.Last();
                        _coilOutputs.Remove(output);
                        _outputs.Remove(output);
                    }

                    OnPropertyChanged("CoilOutputCount", oldValue, value, false);
                }
            }
        }

        public byte AnalogInputCount
        {
            get
            {
                return _analogInputCount;
            }
            private set
            {
                if (_analogInputCount != value)
                {
                    byte oldValue = _analogInputCount;
                    _analogInputCount = value;

                    for (byte i = oldValue; i < _analogInputCount; i++)
                    {
                        EosAnalogInput input = new EosAnalogInput(this, i);
                        _analogInputs.Add(input);
                        _inputs.Add(input);
                    }

                    while (_analogInputs.Count > _analogInputCount)
                    {
                        EosAnalogInput input = _analogInputs.Last();
                        _analogInputs.Remove(input);
                        _inputs.Remove(input);
                    }

                    OnPropertyChanged("AnalogInputCount", oldValue, value, false);
                }
            }
        }

        public byte TextOutputCount
        {
            get
            {
                return _textOutputCount;
            }
            private set
            {
                if (_textOutputCount != value)
                {
                    byte oldValue = _textOutputCount;
                    _textOutputCount = value;

                    for (byte i = oldValue; i < _textOutputCount; i++)
                    {
                        EosTextOutput output = new EosTextOutput(this, i);
                        _textOutputs.Add(output);
                        _outputs.Add(output);
                    }

                    while (_textOutputs.Count > _textOutputCount)
                    {
                        EosTextOutput output = _textOutputs.Last();
                        _textOutputs.Remove(output);
                        _outputs.Remove(output);
                    }

                    OnPropertyChanged("TextOutputCount", oldValue, value, false);
                }
            }
        }

        public List<EosDigitalInput> DigitalInputs
        {
            get { return _digitalInputs; }
        }

        public List<EosAnalogInput> AnalogInputs
        {
            get { return _analogInputs; }
        }

        public List<EosLedOutput> LedOutputs
        {
            get { return _ledOutputs; }
        }

        public List<EosServo> ServoOutputs
        {
            get { return _servoOutputs; }
        }

        public List<EosStepper> StepperOutputs
        {
            get { return _stepperOutputs; }
        }

        public List<EosCoilOutput> CoilOutputs
        {
            get { return _coilOutputs; }
        }

        public List<EosTextOutput> TextOutputs
        {
            get { return _textOutputs; }
        }

        #endregion

        public void ProcessState()
        {
            if (_device != null)
            {
                for (byte number = 0; number < DigitalInputCount; number++)
                {
                    _digitalInputs[number].State = _device.DigitalState(number);
                }

                for (byte number = 0; number < AnalogInputCount; number++)
                {
                    _analogInputs[number].State = _device.AnanlogState(number);
                }
            }
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("DigitalInputs");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    byte number = byte.Parse(reader.GetAttribute("Number"));
                    EosDigitalInput input = new EosDigitalInput(this, number);
                    input.Name = reader.ReadElementString("DigitalInput");
                    _digitalInputs.Add(input);
                    _inputs.Add(input);
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read();
            }
            DigitalInputCount = (byte)DigitalInputs.Count;

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("AnalogInputs");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    byte number = byte.Parse(reader.GetAttribute("Number"));
                    EosAnalogInput input = new EosAnalogInput(this, number);
                    input.Name = reader.ReadElementString("AnalogInput");
                    _analogInputs.Add(input);
                    _inputs.Add(input);
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read();
            }
            AnalogInputCount = (byte)AnalogInputs.Count;

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("LEDOutputs");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    byte number = byte.Parse(reader.GetAttribute("Number"));
                    EosLedOutput output = new EosLedOutput(this, number);
                    output.Name = reader.ReadElementString("LEDOutput");
                    _ledOutputs.Add(output);
                    _outputs.Add(output);
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read();
            }
            LedOutputCount = (byte)LedOutputs.Count;

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("TextOutputs");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    byte number = byte.Parse(reader.GetAttribute("Number"));
                    EosTextOutput output = new EosTextOutput(this, number);
                    output.Name = reader.ReadElementString("TextOutput");
                    _textOutputs.Add(output);
                    _outputs.Add(output);
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read();
            }
            TextOutputCount = (byte)_textOutputs.Count;

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("ServoOutputs");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    byte number = byte.Parse(reader.GetAttribute("Number"));
                    EosServo output = new EosServo(this, number);
                    output.Name = reader.GetAttribute("Name");
                    // TODO: Deprectated fix for bad element name... remove after 1.6
                    if (reader.Name.Equals("ServerOutput"))
                    {
                        reader.ReadStartElement("ServerOutput");
                    }
                    else
                    {
                        reader.ReadStartElement("ServoOutput");
                    }                    
                    output.Calibration.Read(reader);
                    reader.ReadEndElement();
                    _servoOutputs.Add(output);
                    _outputs.Add(output);
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read();
            }
            ServoOutputCount = (byte)ServoOutputs.Count;

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("StepperOutputs");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    byte number = byte.Parse(reader.GetAttribute("Number"));
                    EosStepper output = new EosStepper(this, number);
                    output.Name = reader.GetAttribute("Name");
                    reader.ReadStartElement("StepperOutput");
                    output.Calibration.Read(reader);
                    reader.ReadEndElement();
                    _stepperOutputs.Add(output);
                    _outputs.Add(output);
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read();
            }
            StepperOutputCount = (byte)StepperOutputs.Count;

            if (reader.Name.Equals("CoilOutputs"))
            {
                if (!reader.IsEmptyElement)
                {
                    reader.ReadStartElement("CoilOutputs");
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        byte number = byte.Parse(reader.GetAttribute("Number"));
                        EosCoilOutput output = new EosCoilOutput(this, number);
                        output.Name = reader.GetAttribute("Name");
                        reader.ReadStartElement("CoilOutput");
                        output.Calibration.Read(reader);
                        reader.ReadEndElement();
                        _coilOutputs.Add(output);
                        _outputs.Add(output);
                    }
                    reader.ReadEndElement();
                }
                else
                {
                    reader.Read();
                }
            }
            CoilOutputCount = (byte)CoilOutputs.Count;
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("DigitalInputs");
            foreach (EosDigitalInput input in DigitalInputs)
            {
                writer.WriteStartElement("DigitalInput");
                writer.WriteAttributeString("Number", input.Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteString(input.Name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("AnalogInputs");
            foreach (EosAnalogInput input in AnalogInputs)
            {
                writer.WriteStartElement("AnalogInput");
                writer.WriteAttributeString("Number", input.Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteString(input.Name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("LEDOutputs");
            foreach (EosLedOutput output in LedOutputs)
            {
                writer.WriteStartElement("LEDOutput");
                writer.WriteAttributeString("Number", output.Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteString(output.Name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("TextOutputs");
            foreach (EosTextOutput output in TextOutputs)
            {
                writer.WriteStartElement("TextOutput");
                writer.WriteAttributeString("Number", output.Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteString(output.Name);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("ServoOutputs");
            foreach (EosServo output in ServoOutputs)
            {
                writer.WriteStartElement("ServoOutput");
                writer.WriteAttributeString("Number", output.Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteAttributeString("Name", output.Name);
                output.Calibration.Write(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("StepperOutputs");
            foreach (EosStepper output in StepperOutputs)
            {
                writer.WriteStartElement("StepperOutput");
                writer.WriteAttributeString("Number", output.Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteAttributeString("Name", output.Name);
                output.Calibration.Write(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("CoilOutputs");
            foreach (EosCoilOutput output in CoilOutputs)
            {
                writer.WriteStartElement("CoilOutput");
                writer.WriteAttributeString("Number", output.Number.ToString(System.Globalization.CultureInfo.InvariantCulture));
                writer.WriteAttributeString("Name", output.Name);
                output.Calibration.Write(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}

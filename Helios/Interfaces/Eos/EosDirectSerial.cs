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
    using GadrocsWorkshop.Helios.Collections;
    using GadrocsWorkshop.Helios.ComponentModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Xml;

    [HeliosInterface("Helios.EOS.DirectSerial", "EOS Bus", typeof(EosDirectSerialEditor))]
    public class EosDirectSerial : HeliosInterface
    {
        private EosBusType _type;
        private bool _scanning;
        private EosBus _bus;
        private string _comPort;

        NoResetObservablecollection<EosBoard> _boards;

        public event EosPacketEventHandler ResponseReceived;

        public EosDirectSerial()
            : base("EOS Bus")
        {
            _boards = new NoResetObservablecollection<EosBoard>();
            _boards.CollectionChanged += Boards_CollectionChanged;
        }

        void Boards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (EosBoard board in e.OldItems)
                {
                    if (board != null && (e.NewItems == null || !e.NewItems.Contains(board)))
                    {
                        foreach (EosInput input in board.Inputs)
                        {
                            foreach (IBindingTrigger trigger in input.Triggers)
                            {
                                Triggers.Remove(trigger);
                            }
                            input.Triggers.CollectionChanged -= InputTriggers_CollectionChanged;
                        }
                        board.Inputs.CollectionChanged -= BoardInputs_CollectionChanged;

                        foreach (EosOutput output in board.Outputs)
                        {
                            foreach (IBindingAction action in output.Actions)
                            {
                                Actions.Remove(action);
                            }
                            output.Actions.CollectionChanged -= OutputActions_CollectionChanged;
                        }
                        board.Outputs.CollectionChanged -= BoardOutputs_CollectionChanged;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (EosBoard board in e.NewItems)
                {
                    if (board != null && (e.OldItems == null || !e.OldItems.Contains(board)))
                    {
                        foreach (EosInput input in board.Inputs)
                        {
                            foreach (IBindingTrigger trigger in input.Triggers)
                            {
                                Triggers.Add(trigger);
                            }
                            input.Triggers.CollectionChanged -= InputTriggers_CollectionChanged;
                        }
                        board.Inputs.CollectionChanged += BoardInputs_CollectionChanged;

                        foreach (EosOutput output in board.Outputs)
                        {
                            foreach (IBindingAction action in output.Actions)
                            {
                                Actions.Add(action);
                            }
                            output.Actions.CollectionChanged += OutputActions_CollectionChanged;
                        }
                        board.Outputs.CollectionChanged += BoardOutputs_CollectionChanged;
                    }
                }
            }
        }

        void BoardOutputs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (EosOutput output in e.OldItems)
                {
                    if (output != null && (e.NewItems == null || !e.NewItems.Contains(output)))
                    {
                        foreach (IBindingAction action in output.Actions)
                        {
                            Actions.Remove(action);
                        }
                        output.Actions.CollectionChanged -= OutputActions_CollectionChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (EosOutput output in e.NewItems)
                {
                    if (output != null && (e.OldItems == null || !e.OldItems.Contains(output)))
                    {
                        foreach (IBindingAction action in output.Actions)
                        {
                            Actions.Add(action);
                        }
                        output.Actions.CollectionChanged += OutputActions_CollectionChanged;
                    }
                }
            }
        }

        void BoardInputs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (EosInput input in e.OldItems)
                {
                    if (input != null && (e.NewItems == null || !e.NewItems.Contains(input)))
                    {
                        foreach (IBindingTrigger trigger in input.Triggers)
                        {
                            Triggers.Remove(trigger);
                        }
                        input.Triggers.CollectionChanged -= InputTriggers_CollectionChanged;
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (EosInput input in e.NewItems)
                {
                    if (input != null && (e.OldItems == null || !e.OldItems.Contains(input)))
                    {
                        foreach (IBindingTrigger trigger in input.Triggers)
                        {
                            Triggers.Add(trigger);
                        }
                        input.Triggers.CollectionChanged += InputTriggers_CollectionChanged;
                    }
                }
            }
        }

        void OutputActions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (IBindingAction action in e.OldItems)
                {
                    if (action != null && (e.NewItems == null || !e.NewItems.Contains(action)))
                    {
                        Actions.Remove(action);
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (IBindingAction action in e.NewItems)
                {
                    if (action != null && (e.OldItems == null || !e.OldItems.Contains(action)))
                    {
                        Actions.Add(action);
                    }
                }
            }
        }

        void InputTriggers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (HeliosTrigger trigger in e.OldItems)
                {
                    if (trigger != null && (e.NewItems == null || !e.NewItems.Contains(trigger)))
                    {
                        Triggers.Remove(trigger);
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach (HeliosTrigger trigger in e.NewItems)
                {
                    if (trigger != null && (e.OldItems == null || !e.OldItems.Contains(trigger)))
                    {
                        Triggers.Add(trigger);
                    }
                }
            }
        }

        #region Properties

        public bool IsConnected
        {
            get { return _bus == null ? false : _bus.IsConnected; }
        }

        public EosBus Bus
        {
            get { return _bus; }
        }

        public NoResetObservablecollection<EosBoard> Boards
        {
            get { return _boards; }
        }

        public string Port
        {
            get
            {
                return _comPort;
            }
            set
            {
                if ((_comPort == null && value != null)
                    || (_comPort != null && !_comPort.Equals(value)))
                {
                    string oldValue = _comPort;
                    _comPort = value;
                    OnPropertyChanged("Port", oldValue, value, true);
                    SetupBus();
                }
            }
        }

        public EosBusType BusType
        {
            get
            {
                return _type;
            }
            set
            {
                if (!_type.Equals(value))
                {
                    EosBusType oldValue = _type;
                    _type = value;
                    OnPropertyChanged("BusType", oldValue, value, true);
                    SetupBus();
                }
            }
        }

        private void SetupBus()
        {
            if (_bus != null)
            {
                Disconnect();
                _bus.PropertyChanged -= BusPropertyChanged;
                _bus.DeviceUpdate -= BusDeviceUpdated;
                _bus.ResponseReceived -= BusResponseReceived;
            }

            if (_comPort != null && _comPort.Length > 0)
            {
                switch (_type)
                {
                    case EosBusType.BusMaster:
                        _bus = new EosBusMaster(_comPort, 250000);
                        break;
                    case EosBusType.BusInterface:
                        _bus = new EosBusInterfaceSerial(_comPort, 250000);
                        break;
                }

                IsScanning = true;
                _bus.PropertyChanged += BusPropertyChanged;
                _bus.DeviceUpdate += BusDeviceUpdated;
                _bus.ResponseReceived += BusResponseReceived;
            }
        }

        public bool IsScanning
        {
            get
            {
                return _scanning;
            }
            set
            {
                if (_scanning != value)
                {
                    _scanning = value;
                    OnPropertyChanged("IsScanning", !value, value, false);
                    if (!_scanning)
                    {
                        Dispatcher.BeginInvoke(new Action(ResetBoardList));
                    }
                }
            }
        }

        #endregion

        public void Connect()
        {
            if (_bus != null && !IsConnected)
            {
                if (System.IO.Ports.SerialPort.GetPortNames().Contains(_comPort))
                {
                    _bus.Connect();
                    _bus.StartPolling();
                    _bus.Rescan();
                    ResetBoardList();
                }
            }
        }

        public void Disconnect()
        {
            if (_bus != null && IsConnected)
            {
                foreach (EosBoard board in Boards)
                {
                    foreach (EosStepper stepper in board.StepperOutputs)
                    {
                        if (board.Device != null)
                        {
                            board.Device.SetStepperTargetPosition(stepper.Number, 0);
                        }
                    }
                }
                _bus.StopPolling();
                _bus.Disconnect();
            }
        }

        public void Rescan()
        {
            if (_bus != null)
            {
                _bus.Connect();
                _bus.Rescan();
            }
        }

        protected override void OnProfileChanged(HeliosProfile oldProfile)
        {
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
            base.OnProfileChanged(oldProfile);
        }

        private void Profile_ProfileStarted(object sender, EventArgs e)
        {
            Connect();
            // Prevent profile from continuing until bus is initialized.
            Thread.Sleep(250);
            while (IsScanning)
            {
                Thread.Sleep(250);
            }
        }

        private void Profile_ProfileStopped(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void ResetBoardList()
        {
            foreach (EosBoard board in _boards)
            {
                board.Active = false;
            }

            foreach (EosDevice device in _bus.Devices)
            {
                bool found = false;
                foreach (EosBoard board in _boards)
                {
                    if (board.Address == device.Address)
                    {
                        found = true;
                        board.Active = true;
                        board.Device = device;
                        board.ProcessState();
                    }
                }
                if (!found)
                {
                    EosBoard board = new EosBoard(this, device);
                    board.Active = true;
                    _boards.Add(board);
                }
            }
        }

        #region Bus Event Handlers

        void BusDeviceUpdated(object sender, EosDeviceEventArgs e)
        {
            foreach (EosBoard board in _boards)
            {
                if (e.Device.Address == board.Address)
                {
                    board.ProcessState();
                }
            }
        }

        private void BusPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsScanning = (_bus.State == EosBus.EosBusState.SCANNING);
        }

        private void BusResponseReceived(object sender, EosPacketEventArgs e)
        {
            EosPacketEventHandler handler = ResponseReceived;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }

        #endregion

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.Name.Equals("Type"))
            {
                BusType = (EosBusType)Enum.Parse(typeof(EosBusType), reader.ReadElementString("Type"));
            }
            string port = reader.ReadElementString("Port");

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement("Boards");
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("Board");
                    string name = reader.ReadElementString("Name");
                    byte address = byte.Parse(reader.ReadElementString("Address"), System.Globalization.CultureInfo.InvariantCulture);
                    EosBoard board = new EosBoard(this, address, name);
                    board.ReadXml(reader);
                    reader.ReadEndElement();
                    _boards.Add(board);
                }
                reader.ReadEndElement();
            }
            else
            {
                reader.Read();
            }

            Port = port;
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("Type", BusType.ToString());
            writer.WriteElementString("Port", Port);
            writer.WriteStartElement("Boards");
            foreach (EosBoard board in _boards)
            {
                writer.WriteStartElement("Board");
                writer.WriteElementString("Name", board.Name);
                writer.WriteElementString("Address", board.Address.ToString(System.Globalization.CultureInfo.InvariantCulture));
                board.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
    }
}
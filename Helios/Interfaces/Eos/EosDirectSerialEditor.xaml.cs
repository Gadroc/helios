
namespace GadrocsWorkshop.Helios.Interfaces.Eos
{
    using GadrocsWorkshop.Eos;
    using GadrocsWorkshop.Helios.Windows.Controls;
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for EosDirectSerialEditor.xaml
    /// </summary>
    public partial class EosDirectSerialEditor : HeliosInterfaceEditor
    {
        public EosDirectSerialEditor()
        {
            InitializeComponent();
        }

        public string[] SerialPorts
        {
            get
            {
                return System.IO.Ports.SerialPort.GetPortNames();
            }
        }

        protected override void OnInterfaceChanged(Helios.HeliosInterface oldInterface, Helios.HeliosInterface newInterface)
        {
            EosDirectSerial oldSerialInterface = oldInterface as EosDirectSerial;
            if (oldSerialInterface != null)
            {
                oldSerialInterface.Bus.ResponseReceived -= Bus_ResponseReceived;
            }

            EosDirectSerial serialInterface = newInterface as EosDirectSerial;
            if (serialInterface != null)
            {
                serialInterface.Dispatcher = Dispatcher;
                serialInterface.ResponseReceived += Bus_ResponseReceived;
            }
        }

        private void Bus_ResponseReceived(object sender, EosPacketEventArgs e)
        {
            if (e.Packet != null)
            {
                Dispatcher.Invoke((Action<EosPacket>)UpdateServo, e.Packet);
            }
        }

        private void UpdateServo(EosPacket packet)
        {
            EosDirectSerial bus = Interface as EosDirectSerial;
            foreach (EosBoard board in bus.Boards)
            {
                foreach (EosServo servo in board.ServoOutputs)
                {
                    servo.ParseConfigPacket(packet);
                }
                foreach (EosStepper stepper in board.StepperOutputs)
                {
                    stepper.ParseConfigPacket(packet);
                }
            }
        }

        private void BoardSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EosDirectSerial bus = Interface as EosDirectSerial;
            EosBoard board = BoardListBox.SelectedItem as EosBoard;

            DigitalInputsTab.IsEnabled = board.DigitalInputCount > 0;
            LedTab.IsEnabled = board.LedOutputCount > 0;
            AnalogInputsTab.IsEnabled = board.AnalogInputCount > 0;
            ServoTab.IsEnabled = board.ServoOutputCount > 0;
            StepperTab.IsEnabled = board.StepperOutputCount > 0;

            if (board.ServoOutputCount > 0)
            {
                ServoSelector.SelectedIndex = 0;
            }

            if (board.StepperOutputCount > 0)
            {
                StepperSelector.SelectedIndex = 0;
            }

            TabItem selectedTab = PropertyTab.SelectedItem as TabItem;
            if (selectedTab.IsEnabled == false)
            {
                PropertyTab.SelectedItem = PropertiesTab;
            }

            for (byte i = 0; i < board.ServoOutputCount; i++)
            {
                if (board.Device != null)
                {
                    board.Device.GetServoConfig(i);
                }
            }
            for (byte i = 0; i < board.StepperOutputCount; i++)
            {
                if (board.Device != null)
                {
                    board.Device.GetStepperConfig(i);
                }
            }
        }

        public override void Closed()
        {
            EosDirectSerial eosDirectSerial = this.Interface as EosDirectSerial;
            if (eosDirectSerial != null)
                eosDirectSerial.Disconnect();
            base.Closed();
        }

        private void BacklightOn_Click(object sender, RoutedEventArgs e)
        {
            EosBoard eosBoard = this.BoardListBox.SelectedItem as EosBoard;
            if (eosBoard == null)
                return;
            eosBoard.Device.SetBacklightPower(true);
        }

        private void BacklightOff_Click(object sender, RoutedEventArgs e)
        {
            EosBoard eosBoard = this.BoardListBox.SelectedItem as EosBoard;
            if (eosBoard == null)
                return;
            eosBoard.Device.SetBacklightPower(false);
        }

        private void BacklighLevel_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            EosBoard eosBoard = this.BoardListBox.SelectedItem as EosBoard;
            if (eosBoard == null)
                return;
            eosBoard.Device.SetBacklightLevel((byte)e.NewValue);
        }

        private void PortSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EosDirectSerial eosDirectSerial = this.Interface as EosDirectSerial;
            if (eosDirectSerial == null)
                return;
            eosDirectSerial.Connect();
        }

        private void Rescan_Clicked(object sender, RoutedEventArgs e)
        {
            EosDirectSerial eosDirectSerial = this.Interface as EosDirectSerial;
            if (eosDirectSerial == null || !eosDirectSerial.IsConnected)
                return;
            eosDirectSerial.Rescan();
        }

        private void LedOn_Click(object sender, RoutedEventArgs e)
        {
            EosBoard eosBoard = this.BoardListBox.SelectedItem as EosBoard;
            Button button = e.Source as Button;
            if (eosBoard == null || button == null)
                return;
            byte led = (byte)button.Tag;
            eosBoard.Device.SetLedPower(led, true);
        }

        private void LedOff_Click(object sender, RoutedEventArgs e)
        {
            EosBoard eosBoard = this.BoardListBox.SelectedItem as EosBoard;
            Button button = e.Source as Button;
            if (eosBoard == null || button == null)
                return;
            byte led = (byte)button.Tag;
            eosBoard.Device.SetLedPower(led, false);
        }

        private void ServoTestValueChanged(object sender, TextChangedEventArgs e)
        {
            (this.ServoSelector.SelectedItem as EosServo).ServoValue.ExecuteAction(new BindingValue(this.ServoTestTextBox.Text), true);
        }

        private void StepperTestValueChanged(object sender, TextChangedEventArgs e)
        {
            (this.StepperSelector.SelectedItem as EosStepper).StepperValue.ExecuteAction(new BindingValue(this.StepperTestTextBox.Text), true);
        }

        private void Zero_Click(object sender, RoutedEventArgs e)
        {
            (this.BoardListBox.SelectedItem as EosBoard).Device.ZeroStepperPosition((this.StepperSelector.SelectedItem as EosStepper).Number);
        }
    }
}

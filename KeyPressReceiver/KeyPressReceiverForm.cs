//  Copyright 2019 Piet Van Nes
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace GadrocsWorkshop.Helios.KeyPressReceiver
{
    public partial class KeyPressReceiverForm : Form
    {
        Boolean DontClose = true;
        private delegate void SafeCallDelegate(string text);
        public KeyPressReceiverForm()
        {
            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tbSvrAddress.Text = Properties.Settings.Default.ServerAddress;
            tbSvrPort.Text = "9088"; //Properties.Settings.Default.ServerPort.ToString();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Visible = true;
            this.BringToFront();
        }

        private void KeyPressReceiverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) && (DontClose))
            {
                e.Cancel = true;
                this.Hide();
            }
        }
        private void OnTCPStatusChanged(string dataIn)
        {
            if (this.DontClose == false) { return; }
            //Handle multi-threading
            if (InvokeRequired)
            {
                Invoke(new SafeCallDelegate(OnTCPStatusChanged), new object[] { dataIn });
                return;
            }
            TCPClient TCPClient = TCPClient.Instance;
            if (TCPClient.ClientConnected)
            {
                timer1.Interval = 60000; // 60 seconds still connected test
                lbStatus.Text = "Connected";
                lbStatus.ForeColor = Color.Green;
            }
            else
            {
                timer1.Interval = 10000; // 10 second test to try to connect
                lbStatus.Text = "Disconnected";
                lbStatus.ForeColor = Color.Red;
            }
        }


        private void OnTCPDataReceived(string dataIn)
        {
            if (this.DontClose == false) { return; }
            //Handle multi-threading
            if (InvokeRequired)
            {
                Invoke(new SafeCallDelegate(OnTCPDataReceived), new object[] { dataIn });
                return;
            }
            // Do something with the keyboard event
            string Heartbeat = "HEARTBEAT";
            if (dataIn.Contains(Heartbeat))
            {
                // remove heatbeat and process other messages
                while (dataIn.Contains(Heartbeat))
                {
                    dataIn = dataIn.Remove(dataIn.IndexOf(Heartbeat), Heartbeat.Length);
                }
            }
            if (dataIn.Length == 0)
                return; // now an empty message

            NativeMethods.INPUT keyEvent = new NativeMethods.INPUT();
            int size = Marshal.SizeOf(keyEvent);
            while (dataIn.Length >= size)
            {
                string sThisData = dataIn.Substring(0, size);
                dataIn = dataIn.Remove(0, size);
                IntPtr ptr = Marshal.AllocHGlobal(size);
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(sThisData);
                Marshal.Copy(buffer, 0, ptr, size);
                keyEvent = (NativeMethods.INPUT)Marshal.PtrToStructure(ptr, keyEvent.GetType());
                Marshal.FreeHGlobal(ptr);
                NativeMethods.SendInput(1, new NativeMethods.INPUT[] { keyEvent }, Marshal.SizeOf(keyEvent));
            }
        }

        private void KeyPressReceiverForm_Load(object sender, EventArgs e)
        {
            tbSvrAddress.Text = Properties.Settings.Default.ServerAddress;
            //tbSvrPort.Text = "5009"; //Properties.Settings.Default.ServerPort.ToString();
            tbSvrPort.Text = Properties.Settings.Default.ServerPort.ToString();

            TCPClient TCPClient = TCPClient.Instance;
            TCPClient.DataReceived += OnTCPDataReceived;
            TCPClient.StatusChanged += OnTCPStatusChanged;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DontClose = false;
            TCPClient.Instance.Close();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            TCPClient TCPClient = TCPClient.Instance;
            if (!TCPClient.ClientConnected)
            {
                TCPClient.Open();
            }
            if (TCPClient.ClientConnected)
            {
                timer1.Interval = 60000; // 60 seconds still connected test
                lbStatus.Text = "Connected";
                lbStatus.ForeColor = Color.Green;
                timer1.Interval = 60000; // 60 seconds still connected test
                TCPClient.Send("HEARTBEAT");
            }
            else
            {
                timer1.Interval = 10000; // 10 second test to try to connect
                lbStatus.Text = "Disconnected";
                lbStatus.ForeColor = Color.Red;
                timer1.Interval = 10000; // 10 second test to try to connect
            }
            timer1.Enabled = true;
        }

        private void KeyPressReceiverForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                //this.WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = true;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = true;
            }
        }

        private void bnCancel_Click(object sender, EventArgs e)
        {
            tbSvrAddress.Text = Properties.Settings.Default.ServerAddress;
            tbSvrPort.Text = Properties.Settings.Default.ServerPort.ToString();
            this.Hide();
        }

        private void bnSave_Click(object sender, EventArgs e)
        {
            Boolean _validArgs = false;
            Properties.Settings.Default.ServerAddress = tbSvrAddress.Text;
            if (Int16.TryParse(tbSvrPort.Text, out Int16 j))
            {
                Properties.Settings.Default.ServerPort = j;
                _validArgs = true;
            }
            else
                Console.WriteLine("Keypress Receiver Error:  Port value string could not be parsed to integer. " + tbSvrPort.Text);
            if (_validArgs)
            {
                Properties.Settings.Default.Save();
                TCPClient.Instance.Close();
                lbStatus.Text = "Disconnected";
                lbStatus.ForeColor = Color.Red;
                TCPClient.Instance.Open();
                timer1.Interval = 5000;
                //this.Hide();
                this.WindowState = FormWindowState.Minimized;
            }
        }


        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbSvrAddress.Text = Properties.Settings.Default.ServerAddress;
            tbSvrPort.Text = Properties.Settings.Default.ServerPort.ToString();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Visible = true;
            this.BringToFront();
        }

        private void tbSvrAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbSvrPort_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

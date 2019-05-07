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

namespace KeyPressReceiver
{
    public partial class Form1 : Form
    {
        Boolean DontClose = true;
        private delegate void SafeCallDelegate(string text);
        public Form1()
        {
            InitializeComponent();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tbSvrAddress.Text = Properties.Settings.Default.ServerAddress;
            tbSvrPort.Text = Properties.Settings.Default.ServerPort.ToString();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Visible = true;
            this.BringToFront();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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
            if (dataIn.StartsWith("HEARTBEAT"))
            {
                // ignore heartbeat responses
                return;
            }


            NativeMethods.INPUT keyEvent = new NativeMethods.INPUT();
            int size = Marshal.SizeOf(keyEvent);
            if (dataIn.Length != size)
            {
                // Error handling TBA - dont normally get TCP fragments on a LAN so not critical 
                return;
            }


            IntPtr ptr = Marshal.AllocHGlobal(size);
            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(dataIn);
            Marshal.Copy(buffer, 0, ptr, size);
            keyEvent = (NativeMethods.INPUT)Marshal.PtrToStructure(ptr, keyEvent.GetType());
            Marshal.FreeHGlobal(ptr);
            NativeMethods.SendInput(1, new NativeMethods.INPUT[] { keyEvent }, Marshal.SizeOf(keyEvent));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

        private void Form1_Resize(object sender, EventArgs e)
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
            Properties.Settings.Default.ServerAddress = tbSvrAddress.Text;
            Properties.Settings.Default.ServerPort = Convert.ToInt16(tbSvrPort.Text);
            Properties.Settings.Default.Save();
            TCPClient.Instance.Close();
            lbStatus.Text = "Disconnected";
            lbStatus.ForeColor = Color.Red;
            TCPClient.Instance.Open();
            timer1.Interval = 5000;
            this.Hide();
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
    }
}

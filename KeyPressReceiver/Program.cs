using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace KeyPressReceiver
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static Mutex mutex = new Mutex(false, "Helios.KeypressReceiver");
    [STAThread]
    static void Main()
    {
      // Only allow one instance of the Receiver - added due to user error
      if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
      {
        MessageBox.Show("Another instance is already running.\r\nApplication shutting down.", "Helios Keypress Receiver", MessageBoxButtons.OK);
        return;
      }
      try
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
      }
      finally
      {
        mutex.ReleaseMutex();
      }
    }
  }
}

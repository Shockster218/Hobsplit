using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace HobbitAutosplitter
{
    public static class ProcessManager
    {
        private static Process obs;

        public static event EventHandler OBSProcessFoundEvent;
        public static event EventHandler OBSOpenedEvent;
        public static event EventHandler OBSClosedEvent;

        public static bool obsRunning;

        public static void Init()
        {
            OBSProcessFoundEvent += (s,e) => WaitForOBS();
            OBSOpenedEvent += (s,e) => IsOBSRunning();
            CaptureManager.DoneCapturingEvent += (s, e) => FindOBS();
            _ = Task.Factory.StartNew(() => FindOBS());
        }

        private static void FindOBS()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                MainWindow.instance.OBSOffline();
            });

            while (true)
            {
                Process obsProcess = Process.GetProcesses().Where(x => x.ProcessName.Contains("obs")).FirstOrDefault(x => x.ProcessName.Any(char.IsDigit));
                if (obsProcess != null)
                {
                    obs = obsProcess;
                    OBSProcessFoundEvent?.Invoke(null, EventArgs.Empty);
                    return;
                }
            }
        }

        private static void WaitForOBS()
        {
            while (!IsWindowVisible(obs.MainWindowHandle)) { continue; }
            obsRunning = true;
            OBSOpenedEvent?.AsyncInvoke(null, EventArgs.Empty);
        }

        private static void IsOBSRunning()
        {
            while (obsRunning)
            {
                try
                {
                    Process.GetProcessById(obs.Id);
                }
                catch (ArgumentException)
                {
                    obsRunning = false;
                    obs = null;
                    OBSClosedEvent?.AsyncInvoke(null, EventArgs.Empty);
                }
            }
        }

        public static Process GetOBS()
        {
            return obs;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);
    }
}

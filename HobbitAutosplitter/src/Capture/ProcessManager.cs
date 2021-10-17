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

        public static event SmartEventHandler OBSProcessFoundEvent;
        public static event SmartEventHandler OBSOpenedEvent;
        public static event SmartEventHandler OBSClosedEvent;

        public static bool obsRunning;

        public static void Init()
        {
            OBSProcessFoundEvent += WaitForOBS;
            OBSOpenedEvent += IsOBSRunning;
            CaptureManager.DoneCapturingEvent += (e) => FindOBS();
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
                    OBSProcessFoundEvent?.SmartInvoke(new SmartInvokeArgs(InvokeMode.SYNC));
                    return;
                }
            }
        }

        private static void WaitForOBS(SmartInvokeArgs frameArgs)
        {
            while (!IsWindowVisible(obs.MainWindowHandle)) { continue; }
            obsRunning = true;
            OBSOpenedEvent?.SmartInvoke(new SmartInvokeArgs(InvokeMode.ASYNC));
        }

        private static void IsOBSRunning(SmartInvokeArgs frameArgs)
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
                    OBSClosedEvent?.SmartInvoke(new SmartInvokeArgs(InvokeMode.ASYNC));
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

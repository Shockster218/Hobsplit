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

        public static event EventHandler OBSOpenedEvent;

        public static void Init()
        {
            _ = Task.Factory.StartNew(() => FindOBS());
        }

        public static void FindOBS()
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
                    OBSOpenedEvent?.Invoke(null, EventArgs.Empty);
                    return;
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

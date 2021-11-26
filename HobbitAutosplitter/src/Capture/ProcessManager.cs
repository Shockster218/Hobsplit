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
        }

        public static void FindOBS()
        {
            while (true)
            {
                Process obsProcess = Process.GetProcesses().Where(x => x.ProcessName.Contains("obs")).FirstOrDefault(x => x.ProcessName.Any(char.IsDigit));
                if (obsProcess != null)
                {
                    obs = obsProcess;
                    obs.EnableRaisingEvents = true;
                    obs.Exited += (s,e) => OBSClosed();
                    OBSProcessFoundEvent?.SmartInvoke();
                    return;
                }
            }
        }

        private static void WaitForOBS()
        {
            while (!IsWindowVisible(obs.MainWindowHandle)) { continue; }
            obsRunning = true;
            OBSOpenedEvent?.SmartInvoke();
        }

        private async static void OBSClosed()
        {
            obsRunning = false;
            obs = null;
            await Task.Delay(100);
            OBSClosedEvent?.SmartInvoke();
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

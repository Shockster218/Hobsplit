using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Hobsplit
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
            obs = null;
            OBSProcessFoundEvent += WaitForOBS;
        }

        public static void FindOBS()
        {
            while (true)
            {
                Process obsProcess = Process.GetProcesses().Where(x => x.ProcessName.Contains("obs")).FirstOrDefault(x => x.ProcessName.Any(char.IsDigit));
                if (Settings.Default.autoOBS && obsProcess == null)
                {
                    try
                    {
                        string path = Settings.Default.obsPath;
                        Process obsAuto = new Process();
                        obsAuto.StartInfo.WorkingDirectory = Path.GetDirectoryName(path);
                        obsAuto.StartInfo.FileName = Path.GetFileName(path);
                        obsAuto.Start();
                        if (obsAuto != null)
                        {
                            if (obsAuto.ProcessName.Contains("obs"))
                            {
                                OBSFound(obsAuto);
                                return;
                            }
                            else obsAuto.Close();
                        }
                    }
                    catch { }

                    Settings.Default.autoOBS = false;
                    Settings.Default.obsPath = "No path set!";
                }
                else if(obsProcess != null)
                {
                    OBSFound(obsProcess);
                    return;
                }
            }
        }

        private static void OBSFound(Process obsProcess)
        {
            obs = obsProcess;
            obs.EnableRaisingEvents = true;
            obs.Exited += (s, e) => OBSClosed();
            OBSProcessFoundEvent?.SmartInvoke();
        }

        private static async void WaitForOBS()
        {
            while (obs.MainWindowHandle == IntPtr.Zero) { await Task.Delay(100); }
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

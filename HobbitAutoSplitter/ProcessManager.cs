using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace HobbitAutoSplitter
{
    public class ProcessManager
    {
        public Process process { get; private set; }
        public ProcessBounds processBounds { get; private set; }

        public ProcessManager(Process process)
        {
            this.process = process.Site == null ? StartOBS() : process;
            //processBounds = new ProcessBounds(this, process);
        }

        private Process StartOBS()
        {
            ProcessStartInfo info = new ProcessStartInfo("cmd.exe");
            try
            {
                string path = Settings.Default.obsPath;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.UseShellExecute = true;
                info.Arguments = $"/C {Path.GetPathRoot(path).Remove(2, 1)} & cd {Path.GetDirectoryName(path)} & {Path.GetFileName(path)}";
                Process.Start(info);
                return Process.GetProcesses().Where(x => x.ProcessName.Contains("obs")).FirstOrDefault();
            }
            catch
            {
                MainWindow.instance.NoOBSPath();
            }
            return null;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        public struct ProcessBounds
        {
            public int x { get; private set; }
            public int y { get; private set; }
            public int width { get; private set; }
            public int height { get; private set; }

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

            [DllImport("user32.dll")]
            static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll")]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            public ProcessBounds(ProcessManager pm, Process p)
            {
                RECT rect;
                ShowWindow(p.MainWindowHandle, 3);
                SetForegroundWindow(p.MainWindowHandle);
                GetWindowRect(new HandleRef(pm, p.MainWindowHandle), out rect);
                x = rect.Left;
                y = rect.Top;
                width = rect.Right - rect.Left;
                height = rect.Bottom - rect.Top;
            }
        }
    }
}

using System;
using System.Windows;
using System.Linq;
using System.Diagnostics;

namespace HobbitAutoSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;
        public ProcessManager OBS = null;
        
        public MainWindow()
        {
            instance = this;
            InitializeComponent();
            CheckForOBS();
        }

        private void CheckForOBS()
        {
            Process p = new Process();
            string obsPath = Settings.Default.obsPath;
            p = Process.GetProcesses().Where(x => x.ProcessName.Contains("obs")).FirstOrDefault();
            if (p == null)
            {
                if (string.IsNullOrEmpty(obsPath))
                {
                    MessageBoxResult result = MessageBox.Show("Could not find OBS. Please make sure you have OBS open before continuing.", "Couldn't find OBS", MessageBoxButton.OKCancel);
                    if (result == MessageBoxResult.OK)
                    {
                        CheckForOBS();
                        return;
                    }
                    else
                    {
                        CloseWindow();
                        return;
                    }
                }
            }
            else
            {
                obsPath = p.MainModule.FileName;
                Settings.Default.obsPath = obsPath;
            }

            OBS = new ProcessManager(p);
        }

        public void NoOBSPath()
        {
            MessageBox.Show("Couldn't find OBS path. Did you uninstall or move the directory? Exiting program.");
            CloseWindow();
        }

        private void CloseWindow(object sender = null, EventArgs e = null)
        {
            Settings.Default.Save();
            Application.Current.Shutdown();
        }
    }
}

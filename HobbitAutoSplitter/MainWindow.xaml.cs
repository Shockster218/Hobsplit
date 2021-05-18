using System;
using System.Windows;
using System.Linq;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

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
            string obsPath = Settings.Default.obsDirectory;
            if (string.IsNullOrEmpty(obsPath))
            {
                MessageBoxResult result = MessageBox.Show("Please set your OBS path before continuing.", "Couldn't find OBS", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Filter = "OBS Executable | *.exe";
                    if(dialog.ShowDialog() == true)
                    {
                        Settings.Default.obsDirectory = Path.GetDirectoryName(dialog.FileName);
                        Settings.Default.obsFileName = Path.GetFileName(dialog.FileName);
                    }
                }
                else
                {
                    CloseWindow();
                }
            }

            OBS = new ProcessManager();
        }

        public void NoOBSPath()
        {
            MessageBox.Show("Couldn't find OBS path. Did you uninstall or move the directory? Exiting program.");
            Settings.Default.obsDirectory = String.Empty;
            Settings.Default.obsFileName = String.Empty;
            CloseWindow();
        }

        private void CloseWindow(object sender = null, EventArgs e = null)
        {
            Settings.Default.Save();
            Application.Current.Shutdown();
        }
    }
}

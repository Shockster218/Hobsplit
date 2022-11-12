using System.Windows;
using System.Threading.Tasks;

namespace HobbitAutosplitter
{
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void InitiateStartup(object sender, System.EventArgs e)
        {
            // 1. Update Settings if new version. Check this first before setup to see if they need it.
            // 2. Check first time installation.
            // 3. Initialize all Managers.
            // 4. Check for OBS process.
            // 5. Start main window AFTER obs is found.

            Status_Textblock.Text = "Updating Settings...";

            if (Settings.Default.updateRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.updateRequired = false;
                Settings.Default.Save();
            }

            Status_Textblock.Text = "Checking Setup...";

            //if (Settings.Default.needSetup)
            //{
            //    SetupWindow win = new SetupWindow();
            //    win.Show();
            //    Close();
            //    return;
            //}

            Status_Textblock.Text = "Initializing Managers...";

            if (!SplitManager.Init())
            {
                ComparisonSettingsWindow win = new ComparisonSettingsWindow(true);
                win.Show();
                Close();
                return;
            }

            LivesplitManager.Init();
            CaptureManager.Init();
            ProcessManager.Init();

            Status_Textblock.Text = "Waiting for OBS...";

            ProcessManager.OBSOpenedEvent += OBSFound;
            Task.Factory.StartNew(() => ProcessManager.FindOBS());
        }

        public void OBSFound()
        {
            Status_Textblock.Text = "Starting...";

            MainWindow win = new MainWindow();
            win.Show();
            Close();
            return;
        }
    }
}

using System.Windows;
using System.Windows.Forms;

namespace HobbitAutosplitter
{
    public partial class SplitImagesWindow : Window
    {
        public bool fromStartup;
        public SplitImagesWindow()
        {
            InitializeComponent();
        }

        public SplitImagesWindow(bool fromStartup = false)
        {
            InitializeComponent();
            this.fromStartup = fromStartup;
        }

        public void UpdateSplitImages()
        {
            Splits_Control.LoadSplitImagePaths();
            SaveSplitImagePaths();
            Splits_Control.SetSplitImages(true);
        }

        private void Comparison_Settings_Window_ContentRendered(object sender, System.EventArgs e)
        {
            if (fromStartup)
            {
                ShowErrorMessage();
            }
        }

        private void ShowErrorMessage()
        {
            System.Windows.Forms.MessageBox.Show("An error occured when trying to load your split images. Please reset any broken splits.", "Fix split images", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            SaveSplitImagePaths();
            if (fromStartup) CloseFromStartup();
            else Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CloseFromStartup()
        {
            StartupWindow win = new StartupWindow();
            win.Show();
            Close();
            fromStartup = false;
            return;
        } 

        private void SaveSplitImagePaths()
        {
            Settings.Default.menuPath = Splits_Control.menuPath;
            SplitManager.UpdateSplit(1, Settings.Default.menuPath);
            Settings.Default.dwPath = Splits_Control.dwPath;
            Settings.Default.aupPath = Splits_Control.aupPath;
            Settings.Default.rmPath = Splits_Control.rmPath;
            Settings.Default.thPath = Splits_Control.thPath;
            Settings.Default.ohPath = Splits_Control.ohPath;
            Settings.Default.riddlesPath = Splits_Control.riddlesPath;
            Settings.Default.fasPath = Splits_Control.fasPath;
            Settings.Default.boobPath = Splits_Control.boobPath;
            Settings.Default.awwPath = Splits_Control.awwPath;
            SplitManager.UpdateSplit(12, Settings.Default.awwPath);
            Settings.Default.thiefPath = Splits_Control.thiefPath;
            Settings.Default.iiPath = Splits_Control.iiPath;
            Settings.Default.gotcPath = Splits_Control.gotcPath;
            Settings.Default.tcbPath = Splits_Control.tcbPath;
            Settings.Default.finalPath = Splits_Control.finalPath;
            Settings.Default.Save();
        }
    }
}

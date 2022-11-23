using System.Windows;
using System.Windows.Forms;

namespace Hobsplit
{
    public partial class SplitImagesWindow : Window
    {
        bool startup = false;
        public SplitImagesWindow()
        {
            InitializeComponent();
        }

        public SplitImagesWindow(bool startup)
        {
            InitializeComponent();
            this.startup = startup;
        }

        public void UpdateSplitImages()
        {
            Splits_Control.SetSplitImages();
        }

        private void Split_Images_Window_ContentRendered(object sender, System.EventArgs e)
        {
            Splits_Control.SetSplitImages();
            if (startup) ShowErrorMessage();
        }

        private void ShowErrorMessage()
        {
            Cancel_Button.IsEnabled = false;
            System.Windows.Forms.MessageBox.Show("An error occured when trying to load your split images. Please reset any broken splits.", "Fix split images", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
            if (startup) CloseFromStartup();
            else Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            if (startup) CloseFromStartup();
            else Close();
        }

        private void CloseFromStartup()
        {
            StartupWindow win = new StartupWindow();
            win.Show();
            Close();
            return;
        }
    }
}

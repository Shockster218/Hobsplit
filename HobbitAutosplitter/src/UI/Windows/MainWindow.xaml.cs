using System;
using System.Diagnostics;
using System.Windows;
using System.Threading;

namespace HobbitAutosplitter
{
    public partial class MainWindow
    {
        public static MainWindow instance;
        private Thread captureThread;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }

        private void Video_Crop_Workshop_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach(Window win in OwnedWindows)
            {
                if(win is ImageRegionWindow)
                {
                    win.WindowState = WindowState.Normal;
                    return;
                }
            }

            ImageRegionWindow window = new ImageRegionWindow();
            window.Closed += (s,f) => Activate();
            window.Owner = this;
            window.Show();
        }
        
        private void Comparison_Image_Setter_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window win in OwnedWindows)
            {
                if (win is SplitImagesWindow)
                {
                    win.WindowState = WindowState.Normal;
                    return;
                }
            }

            SplitImagesWindow window = new SplitImagesWindow();
            window.Closed += (s, f) => Activate();
            window.Owner = this;
            window.Show();
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window win in OwnedWindows)
            {
                if (win is SettingsWindow)
                {
                    win.WindowState = WindowState.Normal;
                    return;
                }
            }

            SettingsWindow window = new SettingsWindow();
            window.Closed += (s, f) => Activate();
            window.Owner = this;
            window.Show();
        }

        private void Website_Textblock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("https://hobbitspeedruns.com/");
        }

        private void Github_Textblock_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/Shockster218/Hobbit-Autosplitter");
        }

        private void StartCaptureSession(object sender, EventArgs e)
        {
            captureThread = new Thread(CaptureManager.InitializeCapture);
            captureThread.Start();
        }

        private void Main_Window_Closed(object sender, EventArgs e)
        {
            Settings.Default.Save();
        }

        //public void OBSOffline()
        //{
        //    DisableButtons();
        //    Task.Factory.StartNew(() => ProcessManager.FindOBS());
        //}
        //
        //public void ShowPreview(PreComparisonArgs args)
        //{
        //    obsPreview.Source = ((Bitmap)args.frame).ToBitmapImage();
        //}
        //
        //public void ToggleThiefSplit(LivesplitAction action)
        //{
        //    switch (action)
        //    {
        //        case LivesplitAction.SPLIT:
        //            thiefCheckbox.IsEnabled = false;
        //            break;
        //        case LivesplitAction.RESET:
        //            thiefCheckbox.IsEnabled = true;
        //            break;
        //    }
        //}
        //
        //public void ChangeComparisonReference(LivesplitAction action)
        //{
        //    splitReference.Source = SplitManager.GetCurrentComparison().GetImage().ToBitmapImage();
        //    SetLevelText();
        //}
        //
        //public void OBSOpened()
        //{
        //    splitReference.Source = SplitManager.GetCurrentComparison().GetImage().ToBitmapImage();
        //    SetLevelText();
        //    EnableButtons();
        //}
        //
        //
        //public void SetLevelText()
        //{
        //    levelLab.Content = SplitManager.GetCurrentComparison().GetSplitName();
        //}
        //
        //public void ShowNotEnoughSplitsMessageBox()
        //{
        //    MessageBoxManager.OK = "Retry";
        //    MessageBoxManager.Cancel = "Exit";
        //    MessageBoxManager.Register();
        //    MessageBoxResult result = MessageBox.Show("Incorrect number of split image files found or they are incorrectly named. Please add them to " +
        //        "Assets/Splits or adjust the file names then click \"Retry\"", "Not enough split images", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        //    MessageBoxManager.Unregister();
        //    if(result == MessageBoxResult.OK)
        //    {
        //        SplitManager.PopulateSplitData();
        //    }
        //    else
        //    {
        //        Application.Current.Shutdown();
        //    }
        //}
    }
}

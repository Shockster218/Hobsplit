using System;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.Windows;
using System.Threading;
using System.Windows.Media.Imaging;

namespace HobbitAutosplitter
{
    public partial class MainWindow
    {
        public static MainWindow instance;
        private bool isLiveFeed = false;
        private Thread captureThread;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
            CaptureManager.SendPreviewFrame += ShowPreview;
        }

        private void ShowPreview(BitmapImage image)
        {
            if (isLiveFeed)
            {
                Live_Feed_Image.Source = image;
            }
        }

        private void Live_Feed_Button_Click(object sender, RoutedEventArgs events)
        {
            DoubleAnimation anim = new DoubleAnimation();
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            anim.From = isLiveFeed ? 830 : 225;
            anim.To = isLiveFeed ? 225 : 830;

            Storyboard sb = new Storyboard();
            sb.Children.Add(anim);
            Storyboard.SetTargetName(anim, Name);
            Storyboard.SetTargetProperty(anim, new PropertyPath(WidthProperty));
            sb.Begin(this);

            Live_Feed_Button.Content = isLiveFeed ? "Show Live Feed" : "Hide Live Feed";
            isLiveFeed = !isLiveFeed;
        }

        private void Live_Feed_Crop_Button_Click(object sender, RoutedEventArgs e)
        {
            LivefeedCropWindow myOwnedWindow = new LivefeedCropWindow();
            myOwnedWindow.Owner = this;
            myOwnedWindow.Show();
        }

        private void Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow myOwnedWindow = new SettingsWindow();
            myOwnedWindow.Owner = this;
            myOwnedWindow.Show();
        }

        private void Comaprison_Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            ComparisonSettingsWindow myOwnedWindow = new ComparisonSettingsWindow();
            myOwnedWindow.Owner = this;
            myOwnedWindow.Show();
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
        //
        //private void changeComparison_Click(object sender, RoutedEventArgs e)
        //{
        //    ComparisonCropWindow myOwnedWindow = new ComparisonCropWindow();
        //    myOwnedWindow.Owner = this;
        //    myOwnedWindow.Show();
        //}
        //
        //private void thiefCheckbox_Click(object sender, RoutedEventArgs e)
        //{
        //    SplitManager.SetThiefSplit((bool)thiefCheckbox.IsChecked);
        //    Settings.Default.useThief = (bool)thiefCheckbox.IsChecked;
        //}
    }
}

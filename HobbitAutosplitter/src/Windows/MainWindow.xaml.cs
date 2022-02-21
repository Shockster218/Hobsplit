using System;
using System.Windows.Media.Animation;
using System.Windows;

namespace HobbitAutosplitter
{
    public partial class MainWindow
    {
        public static MainWindow instance;
        private bool isLiveFeed = false;
        public MainWindow()
        {
            InitializeComponent();
            instance = this;
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

            sb.Completed += (s, e) =>
            {
                // if (!isLiveFeed) start sending frames
                // else stop
            };

            sb.Begin(this);
            Live_Feed_Button.Content = isLiveFeed ? "Show Live Feed" : "Hide Live Feed";
            isLiveFeed = !isLiveFeed;
        }

        private void Comparison_Crop_Button_Click(object sender, RoutedEventArgs e)
        {
            ComparisonCropWindow myOwnedWindow = new ComparisonCropWindow();
            myOwnedWindow.Owner = this;
            myOwnedWindow.Show();
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
        //private void LoadSettings()
        //{
        //    bool useThief = Settings.Default.useThief;
        //    SplitManager.SetThiefSplit(useThief);
        //    thiefCheckbox.IsChecked = useThief;
        //
        //    splitButton.Content =   KeyInterop.KeyFromVirtualKey((int)Settings.Default.split).ToString();
        //    unsplitButton.Content = KeyInterop.KeyFromVirtualKey((int)Settings.Default.unsplit).ToString();
        //    resetButton.Content =   KeyInterop.KeyFromVirtualKey((int)Settings.Default.reset).ToString();
        //    pauseButton.Content =   KeyInterop.KeyFromVirtualKey((int)Settings.Default.pause).ToString();
        //
        //    x.Value = Settings.Default.cropLeft;
        //    y.Value = Settings.Default.cropTop;
        //    w.Value = Settings.Default.cropRight != 0 ? Settings.Default.cropRight : 1920;
        //    h.Value = Settings.Default.cropBottom != 0 ? Settings.Default.cropBottom : 1080;
        //}
        //
        //
        //private void x_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    int value;
        //    if(e.NewValue == null) value = 0;
        //    else value = ((int)e.NewValue).Clamp(0, 4000);
        //    CaptureManager.previewCrop.X = value;
        //    Settings.Default.cropLeft = value;
        //    x.Text = value.ToString();
        //}
        //private void y_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    int value;
        //    if (e.NewValue == null) value = 0;
        //    else value = ((int)e.NewValue).Clamp(0, 4000);
        //    CaptureManager.previewCrop.Y = value;
        //    Settings.Default.cropTop = value;
        //    y.Text = value.ToString();
        //}
        //private void w_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    int value;
        //    if (e.NewValue == null) value = 0;
        //    else value = ((int)e.NewValue).Clamp(0, 4000);
        //    CaptureManager.previewCrop.Right = value;
        //    Settings.Default.cropRight = value;
        //    w.Text = value.ToString();
        //}
        //private void h_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    int value;
        //    if (e.NewValue == null) value = 0;
        //    else value = ((int)e.NewValue).Clamp(0, 4000);
        //    CaptureManager.previewCrop.Bottom = value;
        //    Settings.Default.cropBottom = value;
        //    h.Text = value.ToString();
        //}
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

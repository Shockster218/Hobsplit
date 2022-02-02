using System;
using System.Windows.Media.Animation;
using System.Diagnostics;
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
            CaptureManager.Init();
            SplitManager.Init();
            LivesplitManager.Init();
            ProcessManager.Init();
        }


        private void Live_Feed_Button_Click(object sender, RoutedEventArgs events)
        {
            DoubleAnimation anim = new DoubleAnimation();
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            if (!isLiveFeed)
            {
                anim.From = 220;
                anim.To = 870;
            }
            else
            {
                anim.From = 870;
                anim.To = 220;
            }

            Storyboard sb = new Storyboard();
            sb.Children.Add(anim);
            Storyboard.SetTargetName(anim, Name);
            Storyboard.SetTargetProperty(anim, new PropertyPath(WidthProperty));

            sb.Completed += (s, e) =>
            {
                if (!isLiveFeed) Live_Feed_Image.IsEnabled = true;
            };

            sb.Begin(this);
            Live_Feed_Button.Content = isLiveFeed ? "Show Live Feed" : "Hide Live Feed";
            isLiveFeed = !isLiveFeed;
        }

        private void btnHobbitSpeedruns_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://hobbitspeedruns.com/");
        }

        private void btnGithub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/Shockster218/Hobbit-Autosplitter");
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
        //    splitButton.Content = KeyInterop.KeyFromVirtualKey((int)Settings.Default.split).ToString();
        //    unsplitButton.Content = KeyInterop.KeyFromVirtualKey((int)Settings.Default.unsplit).ToString();
        //    resetButton.Content = KeyInterop.KeyFromVirtualKey((int)Settings.Default.reset).ToString();
        //    pauseButton.Content = KeyInterop.KeyFromVirtualKey((int)Settings.Default.pause).ToString();
        //
        //    x.Value = Settings.Default.cropLeft;
        //    y.Value = Settings.Default.cropTop;
        //    w.Value = Settings.Default.cropRight != 0 ? Settings.Default.cropRight : 1920;
        //    h.Value = Settings.Default.cropBottom != 0 ? Settings.Default.cropBottom : 1080;
        //}
        //
        //private void EnableButtons()
        //{
        //    splitButton.IsEnabled = true;
        //    unsplitButton.IsEnabled = true;
        //    resetButton.IsEnabled = true;
        //    pauseButton.IsEnabled = true;
        //
        //    changeComparison.IsEnabled = true;
        //    thiefCheckbox.IsEnabled = true;
        //
        //    x.IsEnabled = true;
        //    y.IsEnabled = true;
        //    w.IsEnabled = true;
        //    h.IsEnabled = true;
        //}
        //
        //private void DisableButtons()
        //{
        //    splitButton.IsEnabled = false;
        //    unsplitButton.IsEnabled = false;
        //    resetButton.IsEnabled = false;
        //    pauseButton.IsEnabled = false;
        //
        //    changeComparison.IsEnabled = false;
        //    thiefCheckbox.IsEnabled = false;
        //
        //    x.IsEnabled = false;
        //    y.IsEnabled = false;
        //    w.IsEnabled = false;
        //    h.IsEnabled = false;
        //}
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
        //private void splitButton_Click(object sender, RoutedEventArgs e)
        //{
        //    splitButton.Content = "Waiting...";
        //}
        //
        //private void unsplitButton_Click(object sender, RoutedEventArgs e)
        //{
        //    unsplitButton.Content = "Waiting...";
        //}
        //
        //private void pauseButton_Click(object sender, RoutedEventArgs e)
        //{
        //    pauseButton.Content = "Waiting...";
        //}
        //
        //private void resetButton_Click(object sender, RoutedEventArgs e)
        //{
        //    resetButton.Content = "Waiting...";
        //}
        //
        //private void splitButton_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (splitButton.IsFocused)
        //    {
        //        Settings.Default.split = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
        //        splitButton.Content = e.Key.ToString();
        //        Keyboard.ClearFocus();
        //    }
        //}
        //
        //private void pauseButton_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (pauseButton.IsFocused)
        //    {
        //        Settings.Default.pause = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
        //        pauseButton.Content = e.Key.ToString();
        //        Keyboard.ClearFocus();
        //    }
        //}
        //
        //private void resetButton_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (resetButton.IsFocused)
        //    {
        //        Settings.Default.reset = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
        //        resetButton.Content = e.Key.ToString();
        //        Keyboard.ClearFocus();
        //    }
        //}
        //
        //private void unsplitButton_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (unsplitButton.IsFocused)
        //    {
        //        Settings.Default.unsplit = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
        //        unsplitButton.Content = e.Key.ToString();
        //        Keyboard.ClearFocus();
        //    }
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

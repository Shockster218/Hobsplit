using System;
using System.Diagnostics;
using System.Windows;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Hobsplit
{
    public partial class MainWindow
    {
        public static MainWindow instance;

        private Stopwatch segmentTimer;
        private Timer timer;
        private Thread captureThread;

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
            segmentTimer = new Stopwatch();
            timer = new Timer(100);
            timer.Elapsed += (s, e) => UpdateTimer();
            Topmost = Settings.Default.alwaysOnTop;

            SplitManager.AdvancedSplitInfo += SetAdvancedSplitInformation;
            ProcessManager.OBSClosedEvent += OBSClosed;
            LivesplitManager.OnLivesplitAction += HandleLiveSplitAction;
        }

        public void AdvancedInformationUIToggle()
        {
            bool sim = Settings.Default.advSimilarity;
            bool index = Settings.Default.advSplitIndex;
            bool state = Settings.Default.advSplitState;
            bool hideRunInfo = Settings.Default.advShowRunInfo;
            bool splitControl = Settings.Default.advSplitControl;

            if (hideRunInfo) Run_Info_GroupBox.Visibility = Visibility.Visible;
            else Run_Info_GroupBox.Visibility = Visibility.Collapsed;

            if (sim || index || state) Advanced_Settings_Groupbox.Visibility = Visibility.Visible;
            else Advanced_Settings_Groupbox.Visibility = Visibility.Collapsed;

            if(sim) Similarity_TextBlock.Visibility = Visibility.Visible;
            else Similarity_TextBlock.Visibility = Visibility.Collapsed;

            if (index) Split_Index_TextBlock.Visibility = Visibility.Visible;
            else Split_Index_TextBlock.Visibility = Visibility.Collapsed;

            if (state) Split_State_TextBlock.Visibility = Visibility.Visible;
            else Split_State_TextBlock.Visibility = Visibility.Collapsed;

            if (splitControl) Split_Control_Component.Visibility = Visibility.Visible;
            else Split_Control_Component.Visibility = Visibility.Collapsed;
        }

        public void SetWindowsOnTop()
        {
            Topmost = Settings.Default.alwaysOnTop;
            foreach (Window win in OwnedWindows) win.Topmost = Settings.Default.alwaysOnTop;
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
            window.Topmost = Settings.Default.alwaysOnTop;
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
            window.Topmost = Settings.Default.alwaysOnTop;
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
            window.Topmost = Settings.Default.alwaysOnTop;
            window.Show();
        }

        private void Advanced_Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window win in OwnedWindows)
            {
                if (win is AdvancedSettingsWindow)
                {
                    win.WindowState = WindowState.Normal;
                    return;
                }
            }

            AdvancedSettingsWindow window = new AdvancedSettingsWindow();
            window.Closed += (s, f) => Activate();
            window.Owner = this;
            window.Topmost = Settings.Default.alwaysOnTop;
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

        private void Main_Window_Rendered(object sender, EventArgs e)
        {
            AdvancedInformationUIToggle();
            captureThread = new Thread(CaptureManager.InitializeCapture);
            captureThread.Start();
        }

        private void HandleLiveSplitAction(LivesplitAction action)
        {
            Current_Level_TextBlock.Text = $"Current Level \n{SplitManager.GetCurrentComparison().GetSplitName()}";
            if (SplitManager.GetSplitIndex() == (int)SplitIndex.DONE) return;
            switch (action) 
            {
                case LivesplitAction.SPLIT:
                    if (SplitManager.GetSplitIndex() == (int)SplitIndex.THIEF) return;
                    if (segmentTimer.IsRunning) 
                    {
                        if(SplitManager.GetSplitIndex() == (int)SplitIndex.DONE) segmentTimer.Stop();
                        segmentTimer.Restart();
                        break;
                    }
                    else
                    {
                        timer.Start();
                        segmentTimer.Start();
                        break;
                    }
                case LivesplitAction.RESET:
                    segmentTimer.Reset();
                    Segment_Timer_TextBlock.Text = "Current Segment Time \n0.00";
                    timer.Stop();
                    break;
            }
        }

        private void UpdateTimer()
        {
            if (!segmentTimer.IsRunning) return;
            if (SplitManager.GetSplitIndex() > 2 && segmentTimer.ElapsedMilliseconds <= 5000) return;
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Segment_Timer_TextBlock.Text = $"Current Segment Time \n{new TimeSpan(segmentTimer.ElapsedTicks).ToString("hh\\:mm\\:ss\\.ff").TrimStart(':','0')}";
            }));
        }

        private void Main_Window_Closed(object sender, EventArgs e)
        {
            Settings.Default.Save();
        }

        private void SetAdvancedSplitInformation(AdvancedSplitInfoArgs args)
        {
            Split_Index_TextBlock.Text = $"Split Index \n{args.splitIndex} ({args.splitName})";
            Similarity_TextBlock.Text = $"Similarity Percentage \nCurrent Split {(args.currentSim * 100).ToString("00.0")}% \n Reset: {(args.resetSim * 100).ToString("00.0")}%";
            Split_State_TextBlock.Text = $"Split State \n{args.state}";
        }

        private void OBSClosed()
        {
            foreach (Window win in OwnedWindows) win.Close();
            StartupWindow window = new StartupWindow(true);
            window.Topmost = Settings.Default.alwaysOnTop;
            window.Show();
            Close();
        }
    }
}

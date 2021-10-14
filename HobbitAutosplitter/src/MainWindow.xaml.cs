using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;

namespace HobbitAutosplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static MainWindow instance;
        private DispatcherTimer dispatcher = new DispatcherTimer();
        private Stopwatch segmentStopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
            instance = this;
            CaptureManager.Init();
            ProcessManager.Init();
            SplitManager.Init();
            LivesplitManager.Init();
            CaptureManager.FrameCreated += ShowFrame;
        }

        public void OBSOffline()
        {
            obsPreview.Source = ((Bitmap)Image.FromFile(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Assets\\obs_offline.jpg")).ToBitmapImage();
        }

        public void ShowFrame(Object sender, FrameEventArgs frameArgs)
        {
            Extensions.InvokeToUIThread(() =>
            {
                obsPreview.Source = ((Bitmap)frameArgs.frame).ToBitmapImage();
                GC.Collect();
            });
        }

        public void EnableCropping()
        {
            x.IsEnabled = true;
            y.IsEnabled = true;
            w.IsEnabled = true;
            h.IsEnabled = true;
            x.Value = Settings.Default.cropLeft;
            y.Value = Settings.Default.cropTop;
            w.Value = Settings.Default.cropRight != 0 ? Settings.Default.cropRight : CaptureManager.crop.Right;
            h.Value = Settings.Default.cropBottom != 0 ? Settings.Default.cropBottom : CaptureManager.crop.Bottom;
        }

        public void DisableCropping()
        {
            x.IsEnabled = false;
            y.IsEnabled = false;
            w.IsEnabled = false;
            h.IsEnabled = false;
        }

        private void x_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            int value;
            if(e.NewValue == null) value = 0;
            else value = ((int)e.NewValue).Clamp(0, 1920);
            CaptureManager.crop.X = value;
            Settings.Default.cropLeft = value;
            x.Text = value.ToString();
        }
        private void y_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            int value;
            if (e.NewValue == null) value = 0;
            else value = ((int)e.NewValue).Clamp(0, 1080);
            CaptureManager.crop.Y = value;
            Settings.Default.cropTop = value;
            y.Text = value.ToString();
        }
        private void w_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            int value;
            if (e.NewValue == null) value = 0;
            else value = ((int)e.NewValue).Clamp(0, 1920);
            CaptureManager.crop.Right = value;
            Settings.Default.cropRight = value;
            w.Text = value.ToString();
        }
        private void h_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            int value;
            if (e.NewValue == null) value = 0;
            else value = ((int)e.NewValue).Clamp(0, 1080);
            CaptureManager.crop.Bottom = value;
            Settings.Default.cropBottom = value;
            h.Text = value.ToString();
        }

        private void splitButton_Click(object sender, RoutedEventArgs e)
        {
            splitButton.Content = "Waiting...";
        }

        private void unsplitButton_Click(object sender, RoutedEventArgs e)
        {
            unsplitButton.Content = "Waiting...";
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            pauseButton.Content = "Waiting...";
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            resetButton.Content = "Waiting...";
        }

        private void splitButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (splitButton.IsFocused)
            {
                Settings.Default.split = (WindowsInput.Native.VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                splitButton.Content = e.Key.ToString();
            }
        }

        private void pauseButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (pauseButton.IsFocused)
            {
                Settings.Default.pause = (WindowsInput.Native.VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                pauseButton.Content = e.Key.ToString();
            }
        }

        private void resetButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (resetButton.IsFocused)
            {
                Settings.Default.reset = (WindowsInput.Native.VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                resetButton.Content = e.Key.ToString();
            }
        }

        private void unsplitButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (unsplitButton.IsFocused)
            {
                Settings.Default.unsplit = (WindowsInput.Native.VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                unsplitButton.Content = e.Key.ToString();
            }
        }
    }
}

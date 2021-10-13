using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Threading;

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
            CaptureManager.FrameCreated += ShowFrame;
        }

        public void OBSOffline()
        {
            obsPreview.Source = ((Bitmap)Image.FromFile(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Assets\\obs_offline.jpg")).ToBitmapImage();
        }

        public void ShowFrame(Object sender, FrameEventArgs frameArgs)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                obsPreview.Source = ((Bitmap)frameArgs.frame).ToBitmapImage();
                GC.Collect();
            });
        }

        public void SetCropValues()
        {
            x.IsEnabled = true;
            y.IsEnabled = true;
            w.IsEnabled = true;
            h.IsEnabled = true;
            x.Value = 0;
            y.Value = 0;
            w.Value = CaptureManager.crop.Width;
            h.Value = CaptureManager.crop.Height;
        }

        private void x_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            int value;
            if(e.NewValue == null) value = 0;
            else value = ((int)e.NewValue).Clamp(0, 1920);
            CaptureManager.crop.X = value;
            x.Text = value.ToString();
        }
        private void y_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            int value;
            if (e.NewValue == null) value = 0;
            else value = ((int)e.NewValue).Clamp(0, 1080);
            CaptureManager.crop.Y = value;
            y.Text = value.ToString();
        }
        private void w_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            int value;
            if (e.NewValue == null) value = 0;
            else value = ((int)e.NewValue).Clamp(0, 1920);
            CaptureManager.crop.Right = value;
            w.Text = value.ToString();
        }
        private void h_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            int value;
            if (e.NewValue == null) value = 0;
            else value = ((int)e.NewValue).Clamp(0, 1080);
            CaptureManager.crop.Bottom = value;
            h.Text = value.ToString();
        }
    }
}

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
    }
}

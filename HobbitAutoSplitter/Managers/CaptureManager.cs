using System;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using Emgu.CV;

namespace HobbitAutoSplitter
{
    public class CaptureManager
    {
        public LivesplitManager livesplitManager = new LivesplitManager();
        private VideoCapture capture = null;
        private int cropValue = 0;
        private Image endFrame = null;

        public void SetEndFrame(Image frame)
        {
            endFrame = frame;
        }

        public Bitmap GetEndFrame(bool cropped)
        {
            if (endFrame != null)
            {
                if (cropped)
                {
                    return ImageProcessor.CropImageToBitmap(endFrame, Constants.trueHeight, Constants.trueWidth, cropValue);
                }
                else
                {
                    return new Bitmap((Bitmap)endFrame.Clone());
                }
            }
            else return null;
        }

        public void SetCropValue(int value)
        {
            cropValue = value;
        }

        public void SetupCapture()
        {
            if (capture != null) capture.Dispose();
            try
            {
                capture = new VideoCapture(MainWindow.instance.inputCombo.SelectedIndex);
                capture.ImageGrabbed += (s, e) => ProcessFrame();
                capture.Start();
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void ProcessFrame()
        {
            Mat m = new Mat();
            capture.Read(m);
            Bitmap croppedImg = ImageProcessor.CropImageToBitmap(m.ToBitmap(), Constants.trueWidth, Constants.trueHeight, cropValue);
            string output = Detector.DetectText(croppedImg);
            livesplitManager.HandleTextOutput(output);
            if(livesplitManager.onTCB()) Task.Run(() => livesplitManager.DetectFinalFrame(m.ToBitmap(), GetEndFrame(false)));
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MainWindow.instance.DisplayImage(croppedImg, output);
            }));
        }

        #region If ever needed to capture directly from window.
        //[DllImport("user32.dll")]
        //private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);
        //
        //[DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //private static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);
        //
        //[DllImport("user32.dll")]
        //private static extern bool SetForegroundWindow(IntPtr hWnd);
        //
        //[DllImport("user32.dll")]
        //private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        //#endregion
        //
        //public async void InitCapture()
        //{
        //    await Task.Factory.StartNew(() => FindOBS());
        //    await Task.Delay(1);
        //    _ = Task.Factory.StartNew(() => CaptureApplication());
        //}
        //private void FindOBS()
        //{
        //    bool foundOBS = false;
        //    while (!foundOBS)
        //    {
        //        try
        //        {
        //            Process obsProcess = Process.GetProcesses().Where(x => x.ProcessName.Contains("obs")).FirstOrDefault(x => x.ProcessName.Any(char.IsDigit));
        //            if (obsProcess != null)
        //            {
        //                process = obsProcess;
        //                foundOBS = true;
        //            }
        //        }
        //        catch { }
        //    }
        //}
        //
        //private void CaptureApplication()
        //{
        //    HandleRef hwnd = new HandleRef(this, process.MainWindowHandle);
        //    RECT rc;
        //    GetWindowRect(hwnd, out rc);
        //
        //    Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
        //    Graphics gfxBmp = Graphics.FromImage(bmp);
        //    IntPtr hdcBitmap = gfxBmp.GetHdc();
        //
        //    PrintWindow(hwnd.Handle, hdcBitmap, 0);
        //
        //    gfxBmp.ReleaseHdc(hdcBitmap);
        //
        //    Application.Current.Dispatcher.Invoke(() => { MainWindow.instance.DisplayImage(ConvertBitmapToBitmapImage(bmp)); });
        //    string output = TextDetector.DetectText(bmp.ToImage<Bgr, Byte>());
        //    livesplitManager.HandleTextOutput(output);
        //
        //    gfxBmp.Dispose();
        //    bmp.Dispose();
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //public struct RECT
        //{
        //    private int _Left;
        //    private int _Top;
        //    private int _Right;
        //    private int _Bottom;
        //
        //    public RECT(RECT Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
        //    {
        //    }
        //    public RECT(int Left, int Top, int Right, int Bottom)
        //    {
        //        _Left = Left;
        //        _Top = Top;
        //        _Right = Right;
        //        _Bottom = Bottom;
        //    }
        //
        //    public int X
        //    {
        //        get { return _Left; }
        //        set { _Left = value; }
        //    }
        //    public int Y
        //    {
        //        get { return _Top; }
        //        set { _Top = value; }
        //    }
        //    public int Left
        //    {
        //        get { return _Left; }
        //        set { _Left = value; }
        //    }
        //    public int Top
        //    {
        //        get { return _Top; }
        //        set { _Top = value; }
        //    }
        //    public int Right
        //    {
        //        get { return _Right; }
        //        set { _Right = value; }
        //    }
        //    public int Bottom
        //    {
        //        get { return _Bottom; }
        //        set { _Bottom = value; }
        //    }
        //    public int Height
        //    {
        //        get { return _Bottom - _Top; }
        //        set { _Bottom = value + _Top; }
        //    }
        //    public int Width
        //    {
        //        get { return _Right - _Left; }
        //        set { _Right = value + _Left; }
        //    }
        //    public System.Drawing.Point Location
        //    {
        //        get { return new System.Drawing.Point(Left, Top); }
        //        set
        //        {
        //            _Left = value.X;
        //            _Top = value.Y;
        //        }
        //    }
        //    public System.Drawing.Size Size
        //    {
        //        get { return new System.Drawing.Size(Width, Height); }
        //        set
        //        {
        //            _Right = value.Width + _Left;
        //            _Bottom = value.Height + _Top;
        //        }
        //    }
        //
        //    public static implicit operator Rectangle(RECT Rectangle)
        //    {
        //        return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
        //    }
        //    public static implicit operator RECT(Rectangle Rectangle)
        //    {
        //        return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
        //    }
        //    public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
        //    {
        //        return Rectangle1.Equals(Rectangle2);
        //    }
        //    public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
        //    {
        //        return !Rectangle1.Equals(Rectangle2);
        //    }
        //
        //    public override string ToString()
        //    {
        //        return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
        //    }
        //
        //    public override int GetHashCode()
        //    {
        //        return ToString().GetHashCode();
        //    }
        //
        //    public bool Equals(RECT Rectangle)
        //    {
        //        return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
        //    }
        //
        //    public override bool Equals(object Object)
        //    {
        //        if (Object is RECT)
        //        {
        //            return Equals((RECT)Object);
        //        }
        //        else if (Object is Rectangle)
        //        {
        //            return Equals(new RECT((Rectangle)Object));
        //        }
        //
        //        return false;
        //    }
        //}
        #endregion
    }
}

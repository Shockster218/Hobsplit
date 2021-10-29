﻿using System;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace HobbitAutosplitter
{
    public static class CaptureManager
    {
        public static event PreComparisonEventHandler FrameCreated;
        public static event DigestEventHandler DigestCompleted;

        public static RECT previewCrop;
        public static void Init()
        {
            ProcessManager.OBSOpenedEvent += CaptureApplication;
        }

        public static void CaptureApplication()
        {
            HandleRef hwnd = new HandleRef(null, ProcessManager.GetOBS().MainWindowHandle);
            RECT rc;
            GetWindowRect(hwnd, out rc);
            previewCrop = new RECT(
                Settings.Default.cropLeft,
                Settings.Default.cropTop,
                Settings.Default.cropRight != 0 ? Settings.Default.cropRight : rc.Right,
                Settings.Default.cropBottom != 0 ? Settings.Default.cropBottom : rc.Bottom
                );

            while (ProcessManager.obsRunning)
            {
                try
                {
                    Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
                    Graphics gfxBmp = Graphics.FromImage(bmp);
                    IntPtr hdcBitmap = gfxBmp.GetHdc();

                    PrintWindow(hwnd.Handle, hdcBitmap, 0);
                    gfxBmp.ReleaseHdc(hdcBitmap);

                    Bitmap previewCropped = bmp.Crop(previewCrop).Resize();
                    Bitmap finalCrop = previewCropped.Crop(SplitManager.GetCrop());
                    if (SplitManager.GetSplitIndex() == 1) finalCrop.RemoveColor();

                    Digest digest = ImagePhash.ComputeDigest(finalCrop.ToLuminanceImage());
                    finalCrop.Dispose();
                    
                    DigestCompleted?.Invoke(new DigestArgs(digest));
                    FrameCreated?.SmartInvoke(new PreComparisonArgs(previewCropped.Clone()));
                    
                    previewCropped.Dispose();
                    gfxBmp.Dispose();
                    bmp.Dispose();
                }
                catch {}
            }
        }

        #region Imports
        [DllImport("user32.dll")]
        private static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        private int _Left;
        private int _Top;
        private int _Right;
        private int _Bottom;

        public static RECT Default { get => Default; private set => new RECT(0, 0, 0, 0); }

        public RECT(RECT Rectangle) : this(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom)
        {
        }
        public RECT(int Left, int Top, int Right, int Bottom)
        {
            _Left = Left;
            _Top = Top;
            _Right = Right;
            _Bottom = Bottom;
        }

        public int X
        {
            get { return _Left; }
            set { _Left = value; }
        }
        public int Y
        {
            get { return _Top; }
            set { _Top = value; }
        }
        public int Left
        {
            get { return _Left; }
            set { _Left = value; }
        }
        public int Top
        {
            get { return _Top; }
            set { _Top = value; }
        }
        public int Right
        {
            get { return _Right; }
            set { _Right = value; }
        }
        public int Bottom
        {
            get { return _Bottom; }
            set { _Bottom = value; }
        }
        public int Height
        {
            get { return _Bottom - _Top; }
            set { _Bottom = value + _Top; }
        }
        public int Width
        {
            get { return _Right - _Left; }
            set { _Right = value + _Left; }
        }
        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set
            {
                _Left = value.X;
                _Top = value.Y;
            }
        }
        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set
            {
                _Right = value.Width + _Left;
                _Bottom = value.Height + _Top;
            }
        }

        public static implicit operator Rectangle(RECT Rectangle)
        {
            return new Rectangle(Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height);
        }
        public static implicit operator RECT(Rectangle Rectangle)
        {
            return new RECT(Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom);
        }
        public static bool operator ==(RECT Rectangle1, RECT Rectangle2)
        {
            return Rectangle1.Equals(Rectangle2);
        }
        public static bool operator !=(RECT Rectangle1, RECT Rectangle2)
        {
            return !Rectangle1.Equals(Rectangle2);
        }

        public override string ToString()
        {
            return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public bool Equals(RECT Rectangle)
        {
            return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
        }

        public override bool Equals(object Object)
        {
            if (Object is RECT)
            {
                return Equals((RECT)Object);
            }
            else if (Object is Rectangle)
            {
                return Equals(new RECT((Rectangle)Object));
            }

            return false;
        }
    }
}

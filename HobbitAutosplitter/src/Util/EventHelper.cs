using Shipwreck.Phash;
using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace HobbitAutosplitter
{
    public delegate void SmartEventHandler();
    public delegate void LivesplitActionEventHandler(LivesplitAction action = LivesplitAction.NONE);
    public delegate void CropEventHandler(CropArgs args);
    public delegate void PreviewFrameEventHandler(BitmapImage frame);

    public class CropArgs : EventArgs
    {
        public float left { get; set; }
        public float right { get; set; }
        public float top { get; set; }
        public float bottom { get; set; }

        public CropArgs(float left, float right, float top, float bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }
    }
}

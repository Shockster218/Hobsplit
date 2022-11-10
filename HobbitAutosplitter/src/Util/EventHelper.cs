using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using Shipwreck.Phash;

namespace HobbitAutosplitter
{
    public delegate void SmartEventHandler();
    public delegate void LivesplitActionEventHandler(LivesplitAction action = LivesplitAction.NONE);
    public delegate void FrameCreatedEventHandler(FrameCreatedArgs args);
    public delegate void DigestEventHandler(DigestArgs args);
    public delegate void CropEventHandler(CropArgs args);
    public delegate void SplitImagesEventHandler();

    public class FrameCreatedArgs : EventArgs
    {
        public byte[] frameData { get; set; }

        public FrameCreatedArgs(byte[] frameData) 
        {
            this.frameData = frameData;
        }

        public void Dispose()
        {
            frameData = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
    }

    public class DigestArgs : EventArgs
    {
        public Digest digest { get; set; }

        public static readonly DigestArgs Default = new DigestArgs(null);

        public DigestArgs(Digest digest)
        {
            this.digest = digest;
        }
    }

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

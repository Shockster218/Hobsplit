using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using Shipwreck.Phash;

namespace HobbitAutosplitter
{
    public delegate void SmartEventHandler();
    public delegate void PreComparisonEventHandler(PreComparisonArgs args);
    public delegate void PostComparisonEventHandler(PostComparisonArgs args);
    public class PreComparisonArgs : EventArgs
    {
        public Bitmap frameBM { get; set; }
        public BitmapImage frameBMI { get; set; }

        public PreComparisonArgs(Object frame) 
        {
            frameBM = (Bitmap)frame;
            frameBMI = frame == null ? null : frameBM.ToBitmapImage();
        }
    }

    public class PostComparisonArgs: EventArgs
    {
        public Digest digest { get; set; }

        public static readonly PostComparisonArgs Default = new PostComparisonArgs(null);

        public PostComparisonArgs(Digest digest)
        {
            this.digest = digest;
        }
    }
}

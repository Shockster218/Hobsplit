using System;
using Shipwreck.Phash;

namespace HobbitAutosplitter
{
    public delegate void SmartEventHandler();
    public delegate void LivesplitActionEventHandler(LivesplitAction action = LivesplitAction.NONE);
    public delegate void PreComparisonEventHandler(PreComparisonArgs args);
    public delegate void DigestEventHandler(DigestArgs args);
    public delegate void CropEventHandler(CropArgs args);
    public class PreComparisonArgs : EventArgs
    {
        public object frame { get; set; }

        public PreComparisonArgs(Object frame) 
        {
            this.frame = frame;
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

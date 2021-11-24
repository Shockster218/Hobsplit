using System;
using Shipwreck.Phash;

namespace HobbitAutosplitter
{
    public delegate void SmartEventHandler();
    public delegate void LivesplitActionEventHandler(LivesplitAction action = LivesplitAction.NONE);
    public delegate void PreComparisonEventHandler(PreComparisonArgs args);
    public delegate void DigestEventHandler(DigestArgs args);
    public class PreComparisonArgs : EventArgs
    {
        public object frame { get; set; }

        public PreComparisonArgs(Object frame) 
        {
            this.frame = frame;
        }
    }

    public class DigestArgs: EventArgs
    {
        public Digest digest { get; set; }

        public static readonly DigestArgs Default = new DigestArgs(null);

        public DigestArgs(Digest digest)
        {
            this.digest = digest;
        }
    }
}

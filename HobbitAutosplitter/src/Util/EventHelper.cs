using System;
using System.Drawing;
using System.Windows.Media.Imaging;
using Shipwreck.Phash;

namespace HobbitAutosplitter
{
    public delegate void SmartEventHandler(SmartInvokeArgs args);
    public delegate void DigestEventHandler(DigestInvokeArgs args);
    public class SmartInvokeArgs : EventArgs
    {
        public Bitmap frameBM { get; set; }
        public BitmapImage frameBMI { get; set; }
        public InvokeMode mode { get; set; }
        public static SmartInvokeArgs Default { get => new SmartInvokeArgs(null); set { } }

        public SmartInvokeArgs(Object frame, InvokeMode mode = InvokeMode.ASYNC) 
        {
            this.frameBM = (Bitmap)frame;
            this.frameBMI = frame == null ? null : frameBM.ToBitmapImage(); ;
            this.mode = mode;
        }
    }

    public class DigestInvokeArgs: EventArgs
    {
        public Digest digest { get; set; }

        public DigestInvokeArgs(Digest digest)
        {
            this.digest = digest;
        }
    }
}

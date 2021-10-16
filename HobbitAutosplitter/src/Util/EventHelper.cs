using System;

namespace HobbitAutosplitter
{
    public class SmartInvokeArgs : EventArgs
    {
        public InvokeMode mode { get; set; }

        public SmartInvokeArgs(InvokeMode mode = InvokeMode.SYNC)
        {
            this.mode = mode;
        }
    }
    public class FrameEventArgs : SmartInvokeArgs
    {
        public Object frame { get; set; }

        public FrameEventArgs(Object frame, InvokeMode mode = InvokeMode.UI) : base(mode)
        {
            this.mode = mode;
            this.frame = frame;
        }
    }
}

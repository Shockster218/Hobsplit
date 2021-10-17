using System;

namespace HobbitAutosplitter
{
    public delegate void SmartEventHandler(SmartInvokeArgs args);
    public class SmartInvokeArgs : EventArgs
    {
        public InvokeMode mode { get; set; }
        public Object frame { get; set; }

        public SmartInvokeArgs(InvokeMode mode, Object frame = null) 
        {
            this.mode = mode;
            this.frame = frame;
        }
    }
}

﻿using System;

namespace HobbitAutosplitter
{
    public delegate void SmartEventHandler(SmartInvokeArgs args);
    public class SmartInvokeArgs : EventArgs
    {
        public Object frame { get; set; }
        public static SmartInvokeArgs Default { get => Default; set { new SmartInvokeArgs(null); } }

        public SmartInvokeArgs(Object frame) 
        {
            this.frame = frame;
        }
    }
}

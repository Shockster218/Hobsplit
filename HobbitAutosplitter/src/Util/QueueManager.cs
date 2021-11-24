using System;
using System.Threading;
using System.Collections.Generic;


namespace HobbitAutosplitter
{
    public static class QueueManager
    {
        public static Queue<Action> comparisonQueue = new Queue<Action>();

        public static void Init()
        {
            Thread queueThread = new Thread(DoQueueWork);
            queueThread.IsBackground = true;
            queueThread.Start();
        }

        public static void Enqueue(DigestEventHandler handler, DigestArgs args)
        {
            try
            {
                comparisonQueue.Enqueue(() => handler.Invoke(args));
            }
            catch { }
        }

        private static void DoQueueWork()
        {
            while (true)
            {
                if (!comparisonQueue.IsEmpty())
                {
                    Action action = comparisonQueue.Dequeue();
                    action();
                }
            }
        }

        private static bool IsEmpty(this Queue<Action> q)
        {
            return q.Count == 0 ? true : false;
        }
    }
}
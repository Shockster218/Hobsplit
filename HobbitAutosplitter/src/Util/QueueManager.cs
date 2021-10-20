using System;
using System.Threading;
using System.Collections.Generic;


namespace HobbitAutosplitter
{
    public static class QueueManager
    {
        public static Queue<Action> comparisonsQueue = new Queue<Action>();

        public static void Init()
        {
            Thread queueThread = new Thread(DoQueueWork);
            queueThread.Start();
        }

        public static void Enqueue(this PostComparisonEventHandler handler, PostComparisonArgs args)
        {
            try
            {
                comparisonsQueue.Enqueue(() => handler.Invoke(args));
            }
            catch { }
        }

        private static void DoQueueWork()
        {
            while (true)
            {
                if (!comparisonsQueue.IsEmpty())
                {
                    Action action = comparisonsQueue.Dequeue();
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

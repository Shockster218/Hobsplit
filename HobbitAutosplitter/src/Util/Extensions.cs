﻿using System;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HobbitAutosplitter
{
    public static class Extensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static void SmartInvoke(this MulticastDelegate multicast)
        {
            MulticastDelegate multiDel = multicast;
            if (multiDel != null)
            {
                var invocationList = multiDel.GetInvocationList();

                foreach (SmartEventHandler handler in invocationList)
                {
                    DispatcherObject dispatcherTarget = handler.Target as DispatcherObject;
                    if(dispatcherTarget != null)
                    {
                        if (dispatcherTarget is Window)
                        {
                            if (!dispatcherTarget.Dispatcher.CheckAccess()) dispatcherTarget.Dispatcher.BeginInvoke(handler);
                        }
                    }
                    else
                    {
                        Task.Run(() => handler.Invoke());
                    }
                }
            }
        }

        public static void SmartInvoke(this MulticastDelegate multicast, FrameCreatedArgs args)
        {
            MulticastDelegate multiDel = multicast;
            if (multiDel != null)
            {
                var invocationList = multiDel.GetInvocationList();

                foreach (FrameCreatedEventHandler handler in invocationList)
                {
                    DispatcherObject dispatcherTarget = handler.Target as DispatcherObject;
                    if (dispatcherTarget != null)
                    {
                        if (dispatcherTarget is Window)
                        {
                            if (!dispatcherTarget.Dispatcher.CheckAccess()) dispatcherTarget.Dispatcher.BeginInvoke(handler, args);
                        }
                    }
                    else
                    {
                        Task.Run(() => handler.Invoke(args));
                    }
                }
            }
        }

        public static void SmartInvoke(this MulticastDelegate multicast, DigestArgs args)
        {
            MulticastDelegate multiDel = multicast;
            if (multiDel != null)
            {
                var invocationList = multiDel.GetInvocationList();

                foreach (DigestEventHandler handler in invocationList)
                {
                    DispatcherObject dispatcherTarget = handler.Target as DispatcherObject;
                    if (dispatcherTarget != null)
                    {
                        if (dispatcherTarget is Window)
                        {
                            if (!dispatcherTarget.Dispatcher.CheckAccess()) dispatcherTarget.Dispatcher.BeginInvoke(handler, args);
                        }
                    }
                    else
                    {
                        QueueManager.Enqueue(handler, args);                    
                    }
                }
            }
        }

        public static void SmartInvoke(this MulticastDelegate multicast, LivesplitAction action)
        {
            MulticastDelegate multiDel = multicast;
            if (multiDel != null)
            {
                var invocationList = multiDel.GetInvocationList();

                foreach (LivesplitActionEventHandler handler in invocationList)
                {
                    DispatcherObject dispatcherTarget = handler.Target as DispatcherObject;
                    if (dispatcherTarget != null)
                    {
                        if (dispatcherTarget is Window)
                        {
                            if (!dispatcherTarget.Dispatcher.CheckAccess()) dispatcherTarget.Dispatcher.BeginInvoke(handler, action);
                        }
                    }
                    else
                    {
                        Task.Run(() => handler.Invoke(action));
                    }
                }
            }
        }
    }
}


using System;
using System.Windows.Threading;
using System.Threading.Tasks;
using Point = System.Windows.Point;
using Window = System.Windows.Window;
using System.Drawing;

namespace HobbitAutosplitter
{
    public static class Extensions
    {
        public static bool IsZero(this Point p)
        {
            return p.X == 0 && p.Y == 0;
        }

        public static Rectangle Multiply(this Rectangle r)
        {
            return new Rectangle(r.X * 2, r.Y * 2, r.Width * 2, r.Height * 2);
        }

        public static Rectangle Divide(this Rectangle r)
        {
            return new Rectangle(r.X / 2, r.Y / 2, r.Width / 2, r.Height / 2);
        }

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
                    if (dispatcherTarget != null)
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
        public static void SmartInvoke(this MulticastDelegate multicast, AdvancedSplitInfoArgs args)
        {
            MulticastDelegate multiDel = multicast;
            if (multiDel != null)
            {
                var invocationList = multiDel.GetInvocationList();

                foreach (AdvancedSplitInformationEventHandler handler in invocationList)
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

        public static void SmartInvoke(this MulticastDelegate multicast, byte[] frame)
        {
            MulticastDelegate multiDel = multicast;
            if (multiDel != null)
            {
                var invocationList = multiDel.GetInvocationList();

                foreach (PreviewFrameEventHandler handler in invocationList)
                {
                    DispatcherObject dispatcherTarget = handler.Target as DispatcherObject;
                    if (dispatcherTarget != null)
                    {
                        if (dispatcherTarget is Window)
                        {
                            if (!dispatcherTarget.Dispatcher.CheckAccess()) dispatcherTarget.Dispatcher.BeginInvoke(handler, frame);
                        }
                    }
                }
            }
        }
    }
}


using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Text.RegularExpressions;

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

        public static void SmartInvoke(this MulticastDelegate multicast, object sender, SmartInvokeArgs args)
        {
            switch (args.mode)
            {
                case InvokeMode.SYNC:
                    SyncInvoke(multicast, sender, args);
                    break;
                case InvokeMode.ASYNC:
                    AsyncInvoke(multicast, sender, args);
                    break;
                case InvokeMode.UI:
                    UIInvoke(multicast, sender, args);
                    break;
            }
        }

        public static void SyncInvoke(MulticastDelegate multicast, object sender, EventArgs args)
        {
            MulticastDelegate handler = multicast;
            if (handler != null)
            {
                Delegate[] invocationList = handler.GetInvocationList();

                foreach (EventHandler h in invocationList)
                {
                    h.Invoke(sender, args);
                }
            }
        }

        public static void AsyncInvoke(MulticastDelegate multicast, object sender, EventArgs args)
        {
            MulticastDelegate handler = multicast;
            if (handler != null)
            {
                Delegate[] invocationList = handler.GetInvocationList();

                foreach (EventHandler h in invocationList)
                {
                    Task.Factory.StartNew(() =>
                    {
                        h.Invoke(sender, args);
                    });
                }
            }
        }

        public static void UIInvoke(MulticastDelegate multicast, object sender, EventArgs args)
        {
            MulticastDelegate handler = multicast;
            if (handler != null)
            {
                Delegate[] invocationList = handler.GetInvocationList();
                foreach (Delegate del in invocationList)
                {
                    DispatcherObject dispatcherTarget = del.Target as DispatcherObject;
                    if (dispatcherTarget != null && !dispatcherTarget.Dispatcher.CheckAccess())
                    {
                        dispatcherTarget.Dispatcher.BeginInvoke(del, sender, args);
                    }
                }
            }
        }

        public static IEnumerable<string> CustomSort(this IEnumerable<string> list)
        {
            int maxLen = list.Select(s => s.Length).Max();

            return list.Select(s => new
            {
                OrgStr = s,
                SortStr = Regex.Replace(s, @"(\d+)|(\D+)", m => m.Value.PadLeft(maxLen, char.IsDigit(m.Value[0]) ? ' ' : '\xffff'))
            })
            .OrderBy(x => x.SortStr)
            .Select(x => x.OrgStr);
        }
    }
}


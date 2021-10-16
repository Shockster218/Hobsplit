using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Text.RegularExpressions;


namespace HobbitAutosplitter
{
    public static class Constants
    {
        public const int width = 640;
        public const int height = 480;
        public const string loadingKeyword = "loading";
    }

    public enum States
    {
        READYTOSTART,
        STARTED,
        GAMEPLAY,
        LOADING,
        FINISHED
    }

    public static class Extensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static void AsyncInvoke(this EventHandler evnt, object sender, EventArgs args)
        {
            EventHandler handler = evnt;
            if (handler != null)
            {
                var invocationList = handler.GetInvocationList();

                foreach(EventHandler h in invocationList)
                {
                    Task.Factory.StartNew(() =>
                    {
                        h.Invoke(sender, args);
                    });
                }
            }
        }

        public static void InvokeToUIThread(this MulticastDelegate multicast, object sender, EventArgs args)
        {
            foreach (Delegate del in multicast.GetInvocationList())
            {
                DispatcherObject dispatcherTarget = del.Target as DispatcherObject;
                if (dispatcherTarget != null && !dispatcherTarget.Dispatcher.CheckAccess())
                {
                    dispatcherTarget.Dispatcher.BeginInvoke(del, sender, args);
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
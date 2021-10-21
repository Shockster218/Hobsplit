﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Threading.Tasks;
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
                        if (dispatcherTarget.GetType() == typeof(MainWindow))
                        {
                            if (!dispatcherTarget.Dispatcher.CheckAccess()) dispatcherTarget.Dispatcher.BeginInvoke(handler);
                        }
                    }
                    else
                    {
                        handler.Invoke();
                    }
                }
            }
        }

        public static void SmartInvoke(this MulticastDelegate multicast, PreComparisonArgs args)
        {
            MulticastDelegate multiDel = multicast;
            if (multiDel != null)
            {
                var invocationList = multiDel.GetInvocationList();

                foreach (PreComparisonEventHandler handler in invocationList)
                {
                    DispatcherObject dispatcherTarget = handler.Target as DispatcherObject;
                    if (dispatcherTarget != null)
                    {
                        if (dispatcherTarget.GetType() == typeof(MainWindow))
                        {
                            if (!dispatcherTarget.Dispatcher.CheckAccess()) dispatcherTarget.Dispatcher.BeginInvoke(handler, args);
                        }
                    }
                    else
                    {
                        handler.Invoke(args);
                    }
                }
            }
        }

        public static void SmartInvoke(this MulticastDelegate multicast, PostComparisonArgs args)
        {
            MulticastDelegate multiDel = multicast;
            if (multiDel != null)
            {
                var invocationList = multiDel.GetInvocationList();

                foreach (PostComparisonEventHandler handler in invocationList)
                {
                    DispatcherObject dispatcherTarget = handler.Target as DispatcherObject;
                    if (dispatcherTarget != null)
                    {
                        if (dispatcherTarget.GetType() == typeof(MainWindow))
                        {
                            if (!dispatcherTarget.Dispatcher.CheckAccess()) dispatcherTarget.Dispatcher.BeginInvoke(handler, args);
                        }
                    }
                    else
                    {
                        handler.Invoke(args);
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


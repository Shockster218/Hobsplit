using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

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

        public static void InvokeToUIThread(Action action)
        {
            App.Current.Dispatcher.Invoke(action);
        }
    }
}
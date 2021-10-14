using System;
using System.Windows;
using WindowsInput;
using System.Windows.Input;
using Shipwreck.Phash;
using System.Drawing;

namespace HobbitAutosplitter
{
    public static class LivesplitManager
    {

        private static InputSimulator sim;

        public static void Init()
        {
            sim = new InputSimulator();
        }

        public static void HandleTextOutput(string output)
        {
           
        }

        public static void DetectFinalFrame(Bitmap current, Bitmap reference)
        {

        }
        
        public static void DetectLevel(Bitmap current, Bitmap reference)
        {

        }

        public static void DetectLoading(Bitmap current, Bitmap reference)
        {

        }
    }
}

using System;
using System.Linq;
using System.IO;
using System.Drawing;

namespace HobbitAutosplitter
{
    public static class SplitManager
    {
        public static SmartEventHandler OnSplit;
        public static SmartEventHandler OnUnsplit;
        public static SmartEventHandler OnReset;
        public static SmartEventHandler OnPause;

        private static SplitData currentComparison;
        private static SplitData previousSplitData;

        private static string[] splitImagePaths;

        private static bool started = false;

        private static float universalSimilarity;
        private static int splitIndex;

        public static void Init()
        {
            CaptureManager.FrameCreated += CompareFrames;
            PopulateSplitData();

            OnSplit += IncrementSplitIndex;
            OnUnsplit += DeincrementSplitIndex;
            OnReset += ResetSplitIndex;
        }

        public static void IncrementSplitIndex(SmartInvokeArgs args) { splitIndex++; SetSplitData(); }

        public static void DeincrementSplitIndex(SmartInvokeArgs args) { splitIndex--; SetSplitData(); }

        public static void ResetSplitIndex(SmartInvokeArgs args) { splitIndex = 0; SetSplitData(); }

        public static float GetUniversalSimilarity() { return universalSimilarity; }

        public static void SetUniversalSimilarity(float similarity) { universalSimilarity = similarity > 1 ? 1 : similarity; }

        private static void SetSplitData() 
        { 
            currentComparison = new SplitData(Constants.splitNames[splitIndex], splitImagePaths[splitIndex]);
            previousSplitData = splitIndex >= 1 ? new SplitData(Constants.splitNames[splitIndex - 1], splitImagePaths[splitIndex - 1]) : new SplitData("Main Menu", splitImagePaths[0]);
        }

        private static void PopulateSplitData()
        {
            splitImagePaths = Directory.EnumerateFiles(Environment.CurrentDirectory + "\\split_images").CustomSort().ToArray();
            if (splitImagePaths.Length != 15)
            {
                // Say not enough images found
                return;
            }

            SetSplitData();
        }

        public static void CompareFrames(SmartInvokeArgs args)
        {
            Bitmap frame;
            if (null == args.frame) return;
            else frame = (Bitmap)args.frame;
            if (!started)
            {
                if (currentComparison.IsFrameSimilar(frame))
                {
                    OnSplit?.SmartInvoke(SmartInvokeArgs.Default);
                }
            }
            else
            {
                if (currentComparison.IsFrameSimilar(frame))
                {
                    OnReset?.SmartInvoke(SmartInvokeArgs.Default);
                    started = false;
                }
            }
        }
    }
}

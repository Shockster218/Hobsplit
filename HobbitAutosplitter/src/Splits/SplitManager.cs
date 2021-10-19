using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace HobbitAutosplitter
{
    public static class SplitManager
    {
        public static SmartEventHandler OnSplit;
        public static SmartEventHandler OnUnsplit;
        public static SmartEventHandler OnReset;
        public static SmartEventHandler OnPause;

        public static DigestEventHandler DigestCompleted;

        public static float swag;

        private static SplitData nextSplit;
        private static SplitData currentComparison;
        private static SplitData previousSplitData;

        private static string[] splitImagePaths;

        private static SplitState splitState = SplitState.IDLE;

        private static float universalSimilarity;
        private static int splitIndex;

        public static void Init()
        {
            CaptureManager.FrameCreated += DigestIncomingFrame;
            DigestCompleted += CompareFrames;
            PopulateSplitData();

            OnSplit += IncrementSplitIndex;
            OnUnsplit += DeincrementSplitIndex;
            OnReset += ResetSplitIndex;
        }

        public static void IncrementSplitIndex(SmartInvokeArgs args) { splitIndex++; SetSplitData(); }

        public static void DeincrementSplitIndex(SmartInvokeArgs args) { splitIndex--; SetSplitData(); }

        public static void ResetSplitIndex(SmartInvokeArgs args) { splitIndex = 0; SetSplitData(); }

        public static float GetUniversalSimilarity() { return universalSimilarity; }

        public static SplitData GetCurrentComparison() { return currentComparison; }

        public static void SetUniversalSimilarity(float similarity) { universalSimilarity = similarity > 1 ? 1 : similarity; }

        private static void SetSplitData() 
        {
            nextSplit = splitIndex <= 14 ? new SplitData(Constants.splitNames[splitIndex + 1], splitImagePaths[splitIndex + 1]) : null;
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

        private static void DigestIncomingFrame(SmartInvokeArgs args)
        {
            Bitmap frame = args.frameBM;
            Digest digest = ImagePhash.ComputeDigest(frame.ToLuminanceImage());
            frame.Dispose();
            DigestCompleted?.SmartInvoke(new DigestInvokeArgs(digest));
        }

        public static void CompareFrames(DigestInvokeArgs args)
        {
            bool c = ImagePhash.GetCrossCorrelation(currentComparison.GetDigest(), args.digest) >= universalSimilarity;

            if (splitState == SplitState.IDLE)
            {
                if (c)
                {
                    splitState = SplitState.WAITING;
                    OnSplit?.SmartInvoke(SmartInvokeArgs.Default);
                }
            }
            else
            {
                if (c)
                {
                    splitState = SplitState.IDLE;
                    OnReset?.SmartInvoke(SmartInvokeArgs.Default);
                }
            }
        }
    }
}

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

        private static SplitData nextComparison;
        private static SplitData currentComparison;
        private static SplitData previousSplitData;
        private static SplitData resetComparison;

        private static string[] splitImagePaths;

        private static SplitState splitState = SplitState.STARTUP;

        private static float universalSimilarity;
        private static int splitIndex;

        public static void Init()
        {
            CaptureManager.FrameCreated += DigestIncomingFrame;
            DigestCompleted += CompareFrames;
            PopulateSplitData();
        }

        public static void IncrementSplitIndex() { splitIndex++; SetSplitData(); }

        public static void DeincrementSplitIndex() { splitIndex--; SetSplitData(); }

        public static void ResetSplitIndex() { splitIndex = 0; SetSplitData(); }

        public static float GetUniversalSimilarity() { return universalSimilarity; }

        public static SplitData GetCurrentComparison() { return currentComparison; }

        public static void SetUniversalSimilarity(float similarity) { universalSimilarity = similarity > 1 ? 1 : similarity; }

        private static void SetSplitData() 
        {
            nextComparison = splitIndex <= 14 ? new SplitData(Constants.splitNames[splitIndex + 1], splitImagePaths[splitIndex + 1], splitIndex + 1) : null;
            currentComparison = new SplitData(Constants.splitNames[splitIndex], splitImagePaths[splitIndex], splitIndex);
            previousSplitData = splitIndex >= 1 ? new SplitData(Constants.splitNames[splitIndex - 1], splitImagePaths[splitIndex - 1], splitIndex - 1) : new SplitData("Main Menu", splitImagePaths[0], 0);
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
            resetComparison = currentComparison;
        }

        private static void DigestIncomingFrame(SmartInvokeArgs args)
        {
            Bitmap frame = args.frameBM;
            Digest digest = ImagePhash.ComputeDigest(frame.Crop(Constants.crop).ToLuminanceImage());
            frame.Dispose();
            DigestCompleted?.SmartInvoke(new DigestInvokeArgs(digest));
        }

        public static void CompareFrames(DigestInvokeArgs args)
        {
            Digest d = args.digest;
            bool c = ImagePhash.GetCrossCorrelation(currentComparison.GetDigest(), d) >= universalSimilarity;
            bool n = ImagePhash.GetCrossCorrelation(nextComparison.GetDigest(), d) >= universalSimilarity;
            bool r = ImagePhash.GetCrossCorrelation(resetComparison.GetDigest(), d) >= universalSimilarity;

            if (r && splitState > SplitState.IDLE)
            {
                splitState = SplitState.IDLE;
                ResetSplitIndex();
                OnReset?.SmartInvoke(SmartInvokeArgs.Default);
            }

            if (c)
            {
            }
            else
            {
                if (splitState == SplitState.IDLE)
                {
                    splitState = SplitState.WAITING;
                    IncrementSplitIndex();
                    OnSplit?.SmartInvoke(SmartInvokeArgs.Default);
                }
            }
        }
    }
}

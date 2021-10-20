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
        public static PostComparisonEventHandler OnSplit;
        public static PostComparisonEventHandler OnUnsplit;
        public static PostComparisonEventHandler OnReset;
        public static PostComparisonEventHandler OnPause;

        public static PostComparisonEventHandler DigestCompleted;

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

        public static SplitState GetCurrentSplitState() { return splitState; }

        public static void SetUniversalSimilarity(float similarity) { universalSimilarity = similarity > 1 ? 1 : similarity; }

        private static void SetSplitData() 
        {
            nextComparison = splitIndex <= 14 ? new SplitData(Constants.splitNames[splitIndex + 1], splitImagePaths[splitIndex + 1], splitIndex + 1) : null;
            currentComparison = new SplitData(Constants.splitNames[splitIndex], splitImagePaths[splitIndex], splitIndex);
            previousSplitData = splitIndex >= 1 ? new SplitData(Constants.splitNames[splitIndex - 1], splitImagePaths[splitIndex - 1], splitIndex - 1) : new SplitData("Main Menu", splitImagePaths[0], 0);
        }

        private static void PopulateSplitData()
        {
            splitImagePaths = Directory.EnumerateFiles(Environment.CurrentDirectory + "\\Assets\\Image\\Splits").CustomSort().ToArray();
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
            DigestCompleted?.SmartInvoke(new PostComparisonArgs(digest));
        }

        public static void CompareFrames(PostComparisonArgs args)
        {
            Digest d = args.digest;
            bool c = ImagePhash.GetCrossCorrelation(currentComparison.GetDigest(), d) >= universalSimilarity;
            bool n = ImagePhash.GetCrossCorrelation(nextComparison.GetDigest(), d) >= universalSimilarity;
            bool r = ImagePhash.GetCrossCorrelation(resetComparison.GetDigest(), d) >= universalSimilarity;

            if (r)
            {
                if(splitState > SplitState.IDLE)
                {
                    splitState = SplitState.IDLE;
                    ResetSplitIndex();
                    OnReset?.SmartInvoke(PostComparisonArgs.Default);
                }
            }
            else
            {
                if (splitState == SplitState.IDLE)
                {
                    splitState = SplitState.LOADING;
                    IncrementSplitIndex();
                    OnSplit?.SmartInvoke(PostComparisonArgs.Default);
                }
            }
        }
    }
}

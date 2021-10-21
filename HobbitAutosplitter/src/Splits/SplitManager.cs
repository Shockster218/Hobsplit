using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace HobbitAutosplitter
{
    public static class SplitManager
    {
        public static PostComparisonEventHandler DigestCompleted;

        private static SplitData nextComparison;
        private static SplitData currentComparison;
        private static SplitData previousComparison;
        private static SplitData resetComparison;

        private static string[] splitImagePaths;

        private static SplitState splitState = SplitState.STARTUP;

        private static float universalSimilarity;
        private static int splitIndex;

        public static void Init()
        {
            PopulateSplitData();
            CaptureManager.FrameCreated += DigestIncomingFrame;
            DigestCompleted += CompareFrames;
        }

        public static void IncrementSplitIndex(int ammount = 1) { splitIndex += ammount; SetSplitData(); }

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
            previousComparison = splitIndex >= 1 ? new SplitData(Constants.splitNames[splitIndex - 1], splitImagePaths[splitIndex - 1], splitIndex - 1) : new SplitData("Main Menu", splitImagePaths[0], 0);
        }

        private static void PopulateSplitData()
        {
            List<string> sorted = Directory.EnumerateFiles(Environment.CurrentDirectory + "\\Assets\\Image\\Splits").CustomSort().ToList();
            sorted.Insert(11, sorted[9]);
            splitImagePaths = sorted.ToArray();
            if (splitImagePaths.Length != 16)
            {
                // Say not enough images found
                return;
            }

            SetSplitData();
            resetComparison = currentComparison;
        }

        private static void DigestIncomingFrame(PreComparisonArgs args)
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
            bool n = null != nextComparison ? ImagePhash.GetCrossCorrelation(nextComparison.GetDigest(), d) >= universalSimilarity : false;
            bool r = ImagePhash.GetCrossCorrelation(resetComparison.GetDigest(), d) >= universalSimilarity;

            if (r)
            {
                if(splitState > SplitState.IDLE)
                {
                    ResetSplitIndex();
                    splitState = SplitState.IDLE;
                    LivesplitManager.Reset();
                }
            }
            else
            {
                if (splitState == SplitState.IDLE)
                {
                    IncrementSplitIndex();
                    splitState = SplitState.GAMEPLAY;
                    LivesplitManager.Split();          
                }
            }

            if (c)
            {
                if (splitState == SplitState.GAMEPLAY)
                {
                    splitState = SplitState.LOADING;
                    LivesplitManager.Pause();
                }
            }
            else
            {
                if(splitState == SplitState.LOADING)
                {
                    splitState = SplitState.GAMEPLAY;
                    LivesplitManager.Pause();
                }
            }

            if (n)
            {
                if(splitIndex >= 1 && splitState == SplitState.GAMEPLAY)
                {
                    if(splitIndex == 9)
                    {
                        IncrementSplitIndex(2);
                        LivesplitManager.Split();
                    }
                    else
                    {
                        IncrementSplitIndex();
                        splitState = SplitState.LOADING;
                        LivesplitManager.Split();
                    }
                }
            }

            // Should only fire if it sees thief split again. Gonna add a double check for the split index but shouldnt be needed
            if (splitIndex == 11)
            {
                bool p = ImagePhash.GetCrossCorrelation(previousComparison.GetDigest(), d) >= universalSimilarity;
                if (p)
                {
                    LivesplitManager.Unsplit();
                    LivesplitManager.Split();
                }
            }
        }
    }
}

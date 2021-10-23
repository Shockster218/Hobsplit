using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace HobbitAutosplitter
{
    public static class SplitManager
    {
        private static SplitData currentComparison;
        private static SplitData nextComparison;
        private static SplitData previousComparison;
        private static SplitData resetComparison;

        private static string[] splitImagePaths;

        private static SplitState splitState = SplitState.STARTUP;

        private static float universalSimilarity;
        private static int splitIndex = -1;


        public static void Init()
        {
            CaptureManager.DigestCompleted += CompareFrames;
            PopulateSplitData();
        }

        public static void IncrementSplitIndex(int ammount = 1) { splitIndex += ammount; SetSplitData(); }
        public static void DeincrementSplitIndex() { splitIndex--; SetSplitData(); }
        public static void ResetSplitIndex() { splitIndex = 0; SetSplitData(); }
        public static float GetUniversalSimilarity() { return universalSimilarity; }
        public static SplitData GetCurrentComparison() { return currentComparison; }
        public static SplitData GetNextComparison() { return nextComparison; }
        public static SplitState GetCurrentSplitState() { return splitState; }
        public static RECT GetCrop() { return splitIndex <= 0 ? Constants.startCrop : Constants.crop; }
        public static int GetSplitIndex() { return splitIndex; }

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

            resetComparison = new SplitData(Constants.splitNames[0], splitImagePaths[0]);
            currentComparison = new SplitData("Start Up", splitImagePaths[0], start:true);
        }

        public static void CompareFrames(DigestArgs args)
        {
            Digest d = args.digest;
            bool c = ImagePhash.GetCrossCorrelation(currentComparison.GetDigest(), d) >= GetUniversalSimilarity();
            bool n = null != nextComparison ? ImagePhash.GetCrossCorrelation(nextComparison.GetDigest(), d) >= GetUniversalSimilarity() : false;
            bool r = ImagePhash.GetCrossCorrelation(resetComparison.GetDigest(), d) >= 0.97f;

            if (r)
            {
                ResetSplitIndex();
                splitState = SplitState.WAITING;
                LivesplitManager.Reset();
            }

            if (splitState == SplitState.WAITING)
            {
                float sim = ImagePhash.GetCrossCorrelation(currentComparison.GetDigest(), d);
                if(sim <= 0.15f)
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
                else if(splitState == SplitState.STARTUP)
                {
                    splitState = SplitState.WAITING;
                    ResetSplitIndex();
                    LivesplitManager.Reset();
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
                if(splitIndex >= 2 && splitState == SplitState.GAMEPLAY)
                {
                    if(splitIndex == 10)
                    {
                        IncrementSplitIndex(2);
                        LivesplitManager.Split();
                    }
                    else
                    {
                        IncrementSplitIndex();
                        splitState = SplitState.LOADING;
                        LivesplitManager.Split();
                        LivesplitManager.Pause();
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

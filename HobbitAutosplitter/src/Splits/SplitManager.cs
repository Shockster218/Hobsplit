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

        private static SplitData[] splits;

        private static SplitState splitState = SplitState.STARTUP;

        private static float universalSimilarity;
        private static int splitIndex = 0;


        public static void Init()
        {
            CaptureManager.DigestCompleted += CompareFrames;
            PopulateSplitData();
        }

        public static void IncrementSplitIndex(int ammount = 1) { splitIndex += ammount; SetSplitData(); }
        public static void DeincrementSplitIndex() { splitIndex--; SetSplitData(); }
        public static void ResetSplitIndex() { splitIndex = 1; SetSplitData(); }
        public static float GetUniversalSimilarity() { return universalSimilarity; }
        public static SplitData GetCurrentComparison() { return currentComparison; }
        public static SplitData GetNextComparison() { return nextComparison; }
        public static SplitState GetCurrentSplitState() { return splitState; }
        public static RECT GetCrop() { return splitIndex == 1 ? Constants.startCrop : Constants.crop; }
        public static int GetSplitIndex() { return splitIndex; }

        public static void SetUniversalSimilarity(float similarity) { universalSimilarity = similarity > 1 ? 1 : similarity; }

        private static void SetSplitData() 
        {
            nextComparison = splitIndex <= splits.Length - 1 ? splits[splitIndex + 1]: null;
            currentComparison = splits[splitIndex];
            previousComparison = splitIndex >= 1 ? splits[splitIndex - 1] : splits[0];
        }

        private static void PopulateSplitData()
        {
            string[] sorted = Directory.EnumerateFiles(Environment.CurrentDirectory + "\\Assets\\Image\\Splits").CustomSort().ToArray();
            if (sorted.Length != 15)
            {
                // Say not enough images found
                return;
            }

            splits = new SplitData[17]
            {
                new SplitData("Start Up / Reset", sorted[0]),
                new SplitData("Main Menu / Start", sorted[0], startCrop:true, removeColor:true),
                new SplitData("Dream World", sorted[1]),
                new SplitData("An Unexpected Party", sorted[2]),
                new SplitData("Roast Mutton", sorted[3]),
                new SplitData("Troll-Hole", sorted[4]),
                new SplitData("Over Hill and Under Hill", sorted[5]),
                new SplitData("Riddles in the Dark", sorted[6]),
                new SplitData("Flies and Spiders", sorted[7]),
                new SplitData("Barrels out of Bond", sorted[8]),
                new SplitData("AWW - Pre Thief", sorted[9]),
                new SplitData("Thief", sorted[10]),
                new SplitData("AWW - Post Thief", sorted[9]),
                new SplitData("Inside Information", sorted[11]),
                new SplitData("Gathering of the Clouds", sorted[12]),
                new SplitData("Clouds Burst", sorted[13]),
                new SplitData("Finished", sorted[14])
            };

            currentComparison = splits[1];
            resetComparison = splits[0];
        }

        public static void CompareFrames(DigestArgs args)
        {
            Digest d = args.digest;
            bool c = ImagePhash.GetCrossCorrelation(currentComparison.GetDigest(), d) >= GetUniversalSimilarity();
            bool n = null != nextComparison ? ImagePhash.GetCrossCorrelation(nextComparison.GetDigest(), d) >= GetUniversalSimilarity() : false;
            bool r = ImagePhash.GetCrossCorrelation(resetComparison.GetDigest(), d) >= GetUniversalSimilarity();

            if (r)
            {
                ResetSplitIndex();
                splitState = SplitState.WAITING;
                LivesplitManager.Reset();
            }

            if (splitState == SplitState.WAITING)
            {
                float sim = ImagePhash.GetCrossCorrelation(currentComparison.GetDigest(), d);
                if(sim <= 0.2f)
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

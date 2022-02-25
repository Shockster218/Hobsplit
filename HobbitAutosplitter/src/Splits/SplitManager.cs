using System;
using Shipwreck.Phash;

namespace HobbitAutosplitter
{
    public static class SplitManager
    {
        private static SplitData currentComparison;
        private static SplitData nextComparison;
        private static SplitData previousComparison;
        private static SplitData resetComparison;

        private static SplitData[] splits;

        private static SplitState splitState = SplitState.GAMEPLAY;
        private static int splitIndex = 0;
        private static bool useThiefSplit = true;
        public static void Init()
        {
            CaptureManager.DigestCompleted += CompareFrames;
        }
        public static void SetThiefSplit(bool value) => useThiefSplit = value;
        public static bool GetThiefSplit() => useThiefSplit;
        public static void IncrementSplitIndex(int ammount = 1) { splitIndex += ammount; SetSplitComparisons(); }
        public static void DeincrementSplitIndex() { splitIndex--; SetSplitComparisons(); }
        public static void ResetSplitIndex() { splitIndex = 1; SetSplitComparisons(); }
        public static SplitData GetCurrentComparison() => currentComparison;
        public static SplitData GetNextComparison() => nextComparison;
        public static SplitState GetCurrentSplitState() => splitState;
        public static RECT GetCrop() => splitIndex == 1 ? Constants.startCrop : Constants.crop;
        public static int GetSplitIndex() => splitIndex;
        public static void UpdateSplitCroppings(double left, double right, double top, double bottom) { foreach (SplitData split in splits) { split.UpdateImageCropping(left, right, top, bottom); } }
        private static float CalculateStartSimilarity() { return (float)Math.Round((1 - Settings.Default.startSimilarity + 0.1f * 4.5f) * 0.1f, 3); }
        private static void SetSplitComparisons() 
        {
            nextComparison = splitIndex <= splits.Length - 1 ? splits[splitIndex + 1]: null;
            currentComparison = splits[splitIndex];
            previousComparison = splitIndex >= 1 ? splits[splitIndex - 1] : splits[0];
        }

        private static void SetSplitData()
        {
            try
            {
                splits = new SplitData[17]
                {
                    new SplitData("Start Up / Reset", Settings.Default.menuPath, Settings.Default.resetSimilarity),
                    new SplitData("Main Menu / Start", Settings.Default.menuPath, Settings.Default.loadsSimilarity, startCrop:true, removeColor:true),
                    new SplitData("Dream World", Settings.Default.dwPath, Settings.Default.loadsSimilarity),
                    new SplitData("An Unexpected Party", Settings.Default.aupPath, Settings.Default.loadsSimilarity),
                    new SplitData("Roast Mutton", Settings.Default.rmPath, Settings.Default.loadsSimilarity),
                    new SplitData("Troll-Hole", Settings.Default.thPath, Settings.Default.loadsSimilarity - 0.03f),
                    new SplitData("Over Hill and Under Hill", Settings.Default.ohPath, Settings.Default.loadsSimilarity),
                    new SplitData("Riddles in the Dark", Settings.Default.riddlesPath, Settings.Default.loadsSimilarity),
                    new SplitData("Flies and Spiders", Settings.Default.fasPath, Settings.Default.loadsSimilarity),
                    new SplitData("Barrels out of Bond", Settings.Default.boobPath, Settings.Default.loadsSimilarity),
                    new SplitData("A Warm Welcome", Settings.Default.awwPath, Settings.Default.loadsSimilarity),
                    new SplitData("Thief", Settings.Default.thiefPath, Settings.Default.thiefSimilarity),
                    new SplitData("A Warm Welcome", Settings.Default.awwPath, Settings.Default.loadsSimilarity),
                    new SplitData("Inside Information", Settings.Default.iiPath, Settings.Default.loadsSimilarity),
                    new SplitData("Gathering of the Clouds", Settings.Default.gotcPath, Settings.Default.loadsSimilarity),
                    new SplitData("Clouds Burst", Settings.Default.tcbPath, Settings.Default.loadsSimilarity),
                    new SplitData("Finished", Settings.Default.finalPath, Settings.Default.finalSimilarity)
                };
            }
            catch
            {
                // Start window to fix split images.
            }
            
            currentComparison = splits[1];
            resetComparison = splits[0];
        }

        public static void CompareFrames(DigestArgs args)
        {
            Digest d = args.digest;
            bool c = currentComparison.IsDigestSimilar(d);
            bool n = null != nextComparison ? nextComparison.IsDigestSimilar(d) : false;
            bool r = resetComparison.IsDigestSimilar(d);

            // Should only fire if it sees thief split again AFTER splitting.
            if (useThiefSplit)
            {
                if(splitIndex == 12)
                {
                    bool p = previousComparison.IsDigestSimilar(d);
                    if (p)
                    {
                        LivesplitManager.Unsplit();
                        LivesplitManager.Split();
                    }
                }
            }
            else
            {
                if (splitIndex == 10)
                {
                    IncrementSplitIndex(2);
                    LivesplitManager.OnLivesplitAction();
                }
            }

            if (r)
            {
                ResetSplitIndex();
                splitState = SplitState.WAITING;
                LivesplitManager.Reset();
            }

            if (splitState == SplitState.WAITING)
            {
                float sim = ImagePhash.GetCrossCorrelation(currentComparison.GetDigest(), d);
                if(sim <= CalculateStartSimilarity())
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
                    LivesplitManager.Split();
                    if(splitIndex == 10)
                    {
                        IncrementSplitIndex(2);
                    }
                    else if(splitIndex == 16)
                    {
                        ResetSplitIndex();
                        splitState = SplitState.WAITING;
                    }
                    else
                    {
                        IncrementSplitIndex(1);
                        splitState = SplitState.LOADING;
                        LivesplitManager.Pause();
                    }
                }
            }
        }

        public static void UpdateSplitSimilarity()
        {
            splits[0].SetSimilarity((float)Settings.Default.resetSimilarity);
            splits[1].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[2].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[3].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[4].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[5].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[6].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[7].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[8].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[9].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[10].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[11].SetSimilarity((float)Settings.Default.thiefSimilarity);
            splits[12].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[13].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[14].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[15].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[16].SetSimilarity((float)Settings.Default.finalSimilarity);
        }
    }
}

using System;
using System.Drawing;
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
        public static bool Init()
        {
            return SetSplitData();
        }

        public static void SetThiefSplit(bool value) => useThiefSplit = value;
        public static bool GetThiefSplit() => useThiefSplit;
        public static void IncrementSplitIndex(int ammount = 1) { splitIndex += ammount; AdjustSplitComparisons(); }
        public static void DeincrementSplitIndex() { splitIndex--; AdjustSplitComparisons(); }
        public static void ResetSplitIndex() { splitIndex = 1; AdjustSplitComparisons(); }
        public static SplitData GetCurrentComparison() => currentComparison;
        public static SplitData GetNextComparison() => nextComparison;
        public static SplitState GetCurrentSplitState() => splitState;
        public static Rectangle GetCrop() => splitIndex == 1 ? Constants.startCrop : Constants.crop;
        public static int GetSplitIndex() => splitIndex;
        public static SplitData[] GetSplitDataArray() => splits;
        public static void UpdateSplit(int index, string path) => splits[index].UpdateSplitImage(path);
        public static void UpdateSplitsFinalCrop() { foreach (SplitData split in splits) { split.UpdateImgWorkableCrop(); } }
        private static float CalculateStartSimilarity() { return (float)Math.Round(Settings.Default.startSimilarity / 2f - .34f, 2); }
        private static void AdjustSplitComparisons() 
        {
            nextComparison = splitIndex <= splits.Length - 1 ? splits[splitIndex + 1]: null;
            currentComparison = splits[splitIndex];
            previousComparison = splitIndex >= 1 ? splits[splitIndex - 1] : splits[0];
        }

        private static bool SetSplitData()
        {
            splits = new SplitData[17]
            {
                    new SplitData("Start Up / Reset", 0, Settings.Default.menuPath, Settings.Default.resetSimilarity),
                    new SplitData("Main Menu / Start", 69, Settings.Default.menuPath, Settings.Default.loadsSimilarity, startCrop:true, removeColor:true),
                    new SplitData("Dream World", 1, Settings.Default.dwPath, Settings.Default.loadsSimilarity),
                    new SplitData("An Unexpected Party", 2, Settings.Default.aupPath, Settings.Default.loadsSimilarity),
                    new SplitData("Roast Mutton", 3, Settings.Default.rmPath, Settings.Default.loadsSimilarity),
                    new SplitData("Troll Hole", 4, Settings.Default.thPath, Settings.Default.loadsSimilarity),
                    new SplitData("Over Hill and Under Hill", 5, Settings.Default.ohPath, Settings.Default.loadsSimilarity),
                    new SplitData("Riddles in the Dark", 6, Settings.Default.riddlesPath, Settings.Default.loadsSimilarity),
                    new SplitData("Flies and Spiders", 7, Settings.Default.fasPath, Settings.Default.loadsSimilarity),
                    new SplitData("Barrels out of Bond", 8, Settings.Default.boobPath, Settings.Default.loadsSimilarity),
                    new SplitData("A Warm Welcome", 9, Settings.Default.awwPath, Settings.Default.loadsSimilarity),
                    new SplitData("Thief", 10, Settings.Default.thiefPath, Settings.Default.thiefSimilarity),
                    new SplitData("A Warm Welcome", 420, Settings.Default.awwPath, Settings.Default.loadsSimilarity),
                    new SplitData("Inside Information", 11, Settings.Default.iiPath, Settings.Default.loadsSimilarity),
                    new SplitData("Gathering of the Clouds", 12, Settings.Default.gotcPath, Settings.Default.loadsSimilarity),
                    new SplitData("Clouds Burst", 13, Settings.Default.tcbPath, Settings.Default.loadsSimilarity),
                    new SplitData("Finished", 14, Settings.Default.finalPath, Settings.Default.finalSimilarity)
            };

            bool successfulLoad = true;

            for(int i = 0; i < splits.Length; i++)
            {
                try
                {
                    splits[i].UpdateSplitImage();
                }
                catch
                {
                    successfulLoad = false;
                }
            }

            currentComparison = splits[1];
            resetComparison = splits[0];

            return successfulLoad;
        }

        public static void CompareFrames(Digest digest)
        {
            Digest d = digest;
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

        public static void UpdateImagesPathSetting()
        {
            Settings.Default.menuPath = splits[0].GetImagePath();
            Settings.Default.dwPath = splits[2].GetImagePath();
            Settings.Default.aupPath = splits[3].GetImagePath();
            Settings.Default.rmPath = splits[4].GetImagePath();
            Settings.Default.thPath = splits[5].GetImagePath();
            Settings.Default.ohPath = splits[6].GetImagePath();
            Settings.Default.riddlesPath = splits[7].GetImagePath();
            Settings.Default.fasPath = splits[8].GetImagePath();
            Settings.Default.boobPath = splits[9].GetImagePath();
            Settings.Default.awwPath = splits[10].GetImagePath();
            Settings.Default.thiefPath = splits[11].GetImagePath();
            Settings.Default.iiPath = splits[13].GetImagePath();
            Settings.Default.gotcPath = splits[14].GetImagePath();
            Settings.Default.tcbPath = splits[15].GetImagePath();
            Settings.Default.finalPath = splits[16].GetImagePath();
        }

        public static void UpdateSplitSimilarity()
        {
            // 0 = reset comparison 
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
            // 11 = thief comparison
            splits[11].SetSimilarity((float)Settings.Default.thiefSimilarity);
            splits[12].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[13].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[14].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[15].SetSimilarity((float)Settings.Default.loadsSimilarity);
            // 16 = barrel touch comparison (final split)
            splits[16].SetSimilarity((float)Settings.Default.finalSimilarity);
        }
    }
}

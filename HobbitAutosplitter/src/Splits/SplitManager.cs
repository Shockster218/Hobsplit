using System;
using System.Linq;
using System.IO;
using Shipwreck.Phash;
using System.Windows.Media.Imaging;

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
        public static void SetThiefSplit(bool value) { useThiefSplit = value; }
        public static bool GetThiefSplit() { return useThiefSplit; }
        public static void IncrementSplitIndex(int ammount = 1) { splitIndex += ammount; SetSplitData(); }
        public static void DeincrementSplitIndex() { splitIndex--; SetSplitData(); }
        public static void ResetSplitIndex() { splitIndex = 1; SetSplitData(); }
        public static SplitData GetCurrentComparison() { return currentComparison; }
        public static SplitData GetNextComparison() { return nextComparison; }
        public static SplitState GetCurrentSplitState() { return splitState; }
        public static RECT GetCrop() { return splitIndex == 1 ? Constants.startCrop : Constants.crop; }
        public static int GetSplitIndex() { return splitIndex; }
        public static void UpdateSplitCroppings(double left, double right, double top, double bottom)
        {
            foreach(SplitData split in splits)
            {
                split.UpdateImageCropping(left, right, top, bottom);
            }
        }
        private static void SetSplitData() 
        {
            nextComparison = splitIndex <= splits.Length - 1 ? splits[splitIndex + 1]: null;
            currentComparison = splits[splitIndex];
            previousComparison = splitIndex >= 1 ? splits[splitIndex - 1] : splits[0];
        }

        //public static void PopulateSplitData()
        //{
        //    string[] sorted = Directory.EnumerateFiles(Environment.CurrentDirectory + "\\Assets\\Splits")
        //        .Where(file => new string[] { ".jpg", ".jpeg", ".png", ".bmp" }
        //        .Contains(Path.GetExtension(file)))
        //        .CustomSort().ToArray();
        //
        //    if (sorted.Length != 15)
        //    {
        //        App.Current.Dispatcher.Invoke(() => MainWindow.instance.ShowNotEnoughSplitsMessageBox());
        //        return;
        //    }
        //
        //    splits = new SplitData[17]
        //    {
        //        new SplitData("Start Up / Reset", sorted[0], similarity:0.9f),
        //        new SplitData("Main Menu / Start", sorted[0], startCrop:true, removeColor:true),
        //        new SplitData("Dream World", sorted[1]),
        //        new SplitData("An Unexpected Party", sorted[2]),
        //        new SplitData("Roast Mutton", sorted[3]),
        //        new SplitData("Troll-Hole", sorted[4], similarity:0.93f),
        //        new SplitData("Over Hill and Under Hill", sorted[5]),
        //        new SplitData("Riddles in the Dark", sorted[6]),
        //        new SplitData("Flies and Spiders", sorted[7]),
        //        new SplitData("Barrels out of Bond", sorted[8]),
        //        new SplitData("A Warm Welcome", sorted[9]),
        //        new SplitData("Thief", sorted[10], similarity:0.975f),
        //        new SplitData("A Warm Welcome", sorted[9]),
        //        new SplitData("Inside Information", sorted[11]),
        //        new SplitData("Gathering of the Clouds", sorted[12]),
        //        new SplitData("Clouds Burst", sorted[13]),
        //        new SplitData("Finished", sorted[14], similarity:0.9f)
        //    };
        //
        //    currentComparison = splits[1];
        //    resetComparison = splits[0];
        //}

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

        private static float CalculateStartSimilarity() { return (float)Math.Round((1 - Settings.Default.startSimilarity + 0.1f * 4.5f) * 0.1f, 3);  }
    }
}

using System;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Windows;

namespace HobbitAutosplitter
{
    public static class SplitManager
    {
        public static EventHandler OnSplit;
        public static EventHandler OnUnsplit;
        public static EventHandler OnReset;
        public static EventHandler OnPause;

        public static SplitData currentComparison;
        public static SplitData previousComparison;

        private static float universalSimilarity;
        private static int splitIndex;

        private static bool started;
        private static bool loading;

        private static LoadSplitData[] splits;

        private static ResetSplitData resetSplit;
        private static EndSplitData endSplit;
        private static ThiefSplitData thiefSplit;

        public static void Init()
        {
            OnSplit += (s,e) => SetSplitReference();
            CaptureManager.FrameCreated += CheckLoadFrames;
            PopulateSplitData();
        }

        public static SplitData GetCurrentSplit() { return splits[splitIndex]; }

        public static void IncrementSplitIndex() { splitIndex++; }

        public static void ResetSplitIndex() { splitIndex = 0; }

        public static float GetUniversalSimilarity() { return universalSimilarity; }

        public static void SetUniversalSimilarity(float similarity) { universalSimilarity = similarity > 1 ? 1 : similarity; }

        private static void PopulateSplitData()
        {
            string[] splitImagePaths = Directory.EnumerateFiles(Environment.CurrentDirectory + "\\split_images").CustomSort().ToArray();
            if (splitImagePaths.Length != 15)
            {
                // Say not enough images found
                return;
            }

            splits = new LoadSplitData[12]
            {
                new LoadSplitData("Dream World", splitImagePaths[0]),               // 1   
                new LoadSplitData("An Unexpected Party", splitImagePaths[1]),       // 2
                new LoadSplitData("Roast Mutton", splitImagePaths[2]),              // 3
                new LoadSplitData("Troll Hole", splitImagePaths[3]),                // 4
                new LoadSplitData("Over Hill and Under Hill", splitImagePaths[4]),  // 5
                new LoadSplitData("Riddles in the Dark", splitImagePaths[5]),       // 6
                new LoadSplitData("Flies and Spiders", splitImagePaths[6]),         // 7
                new LoadSplitData("Barrels out of Bond", splitImagePaths[7]),       // 8
                new LoadSplitData("A Warm Welcome", splitImagePaths[8]),            // 9
                new LoadSplitData("Inside Info", splitImagePaths[10]),              // 11
                new LoadSplitData("Gathering of the Clouds", splitImagePaths[11]),  // 12
                new LoadSplitData("Clouds Burst", splitImagePaths[12]),             // 13
            };

            resetSplit = new ResetSplitData("Reset", splitImagePaths[13]);
            endSplit = new EndSplitData("End", splitImagePaths[12]);
            thiefSplit = new ThiefSplitData("Thief", splitImagePaths[14]);

            currentComparison = resetSplit;
        }

        public static void SetSplitReference()
        {
            Application.Current.Dispatcher.Invoke(() => MainWindow.instance.ChangeComparisonReference(currentComparison.GetImage()));
        }

        public static void CheckLoadFrames(object sender, FrameEventArgs frameArgs)
        {

        }
    }
}

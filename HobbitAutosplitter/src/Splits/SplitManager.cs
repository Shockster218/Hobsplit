using System.Drawing;

namespace HobbitAutosplitter
{
    public static class SplitManager
    {
        private static float universalSimilarity;
        private static int splitIndex;
        private static SplitData[] splits = new SplitData[15];
        private static Image loadImage;

        public static void Initialize()
        {
            //universalSimilarity = Properties.Settings.Default.unisim;
            PopulateSplitData();
        }

        public static SplitData GetCurrentSplit() { return splits[splitIndex]; }

        public static void IncrementSplitIndex() { splitIndex++; }

        public static void ResetSplitIndex() { splitIndex = 0; }

        public static float GetUniversalSimilarity() { return universalSimilarity; }

        public static void SetUniversalSimilarity(float similarity) { universalSimilarity = similarity > 1 ? 1 : similarity; }

        public static Image GetLoadImage() { return loadImage; }

        public static void SetLoadImage(Image image) { loadImage = image; }

        private static void PopulateSplitData()
        {
            //implmenet json file here
            //else
            splits = new SplitData[]
            {
                new SplitData("Loading"),
                new SplitData("Start"),
                new SplitData("DreamWorld"),
                new SplitData("An Unexpected Party"),
                new SplitData("Roast Mutton"),
                new SplitData("Troll Hole"),
                new SplitData("Over Hill and Under Hill"),
                new SplitData("Riddles in the Dark"),
                new SplitData("Flies and Spiders"),
                new SplitData("Barrels out of Bond"),
                new SplitData("Thief Split"),
                new SplitData("A Warm Welcome"),
                new SplitData("Inside Info"),
                new SplitData("Gathering of the Clouds"),
                new SplitData("Clouds Burst"),
            };
        }
    }
}

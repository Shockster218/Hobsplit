namespace HobbitAutosplitter
{
    public static class Constants
    {
        public const int width = 640;
        public const int height = 480;
        public const string loadingKeyword = "loading";

        public static readonly string[] splitNames =
        {
            "Main Menu",
            "Dream World",
            "An Unexpected Party",
            "Roast Mutton",
            "Troll Hole",
            "Over Hill and Under Hill",
            "Riddles in the Dark",
            "Flies and Spiders",
            "Barrels out of Bond",
            "AWW - Pre Thief",
            "Thief Split",
            "AWW - Post Thief",
            "Inside Information",
            "Gathering of the Clouds",
            "Clouds Burst",
            "Finished"
        };

        public static readonly RECT crop = new RECT(40, 100, 460, 200);

        // Might need individual split croppings in the future. Right now hardcoded but might allow user to set in future.
        // Edit: Split specific croppings wont work unless I create multiple digests for current, next, previous split which causes performance issues on UI thread. Keeping for reference.
        public static readonly RECT[] splitCrops =
        {
                   // L   T   R    B
            new RECT(160,310,470,380),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(180,140,460,340),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(30,120,450,170),
            new RECT(5,150,250,400)
        };
    }

    public enum SplitState
    {
        IDLE,
        GAMEPLAY,
        LOADING,
        STARTUP
    }

    public enum InvokeMode
    {
        SYNC,
        ASYNC
    }
}
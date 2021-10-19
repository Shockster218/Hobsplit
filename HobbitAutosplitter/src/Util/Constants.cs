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
            "A Warm Welcome",
            "AWW - Post Thief Split",
            "Inside Information",
            "Gathering of the Clouds",
            "Clouds Burst",
        };

        public static readonly RECT?[] splitCrops =
        {
            new RECT(160,310,width-175,height-100),           // Main Menu
            new RECT(200,200,width,height),           // Dream World   ==
            new RECT(200,200,width,height),           // AUP           ====
            new RECT(200,200,width,height),           // RM            ======
            new RECT(200,200,width,height),           // Troll hole    =======
            new RECT(200,200,width,height),           // Over hill     =========
            new RECT(200,200,width,height),           // Riddles       ==========
            new RECT(200,200,width,height),           // FaS           ===========  All the Same
            new RECT(200,200,width,height),           // BooB          ==========
            new RECT(200,200,width,height),           // AWW           =========
            new RECT(200,200,width,height),           // Thief         =======
            new RECT(200,200,width,height),           // Inside Info   ======
            new RECT(200,200,width,height),           // GOTC          ====
            new RECT(200,200,width,height),           // Clouds Burst  ==
            new RECT(200,200,width,height)            // Finished
        };
    }

    public enum SplitState
    {
        IDLE,
        WAITING,
        LOADING,
        NEWLEVEL,
    }

    public enum InvokeMode
    {
        SYNC,
        ASYNC
    }
}
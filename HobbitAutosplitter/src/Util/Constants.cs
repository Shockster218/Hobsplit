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
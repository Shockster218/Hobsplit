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

        public static readonly RECT crop = new RECT(180, 140, 460, 340);

        // Might need individual split croppings in the future. Right now hardcoded but might allow user to set in future.
        public static readonly RECT[] splitCrops =
        {
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480),
            new RECT(0,0,640,480)
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
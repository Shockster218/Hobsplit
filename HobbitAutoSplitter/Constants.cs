namespace HobbitAutoSplitter
{
    public static class Constants
    {
        public const int trueWidth = 640;
        public const int trueHeight = 480;
        public const string loadingKeyword = "loading";
        public static readonly string[] resetKeywords = {
            "newgame",
            "loadgame",
            "woadgame",
            "badgame",
        };
        public static readonly string[] levelsDetection = {
            "anunexpectedparty",
            "roastmutton",        
            "troll-hole",
            "overhillandunderhill",
            "riddlesinthedark",
            "fliesandspiders",
            "barrelsoutofbond",
            "awarmwelcome",
            "insideinformation",
            "thegatheringoftheclouds",
            "cloudsburst"
        };
        public static readonly string[] levels = {
            "Main Menu",
            "Dream World",
            "An Unexpected Party",
            "Roast Mutton",
            "Troll-Hole",
            "Over Hill and Under Hill",
            "Riddles in the Dark",
            "Flies and Spiders",
            "Barrels out of Bond",
            "A Warm Welcome",
            "Inside Information",
            "The Gathering of the Clouds",
            "Clouds Burst"
        };
    }

    public enum States
    {
        READYTOSTART,
        STARTED,
        GAMEPLAY,
        LOADING,
        FINISHED
    }
}
namespace HobbitAutosplitter
{
    public static class Constants
    {
        public const int trueWidth = 640;
        public const int trueHeight = 480;
        public const string loadingKeyword = "loading";
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
namespace HobbitAutosplitter
{
    public static class Constants
    {
        public const int width = 640;
        public const int height = 360;
        public const string loadingKeyword = "loading";

        public static readonly RECT crop = new RECT(175, 75, 525, 150);
        public static readonly RECT startCrop = new RECT(100, 120, 460, 170);
    }

    public enum SplitState
    {
        STARTUP,
        WAITING,
        GAMEPLAY,
        LOADING
    }

    public enum InvokeMode
    {
        SYNC,
        ASYNC
    }

    public enum LivesplitAction
    {
        NONE,
        SPLIT,
        UNSPLIT,
        RESET,
        PAUSE
    }
}
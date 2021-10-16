namespace HobbitAutosplitter
{
    public static class Constants
    {
        public const int width = 640;
        public const int height = 480;
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

    public enum InvokeMode
    {
        SYNC,
        ASYNC,
        UI,      
    }
}
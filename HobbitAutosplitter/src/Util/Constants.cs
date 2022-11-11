using System.Drawing;

namespace HobbitAutosplitter
{
    public static class Constants
    {
        public const int previewWidth = 640;
        public const int previewHeight = 480;
        public const int comparisonWidth = 320;
        public const int comparisonHeight = 240;
        public const string loadingKeyword = "loading";

        public static readonly Rectangle crop = new Rectangle(175, 75, 525, 150);
        public static readonly Rectangle startCrop = new Rectangle(100, 120, 460, 170);
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
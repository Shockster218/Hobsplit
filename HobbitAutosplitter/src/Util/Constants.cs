using System.Drawing;

namespace HobbitAutosplitter
{
    public static class Constants
    {
        public const int comparisonWidth = 320;
        public const int comparisonHeight = 240;
        public const string loadingKeyword = "loading";

        public static readonly Rectangle crop = new Rectangle(88, 38, 263 - 88, 75 - 38);
        public static readonly Rectangle startCrop = new Rectangle(50, 120, 230 - 50, 170 - 120);
    }

    public enum SplitState
    {
        // State when game is first started and waiting for main menu capture
        STARTUP,
        // State AFTER main menu is captured and waiting for run to start.
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
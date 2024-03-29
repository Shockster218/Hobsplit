﻿namespace HobbitAutosplitter
{
    public static class Constants
    {
        public const int width = 640;
        public const int height = 360;
        public const string loadingKeyword = "loading";

        public static readonly string[] splitNames =
        {
            "Start Up/ Reset",
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

        public static readonly RECT crop = new RECT(175, 75, 525, 150);
        public static readonly RECT startCrop = new RECT(100, 120, 460, 170);
        //16:9
        //public static readonly RECT crop = new RECT(40, 75, 460, 150); 640x360
        //public static readonly RECT crop = new RECT(56, 105, 644, 210); 896x504

        //4:3
        //public static readonly RECT crop = new RECT(40, 100, 460, 200); 640x480
        //public static readonly RECT crop = new RECT(50, 125, 575, 250); 800x600
        //public static readonly RECT crop = new RECT(60, 150, 690, 300); 960x720

        // Might need individual split croppings in the future. Right now hardcoded but might allow user to set in future.
        // Edit: Split specific croppings wont work unless I create multiple digests for current, next, previous split which causes performance issues on UI thread. Keeping for reference.
        //public static readonly RECT[] splitCrops =
        //{
        //           // L   T   R    B
        //    new RECT(160,310,470,380),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(180,140,460,340),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(30,120,450,170),
        //    new RECT(5,150,250,400)
        //};
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
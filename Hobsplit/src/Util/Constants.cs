﻿using System.Drawing;

namespace Hobsplit
{
    public static class Constants
    {
        public const int comparisonWidth = 160;
        public const int comparisonHeight = 120;

        public static readonly Rectangle crop = new Rectangle(20, 32, 55, 50);
        public static readonly Rectangle startCrop = new Rectangle(35, 45, 45, 70);

        public static readonly string[] LevelNames =
        {
            "Main Menu",
            "Dream World",
            "Unexpected Party",
            "Roast Mutton",
            "Troll Hole",
            "Overhill",
            "Riddles",
            "Flies and Spiders",
            "Barrels",
            "A Warm Welcome",
            "Thief",
            "A Warm Welcome",
            "Inside Info",
            "Gathering",
            "Clouds Burst",
            "End"
        };
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

    public enum SplitIndex
    {
        RESET,
        START,
        DW,
        AUP,
        RM,
        TH,
        OH,
        RITD,
        FAS,
        BOOB,
        AWWPRE,
        THIEF,
        AWWPOST,
        II,
        GOTC,
        TCB,
        DONE
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
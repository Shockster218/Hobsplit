using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Timers;
using Shipwreck.Phash;

namespace Hobsplit
{
    public static class SplitManager
    {
        public static AdvancedSplitInformationEventHandler AdvancedSplitInfo;

        private static SplitData currentComparison;
        private static SplitData nextComparison;
        private static SplitData previousComparison;
        private static SplitData resetComparison;
        private static Timer advancedInfoTimer;

        private static SplitData[] splits;

        private static SplitState splitState = SplitState.STARTUP;

        private static int splitIndex = 0;

        private static float currentSim = 0f;
        private static float resetSim = 0f;
        private static float lastStartSim = 0.3f;

        private static bool startMenuFadeIn = false;

        public static bool Init()
        {
            if (advancedInfoTimer == null)
            {
                advancedInfoTimer = new Timer(250);
                advancedInfoTimer.Elapsed += (s, e) => AdvancedInfoSender();
                ProcessManager.OBSOpenedEvent += StartAdvancedInfoTimer;
            }
            return SetSplitData();
        }
        public static void IncrementSplitIndex(int ammount = 1) { splitIndex += ammount; AdjustSplitComparisons(); }
        public static void DeincrementSplitIndex() { splitIndex--; AdjustSplitComparisons(); }
        public static void ResetSplitIndex() { splitIndex = 1; AdjustSplitComparisons(); }
        public static SplitData GetResetComparison() => resetComparison;
        public static SplitData GetCurrentComparison() => currentComparison;
        public static SplitData GetNextComparison() => nextComparison;
        public static SplitState GetSplitState() => splitState;
        public static int GetSplitIndex() => splitIndex;
        public static SplitData[] GetSplitDataArray() => splits;
        public static void UpdateSplit(int index, string path) => splits[index].UpdateSplitImage(path);
        public static void UpdateSplitsImageWorkable() { foreach (SplitData split in splits) { split.UpdateImgWorkableCrop(); } }
        private static void AdjustSplitComparisons() 
        {
            nextComparison = splitIndex < splits.Length - 1 ? splits[splitIndex + 1]: null;
            currentComparison = splits[splitIndex];
            previousComparison = splitIndex >= 1 ? splits[splitIndex - 1] : splits[0];
        }

        private static bool SetSplitData()
        {
            splits = new SplitData[]
            {
                    new SplitData("Start Up / Reset", 0, Settings.Default.menuPath, Settings.Default.resetSimilarity),
                    new SplitData("Main Menu / Start", 69, Settings.Default.menuPath, Settings.Default.startSimilarity, removeColor:true),
                    new SplitData("Dream World", 1, Settings.Default.dwPath, Settings.Default.loadsSimilarity),
                    new SplitData("An Unexpected Party", 2, Settings.Default.aupPath, Settings.Default.loadsSimilarity),
                    new SplitData("Roast Mutton", 3, Settings.Default.rmPath, Settings.Default.loadsSimilarity),
                    new SplitData("Troll Hole", 4, Settings.Default.thPath, Settings.Default.loadsSimilarity),
                    new SplitData("Over Hill and Under Hill", 5, Settings.Default.ohPath, Settings.Default.loadsSimilarity),
                    new SplitData("Riddles in the Dark", 6, Settings.Default.riddlesPath, Settings.Default.loadsSimilarity),
                    new SplitData("Flies and Spiders", 7, Settings.Default.fasPath, Settings.Default.loadsSimilarity),
                    new SplitData("Barrels out of Bond", 8, Settings.Default.boobPath, Settings.Default.loadsSimilarity),
                    new SplitData("A Warm Welcome", 9, Settings.Default.awwPath, Settings.Default.loadsSimilarity),
                    new SplitData("Thief", 10, Settings.Default.thiefPath, Settings.Default.thiefSimilarity),
                    new SplitData("A Warm Welcome", 420, Settings.Default.awwPath, Settings.Default.loadsSimilarity),
                    new SplitData("Inside Information", 11, Settings.Default.iiPath, Settings.Default.loadsSimilarity),
                    new SplitData("Gathering of the Clouds", 12, Settings.Default.gotcPath, Settings.Default.loadsSimilarity),
                    new SplitData("Clouds Burst", 13, Settings.Default.tcbPath, Settings.Default.loadsSimilarity),
                    new SplitData("Finished", 14, Settings.Default.finalPath, Settings.Default.finalSimilarity)
            };

            bool successfulLoad = true;

            for(int i = 0; i < splits.Length; i++)
            {
                string path = splits[i].GetImagePath();
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) successfulLoad = false;
                else splits[i].UpdateSplitImage();
            }

            splitIndex = 1;
            currentComparison = splits[1];
            resetComparison = splits[0];

            return successfulLoad;
        }

        public static void CompareFrames(Digest digest)
        {
            Digest d = digest;
            bool c = currentComparison.IsDigestSimilar(d);
            bool n = null != nextComparison ? nextComparison.IsDigestSimilar(d) : false;
            bool r = resetComparison.IsDigestSimilar(d);

            // Should only fire if it sees thief split again AFTER splitting.
            if (Settings.Default.useThief)
            {
                if(splitIndex == (int)SplitIndex.AWWPOST)
                {
                    bool p = previousComparison.IsDigestSimilar(d);
                    if (p)
                    {
                        LivesplitManager.Unsplit();
                        LivesplitManager.Split();
                        PlayThiefSound();
                    }
                }
            }

            // Reset check
            if (r && !Settings.Default.manualSplit && splitState != SplitState.WAITING)
            {
                ResetSplitIndex();
                splitState = SplitState.WAITING;
                LivesplitManager.Reset();
            }

            // Start from main menu check
            if (splitState == SplitState.WAITING && !Settings.Default.manualSplit)
            {
                float sim = currentComparison.GetCurrentCorrelation(d);
                if (sim <= lastStartSim - 0.3f)
                {
                    if(!startMenuFadeIn) return;
                    IncrementSplitIndex();
                    splitState = SplitState.GAMEPLAY;
                    LivesplitManager.Split();
                    startMenuFadeIn = false;
                    lastStartSim = 0f;
                }
                else 
                {
                    lastStartSim = sim;
                    if(!startMenuFadeIn && sim >= Settings.Default.startSimilarity)
                    {
                        startMenuFadeIn = true;
                        PlayReadySound();
                    }
                }
            }

            // Load pause and start check. Also gets the autosplitter ready from start up
            if (c)
            {
                // Pause on load screen that is from the current comparison aka current level
                if (splitState == SplitState.GAMEPLAY)
                {
                    splitState = SplitState.LOADING;
                    LivesplitManager.Pause();
                }
            }
            else
            {
                // Resume after loads go away
                if(splitState == SplitState.LOADING)
                {
                    splitState = SplitState.GAMEPLAY;
                    LivesplitManager.Pause();
                }
            }

            // Check for next level load screen to split
            if (n)
            {
                // Make sure we are at dream world or over
                if(splitIndex >= (int)SplitIndex.DW && splitState == SplitState.GAMEPLAY)
                {
                    LivesplitManager.Split();
                    // Thief check. Double incremenet
                    if(splitIndex == (int)SplitIndex.AWWPRE && Settings.Default.useThief)
                    {
                        IncrementSplitIndex(2);
                        PlayThiefSound();
                    }
                    // End of run
                    else if(splitIndex == (int)SplitIndex.DONE)
                    {
                        ResetSplitIndex();
                        splitState = SplitState.WAITING;
                    }
                    // Normal split
                    else
                    {
                        IncrementSplitIndex();
                        splitState = SplitState.LOADING;
                        LivesplitManager.Pause();
                    }
                }
            }

            currentSim = currentComparison.GetCurrentCorrelation(d);
            resetSim = resetComparison.GetCurrentCorrelation(d);
        }

        public static void UpdateSplitSimilarity()
        {
            // 0 = reset comparison 
            splits[0].SetSimilarity((float)Settings.Default.resetSimilarity);
            splits[1].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[2].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[3].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[4].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[5].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[6].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[7].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[8].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[9].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[10].SetSimilarity((float)Settings.Default.loadsSimilarity);
            // 11 = thief comparison
            splits[11].SetSimilarity((float)Settings.Default.thiefSimilarity);
            splits[12].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[13].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[14].SetSimilarity((float)Settings.Default.loadsSimilarity);
            splits[15].SetSimilarity((float)Settings.Default.loadsSimilarity);
            // 16 = barrel touch comparison (final split)
            splits[16].SetSimilarity((float)Settings.Default.finalSimilarity);
        }

        public static void PlayReadySound()
        {
            using(SoundPlayer player = new SoundPlayer(Environment.CurrentDirectory + "\\Assets\\Audio\\ready.wav")) 
            {
                player.Play();
            }
        }

        public static void PlayThiefSound()
        {
            using (SoundPlayer player = new SoundPlayer(Environment.CurrentDirectory + "\\Assets\\Audio\\thief.wav"))
            {
                player.Play();
            }
        }

        private static void AdvancedInfoSender()
        {
            string currentSplitName = splits[splitIndex].GetSplitName();
            AdvancedSplitInfo?.SmartInvoke(new AdvancedSplitInfoArgs(splitIndex, currentSplitName, currentSim, resetSim, splitState));
        }

        private static void StartAdvancedInfoTimer()
        {
            advancedInfoTimer.Start();
        }
    }
}

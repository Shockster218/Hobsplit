using WindowsInput;
using WindowsInput.Native;

namespace HobbitAutosplitter
{
    public static class LivesplitManager
    {

        private static InputSimulator sim;
        private static VirtualKeyCode split;
        private static VirtualKeyCode unsplit;
        private static VirtualKeyCode reset;
        private static VirtualKeyCode pause;

        public static void Init()
        {
            sim = new InputSimulator();
            split = Settings.Default.split;
            unsplit = Settings.Default.unsplit;
            reset = Settings.Default.reset;
            pause = Settings.Default.pause;
            SplitManager.OnSplit += (e) => Split();
            SplitManager.OnUnsplit += (e) => Unsplit();
            SplitManager.OnReset += (e) => Reset();
            SplitManager.OnPause += (e) => Pause();
        }

        public static void Split()
        {
            sim.Keyboard.KeyPress(split);
        }

        public static void Unsplit()
        {
            sim.Keyboard.KeyPress(unsplit);
        }

        public static void Reset()
        {
            sim.Keyboard.KeyPress(reset);
        }

        public static void Pause()
        {
            sim.Keyboard.KeyPress(pause);
        }
    }
}

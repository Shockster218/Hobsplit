using WindowsInput;
using WindowsInput.Native;

namespace HobbitAutosplitter
{
    public static class LivesplitManager
    {
        public static DigestEventHandler OnSplit;
        public static DigestEventHandler OnUnsplit;
        public static DigestEventHandler OnReset;

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
        }

        public static void Split()
        {
            sim.Keyboard.KeyDown(split);
            OnSplit?.SmartInvoke(DigestArgs.Default);
        }

        public static void Unsplit()
        {
            sim.Keyboard.KeyDown(unsplit);
            OnUnsplit?.SmartInvoke(DigestArgs.Default);
        }

        public static void Reset()
        {
            sim.Keyboard.KeyDown(reset);
            OnReset?.SmartInvoke(DigestArgs.Default);
        }

        public static void Pause()
        {
            sim.Keyboard.KeyDown(pause);
        }
    }
}

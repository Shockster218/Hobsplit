using WindowsInput;
using WindowsInput.Native;

namespace HobbitAutosplitter
{
    public static class LivesplitManager
    {
        public static LivesplitActionEventHandler OnLivesplitAction;

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
            sim.Keyboard.KeyUp(split);
            OnLivesplitAction?.SmartInvoke(LivesplitAction.SPLIT);
        }

        public static void Unsplit()
        {
            sim.Keyboard.KeyDown(unsplit);
            sim.Keyboard.KeyUp(unsplit);
            OnLivesplitAction?.SmartInvoke(LivesplitAction.UNSPLIT);
        }

        public static void Reset()
        {
            sim.Keyboard.KeyDown(reset);
            sim.Keyboard.KeyUp(reset);
            OnLivesplitAction?.SmartInvoke(LivesplitAction.RESET);
        }

        public static void Pause()
        {
            sim.Keyboard.KeyDown(pause);
        }
    }
}

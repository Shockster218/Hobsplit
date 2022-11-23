using WindowsInput;
using WindowsInput.Native;
using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Hobsplit
{
    public static class LivesplitManager
    {
        public static LivesplitActionEventHandler OnLivesplitAction;

        private static IKeyboardMouseEvents kbHook;

        private static InputSimulator sim;

        private static VirtualKeyCode split;
        private static VirtualKeyCode unsplit;
        private static VirtualKeyCode reset;
        private static VirtualKeyCode pause;

        public static void Init()
        {
            SetKeybinds();
            sim = new InputSimulator();
            kbHook = Hook.GlobalEvents();
            kbHook.KeyDown += KeyDown;
        }

        private static void SetSplitKeybind() { split = Settings.Default.split; }
        private static void SetUnsplitKeybind() { unsplit = Settings.Default.unsplit; }
        private static void SetResetKeybind() { reset = Settings.Default.reset; }
        private static void SetPauseKeybind() { pause = Settings.Default.pause; }

        public static void SetKeybinds()
        {
            SetSplitKeybind();
            SetUnsplitKeybind();
            SetResetKeybind();
            SetPauseKeybind();
        }

        private static void KeyDown(object sender, KeyEventArgs e)
        {
            if (!Settings.Default.manualSplit) return;
            kbHook.KeyDown -= KeyDown;

            if (e.KeyValue == (int)split && SplitManager.GetCurrentSplitState() == SplitState.WAITING)
            {
                SplitManager.IncrementSplitIndex();
                Split();
            }
            else if (e.KeyValue == (int)reset)
            {
                SplitManager.ResetSplitIndex();
                Reset();
            }

            // Delay here needed to resub event otherwise it would fire 10 times at once.
            Task.Factory.StartNew(async() => 
            {
                await Task.Delay(100);
                kbHook.KeyDown += KeyDown;
            });
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
            sim.Keyboard.KeyUp(pause);
        }
    }
}

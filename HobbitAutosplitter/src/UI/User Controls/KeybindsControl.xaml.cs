using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput.Native;

namespace HobbitAutosplitter
{

    public partial class KeybindsControl : UserControl
    {
        public VirtualKeyCode splitKey { get; private set; }
        public VirtualKeyCode unsplitKey { get; private set; }
        public VirtualKeyCode resetKey { get; private set; }
        public VirtualKeyCode pauseKey { get; private set; }

        public KeybindsControl()
        {
            InitializeComponent();
            GetAndSetKeycodes();
        }

        private void GetAndSetKeycodes()
        {
            splitKey = Settings.Default.split;
            unsplitKey = Settings.Default.unsplit;
            resetKey = Settings.Default.reset;
            pauseKey = Settings.Default.pause;

            Split_Button.Content = KeyInterop.KeyFromVirtualKey((int)Settings.Default.split).ToString();
            Unsplit_Button.Content = KeyInterop.KeyFromVirtualKey((int)Settings.Default.unsplit).ToString();
            Reset_Button.Content = KeyInterop.KeyFromVirtualKey((int)Settings.Default.reset).ToString();
            Pause_Button.Content = KeyInterop.KeyFromVirtualKey((int)Settings.Default.pause).ToString();
        }

        private void Split_Button_Click(object sender, RoutedEventArgs e)
        {
            Split_Button.Content = "Waiting...";
        }

        private void Unsplit_Button_Click(object sender, RoutedEventArgs e)
        {
            Unsplit_Button.Content = "Waiting...";
        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            Reset_Button.Content = "Waiting...";
        }

        private void Pause_Button_Click(object sender, RoutedEventArgs e)
        {
            Pause_Button.Content = "Waiting...";
        }

        private void Split_Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (Split_Button.IsFocused)
            {
                splitKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                Split_Button.Content = e.Key.ToString();
                Keyboard.ClearFocus();
            }
        }

        private void Unsplit_Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (Unsplit_Button.IsFocused)
            {
                unsplitKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                Unsplit_Button.Content = e.Key.ToString();
                Keyboard.ClearFocus();
            }
        }

        private void Reset_Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (Reset_Button.IsFocused)
            {
                resetKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                Reset_Button.Content = e.Key.ToString();
                Keyboard.ClearFocus();
            }
        }

        private void Pause_Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (Pause_Button.IsFocused)
            {
                pauseKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                Pause_Button.Content = e.Key.ToString();
                Keyboard.ClearFocus();
            }
        }

        private void Split_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Split_Button.Content = KeyInterop.KeyFromVirtualKey((int)splitKey).ToString();
            Keyboard.ClearFocus();
        }

        private void Unsplit_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Unsplit_Button.Content = KeyInterop.KeyFromVirtualKey((int)unsplitKey).ToString();
            Keyboard.ClearFocus();
        }

        private void Reset_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Reset_Button.Content = KeyInterop.KeyFromVirtualKey((int)resetKey).ToString();
            Keyboard.ClearFocus();
        }

        private void Pause_Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Pause_Button.Content = KeyInterop.KeyFromVirtualKey((int)pauseKey).ToString();
            Keyboard.ClearFocus();
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowsInput.Native;

namespace Hobsplit
{

    public partial class KeybindsControl : UserControl
    {
        private VirtualKeyCode splitKey;
        private VirtualKeyCode unsplitKey;
        private VirtualKeyCode resetKey;
        private VirtualKeyCode pauseKey;

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

        public void Save()
        {
            Settings.Default.split = splitKey;
            Settings.Default.unsplit = unsplitKey;
            Settings.Default.reset = resetKey;
            Settings.Default.pause = pauseKey;
            LivesplitManager.SetKeybinds();
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
                Key key = e.Key == Key.System ? e.SystemKey : e.Key;
                splitKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(key);
                Split_Button.Content = key.ToString();
                Keyboard.ClearFocus();
            }
        }

        private void Unsplit_Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (Unsplit_Button.IsFocused)
            {
                Key key = e.Key == Key.System ? e.SystemKey : e.Key;
                unsplitKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(key);
                Unsplit_Button.Content = key.ToString();
                Keyboard.ClearFocus();
            }
        }

        private void Reset_Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (Reset_Button.IsFocused)
            {
                Key key = e.Key == Key.System ? e.SystemKey : e.Key;
                resetKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(key);
                Reset_Button.Content = key.ToString();
                Keyboard.ClearFocus();
            }
        }

        private void Pause_Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (Pause_Button.IsFocused)
            {
                Key key = e.Key == Key.System ? e.SystemKey : e.Key;
                pauseKey = (VirtualKeyCode)KeyInterop.VirtualKeyFromKey(key);
                Pause_Button.Content = key.ToString();
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

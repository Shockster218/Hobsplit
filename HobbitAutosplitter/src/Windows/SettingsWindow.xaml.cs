using System.Windows;
using System.Windows.Input;
using WindowsInput.Native;

namespace HobbitAutosplitter
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private VirtualKeyCode splitKey;
        private VirtualKeyCode unsplitKey;
        private VirtualKeyCode resetKey;
        private VirtualKeyCode pauseKey;

        public SettingsWindow()
        {
            InitializeComponent();
            GetAndSetKeycodes();
            GetAndSetToggles();
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

        private void GetAndSetToggles()
        {
            Thief_Split_Toggle.IsChecked = Settings.Default.useThief;
            Manual_Controls_Toggle.IsChecked = Settings.Default.manualSplit;
            Ready_Sound_Toggle.IsChecked = Settings.Default.playReadySound;
            Thief_Sound_Toggle.IsChecked = Settings.Default.playThiefSound;
        }

        private void Apply_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.useThief = (bool)Thief_Split_Toggle.IsChecked;
            Settings.Default.manualSplit = (bool)Manual_Controls_Toggle.IsChecked;
            Settings.Default.playReadySound = (bool)Ready_Sound_Toggle.IsChecked;
            Settings.Default.playThiefSound = (bool)Thief_Sound_Toggle.IsChecked;

            Settings.Default.split = splitKey;
            Settings.Default.unsplit = unsplitKey;
            Settings.Default.reset = resetKey;
            Settings.Default.pause = pauseKey;

            Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

using System.Windows;

namespace HobbitAutosplitter
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Apply_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.useThief = Toggles.GetThiefToggle();
            Settings.Default.manualSplit = Toggles.GetManualToggle();
            Settings.Default.playReadySound = Toggles.GetReadySoundToggle();
            Settings.Default.playThiefSound = Toggles.GetThiefSoundToggle();
            Settings.Default.autoOBS = Toggles.GetAutoOBSToggle();

            Settings.Default.split = Keybinds.splitKey;
            Settings.Default.unsplit = Keybinds.unsplitKey;
            Settings.Default.reset = Keybinds.resetKey;
            Settings.Default.pause = Keybinds.pauseKey;
            Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

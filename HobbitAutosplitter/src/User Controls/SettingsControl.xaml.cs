using System.Windows.Controls;

namespace HobbitAutosplitter
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
            GetAndSetToggles();
        }

        public bool GetThiefToggle() => (bool)Thief_Split_Toggle.IsChecked;
        public bool GetManualToggle() => (bool)Manual_Controls_Toggle.IsChecked;
        public bool GetReadySoundToggle() => (bool)Ready_Sound_Toggle.IsChecked;
        public bool GetThiefSoundToggle() => (bool)Thief_Sound_Toggle.IsChecked;


        private void GetAndSetToggles()
        {
            Thief_Split_Toggle.IsChecked = Settings.Default.useThief;
            Manual_Controls_Toggle.IsChecked = Settings.Default.manualSplit;
            Ready_Sound_Toggle.IsChecked = Settings.Default.playReadySound;
            Thief_Sound_Toggle.IsChecked = Settings.Default.playThiefSound;
        }
    }
}

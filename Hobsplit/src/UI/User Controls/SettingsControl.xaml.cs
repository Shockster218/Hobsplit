using System.Windows;
using System.Windows.Controls;

namespace Hobsplit
{
    public partial class SettingsControl : UserControl
    {
        private bool startup = true;
        public SettingsControl()
        {
            InitializeComponent();
            GetAndSetToggles();
            startup = false;
        }

        private void GetAndSetToggles()
        {
            Thief_Split_Toggle.IsChecked = Settings.Default.useThief;
            Manual_Controls_Toggle.IsChecked = Settings.Default.manualSplit;
            Ready_Sound_Toggle.IsChecked = Settings.Default.playReadySound;
            Thief_Sound_Toggle.IsChecked = Settings.Default.playThiefSound;
            Open_OBS_Toggle.IsChecked = Settings.Default.autoOBS;
        }

        public void Save()
        {
            Settings.Default.useThief = (bool)Thief_Split_Toggle.IsChecked;
            Settings.Default.manualSplit = (bool)Manual_Controls_Toggle.IsChecked;
            Settings.Default.playReadySound = (bool)Ready_Sound_Toggle.IsChecked;
            Settings.Default.playThiefSound = (bool)Thief_Sound_Toggle.IsChecked;
            Settings.Default.autoOBS = (bool)Open_OBS_Toggle.IsChecked;
        }

        private void Open_OBS_Toggle_Checked(object sender, RoutedEventArgs e) => Change_Path_Component.Toggle(true);

        private void Open_OBS_Toggle_Unchecked(object sender, RoutedEventArgs e) => Change_Path_Component.Toggle(false);

        private void Ready_Sound_Toggle_Checked(object sender, RoutedEventArgs e) { if (!startup) SplitManager.PlayReadySound(); }

        private void Thief_Sound_Toggle_Checked(object sender, RoutedEventArgs e) { if (!startup) SplitManager.PlayThiefSound(); }
    }
}

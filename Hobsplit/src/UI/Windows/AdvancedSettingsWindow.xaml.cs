using System.Windows;

namespace Hobsplit
{
    public partial class AdvancedSettingsWindow : Window
    {
        public AdvancedSettingsWindow()
        {
            InitializeComponent();
            GetAndSetToggles();
        }

        private void GetAndSetToggles()
        {
            Hide_Information_Toggle.IsChecked = Settings.Default.advShowRunInfo;
            Similarity_Toggle.IsChecked = Settings.Default.advSimilarity;
            Split_Index_Toggle.IsChecked = Settings.Default.advSplitIndex;
            Split_State_Toggle.IsChecked = Settings.Default.advSplitState;
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Similarity_Control.Save();
            Settings.Default.Save();
            SplitManager.UpdateSplitSimilarity();
            MainWindow.instance.AdvancedInformationUIToggle();
            Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save()
        {
            Settings.Default.advShowRunInfo = (bool)Hide_Information_Toggle.IsChecked;
            Settings.Default.advSimilarity = (bool)Similarity_Toggle.IsChecked;
            Settings.Default.advSplitIndex = (bool)Split_Index_Toggle.IsChecked;
            Settings.Default.advSplitState = (bool)Split_State_Toggle.IsChecked;
        }
    }
}

using System.Windows;

namespace Hobsplit
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Settings_Control.Save();
            Keybinds_Control.Save();
            MainWindow.instance.SetWindowsOnTop();
            Settings.Default.Save();
            Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

using System.Windows;
using System.Drawing;

namespace HobbitAutosplitter
{
    public partial class LivefeedCropWindow : Window
    {
        public LivefeedCropWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

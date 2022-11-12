using System.Windows;

namespace HobbitAutosplitter
{
    public partial class VideoCropWorkshopWindow : Window
    {
        public VideoCropWorkshopWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SplitManager.UpdateSplitsFinalCrop();
            CaptureManager.UpdatePreviewCrop();
            Settings.Default.Save();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

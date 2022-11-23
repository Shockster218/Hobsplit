using System.Windows;

namespace Hobsplit
{
    public partial class ImageRegionWindow : Window
    {
        private bool gameZoom = false;
        private bool compZoom = false;
        public ImageRegionWindow()
        {
            InitializeComponent();
        }

        private void Set_Image_Region_Window_ContentRendered(object sender, System.EventArgs e)
        {
            Game_Image_Control.IsSource(true);
            Comparison_Image_Control.IsSource(false);
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            Game_Image_Control.Save();
            Comparison_Image_Control.Save();
            SplitManager.UpdateSplitsImageWorkable();
            Settings.Default.Save();
            Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Game_Image_Zoom_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!gameZoom)
            {
                Game_Image_Control.ZoomIn();
                Game_Image_Zoom_Button.Content = "Zoom Out";
            }
            else
            {
                Game_Image_Control.ZoomOut();
                Game_Image_Zoom_Button.Content = "Zoom In";
            }

            gameZoom = !gameZoom;
        }

        private void Comparison_Image_Zoom_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!compZoom)
            {
                Comparison_Image_Control.ZoomIn();
                Comparison_Image_Zoom_Button.Content = "Zoom Out";
            }
            else
            {
                Comparison_Image_Control.ZoomOut();
                Comparison_Image_Zoom_Button.Content = "Zoom In";
            }

            compZoom = !compZoom;
        }
    }
}

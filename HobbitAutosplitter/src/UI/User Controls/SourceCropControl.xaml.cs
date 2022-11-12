using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace HobbitAutosplitter
{
    public partial class SourceCropControl : UserControl
    {
        private Bitmap frame;
        public SourceCropControl()
        {
            InitializeComponent();
        }

        private void SetGameplayImage()
        {
            frame = CaptureManager.GetPreviewFrame().Clone() as Bitmap;
            frame = frame.Crop(new Rectangle(
                frame.Width - (int)(Settings.Default.sourceCropRight / 100 * frame.Width),
                frame.Height - (int)(Settings.Default.sourceCropBottom / 100 * frame.Height),
                (int)Settings.Default.sourceCropLeft / 100 * frame.Width,
                (int)Settings.Default.sourceCropTop / 100 * frame.Height
            ));

            Gameplay_Image.Source = frame.ToBitmapImage();
            frame.Dispose();
        }

        private void SaveSourceCrop(object sender, RoutedEventArgs e)
        {
            Settings.Default.sourceCropLeft = CropControl.valueLeft;
            Settings.Default.sourceCropTop = CropControl.valueTop;
            Settings.Default.sourceCropRight = CropControl.valueRight;
            Settings.Default.sourceCropBottom = CropControl.valueBottom;
            CaptureManager.UpdatePreviewCrop();
            frame.Dispose();
        }
    }
}

using System.Windows;
using System.Drawing;

namespace HobbitAutosplitter
{
    public partial class LivefeedCropWindow : Window
    {
        private Bitmap frame;
        public LivefeedCropWindow()
        {
            InitializeComponent();
            //CropControl.OnValueChanged += SetGameplayImage;
        }

        private void SetGameplayImage(CropArgs args)
        {
            frame = CaptureManager.GetCurrentFrameData();
            Bitmap crop = (Bitmap)frame.Clone();
            crop.Crop(new Rectangle(
                (int)args.left / 100 * frame.Width,
                (int)args.top / 100 * frame.Height,
                frame.Width - (int)(args.right / 100 * frame.Width),
                frame.Height - (int)(args.bottom / 100 * frame.Height)
                ));
            Gameplay_Image.Source = crop.ToBitmapImage();
            crop.Dispose();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.cropLeft = CropControl.valueLeft;
            Settings.Default.cropTop = CropControl.valueTop;
            Settings.Default.cropRight = CropControl.valueRight;
            Settings.Default.cropBottom = CropControl.valueBottom;
            CaptureManager.UpdatePreviewCrop();
            frame.Dispose();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            frame.Dispose();
            Close();
        }
    }
}

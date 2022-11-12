using System.Windows.Controls;
using System.Windows;


namespace HobbitAutosplitter
{
    public partial class ComparisonCropControl : UserControl
    {
        private SplitData reference = null;
        public ComparisonCropControl()
        {
            InitializeComponent();
            reference = SplitManager.GetCurrentComparison();
            CropControl.UpdateImage += UpdateReference;
            Window.GetWindow(this).Closing += (s, e) => Close();
        }

        private void UpdateReference()
        {
            if (null == reference) return;
            UpdateCropSettings();
            reference.UpdateImgWorkableCrop();
            Split_Reference_Image.Source = reference.GetImage().ToBitmapImage();
        }

        private void UpdateCropSettings()
        {
            Settings.Default.compCropLeft = CropControl.valueLeft;
            Settings.Default.compCropTop = CropControl.valueTop;
            Settings.Default.compCropRight = CropControl.valueRight;
            Settings.Default.compCropLeft = CropControl.valueBottom;
        }

        private void Close()
        {
            reference.Dispose();
        }
    }
}

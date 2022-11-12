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
        }

        private void HandleValueChanged()
        {
            if (null == reference) return;
            reference.UpdateImgWorkableCrop();
            Split_Reference_Image.Source = reference.GetImage().ToBitmapImage();
        }

        private void SaveComparisonCrop(object sender, RoutedEventArgs e)
        {
            Settings.Default.compCropLeft = CropControl.valueLeft;
            Settings.Default.compCropTop = CropControl.valueTop;
            Settings.Default.compCropRight = CropControl.valueRight;
            Settings.Default.compCropLeft = CropControl.valueBottom;
            SplitManager.UpdateSplitsFinalCrop();
            reference.Dispose();
        }
    }
}

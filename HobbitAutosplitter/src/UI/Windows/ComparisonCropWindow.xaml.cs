using System.Windows;
using System;

namespace HobbitAutosplitter
{
    public partial class ComparisonCropWindow : Window
    {
        private SplitData reference = null;
        public ComparisonCropWindow()
        {
            InitializeComponent();
            //CropControl.OnValueChanged += HandleValueChanged;
        }

        private void HandleValueChanged(CropArgs args)
        {
            if (null == reference) return;
            reference.UpdateImageCropping(args.left, args.right, args.top, args.bottom);
            Split_Reference_Image.Source = reference.GetImage().ToBitmapImage();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.refCropLeft = CropControl.valueLeft;
            Settings.Default.refCropTop = CropControl.valueTop;
            Settings.Default.refCropRight = CropControl.valueRight;
            Settings.Default.refCropBottom = CropControl.valueBottom;
            SplitManager.UpdateSplitCroppings();
            reference.Dispose();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            reference.Dispose();
            Close();
        }
    }
}

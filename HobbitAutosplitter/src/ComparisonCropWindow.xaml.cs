using System.Windows;

namespace HobbitAutosplitter
{
    public partial class ComparisonCropWindow : Window
    {
        private double value = 0;
        private SplitData reference = null;
        private bool settingCropValue = false;
        private bool settingSliderValue = false;
        public ComparisonCropWindow()
        {
            InitializeComponent();
            reference = SplitManager.GetCurrentComparison();
            splitReference.Source = reference.GetImage().ToBitmapImage();
            value = Settings.Default.referenceCropPercentage;
            referenceCrop.Value = value;
            referenceSlider.Value = value;
        }

        private void referenceCrop_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(1, 100);
                        if (_value <= 0) _value = 0.1;

                        referenceSlider.Value = _value;
                        value = _value;
                        HandleValueChanged();
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void referenceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    referenceCrop.Value = _value;
                    value = _value;
                    HandleValueChanged();
                }
                finally { settingSliderValue = false; }
            }
        }

        private void HandleValueChanged()
        {
            if (null == reference) return;
            reference.UpdateImageCropping(value);
            splitReference.Source = reference.GetImage().ToBitmapImage();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.referenceCropPercentage = value;
            SplitManager.UpdateSplitCroppings(value);
            App.Current.Dispatcher.Invoke(() => 
            {
                MainWindow.instance.ChangeComparisonReference();
            });
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

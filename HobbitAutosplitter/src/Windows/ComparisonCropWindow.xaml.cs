using System.Windows;

namespace HobbitAutosplitter
{
    public partial class ComparisonCropWindow : Window
    {
        private double valueLeft = 0;
        private double valueRight = 0;
        private SplitData reference = null;
        private bool settingCropValue = false;
        private bool settingSliderValue = false;
        public ComparisonCropWindow()
        {
            InitializeComponent();
            reference = SplitManager.GetCurrentComparison();
            Split_Reference_Image.Source = reference.GetImage().ToBitmapImage();
            valueLeft = Settings.Default.referenceCropPercentageLeft;
            valueRight = Settings.Default.referenceCropPercentageRight;
            Crop_Left_UpDown.Value = valueLeft;
            Crop_Left_Slider.Value = valueLeft;
            Crop_Right_UpDown.Value = valueRight;
            Crop_Right_Slider.Value = valueRight;
        }

        private void referenceCropLeft_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
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

                        Crop_Left_Slider.Value = _value;
                        valueLeft = _value;
                        HandleValueChanged();
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void referenceSliderLeft_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    Crop_Left_Slider.Value = _value;
                    valueLeft = _value;
                    HandleValueChanged();
                }
                finally { settingSliderValue = false; }
            }
        }

        private void referenceCropRight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
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

                        Crop_Right_Slider.Value = _value;
                        valueRight = _value;
                        HandleValueChanged();
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void referenceSliderRight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    Crop_Right_Slider.Value = _value;
                    valueRight = _value;
                    HandleValueChanged();
                }
                finally { settingSliderValue = false; }
            }
        }

        private void HandleValueChanged()
        {
            if (null == reference) return;
            reference.UpdateImageCropping(valueLeft, valueRight);
            Split_Reference_Image.Source = reference.GetImage().ToBitmapImage();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.referenceCropPercentageLeft = valueLeft;
            Settings.Default.referenceCropPercentageRight = valueRight;
            SplitManager.UpdateSplitCroppings(valueLeft, valueRight);
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

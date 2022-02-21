using System.Windows;

namespace HobbitAutosplitter
{
    public partial class ComparisonCropWindow : Window
    {
        private double valueLeft = 0;
        private double valueRight = 0;
        private double valueTop = 0;
        private double valueBottom = 0;
        private SplitData reference = null;
        private bool settingCropValue = false;
        private bool settingSliderValue = false;
        public ComparisonCropWindow()
        {
            InitializeComponent();
            SetInitialUIValues();
        }

        private void Crop_Left_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(51, 100);
                        Crop_Left_Slider.Value = _value;
                        valueLeft = _value;
                        HandleValueChanged();
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void Crop_Left_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
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

        private void Crop_Top_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(51, 100);
                        Crop_Top_Slider.Value = _value;
                        valueTop = _value;
                        HandleValueChanged();
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void Crop_Top_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    Crop_Top_Slider.Value = _value;
                    valueTop = _value;
                    HandleValueChanged();
                }
                finally { settingSliderValue = false; }
            }
        }

        private void Crop_Right_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(51, 100);
                        Crop_Right_Slider.Value = _value;
                        valueRight = _value;
                        HandleValueChanged();
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void Crop_Right_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
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

        private void Crop_Bottom_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(51, 100);
                        Crop_Bottom_Slider.Value = _value;
                        valueBottom = _value;
                        HandleValueChanged();
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void Crop_Bottom_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    Crop_Bottom_Slider.Value = _value;
                    valueBottom = _value;
                    HandleValueChanged();
                }
                finally { settingSliderValue = false; }
            }
        }

        private void HandleValueChanged()
        {
            if (null == reference) return;
            reference.UpdateImageCropping(valueLeft, valueRight, valueTop, valueBottom);
            Split_Reference_Image.Source = reference.GetImage().ToBitmapImage();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.refCropLeft = valueLeft;
            Settings.Default.refCropTop = valueTop;
            Settings.Default.refCropRight = valueRight;
            Settings.Default.refCropBottom = valueBottom;
            SplitManager.UpdateSplitCroppings(valueLeft, valueRight, valueTop, valueBottom);
            reference.Dispose();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            reference.Dispose();
            Close();
        }

        private void SetInitialUIValues()
        {
            reference = SplitManager.GetCurrentComparison();
            Split_Reference_Image.Source = reference.GetImage().ToBitmapImage();
            valueLeft = Settings.Default.refCropLeft;
            valueRight = Settings.Default.refCropRight;
            valueTop = Settings.Default.refCropTop;
            valueBottom = Settings.Default.refCropBottom;
            Crop_Left_UpDown.Value = valueLeft;
            Crop_Left_Slider.Value = valueLeft;
            Crop_Right_UpDown.Value = valueRight;
            Crop_Right_Slider.Value = valueRight;
            Crop_Top_UpDown.Value = valueTop;
            Crop_Top_Slider.Value = valueTop;
            Crop_Bottom_UpDown.Value = valueBottom;
            Crop_Bottom_Slider.Value = valueBottom;
        }
    }
}

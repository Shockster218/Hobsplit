using System.Windows;
using System.Drawing;

namespace HobbitAutosplitter
{
    public partial class LivefeedCropWindow : Window
    {
        private double valueLeft = 0;
        private double valueRight = 0;
        private double valueTop = 0;
        private double valueBottom = 0;
        private bool settingCropValue = false;
        private bool settingSliderValue = false;
        private Bitmap frame;
        public LivefeedCropWindow()
        {
            InitializeComponent();
            SetGameplayImage();
            SetInitialUIValues();
        }

        private void SetGameplayImage()
        {
            frame = CaptureManager.GetCurrentFrame();
            Bitmap crop = (Bitmap)frame.Clone();
            crop.Crop(new RECT(
                (int)valueLeft / 100 * frame.Width,
                (int)valueTop / 100 * frame.Height,
                frame.Width - (int)(valueRight / 100 * frame.Width),
                frame.Height - (int)(valueBottom / 100 * frame.Height)
                ));
            Gameplay_Image.Source = crop.ToBitmapImage();
            crop.Dispose();
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
                        SetGameplayImage();
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
                    Crop_Left_UpDown.Value = _value;
                    valueLeft = _value;
                    SetGameplayImage();
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
                        SetGameplayImage();
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
                    Crop_Top_UpDown.Value = _value;
                    valueTop = _value;
                    SetGameplayImage();
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
                        SetGameplayImage();
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
                    Crop_Right_UpDown.Value = _value;
                    valueRight = _value;
                    SetGameplayImage();
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
                        SetGameplayImage();
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
                    Crop_Bottom_UpDown.Value = _value;
                    valueBottom = _value;
                    SetGameplayImage();
                }
                finally { settingSliderValue = false; }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.cropLeft = valueLeft;
            Settings.Default.cropTop = valueTop;
            Settings.Default.cropRight = valueRight;
            Settings.Default.cropBottom = valueBottom;
            CaptureManager.SetPreviewCrop(valueLeft, valueRight, valueTop, valueBottom);
            frame.Dispose();
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            frame.Dispose();
            Close();
        }

        private void SetInitialUIValues()
        {
            valueLeft = Settings.Default.cropLeft;
            valueRight = Settings.Default.cropRight;
            valueTop = Settings.Default.cropTop;
            valueBottom = Settings.Default.cropBottom;
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

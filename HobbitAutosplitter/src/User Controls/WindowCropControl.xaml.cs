using System;
using System.Windows;
using System.Windows.Controls;

namespace HobbitAutosplitter
{
    /// <summary>
    /// Interaction logic for WindowCropControl.xaml
    /// </summary>
    public partial class WindowCropControl : UserControl
    {
        public double valueLeft { get; private set; }
        public double valueRight { get; private set; }
        public double valueTop { get; private set; }
        public double valueBottom { get; private set; }
        private bool settingCropValue = false;
        private bool settingSliderValue = false;

        public CropEventHandler OnValueChanged;

        public WindowCropControl()
        {
            InitializeComponent();
        }
        private void SetInitialUIValues(float left, float right, float top, float bottom)
        {
            valueLeft = left;
            valueRight = right;
            valueTop = top;
            valueBottom = bottom;
            Crop_Left_UpDown.Value = valueLeft;
            Crop_Right_UpDown.Value = valueRight;
            Crop_Top_UpDown.Value = valueTop;
            Crop_Bottom_UpDown.Value = valueBottom;
            Crop_Left_Slider.Value = 51;
            Crop_Right_Slider.Value = 51;
            Crop_Top_Slider.Value = 51;
            Crop_Bottom_Slider.Value = 51;
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
                        OnValueChanged.Invoke(new CropArgs((float)valueLeft, (float)valueRight, (float)valueTop, (float)valueBottom));
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
                    OnValueChanged.Invoke(new CropArgs((float)valueLeft, (float)valueRight, (float)valueTop, (float)valueBottom));
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
                        OnValueChanged.Invoke(new CropArgs((float)valueLeft, (float)valueRight, (float)valueTop, (float)valueBottom));
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
                    OnValueChanged.Invoke(new CropArgs((float)valueLeft, (float)valueRight, (float)valueTop, (float)valueBottom));
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
                        OnValueChanged.Invoke(new CropArgs((float)valueLeft, (float)valueRight, (float)valueTop, (float)valueBottom));
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
                    OnValueChanged.Invoke(new CropArgs((float)valueLeft, (float)valueRight, (float)valueTop, (float)valueBottom));
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
                        OnValueChanged.Invoke(new CropArgs((float)valueLeft, (float)valueRight, (float)valueTop, (float)valueBottom));
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
                    OnValueChanged.Invoke(new CropArgs((float)valueLeft, (float)valueRight, (float)valueTop, (float)valueBottom));
                }
                finally { settingSliderValue = false; }
            }
        }
    }
}

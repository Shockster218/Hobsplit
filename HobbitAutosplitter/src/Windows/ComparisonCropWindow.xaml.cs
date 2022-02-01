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
            splitReference.Source = reference.GetImage().ToBitmapImage();
            valueLeft = Settings.Default.referenceCropPercentageLeft;
            valueRight = Settings.Default.referenceCropPercentageRight;
            referenceCropLeft.Value = valueLeft;
            referenceSliderLeft.Value = valueLeft;
            referenceCropRight.Value = valueRight;
            referenceSliderRight.Value = valueRight;
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

                        referenceSliderLeft.Value = _value;
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
                    referenceCropLeft.Value = _value;
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

                        referenceSliderRight.Value = _value;
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
                    referenceCropRight.Value = _value;
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
            splitReference.Source = reference.GetImage().ToBitmapImage();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.referenceCropPercentageLeft = valueLeft;
            Settings.Default.referenceCropPercentageRight = valueRight;
            SplitManager.UpdateSplitCroppings(valueLeft, valueRight);
            App.Current.Dispatcher.Invoke(() => 
            {
                MainWindow.instance.ChangeComparisonReference(LivesplitAction.NONE);
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

using System.Windows;
using System.Windows.Controls;


namespace HobbitAutosplitter
{
    /// <summary>
    /// Interaction logic for ComparisonSettingsWindow.xaml
    /// </summary>
    public partial class ComparisonSettingsWindow : Window
    {
        private double valueReset = 0;
        private double valueStart = 0;
        private double valueLoads = 0;
        private double valueThief = 0;
        private double valueFinal = 0;
        private bool settingCropValue = false;
        private bool settingSliderValue = false;

        public ComparisonSettingsWindow()
        {
            InitializeComponent();
            GetSimilarityValues();
            SetSimilarityValues();
        }

        private void GetSimilarityValues()
        {
            valueReset = Settings.Default.resetSimilarity;
            valueStart = Settings.Default.startSimilarity;
            valueLoads = Settings.Default.loadsSimilarity;
            valueThief = Settings.Default.thiefSimilarity;
            valueFinal = Settings.Default.finalSimilarity;
        }

        private void SetSimilarityValues()
        {
            Reset_Screen_UpDown.Value = valueReset;
            Start_UpDown.Value = valueStart;
            Load_Screens_UpDown.Value = valueLoads;
            Thief_Split_UpDown.Value = valueThief;
            Final_Split_UpDown.Value = valueFinal;
            Reset_Screen_Slider.Minimum = 0.7f;
            Start_Slider.Minimum = 0.7f;
            Load_Screens_Slider.Minimum = 0.7f;
            Thief_Split_Slider.Minimum = 0.7f;
            Final_Split_Slider.Minimum = 0.7f;
        }

        private void Reset_Screen_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(0.7, 1);
                        Reset_Screen_Slider.Value = _value;
                        valueReset = _value;
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void Reset_Screen_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    Reset_Screen_UpDown.Value = _value;
                    valueReset = _value;
                }
                finally { settingSliderValue = false; }
            }
        }

        private void Start_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(0.7, 1);
                        Start_Slider.Value = _value;
                        valueStart = _value;
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void Start_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    Start_UpDown.Value = _value;
                    valueStart = _value;
                }
                finally { settingSliderValue = false; }
            }
        }

        private void Load_Screens_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(0.7, 1);
                        Load_Screens_Slider.Value = _value;
                        valueLoads = _value;
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void Load_Screens_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    Load_Screens_UpDown.Value = _value;
                    valueLoads = _value;
                }
                finally { settingSliderValue = false; }
            }
        }

        private void Thief_Split_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(0.7, 1);
                        Thief_Split_Slider.Value = _value;
                        valueThief = _value;
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void Thief_Split_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    Thief_Split_UpDown.Value = _value;
                    valueThief = _value;
                }
                finally { settingSliderValue = false; }
            }
        }

        private void Final_Split_UpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!settingCropValue)
            {
                settingCropValue = true;
                try
                {
                    double _value;
                    if (null != e.NewValue)
                    {
                        _value = ((double)e.NewValue).Clamp(0.7, 1);
                        Final_Split_Slider.Value = _value;
                        valueFinal = _value;
                    }
                }
                finally { settingCropValue = false; }
            }
        }

        private void Final_Split_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!settingSliderValue)
            {
                settingSliderValue = true;
                try
                {
                    double _value = e.NewValue;
                    Final_Split_UpDown.Value = _value;
                    valueFinal = _value;
                }
                finally { settingSliderValue = false; }
            }
        }

        private void Reset_Screen_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            valueReset = 0.9f;
            Reset_Screen_UpDown.Value = valueReset;
        }

        private void Start_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            valueStart = 0.945f;
            Start_UpDown.Value = valueStart;
        }

        private void Load_Screens_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            valueLoads = 0.935f;
            Load_Screens_UpDown.Value = valueLoads;
        }

        private void Thief_Split_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            valueThief = 0.975f;
            Thief_Split_UpDown.Value = valueThief;
        }

        private void Final_Split_Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            valueFinal = 0.9f;
            Final_Split_UpDown.Value = valueFinal;
        }

        private void Split_Item_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            canvas.Children[1].Visibility = Visibility.Visible;
        }

        private void Split_Item_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            canvas.Children[1].Visibility = Visibility.Hidden;
        }
    }
}

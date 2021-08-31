using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;
using forms = System.Windows.Forms;
using DirectShowLib;

namespace HobbitAutoSplitter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instance;
        private CaptureManager captureManager = new CaptureManager();
        private DispatcherTimer dispatcher = new DispatcherTimer();
        private Stopwatch segmentStopwatch = new Stopwatch();
        private bool drawWBool = false;
        private bool drawHBool = false;
        private bool minWBool = false;
        private bool maxWBool = false;
        private bool minHBool = false;
        private bool maxHBool = false;
        private bool areaBool = false;
        private bool rBool = false;
        private bool gBool = false;
        public MainWindow()
        {
            instance = this;
            InitializeComponent();
        }

        #region Main Window
        private void windowMain_ContentRendered(object sender, EventArgs e)
        {
            SetSliderValues();
            SetKeybinds();
            PopulateInputDevices();
            LoadEndFrame(Settings.Default.endFramePath);
            captureManager.SetupCapture();
        }

        private void CloseWindow(object sender = null, EventArgs e = null)
        {
            Settings.Default.crop = (int)capCropSlider.Value;
            Settings.Default.device = inputCombo.SelectedIndex;
            Settings.Default.Save();
            Application.Current.Shutdown();
        }
        #endregion

        #region Image Display and Cropping
        public void DisplayImage(Bitmap bmImage, string output = "")
        {
            if (!hideVideoCheckbox.IsChecked.GetValueOrDefault())
            {
                imageDisplay.Source = ImageProcessor.ConvertBitmapToBitmapImage(bmImage);
            }
            else
            {
                imageDisplay.Source = null;
            }

            if (showOutputCheckbox.IsChecked.GetValueOrDefault())
            {
                OutputLabel.Visibility = Visibility.Visible;
                OutputLabel.Content = output;
            }
            else
            {
                if (OutputLabel.Visibility == Visibility.Visible)
                {
                    OutputLabel.Visibility = Visibility.Hidden;
                }
            }
        }

        private void capCropSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            captureManager.SetCropValue((int)capCropSlider.Value);
            if (captureManager.GetEndFrame(true) != null) DisplayEndFrameImage();
        }

        private void setEndFrameButton_Click(object sender, RoutedEventArgs e)
        {
            forms.OpenFileDialog dialog = new forms.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (dialog.ShowDialog() == forms.DialogResult.OK)
            {
                LoadEndFrame(dialog.FileName);
            }
        }

        private void LoadEndFrame(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (File.Exists(fileName))
                {
                    using (Stream bmpStream = File.Open(fileName, FileMode.Open))
                    {
                        captureManager.SetEndFrame(Image.FromStream(bmpStream));
                    }

                    Settings.Default.endFramePath = fileName;
                    DisplayEndFrameImage();
                }
            }
        }

        private void DisplayEndFrameImage()
        {
            endFrameDisplay.Source = ImageProcessor.ConvertBitmapToBitmapImage(captureManager.GetEndFrame(true));
        }
        #endregion

        #region Keybinds
        private void SetKeybinds()
        {
            try
            {
                splitButton.Content = Settings.Default.split.ToString() == "NONAME" ? "Set Key..." : Settings.Default.split.ToString();
                pauseButton.Content = Settings.Default.pause.ToString() == "NONAME" ? "Set Key..." : Settings.Default.pause.ToString();
                resetButton.Content = Settings.Default.reset.ToString() == "NONAME" ? "Set Key..." : Settings.Default.reset.ToString();
                unsplitButton.Content = Settings.Default.unsplit.ToString() == "NONAME" ? "Set Key..." : Settings.Default.unsplit.ToString();
            }
            catch
            {
                MessageBox.Show("Could not load keybinds.");
            }
        }

        private void splitButton_Click(object sender, RoutedEventArgs e)
        {
            splitButton.Content = "Waiting...";
        }

        private void unsplitButton_Click(object sender, RoutedEventArgs e)
        {
            unsplitButton.Content = "Waiting...";
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            pauseButton.Content = "Waiting...";
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            resetButton.Content = "Waiting...";
        }

        private void splitButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (splitButton.IsFocused)
            {
                Settings.Default.split = (WindowsInput.Native.VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                splitButton.Content = e.Key.ToString();
            }
        }

        private void pauseButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (pauseButton.IsFocused)
            {
                Settings.Default.pause = (WindowsInput.Native.VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                pauseButton.Content = e.Key.ToString();
            }
        }

        private void resetButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (resetButton.IsFocused)
            {
                Settings.Default.reset = (WindowsInput.Native.VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                resetButton.Content = e.Key.ToString();
            }
        }

        private void unsplitButton_KeyDown(object sender, KeyEventArgs e)
        {
            if (unsplitButton.IsFocused)
            {
                Settings.Default.unsplit = (WindowsInput.Native.VirtualKeyCode)KeyInterop.VirtualKeyFromKey(e.Key);
                unsplitButton.Content = e.Key.ToString();
            }
        }

        private void grid_Focus(object sender, MouseEventArgs e)
        {
            Keyboard.ClearFocus();
            splitButton.Content = Settings.Default.split;
            pauseButton.Content = Settings.Default.pause;
            resetButton.Content = Settings.Default.reset;
            unsplitButton.Content = Settings.Default.unsplit;
        }
        #endregion

        #region Text Detection Sliders
        private void SetSliderValues()
        {
            drawW_ValueChanged(null, null);
            drawH_ValueChanged(null, null);
            minW_ValueChanged(null, null);
            maxW_ValueChanged(null, null);
            minH_ValueChanged(null, null);
            maxH_ValueChanged(null, null);
            area_ValueChanged(null, null);
            r_ValueChanged(null, null);
            g_ValueChanged(null, null);
            capCropSlider.Value = Settings.Default.crop;
        }

        private void drawW_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (drawWBool)
            {
                if (e != null)
                {
                    drawW.Value = Math.Round(e.NewValue);
                    Detector.drawW = (int)drawW.Value;
                }
            }
            else
            {
                if (drawW.Value != Detector.drawW)
                {
                    drawW.Value = Detector.drawW;
                }
                else
                {
                    drawWBool = true;
                }
            }
            drawWCount.Content = drawW.Value.ToString();
        }

        private void drawH_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (drawHBool)
            {
                if (e != null)
                {
                    drawH.Value = Math.Round(e.NewValue);
                    Detector.drawH = (int)drawH.Value;
                }
            }
            else
            {
                if (drawH.Value != Detector.drawH)
                {
                    drawH.Value = Detector.drawH;
                }
                else
                {
                    drawHBool = true;
                }
            }
            drawHCount.Content = drawH.Value.ToString();
        }

        private void minW_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (minWBool)
            {
                if (e != null)
                {
                    minW.Value = Math.Round(e.NewValue);
                    Detector.minW = (int)minW.Value;
                }
            }
            else
            {
                if (minW.Value != Detector.minW)
                {
                    minW.Value = Detector.minW;
                }
                else
                {
                    minWBool = true;
                }
            }
            minWidthCount.Content = minW.Value.ToString();
        }

        private void maxW_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (maxWBool)
            {
                if (e != null)
                {
                    maxW.Value = Math.Round(e.NewValue);
                    Detector.maxW = (int)maxW.Value;
                }
            }
            else
            {
                if (maxW.Value != Detector.maxW)
                {
                    maxW.Value = Detector.maxW;
                }
                else
                {
                    maxWBool = true;
                }
            }
            maxWidthCount.Content = maxW.Value.ToString();
        }

        private void minH_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (minHBool)
            {
                if (e != null)
                {
                    minH.Value = Math.Round(e.NewValue);
                    Detector.minH = (int)minH.Value;
                }
            }
            else
            {
                if (minH.Value != Detector.minH)
                {
                    minH.Value = Detector.minH;
                }
                else
                {
                    minHBool = true;
                }
            }
            minHeightCount.Content = minH.Value.ToString();
        }

        private void maxH_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (maxHBool)
            {
                if (e != null)
                {
                    maxH.Value = Math.Round(e.NewValue);
                    Detector.maxH = (int)maxH.Value;
                }
            }
            else
            {
                if (maxH.Value != Detector.maxH)
                {
                    maxH.Value = Detector.maxH;
                }
                else
                {
                    maxHBool = true;
                }
            }
            maxHeightCount.Content = maxH.Value.ToString();
        }

        private void area_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (areaBool)
            {
                if (e != null)
                {
                    area.Value = Math.Round(e.NewValue);
                    Detector.area = (int)area.Value;
                }
            }
            else
            {
                if (area.Value != Detector.area)
                {
                    area.Value = Detector.area;
                }
                else
                {
                    areaBool = true;
                }
            }
            areaCount.Content = area.Value.ToString();
        }
        private void r_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (rBool)
            {
                if (e != null)
                {
                    r.Value = Math.Round(e.NewValue);
                    Detector.r = (int)r.Value;
                }
            }
            else
            {
                if (r.Value != Detector.r)
                {
                    r.Value = Detector.r;
                }
                else
                {
                    rBool = true;
                }
            }
            rCount.Content = r.Value.ToString();
        }

        private void g_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (gBool)
            {
                if (e != null)
                {
                    g.Value = Math.Round(e.NewValue);
                    Detector.g = (int)g.Value;
                }
            }
            else
            {
                if (g.Value != Detector.g)
                {
                    g.Value = Detector.g;
                }
                else
                {
                    gBool = true;
                }
            }
            gCount.Content = g.Value.ToString();
        }
        #endregion

        #region Input Device
        private void PopulateInputDevices()
        {
            foreach (DsDevice device in DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice))
            {
                inputCombo.Items.Add(device.Name);
            }

            inputCombo.SelectedIndex = Settings.Default.device;
        }

        private void inputCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            captureManager.SetupCapture();
        }
        #endregion

        #region Segment Timer
        public void InitSegmentTimer()
        {
            dispatcher.Interval = new TimeSpan(0, 0, 0, 0, 10);
            dispatcher.Tick += (sender, args) =>
            {
                TimeSpan elapsed = TimeSpan.FromMilliseconds(segmentStopwatch.ElapsedMilliseconds);
                string result = string.Empty;
                if (elapsed.TotalMinutes < 1)
                {
                    if (elapsed.TotalSeconds > 10)
                    {
                        result = elapsed.ToString(@"ss\.ff");
                    }
                    else
                    {
                        result = elapsed.ToString(@"s\.ff");
                    }
                }
                else if (elapsed.TotalHours < 1)
                {
                    if (elapsed.TotalMinutes > 10)
                    {
                        result = elapsed.ToString(@"mm\:ss\.ff");
                    }
                    else
                    {
                        result = elapsed.ToString(@"m\:ss\.ff");
                    }
                }
                else
                {
                    if (elapsed.TotalHours > 10)
                    {
                        result = elapsed.ToString(@"hh\:mm\:ss\.ff");
                    }
                    else
                    {
                        result = elapsed.ToString(@"h\:mm\:ss\.ff");
                    }
                }
                segmentTimer.Text = result;
            };

            dispatcher.Start();
            segmentStopwatch.Start();
        }

        public void ResetSegmentTimer()
        {
            dispatcher.Stop();
            segmentStopwatch.Reset();
            segmentTimer.Text = "0.00";
        }
        #endregion

        #region Level and Status Text
        public void SetLevelText(int level)
        {
            if (level == -1) levelLab.Content = "Main menu";
            else levelLab.Content = Constants.levels[level];
        }

        public void SetStatus(States state)
        {
            switch (state)
            {
                case States.READYTOSTART:
                    statusLab.Content = "Ready to Start!";
                    statusLab.Foreground = new SolidColorBrush(Colors.Purple);
                    break;
                case States.STARTED:
                    statusLab.Content = "Started!";
                    statusLab.Foreground = new SolidColorBrush(Colors.Thistle);
                    break;
                case States.GAMEPLAY:
                    statusLab.Content = "Gameplay";
                    statusLab.Foreground = new SolidColorBrush(Colors.CornflowerBlue);
                    break;
                case States.LOADING:
                    statusLab.Content = "Loading...";
                    statusLab.Foreground = new SolidColorBrush(Colors.Salmon);
                    break;
                case States.FINISHED:
                    statusLab.Content = "Finished!";
                    statusLab.Foreground = new SolidColorBrush(Colors.LightGreen);
                    break;
            }
        }
        #endregion
    }
}

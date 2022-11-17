using Point = System.Windows.Point;
using System.Windows.Input;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using System.Windows;

namespace HobbitAutosplitter
{
    // Left = Left Margin
    // Top = Top Margin
    // Right = Grid Width + Left Margin
    // Bottom = Grid Height + Top Margin
    // Width = Grid Width
    // Height = Grid Height

    public partial class ImageCropControl : UserControl
    {
        private Point lastMousePosition;
        private bool wasPressed;
        private Rectangle gridRect;
        private bool isSource;
        private SolidColorBrush dragBrush;
        private SolidColorBrush notDragBrush;
        private Thickness gridMargin;
        private byte[] originalImage;
        private int minMarginDistance = 50;

        public ImageCropControl()
        {
            InitializeComponent();
            lastMousePosition = new Point(0,0);
            dragBrush = new SolidColorBrush(Color.FromArgb(35, 66, 255, 0));
            notDragBrush = new SolidColorBrush(Color.FromArgb(0, 66, 255, 0));
            gridMargin = new Thickness(0);
        }

        public void IsSource(bool isSource)
        {
            this.isSource = isSource;
            if (isSource)
            {
                gridRect = Settings.Default.sourceRect;
                originalImage = CaptureManager.GetFrameData();
            }
            else
            {
                gridRect = Settings.Default.comparisonRect;
                originalImage = SplitManager.GetSplitDataArray()[0].GetImageOriginal().ToByteArray();
            }

            gridRect.Multiply();
            Image.Source = originalImage.ToBitmapImage();
            SetGridDimensions(gridRect);
        }

        public void ZoomIn()
        {
            Bitmap clone = originalImage.ToBitmap();
            clone = clone.Crop(gridRect, 640, 480);
            Image.Source = clone.ToBitmapImage();
            Crop_Grid.Visibility = Visibility.Collapsed;
            clone.Dispose();
        }

        public void ZoomOut() 
        {
            Image.Source = originalImage.ToBitmapImage();
            Crop_Grid.Visibility = Visibility.Visible;
        }

        public void SetGridDimensions(Rectangle rect)
        {
            gridMargin.Left = rect.X;
            gridMargin.Top = rect.Y;
            gridMargin.Right= rect.Width;
            gridMargin.Bottom = rect.Height;
            Crop_Grid.Margin = gridMargin;
        }

        public void Save()
        {
            gridRect.Divide();
            ZoomOut();
            if (isSource) Settings.Default.sourceRect = gridRect;
            else Settings.Default.comparisonRect = gridRect;
        }

        #region MouseEvents
        private void LeftMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                wasPressed = true;
                Crop_Grid.Background = dragBrush;
                if (lastMousePosition.IsZero())
                {
                    lastMousePosition = e.GetPosition(this);
                    return;
                }
                else
                {
                    double distance = (e.GetPosition(this).X - lastMousePosition.X);
                    lastMousePosition = e.GetPosition(this);
                    if (distance > 0 && Crop_Grid.ActualWidth <= minMarginDistance) return;
                    gridMargin.Left += distance;
                    gridMargin.Left = gridMargin.Left < 0 ? 0 : gridMargin.Left;
                    Crop_Grid.Margin = gridMargin;
                }
            }
            else if(wasPressed)
            {
                Crop_Grid.Background = notDragBrush;
                gridRect.X = (int)gridMargin.Left;
                lastMousePosition.X = 0;
                lastMousePosition.Y = 0;
                wasPressed = false;
            }
        }

        private void RightMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                wasPressed = true;
                Crop_Grid.Background = dragBrush;
                if (lastMousePosition.IsZero())
                {
                    lastMousePosition = e.GetPosition(this);
                    return;
                }
                else
                {
                    double distance = (e.GetPosition(this).X - lastMousePosition.X);
                    lastMousePosition = e.GetPosition(this);
                    if (distance < 0 && Crop_Grid.ActualWidth <= minMarginDistance) return;
                    gridMargin.Right -= distance;
                    gridMargin.Right = gridMargin.Right < 0 ? 0 : gridMargin.Right;
                    Crop_Grid.Margin = gridMargin;
                }
            }
            else if (wasPressed)
            {
                Crop_Grid.Background = notDragBrush;
                gridRect.Width = (int)gridMargin.Right;
                lastMousePosition.X = 0;
                lastMousePosition.Y = 0;
                wasPressed = false;
            }
        }

        private void TopMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                wasPressed = true;
                Crop_Grid.Background = dragBrush;
                if (lastMousePosition.IsZero())
                {
                    lastMousePosition = e.GetPosition(this);
                    return;
                }
                else
                {
                    double distance = (e.GetPosition(this).Y - lastMousePosition.Y);
                    lastMousePosition = e.GetPosition(this);
                    if (distance > 0 && Crop_Grid.ActualHeight <= minMarginDistance) return;
                    gridMargin.Top += distance;
                    gridMargin.Top = gridMargin.Top < 0 ? 0 : gridMargin.Top;
                    Crop_Grid.Margin = gridMargin;
                }
            }
            else if (wasPressed)
            {
                Crop_Grid.Background = notDragBrush;
                gridRect.Y = (int)gridMargin.Top;
                lastMousePosition.X = 0;
                lastMousePosition.Y = 0;
                wasPressed = false;
            }
        }

        private void BottomMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                wasPressed = true;
                Crop_Grid.Background = dragBrush;
                if (lastMousePosition.IsZero())
                {
                    lastMousePosition = e.GetPosition(this);
                    return;
                }
                else
                {
                    double distance = (e.GetPosition(this).Y - lastMousePosition.Y);
                    lastMousePosition = e.GetPosition(this);
                    if (distance < 0 && Crop_Grid.ActualHeight <= minMarginDistance) return;
                    gridMargin.Bottom -= distance;
                    gridMargin.Bottom = gridMargin.Bottom < 0 ? 0 : gridMargin.Bottom;
                    Crop_Grid.Margin = gridMargin;
                }
            }
            else if (wasPressed)
            {
                Crop_Grid.Background = notDragBrush;
                gridRect.Height = (int)gridMargin.Bottom;
                lastMousePosition.X = 0;
                lastMousePosition.Y = 0;
                wasPressed = false;
            }
        }
        #endregion
    }
}

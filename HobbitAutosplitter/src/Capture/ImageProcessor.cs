using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Media.Imaging;

namespace HobbitAutosplitter
{
    public static class ImageProcessor
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            BitmapImage image = new BitmapImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
        
            image.Freeze();
            return image;
        }

        public static Bitmap Crop(this Bitmap source, RECT crop)
        {
            Bitmap cropped = new Bitmap(crop.Right, crop.Bottom);
            using (Graphics graphics = Graphics.FromImage(cropped))
            {
                graphics.DrawImage(source, new Rectangle(0, 0, crop.Right, crop.Bottom), crop, GraphicsUnit.Pixel);
                graphics.Dispose();
            }

            return cropped;
        }

        public static Bitmap Resize(this Bitmap source)
        {
            Rectangle resizeRect = new Rectangle(0, 0, Constants.width, Constants.height);
            Bitmap resize = new Bitmap(Constants.width, Constants.height);

            resize.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(resize))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(source, resizeRect, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
                graphics.Dispose();
            }

            return resize;
        }
    }
}

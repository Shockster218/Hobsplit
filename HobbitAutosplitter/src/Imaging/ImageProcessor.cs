using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace HobbitAutosplitter
{
    public static class ImageProcessor
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        public static Bitmap CropImageToBitmap(Image source, int width, int height, int xCrop, int yCrop)
        {
            Rectangle crop = new Rectangle
            {
                X = source.Width - (int)Math.Round(xCrop / 100d * source.Width),
                Y = source.Height - (int)Math.Round(yCrop / 100d * source.Height),
                Width = source.Width,
                Height = source.Height
            };

            Bitmap cropped = new Bitmap(crop.Width - crop.X, crop.Height - crop.Y);
            using (Graphics graphics = Graphics.FromImage(cropped))
            {
                graphics.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height), crop, GraphicsUnit.Pixel);
                graphics.Dispose();
            }

            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);

            destImage.SetResolution(cropped.HorizontalResolution, cropped.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(cropped, destRect, 0, 0, cropped.Width, cropped.Height, GraphicsUnit.Pixel, wrapMode);
                }

                graphics.Dispose();
            }

            return destImage;
        }
    }
}

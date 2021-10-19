using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace HobbitAutoSplitter
{
    public static class ImageProcessor
    {
        public static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
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

        public static Bitmap CropImageToBitmap(Image source, int width, int height, int cropValue)
        {
            Rectangle crop = new Rectangle
            {
                X = source.Width - (int)Math.Round(cropValue / 100d * source.Width),
                Y = 0,
                Width = source.Width,
                Height = source.Height
            };

            Bitmap cropped = new Bitmap(crop.Width - crop.X, crop.Height);
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

        public static Bitmap EndFrameCrop(Bitmap img)
        {
            Bitmap resize = new Bitmap(img, new Size(640, 480));
            Rectangle crop = new Rectangle
            {
                X = resize.Width - (int)Math.Round(50 / 100d * resize.Width),
                Y = resize.Height - (int)Math.Round(50 / 100d * resize.Height),
                Width = (int)Math.Round(50 / 100d * resize.Width),
                Height = (int)Math.Round(50 / 100d * resize.Height)
            };
            Bitmap dest = new Bitmap(resize.Width - crop.X, resize.Height - crop.Y);
            using (Graphics graphics = Graphics.FromImage(dest))
            {
                graphics.DrawImage(resize, new Rectangle(0, 0, resize.Width, resize.Height), crop, GraphicsUnit.Pixel);
                graphics.Dispose();
            }

            return dest;
        }
    }
}

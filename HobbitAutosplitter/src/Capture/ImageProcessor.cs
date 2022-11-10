using System.Runtime.InteropServices;
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
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();

                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
        public static BitmapImage ToBitmapImage(this byte[] array)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(array);
            image.EndInit();
            return image;
        }

        public static byte[] ToByteArray(this Bitmap bm)
        {
            using (var stream = new MemoryStream())
            {
                bm.Save(stream, ImageFormat.Bmp);
                return stream.ToArray();
            }
        }

        public static Bitmap ToBitmap(this byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return new Bitmap(ms);
            }
        }

        public static Bitmap Crop(this Bitmap source, Rectangle rect)
        {
            Bitmap cropped = new Bitmap(rect.Width, rect.Height);
            using (Graphics graphics = Graphics.FromImage(cropped))
            {
                graphics.DrawImage(source, new Rectangle(0, 0, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);
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

        public static Bitmap RemoveColor(this Bitmap bitmap)
        {
            const float limit = 0.03f;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color c = bitmap.GetPixel(i, j);
                    if (c.GetBrightness() > limit)
                    {
                        bitmap.SetPixel(i, j, Color.Transparent);
                    }
                }
            }
            return bitmap;
        }
    }
}

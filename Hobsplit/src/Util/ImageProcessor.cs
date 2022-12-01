using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Media.Imaging;

namespace Hobsplit
{
    public static class ImageProcessor
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
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

        public static Bitmap Crop(this Bitmap source, Rectangle rect, int width = Constants.comparisonWidth, int height = Constants.comparisonHeight)
        {
            Rectangle temp = new Rectangle(rect.X, rect.Y, width - rect.Right, height - rect.Bottom);
            using (Bitmap target = new Bitmap(source.Width, source.Height))
            {
                using (Graphics graphics = Graphics.FromImage(target))
                {
                    graphics.DrawImage(source, new Rectangle(0, 0, target.Width, target.Height), temp, GraphicsUnit.Pixel);
                    return target.Clone() as Bitmap;
                }
            }
        }

        public static Bitmap Resize(this Bitmap source, int width = Constants.comparisonWidth, int height = Constants.comparisonHeight)
        {
            Rectangle resizeRect = new Rectangle(0, 0, width, height);
            Bitmap resize = new Bitmap(width, height);

            resize.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(resize))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.Low;
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                graphics.DrawImage(source, resizeRect, 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
                graphics.Dispose();
                return resize;
            }
        }

        public static Bitmap MakeGrayscale3(this Bitmap bitmap)
        {
            const float limit = 0.2f;
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

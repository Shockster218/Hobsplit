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

            image.Freeze();

            return image;
        }

        public static Bitmap CropImageToBitmap(Image source, RECT crop)
        {
            Bitmap cropped = new Bitmap(crop.Right, crop.Bottom);
            using (Graphics graphics = Graphics.FromImage(cropped))
            {
                graphics.DrawImage(source, new Rectangle(0, 0, crop.Right, crop.Bottom), crop, GraphicsUnit.Pixel);
                graphics.Dispose();
            }

            Rectangle destRect = new Rectangle(0, 0, Constants.width, Constants.height);
            Bitmap destImage = new Bitmap(Constants.width, Constants.height);

            destImage.SetResolution(cropped.HorizontalResolution, cropped.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(cropped, destRect, 0, 0, cropped.Width, cropped.Height, GraphicsUnit.Pixel);
                graphics.Dispose();
            }

            return destImage;
        }
    }
}

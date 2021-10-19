using System.Drawing;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace HobbitAutosplitter
{
    public class SplitData
    {
        private string name;
        private Bitmap image;
        private Bitmap cropped;
        private RECT crop;
        private Digest digest;

        public SplitData(string name, string imagePath, RECT? crop = null)
        {
            image = SetImage(imagePath);
            this.name = name;
            this.crop = crop == null ? new RECT(0, 0, image.Width, image.Height) : (RECT)crop;
            cropped = image.Crop(this.crop);
            digest = ImagePhash.ComputeDigest(cropped.ToLuminanceImage());
        }

        public SplitData(SplitData data)
        {
            image = data.GetImage();
            name = data.GetSplitName();
            crop = data.GetCrop();
            cropped = data.GetImageCropped();
            digest = data.GetDigest();
        }
        private Bitmap SetImage(string path) 
        {
            Bitmap bm = new Bitmap(Image.FromFile(path));
            return bm.Crop(new RECT((int)(0.29 * bm.Width), 0, bm.Width, bm.Height)).Resize();
        }

        public Bitmap GetImage() { return image; }

        public Bitmap GetImageCropped() { return cropped; }

        public string GetSplitName() { return name; }

        public RECT GetCrop() { return crop; }

        public Digest GetDigest() { return digest; }
    }
}

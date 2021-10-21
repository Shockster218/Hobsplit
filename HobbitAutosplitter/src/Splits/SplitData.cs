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
        private Digest digest;

        public SplitData(string name, string imagePath, int splitIndex)
        {
            image = SetImage(imagePath);
            this.name = name;
            cropped = image.Crop(Constants.crop);
            digest = ImagePhash.ComputeDigest(cropped.ToLuminanceImage());
        }

        public SplitData(SplitData data)
        {
            image = data.GetImage();
            name = data.GetSplitName();
            cropped = data.GetImageCropped();
            digest = data.GetDigest();
        }
        private Bitmap SetImage(string path) 
        {
            Bitmap bm = new Bitmap(Image.FromFile(path));
            return bm.Crop(new RECT((int)(0.272 * bm.Width), 0, bm.Width, bm.Height)).Resize();
        }

        public Bitmap GetImage() { return image; }

        public Bitmap GetImageCropped() { return cropped; }

        public string GetSplitName() { return name; }

        public Digest GetDigest() { return digest; }
    }
}

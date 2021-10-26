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
        private float similarity;

        public SplitData(string name, string imagePath, bool startCrop = false, bool removeColor = false, float similarity = 0.965f)
        {
            image = SetImage(imagePath);
            this.name = name;
            cropped = image.Crop(startCrop ? Constants.startCrop : Constants.crop);
            if(removeColor) cropped.RemoveColor();
            digest = ImagePhash.ComputeDigest(cropped.ToLuminanceImage());
            this.similarity = similarity;
        }

        private Bitmap SetImage(string path) 
        {
            Bitmap bm = new Bitmap(Image.FromFile(path));
            return bm.Crop(new RECT((int)(0.272 * bm.Width), 0, bm.Width, bm.Height)).Resize();
        }

        public bool IsDigestSimilar(Digest d)
        {
            return ImagePhash.GetCrossCorrelation(digest, d) >= similarity;
        }

        public Bitmap GetImage() { return image; }

        public Bitmap GetImageCropped() { return cropped; }

        public string GetSplitName() { return name; }

        public Digest GetDigest() { return digest; }
    }
}

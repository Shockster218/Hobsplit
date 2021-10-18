using System.Drawing;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace HobbitAutosplitter
{
    public class SplitData
    {
        protected string name;
        protected Bitmap image;
        protected RECT crop;

        public SplitData(string name, string imagePath, RECT? crop = null)
        {
            image = SetImage(imagePath);
            this.name = name;
            this.crop = crop == null ? new RECT(0, 0, image.Width, image.Height) : (RECT)crop;
        }

        public bool IsFrameSimilar(Bitmap incoming)
        {
            Bitmap f1 = GetImageCropped();
            Bitmap f2 = incoming.Crop(crop);

            if (CalculateFrameSimilarity(f1, f2) >= SplitManager.GetUniversalSimilarity()) return true;
            else return false;
        }

        private float CalculateFrameSimilarity(Bitmap f1, Bitmap f2)
        {
            Digest d1 = ImagePhash.ComputeDigest(f1.ToLuminanceImage());
            Digest d2 = ImagePhash.ComputeDigest(f2.ToLuminanceImage());

            return ImagePhash.GetCrossCorrelation(d1, d2);
        }
        private Bitmap SetImage(string path) 
        {
            Bitmap bm = new Bitmap(Image.FromFile(path));
            return bm.Crop(new RECT((int)(0.29 * bm.Width), 0, bm.Width, bm.Height)).Resize();
        }

        public Bitmap GetImage() { return image; }

        public Bitmap GetImageCropped() { return image.Crop(crop); }

        public string GetSplitName() { return name; }
    }
}

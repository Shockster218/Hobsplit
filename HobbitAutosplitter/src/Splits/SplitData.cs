using System.Drawing;

namespace HobbitAutosplitter
{
    public abstract class SplitData
    {
        protected string name;
        protected Bitmap image;
        protected RECT crop;

        protected SplitData(string name, string imagePath)
        {
            this.name = name;
            image = SetImage(imagePath);
        }

        public abstract float GetIncomingFrameSimilarity();
        private Bitmap SetImage(string path) 
        {
            Bitmap bm = new Bitmap(Image.FromFile(path));
            return bm.Crop(new RECT((int)(0.29 * bm.Width), 0, bm.Width, bm.Height));
        }

        public Bitmap GetImage() { return image; }

        public string GetSplitName() { return name; }
    }
}

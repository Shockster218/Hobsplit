using System.Drawing;
using System.IO;

namespace HobbitAutosplitter
{
    public class SplitData
    {
        private string name;
        private Image image;
        private float comparisonSimilarity;

        public SplitData(string name)
        {
            this.name = name;
            image = null;
            comparisonSimilarity = 0.98f;
        }

        public void SetComparisonSimilarity(float comparisonSimilarity) { this.comparisonSimilarity = comparisonSimilarity; }

        public bool SetImagePath(string path)
        {
            if (File.Exists(path))
            {
                image = Image.FromFile(path);
                return true;
            }
            return false;
        }

        public Image GetImage() { return image; }

        public string GetSplitName() { return name; }

        public float GetComparisonSimilarity() { return comparisonSimilarity; }
    }
}

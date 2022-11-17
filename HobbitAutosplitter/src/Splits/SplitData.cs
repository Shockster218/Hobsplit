using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Drawing;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace HobbitAutosplitter
{
    public class SplitData : IDisposable
    {
        private string name;
        private int splitNumber;

        private string imagePath;
        private bool removeColor;

        private Bitmap originalImg;
        private Bitmap originalImgWorkable;

        private Digest digest;
        private float similarity;

        private bool _disposedValue;

        public SplitData(string name, int splitNumber, string imagePath, double similarity, bool removeColor = false)
        {
            this.name = name;
            this.splitNumber = splitNumber;
            this.imagePath = imagePath;
            this.similarity = (float)similarity;
            this.removeColor = removeColor;
        }

        public void UpdateSplitImage()
        {
            originalImg = SetImage(imagePath).Resize(640, 480);
            UpdateImgWorkableCrop();
            if (removeColor) originalImgWorkable.RemoveColor();
            digest = ImagePhash.ComputeDigest(originalImgWorkable.ToLuminanceImage());
        }

        public void UpdateSplitImage(string path)
        {
            imagePath = path;
            originalImg = SetImage(imagePath).Resize(640, 480);
            UpdateImgWorkableCrop();
            if (removeColor) originalImgWorkable.RemoveColor();
            digest = ImagePhash.ComputeDigest(originalImgWorkable.ToLuminanceImage());
        }
        private Bitmap SetImage(string path) => new Bitmap(Image.FromFile(path));

        public int GetSplitNumber() => splitNumber;

        public bool IsDigestSimilar(Digest d) => ImagePhash.GetCrossCorrelation(digest, d) >= similarity;

        public string GetImagePath() => imagePath;

        public Bitmap GetImageOriginal() => originalImg;

        public Bitmap GetImageOriginalWorkable() => originalImgWorkable;

        public string GetSplitName() => name;

        public Digest GetDigest() => digest;

        public void SetSimilarity(float similarity) => this.similarity = similarity;

        public Bitmap GetFinalImage()
        {
            Bitmap final = originalImgWorkable.Clone() as Bitmap;
            final = final.Resize();
            final = final.Crop(Constants.crop);
            return final;
        }

        public void UpdateImgWorkableCrop()
        {
            originalImgWorkable = originalImg.Clone() as Bitmap;
            originalImgWorkable = originalImgWorkable.Crop(Settings.Default.comparisonRect, 640, 480);
        }

        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _safeHandle.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}

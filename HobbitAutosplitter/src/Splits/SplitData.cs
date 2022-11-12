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
        private Bitmap originalImgFinalized;

        private Digest digest;
        private float similarity;
        bool startCrop;
        private bool _disposedValue;

        public SplitData(string name, int splitNumber, string imagePath, double similarity, bool startCrop = false, bool removeColor = false)
        {
            this.name = name;
            this.splitNumber = splitNumber;
            this.imagePath = imagePath;
            this.similarity = (float)similarity;
            this.startCrop = startCrop;
            this.removeColor = removeColor;
        }

        public void UpdateSplitImage()
        {
            originalImg = SetImage(imagePath).Resize(Constants.comparisonWidth, Constants.comparisonHeight);
            UpdateImgWorkableCrop();
            UpdateFinalImageCrop();
            if (removeColor) originalImgFinalized.RemoveColor();
            digest = ImagePhash.ComputeDigest(originalImgFinalized.ToLuminanceImage());
        }

        public void UpdateSplitImage(string path)
        {
            imagePath = path;
            originalImg = SetImage(imagePath).Resize(Constants.comparisonWidth, Constants.comparisonHeight);
            UpdateImgWorkableCrop();
            UpdateFinalImageCrop();
            if (removeColor) originalImgFinalized.RemoveColor();
            digest = ImagePhash.ComputeDigest(originalImgFinalized.ToLuminanceImage());
        }
        private Bitmap SetImage(string path) => new Bitmap(Image.FromFile(path));

        public int GetSplitNumber() => splitNumber;

        public bool IsDigestSimilar(Digest d) => ImagePhash.GetCrossCorrelation(digest, d) >= similarity;

        public string GetImagePath() => imagePath;

        public Bitmap GetImage() => originalImgWorkable;

        public Bitmap GetImageCropped() => originalImgFinalized;

        public string GetSplitName() => name;

        public Digest GetDigest() => digest;

        public void SetSimilarity(float similarity) => this.similarity = similarity;

        public void UpdateImgWorkableCrop()
        {
            double left = Settings.Default.sourceCropLeft;
            double right = Settings.Default.sourceCropRight;
            double top = Settings.Default.sourceCropTop;
            double bottom = Settings.Default.sourceCropBottom;

            originalImgWorkable = originalImg.Crop(new Rectangle(
                originalImg.Width - (int)(right / 100 * originalImg.Width),
                originalImg.Height - (int)(bottom / 100 * originalImg.Height),
                (int)left / 100 * originalImg.Width,
                (int)top / 100 * originalImg.Height
                ));
        }

        public void UpdateFinalImageCrop()
        {
            originalImgFinalized = originalImgWorkable.Crop(startCrop ? Constants.startCrop : Constants.crop);
            digest = ImagePhash.ComputeDigest(originalImgFinalized.ToLuminanceImage());
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

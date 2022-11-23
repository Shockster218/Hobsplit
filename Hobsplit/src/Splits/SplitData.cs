using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Drawing;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;

namespace Hobsplit
{
    public class SplitData : IDisposable
    {
        private string name;
        private int splitNumber;
        private bool imageInit;

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
            if (imageInit) return;
            originalImg = SetImage(imagePath).Resize(640, 480);
            UpdateImgWorkableCrop();
            if (removeColor) originalImgWorkable.RemoveColor();
            digest = ImagePhash.ComputeDigest(originalImgWorkable.ToLuminanceImage());
            imageInit = true;
        }

        public void UpdateSplitImage(string path)
        {
            if (imageInit) return;
            imagePath = path;
            originalImg = SetImage(imagePath).Resize(640, 480);
            UpdateImgWorkableCrop();
            if (removeColor) originalImgWorkable.RemoveColor();
            digest = ImagePhash.ComputeDigest(originalImgWorkable.ToLuminanceImage());
            imageInit = true;
        }
        private Bitmap SetImage(string path) => new Bitmap(Image.FromFile(path));

        public int GetSplitNumber() => splitNumber;

        public bool IsDigestSimilar(Digest d) => ImagePhash.GetCrossCorrelation(digest, d) >= similarity;

        public float GetCurrentCorrelation(Digest d) => ImagePhash.GetCrossCorrelation(digest, d);

        public bool IsImageInitialized() => imageInit;

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

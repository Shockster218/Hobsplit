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
        private Bitmap baseImage;
        private Bitmap baseCrop;
        private Bitmap cropped;
        private Digest digest;
        private float similarity;
        bool startCrop;
        private bool _disposedValue;

        public SplitData(string name, string imagePath, double similarity, bool startCrop = false, bool removeColor = false)
        {
            this.name = name;
            this.similarity = (float)similarity;
            this.startCrop = startCrop;
            baseImage = SetImage(imagePath);
            UpdateImageCropping(Settings.Default.refCropLeft, Settings.Default.refCropRight, Settings.Default.refCropTop, Settings.Default.refCropTop);
            if(removeColor) cropped.RemoveColor();
            digest = ImagePhash.ComputeDigest(cropped.ToLuminanceImage());
        }

        private Bitmap SetImage(string path) => new Bitmap(Image.FromFile(path)).Resize();

        public bool IsDigestSimilar(Digest d) => ImagePhash.GetCrossCorrelation(digest, d) >= similarity;

        public Bitmap GetImage() => baseCrop;

        public Bitmap GetImageCropped() => cropped;

        public string GetSplitName() => name;

        public Digest GetDigest() => digest;

        public void SetSimilarity(float similarity) => this.similarity = similarity;

        public void UpdateImageCropping(double left, double right, double top, double bottom)
        {
            baseCrop = baseImage.Crop(new RECT(
                (int)left / 100 * baseImage.Width,
                (int)top / 100 * baseImage.Height,
                baseImage.Width - (int)(right / 100 * baseImage.Width),
                baseImage.Height - (int)(bottom / 100 * baseImage.Height)
                ));
            cropped = baseCrop.Crop(startCrop ? Constants.startCrop : Constants.crop);
            digest = ImagePhash.ComputeDigest(cropped.ToLuminanceImage());
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

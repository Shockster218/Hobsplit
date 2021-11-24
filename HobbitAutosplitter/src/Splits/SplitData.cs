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
        private Bitmap preview;
        private Bitmap cropped;
        private Digest digest;
        private float similarity;
        bool startCrop;
        private bool _disposedValue;

        public SplitData(string name, string imagePath, bool startCrop = false, bool removeColor = false, float similarity = 0.965f)
        {
            this.name = name;
            this.similarity = similarity;
            this.startCrop = startCrop;
            baseImage = SetImage(imagePath);
            UpdateImageCropping(Settings.Default.referenceCropPercentage);
            if(removeColor) cropped.RemoveColor();
            digest = ImagePhash.ComputeDigest(cropped.ToLuminanceImage());
        }

        private Bitmap SetImage(string path) 
        {
            return new Bitmap(Image.FromFile(path)).Resize();
        }

        public bool IsDigestSimilar(Digest d)
        {
            return ImagePhash.GetCrossCorrelation(digest, d) >= similarity;
        }

        public Bitmap GetImage() { return preview; }

        public Bitmap GetImageCropped() { return cropped; }

        public string GetSplitName() { return name; }

        public Digest GetDigest() { return digest; }

        public void UpdateImageCropping(double value)
        {
            preview = baseImage.Crop(new RECT((int)(value / 100 * baseImage.Width), 0, baseImage.Width, baseImage.Height));
            cropped = preview.Crop(startCrop ? Constants.startCrop : Constants.crop);
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

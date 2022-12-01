using System;
using System.Drawing;
using Rectangle = System.Drawing.Rectangle;
using System.Drawing.Imaging;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;
using SharpDX;
using SharpDX.Direct3D11;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using SharpDX.DXGI;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;
using System.Threading.Tasks;

namespace Hobsplit
{
    public static class CaptureManager
    {
        private static byte[] frameData;
        private static int frameHeight;
        private static int frameWidth;

        private static IDirect3DDevice device;
        private static Device d3dDevice;
        private static GraphicsCaptureItem item;
        private static Direct3D11CaptureFramePool framePool;
        private static GraphicsCaptureSession session;
        private static Texture2DDescription textureDesc;

        public static void Init()
        {
            ProcessManager.OBSClosedEvent += StopCapture;
        }

        public static void InitializeCapture()
        {
            item = CaptureHelper.CreateItemForWindow(ProcessManager.GetOBS().MainWindowHandle);
            device = DirectXHelper.CreateDevice();
            d3dDevice = DirectXHelper.CreateSharpDXDevice(device);

            Factory2 dxgiFactory = new Factory2();

            textureDesc = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = item.Size.Width,
                Height = item.Size.Height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = { Count = 1, Quality = 0 },
                Usage = ResourceUsage.Staging
            };


            framePool = Direct3D11CaptureFramePool.CreateFreeThreaded(
                device,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                1,
                item.Size);
            session = framePool.CreateCaptureSession(item);

            framePool.FrameArrived += OnFrameArrived;

            session.StartCapture();
        }

        private static void OnFrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            using (Direct3D11CaptureFrame frame = sender.TryGetNextFrame())
            using (Texture2D screenTexture = new Texture2D(d3dDevice, textureDesc))
            using (Texture2D texture2d = DirectXHelper.CreateSharpDXTexture2D(frame.Surface))
            {
                frameHeight = frame.ContentSize.Height;
                frameWidth = frame.ContentSize.Width;

                d3dDevice.ImmediateContext.CopyResource(texture2d, screenTexture);
                DataBox mapSource = d3dDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);

                Bitmap bitmap = new Bitmap(frameWidth, frameHeight);
                BitmapData mapDest = bitmap.LockBits(new Rectangle(0, 0, frameWidth, frameHeight), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                IntPtr sourcePtr = mapSource.DataPointer;
                IntPtr destPtr = mapDest.Scan0;

                for (int y = 0; y < frameHeight; y++)
                {
                    Utilities.CopyMemory(destPtr, sourcePtr, frameWidth * 4);
                    sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                    destPtr = IntPtr.Add(destPtr, mapDest.Stride);
                }

                bitmap.UnlockBits(mapDest);
                d3dDevice.ImmediateContext.UnmapSubresource(screenTexture, 0);

                bitmap = bitmap.Resize(640, 480);
                frameData = bitmap.ToByteArray();

                bitmap.Dispose();
                
                PrepareFrameForComparison();
            }
        }

        private static async void PrepareFrameForComparison()
        {
            Bitmap frame = frameData.ToBitmap();
            frame = frame.Crop(Settings.Default.sourceRect, 640, 480);

            frame = frame.Resize();

            if (SplitManager.GetSplitState() == SplitState.WAITING) frame = frame.Crop(Constants.startCrop);
            else frame = frame.Crop(Constants.crop);

            Digest digest = await Task.Factory.StartNew(() => ImagePhash.ComputeDigest(frame.ToLuminanceImage()));
            frame.Dispose();

            SplitManager.CompareFrames(digest);
        }

        public static byte[] GetFrameData()
        {
            return frameData;
        }

        public static void StopCapture()
        {
            session?.Dispose();
            framePool?.Dispose();
            d3dDevice?.Dispose();
        }
    }
}

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

namespace HobbitAutosplitter
{
    public static class CaptureManager
    {
        public static event PreviewFrameEventHandler SendPreviewFrame;

        private static Bitmap previewFrame;
        private static int frameHeight;
        private static int frameWidth;

        private static IDirect3DDevice device;
        private static Device d3dDevice;
        private static GraphicsCaptureItem item;
        private static Direct3D11CaptureFramePool framePool;
        private static GraphicsCaptureSession session;
        private static Texture2DDescription textureDesc;

        private static Rectangle previewCrop;

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

                Bitmap bitmap = new Bitmap(frameWidth, frameHeight, PixelFormat.Format32bppRgb);
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

                previewFrame = bitmap.Clone() as Bitmap;
                bitmap.Dispose();
                
                HandleFrameComparison();
            }
        }

        private static void HandleFrameComparison()
        {
            previewFrame = previewFrame.Crop(GetPreviewCrop());

            CreateDigest(previewFrame.Clone() as Bitmap);

            previewFrame = previewFrame.Resize(Constants.previewWidth, Constants.previewHeight);

            SendPreviewFrame?.SmartInvoke(((Bitmap)previewFrame.Clone()).ToBitmapImage());
            previewFrame.Dispose();
        }

        private static async void CreateDigest(Bitmap frame)
        {
            frame = frame.Resize(Constants.comparisonWidth, Constants.comparisonHeight);
            if (SplitManager.GetSplitIndex() == 1) frame.RemoveColor();
            Digest digest = await Task.Factory.StartNew(() => ImagePhash.ComputeDigest(frame.ToLuminanceImage()));
            
            frame.Dispose();
            
            //SplitManager.CompareFrames(digest);
        }

        public static void UpdatePreviewCrop()
        {
            previewCrop = new Rectangle(
                frameWidth - (int)(Settings.Default.cropRight / 100 * frameWidth),
                frameHeight - (int)(Settings.Default.cropBottom / 100 * frameHeight),
                (int)Settings.Default.cropLeft / 100 * frameWidth,
                (int)Settings.Default.cropTop / 100 * frameHeight
            );
        }

        public static Rectangle GetPreviewCrop()
        {
            if(previewCrop.Height == 0 || previewCrop.Width == 0) UpdatePreviewCrop();
            return previewCrop;
        }

        public static void StopCapture()
        {
            session?.Dispose();
            framePool?.Dispose();
            d3dDevice?.Dispose();
        }

        public static Bitmap GetPreviewFrame()
        {
            return previewFrame;
        }
    }
}

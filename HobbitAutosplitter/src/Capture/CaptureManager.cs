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
using Windows.Graphics;
using Windows.Graphics.Capture;
using Windows.Graphics.DirectX;

namespace HobbitAutosplitter
{
    public static class CaptureManager
    {
        public static event FrameCreatedEventHandler FrameCreated;
        public static event DigestEventHandler DigestCompleted;

        private static byte[] currentFrameData;
        private static object previewFrame;

        private static IDirect3DDevice device;
        private static Device d3dDevice;
        private static SwapChain1 swapChain;
        private static SizeInt32 lastSize;
        private static GraphicsCaptureItem item;
        private static Direct3D11CaptureFramePool framePool;
        private static GraphicsCaptureSession session;
        private static Texture2D screenTexture;

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
            SwapChainDescription1 description = new SwapChainDescription1()
            {
                Width = item.Size.Width,
                Height = item.Size.Height,
                Format = Format.B8G8R8A8_UNorm,
                Stereo = false,
                SampleDescription = new SampleDescription()
                {
                    Count = 1,
                    Quality = 0
                },
                Usage = Usage.RenderTargetOutput,
                BufferCount = 2,
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.FlipSequential,
                AlphaMode = AlphaMode.Premultiplied,
                Flags = SwapChainFlags.None
            };

            Texture2DDescription textureDesc = new Texture2DDescription
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

            screenTexture = new Texture2D(d3dDevice, textureDesc);

            swapChain = new SwapChain1(dxgiFactory, d3dDevice, ref description);

            framePool = Direct3D11CaptureFramePool.CreateFreeThreaded(
                device,
                DirectXPixelFormat.B8G8R8A8UIntNormalized,
                2,
                item.Size);
            session = framePool.CreateCaptureSession(item);
            lastSize = item.Size;

            framePool.FrameArrived += OnFrameArrived;

            session.StartCapture();
        }

        private static void OnFrameArrived(Direct3D11CaptureFramePool sender, object args)
        {
            bool newSize = false;

            using (Direct3D11CaptureFrame frame = sender.TryGetNextFrame())
            {
                if (frame.ContentSize.Width != lastSize.Width ||
                    frame.ContentSize.Height != lastSize.Height)
                {
                    newSize = true;
                    lastSize = frame.ContentSize;
                    swapChain.ResizeBuffers(
                        2,
                        lastSize.Width,
                        lastSize.Height,
                        Format.B8G8R8A8_UNorm,
                        SwapChainFlags.None);
                }

                using (Texture2D texture2d = DirectXHelper.CreateSharpDXTexture2D(frame.Surface))
                {
                    int height = frame.ContentSize.Height;
                    int width = frame.ContentSize.Width;

                    d3dDevice.ImmediateContext.CopyResource(texture2d, screenTexture);
                    DataBox mapSource = d3dDevice.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);

                    Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                    BitmapData mapDest = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    IntPtr sourcePtr = mapSource.DataPointer;
                    IntPtr destPtr = mapDest.Scan0;

                    for (int y = 0; y < height; y++)
                    {
                        Utilities.CopyMemory(destPtr, sourcePtr, width * 4);
                        sourcePtr = IntPtr.Add(sourcePtr, mapSource.RowPitch);
                        destPtr = IntPtr.Add(destPtr, mapDest.Stride);
                    }

                    bitmap.UnlockBits(mapDest);
                    d3dDevice.ImmediateContext.UnmapSubresource(screenTexture, 0);
                    currentFrameData = bitmap.ToByteArray();
                    bitmap.Dispose();
                }

            }

            swapChain.Present(0, PresentFlags.None);

            if (newSize)
            {
                framePool.Recreate(
                    device,
                    DirectXPixelFormat.B8G8R8A8UIntNormalized,
                    2,
                    lastSize);
            }

            FrameCreated?.SmartInvoke(new FrameCreatedArgs(currentFrameData));
        }

        private static void HandleFrameComparison()
        {
            Bitmap previewCropped = currentFrameData.ToBitmap().Crop(GetPreviewCrop()).Resize();

            previewFrame = previewCropped;

            Bitmap finalCrop = previewCropped.Crop(SplitManager.GetCrop());
            previewCropped.Dispose();

            if (SplitManager.GetSplitIndex() == 1) finalCrop.RemoveColor();
            Digest digest = ImagePhash.ComputeDigest(finalCrop.ToLuminanceImage());
            finalCrop.Dispose();

            DigestCompleted?.Invoke(new DigestArgs(digest));
        }

        public static void UpdatePreviewCrop()
        {
            previewCrop = new Rectangle(
                (int)Settings.Default.cropLeft / 100 * lastSize.Width,
                (int)Settings.Default.cropTop / 100 * lastSize.Height,
                lastSize.Width - (int)(Settings.Default.cropRight / 100 * lastSize.Width),
                lastSize.Height - (int)(Settings.Default.cropBottom / 100 * lastSize.Height)
            );
        }

        public static Rectangle GetPreviewCrop()
        {
            return previewCrop;
        }

        public static void StopCapture()
        {
            session?.Dispose();
            framePool?.Dispose();
            swapChain?.Dispose();
            d3dDevice?.Dispose();
        }

        public static Bitmap GetCurrentFrameData()
        {
            return currentFrameData.ToBitmap();
        }
    }
}

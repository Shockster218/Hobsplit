using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;
using Windows.Graphics.Capture;

namespace Hobsplit
{
    public static class CaptureHelper
    {
        static readonly Guid GraphicsCaptureItemGuid = new Guid("79C3F95B-31F7-4EC2-A464-632EF5D30760");

        [ComImport]
        [Guid("3628E81B-3CAC-4C60-B7F4-23CE0E0C3356")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComVisible(true)]
        interface IGraphicsCaptureItemInterop
        {
            IntPtr CreateForWindow(
                [In] IntPtr window,
                [In] ref Guid iid);
        }

        public static GraphicsCaptureItem CreateItemForWindow(IntPtr hwnd)
        {
            var factory = WindowsRuntimeMarshal.GetActivationFactory(typeof(GraphicsCaptureItem));
            var interop = (IGraphicsCaptureItemInterop)factory;
            var itemPointer = interop.CreateForWindow(hwnd, GraphicsCaptureItemGuid);
            var item = Marshal.GetObjectForIUnknown(itemPointer) as GraphicsCaptureItem;
            Marshal.Release(itemPointer);

            return item;
        }
    }
}

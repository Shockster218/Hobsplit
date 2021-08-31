using System.Collections.Generic;
using Tesseract;
using drawing = System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

namespace HobbitAutoSplitter
{
    public static class Detector
    {
        public static int drawW = 12;
        public static int drawH = 3;
        public static int area = 3;
        public static int minW = 60;        
        public static int maxW = 500;
        public static int minH = 12;
        public static int maxH = 50;
        public static int r = 200;
        public static int g = 215;
        public static int b = 0;

        private static TesseractEngine tess = new TesseractEngine("tessdata", "eng", EngineMode.LstmOnly);

        public static string DetectText(drawing.Bitmap _img)
        {
            Image<Bgr, byte> img = _img.ToImage<Bgr, byte>();
            Image<Gray, byte> sobel = img.Convert<Gray, byte>().Sobel(1, 0, 3).AbsDiff(new Gray(0.0)).Convert<Gray, byte>().ThresholdBinary(new Gray(100), new Gray(255));
            Mat SE = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new drawing.Size(drawW, drawH), new drawing.Point(-1, -1));
            sobel = sobel.MorphologyEx(MorphOp.Dilate, SE, new drawing.Point(-1, -1), 1, BorderType.Reflect, new MCvScalar(255));
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat m = new Mat();

            CvInvoke.FindContours(sobel, contours, m, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            List<drawing.Rectangle> list = new List<drawing.Rectangle>();

            for (int i = 0; i < contours.Size; i++)
            {
                drawing.Rectangle brect = CvInvoke.BoundingRectangle(contours[i]);

                double ar = brect.Width / brect.Height;
                if (ar > area && brect.Width > minW && brect.Width < maxW && brect.Height > minH && brect.Height < maxH)
                {
                    list.Add(brect);
                }
            }

            Image<Bgr, byte> imgout = img.CopyBlank();
            foreach (var rec in list)
            {
                CvInvoke.Rectangle(img, rec, new MCvScalar(0, 0, 255), 2, LineType.FourConnected);
                CvInvoke.Rectangle(imgout, rec, new MCvScalar(r, g, b), -1, LineType.FourConnected);
            }
            imgout._And(img);

            using (Page page = tess.Process(new drawing.Bitmap(imgout.ToBitmap(), new drawing.Size(900, 675))))
            {
                return page.GetText();
            }
        }
    }
}

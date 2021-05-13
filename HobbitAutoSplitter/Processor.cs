using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using drawing = System.Drawing;

using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;

namespace HobbitAutoSplitter
{
    class Processor
    {
        private Process process;

        public Processor(string inputPath, int start, int end, int fps)
        {
            process = new Process();
        }

        private void DetectText(Image<Bgr, byte> img)
        {
            Image<Gray, byte> sobel = img.Convert<Gray, byte>().Sobel(1, 0, 3).AbsDiff(new Gray(0.0)).Convert<Gray, byte>().ThresholdBinary(new Gray(50), new Gray(255));
            Mat SE = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new drawing.Size(50, 30), new drawing.Point(-1, -1));
            sobel = sobel.MorphologyEx(MorphOp.Dilate, SE, new drawing.Point(-1, -1), 1, BorderType.Reflect, new MCvScalar(255));
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat m = new Mat();

            CvInvoke.FindContours(sobel, contours, m, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            List<drawing.Rectangle> list = new List<drawing.Rectangle>();

            for (int i = 0; i < contours.Size; i++)
            {
                drawing.Rectangle brect = CvInvoke.BoundingRectangle(contours[i]);

                double ar = brect.Width / brect.Height;
                if (ar > 2 && brect.Width > 25 && brect.Height > 50 && brect.Height < 60)
                {
                    brect.Inflate(10, 20);
                    list.Add(brect);
                }
            }

            Image<Bgr, byte> imgout = img.CopyBlank();
            foreach (var r in list)
            {
                CvInvoke.Rectangle(img, r, new MCvScalar(0, 0, 255), 2, LineType.FourConnected);
                CvInvoke.Rectangle(imgout, r, new MCvScalar(245, 255, 0), -1, LineType.FourConnected);
            }
            imgout._And(img);

            Tesseract tess = new Tesseract("tessdata", "eng", OcrEngineMode.TesseractOnly);
            tess.SetImage(imgout);
            tess.Recognize();
            string result = tess.GetUTF8Text();
        }

        private BitmapImage ConvertBitmapToBitmapImage(drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
    }
}

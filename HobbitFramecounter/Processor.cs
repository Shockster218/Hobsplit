using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
using drawing = System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;

namespace HobbitFramecounter
{
    class Processor
    {
        private Process process;
        private string inputPath;
        private string outputPath;
        private int start;
        private int end;
        private int fps;

        public Processor(string inputPath, int start, int end, int fps)
        {
            process = new Process();
            this.inputPath = inputPath;
        }

        //private void Test()
        //{
        //    Mat m = new Mat(inputPath);
        //    DetectText(m.ToImage<Bgr, byte>());
        //    m.Dispose();
        //}

        private void ConvertVideoToImage()
        {
            outputPath = $"{Config.convertedDir}\\{inputPath.TrimEnd(System.IO.Path.DirectorySeparatorChar)}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.FileName = Config.ffmpegDir;
            process.StartInfo.Arguments = $"-ss {}-i {inputPath} -r 1/1 {outputPath}/%0d.bmp";

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;

            process.Exited += (sender, args) =>
            {
                ProcessImage();
            };

            process.EnableRaisingEvents = true;

            process.Start();
            
        }

        private void ProcessImage()
        {
            if (outputPath != string.Empty)
            {
                try
                {
                    foreach (String file in System.IO.Directory.GetFiles(outputPath))
                    {
                        Mat m = new Mat(file);
                        DetectText(m.ToImage<Bgr, byte>());
                        m.Dispose();
                    }
                }
                catch
                {
                    //Error in conversion.
                }
            }
        }

        private void DetectText(Image<Bgr, byte> img)
        {
            Image<Gray, byte> sobel = img.Convert<Gray, byte>().Sobel(1, 0, 3).AbsDiff(new Gray(0.0)).Convert<Gray, byte>().ThresholdBinary(new Gray(50), new Gray(255));
            Mat SE = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new drawing.Size(47, 30), new drawing.Point(-1, -1));
            sobel = sobel.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Dilate, SE, new drawing.Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Reflect, new MCvScalar(255));
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Mat m = new Mat();

            CvInvoke.FindContours(sobel, contours, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            List<drawing.Rectangle> list = new List<drawing.Rectangle>();

            for (int i = 0; i < contours.Size; i++)
            {
                drawing.Rectangle brect = CvInvoke.BoundingRectangle(contours[i]);

                double ar = brect.Width / brect.Height;
                if (ar > 2 && brect.Width > 25 && brect.Height > 50 && brect.Height < 60)
                {
                    list.Add(brect);
                }
            }


            Image<Bgr, byte> imgout = img.CopyBlank();
            foreach (var r in list)
            {
                CvInvoke.Rectangle(img, r, new MCvScalar(0, 0, 255), 2);
                CvInvoke.Rectangle(imgout, r, new MCvScalar(0, 255, 255), -1);
            }
            imgout._And(img);
            MainWindow.instance.SetImage(ConvertBitmapToBitmapImage(imgout.ToBitmap()));
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

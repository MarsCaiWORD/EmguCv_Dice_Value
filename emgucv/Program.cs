using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Drawing;
using System.IO;

namespace emgucv
{
    class Program
    {
        static void Main(string[] args)
        {

            var path = new DirectoryInfo(System.Windows.Forms.Application.StartupPath).Parent.Parent.FullName;
            //載圖
            String imagePath = $"{path}/dice.png";
            Image<Bgr, Byte> src = new Image<Bgr, byte>(@imagePath);
            CvInvoke.Imshow("src", src);
            //轉灰
            UMat grayImage = new UMat();
            CvInvoke.CvtColor(src, grayImage, ColorConversion.Bgr2Gray);

            //高斯去躁模糊
            CvInvoke.GaussianBlur(grayImage, grayImage, new Size(5, 5), 5);
            CvInvoke.Imshow("Blur Image", grayImage);


            //霍夫測園
            //CircleF[] circles = CvInvoke.HoughCircles(grayImage, HoughType.Gradient, 0.5, 41, 70, 30, 10,175);


            //建新圖
            UMat cannyEdges = new UMat();
            //抓節點 200~255 色階
            CvInvoke.Canny(grayImage, cannyEdges, 255, 255);
            CvInvoke.Imshow("Canny Image", cannyEdges);

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {

                CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

                //霍夫測園
                CircleF[] circles = CvInvoke.HoughCircles(cannyEdges, HoughType.Gradient, 1, 5, 10, 20, 0, 100);
                //int count = contours.Size;
                Console.Write("點數為:" + circles.Length);

                //建新圖 畫判斷的位置
                Image<Bgr, Byte> circleImage2 = src.Clone();
                foreach (CircleF circle in circles)
                {
                    circleImage2.Draw(circle, new Bgr(Color.Blue), 5);
                    CvInvoke.Imshow("HoughCircles", circleImage2);
                }


            }






            CvInvoke.WaitKey(0);


        }
    }
}

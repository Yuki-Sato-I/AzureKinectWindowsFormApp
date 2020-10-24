using System;
using System.IO;
using System.Text;
using System.Numerics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using Microsoft.Azure.Kinect.BodyTracking;
using Microsoft.Azure.Kinect.Sensor;
using Image = Microsoft.Azure.Kinect.Sensor.Image;
using BitmapData = System.Drawing.Imaging.BitmapData;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using ImageFormat = Microsoft.Azure.Kinect.Sensor.ImageFormat;

namespace kinect_get_data
{
    class GetData
    {
        public static void ColorImage(Capture capture, in Transformation transformation, in Bitmap colorBitmap)
        {
            Image colorImage = transformation.ColorImageToDepthCamera(capture);
            BGRA[] colorArray = colorImage.GetPixels<BGRA>().ToArray();
            BitmapData bitmapData = colorBitmap.LockBits(new Rectangle(0, 0, colorBitmap.Width, colorBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                //各ピクセルの値へのポインタ
                byte* pixels = (byte*)bitmapData.Scan0;
                int index = 0;
                //1ピクセルずつ処理
                for (int i = 0; i < colorArray.Length; i++)
                {
                    pixels[index++] = colorArray[i].B;
                    pixels[index++] = colorArray[i].G;
                    pixels[index++] = colorArray[i].R;
                    pixels[index++] = 255;//Alpha値を固定して不透過に
                }
            }
            colorBitmap.UnlockBits(bitmapData);
        }

        public static void DepthImage(Capture capture, in Bitmap depthBitmap)
        {
            Image depthImage = capture.Depth;
            ushort[] depthArray = depthImage.GetPixels<ushort>().ToArray();
            BitmapData bitmapData = depthBitmap.LockBits(new Rectangle(0, 0, depthBitmap.Width, depthBitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                //各ピクセルの値へのポインタ
                byte* pixels = (byte*)bitmapData.Scan0;
                int index;
                int depth;
                //一ピクセルずつ処理
                for (int i = 0; i < depthArray.Length; i++)
                {
                    //500～5000mmを0～255に変換
                    depth = (int)(255 * (depthArray[i] - 500) / 5000.0);
                    if (depth < 0 || depth > 255) depth = 0;
                    index = i * 4;
                    pixels[index++] = (byte)depth;
                    pixels[index++] = (byte)depth;
                    pixels[index++] = (byte)depth;
                    pixels[index++] = 255;
                }
            }
            depthBitmap.UnlockBits(bitmapData);
        }

    }
}

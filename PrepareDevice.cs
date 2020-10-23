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
    class PrepareDevice
    {
        public static void InitialDevice(ref bool running, ref Device device, ref Transformation transformation)
        {
            running = true;
            device = Device.Open(0);
            device.StartCameras(new DeviceConfiguration()
            {
                CameraFPS = FPS.FPS30,
                ColorFormat = ImageFormat.ColorBGRA32,
                ColorResolution = ColorResolution.R720p,
                DepthMode = DepthMode.NFOV_Unbinned,
                WiredSyncMode = WiredSyncMode.Standalone,
            });
            device.StartImu();
            transformation = device.GetCalibration().CreateTransformation();
        }

        public static void InitBitmap(in Device device, ref Bitmap depthBitmap, ref Bitmap colorBitmap)
        {
            int width = device.GetCalibration().DepthCameraCalibration.ResolutionWidth;
            int height = device.GetCalibration().DepthCameraCalibration.ResolutionHeight;
            depthBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            colorBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        }



    }
}

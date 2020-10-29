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

    public partial class Form1 : Form
    {

        Device device;

        bool running = false;
        bool measure = false; //測定開始フラグ
        Bitmap depthBitmap;
        Bitmap colorBitmap;
        Transformation transformation;

        // csv出力するためのデータ

        string[][] data = new string[][]
        {
            new string[]{"center X", "center Y", "center Z", "acc X", "acc Y", "acc Z", "COG time", "Acc time" }
        };

        public Form1()
        {
            InitializeComponent();
            PrepareDevice.InitialDevice(ref running, ref device, ref transformation);
            PrepareDevice.InitBitmap(in device, ref depthBitmap, ref colorBitmap);
            GetDataFromKinect();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
            device.Dispose();
        }

        private void DisplayInformation(Frame frame, ImuSample imu)
        {
            
            var heightPixels = frame.BodyIndexMap.HeightPixels;
            var widthPixels = frame.BodyIndexMap.WidthPixels;
            var totalPixels = heightPixels * widthPixels;
            TotalPixels.Text = totalPixels.ToString();

            string frameTimeStamp = frame.DeviceTimestamp.ToString();
            string imuTimeStamp = imu.AccelerometerTimestamp.ToString();

            IMUTimestamp.Text = imuTimeStamp;
            FrameTimestamp.Text = frameTimeStamp;

            //GetImu();

            var position = frame.GetBodySkeleton(0).GetJoint(JointId.SpineNavel).Position;
            var quaternion = frame.GetBodySkeleton(0).GetJoint(JointId.SpineNavel).Quaternion;

            NumberOfBodies.Text = frame.NumberOfBodies.ToString();

            positionData.Text = "Position:\n　X: " + position.X + "\n　Y: " + (-1 * position.Y) + "\n　Z: " + position.Z;
            quaternionData.Text = "Quaternion:\n　X: " + quaternion.X + "\n　Y: " + quaternion.Y + "\n　Z: " + quaternion.Z + "\n　W: " + quaternion.W;

            GetData.ColorImage(frame.Capture, in transformation, in colorBitmap);
            GetData.DepthImage(frame.Capture, in depthBitmap);

            pictureBox1.Image = depthBitmap;
            pictureBox2.Image = colorBitmap;
        }
        
        public async Task<int> GetDataFromKinect ()
        {
            var deviceCailbration = device.GetCalibration();

            using (Tracker tracker = Tracker.Create(deviceCailbration, new TrackerConfiguration() {ProcessingMode = TrackerProcessingMode.Gpu, SensorOrientation = SensorOrientation.Default}))
            {
                while(running)
                {
                    ImuSample imu;
                    using (Capture sensorCapture = await Task.Run(() => { return device.GetCapture(); }).ConfigureAwait(true))
                    {
                        imu = device.GetImuSample();
                        TimeSpan time = new TimeSpan(0, 0, 5, 0, 0);
                        try
                        {
                           tracker.EnqueueCapture(sensorCapture, time);

                            using (Frame frame = await Task.Run(() => tracker.PopResult(TimeSpan.Zero, throwOnTimeout: false)))
                            {
                                string frameTimeStamp = frame.DeviceTimestamp.ToString();
                                string imuTimeStamp = imu.AccelerometerTimestamp.ToString();

                                if (frame != null)
                                {
                                    DisplayInformation(frame, imu);
                                    
                                    if (measure)
                                    {
                                        // caluculate center of gravity and write data
                                        Array.Resize(ref data, data.Length + 1);

                                        var COGdata = Calc.CenterOfGravity(frame.GetBodySkeleton(0));
                                        var sample = imu.AccelerometerSample;
                                        data[data.Length - 1] = new string[] {
                                            COGdata[0],
                                            COGdata[1],
                                            COGdata[2],
                                            sample.X.ToString(),
                                            sample.Y.ToString(),
                                            sample.Z.ToString(),
                                            frameTimeStamp,
                                            imuTimeStamp
                                        };

                                    }

                                }
                            }
                            this.Update();
                        }
                        catch(Exception ex)
                        {
                            Error.Text = ex.Message;
                        }
      
                    }

                }

            }
            return 0;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MeasureBtn_Click(object sender, EventArgs e)
        {
            measure = !measure;
            measureBtn.Text = measure ? "計測停止" : "計測開始";
            
            if (!measure)
            {
                csvLabel.Text = OutData.CSV(answerLabel.Text, data);
            }
            else
            {
                csvLabel.Text = "計測中";
            }
        }

    }
}

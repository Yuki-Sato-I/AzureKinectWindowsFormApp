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
        /// <summary>
        /// 身体各部位の重心位置比率
        /// <value name="Head">頭</typeparam >
        /// <value name="Torso">胴体</value>
        /// <value name="UpperArm">上腕</value>
        /// <value name="Forearm">前腕</value>
        /// <value name="Hand">手</value>
        /// <value name="Thigh">大腿</value>
        /// <value name="LowerLeg">下腿</value>
        /// <value name="Leg">足</value>
        /// </summary>
        static class PositionRatioOfGravity
        {
            public const float Head = 0.46f;
            public const float Torso = 0.52f;
            public const float UpperArm = 0.46f;
            public const float Forearm = 0.41f;
            public const float Hand = 0.50f;
            public const float Thigh = 0.42f;
            public const float LowerLeg = 0.41f;
            public const float Leg = 0.50f;
        }

        static class PositionRatioOfWeight
        {
            public const float Head = 0.078f;
            public const float Torso = 0.479f;
            public const float UpperArm = 0.0265f;
            public const float Forearm = 0.015f;
            public const float Hand = 0.09f;
            public const float Thigh = 0.1f;
            public const float LowerLeg = 0.0535f;
            public const float Leg = 0.019f;
            public const float LowerBody = (Thigh + LowerLeg + Leg) * 2f;
            public const float UpperBody = Head + Torso + (UpperArm + Forearm + Hand) * 2f;
            public const float Arm = UpperArm + Hand + Forearm;
        }

        Device device;
        bool running = false;
        bool measure = false; //測定開始フラグ
        Bitmap depthBitmap;
        Bitmap colorBitmap;
        Transformation transformation;

        //csv出力するためのデータ
        string[][] data = new string[][]
        {
            new string[]{"center X", "test X", "center Y", "test Y", "center Z", "test Z" }
        };

        public Form1()
        {
            InitializeComponent();
            PrepareDevice.InitialDevice(ref running, ref device, ref transformation);
            PrepareDevice.InitBitmap(in device, ref depthBitmap, ref colorBitmap);
            GetDataFromKinect();
        }

        //加速度　Accelerometer m/s^2(メートル毎秒毎秒)
        // FIXME 未完成　加速度の値がおかしい気がする
        private void GetImu()
        {
            ImuSample imu;
            try
            {
                imu = device.GetImuSample();
                var x = imu.AccelerometerSample.X;
                var y = imu.AccelerometerSample.Y;
                var z = imu.AccelerometerSample.Z;

                NumberOfHeightPixels.Text = x.ToString();
                NumberOfWidthPixels.Text = y.ToString();
                TotalPixels.Text = z.ToString();
            }
            catch (Exception ex)
            {
                Error.Text = ex.Message;
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            running = false;
            device.Dispose();
        }

        private void DisplayInformation(Frame frame)
        {
            
            var heightPixels = frame.BodyIndexMap.HeightPixels;
            var widthPixels = frame.BodyIndexMap.WidthPixels;
            var totalPixels = heightPixels * widthPixels;
            NumberOfHeightPixels.Text = heightPixels.ToString();
            NumberOfWidthPixels.Text = widthPixels.ToString();
            TotalPixels.Text = totalPixels.ToString();
            

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
                    using (Capture sensorCapture = await Task.Run(() => { return device.GetCapture(); }).ConfigureAwait(true))
                    {
                        TimeSpan time = new TimeSpan(0, 0, 5, 0, 0);
                        try
                        {
                            tracker.EnqueueCapture(sensorCapture, time);

                            using (Frame frame = await Task.Run(() => tracker.PopResult(TimeSpan.Zero, throwOnTimeout: false)))
                            {

                                if (frame != null)
                                {
                                    DisplayInformation(frame);

                                    if (measure)
                                    {
                                        // caluculate center of gravity and write data
                                        CalcCenterOfGravity(frame.GetBodySkeleton(0), ref data);
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

        // calculation center of gravity
        private void CalcCenterOfGravity(Skeleton skeleton, ref string[][] data)
        {
            // test
            var testData = skeleton.GetJoint(JointId.SpineChest).Position;


            // getted from Azure Kinect device
            var footRight     = skeleton.GetJoint(JointId.FootRight).Position;
            var footLeft      = skeleton.GetJoint(JointId.FootLeft).Position;
            var ankleRight    = skeleton.GetJoint(JointId.AnkleRight).Position;
            var ankleLeft     = skeleton.GetJoint(JointId.AnkleLeft).Position;
            var kneeRight     = skeleton.GetJoint(JointId.KneeRight).Position;
            var kneeLeft      = skeleton.GetJoint(JointId.KneeLeft).Position;
            var hipRight      = skeleton.GetJoint(JointId.HipRight).Position;
            var hipLeft       = skeleton.GetJoint(JointId.HipLeft).Position;

            var handtipRight  = skeleton.GetJoint(JointId.HandTipRight).Position;
            var handtipLeft   = skeleton.GetJoint(JointId.HandTipLeft).Position;
            var wristRight    = skeleton.GetJoint(JointId.WristRight).Position;
            var wristLeft     = skeleton.GetJoint(JointId.WristLeft).Position;
            var elbowRight    = skeleton.GetJoint(JointId.ElbowRight).Position;
            var elbowLeft     = skeleton.GetJoint(JointId.ElbowLeft).Position;
            var clavicleRight = skeleton.GetJoint(JointId.ClavicleRight).Position;
            var clavicleLeft  = skeleton.GetJoint(JointId.ClavicleLeft).Position;
            var pelvis        = skeleton.GetJoint(JointId.Pelvis).Position;
            var neck          = skeleton.GetJoint(JointId.Neck).Position;
            var earRight      = skeleton.GetJoint(JointId.Neck).Position;
            var earLeft       = skeleton.GetJoint(JointId.Neck).Position;

            // 新しい値 = 中枢に近い点 * (1-比率) + 中枢から遠い点 * (比率)

            // 足
            Vector3 legRight      = Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), footRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), ankleRight);
            Vector3 legLeft       = Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), footLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), ankleLeft);
            // 下腿
            Vector3 lowerLegRight = Vector3.Multiply(new Vector3(1-PositionRatioOfGravity.LowerLeg), kneeRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), ankleRight);
            Vector3 lowerLegLeft  = Vector3.Multiply(new Vector3(1-PositionRatioOfGravity.LowerLeg), kneeLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), ankleLeft);
            // 大腿
            Vector3 thighRight    = Vector3.Multiply(new Vector3(1-PositionRatioOfGravity.Thigh), hipRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Thigh), kneeRight);
            Vector3 thighLeft     = Vector3.Multiply(new Vector3(1-PositionRatioOfGravity.Thigh), hipLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Thigh), kneeLeft);
            // 手
            Vector3 handRight     = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Hand), wristRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Hand), handtipRight);
            Vector3 handLeft      = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Hand), wristLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Hand), handtipLeft);
            // 前腕
            Vector3 forearmRight  = Vector3.Multiply(new Vector3(1-PositionRatioOfGravity.Forearm), elbowRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Forearm), wristRight);
            Vector3 forearmLeft   = Vector3.Multiply(new Vector3(1-PositionRatioOfGravity.Forearm), elbowLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Forearm), wristLeft);
            //上腕
            Vector3 upperRight    = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.UpperArm), clavicleRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Forearm), elbowRight);
            Vector3 upperLeft     = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.UpperArm), clavicleLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Forearm), elbowLeft);
            //胴
            Vector3 torso         = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Torso), neck) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Torso), pelvis);
            //頭 どうやって計算しよ(とりあえず耳と耳の間)
            Vector3 head          = Vector3.Multiply(earRight + earLeft, new Vector3(0.5f));

            // 身体重心の計算
            // 左足 
            Vector3 belowKneeLeft    = Vector3.Multiply(legLeft, PositionRatioOfWeight.LowerLeg / (PositionRatioOfWeight.LowerLeg+PositionRatioOfWeight.Leg)) + Vector3.Multiply(lowerLegLeft, PositionRatioOfWeight.Leg / (PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg));
            Vector3 lowerBodyLeft    = Vector3.Multiply(belowKneeLeft, PositionRatioOfWeight.Thigh / (PositionRatioOfWeight.Thigh + PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg)) + Vector3.Multiply(thighLeft, PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg / (PositionRatioOfWeight.Thigh + PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg));

            // 右足
            Vector3 belowKneeRight   = Vector3.Multiply(legRight, PositionRatioOfWeight.LowerLeg / (PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg)) + Vector3.Multiply(lowerLegRight, PositionRatioOfWeight.Leg / (PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg));
            Vector3 lowerBodyRight   = Vector3.Multiply(belowKneeRight, PositionRatioOfWeight.Thigh / (PositionRatioOfWeight.Thigh + PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg)) + Vector3.Multiply(thighRight, PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg / (PositionRatioOfWeight.Thigh + PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg));

            // 下半身*
            Vector3 lowerBody        = Vector3.Multiply(lowerBodyRight + lowerBodyLeft, new Vector3(0.5f));

            //左腕*
            Vector3 underElbowLeft   = Vector3.Multiply(forearmLeft, PositionRatioOfWeight.Hand / (PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm)) + Vector3.Multiply(handLeft, PositionRatioOfWeight.Forearm / (PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm));
            Vector3 armLeft          = Vector3.Multiply(underElbowLeft, PositionRatioOfWeight.UpperArm / (PositionRatioOfWeight.UpperArm + PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm)) + Vector3.Multiply(upperLeft, PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm / (PositionRatioOfWeight.UpperArm + PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm));

            //右腕*
            Vector3 underElbowRight  = Vector3.Multiply(forearmRight, PositionRatioOfWeight.Hand / (PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm)) + Vector3.Multiply(handRight, PositionRatioOfWeight.Forearm / (PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm));
            Vector3 armRight         = Vector3.Multiply(underElbowRight, PositionRatioOfWeight.UpperArm / (PositionRatioOfWeight.UpperArm + PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm)) + Vector3.Multiply(upperRight, PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm / (PositionRatioOfWeight.UpperArm + PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm));

            //胴体と頭
            Vector3 bodyAndHead      = Vector3.Multiply(head, PositionRatioOfWeight.Torso / (PositionRatioOfWeight.Torso + PositionRatioOfWeight.Head)) + Vector3.Multiply(torso, PositionRatioOfWeight.Head / (PositionRatioOfWeight.Torso + PositionRatioOfWeight.Head));
            //右腕左腕
            Vector3 arms             = Vector3.Multiply(armRight + armLeft, new Vector3(0.5f));

            //上半身
            Vector3 upperBody        = Vector3.Multiply(bodyAndHead, PositionRatioOfWeight.Arm * 2 / (PositionRatioOfWeight.Arm*2 + PositionRatioOfWeight.Torso +PositionRatioOfWeight.Head)) + Vector3.Multiply(arms, PositionRatioOfWeight.Head + PositionRatioOfWeight.Torso / (PositionRatioOfWeight.Arm * 2 + PositionRatioOfWeight.Torso + PositionRatioOfWeight.Head));

            //重心
            Vector3 CenterOfGravity = Vector3.Multiply(upperBody, PositionRatioOfWeight.LowerBody / (PositionRatioOfWeight.LowerBody + PositionRatioOfWeight.UpperBody)) + Vector3.Multiply(lowerBody, PositionRatioOfWeight.UpperBody / (PositionRatioOfWeight.LowerBody + PositionRatioOfWeight.UpperBody));


            Array.Resize(ref data, data.Length + 1);
            data[data.Length - 1] = new string[] {
                CenterOfGravity.X.ToString(), testData.X.ToString(),
                CenterOfGravity.Y.ToString(), testData.Y.ToString(),
                CenterOfGravity.Z.ToString(), testData.Z.ToString()
            };

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // もう使ってないボタン
        private void Button1_Click(object sender, EventArgs e)
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
        }

    }
}

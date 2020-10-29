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
    class Calc
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

        // calculation center of gravity
        public static string[] CenterOfGravity(Skeleton skeleton)
        {
            // got from Azure Kinect device
            var footRight = skeleton.GetJoint(JointId.FootRight).Position;
            var footLeft = skeleton.GetJoint(JointId.FootLeft).Position;
            var ankleRight = skeleton.GetJoint(JointId.AnkleRight).Position;
            var ankleLeft = skeleton.GetJoint(JointId.AnkleLeft).Position;
            var kneeRight = skeleton.GetJoint(JointId.KneeRight).Position;
            var kneeLeft = skeleton.GetJoint(JointId.KneeLeft).Position;
            var hipRight = skeleton.GetJoint(JointId.HipRight).Position;
            var hipLeft = skeleton.GetJoint(JointId.HipLeft).Position;

            var handtipRight = skeleton.GetJoint(JointId.HandTipRight).Position;
            var handtipLeft = skeleton.GetJoint(JointId.HandTipLeft).Position;
            var wristRight = skeleton.GetJoint(JointId.WristRight).Position;
            var wristLeft = skeleton.GetJoint(JointId.WristLeft).Position;
            var elbowRight = skeleton.GetJoint(JointId.ElbowRight).Position;
            var elbowLeft = skeleton.GetJoint(JointId.ElbowLeft).Position;
            var clavicleRight = skeleton.GetJoint(JointId.ClavicleRight).Position;
            var clavicleLeft = skeleton.GetJoint(JointId.ClavicleLeft).Position;
            var pelvis = skeleton.GetJoint(JointId.Pelvis).Position;
            var neck = skeleton.GetJoint(JointId.Neck).Position;
            var earRight = skeleton.GetJoint(JointId.Neck).Position;
            var earLeft = skeleton.GetJoint(JointId.Neck).Position;

            // 新しい値 = 中枢に近い点 * (1-比率) + 中枢から遠い点 * (比率)

            // 足
            Vector3 legRight = Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), footRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), ankleRight);
            Vector3 legLeft = Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), footLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), ankleLeft);
            // 下腿
            Vector3 lowerLegRight = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.LowerLeg), kneeRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), ankleRight);
            Vector3 lowerLegLeft = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.LowerLeg), kneeLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Leg), ankleLeft);
            // 大腿
            Vector3 thighRight = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Thigh), hipRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Thigh), kneeRight);
            Vector3 thighLeft = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Thigh), hipLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Thigh), kneeLeft);
            // 手
            Vector3 handRight = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Hand), wristRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Hand), handtipRight);
            Vector3 handLeft = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Hand), wristLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Hand), handtipLeft);
            // 前腕
            Vector3 forearmRight = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Forearm), elbowRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Forearm), wristRight);
            Vector3 forearmLeft = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Forearm), elbowLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Forearm), wristLeft);
            //上腕
            Vector3 upperRight = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.UpperArm), clavicleRight) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Forearm), elbowRight);
            Vector3 upperLeft = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.UpperArm), clavicleLeft) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Forearm), elbowLeft);
            //胴
            Vector3 torso = Vector3.Multiply(new Vector3(1 - PositionRatioOfGravity.Torso), neck) + Vector3.Multiply(new Vector3(PositionRatioOfGravity.Torso), pelvis);
            //頭 どうやって計算しよ(とりあえず耳と耳の間)
            Vector3 head = Vector3.Multiply(earRight + earLeft, new Vector3(0.5f));

            // 身体重心の計算
            // 左足 
            Vector3 belowKneeLeft = Vector3.Multiply(legLeft, PositionRatioOfWeight.LowerLeg / (PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg)) + Vector3.Multiply(lowerLegLeft, PositionRatioOfWeight.Leg / (PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg));
            Vector3 lowerBodyLeft = Vector3.Multiply(belowKneeLeft, PositionRatioOfWeight.Thigh / (PositionRatioOfWeight.Thigh + PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg)) + Vector3.Multiply(thighLeft, PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg / (PositionRatioOfWeight.Thigh + PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg));

            // 右足
            Vector3 belowKneeRight = Vector3.Multiply(legRight, PositionRatioOfWeight.LowerLeg / (PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg)) + Vector3.Multiply(lowerLegRight, PositionRatioOfWeight.Leg / (PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg));
            Vector3 lowerBodyRight = Vector3.Multiply(belowKneeRight, PositionRatioOfWeight.Thigh / (PositionRatioOfWeight.Thigh + PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg)) + Vector3.Multiply(thighRight, PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg / (PositionRatioOfWeight.Thigh + PositionRatioOfWeight.LowerLeg + PositionRatioOfWeight.Leg));

            // 下半身*
            Vector3 lowerBody = Vector3.Multiply(lowerBodyRight + lowerBodyLeft, new Vector3(0.5f));

            //左腕*
            Vector3 underElbowLeft = Vector3.Multiply(forearmLeft, PositionRatioOfWeight.Hand / (PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm)) + Vector3.Multiply(handLeft, PositionRatioOfWeight.Forearm / (PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm));
            Vector3 armLeft = Vector3.Multiply(underElbowLeft, PositionRatioOfWeight.UpperArm / (PositionRatioOfWeight.UpperArm + PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm)) + Vector3.Multiply(upperLeft, PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm / (PositionRatioOfWeight.UpperArm + PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm));

            //右腕*
            Vector3 underElbowRight = Vector3.Multiply(forearmRight, PositionRatioOfWeight.Hand / (PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm)) + Vector3.Multiply(handRight, PositionRatioOfWeight.Forearm / (PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm));
            Vector3 armRight = Vector3.Multiply(underElbowRight, PositionRatioOfWeight.UpperArm / (PositionRatioOfWeight.UpperArm + PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm)) + Vector3.Multiply(upperRight, PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm / (PositionRatioOfWeight.UpperArm + PositionRatioOfWeight.Hand + PositionRatioOfWeight.Forearm));

            //胴体と頭
            Vector3 bodyAndHead = Vector3.Multiply(head, PositionRatioOfWeight.Torso / (PositionRatioOfWeight.Torso + PositionRatioOfWeight.Head)) + Vector3.Multiply(torso, PositionRatioOfWeight.Head / (PositionRatioOfWeight.Torso + PositionRatioOfWeight.Head));
            //右腕左腕
            Vector3 arms = Vector3.Multiply(armRight + armLeft, new Vector3(0.5f));

            //上半身
            Vector3 upperBody = Vector3.Multiply(bodyAndHead, PositionRatioOfWeight.Arm * 2 / (PositionRatioOfWeight.Arm * 2 + PositionRatioOfWeight.Torso + PositionRatioOfWeight.Head)) + Vector3.Multiply(arms, PositionRatioOfWeight.Head + PositionRatioOfWeight.Torso / (PositionRatioOfWeight.Arm * 2 + PositionRatioOfWeight.Torso + PositionRatioOfWeight.Head));

            //重心
            Vector3 CenterOfGravity = Vector3.Multiply(upperBody, PositionRatioOfWeight.LowerBody / (PositionRatioOfWeight.LowerBody + PositionRatioOfWeight.UpperBody)) + Vector3.Multiply(lowerBody, PositionRatioOfWeight.UpperBody / (PositionRatioOfWeight.LowerBody + PositionRatioOfWeight.UpperBody));

            return new string[] {
                CenterOfGravity.X.ToString(),
                CenterOfGravity.Y.ToString(),
                CenterOfGravity.Z.ToString()
            };

        }

    }
}

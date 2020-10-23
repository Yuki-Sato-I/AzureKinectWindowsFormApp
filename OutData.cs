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
    class OutData
    {

        public static string CSV(string answerLabelText, string[][] data)
        {
            DateTime dateTime = DateTime.Now;
            string fileName = dateTime.Year + "_" + dateTime.Month + "_" + dateTime.Day + "_" + dateTime.Hour + "_" + dateTime.Minute + "_" + dateTime.Second + ".csv";

            if (answerLabelText != "")
            {
                fileName = "(" + answerLabelText + ")" + "_" + fileName;
            }

            string outText;

            try
            {
                StreamWriter file = new StreamWriter("/Users/yuki/Desktop/data/" + fileName, false, Encoding.UTF8);
                foreach (string[] lineData in data)
                {
                    file.WriteLine(String.Join(",", lineData));
                }
                file.Close();
                outText = "The file was written successfully.";
            }
            catch (Exception ex)
            {
                outText = ex.Message;
            }

            return outText;
        }

        public static void DisplayInformation(ref string a)
        {
            a = "aaaa";
        }

    }
}

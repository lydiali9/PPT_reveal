using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace WinFormCharpWebCam
{
    //Design by Pongsakorn Poosankam
    public partial class mainWinForm : Form
    {
        public mainWinForm()
        {
            InitializeComponent();
        }
        WebCam webcam;
        private void mainWinForm_Load(object sender, EventArgs e)
        {
            webcam = new WebCam();
            webcam.InitializeWebCam(ref imgVideo);
        }

        private void bntStart_Click(object sender, EventArgs e)
        {
            webcam.Start();
        }

        private void bntStop_Click(object sender, EventArgs e)
        {
            webcam.Stop();
        }

        private void bntContinue_Click(object sender, EventArgs e)
        {
            webcam.Continue();
        }

        private void bntCapture_Click(object sender, EventArgs e)
        {
            imgCapture.Image = imgVideo.Image;
        }

        private void bntSave_Click(object sender, EventArgs e)
        {
            Helper.SaveImageCapture(imgCapture.Image);
        }

        private void bntVideoFormat_Click(object sender, EventArgs e)
        {
            webcam.ResolutionSetting();
        }

        private void bntVideoSource_Click(object sender, EventArgs e)
        {
            webcam.AdvanceSetting();
        }

        [DllImport("C:/Users/lli508/Documents/visual studio 2015/Projects/OpenCVFaceRecognition/x64/Debug/OpenCVFaceRecognition.dll", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool detectFaces(string filePath);
        private void button1_Click(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(@"C:\Users\lli508\Desktop\test");
            if (files.Count() > 1 || files.Count() == 0)
            {
                return;
            }

            try {
                if (!detectFaces(files)) {
                    return;
                }
            } catch (Exception ex) {
                return;
            }

            FaceService.FaceService fs = new FaceService.FaceService();
            var persons = fs.IdentifyTest(files[0]);

            File.Delete(files[0]);
            foreach(var person in persons)
            {
                MessageBox.Show(person.UserData);
            }
        }
    }
}

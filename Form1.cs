using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace FaceRecognition
{
    public partial class Form1 : Form
    {
        //VideoCapture cap;
        //private Mat _frame;
        private CascadeClassifier haarFace, haarEye, haarMouth, haarNose;
        private Image<Bgr, Byte> myImage;

        private void ProcessFrame(object sender, EventArgs e)
        {
           /* if (cap != null && cap.Ptr != IntPtr.Zero)
            {
                
                cap.Retrieve(_frame, 0);
                imageBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                imageBox1.Image = _frame;
            }*/
        }

        public Form1()
        {
            InitializeComponent();
           /* cap = new VideoCapture(0);
            cap.ImageGrabbed += ProcessFrame;
            _frame = new Mat();
            if (cap != null)
            {
                try
                {
                    cap.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if(openFile.ShowDialog()==DialogResult.OK)
            {
                myImage = new Image<Bgr, byte>(openFile.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = myImage.ToBitmap();
            }

            using (Image<Bgr, byte> nextFrame = myImage)
            {
                if (nextFrame != null)
                {
                    // there's only one channel (greyscale), hence the zero index
                    //var faces = nextFrame.DetectHaarCascade(haar)[0];
                   
                    var grayframe = nextFrame.Convert<Gray, byte>();
                    var faces = haarFace.DetectMultiScale(grayframe, 1.1, 10, Size.Empty); //the actual face detection happens here

                    foreach (var face in faces)
                    {
                        nextFrame.Draw(face, new Bgr(Color.Red), 3);

                        grayframe.ROI = face;
                        
                        var mouth = haarMouth.DetectMultiScale(grayframe, 1.1, 140, Size.Empty);
                        var eyes = haarEye.DetectMultiScale(grayframe, 1.1, 30, Size.Empty);
                        var nose = haarNose.DetectMultiScale(grayframe, 1.1, 70, Size.Empty);

                        grayframe.ROI = Rectangle.Empty;

                        foreach (var m in mouth)
                        {
                            Rectangle mouthRect = m;
                            mouthRect.Offset(face.X, face.Y);
                            nextFrame.Draw(mouthRect, new Bgr(Color.Blue), 2);
                        }

                        foreach(var eye in eyes)
                        {
                            Rectangle eyeRect = eye;
                            eyeRect.Offset(face.X, face.Y);
                            nextFrame.Draw(eyeRect, new Bgr(Color.Green), 3);
                        }

                        foreach(var n in nose)
                        {
                            Rectangle noseRect = n;
                            noseRect.Offset(face.X, face.Y);
                            nextFrame.Draw(noseRect, new Bgr(Color.Yellow), 3);
                        }

                    }

                    pictureBox1.Image = nextFrame.ToBitmap();

                }
            }
        }

        private void imageBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           haarFace = new CascadeClassifier(@"C:\Users\elfy_\Desktop\faculta\Structure of Computer Systems\FaceRecognition\FaceRecognition\haarcascade_frontalface_alt2.xml");
           haarEye = new CascadeClassifier(@"C:\Users\elfy_\Desktop\faculta\Structure of Computer Systems\FaceRecognition\FaceRecognition\haarcascade_eye.xml");
           haarMouth = new CascadeClassifier(@"C:\Users\elfy_\Desktop\faculta\Structure of Computer Systems\FaceRecognition\FaceRecognition\haarcascade_smile.xml");
           haarNose = new CascadeClassifier(@"C:\Users\elfy_\Desktop\faculta\Structure of Computer Systems\FaceRecognition\FaceRecognition\haarcascade_nose.xml");
        }
    }
}

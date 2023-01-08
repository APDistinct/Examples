using FLChat.Core.Media;
using FLChat.WebService.MediaType;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureCutterShow
{
    public partial class Form1 : Form
    {
        private byte[] filedata;
        private byte[] newdata;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                var fileName = openFileDialog1.FileName;

                pictureBox1.Image = new Bitmap(fileName);
                filedata = System.IO.File.ReadAllBytes(fileName);

                //Read the contents of the file into a stream
                //var fileStream = openFileDialog1.OpenFile();

                //using (StreamReader reader = new StreamReader(fileStream))
                //{
                //    fileContent = reader.ReadToEnd();
                //}
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int w = pictureBox1.Image.Width;
            int h = pictureBox1.Image.Height;
            label1w.Text = w.ToString();
            label1h.Text = h.ToString();
            if (filedata.GetImageSize(out int? width, out int? height))
            {
                label1w.Text += $"  : {width}";
                label1h.Text += $"  : {height}";
            }            
        }

        private void buttonCut_Click(object sender, EventArgs e)
        {
            ChangeImage();
            ShowImage();
        }

        private void ChangeImage()
        {
            newdata = new byte[filedata.Length];
            filedata.CopyTo(newdata, 0);
            PictureFoursquareCutter cutter = new PictureFoursquareCutter();
            cutter.Cut(ref newdata);            
        }

        private void ShowImage()
        {
            MemoryStream ms = new MemoryStream(newdata);
            pictureBox2.Image = Image.FromStream(ms);
            int w = pictureBox2.Image.Width;
            int h = pictureBox2.Image.Height;
            label2w.Text = w.ToString();
            label2h.Text = h.ToString();
            if (newdata.GetImageSize(out int? width, out int? height))
            {
                label2w.Text += $"  : {width}";
                label2h.Text += $"  : {height}";
            }
        }
    }
}

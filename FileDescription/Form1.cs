using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using static Iplugin.PetPlug;
using Iplugin;

namespace FileDescription
{
    public partial class Form1 : Form
    {
        private ClosedCallback callback = null;
        private IPetPlug plug = null;
        public Form1(ClosedCallback callback, IPetPlug plug)
        {
            this.callback = callback;
            this.plug = plug;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int width = 128;
            int height = 64;
            Bitmap bmp = new Bitmap(width, height);
            Random rand = new Random();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double value = rand.NextDouble();
                    Color color = value > 0.5 ? Color.White : Color.Black;
                    bmp.SetPixel(x, y, color);
                }
            }
            string dir = @"D:\Download\pic";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string fileName = $"{DateTime.Now:yyyyMMddHHmmssfff}.png";
            string filePath = Path.Combine(dir, fileName);
            bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            MessageBox.Show($"图片已保存到: {filePath}");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (callback != null && plug != null)
            {
                ClosedCallback callbackTmp = callback;
                IPetPlug plugTmp = plug;
                plug = null;
                callback = null;
                plug = null;
                callbackTmp(plugTmp);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}

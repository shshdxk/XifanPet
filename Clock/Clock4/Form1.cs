using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WinSystem;

namespace Clock4
{
    public partial class Form1 : Form
    {
        private Image animatedImage;
        private int frameCount;
        private int currentFrame = 0;
        private Timer timer = new Timer();
        private FrameDimension frameDimension;
        private string imagePath;

        public Form1()
        {
            InitializeComponent();
            InitAnimatedPng();
        }

        private void InitAnimatedPng()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            imagePath = AppDomain.CurrentDomain.BaseDirectory + @"Resource\\animated.png";
            animatedImage = Image.FromFile(imagePath);

            Guid[] dimensions = animatedImage.FrameDimensionsList;
            frameDimension = new FrameDimension(dimensions[0]);
            frameCount = animatedImage.GetFrameCount(frameDimension);

            this.Width = animatedImage.Width;
            this.Height = animatedImage.Height;

            timer.Interval = GetFrameDelay(animatedImage, frameDimension, 0);
            timer.Tick += (s, e) => { NextFrame(); };
            timer.Start();

            ShowFrame();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cp;
            }
        }

        private void NextFrame()
        {
            currentFrame = (currentFrame + 1) % frameCount;
            ShowFrame();
            timer.Interval = GetFrameDelay(animatedImage, frameDimension, currentFrame);
        }

        private void ShowFrame()
        {
            animatedImage.SelectActiveFrame(frameDimension, currentFrame);
            using (Bitmap bmp = new Bitmap(animatedImage.Width, animatedImage.Height, PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(animatedImage, 0, 0, animatedImage.Width, animatedImage.Height);
                SetBits(this.Handle, bmp, this.Left, this.Top);
            }
        }

        private int GetFrameDelay(Image img, FrameDimension dim, int frameIndex)
        {
            try
            {
                img.SelectActiveFrame(dim, frameIndex);
                var item = img.GetPropertyItem(0x5100);
                int delay = BitConverter.ToInt32(item.Value, 4 * frameIndex);
                return Math.Max(10, delay * 10); // 转为毫秒，最小10ms
            }
            catch
            {
                return 100; // 默认100ms
            }
        }

        public static void SetBits(IntPtr hwnd, Bitmap bitmap, int left, int top)
        {
            IntPtr screenDC = Win32Api.GetDC(IntPtr.Zero);
            IntPtr memDC = Win32Api.CreateCompatibleDC(screenDC);
            IntPtr hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
            IntPtr oldBitmap = Win32Api.SelectObject(memDC, hBitmap);

            Win32Api.Size size = new Win32Api.Size(bitmap.Width, bitmap.Height);
            Win32Api.POINT pointSource = new Win32Api.POINT(0, 0);
            Win32Api.POINT topPos = new Win32Api.POINT(left, top);

            Win32Api.BLENDFUNCTION blend = new Win32Api.BLENDFUNCTION
            {
                BlendOp = 0,
                BlendFlags = 0,
                SourceConstantAlpha = 255,
                AlphaFormat = 1
            };

            Win32Api.UpdateLayeredWindow(hwnd, screenDC, ref topPos, ref size, memDC, ref pointSource, 0, ref blend, Win32Api.ULW_ALPHA);

            Win32Api.SelectObject(memDC, oldBitmap);
            Win32Api.DeleteObject(hBitmap);
            Win32Api.DeleteDC(memDC);
            Win32Api.ReleaseDC(IntPtr.Zero, screenDC);
        }
    }
}

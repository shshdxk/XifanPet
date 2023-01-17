using Iplugin.Pet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSystem;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Clock1
{
    public partial class Clock1 : Form
    {
        public Clock1()
        {
            InitializeComponent();
        }

        private Timer timer = new Timer();
        private Boolean through;
        private Bitmap bitmapTime = null;
        private Graphics g;
        private Boolean isShow = false;
        private Font font = new Font("Arial", 16);

        #region 重载

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            isShow = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            //InitializeStylesThrough();
            base.OnHandleCreated(e);
            isShow = true;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                if (through)
                {
                    cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED=
                } 
                else
                {
                    cParms.ExStyle &= 0x00080000; // WS_EX_LAYERED
                }
                return cParms;
            }
        }

        #endregion
        private void InitializeStylesThrough()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private void InitializeStylesRecover()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, false);
            SetStyle(ControlStyles.UserPaint, false);
            UpdateStyles();
        }

        private void Clock1_Load(object sender, EventArgs e)
        {
            //this.Hide();
            //Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            //Win32Api.GetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE);
            timer.Interval = 20;
            timer.Enabled = true;
            timer.Tick += new EventHandler(timerTick);
        }

        void timerTick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String nowStr = now.ToString("HH:mm:ss");
            label1.Text = nowStr;
            if (through)
            {
                if (bitmapTime == null)
                {
                    bitmapTime = new Bitmap(this.Width, this.Height);
                    g = Graphics.FromImage(bitmapTime);
                }
                g.Clear(Color.Transparent);
                g.DrawString(nowStr, font, new SolidBrush(Color.Red), 0, 0);
                SetBits(bitmapTime);
            }
        }


        public void SetBits(Bitmap bitmap)
        {
            IntPtr oldBits = IntPtr.Zero;
            IntPtr screenDC = Win32Api.GetDC(IntPtr.Zero);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr memDc = Win32Api.CreateCompatibleDC(screenDC);

            try
            {
                Win32Api.POINT topLoc = new Win32Api.POINT(Left, Top);
                Win32Api.Size bitMapSize = new Win32Api.Size(this.Width, this.Height);
                Win32Api.BLENDFUNCTION blendFunc = new Win32Api.BLENDFUNCTION();
                Win32Api.POINT srcLoc = new Win32Api.POINT(0, 0);

                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = Win32Api.SelectObject(memDc, hBitmap);

                blendFunc.BlendOp = Win32Api.AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = 255;
                blendFunc.AlphaFormat = Win32Api.AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;
                Win32Api.UpdateLayeredWindow(Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, Win32Api.ULW_ALPHA);

            }
            catch (ObjectDisposedException ignore)
            {
                Console.WriteLine(ignore.ToString());
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    Win32Api.SelectObject(memDc, oldBits);
                    Win32Api.DeleteObject(hBitmap);
                }
                Win32Api.ReleaseDC(IntPtr.Zero, screenDC);
                Win32Api.DeleteDC(memDc);
            }
        }

        public void MouseThrough()
        {
            InitializeStylesThrough();
            this.FormBorderStyle = FormBorderStyle.None;
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            through = true;
            this.isShow = true;
            this.Show();
        }

        public void MouseRecover()
        {
            InitializeStylesRecover();
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, 0x90000);
            through = false;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.isShow = true;
            this.Show();
        }

        private void Clock1_SizeChanged(object sender, EventArgs e)
        {
            this.panel1.Width = this.Width;
            this.panel1.Height= this.Height - 45;
            double width = (this.Width - 20) / 5.62;
            double height = (this.Height - 60) / 1.5;
            double size = Math.Min(width, height);
            font = new Font("Arial", (int)size);
            label1.Font = font;
            if (bitmapTime != null)
            {
                bitmapTime.Dispose();
                bitmapTime = null;
            }
        }
    }
}

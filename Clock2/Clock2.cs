using Newtonsoft.Json;
using PetCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSystem;

namespace Clock2
{
    public partial class Clock2 : Form
    {
        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private PrivateFontCollection pfc = null;
        private Font font = null;
        private String time = null;
        private SolidBrush textSB = new SolidBrush(Color.Red);
        private SolidBrush bachgroundTextSB = new SolidBrush(Color.FromArgb(128, 128, 128, 128));

        private Timer timer = new Timer();
        private Bitmap bitmapTime = null;
        private Graphics g;
        private Setting setting = new Setting();
        private Boolean initing = true;
        private Boolean isMove = false;
        // 鼠标1x坐标
        private int p1x = 0;
        // 鼠标1y坐标
        private int p1y = 0;

        public Clock2()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //InitializeStylesThrough();
            //Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            //Win32Api.RECT rect = Common.ImplantDesktop(this.Handle);
            //this.Left = rect.right - rect.left - 800;
            //this.Top = 30;

            Common.ImplantDesktop(this.Handle);

            //Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            pfc = new PrivateFontCollection();
            byte[] fontBytes = Properties.Resources.digitalDisplay;
            IntPtr fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
            Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
            pfc.AddMemoryFont(fontData, fontBytes.Length);
            Marshal.FreeCoTaskMem(fontData);
            string settingPath = path + @"\setting.json";
            if (File.Exists(settingPath))
            {
                using (StreamReader sr = new StreamReader(settingPath))
                {
                    try
                    {
                        setting = JsonConvert.DeserializeObject<Setting>(sr.ReadToEnd());
                        this.Top = setting.Top;
                        this.Left = setting.Left;
                    }
                    catch
                    {
                    }
                }
            }
            font = ReadFont(40);
            initing = false;
        }

        private Font ReadFont(float size)
        {
            return new Font(pfc.Families[0], (float)size);
        }

        #region 重载


        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {

            InitializeStylesThrough();
            this.FormBorderStyle = FormBorderStyle.None;
            //Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            base.OnHandleCreated(e);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
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

        //        private void InitializeStylesRecover()
        //        {
        //            SetStyle(ControlStyles.AllPaintingInWmPaint, false);
        //            SetStyle(ControlStyles.UserPaint, false);
        //            UpdateStyles();
        //        }

        private void Clock2_Load(object sender, EventArgs e)
        {
            timer.Interval = 20;
            timer.Enabled = true;
            timer.Tick += new EventHandler(timerTick);
        }

        void timerTick(object sender, EventArgs e)
        {
            if (font == null)
            {
                return;
            }
            try
            {
                DateTime now = DateTime.Now;
                String nowStr = now.ToString("HH:mm:ss");
                if (time == nowStr)
                {
                    return;
                }
                time = nowStr;
                if (bitmapTime == null)
                {
                    bitmapTime = new Bitmap(this.Width, this.Height);
                    g = Graphics.FromImage(bitmapTime);
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                }
                g.Clear(Color.Transparent);
                g.DrawString(nowStr, font, bachgroundTextSB, 3, 3);
                g.DrawString(nowStr, font, textSB, 1, 1);

                DrawClock(g);
                SetBits(bitmapTime);
            }
            catch (Exception ignore)
            {
                Console.WriteLine(ignore.ToString());
            }
        }

        private void DrawClock(Graphics g)
        {

        }

        private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
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

        //public void MouseThrough()
        //{
        //    InitializeStylesThrough();
        //    this.FormBorderStyle = FormBorderStyle.None;
        //    Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
        //    through = true;
        //    this.Show();
        //}

        //public void MouseRecover()
        //{
        //    InitializeStylesRecover();
        //    Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, 0x90000);
        //    through = false;
        //    this.FormBorderStyle = FormBorderStyle.None;
        //    this.Show();
        //}

        private void SaveSetting()
        {
            if (initing)
            {
                return;
            }
            string settingPath = path + @"\setting.json";
            using (StreamWriter sw = new StreamWriter(settingPath))
            {
                setting.Top = this.Top;
                setting.Left = this.Left;
                sw.Write(JsonConvert.SerializeObject(setting));
            }
        }

        #region 移动事件
        private void Clock2_MouseDown(object sender, MouseEventArgs e)
        {
            isMove = true;
            p1x = e.X;
            p1y = e.Y;
        }

        private void Clock2_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
            SaveSetting();
        }

        private void Clock2_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                this.Left = this.Left + e.X - p1x;
                this.Top = this.Top + e.Y - p1y;
            }
        }

        #endregion

    }
}

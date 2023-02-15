using Clock1.Properties;
using Iplugin.Pet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

namespace DesktopCalendar
{
    public partial class DesktopCalendar : Form
    {
        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        PrivateFontCollection pfc = null;
        private Font font = null;

        private Timer timer = new Timer();
        private Boolean through;
        private Bitmap bitmapTime = null;
        private Graphics g;
        private Graphics gp = null;
        private Boolean isMove = false;
        // 鼠标1x坐标
        private int p1x = 0;
        // 鼠标1y坐标
        private int p1y = 0;

        private Setting setting = new Setting();
        Boolean initing = true;

        public DesktopCalendar()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {

            InitializeStylesThrough();
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            IntPtr dWnd = Win32Api.FindWindow("Progman", null);
            //if (dWnd != IntPtr.Zero)
            //{
            //    Win32Api.SendMessage(dWnd, 0x052c, 0, 0);
            //}
            IntPtr pWnd = Win32Api.FindWindowEx(dWnd, 0, "SHELLDLL_DefVIew", null);
            pWnd = Win32Api.FindWindowEx(pWnd, 0, "SysListView32", null);
            if (pWnd == IntPtr.Zero)
            {
                Boolean first = true;
                dWnd = IntPtr.Zero;
                do
                {
                    if (dWnd != IntPtr.Zero && first)
                    {
                        first = false;
                    }

                    dWnd = Win32Api.FindWindowEx(IntPtr.Zero, (uint)dWnd.ToInt64(), "WorkerW", null);
                    pWnd = Win32Api.FindWindowEx(dWnd, 0, "SHELLDLL_DefVIew", null);
                    pWnd = Win32Api.FindWindowEx(pWnd, 0, "SysListView32", null);
                } while (dWnd != IntPtr.Zero && pWnd == IntPtr.Zero);
                //if (dWnd != IntPtr.Zero)
                //{
                //    Win32Api.SendMessage(dWnd, 0x052c, 0, 0);
                //}
            }
            //pWnd = IntPtr.Zero;
            Console.WriteLine(Win32Api.GetDesktopWindow());
            SetDesktop(dWnd, this.Handle, pWnd);
            font = ReadFont(40);

            //bitmapTime = new Bitmap(800, 600);
            //g = Graphics.FromImage(bitmapTime);
            //g.TextRenderingHint = TextRenderingHint.AntiAlias;
        }

        private void SetDesktop(IntPtr deskTopPtr, IntPtr child, IntPtr parent)
        {
            Win32Api.SendMessage(deskTopPtr, 0x052c, 0, 0);
            Win32Api.SetParent(child, parent);
        }

        #region 重载

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //    base.OnClosing(e);
        //}

        //        protected override void OnHandleCreated(EventArgs e)
        //        {

        //            //InitializeStylesThrough();
        //            //this.FormBorderStyle = FormBorderStyle.None;
        //            //Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
        //            base.OnHandleCreated(e);
        //        }

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

        private void Clock1_Load(object sender, EventArgs e)
        {
            timer.Interval = 100;
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
                if (bitmapTime == null)
                {

                    bitmapTime = new Bitmap(800, 600);
                    g = Graphics.FromImage(bitmapTime);
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                }
                DrawCalendar();
                SetBits(bitmapTime);
                //if (through)
                //{
                //    g.Clear(Color.Transparent);
                //    g.DrawString(nowStr, font, new SolidBrush(Color.Red), 0, 0);
                //    SetBits(bitmapTime);
                //}
                //else
                //{
                //    g.Clear(Color.White);
                //    g.DrawString(nowStr, font, new SolidBrush(Color.Red), 0, 0);
                //    gp.DrawImage(bitmapTime, 0, 0);
                //}
            }
            catch (Exception ignore)
            {
                Console.WriteLine(ignore.ToString());
            }

        }

        private void DrawCalendar()
        {
            g.Clear(Color.Transparent);
            g.DrawString("1234567", font, new SolidBrush(Color.Red), 0, 0);

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

        private Font ReadFont(float size)
        {
            return new Font("Arial", size);
        }

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
                setting.Width = this.Width;
                setting.Height = this.Height;
                sw.Write(JsonConvert.SerializeObject(setting));
            }
        }


    }
}

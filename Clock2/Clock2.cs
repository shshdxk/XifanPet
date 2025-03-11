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
        private string time = null;
        private SolidBrush textSB = new SolidBrush(Color.White);
        private string[] weeks = new string[] { "日", "一", "二", "三", "四", "五", "六" };
        private Bitmap backend = null;

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
        // 颜色定义
        Color bgColor = Color.FromArgb(88, 101, 242);  // 紫色背景
        Color clockColor = Color.Black;  // 黑色表盘
        Color textColor = Color.White;  // 白色文本
        private SolidBrush highlightColor = new SolidBrush(Color.DeepSkyBlue); // 蓝色刻度
        private SolidBrush circleAccent = new SolidBrush(Color.Fuchsia); // 粉色圆形
        // 分针
        private Pen handPenm = new Pen(Color.White, 2);
        // 秒针
        private Pen handPens = new Pen(Color.White, 1.5f);
        // 时针
        private Font hourFont = new Font("Arial", 10, FontStyle.Bold);
        private SolidBrush textColorB = new SolidBrush(Color.White); // 蓝色刻度
        // 日期
        private Font dayFont = new Font("Arial", 12);

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
            if (backend == null)
            {
                backend = new Bitmap(400, 200);
                DrawClockBack();
            }
            try
            {
                DateTime now = DateTime.Now;
                string nowStr = now.ToString("HH:mm:ss");
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
                g.DrawImage(backend, 0, 0);
                DrawClock(g, now);
                Common.SetBits(Handle, bitmapTime, Left, Top);
            }
            catch (Exception ignore)
            {
                Console.WriteLine(ignore.ToString());
            }
        }

        private void DrawClock(Graphics g, DateTime date)
        {
            DateTime now = date;
            string nowStr = now.ToString("HH:mm:ss");

            g.DrawString(nowStr, font, textSB, 170, 70);

            string d = now.ToString("yyyy-MM-dd 星期") + weeks[(int)now.DayOfWeek];
            g.DrawString(d, dayFont, textSB, 200, 125);

            // 绘制黑色表盘
            int centerX = 94, centerY = 81, radius = 68;

            int hour = now.Hour;
            int minute = now.Minute;
            int second = now.Second;

            double angleHour = (hour % 12 + minute / 60.0) * 30 * Math.PI / 180;
            double angleMinute = (minute + second / 60.0) * 6 * Math.PI / 180;
            double angleSecond = second * 6 * Math.PI / 180;
            double length = radius * 0.8;

            // 绘制时针
            double hx = centerX + radius * 0.6 * Math.Sin(angleHour);
            double hy = centerY - radius * 0.6 * Math.Cos(angleHour);
            g.DrawLine(handPenm, centerX, centerY, (float)hx, (float)hy);
            float dx = hour > 9 ? 10f : 5f;
            g.FillEllipse(circleAccent, (float)(hx - 12), (float)(hy - 12), 24, 24);
            g.DrawString(hour + "", hourFont, textColorB, (float)(hx - dx), (float)(hy - 6.6666f));


            // 绘制分针
            double mx1 = centerX - radius * 0.1 * Math.Sin(angleMinute);
            double my1 = centerY + radius * 0.1 * Math.Cos(angleMinute);
            double mx2 = centerX + radius * 0.7 * Math.Sin(angleMinute);
            double my2 = centerY - radius * 0.7 * Math.Cos(angleMinute);
            g.DrawLine(handPenm, (float)mx1, (float)my1, (float)mx2, (float)my2);

            // 绘制秒针
            double sx1 = centerX - radius * 0.1 * Math.Sin(angleSecond);
            double sy1 = centerY + radius * 0.1 * Math.Cos(angleSecond);
            double sx2 = centerX + length * Math.Sin(angleSecond);
            double sy2 = centerY - length * Math.Cos(angleSecond);
            g.DrawLine(handPens, (float)sx1, (float)sy1, (float)sx2, (float)sy2);
            g.FillEllipse(highlightColor, centerX - 3, centerY - 3, 6, 6);
        }

        /// <summary>
        /// 绘制背景
        /// </summary>
        private void DrawClockBack()
        {
            Graphics g = Graphics.FromImage(backend);
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            // 绘制背景框
            Rectangle bgRect = new Rectangle(8, 50, 390, 120);
            g.DrawRoundedRectanglePath(new SolidBrush(bgColor), bgRect, 10);

            // 绘制黑色表盘
            int centerX = 94, centerY = 81, radius = 68;
            g.FillEllipse(new SolidBrush(clockColor), centerX - radius, centerY - radius, radius * 2, radius * 2);
            Pen pen1 = new Pen(textColor, 2);
            Pen pen2 = new Pen(textColor, (float)1.5);
            // 绘制刻度（60 个短刻度）
            for (int i = 0; i < 60; i++)
            {
                double angle = i * 6 * Math.PI / 180;
                double length1 = i % 5 == 0 ? 0.86 : 0.93;
                double length2 = i % 5 == 0 ? 1.05 : 1;
                int x1 = centerX + (int)(radius * length1 * Math.Cos(angle));
                int y1 = centerY + (int)(radius * length1 * Math.Sin(angle));
                int x2 = centerX + (int)(radius * length2 * Math.Cos(angle));
                int y2 = centerY + (int)(radius * length2 * Math.Sin(angle));

                Pen tickPen = i % 5 == 0 ? pen1 : pen2;
                g.DrawLine(tickPen, x1, y1, x2, y2);
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

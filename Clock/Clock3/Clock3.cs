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

namespace Clock3
{
    public partial class Clock3 : Form
    {
        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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

        // 加载资源图片
        private Bitmap watchDial = null;
        private Bitmap hourHand = null;
        private Bitmap minuteHand = null;
        public Clock3()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {

            //Common.ImplantDesktop(this.Handle);

            watchDial = new Bitmap(path + @"\Resource\watchDial.png");
            hourHand = new Bitmap(path + @"\Resource\hourHand.png");
            minuteHand = new Bitmap(path + @"\Resource\minuteHand.png");
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
            return new Font("Arial", (float)size);
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
                cParms.ExStyle |= 0x00080080; // WS_EX_LAYERED
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
                backend = new Bitmap(308, 348);
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

        /// <summary>
        /// 绘制背景
        /// </summary>
        private void DrawClockBack()
        {
            if (watchDial != null)
            {
                Graphics g = Graphics.FromImage(backend);
                // 绘制表盘
                g.DrawImage(watchDial, 0, 0, this.Width, this.Height);
            }
        }

        private void DrawClock(Graphics g, DateTime date)
        {

            // 获取当前时间
            int hour = date.Hour;
            int minute = date.Minute;
            int second = date.Second;

            // 计算时针和分针的角度
            double hourAngle = ((double)hour % 12 + (double)minute / 60.0) * 30 - 132.5; // 每小时30度
            double minuteAngle = minute * 6 + second / 10 - 48; // 每分钟6度

            if (hourHand != null)
            {
                // 绘制时针
                g.TranslateTransform(154, 153); // 将原点移到表盘中心
                g.RotateTransform((float)hourAngle); // 旋转时针
                g.DrawImage(hourHand, -35, -35); // 绘制时针
                g.ResetTransform(); // 重置变换
            }

            if (minuteHand != null)
            {
                // 绘制分针
                g.TranslateTransform(154, 153); // 将原点移到表盘中心
                g.RotateTransform((float)minuteAngle); // 旋转分针
                g.DrawImage(minuteHand, -23, -68); // 绘制分针
                g.ResetTransform(); // 重置变换
            }
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

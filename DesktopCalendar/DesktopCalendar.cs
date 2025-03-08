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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSystem;

namespace DesktopCalendar
{
    public partial class DesktopCalendar : Form
    {
        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private string[] weeks = new string[] { "日", "一", "二", "三", "四", "五", "六" };
        private PrivateFontCollection pfc = null;
        private SolidBrush backgroundPen = new SolidBrush(Color.FromArgb(80, 240, 240, 240));
        private SolidBrush backgroundNowPen = new SolidBrush(Color.FromArgb(180, 0, 240, 0));
        private Font monthFont = null;
        private SolidBrush montuSB = new SolidBrush(Color.Black);
        private Font dayFont = null;
        private SolidBrush day5SB = new SolidBrush(Color.DarkBlue);
        private SolidBrush day2SB = new SolidBrush(Color.Red);
        private SolidBrush dayOutSB = new SolidBrush(Color.FromArgb(80, 67, 94, 62));
        private Font chinaDayFont = null;
        private SolidBrush chinaDaySB = new SolidBrush(Color.FromArgb(255, 67, 94, 62));
        private Font weekFont = null;
        private SolidBrush weekSB = new SolidBrush(Color.DeepSkyBlue);

        private DateTime oldTime = DateTime.Now;

        private IntPtr pWnd = IntPtr.Zero;
        private Timer timer = new Timer();
        private Bitmap bitmapTime = null;
        private Graphics g;
        private Setting setting = new Setting();
        private Boolean initing = true;

        public DesktopCalendar()
        {
            InitializeComponent();
            //if (!Init())
            //{
                Init();
            //}
        }

        private void Init()
        {
            // 找到Progman类型的窗口句柄
            // 调用EnumWindows找到Peogman上面第一层类名为WorkerW的窗口句柄
            // 将本窗体嵌入这个窗口
            // https://www.freesion.com/article/2801344904/
            InitializeStylesThrough();
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            Win32Api.RECT rect = Common.ImplantDesktop(this.Handle);
            this.Left = rect.right - rect.left - 800;
            this.Top = 30;
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
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
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

        private void DesktopCalendar_Load(object sender, EventArgs e)
        {
            pfc = new PrivateFontCollection();
            byte[] fontBytes = Properties.Resources.LoremIpsum;
            IntPtr fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
            Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
            pfc.AddMemoryFont(fontData, fontBytes.Length);
            Marshal.FreeCoTaskMem(fontData);

            ReadFont();
            DrawCalendar();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += new EventHandler(timerTick);
        }

        private void ReadFont()
        {
            monthFont = new Font("Arial", 40);
            weekFont = new Font("Arial", 25);
            dayFont = new Font(pfc.Families[0], 20, FontStyle.Bold);
            chinaDayFont = new Font("Arial", 10, FontStyle.Bold);
        }

        void timerTick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (oldTime.Day != now.Day)
            {
                DrawCalendar();
                oldTime = now;
            }
        }

        private void DrawCalendar()
        {
            if (bitmapTime == null)
            {

                bitmapTime = new Bitmap(800, 600);
                g = Graphics.FromImage(bitmapTime);
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            g.Clear(Color.Transparent);
            DateTime now = DateTime.Now;
            int nowMonth = now.Month;
            int nowDay = now.Day;
            g.DrawString(now.ToString("yyyy年"), monthFont, montuSB, -10, 0);
            g.DrawString(now.ToString("MM月"), monthFont, montuSB, 420, 0);
            for (int i = 0; i < 7; i++)
            {
                Rectangle rg = new Rectangle(i * 80, 57, 60, 40);
                using (GraphicsPath gp = CreateRoundedRectanglePath(rg, 3))
                {
                    g.FillPath(backgroundPen, gp);
                }
                g.DrawString(weeks[i], weekFont, weekSB, i * 80 + 7, 60);
            }
            DateTime mFirst = now.AddDays(1 - nowDay);
            DateTime mLast = now.AddDays(1 - nowDay).AddMonths(1).AddDays(-1);
            DateTime first = mFirst.AddDays(-((int)mFirst.DayOfWeek));
            DateTime last = mLast.AddDays(6 - ((int)mLast.DayOfWeek));
            int count = (int) (last - first).TotalDays + 1;
            for (int i = 0; i < count; i++)
            {
                DateTime time = first.AddDays(i);
                int line = i / 7;
                int cell = i % 7;
                int day = time.Day;
                int offset = 17;
                if (day < 10)
                {
                    offset = 22;
                }
                Rectangle rg = new Rectangle(cell * 80, 110 + line * 60, 60, 50);
                using (GraphicsPath gp = CreateRoundedRectanglePath(rg, 3))
                {
                    if (day == nowDay && nowMonth == time.Month)
                    {
                        g.FillPath(backgroundNowPen, gp);
                    }
                    else
                    {
                        g.FillPath(backgroundPen, gp);
                    }
                }
                if (nowMonth == time.Month)
                {
                    if (cell == 0 || cell == 6)
                    {
                        g.DrawString(day + "", dayFont, day2SB, cell * 80 + offset, 110 + line * 60);
                    }
                    else
                    {
                        g.DrawString(day + "", dayFont, day5SB, cell * 80 + offset, 110 + line * 60);
                    }
                    g.DrawString(ChinaDate.GetDay(time), chinaDayFont, chinaDaySB, cell * 80 + 10, 140 + line * 60);
                }
                else
                {
                    g.DrawString(day + "", dayFont, dayOutSB, cell * 80 + offset, 110 + line * 60);
                    g.DrawString(ChinaDate.GetDay(time), chinaDayFont, dayOutSB, cell * 80 + 10, 140 + line * 60);
                }
            }

            Common.SetBits(Handle, bitmapTime, Left, Top);
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
                setting.Width = this.Width;
                setting.Height = this.Height;
                sw.Write(JsonConvert.SerializeObject(setting));
            }
        }

    }
}

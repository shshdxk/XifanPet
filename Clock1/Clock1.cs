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

namespace Clock1
{
    public partial class Clock1 : Form
    {
        string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private PrivateFontCollection pfc = null;
        private SolidBrush textSB = new SolidBrush(Color.Red);
        private SolidBrush bachgroundTextSB = new SolidBrush(Color.FromArgb(128, 128, 128, 128));
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

        public Clock1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
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
                        if (setting.Width < 50)
                        {
                            setting.Width = 50;
                        }
                        if (setting.Height < 60)
                        {
                            setting.Height = 60;
                        }
                        this.Top = setting.Top;
                        this.Left = setting.Left;
                        this.Width = setting.Width;
                        this.Height = setting.Height;
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                font = ReadFont(20);
            }
            initing = false;
        }

        #region 重载

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            //InitializeStylesThrough();
            base.OnHandleCreated(e);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                if (through)
                {
                    cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
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
                if (bitmapTime == null)
                {
                    bitmapTime = new Bitmap(this.Width, this.Height);
                    g = Graphics.FromImage(bitmapTime);
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    gp = panel1.CreateGraphics();
                }
                if (through)
                {
                    g.Clear(Color.Transparent);
                    g.DrawString(nowStr, font, bachgroundTextSB, 3, 3);
                    g.DrawString(nowStr, font, textSB, 1, 1);
                    SetBits(bitmapTime);
                }
                else
                {
                    g.Clear(Color.White);
                    g.DrawString(nowStr, font, bachgroundTextSB, 3, 3);
                    g.DrawString(nowStr, font, textSB, 1, 1);
                    gp.DrawImage(bitmapTime, 0, 0);
                }
            }
            catch (Exception ignore)
            {
                Console.WriteLine(ignore.ToString());
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
            this.Show();
        }

        public void MouseRecover()
        {
            InitializeStylesRecover();
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, 0x90000);
            through = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Show();
        }

        private void Clock1_SizeChanged(object sender, EventArgs e)
        {
            this.panel1.Width = this.Width;
            this.panel1.Height = this.Height - 22;
            double width = this.Width / 5.8;
            double height = (this.Height - statusStrip1.Height) / 1.5;
            double size = Math.Min(width, height);
            if (font != null)
            {
                font.Dispose();
                font = null;
            }
            font = ReadFont((float)size);
            if (bitmapTime != null)
            {
                bitmapTime.Dispose();
                bitmapTime = null;
                g.Dispose();
                gp.Dispose();
                gp = null;
                g = null;
            }
            panelBottom.Top = this.Height - 3;
            panelBottom.Width = this.Width - 10;
            panelRight.Left = this.Width - 3;
            panelRight.Height = this.Height - 10;
            panelN.Left = this.Width - 10;
            panelN.Top = this.Height - 10;

        }
        private Font ReadFont(float size)
        {
            return new Font(pfc.Families[0], (float)size);
        }

        #region panel1移动事件
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                this.Left = this.Left + e.X - p1x;
                this.Top = this.Top + e.Y - p1y;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
            SaveSetting();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            isMove = true;
            p1x = e.X;
            p1y = e.Y;
        }
        #endregion

        #region panelRight移动事件

        private void panelRight_MouseDown(object sender, MouseEventArgs e)
        {
            isMove = true;
            p1x = e.X;
        }

        private void panelRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                this.Width = this.Width + e.X - p1x;
            }
        }

        private void panelRight_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
            SaveSetting();
        }
        #endregion

        #region panelBottom移动事件
        private void panelBottom_MouseDown(object sender, MouseEventArgs e)
        {
            isMove = true;
            p1y = e.Y;
        }

        private void panelBottom_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
            SaveSetting();
        }

        private void panelBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                this.Height = this.Height + e.Y - p1y;
            }
        }
        #endregion

        #region panelN移动事件
        private void panelN_MouseDown(object sender, MouseEventArgs e)
        {
            isMove = true;
            p1x = e.X;
            p1y = e.Y;
        }

        private void panelN_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
            SaveSetting();
        }

        private void panelN_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                this.Height = this.Height + e.Y - p1y;
                this.Width = this.Width + e.X - p1x;
            }
        }
        #endregion
    
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

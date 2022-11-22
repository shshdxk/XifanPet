using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using WinSystem;
using System.IO;
using System.Reflection;
using Rep;
using Iplugin.Pet;

namespace XifanPet
{
    public partial class MainForm : Form
    {
        IPet pet = null;
        ActionResource actionResource = null;
        Point oldPoint = new Point(0, 0);
        bool mouseDown = false;
        bool haveHandle = false;
        Timer timerSpeed = new Timer();
        // 最大移动距离
        int MaxCount = 50;
        // 当前移动距离
        int count = 0;
        // x轴速度
        float stepX = 2f;
        // y轴速度
        float stepY = 0f;
        float left = 0f, top = 0f;

        Random r = new Random();

        bool toRight = true;        //是否向右

        public MainForm()
        {
            InitializeComponent();
            top = Screen.PrimaryScreen.WorkingArea.Height / 2f;

            timerSpeed.Interval = 20;
            timerSpeed.Enabled = true;
            timerSpeed.Tick += new EventHandler(timerSpeed_Tick);

            this.MouseDown += new MouseEventHandler(Form2_MouseDown);
            this.MouseUp += new MouseEventHandler(Form2_MouseUp);
            this.MouseMove += new MouseEventHandler(Form2_MouseMove);
            //SetPenetrate();
        }

        #region 重载

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            haveHandle = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            InitializeStyles();
            base.OnHandleCreated(e);
            haveHandle = true;
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

        void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            count = 0;
            MaxCount = r.Next(70) + 40;
            timerSpeed.Interval = r.Next(20) + 2;
            //speedMode = true;
            mouseDown = false;
        }

        private void InitializeStyles()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            UpdateStyles();
        }
        float y = 0f;
        int countN = 0;
        void timerSpeed_Tick(object sender, EventArgs e)
        {
            if (actionResource == null)
            {
                return;
            }
            if (!mouseDown)
            {
                count++;
                if (count > MaxCount)
                {
                    MaxCount = r.Next(70) + 30;

                    count = 0;
                    stepX = (float)r.NextDouble() * 3f + 1f;
                    stepY = (float)r.NextDouble() * 2f;
                    if (stepY < 0.3f) stepY = 0f;
                    int s = r.Next(2);
                    countN++;
                    stepY = (s == 0 ? -1 : 1) * stepY;
                    y += stepY;
                    //Console.WriteLine(y + "  " + stepY + "  " + (y / countN));
                }
                left = (left + (toRight ? 1 : -1) * stepX);
                top = (top + stepY);
                FixLeftTop();
                this.Left = (int)left;
                this.Top = (int)top;
            }
            actionResource.GetFrame(toRight);
            SetBits(actionResource.GetPic(toRight));
        }

        private void FixLeftTop()
        {
            if (toRight && left > 1000)
            {
                toRight = false;
                count = 0;
            }
            else if (!toRight && left < 100)
            {
                toRight = true;
                count = 0;
            }
            if (top < 100)
            {
                stepY = 1f;
                count = 0;
            }
            else if (top > 700)
            {
                stepY = -1f;
                count = 0;
            }
        }

        void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Left += (e.X - oldPoint.X);
                this.Top += (e.Y - oldPoint.Y);
                left = this.Left;
                top = this.Top;
                FixLeftTop();
            }
        }

        void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                oldPoint = e.Location;
                mouseDown = true;
            }
        }

        public void SetBits(Bitmap bitmap)
        {
            if (!haveHandle) return;

            if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");

            IntPtr oldBits = IntPtr.Zero;
            IntPtr screenDC = Win32Api.GetDC(IntPtr.Zero);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr memDc = Win32Api.CreateCompatibleDC(screenDC);

            try
            {
                Win32Api.POINT topLoc = new Win32Api.POINT(Left, Top);
                Win32Api.Size bitMapSize = new Win32Api.Size(bitmap.Width, bitmap.Height);
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

        private void FishForm_Load(object sender, EventArgs e)
        {
            //Win32Api.GetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE);
            //Console.WriteLine(Win32Api.GetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE));
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            //Console.WriteLine(Win32Api.GetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE));
            l = Life.getInstance();
            DynamicMenu.LoadAllPlugs(this.contextMenuStripIcon);
            DynamicPet.LoadAllPets();
            InitPet();
        }

        private void InitPet()
        {
            List<string> petTypes = DynamicPet.GetPetTypes();
            if (petTypes.Count == 0)
            {
                return;
            }
            pet = DynamicPet.GetPet(petTypes[0]);
            actionResource = pet.GetAction();
            toRight = actionResource.Right;
            left = -actionResource.Width;
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DynamicMenu.closeAllPlugins();
            this.Dispose();
        }

        private void 穿透ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
        }

        private void 恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, 0x90000);
        }

        private Life l = null;

        private void 吃ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int p = l.EatDrink.EatPoint;
            l.EatDrink.EatPoint = p + 100;

            Console.WriteLine("饱食度：" + l.EatDrink.EatPoint + "，清洁度：" + l.Clean.CleanPoint);
        }

        private void 洗ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int p = l.Clean.CleanPoint;
            l.Clean.CleanPoint = p + 100;

            Console.WriteLine("饱食度：" + l.EatDrink.EatPoint + "，清洁度：" + l.Clean.CleanPoint);
        }

        private void FishForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
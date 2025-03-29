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
using Iplugin;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetCommon;

namespace XifanPet
{
    public partial class MainForm : Form
    {
        private string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //    base.OnClosing(e);
        //    haveHandle = false;
        //}

        //protected override void OnHandleCreated(EventArgs e)
        //{
        //    InitializeStyles();
        //    base.OnHandleCreated(e);
        //    haveHandle = true;
        //}

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
            if (actionResource.Walk)
            {
                count = 0;
                MaxCount = r.Next(70) + 40;
                timerSpeed.Interval = r.Next(20) + 2;
            }
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
            if (actionResource.Walk && !mouseDown)
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
            Common.SetBits(Handle, bitmap, Left, Top);
        }

        private void FishForm_Load(object sender, EventArgs e)
        {
            InitializeStyles();
            //Win32Api.GetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE);
            //Console.WriteLine(Win32Api.GetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE));
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            //Console.WriteLine(Win32Api.GetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE));
            l = Life.getInstance();
            DynamicMenu.LoadAllPlugs(this.contextMenuStripIcon);
            DynamicPet.LoadAllPets();
            InitPet();
            InitSetting();
            haveHandle = true;
        }

        private void InitPet()
        {
            List<string> petTypes = DynamicPet.GetPetTypes();
            if (petTypes.Count == 0)
            {
                return;
            }
            foreach (IPet ipet in DynamicPet.GetPets().Values)
            {
                ToolStripMenuItem menu = new ToolStripMenuItem(ipet.GetAction().Name);
                menu.Tag = ipet;
                menu.Click += new EventHandler(PetMenuClick);
                宠物ToolStripMenuItem.DropDownItems.Add(menu);
            }
            ((ToolStripMenuItem)宠物ToolStripMenuItem.DropDownItems[0]).Checked = true;
            pet = DynamicPet.GetPet(petTypes[0]);
            actionResource = pet.GetAction();
            toRight = actionResource.Right;
        }

        /// <summary>
        /// 宠物菜单的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PetMenuClick(object sender, EventArgs e)
        {
            if (宠物ToolStripMenuItem.DropDownItems.Count <= 1)
            {
                return;
            }
            foreach (ToolStripMenuItem menu in 宠物ToolStripMenuItem.DropDownItems)
            {
                menu.Checked = false;
            }
            ToolStripMenuItem petMeun = (ToolStripMenuItem)sender;
            petMeun.Checked = true;
            IPet ipet = (IPet)petMeun.Tag;
            petMeun.Checked = true;
            pet = ipet;
            actionResource = pet.GetAction();
            toRight = actionResource.Right;
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            DynamicMenu.CloseAllPlugins();
            this.Dispose();
        }

        private void 穿透ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DynamicMenu.Through = true;
            穿透ToolStripMenuItem.Checked = true;
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, Win32Api.WS_EX_TRANSPARENT | Win32Api.WS_EX_LAYERED);
            foreach (IPetPlug plugin in DynamicMenu.GetUsedPlugins().Values)
            {
                plugin.MouseThrough();
            }
            SettingManager.SaveSetting(true);
        }

        private void 恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DynamicMenu.Through = false;
            穿透ToolStripMenuItem.Checked = false;
            Win32Api.SetWindowLong(this.Handle, Win32Api.GWL_EXSTYLE, 0x90000);
            foreach (IPetPlug plugin in DynamicMenu.GetUsedPlugins().Values)
            {
                plugin.MouseRecover();
            }
            SettingManager.SaveSetting(false);
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

        private void InitSetting()
        {
            Setting setting = SettingManager.ReadSetting();
            DynamicMenu.Through = setting.Through;
            if (setting.Through)
            {
                穿透ToolStripMenuItem.Checked = true;
            }
            else
            {
                穿透ToolStripMenuItem.Checked = false;
            }
            if (setting.Plugins != null)
            {
                foreach (string item in setting.Plugins)
                {
                    if (DynamicMenu.GetAllPlugins().ContainsKey(item))
                    {
                        DynamicMenu.OpenPlugin(item);
                    }
                }
            }
            SettingManager.initing = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            haveHandle = false;
            SettingManager.SaveSetting(null);
        }

        private void pluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPlugins formPlugins = new FormPlugins();
            formPlugins.ShowDialog();
        }

    }
}
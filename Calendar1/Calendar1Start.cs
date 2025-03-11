using System;
using System.Collections.Generic;
using System.Text;
using Iplugin;

namespace Calendar1
{
    public class Calendar1Start : PetPlug
    {
        private Calendar1 mp = null;
        private Menu[] menus = { };
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialization() {
            menus = new Menu[]{ new Menu("日历", 10), new Menu("桌面日历1", 1) };
        }
        /// <summary>
        /// 打开插件
        /// </summary>
        public override void OpenPlug()
        {
            if (mp == null || mp.IsDisposed)
            {
                mp = new Calendar1();
            }
            //mp.Show();
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public override Menu[] GetMenu()
        {
            return menus;
        }
        /// <summary>
        /// 关闭插件
        /// </summary>
        public override void Close()
        {
            if (mp != null && !mp.IsDisposed)
            {
                mp.Close();
                mp.Dispose();
            }
        }

        public override void MouseThrough()
        {
            //mp.MouseThrough();
            mp.Show();
        }

        public override void MouseRecover()
        {
            //mp.MouseRecover();
            mp.Show();
        }
    }
}

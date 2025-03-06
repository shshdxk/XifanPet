using System;
using System.Collections.Generic;
using System.Text;
using Iplugin;

namespace Clock2
{
    public class Clock2Start : PetPlug
    {
        private Clock2 mp = null;
        private Menu[] menus = { };
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialization() {
            menus = new Menu[]{ new Menu("时钟", 10), new Menu("数字时钟", 1), new Menu("时钟2", 2) };
        }
        /// <summary>
        /// 打开插件
        /// </summary>
        public override void OpenPlug()
        {
            if (mp == null || mp.IsDisposed)
            {
                mp = new Clock2();
            }
            mp.Show();
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
            mp.Show();
        }

        public override void MouseRecover()
        {
            mp.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Iplugin;

namespace Clock3
{
    public class Clock3Start : PetPlug
    {
        private Clock3 mp = null;
        private Menu[] menus = { };
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialization() {
            menus = new Menu[]{ new Menu("时钟", 10), new Menu("数字时钟", 1), new Menu("时钟3", 3) };
        }
        /// <summary>
        /// 打开插件
        /// </summary>
        public override void OpenPlug()
        {
            if (mp == null || mp.IsDisposed)
            {
                mp = new Clock3();
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

using System;
using System.Collections.Generic;
using System.Text;
using Iplugin;

namespace FileDescription
{
    public class FileDescriptionStart : PetPlug
    {
        private ClosedCallback callback = null;
        private Form1 mp = null;
        private Menu[] menus = { };
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Initialization() {
            menus = new Menu[]{ new Menu("工具", 9999), new Menu("文件描述", 2) };
        }
        /// <summary>
        /// 打开插件
        /// </summary>
        public override void OpenPlug()
        {
            if (mp == null || mp.IsDisposed)
            {
                mp = new Form1(callback, this);
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
            }
        }

        public override void Closed(ClosedCallback callback)
        {
            this.callback = callback;
        }

    }
}

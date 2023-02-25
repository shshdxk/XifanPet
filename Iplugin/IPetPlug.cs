using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Iplugin
{
    public interface IPetPlug
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Initialization();
        /// <summary>
        /// 打开插件
        /// </summary>
        void OpenPlug();
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        Menu[] GetMenu();
        /// <summary>
        /// 鼠标穿透动作
        /// </summary>
        void MouseThrough();
        /// <summary>
        /// 鼠标恢复动作
        /// </summary>
        void MouseRecover();
        /// <summary>
        /// 关闭时调用
        /// </summary>
        void Close();

        /// <summary>
        /// 插件关闭回调
        /// </summary>
        /// <param name="callback"></param>
        void Closed(PetPlug.ClosedCallback callback);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iplugin.Pet
{
    public interface IPet
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Initialization();

        /// <summary>
        /// 获取动作资源
        /// </summary>
        /// <returns></returns>
        ActionResource GetAction();

    }
}

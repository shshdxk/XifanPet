using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSystem;

namespace PetCommon
{
    public class Common
    {
        /// <summary>
        /// 将窗口嵌入桌面
        /// </summary>
        /// <param name="intPtr">需要嵌入的窗口</param>
        /// <returns>桌面大小位置</returns>
        public static Win32Api.RECT ImplantDesktop(IntPtr intPtr)
        {
            Win32Api.RECT rect = new Win32Api.RECT();
            IntPtr dWnd = Win32Api.FindWindow("Progman", null);
            if (dWnd != IntPtr.Zero)
            {
                IntPtr pWnd = Win32Api.FindWindowEx(dWnd, 0, "SHELLDLL_DefView", null);
                if (pWnd != IntPtr.Zero)
                {
                    Win32Api.SendMessage(dWnd, 0x052c, 0, 0);
                    Win32Api.SetParent(intPtr, pWnd);
                    Win32Api.GetWindowRect(pWnd, out rect);
                }
            }
            return rect;
        }
    }
}

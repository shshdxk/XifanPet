using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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

        /// <summary>
        /// 将bitmap设置到窗口上
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="bitmap"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public static void SetBits(IntPtr handle, Bitmap bitmap, int left, int top)
        {
            IntPtr oldBits = IntPtr.Zero;
            IntPtr screenDC = Win32Api.GetDC(IntPtr.Zero);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr memDc = Win32Api.CreateCompatibleDC(screenDC);

            try
            {
                Win32Api.POINT topLoc = new Win32Api.POINT(left, top);
                Win32Api.Size bitMapSize = new Win32Api.Size(bitmap.Width, bitmap.Height);
                Win32Api.BLENDFUNCTION blendFunc = new Win32Api.BLENDFUNCTION();
                Win32Api.POINT srcLoc = new Win32Api.POINT(0, 0);

                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = Win32Api.SelectObject(memDc, hBitmap);

                blendFunc.BlendOp = Win32Api.AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = 255;
                blendFunc.AlphaFormat = Win32Api.AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;
                Win32Api.UpdateLayeredWindow(handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, Win32Api.ULW_ALPHA);

            }
            catch (ObjectDisposedException)
            {
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

    }
}

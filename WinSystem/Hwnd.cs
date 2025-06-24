using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace WinSystem
{
    /// <summary>
    /// 句柄操作
    /// </summary>
    public class Hwnd
    {
        /// <summary>
        /// 获取焦点所在窗口句柄
        /// </summary>
        /// <param name="hwnd">焦点所在窗口或父窗口句柄</param>
        /// <returns>焦点所在窗口句柄</returns>
        public static IntPtr getFocusByhwnd(IntPtr hwnd)
        {
            int idAttachTo = AppDomain.GetCurrentThreadId();
            int pid = 0;
            int idAttach = Win32Api.GetWindowThreadProcessId(hwnd, out pid);
            Win32Api.AttachThreadInput(idAttach, idAttachTo, true);
            IntPtr nowFocus = Win32Api.GetFocus();
            Win32Api.AttachThreadInput(idAttach, idAttachTo, false);
            return nowFocus;
        }

        /// <summary>
        /// 判断焦点是否在指定窗口句柄
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns>焦点在指定句柄true，焦点不在指定句柄false</returns>
        public static bool isFocus(IntPtr hwnd)
        {
            return hwnd!=IntPtr.Zero && hwnd == getFocusByhwnd(hwnd);
        }

        /// <summary>
        /// 判断鼠标点击的是否是指定窗口句柄
        /// </summary>
        /// <param name="intptr">指定窗口句柄</param>
        /// <param name="ef">松开鼠标事件数据</param>
        /// <param name="eb">按下鼠标事件数据</param>
        /// <param name="rect">点击确认范围</param>
        /// <returns>true:是 false:否</returns>
        public static bool isClickHwndIn(IntPtr intptr, MouseEventArgs ef, MouseEventArgs eb, Win32Api.RECT rect)
        {

            Win32Api.RECT recta;
            Win32Api.GetWindowRect(intptr, out recta);
            ////int t2 = rect.top + 48;
            Win32Api.RECT rect1;
            rect1.left = rect.left < 0 ? recta.left : (recta.left + rect.left);
            rect1.right = rect.right < 0 ? recta.right : (recta.left + rect.right);
            rect1.top = rect.top < 0 ? recta.top : (recta.top + rect.top);
            rect1.bottom = rect.bottom < 0 ? recta.bottom : (recta.top + rect.bottom); ;
            return (ef.X <= rect1.right && ef.X >= rect1.left && ef.Y <= rect1.bottom && ef.Y >= rect1.top &&
                eb.X <= rect1.right && eb.X >= rect1.left && eb.Y <= rect1.bottom && eb.Y >= rect1.top &&
                intptr == Win32Api.WindowFromPoint(new Point(ef.X, ef.Y)));

            //return (MouseButtons.Left.Equals(e.Button) || MouseButtons.Right.Equals(e.Button))
            //    && intptr == Win32Api.WindowFromPoint(new System.Drawing.Point(e.X, e.Y))
            //    && isFocus(intptr);
        }

        /// <summary>
        /// 判断鼠标点击的是否是指定窗口句柄
        /// </summary>
        /// <param name="intptr">指定窗口句柄</param>
        /// <param name="e">鼠标事件数据</param>
        /// <returns>true:是 false:否</returns>
        public static bool isClickHwnd(IntPtr intptr, System.Windows.Forms.MouseEventArgs e)
        {

            return (MouseButtons.Left.Equals(e.Button) || MouseButtons.Right.Equals(e.Button))
                && intptr == Win32Api.WindowFromPoint(new System.Drawing.Point(e.X, e.Y))
                && isFocus(intptr);
        }

        /// <summary>
        /// 查找窗口标题是以指定字符串开头的窗口
        /// </summary>
        /// <param name="titleStart">要比较的字符串</param>
        /// <returns>窗口句柄</returns>
        public static IntPtr FindFormStartWith(string titleStart)
        {
            string title="";
            return FindFormStartWith(titleStart,out title);
        }

        /// <summary>
        /// 查找窗口标题是以指定字符串开头的窗口
        /// </summary>
        /// <param name="titleStart">要比较的字符串</param>
        /// <param name="title">窗口完整标题</param>
        /// <returns>窗口句柄</returns>
        public static IntPtr FindFormStartWith(string titleStart,out string title)
        {
            IntPtr intptr = IntPtr.Zero;
            do
            {
                intptr = Win32Api.FindWindowEx(IntPtr.Zero, (uint)intptr, null, null);
                StringBuilder sb = new StringBuilder(256);
                Win32Api.GetWindowText(intptr, sb, 256);
                title = sb.ToString();
                if (title.StartsWith(titleStart))
                    return intptr;
            } while (intptr != IntPtr.Zero);
            return IntPtr.Zero;
        }

        /// <summary>
        /// 根据句柄截取图像并保存成指定文件（bmp格式），图像可以是被窗口挡着的，但对非GDI程序无效
        /// </summary>
        /// <param name="intptr">窗口句柄</param>
        /// <param name="filePath">保存路径</param>
        public static void GetWindowCapture(IntPtr intptr, string filePath)
        {
            GetWindowCapture(intptr).Save(filePath, ImageFormat.Bmp);
        }
        /// <summary>
        /// 根据句柄截取图像，图像可以是被窗口挡着的，但对非GDI程序无效
        /// </summary>
        /// <param name="intptr">窗口句柄</param>
        public static Bitmap GetWindowCapture(IntPtr intptr)
        {
            IntPtr hscrdc = Win32Api.GetWindowDC(intptr);
            Win32Api.RECT windowRect = new Win32Api.RECT();
            Win32Api.GetWindowRect(intptr, out windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            IntPtr hbitmap = Win32Api.CreateCompatibleBitmap(hscrdc, width, height);
            IntPtr hmemdc = Win32Api.CreateCompatibleDC(hscrdc);
            Win32Api.SelectObject(hmemdc, hbitmap);
            Win32Api.PrintWindow(intptr, hmemdc, 0);
            Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
            Win32Api.DeleteDC(hscrdc);//删除用过的对象
            Win32Api.DeleteDC(hmemdc);//删除用过的对象
            return bmp;
        }

        /// <summary>
        /// 寻找多个窗口中，第一个出现的窗口（用于等待多个窗口中的一个出现），若多窗口同时存在，随机返回一个
        /// </summary>
        /// <param name="title">窗口标题，最多支持二级窗口</param>
        /// <param name="outTime">超时时间</param>
        /// <returns>第几个窗口，失败返回-1</returns>
        private static int FindOneFormCount(List<string[]> title, int outTime)
        {
            startTime = DateTime.Now;
            thn = -1;
            //intp = IntPtr.Zero;
            loco1 = title;
            sleeper = new Thread[loco1.Count];
            for (int i = 0; i < loco1.Count; i++)
            {
                sleeper[i] = new Thread(new ParameterizedThreadStart(SleepThread));
                sleeper[i].IsBackground = true;
            }
            for (int i = 0; i < sleeper.Length; i++)
            {
                sleeper[i].Start(i);
            }
            Thread.Sleep(100);
            stoper = new Thread(
                delegate() {
                    int time = (int)outTime - (DateTime.Now - startTime).Milliseconds;
                    Thread.Sleep(time > 0 ? time : 1);
                    for (int i = 0; i < sleeper.Length; i++)
                    {
                        if (sleeper[i].IsAlive) sleeper[i].Abort();
                    }
                }
                );
            stoper.IsBackground = true;
            //int time = outTime - 100;
            //stoper.Start(time>0?time:0);
            stoper.Start(outTime);
            foreach (string[] ss in loco1)
            {
                lock (ss) { }
            }
            if (stoper.IsAlive) stoper.Abort();
            return thn;
        }
        private static List<string[]> loco1 = new List<string[]>();
        private static string str = "";
        private static int thn = -1;
        private static DateTime startTime;
        private static Thread[] sleeper;
        private static Thread stoper;
        private static void SleepThread(object p)
        {
            int i = (int)p;
            lock (loco1[i])
            {
                string[] name = (string[])loco1[i];
                IntPtr pay_hwnd = IntPtr.Zero;
                while (thn == -1 && pay_hwnd == IntPtr.Zero)
                {
                    pay_hwnd = IntPtr.Zero;
                    if (name.Length == 1)
                    {
                        pay_hwnd = Win32Api.FindWindowEx(IntPtr.Zero, 0, null, name[0]);
                    }
                    else if (name.Length == 2)
                    {
                        if (name[0].Equals(""))
                        {
                            IntPtr inp = IntPtr.Zero;
                            bool start = false;
                            while ((inp != IntPtr.Zero || !start) && pay_hwnd == IntPtr.Zero)
                            {
                                if (thn != -1) break;
                                start = true;
                                inp = Win32Api.FindWindowEx(IntPtr.Zero, (uint)inp, null, name[0]);
                                pay_hwnd = Win32Api.FindWindowEx(inp, 0, null, name[1]);
                            }
                        }
                        else
                        {
                            pay_hwnd = Win32Api.FindWindowEx(IntPtr.Zero, 0, null, name[0]);
                            pay_hwnd = Win32Api.FindWindowEx(pay_hwnd, 0, null, name[1]);
                        }
                    }
                    if (thn == -1)
                        Thread.Sleep(100);
                }
                if (pay_hwnd != IntPtr.Zero)
                {
                    lock (str)
                    {
                        if (thn == -1)
                        {
                            thn = i;
                        }
                    }
                }
            }
        }
    }
}

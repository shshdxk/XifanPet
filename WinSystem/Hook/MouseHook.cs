using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;


namespace WinSystem.Hook
{
    /// <summary>
    /// 鼠标钩子
    /// </summary>
    public class MouseHook
    {
        int hHook;

        Win32Api.HookProc MouseHookDelegate;
        //记录鼠标点下的坐标
        Point point = new Point();
        //上一次鼠标事件
        private MouseEventArgs eb;
        //上一次按下时间
        private long beforeTime;
        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPITAL = 0x14;
        //鼠标按下与松开的时候的两个句柄
        private IntPtr mouse_down_hw = IntPtr.Zero;
        private IntPtr mouse_up_hw = IntPtr.Zero;
        /// <summary>
        /// 双击超时时间
        /// </summary>
        private int outTime = Win32Api.GetDoubleClickTime()*10000;
        //是否是双击
        private bool isDoubleL = false;
        private int dX = Win32Api.GetSystemMetrics(Win32Api.SM_CXDOUBLECLK) >> 1;
        private int dY = Win32Api.GetSystemMetrics(Win32Api.SM_CYDOUBLECLK) >> 1;
        //按下时的鼠标钩子结构体
        private Win32Api.MouseHookStruct mouseHookStructDown;

        #region 事件定义
        /// <summary>
        /// 鼠标事件
        /// </summary>
        public event MouseEventHandler OnMouseActivity;
        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        public event MouseEventHandler OnMouseDoubleClickBActivity;
        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        public event MouseEventHandler OnMouseDoubleClickFActivity;
        /// <summary>
        /// 一次点击事件（不区分按下松开事件）
        /// </summary>
        public event MouseEventBFHandler OnMouseBFActivity;
        /// <summary>
        /// 一次点击事件（区分按下松开事件）
        /// </summary>
        /// <param name="sender">事件来源</param>
        /// <param name="ef">完成的事件</param>
        /// <param name="eb">前一次事件</param>
        public delegate void MouseEventBFHandler(object sender, MouseEventArgs ef, MouseEventArgs eb);
        /////<summary>
        /////鼠标更新事件
        /////</summary>
        /////<remarks>当鼠标移动或者滚轮滚动时触发</remarks>
        //public event MouseEventHandler OnMouseUpdate;
        /////<summary>
        /////按键按下事件
        /////</summary>
        //public event KeyEventHandler OnKeyDown;
        /////<summary>
        /////按键按下并释放事件
        /////</summary>
        //public event KeyPressEventHandler OnKeyPress;
        /////<summary>
        /////按键释放事件
        /////</summary>
        //public event KeyEventHandler OnKeyUp;

        #endregion 事件定义
        /// <summary>
        /// 鼠标钩子
        /// </summary>
        public MouseHook() { }
        /// <summary>
        /// 注入全局鼠标钩子
        /// </summary>
        /// <returns></returns>
        public int SetHook()
        {
            beforeTime = 0L;
            MouseHookDelegate = new Win32Api.HookProc(MouseHookProc);
            Process cProcess = Process.GetCurrentProcess();
            ProcessModule cModule = cProcess.MainModule;
            var mh = Win32Api.GetModuleHandle(cModule.ModuleName);
            //IntPtr mh = new IntPtr(1705710);
            //var mh = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);
            hHook = Win32Api.SetWindowsHookEx(Win32Api.WH_MOUSE_LL, MouseHookDelegate, mh, 0);
            return hHook;
        }
        /// <summary>
        /// 卸载鼠标钩子
        /// </summary>
        public void UnHook()
        {
            Win32Api.UnhookWindowsHookEx(hHook);
        }
        
        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //if (nCode >= 0)
            //{
            //    Win32Api.MouseHookStruct MyMouseHookStruct = (Win32Api.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.MouseHookStruct));

            //    KeyEventArgs e = new MouseEventHandler((POINT)MyMouseHookStruct.pt);
            //    OnMouseMoveEvent(this.e);
            //}
            if ((nCode >= 0) && (OnMouseActivity != null || OnMouseBFActivity != null || OnMouseDoubleClickBActivity != null || OnMouseDoubleClickFActivity != null))
            {
                Win32Api.MouseHookStruct mouseHookStruct = (Win32Api.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.MouseHookStruct));
                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                int clickCount = 0;
                long now = 0L;
                switch (wParam)
                {
                        //左键点击判断，若按下与抬起是同一个句柄则，认为是一次点击
                    case (int)Win32Api.WM_LBUTTONDOWN:
                        point.X = mouseHookStruct.pt.x;
                        point.Y = mouseHookStruct.pt.y;
                        mouse_down_hw = Win32Api.WindowFromPoint(point);
                        eb = new MouseEventArgs(button, clickCount, mouseHookStruct.pt.x, mouseHookStruct.pt.y, mouseDelta);
                        //需要双击事件时判断双击事件
                        if (OnMouseDoubleClickBActivity != null || OnMouseDoubleClickFActivity != null)
                        {
                            now = DateTime.Now.ToFileTime();
                            //判断双击时间间隔和坐标范围
                            if (now - beforeTime < outTime && mouse_down_hw == mouse_up_hw
                                && mouseHookStruct.pt.x >= mouseHookStructDown.pt.x - dX
                                && mouseHookStruct.pt.x <= mouseHookStructDown.pt.x + dX
                                && mouseHookStruct.pt.y >= mouseHookStructDown.pt.y - dY
                                && mouseHookStruct.pt.y <= mouseHookStructDown.pt.y + dY
                                )
                            {
                                isDoubleL = true;
                                //有按下双击检测
                                if (OnMouseDoubleClickBActivity != null)
                                {
                                    clickCount = 2;
                                    button = MouseButtons.Left;
                                    beforeTime = 0L;
                                }
                            }
                            else
                            {
                                //不是双击事件重置点击时间
                                beforeTime = DateTime.Now.ToFileTime();
                                isDoubleL = false;
                            }
                        }
                        break;
                    case Win32Api.WM_LBUTTONUP:
                        point.X = mouseHookStruct.pt.x;
                        point.Y = mouseHookStruct.pt.y;
                        mouse_up_hw = Win32Api.WindowFromPoint(point);

                        if (mouse_down_hw == mouse_up_hw)
                        {
                            mouseHookStructDown = mouseHookStruct;
                            if (isDoubleL)
                            { //有松开双击检测
                                if (OnMouseDoubleClickFActivity != null)
                                {
                                    clickCount = 2;
                                    button = MouseButtons.Left;
                                    beforeTime = 0L;
                                }
                            }
                            else
                            {
                                clickCount = 1;
                                button = MouseButtons.Left;
                            }
                        }
                        break;
                    case (int)Win32Api.WM_RBUTTONDOWN:
                        button = MouseButtons.Right;
                        break;
                    case (int)Win32Api.WM_MBUTTONDOWN:
                        button = MouseButtons.Middle;
                        break;
                    case (int)Win32Api.WM_MOUSEWHEEL:
                        mouseDelta = (short)((mouseHookStruct.hwnd >> 16) & 0xffff);
                        break;
                }
                MouseEventArgs e = new MouseEventArgs(button, clickCount, mouseHookStruct.pt.x, mouseHookStruct.pt.y, mouseDelta);
                if (isDoubleL)
                {
                    //按下双击回调
                    if (OnMouseDoubleClickBActivity != null && Win32Api.WM_LBUTTONDOWN == wParam)
                    {
                        OnMouseDoubleClickBActivity(this, e);
                    }
                    //松开双击回调
                    if (OnMouseDoubleClickFActivity != null && Win32Api.WM_LBUTTONUP == wParam)
                    {
                        OnMouseDoubleClickFActivity(this, e);
                    }
                }
                else
                {
                    //区分按下和松开的单击事件
                    if (clickCount == 1 && OnMouseBFActivity != null)
                    {
                        OnMouseBFActivity(this, e, eb);
                    }
                    //其他鼠标事件
                    if (OnMouseActivity != null)
                    {
                        OnMouseActivity(this, e);
                    }
                }
            }

            return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
        }
    }
}

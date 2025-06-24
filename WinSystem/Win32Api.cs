using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WinSystem
{
    /// <summary>
    /// 调用系统底层函数
    /// </summary>
    public class Win32Api
    {

        #region 常数和结构
        #region 常数
        /// <summary>
        /// 非系统键按下
        /// </summary>
        public const int WM_KEYDOWN = 0x100;
        /// <summary>
        /// 非系统键松开
        /// </summary>
        public const int WM_KEYUP = 0x101;
        /// <summary>
        /// 系统键按下
        /// </summary>
        public const int WM_SYSKEYDOWN = 0x104;
        /// <summary>
        /// 非系统键松开
        /// </summary>
        public const int WM_SYSKEYUP = 0x105;
        /// <summary>
        /// 键盘敲击
        /// </summary>
        public const int WH_KEYBOARD_LL = 13;
        /// <summary>
        /// 全局鼠标事件
        /// </summary>
        public const int WH_MOUSE_LL = 14;
        /// <summary>
        /// 显示
        /// </summary>
        public const int WS_SHOWNORMAL = 1;
        /// <summary>
        /// 移动鼠标时发生，同WM_MOUSEFIRST
        /// </summary>
        public const int WM_MOUSEMOVE = 0x200;
        /// <summary>
        /// 按下鼠标左键
        /// </summary>
        public const int WM_LBUTTONDOWN = 0x201;
        /// <summary>
        /// 释放鼠标左键
        /// </summary>
        public const int WM_LBUTTONUP = 0x202;
        /// <summary>
        /// 双击鼠标左键
        /// </summary>
        public const int WM_LBUTTONDBLCLK = 0x203;
        /// <summary>
        /// 按下鼠标右键
        /// </summary>
        public const int WM_RBUTTONDOWN = 0x204;
        /// <summary>
        ///  释放鼠标右键
        /// </summary>
        public const int WM_RBUTTONUP = 0x205;
        /// <summary>
        /// 双击鼠标右键
        /// </summary>
        public const int WM_RBUTTONDBLCLK = 0x206;
        /// <summary>
        /// 按下鼠标中键
        /// </summary>
        public const int WM_MBUTTONDOWN = 0x207;
        /// <summary>
        /// 鼠标滚轮滚动
        /// </summary>
        public const int WM_MOUSEWHEEL = 0x020A;
        /// <summary>
        /// 单选复选选中状态
        /// </summary>
        public const int BM_GETCHECK = 0x00F0;
        /// <summary>
        /// 通道叠加
        /// </summary>
        public const byte AC_SRC_OVER = 0;
        /// <summary>
        /// 开启α混合
        /// </summary>
        public const Int32 ULW_ALPHA = 2;
        /// <summary>
        /// α通道叠加值
        /// </summary>
        public const byte AC_SRC_ALPHA = 1;
        /// <summary>
        /// 使窗体支持透明ModifyStyleEx(0, WS_EX_LAYERED);效果
        /// </summary>
        public const uint WS_EX_LAYERED = 0x80000;
        /// <summary>
        /// 半透明窗口
        /// </summary>
        public const int WS_EX_TRANSPARENT = 0x20;
        /// <summary>
        /// 设定一个新的窗口风格。
        /// </summary>
        public const int GWL_STYLE = (-16);
        /// <summary>
        /// 设定一个新的扩展风格。
        /// </summary>
        public const int GWL_EXSTYLE = (-20);
        /// <summary>
        /// 表示把窗体设置成半透明样式
        /// </summary>
        public const int LWA_ALPHA = 0;

        #region 系统配置编号
        //public const int SM_CXSCREEN = 0;
        //public const int SM_CYSCREEN = 1;
        //public const int SM_CXVSCROLL = 2;
        //public const int SM_CYHSCROLL = 3;
        //public const int SM_CYCAPTION = 4;
        //public const int SM_CXBORDER = 5;
        //public const int SM_CYBORDER = 6;
        //public const int SM_CXDLGFRAME = 7;
        //public const int SM_CYDLGFRAME = 8;
        //public const int SM_CYHTHUMB = 9;
        //public const int SM_CXHTHUMB = 10;
        //public const int SM_CXICON = 1;
        //public const int SM_CYICON = 12;
        //public const int SM_CXCURSOR = 13;
        //public const int SM_CYCURSOR = 14;
        //public const int SM_CYMENU = 15;
        //public const int SM_CXFULLSCREEN = 16;
        //public const int SM_CYFULLSCREEN = 17;
        //public const int SM_CYKANJIWINDOW = 18;
        //public const int SM_MOUSEPRESENT = 19;
        //public const int SM_CYVSCROLL = 20;
        //public const int SM_CXHSCROLL = 21;
        //public const int SM_DEBUG = 22;
        //public const int SM_SWAPBUTTON = 23;
        //public const int SM_CXMIN = 28;
        //public const int SM_CYMIN = 29;
        //public const int SM_CXSIZE = 30;
        //public const int SM_CYSIZE = 31;
        //public const int SM_CXMINTRACK = 34;
        //public const int SM_CYMINTRACK = 35;
        /// <summary>
        /// 双击矩形区域的宽
        /// </summary>
        public const int SM_CXDOUBLECLK = 36;
        /// <summary>
        /// 双击矩形区域的高
        /// </summary>
        public const int SM_CYDOUBLECLK = 37;
        //public const int SM_CXICONSPACING = 38;
        //public const int SM_CYICONSPACING = 39;
        //public const int SM_MENUDROPALIGNMENT = 40;
        #endregion

        #endregion

        #region 键盘
        /// <summary>
        /// 声明键盘钩子的封送结构类型 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardHookStruct
        {
            /// <summary>
            /// 表示一个在1到254间的虚似键盘码
            /// </summary>
            public int vkCode;
            /// <summary>
            /// 表示硬件扫描码 
            /// </summary>
            public int scanCode;
            /// <summary>
            /// 
            /// </summary>
            public int flags;
            /// <summary>
            /// 
            /// </summary>
            public int time;
            /// <summary>
            /// 
            /// </summary>
            public int dwExtraInfo;
        }
        #endregion

        #region 鼠标
        /// <summary>
        /// 鼠标坐标结构类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// x像素
            /// </summary>
            public Int32 x;
            /// <summary>
            /// y像素
            /// </summary>
            public Int32 y;
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public POINT(Int32 x, Int32 y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// 鼠标钩子结构类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseHookStruct
        {
            /// <summary>
            /// 坐标
            /// </summary>
            public POINT pt;
            /// <summary>
            /// 句柄
            /// </summary>
            public int hwnd;
            /// <summary>
            /// 
            /// </summary>
            public int wHitTestCode;
            /// <summary>
            /// 
            /// </summary>
            public int dwExtraInfo;
        }
        /// <summary>
        /// 区域结构类型
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// 左端位置
            /// </summary>
            public int left;
            /// <summary>
            /// 顶端位置
            /// </summary>
            public int top;
            /// <summary>
            /// 右端位置
            /// </summary>
            public int right;
            /// <summary>
            /// 下端位置
            /// </summary>
            public int bottom;
        }

        /// <summary>
        /// 窗口大小
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            /// <summary>
            /// 窗口x轴宽度
            /// </summary>
            public Int32 cx;
            /// <summary>
            /// 窗口y轴高度
            /// </summary>
            public Int32 cy;
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public Size(Int32 x, Int32 y)
            {
                cx = x;
                cy = y;
            }
        }

        /// <summary>
        /// 颜色通道
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            /// <summary>
            /// BlendOp
            /// </summary>
            public byte BlendOp;
            /// <summary>
            /// BlendFlags
            /// </summary>
            public byte BlendFlags;
            /// <summary>
            /// SourceConstantAlpha
            /// </summary>
            public byte SourceConstantAlpha;
            /// <summary>
            /// AlphaFormat
            /// </summary>
            public byte AlphaFormat;
        }

        #endregion
        #endregion


        #region Api
        /// <summary>
        /// 钩子委托函数
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        /// <summary>
        /// 安装钩子的函数 
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="lpfn"></param>
        /// <param name="hInstance"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        /// <summary>
        /// 卸下钩子的函数 
        /// </summary>
        /// <param name="idHook"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// 下一个钩挂的函数 
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        /// <summary>
        /// 下一个钩挂的函数 
        /// </summary>
        /// <param name="idHook"></param>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 转成Ascii编码
        /// </summary>
        /// <param name="uVirtKey"></param>
        /// <param name="uScanCode"></param>
        /// <param name="lpbKeyState"></param>
        /// <param name="lpwTransKey"></param>
        /// <param name="fuState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        /// <summary>
        /// 获取键盘状态
        /// </summary>
        /// <param name="pbKeyState"></param>
        /// <returns></returns>
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        /// <summary>
        /// 获取一个应用程序或动态链接库的模块句柄
        /// </summary>
        /// <param name="lpModuleName"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        /// <summary>
        /// 获取窗口句柄
        /// </summary>
        /// <param name="lpClassName">窗口类名</param>
        /// <param name="lpWindowName">窗口标题</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        ///  获取窗口句柄
        /// </summary>
        /// <param name="hwndParent">父窗口句柄</param>
        /// <param name="hwndChildAfter">上一个窗口句柄</param>
        /// <param name="lpszClass">窗口类名</param>
        /// <param name="lpszWindow">窗口标题</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, uint hwndChildAfter, string lpszClass, string lpszWindow);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="wMsg">消息1</param>
        /// <param name="wParam">消息2</param>
        /// <param name="lParam">消息值</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="wMsg">消息1</param>
        /// <param name="wParam">消息2</param>
        /// <param name="lParam">消息值</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessageStr(IntPtr hwnd, uint wMsg, int wParam, string lParam);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="wMsg">消息</param>
        /// <param name="wParam">消息值1</param>
        /// <param name="lParam">消息值2</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessageStrB(IntPtr hwnd, uint wMsg, int wParam, StringBuilder lParam);

        /// <summary>
        /// 创建指定窗口的线程设置到前台，并且激活该窗口，对非自身程序无效
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        /// <summary>
        /// 获取窗口类名
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="text">类名返回值</param>
        /// <param name="MaxCount">类名最大长度</param>
        [DllImport("User32.dll")]
        public static extern void GetClassName(IntPtr hwnd, StringBuilder text, int MaxCount);

        /// <summary>
        /// 获取窗口标题
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="text">标题返回值</param>
        /// <param name="MaxLen">标题最大长度</param>
        [DllImport("User32.dll")]
        public static extern void GetWindowText(IntPtr hwnd, StringBuilder text, int MaxLen);

        /// <summary>
        /// 设置窗口层次
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="cmdShow">层次</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hwnd, int cmdShow);

        /// <summary>
        /// 获取窗口大小
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="lpRect">区域</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, out RECT lpRect);

        /// <summary>
        /// 获取指定坐标处窗口句柄
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns>窗口句柄</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);

        /// <summary>
        /// 获取活跃窗口句柄，对非自身程序无效
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// 获取指定窗口线程ID，out进程ID
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="pid">线程ID</param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int pid);

        /// <summary>
        /// 线程注入、卸载
        /// </summary>
        /// <param name="idAttach">要注入的线程ID</param>
        /// <param name="idAttachTo">当前线程ID</param>
        /// <param name="fAttach">行为 true：注入，false：卸载</param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "AttachThreadInput")]
        public static extern int AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);

        /// <summary>
        /// 获取焦点所在句柄
        /// </summary>
        /// <returns>窗口句柄</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr GetFocus();

        /// <summary>
        /// 获取系统鼠标双击时间间隔（单位：毫秒）
        /// </summary>
        /// <returns>鼠标双击时间间隔 毫秒</returns>
        [DllImport("user32.dll", EntryPoint = "GetDoubleClickTime")]
        public extern static int GetDoubleClickTime();

        /// <summary>
        /// 得到被定义的系统数据或者系统配置信息
        /// </summary>
        /// <param name="intnIndex">配置编码</param>
        /// <returns>配置信息</returns>
        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public extern static int GetSystemMetrics(int intnIndex);

        /// <summary>
        /// 创建驱动设备环境
        /// </summary>
        /// <param name="lpszDriver">驱动名</param>
        /// <param name="lpszDevice">设备名</param>
        /// <param name="lpszOutput">输出</param>
        /// <param name="lpInitData">输入</param>
        /// <returns>设备上下文环境的句柄</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateDC(string lpszDriver,string lpszDevice,string lpszOutput,IntPtr lpInitData);
        /// <summary>
        /// 获取窗口的设备环境
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <returns>设备上下文环境的句柄</returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        /// <summary>
        /// 该函数创建一个与指定设备兼容的内存设备上下文环境（DC）
        /// </summary>
        /// <param name="hdc">设备上下文环境的句柄</param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        /// <summary>
        /// 检索一指定窗口的客户区域或整个屏幕的显示设备上下文环境的句柄，以后可以在GDI函数中使用该句柄来在设备上下文环境中绘图
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        /// 创建与指定的设备环境相关的设备兼容的位图
        /// </summary>
        /// <param name="hdc">设备上下文环境的句柄</param>
        /// <param name="width">图像宽</param>
        /// <param name="height">图像高</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int width, int height);

        /// <summary>
        /// 选择一对象到指定的设备上下文环境中
        /// </summary>
        /// <param name="hdc">设备上下文环境的句柄</param>
        /// <param name="hgdiobj">被选择的对象的句柄</param>
        /// <returns>如果选择对象不是区域并且函数执行成功，那么返回值是被取代的对象的句柄；如果选择对象是区域并且函数执行成功</returns>
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hdc,IntPtr hgdiobj);

        /// <summary>
        /// 释放设备上下文环境（DC）
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hDC"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        /// <summary>
        /// 删除指定的设备上下文环境
        /// </summary>
        /// <param name="hdc">设备上下文环境的句柄</param>
        /// <returns>成功:非0 失败:0</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteDC(IntPtr hdc);
       
        /// <summary>
        /// 截取非屏幕区的窗口
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hdcBlt">被选择的对象的句柄</param>
        /// <param name="nFlags">可选标志，指定绘图选项</param>
        /// <returns>成功:非0 失败:0</returns>
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, UInt32 nFlags);

        /// <summary>
        /// 删除一个逻辑笔、画笔、字体、位图、区域或者调色板，释放所有与该对象有关的系统资源，在对象被删除之后，指定的句柄也就失效了
        /// 注释：当一个绘画对象（如笔或画笔）当前被选入一个设备上下文环境时不要删除该对象。当一个调色板画笔被删除时，与该画笔相关的位图并不被删除，该图必须单独地删除。
        /// </summary>
        /// <param name="hObj"></param>
        /// <returns>成功，返回非零值；如果指定的句柄无效或者它已被选入设备上下文环境，则返回值为零</returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteObject(IntPtr hObj);

        /// <summary>
        /// 更新一个分层窗口的位置，大小，形状，内容和半透明度
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hdcDst"></param>
        /// <param name="pptDst"></param>
        /// <param name="psize"></param>
        /// <param name="hdcSrc"></param>
        /// <param name="pptSrc"></param>
        /// <param name="crKey"></param>
        /// <param name="pblend"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref POINT pptDst, ref Size psize, IntPtr hdcSrc, ref POINT pptSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);
        /// <summary>
        /// 此函数用于设置分层窗口透明度，常和 UpdateLayeredWindow 函数结合使用。
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="crKey"></param>
        /// <param name="bAlpha"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);
        /// <summary>
        /// 根据世界转换修改区域
        /// </summary>
        /// <param name="lpXform"></param>
        /// <param name="nCount"></param>
        /// <param name="rgnData"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr ExtCreateRegion(IntPtr lpXform, uint nCount, IntPtr rgnData);
        /// <summary>
        /// 改变指定窗口的属性．函数也将指定的一个32位值设置在窗口的额外存储空间的指定偏移位置
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);
        /// <summary>
        /// 获得指定窗口的有关信息，函数也获得在额外窗口内存中指定偏移位地址的32位度整型值
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "GetWindowLong")]
        public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

        /// <summary>
        /// 获取桌面句柄
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 设置嵌套窗口
        /// </summary>
        /// <param name="hWndChild"></param>
        /// <param name="hWndNewParent"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        /// <summary>
        /// 窗口句柄枚举回调
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);

        /// <summary>
        /// 列举窗口所有句柄
        /// </summary>
        /// <param name="callPtr"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int EnumWindows(EnumWindowsCallback callPtr, int lParam);
        #endregion

    }
}
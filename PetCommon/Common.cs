using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinSystem;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing.Imaging;

namespace PetCommon
{
    public class Common
    {

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

                hBitmap = bitmap.GetHbitmap(System.Drawing.Color.FromArgb(0));
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

        /// <summary>
        /// 支持 ImageSharp Image<Rgba32> 的 SetBits 重载
        /// </summary>
        public static void SetBits(IntPtr handle, Image<Rgba32> image, int left, int top)
        {
            using (var bitmap = ImageSharpToBitmap(image))
            {
                SetBits(handle, bitmap, left, top);
            }
        }

        /// <summary>
        /// ImageSharp Image<Rgba32> 转 Bitmap
        /// </summary>
        public static Bitmap ImageSharpToBitmap(Image<Rgba32> image)
        {
            var bmp = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            var rect = new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height);
            var data = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            try
            {
                // 不使用 unsafe，直接用 ImageSharp 的 CopyPixelDataTo(byte[])
                int bytes = Math.Abs(data.Stride) * data.Height;
                byte[] pixelBytes = new byte[bytes];
                image.CopyPixelDataTo(pixelBytes);
                System.Runtime.InteropServices.Marshal.Copy(pixelBytes, 0, data.Scan0, bytes);
            }
            finally
            {
                bmp.UnlockBits(data);
            }
            return bmp;
        }
    }
}

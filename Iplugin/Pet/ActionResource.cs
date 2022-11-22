using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iplugin.Pet
{
    public class ActionResource
    {

        private int frame = 0;

        /// <summary>
        /// 全部帧的图片(向左)
        /// </summary>
        private List<Bitmap> leftPics = new List<Bitmap>();
        /// <summary>
        /// 全部帧的图片(向右)
        /// </summary>
        private List<Bitmap> rightPics = new List<Bitmap>();

        /// <summary>
        /// 获取指定帧图片
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Bitmap GetPic(Boolean right)
        {
            if (right)
            {
                return rightPics[frame % rightPics.Count];
            } 
            else
            {
                return leftPics[frame % leftPics.Count];
            }
        }

        /// <summary>
        /// 每一帧图片宽度
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// 每一帧图片高度
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// 是否是向右的图片
        /// </summary>
        public Boolean Right { get; }

        /// <summary>
        /// 第几帧
        /// </summary>
        public int GetFrame(Boolean right)
        {
            int f = this.frame;
            if (right)
            {
                this.frame = (this.frame + 1) % this.rightPics.Count;

            }
            else
            {
                this.frame = (this.frame + 1) % this.leftPics.Count;
            }
            return f;
        }

        public ActionResource(List<Bitmap> leftPics, List<Bitmap> rightPics, int width, int height, bool right)
        {
            this.leftPics = leftPics;
            this.rightPics = rightPics;
            Width = width;
            Height = height;
            Right = right;
        }
    }
}

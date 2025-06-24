using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Iplugin.Pet
{
    public class ActionResource
    {

        private int frame = 0;

        /// <summary>
        /// 全部帧的图片(向左)
        /// </summary>
        private List<Image<Rgba32>> leftPics = new List<Image<Rgba32>>();
        /// <summary>
        /// 全部帧的图片(向右)
        /// </summary>
        private List<Image<Rgba32>> rightPics = new List<Image<Rgba32>>();

        public string Name { get; }

        /// <summary>
        /// 获取指定帧图片
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public Image<Rgba32> GetPic(Boolean right)
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

        public Boolean Walk { get; set; }
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

        public ActionResource(string name, List<Image<Rgba32>> leftPics, List<Image<Rgba32>> rightPics, int width, int height, bool right)
        {
            this.Name = name;
            this.leftPics = leftPics;
            this.rightPics = rightPics;
            Width = width;
            Height = height;
            Right = right;
            Walk = true;
        }
    }
}

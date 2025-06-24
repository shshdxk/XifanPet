using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Iplugin.Pet;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Fish
{
    public class FishPet : IPet
    {
        public ActionResource actionResource;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialization()
        {
            int height = 84, width = 143;
            bool isRight = true;
            string imagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "pic", "pic.png");
            using var image = Image.Load<Rgba32>(imagePath);
            List<Image<Rgba32>> rightPics = new List<Image<Rgba32>>();
            List<Image<Rgba32>> leftPics = new List<Image<Rgba32>>();
            for (int widthStart = 0; widthStart + width <= image.Width; widthStart += width)
            {
                // 右向帧
                var rightFrame = image.Clone(ctx => ctx.Crop(new Rectangle(widthStart, 0, width, height)));
                rightPics.Add(rightFrame);
                // 左向帧（水平翻转）
                var leftFrame = rightFrame.Clone(ctx => ctx.Flip(FlipMode.Horizontal));
                leftPics.Add(leftFrame);
            }
            actionResource = new ActionResource("鱼", leftPics, rightPics, width, height, isRight);
        }

        /// <summary>
        /// 获取动作资源
        /// </summary>
        /// <returns></returns>
        public ActionResource GetAction()
        {
            return actionResource;
        }
    }
}

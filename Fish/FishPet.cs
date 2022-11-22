using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Iplugin.Pet;
using static System.Net.Mime.MediaTypeNames;

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
            Boolean isRight = true;
            Bitmap image = new Bitmap(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\pic\\pic.png");
            List<Bitmap> rightPics = new List<Bitmap>();
            List<Bitmap> leftPics = new List<Bitmap>();
            for (int widthStart = 0; widthStart < image.Width; widthStart += width)
            {
                Bitmap rightPic = new Bitmap(width, height);
                Graphics.FromImage(rightPic).DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(widthStart, 0, width, height), GraphicsUnit.Pixel);
                rightPics.Add(rightPic);


                Bitmap leftPic = new Bitmap(width, height);
                Graphics.FromImage(leftPic).DrawImage(image, new Rectangle(0, 0, width, height), new Rectangle(widthStart+width, 0, -width, height), GraphicsUnit.Pixel);
                leftPics.Add(leftPic);
            }
            actionResource = new ActionResource(leftPics, rightPics, width, height, isRight);
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

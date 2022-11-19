using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XifanPet
{
    public static class Resource
    {

        public static Boolean inited = false;
        public static Bitmap leftP;
        public static Bitmap rightP;

        public static void init()
        {
            if (inited)
            {
                return;
            }
            DirectoryInfo picDir = new DirectoryInfo(Application.StartupPath + @"\pic");
            if (!picDir.Exists)
            {
                Console.WriteLine("no images");
            }
            leftP = new Bitmap(Application.StartupPath + @"\pic\Left.png");
            rightP = new Bitmap(Application.StartupPath + @"\pic\Right.png");
            inited = true;
        }

    }
}

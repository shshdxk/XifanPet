using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using Iplugin.Pet;

namespace XifanPet
{
    public class DynamicPet
    {
        ///<summary>
        /// 存放插件的集合
        ///</summary>
        private static Dictionary<String, IPet> pets = new Dictionary<String, IPet>();

        public static List<String> GetPetTypes()
        {
            return pets.Keys.ToList();
        }

        public static IPet GetPet(String type)
        {
            IPet pet;
            if (pets.TryGetValue(type, out pet))
            {
                return pet;
            }
            return null;
        }

        ///<summary>
        /// 载入所有插件
        ///</summary>
        public static void LoadAllPets()
        {
            DirectoryInfo pluginsDir = new DirectoryInfo(Application.StartupPath + @"\pet");
            if (!pluginsDir.Exists)
            {
                return;
            }
            //获取插件目录(pet)下所有文件
            DirectoryInfo[] filePaths = pluginsDir.GetDirectories();
            foreach (DirectoryInfo filePath in filePaths)
            {
                FileInfo[] files = filePath.GetFiles("*.DLL");
                foreach (FileInfo fileP in files)
                {
                    String file = fileP.FullName;
                    try
                    {
                        //载入dll
                        Assembly ab = Assembly.LoadFrom(file);
                        Type[] types = ab.GetTypes();
                        foreach (Type t in types)
                        {
                            //如果某些类实现了预定义的IMsg.IMsgPlug接口，则认为该类适配与主程序(是主程序的插件)
                            if (t.GetInterface("IPet") != null)
                            {
                                Console.WriteLine(fileP.FullName + "  00000000");
                                Console.WriteLine(t.FullName + "  11111111");
                                if (!pets.ContainsKey(t.FullName))
                                {
                                    Object selObj = ab.CreateInstance(t.FullName);
                                    t.GetMethod("Initialization").Invoke(selObj, null);
                                    pets.Add(t.FullName, (IPet)selObj);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }
    }
}

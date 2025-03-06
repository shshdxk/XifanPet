using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using Iplugin;
using Iplugin.Pet;
using XifanPet.Properties;

namespace XifanPet
{
    public class DynamicMenu
    {
        /// <summary>
        /// 菜单
        /// </summary>
        private static ContextMenuStrip _contextMenuStrip = null;
        ///<summary>
        /// 存放插件的集合
        ///</summary>
        private static Dictionary<String, IPetPlug> plugins = new Dictionary<String, IPetPlug>();
        ///<summary>
        /// 使用的插件的集合
        ///</summary>
        private static Dictionary<String, IPetPlug> usedPlugins = new Dictionary<String, IPetPlug>();
        /// <summary>
        /// 是否穿透
        /// </summary>
        public static Boolean Through = false;
        ///<summary>
        /// 载入所有插件
        ///</summary>
        public static void LoadAllPlugs(ContextMenuStrip contextMenuStrip)
        {
            _contextMenuStrip = contextMenuStrip;
            DirectoryInfo pluginsDir = new DirectoryInfo(Application.StartupPath + @"\plugins");
            if (!pluginsDir.Exists)
            {
                return;
            }
            //获取插件目录(plugins)下所有文件
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
                            if (t.GetInterface("IPetPlug") != null && ContainsBaseType(t, "PetPlug"))
                            {
                                if (!plugins.ContainsKey(t.FullName))
                                {
                                    IPetPlug selObj = (IPetPlug)ab.CreateInstance(t.FullName);
                                    plugins.Add(t.FullName, selObj);
                                    selObj.Initialization();
                                    selObj.Closed(new PetPlug.ClosedCallback(ClosedCallback));
                                    SetMenu(selObj.GetMenu(), selObj);
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

        private static Boolean ContainsBaseType(Type o, string baseType)
        {
            Type t = o.BaseType;
            while (t != null)
            {
                if (object.Equals(t.Name, baseType))
                {
                    return true;
                }
                t = t.BaseType;
            }
            return false;
        }
        /// <summary>
        /// 动态生成插件菜单
        /// </summary>
        /// <param name="menus">菜单</param>
        private static void SetMenu(Iplugin.Menu[] menus, IPetPlug o)
        {
            ToolStripItemCollection item = _contextMenuStrip.Items;
            for (int i = 0; i < menus.Length; i++)
            {
                Iplugin.Menu om = menus[i];
                String text = om.MenuName;
                String name = text + "ToolStripMenuItem";
                ToolStripMenuItem menu = null;
                if (!item.ContainsKey(name))
                {
                    menu = new ToolStripMenuItem(text);
                    menu.Name = name;
                    if (!String.IsNullOrEmpty(om.MenuIco))
                    {
                        try
                        {
                            menu.Image = new Bitmap(om.MenuIco);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (i == menus.Length - 1)
                    {
                        menu.Tag = o;
                        menu.Click += new EventHandler(menu_Click);
                    }
                    InsertMenu(item, menu, om.Index);
                }
                else
                {
                    menu = (ToolStripMenuItem)(item.Find(name, false)[0]);
                }
                item = menu.DropDownItems;
            }
        }
        /// <summary>
        /// 将菜单按指定顺序插入到指定位置
        /// </summary>
        /// <param name="item">菜单组</param>
        /// <param name="menu">要插入的菜单</param>
        /// <param name="ind">要插入的顺序</param>
        private static void InsertMenu(ToolStripItemCollection item, ToolStripMenuItem menu, int ind)
        {
            if (ind == 0 || item.Count == 0)
            {
                // 插入位置为0或是没有菜单，在菜单末尾插入菜单
                item.Insert((item.Count > 0 ? item.Count - 1 : 0), menu);
            }
            else
            {
                int insertIndex = 0;
                foreach (ToolStripMenuItem oneMenu in item)
                {
                    if (oneMenu.MergeIndex > ind)
                    {
                        item.Insert(insertIndex, menu);
                        break;
                    }
                    insertIndex++;
                }
                if (insertIndex == item.Count)
                {
                    item.Insert(item.Count, menu);
                }
            }
        }
        /// <summary>
        /// 菜单的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void menu_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tool = (ToolStripMenuItem)sender;
            IPetPlug o = (IPetPlug)tool.Tag;
            string key = o.GetType().FullName;
            if (!usedPlugins.ContainsKey(key))
            {
                usedPlugins.Add(key, o);
            }
            o.OpenPlug();
            if (Through)
            {
                o.MouseThrough();
            }
            else
            {
                o.MouseRecover();
            }

        }

        public static void CloseAllPlugins()
        {
            foreach (IPetPlug o in plugins.Values)
            {
                ClosePlugin(o);
            }
        }

        public static void ClosePlugin(IPetPlug o)
        {
            if (o != null)
            {
                string key = o.GetType().FullName;
                if (usedPlugins.ContainsKey(key))
                {
                    usedPlugins.Remove(key);
                }
                o.Close();
            }
        }

        public static void ClosePlugin(string item)
        {
            IPetPlug plugin = DynamicMenu.GetAllPlugins()[item];
            if (plugin != null)
            {
                usedPlugins.Remove(item);
                plugin.Close();
            }
        }


        public static void OpenPlugin(string item)
        {

            IPetPlug plugin = DynamicMenu.GetAllPlugins()[item];
            if (plugin != null)
            {
                plugin.OpenPlug();
                if (Through)
                {
                    plugin.MouseThrough();
                }
                else
                {
                    plugin.MouseRecover();
                }
                usedPlugins.Add(item, plugin);
            }
        }

        public static void ClosedCallback(IPetPlug plug)
        {
            string key = plug.GetType().FullName;
            if (usedPlugins.ContainsKey(key))
            {
                usedPlugins.Remove(key);
            }
        }

        public static Dictionary<string, IPetPlug> GetUsedPlugins()
        {
            return usedPlugins;
        }
        public static Dictionary<string, IPetPlug> GetAllPlugins()
        {
            return plugins;
        }

        public static IPetPlug GetPlugin(String name)
        {
            IPetPlug plugin;
            if (plugins.TryGetValue(name, out plugin))
            {
                return plugin;
            }
            return null;
        }

    }
}

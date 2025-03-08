using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XifanPet.Properties;

namespace XifanPet
{
    public class Setting
    {
        public List<String> Plugins { get; set; }

        private Boolean through = true;
        public Boolean Through { get { return through; } set { this.through = value; } }

    }

    public class SettingManager
    {

        private static string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static Setting setting = new Setting();
        public static bool initing = true;
        public static void SaveSetting(Boolean? throughChecked)
        {
            if (initing)
            {
                return;
            }
            string settingPath = path + @"\setting.json";
            using (StreamWriter sw = new StreamWriter(settingPath))
            {
                setting.Plugins = DynamicMenu.GetUsedPlugins().Keys.ToList();
                if (throughChecked != null)
                {
                    setting.Through = throughChecked.Value;
                }
                sw.Write(JsonConvert.SerializeObject(setting));
            }
        }

        public static Setting ReadSetting()
        {
            string settingPath = path + @"\setting.json";
            if (File.Exists(settingPath))
            {
                using (StreamReader sr = new StreamReader(settingPath))
                {
                    try
                    {
                        setting = JsonConvert.DeserializeObject<Setting>(sr.ReadToEnd());
                        if (setting == null)
                        {
                            setting = new Setting();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                setting = new Setting();
            }
            return setting;
        }
    }
}

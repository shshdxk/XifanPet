using Iplugin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XifanPet
{
    public partial class FormPlugins : Form
    {
        public FormPlugins()
        {
            InitializeComponent();
        }

        private void FormPlugins_Load(object sender, EventArgs e)
        {
            Dictionary<string, IPetPlug> pluginsDict = DynamicMenu.GetAllPlugins();
            Dictionary<string, IPetPlug> usedPluginsDict = DynamicMenu.GetUsedPlugins();
            TreeNode root = new TreeNode();
            root.Text = "插件";
            root.Checked = pluginsDict.Count > 0;
            foreach (KeyValuePair<string, IPetPlug> kv in pluginsDict)
            {
                IPetPlug plug = kv.Value;
                TreeNodeCollection nodes = root.Nodes;
                foreach (Iplugin.Menu menu in plug.GetMenu())
                {
                    TreeNode node;
                    string name = menu.MenuName;
                    if (nodes.ContainsKey(name))
                    {
                        node = nodes.Find(name, false)[0];
                    }
                    else
                    {
                        node = new TreeNode(name);
                        node.Text = name;
                        node.Tag = plug;
                        if (usedPluginsDict.ContainsKey(kv.Key))
                        {
                            node.Checked = true;
                        }
                        InsertPlugin(nodes, node, menu.Index);
                    }
                    nodes = node.Nodes;
                }
            }

            Console.WriteLine(JsonConvert.SerializeObject(root));
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(root);
        }

        private void InsertPlugin(TreeNodeCollection nodes, TreeNode node, int index)
        {
            // TODO
            nodes.Add(node);
        }
    }


}

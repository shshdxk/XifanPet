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
using System.Xml.Linq;

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
            root.Checked = true;
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
                        node.Tag = kv.Key;
                        if (usedPluginsDict.ContainsKey(kv.Key))
                        {
                            node.Checked = true;
                        }
                        InsertPlugin(nodes, node, menu.Index);
                    }
                    nodes = node.Nodes;
                }
            }
            root.ExpandAll();

            Console.WriteLine(JsonConvert.SerializeObject(root));
            myTreeView1.Nodes.Clear();
            myTreeView1.Nodes.Add(root);
        }

        private void InsertPlugin(TreeNodeCollection nodes, TreeNode node, int index)
        {
            // TODO
            nodes.Add(node);
        }

        private void selectNode(TreeNode node)
        {
            if (node.Level == 0)
            {
                return;
            }
            if (node.Nodes.Count == 0)
            {
                string plugName = node.Tag as string;
                DynamicMenu.OpenPlugin(plugName);
            }

            if (!node.Checked)
            {
                node.Checked = true;
            }
            selectNode(node.Parent);
        }

        private void unSelectNode(TreeNode node)
        {
            if (node.Level == 0)
            {
                return;
            }

            if (node.Nodes.Count == 0)
            {
                string plugName = node.Tag as string;
                DynamicMenu.ClosePlugin(plugName);
            }
            if (node.Checked)
            {
                node.Checked = false;
            }
            foreach (TreeNode child in node.Nodes)
            {
                unSelectNode(child);

            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            
            TreeNode node = e.Node;
            if (node.Level == 0)
            {
                if (!node.Checked)
                {
                    node.Checked = true;
                }
            }
            if (node.Checked)
            {
                selectNode(node);
            }
            else
            {
                unSelectNode(node);
            }
        }


    }


}

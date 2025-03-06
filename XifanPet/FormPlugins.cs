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
            InitViewTree();
        }

        private void InitViewTree()
        {
            Dictionary<string, IPetPlug> pluginsDict = DynamicMenu.GetAllPlugins();
            Dictionary<string, IPetPlug> usedPluginsDict = DynamicMenu.GetUsedPlugins();
            TreeNode root = new TreeNode();
            root.Text = "插件";
            root.Checked = true;

            List<string> keys = pluginsDict.Keys.ToList();
            keys.Sort((a1, a2) => {
                IPetPlug p1 = pluginsDict[a1];
                IPetPlug p2 = pluginsDict[a2];
                return p1.GetMenu()[0].Index - p2.GetMenu()[0].Index;
            });

            foreach (string key in keys)
            {
                IPetPlug plug = pluginsDict[key];
                TreeNodeCollection nodes = root.Nodes;
                foreach (Iplugin.Menu menu in plug.GetMenu())
                {
                    TreeNode node;
                    string name = menu.MenuName;
                    TreeNode ch = CheckChildNodesText(nodes, name);
                    if (ch != null)
                    {
                        node = ch;
                    }
                    else
                    {
                        node = new TreeNode(name);
                        node.Text = name;
                        node.Tag = key;
                        InsertPlugin(nodes, node, menu.Index);
                    }
                    if (usedPluginsDict.ContainsKey(key))
                    {
                        node.Checked = true;
                    }
                    nodes = node.Nodes;
                }
            }
            root.ExpandAll();


            myTreeView1.Nodes.Clear();
            myTreeView1.Nodes.Add(root);
        }
        private TreeNode CheckChildNodesText(TreeNodeCollection parentNode, string text)
        {
            if (parentNode.Count == 0)
            {
                return null;
            }
            foreach (TreeNode childNode in parentNode)
            {
                if (object.Equals(text, childNode.Text))
                {
                    return childNode;
                }
            }
            return null;
        }

        private void InsertPlugin(TreeNodeCollection nodes, TreeNode node, int index)
        {
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

        private void FormPlugins_Activated(object sender, EventArgs e)
        {
            InitViewTree();
        }
    }


}

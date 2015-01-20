using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient.LocalEntities
{
    public partial class LocalTreeView : TreeView
    {
        public LocalTreeView()
        {
            InitializeComponent();
        }

        public LocalTreeView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void AddRootNode(TreeNode rootNode)
        {
            this.Nodes.Add(rootNode);
        }

        public void AddNode(TreeNode childNode, TreeNode parentNode)
        {
            parentNode.Nodes.Add(childNode);
        }

        private TreeNode CreateNode(FileSystemInfo fileInfo, int imageIndex, TreeNode parentNode)
        {
            TreeNode node = GenerateTreeNode(fileInfo, imageIndex);
            return node;
        }

        public TreeNode GenerateTreeNode(FileSystemInfo fileInfo, int imageIndex)
        {
            string name = fileInfo.Name;
            TreeNode node = new TreeNode(name);
            node.Name = name;
            node.Tag = fileInfo;
            node.ImageIndex = imageIndex;
            node.SelectedImageIndex = imageIndex;

            return node;
        }
    }
}

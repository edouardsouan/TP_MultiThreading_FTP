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

        public void AddNode(FileSystemInfo fileInfo, int imageIndex, TreeNode parentNode)
        {
            TreeNode childNode = CreateNode(fileInfo, imageIndex, parentNode);
            parentNode.Nodes.Add(childNode);
        }

        public void AddNodes(List<DirectoryInfo> directoryToAdd, TreeNode parentNode)
        {
            foreach (DirectoryInfo directoryInfo in directoryToAdd)
            {
                AddNode(directoryInfo, 1, parentNode);
            }
        }

        public void AddNodes(List<FileInfo> filesToAdd, TreeNode parentNode)
        {
            foreach (FileInfo fileInfo in filesToAdd)
            {
                AddNode(fileInfo, 2, parentNode);
            }
        }
    }
}

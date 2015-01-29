using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_Client.LocalEntities
{
    public class LocalTreeView : TreeView
    {
        public LocalTreeView()
        {
        }

        public void AddRootNode(TreeNode rootNode)
        {
            this.Nodes.Add(rootNode);
        }

        public void AddNode(TreeNode childNode, TreeNode parentNode)
        {
            parentNode.Nodes.Add(childNode);
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

        public bool IsNodeADirectory(TreeNode node)
        {
            bool isADirectory = false;
            if (node.ImageIndex == 0 || node.ImageIndex == 1)
            {
                isADirectory = true;
            }
            return isADirectory;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient.FormEntites
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

        public void AddNode(FileSystemInfo fileInfo, int imageIndex)
        {
            TreeNode rootNode = new TreeNode(fileInfo.Name);
            rootNode.Tag = fileInfo;
            rootNode.ImageIndex = imageIndex;
            rootNode.SelectedImageIndex = imageIndex;
            this.Nodes.Add(rootNode);
        }

        public void AddNode(FileSystemInfo fileInfo, int imageIndex, TreeNode parentNode)
        {
            TreeNode childNode = new TreeNode(fileInfo.Name);
            childNode.Tag = fileInfo;
            childNode.ImageIndex = imageIndex;
            childNode.SelectedImageIndex = imageIndex;
            parentNode.Nodes.Add(childNode);
        }
    }
}

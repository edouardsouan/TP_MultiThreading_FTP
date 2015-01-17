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

        public void AddRootNodes(DirectoryInfo logicalDrive)
        {
            TreeNode rootNode = new TreeNode(logicalDrive.Name);
            rootNode.Tag = logicalDrive;
            rootNode.ImageIndex = 0;
            rootNode.SelectedImageIndex = 0;
            this.Nodes.Add(rootNode);
        }

        public void AddNodes()
        {
        }
    }
}

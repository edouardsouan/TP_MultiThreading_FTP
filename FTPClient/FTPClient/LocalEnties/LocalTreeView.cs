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

        public void AddRootNode(FileSystemInfo fileInfo, int imageIndex)
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

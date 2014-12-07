using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateTreeViewWithLogicalDrives();
        }

        private void PopulateTreeViewWithLogicalDrives()
        {
            TreeNode rootNode;

            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(drive);
                if (directoryInfo.Exists)
                {
                    rootNode = new TreeNode(directoryInfo.Name);
                    rootNode.Tag = directoryInfo;
                    treeViewLocalFiles.Nodes.Add(rootNode); 
                }
            }
        }

        private void treeViewLocalFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            
            if (nodeClicked.Nodes.Count == 0)
            {
                PropulateTreeNodeWithDirectories(nodeClicked);
                PropulateTreeNodeWithFiles(nodeClicked);
            }
        }

        private void PropulateTreeNodeWithDirectories(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            foreach (DirectoryInfo subDir in nodeClickedInfo.GetDirectories())
            {
                TreeNode newDirNode = new TreeNode(subDir.Name, 0, 0);
                newDirNode.Tag = nodeClickedInfo;
                newDirNode.ImageIndex = 0;
                nodeClicked.Nodes.Add(newDirNode);
            }
            
        }

        private void PropulateTreeNodeWithFiles(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            foreach (FileInfo file in nodeClickedInfo.GetFiles())
            {
                TreeNode newFileNode = new TreeNode(file.Name, 0, 0);
                newFileNode.Tag = nodeClickedInfo;
                newFileNode.ImageIndex = 1;
                nodeClicked.Nodes.Add(newFileNode);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

// Complete Form1.cs and manage the local aspect 

namespace FTPClient
{
    public partial class Form1 : Form
    {
        private void Form1_Load(object sender, EventArgs e)
        {
            Local_ShowLogicalDrives();
        }

        private void Local_ShowLogicalDrives()
        {
            string[] drives = Environment.GetLogicalDrives();

            foreach (string drive in drives)
            {
                DirectoryInfo logicalDrive = new DirectoryInfo(drive);
                TreeNode newNode = localTreeView.GenerateTreeNode(logicalDrive, 0);
                localTreeView.AddRootNode(newNode);
                localListView.AddItem(newNode);
            }
        }

        private List<DirectoryInfo> Local_GetLocalDirectories(DirectoryInfo directoryInfo)
        {
            List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();

            try
            {
                subDirectories = directoryInfo.GetDirectories().ToList();
            }
            catch (UnauthorizedAccessException exception)
            {
                logWindow.WriteLog("Access Denied : " + directoryInfo.Name, Color.Red);
                Console.WriteLine(exception.ToString());
            }

            return subDirectories;
        }

        private List<DirectoryInfo> Local_GetLocalDirectories(TreeNode nodeClicked)
        {
            DirectoryInfo directoryInfo = (DirectoryInfo)nodeClicked.Tag;
            List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();

            if (nodeClicked.Nodes.Count == 0)
            {
                subDirectories = Local_GetLocalDirectories(directoryInfo);
            }
            else
            {
                TreeNodeCollection subNodes = nodeClicked.Nodes;
                foreach (TreeNode subNode in subNodes)
                {
                    if (IsADirectory(subNode))
                    {
                        subDirectories.Add((DirectoryInfo)subNode.Tag);
                    }
                }
            }

            return subDirectories;
        }

        private List<FileInfo> Local_GetLocalFiles(DirectoryInfo directoryInfo)
        {
            List<FileInfo> subFiles = new List<FileInfo>();

            try
            {
                subFiles = directoryInfo.GetFiles().ToList();
            }
            catch (UnauthorizedAccessException exception)
            {
                logWindow.WriteLog("Access Denied : " + directoryInfo.Name, Color.Red);
                Console.WriteLine(exception.ToString());
            }

            return subFiles;
        }

        private List<FileInfo> Local_GetLocalFiles(TreeNode nodeClicked)
        {
            DirectoryInfo directoryInfo = (DirectoryInfo)nodeClicked.Tag;
            List<FileInfo> subFiles = new List<FileInfo>();

            if (nodeClicked.Nodes.Count == 0)
            {
                subFiles = Local_GetLocalFiles(directoryInfo);
            }
            else
            {
                TreeNodeCollection subNodes = nodeClicked.Nodes;
                foreach (TreeNode subNode in subNodes)
                {
                    if (!IsADirectory(subNode))
                    {
                        subFiles.Add((FileInfo)subNode.Tag);
                    }
                }
            }

            return subFiles;
        }

        private void Local_ShowLinkedElements(TreeNode nodeSelected)
        {
            if(IsADirectory((FileSystemInfo)nodeSelected.Tag))
            {
                List<DirectoryInfo> subDirectories = Local_GetLocalDirectories(nodeSelected);
                List<FileInfo> subFiles = Local_GetLocalFiles(nodeSelected);

                if (nodeSelected.Nodes.Count == 0)
                {
                    localTreeView.AddNodes(subDirectories, nodeSelected);
                    localTreeView.AddNodes(subFiles, nodeSelected);
                }
                nodeSelected.Expand();

                localListView.ClearItems();

                DisplayParentNodeInListView(nodeSelected);

                foreach(DirectoryInfo subDir in subDirectories)
                {
                    TreeNode dirNode = localTreeView.GenerateTreeNode(subDir, 1);
                    localListView.AddItem(dirNode);
                }

                foreach(FileSystemInfo subFile in subFiles)
                {
                    TreeNode fileNode = localTreeView.GenerateTreeNode(subFile, 2);
                    localListView.AddItem(fileNode);
                }
            }
        }

        private void DisplayParentNodeInListView(TreeNode nodeSelected)
        {
            // ListViewItem parentItem = localListView.GenerateItem((FileSystemInfo)nodeSelected.Tag);
            // parentItem.Text = "..";
            // TreeNode parentNode = localTreeView.GenerateTreeNode(parentItem, parentItem.ImageIndex);
            // nodeSelected.Name = "..";
            localListView.AddItem(nodeSelected,"..");
        }

        private void treeViewLocal_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            Local_ShowLinkedElements(nodeClicked);

            localPath = nodeClicked.FullPath;
        }
    }
}

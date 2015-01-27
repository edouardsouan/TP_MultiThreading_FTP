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

        private void treeViewLocal_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            Local_OpenNode(nodeClicked);
        }

        private void localListView_MouseDoubleClick(object sender, EventArgs e)
        {
            if (localListView.SelectedItems.Count > 0)
            {
                TreeNode nodeClicked = (TreeNode)localListView.SelectedItems[0].Tag;
                Local_OpenNode(nodeClicked);
            }

        }

        private void Local_OpenNode(TreeNode nodeClicked)
        {
            if (IsADirectory(nodeClicked))
            {
                Local_ShowLinkedElements(nodeClicked);
                localPath = nodeClicked.FullPath;
            }
        }

        #region Functions for FileSystemInfo
        private void Local_ShowLogicalDrives()
        {
            string[] drives = Environment.GetLogicalDrives();

            foreach (string drive in drives)
            {
                DirectoryInfo logicalDrive = new DirectoryInfo(drive);
                TreeNode newNode = localTreeView.GenerateTreeNode(logicalDrive, 0);
                localTreeView.AddRootNode(newNode);
                localListView.AddItem(newNode, newNode.Name);
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
        #endregion

        #region Functions and Methodes for TreeNode
        private void Local_ShowLinkedElements(TreeNode nodeSelected)
        {
            if (IsADirectory((FileSystemInfo)nodeSelected.Tag))
            {
                localListView.ClearItems();
               //  DisplayParentNodeInListView(nodeSelected);

                List<DirectoryInfo> subDirectories = Local_GetLocalDirectories(nodeSelected);
                List<FileInfo> subFiles = Local_GetLocalFiles(nodeSelected);

                bool addDirectoriesAndFiles = false;
                if (nodeSelected.Nodes.Count == 0)
                {
                    addDirectoriesAndFiles = true;
                }
                
                foreach (DirectoryInfo subDir in subDirectories)
                {
                    TreeNode dirNode = localTreeView.GenerateTreeNode(subDir, 1);

                    if (addDirectoriesAndFiles)
                    {
                        localTreeView.AddNode(dirNode, nodeSelected);
                    }

                    localListView.AddItem(dirNode, dirNode.Name);
                }

                foreach (FileSystemInfo subFile in subFiles)
                {
                    TreeNode fileNode = localTreeView.GenerateTreeNode(subFile, 2);

                    if (addDirectoriesAndFiles)
                    {
                        localTreeView.AddNode(fileNode, nodeSelected);
                    }

                    localListView.AddItem(fileNode, fileNode.Name);
                }

                nodeSelected.Expand();
            }
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

        private void DisplayParentNodeInListView(TreeNode nodeSelected)
        {
            localListView.AddItem(nodeSelected,"..");
        }
        #endregion

    }
}

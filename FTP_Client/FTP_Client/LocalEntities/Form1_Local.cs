using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

// Complete Form1.cs and manage the local aspect 

namespace FTP_Client
{
    public partial class Form1 : Form
    {

        private void Local_OpenNode(TreeNode nodeClicked)
        {
            if (localTreeView.IsNodeADirectory(nodeClicked))
            {
                Local_ShowLinkedElements(nodeClicked);
                localPath = nodeClicked.FullPath;
            }
        }
        
        #region Local travel
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
                logWindow.WriteLog("\nAccess Denied : " + directoryInfo.Name, Color.Red);
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
                logWindow.WriteLog("\nAccess Denied : " + directoryInfo.Name, Color.Red);
                Console.WriteLine(exception.ToString());
            }

            return subFiles;
        }

        private void Local_ShowLinkedElements(TreeNode nodeSelected)
        {
            if ( localTreeView.IsNodeADirectory(nodeSelected) )
            {
                localListView.ClearItems();
                DisplayParentNodeInListView(nodeSelected);

                if (nodeSelected.Nodes.Count > 0)
                {
                    foreach (TreeNode subNode in nodeSelected.Nodes)
                    {
                        localListView.AddItem(subNode, subNode.Name);
                    }
                }
                else
                {
                    DirectoryInfo directoryInfo = (DirectoryInfo)nodeSelected.Tag;

                    List<DirectoryInfo>  subDirectories = Local_GetLocalDirectories(directoryInfo);
                    foreach (DirectoryInfo subDir in subDirectories)
                    {
                        TreeNode dirNode = localTreeView.GenerateTreeNode(subDir, 1);
                        localTreeView.AddNode(dirNode, nodeSelected);
                        localListView.AddItem(dirNode, dirNode.Name);
                    }

                    List<FileInfo> subFiles = Local_GetLocalFiles(directoryInfo);
                    foreach (FileSystemInfo subFile in subFiles)
                    {
                        TreeNode fileNode = localTreeView.GenerateTreeNode(subFile, 2);
                        localTreeView.AddNode(fileNode, nodeSelected);
                        localListView.AddItem(fileNode, fileNode.Name);
                    }
                }

                nodeSelected.Expand();
            }
        }

        private void DisplayParentNodeInListView(TreeNode nodeSelected)
        {
            if (nodeSelected.Parent != null)
            {
                localListView.AddItem(nodeSelected, "..");
            }
        }
        #endregion
    }
}

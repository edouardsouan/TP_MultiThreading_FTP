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
                localTreeView.AddRootNode(logicalDrive, 0);
                localListView.AddItem(logicalDrive);
            }
        }

        private List<DirectoryInfo> Local_GetLocalDirectories(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            List<DirectoryInfo> subDirectories = new List<DirectoryInfo>();

            if (nodeClicked.Nodes.Count == 0)
            {
                try
                {
                    subDirectories = nodeClickedInfo.GetDirectories().ToList();
                }
                catch (UnauthorizedAccessException exception)
                {
                    logWindow.WriteLog("Access Denied : "+nodeClickedInfo.Name, Color.Red);
                    Console.WriteLine(exception.ToString());
                }
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
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            List<FileInfo> subFiles = new List<FileInfo>();

            if (nodeClicked.Nodes.Count == 0)
            {
                try
                {
                    subFiles = nodeClickedInfo.GetFiles().ToList();
                }
                catch (UnauthorizedAccessException exception)
                {
                    logWindow.WriteLog("Access Denied : "+nodeClickedInfo.Name, Color.Red);
                    Console.WriteLine(exception.ToString());
                }
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
                localListView.AddItems(subDirectories);
                localListView.AddItems(subFiles);
            }
        }

        private void treeViewLocal_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            Local_ShowLinkedElements(nodeClicked);

            localPath = nodeClicked.FullPath;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using FTP_Client.ServerEntities;

// Complete Form1.cs and manage the server aspect 

namespace FTP_Client
{
    public partial class Form1 : Form
    {
        FTPManager ftpManager;
        bool isSendingListCommand = false;

        #region managing server events
        public void Server_RefreshView()
        {
            ListViewItem parentItem = serverListView.Items[0];
            TreeNode parentNode = (TreeNode)parentItem.Tag;
            parentNode.Nodes.Clear();
            Server_ShowLinkedFTPElements(serverPath, parentNode);
        }

        private void Server_OpenNode(TreeNode nodeClicked)
        {
            if (serverTreeView.IsNodeADirectory(nodeClicked))
            {
                serverPath = nodeClicked.FullPath.Replace("\\", "/");

                serverListView.Items.Clear();
                Server_DisplayParentNode(nodeClicked);

                if (nodeClicked.Nodes.Count == 0)
                {
                    Server_ShowLinkedFTPElements(nodeClicked.FullPath, nodeClicked);
                }
                else
                {
                    Server_ShowSubItems(nodeClicked);
                }

                nodeClicked.Expand();
            }
        }

        private void Server_DisplayParentNode(TreeNode parentNode)
        {
            if (parentNode.Parent != null)
            {
                serverListView.AddItem(parentNode, "..");
            }
        }
        #endregion

        #region FTP : get and show distant files/directories
        private async void Server_ShowLinkedFTPElements(string serverPath, TreeNode parentNode)
        {
            if (!isSendingListCommand)
            {
                try
                {
                    isSendingListCommand = true;
                    FtpWebRequest ftpRequest = ftpManager.CreatRequestListDirectoriesAndFiles(serverPath);
                    FtpWebResponse ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync();

                    logWindow.WriteLog(ftpResponse);
                    string[] serverData = ftpManager.ParseRawData(ftpResponse);
                    Server_ShowFiles(serverData, parentNode);
                }
                catch (WebException ex)
                {
                    logWindow.WriteLog(ex.Message, Color.Red);
                }
                finally
                {
                    isSendingListCommand = false;
                }
            }
        }

        private void Server_ShowFiles(string[] serverData, TreeNode parentNode)
        {
            serverListView.ClearItems();
            Server_DisplayParentNode(parentNode);

            foreach (string aData in serverData)
            {
                FileServer fileServer = new FileServer(aData);
                if (fileServer.IsNameOKToDisplay())
                {
                    TreeNode fileNode = serverTreeView.CreateNode(fileServer);

                    serverTreeView.AddNode(fileNode, parentNode);
                    serverListView.AddItem(fileNode, fileNode.Name);
                }
            }

            parentNode.Expand();
        }

        private void Server_ShowSubItems(TreeNode parentNode)
        {
            TreeNodeCollection subNodes = parentNode.Nodes;
            foreach (TreeNode subNode in subNodes)
            {
                serverListView.AddItem(subNode, subNode.Name);
            }
        }
        #endregion

        #region FTP Actions
        private void Server_CreateDirectory(string serverPathTarget)
        {
            try
            {
                FtpWebRequest makeDirRequest = ftpManager.CreatRequestMakeDirectory(serverPathTarget);
                FtpWebResponse makeDirResponse = (FtpWebResponse)makeDirRequest.GetResponse();
                logWindow.WriteLog(makeDirResponse);
                makeDirResponse.Close();
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void Server_Rename(string oldName, string newName)
        {
            string oldPath = serverPath + "/" + oldName;

            try  
            {
                FtpWebRequest renameRequest = ftpManager.CreatRequestRename(oldPath, newName);

                FtpWebResponse response = (FtpWebResponse)renameRequest.GetResponse();
                using (Stream ftpStream = response.GetResponseStream())
                {
                    ftpStream.Close();
                    response.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                logWindow.WriteLog("Error rename", Color.Red);
            }
        }
        #endregion
    }
}

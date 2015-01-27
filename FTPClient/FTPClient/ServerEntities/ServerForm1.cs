using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

using FTPClient.ServerEntities;

// Complete Form1.cs and manage the FTP server aspect 

namespace FTPClient
{
    public partial class Form1 : Form
    {
        FTPManager ftpManager;
        bool isSendingListCommand = false;

        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (serverTreeView.Nodes.Count == 0)
            { 
                serverTreeView.InitRoot();

                ftpManager = new FTPManager(this.txtUserName.Text, this.txtPassword.Text, this.txtServer.Text, this.txtPort.Text);
                Server_ShowLinkedFTPElements("", serverTreeView.Nodes[0]);
            }
        }

        private void treeViewServer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            Server_OpenNode(nodeClicked);
        }

        private void serverListView_MouseDoubleClick(object sender, EventArgs e)
        {
            if (serverListView.SelectedItems.Count > 0)
            {
                TreeNode nodeClicked = (TreeNode)serverListView.SelectedItems[0].Tag;
                Server_OpenNode(nodeClicked);
            }

        }

        private void Server_OpenNode(TreeNode nodeClicked)
        {
            if (IsADirectory(nodeClicked))
            {
                serverPath = nodeClicked.FullPath;

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

        #region Functions and Methodes for TreeNode
        private async void Server_ShowLinkedFTPElements(string serverPath, TreeNode parentNode)
        {
            if (!isSendingListCommand)
            {
                try
                {
                    isSendingListCommand = true;
                    FtpWebRequest ftpRequest = ftpManager.CreatRequestListDirectoriesAndFiles(serverPath);

                    Console.WriteLine(ftpRequest.ToString());

                    FtpWebResponse ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync();

                    Console.WriteLine(ftpResponse.ToString());
                    
                    logWindow.WriteLog(ftpResponse);
                    string[] serverData = ftpManager.ParseRawData(ftpResponse);
                    Server_ShowFiles(serverData, parentNode, serverPath);
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

        private void Server_ShowFiles(string[] serverData, TreeNode parentNode, string serverPath)
        {
            serverListView.ClearItems();
            // Server_DisplayParentNode(parentNode);
           
            foreach (string aData in serverData)
            {
                FileServer fileServer = new FileServer(aData);
                if (fileServer.IsNameOKToDisplay())
                {
                    TreeNode fileNode = serverTreeView.CreateNode(fileServer, parentNode);

                    serverTreeView.AddNode(fileNode, parentNode);
                    serverListView.AddItem(fileNode, fileNode.Name);
                }
            }

            parentNode.Expand();
        }

        private void Server_DisplayParentNode(TreeNode parentNode)
        {
            if (parentNode.Parent != null)
            {
                serverListView.AddItem(parentNode, "..");
            }
        }
        #endregion

        private void Server_ShowSubItems(TreeNode parentNode)
        {
            TreeNodeCollection subNodes = parentNode.Nodes;
            foreach (TreeNode subNode in subNodes)
            {
                serverListView.AddItem(subNode, subNode.Name);
            }
        }

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
    }
}

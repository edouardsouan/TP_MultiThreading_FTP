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

namespace FTP_Client
{
    public partial class Form1 : Form
    {
        FTPManager ftpManager;
        bool isSendingListCommand = false;

        private void btnConnection_Click(object sender, EventArgs e)
        {
            serverTreeView.Nodes.Clear();
            serverTreeView.InitRoot();

            ftpManager = new FTPManager(this.txtUserName.Text, this.txtPassword.Text, this.txtServer.Text, this.txtPort.Text);
            Server_ShowLinkedFTPElements("", serverTreeView.Nodes[0]);
        }

        private void serverTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            Server_OpenNode(nodeClicked);
        }

        private void serverListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (serverListView.SelectedItems.Count > 0)
            {
                TreeNode nodeClicked = (TreeNode)serverListView.SelectedItems[0].Tag;
                Server_OpenNode(nodeClicked);
            }
        }

        private void Server_OpenNode(TreeNode nodeClicked)
        {
            if (serverTreeView.IsADirectory(nodeClicked))
            {
                if(nodeClicked.Text.Equals(".."))
                {
                    serverPath = nodeClicked.Parent.FullPath;
                }
                else
                {
                    serverPath = nodeClicked.FullPath;
                }

                serverListView.Items.Clear();
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

        #region FTP travel
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
            Server_DisplayParentNode(parentNode);

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
        # endregion
    }
}

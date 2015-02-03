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
                    Server_ShowLinkedFTPElements(serverPath, nodeClicked);
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

                    logWindow.WriteLog(ftpRequest);
                    if (showConnectionLog)
                    {
                        logWindow.WriteLogConnectionResponse(ftpResponse);
                        showConnectionLog = false;
                    }
                    else
                    {
                        logWindow.WriteLog(ftpResponse);
                    }

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
                logWindow.WriteLog(makeDirRequest);
                logWindow.WriteLog(makeDirResponse);
                makeDirResponse.Close();
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void Server_Rename(TreeNode nodeToRename, string newName)
        {
            string oldPath = serverPath + "/" + nodeToRename.Name;

            try
            {
                FtpWebRequest renameRequest = ftpManager.CreatRequestRename(oldPath, newName);
                FtpWebResponse renameResponse = (FtpWebResponse)renameRequest.GetResponse();

                logWindow.WriteLog(renameRequest);
                logWindow.WriteLog(renameResponse);

                using (Stream ftpStream = renameResponse.GetResponseStream())
                {
                    ftpStream.Close();
                    renameResponse.Close();
                }

                nodeToRename.Name = newName;
                Server_OpenNode(nodeToRename.Parent);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                logWindow.WriteLog("Error rename", Color.Red);
            }
        }
        #endregion

        #region Delete
        private void Server_Delete(TreeNode nodeToDelete)
        {
            try
            {
                string fullPath = nodeToDelete.FullPath.Replace("\\", "/");

                if (serverTreeView.IsNodeADirectory(nodeToDelete))
                {
                    Server_DeleteFolder(fullPath);
                }
                else
                {
                    Server_DeleteFile(fullPath);
                }

                TreeNode parentNode = nodeToDelete.Parent;
                nodeToDelete.Remove();
                Server_OpenNode(parentNode);
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.ToString());
                MessageBox.Show("Access denied");
            }
        }

        private void Server_DeleteFolder(string fullPath)
        {
            FtpWebRequest ftpRequest = ftpManager.CreatRequestListDirectoriesAndFiles(fullPath);
            FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            string[] serverData = ftpManager.ParseRawData(ftpResponse);

            foreach (string aData in serverData)
            {
                FileServer fileServer = new FileServer(aData);
                if (fileServer.IsNameOKToDisplay())
                {
                    if (fileServer.IsADirectory())
                    {
                        Server_DeleteFolder(fullPath + "/" + fileServer.GetName());
                    }
                    else
                    {
                        Server_DeleteFile(fullPath + "/" + fileServer.GetName());
                    }
                }
            }

            try
            {
                FtpWebRequest deleteRequest = ftpManager.CreatRequestDeleteDirectory(fullPath);
                FtpWebResponse deleteResponse = (FtpWebResponse)deleteRequest.GetResponse();

                logWindow.WriteLog(deleteRequest);
                logWindow.WriteLog(deleteResponse);

                String result = String.Empty;
                Int64 size = deleteResponse.ContentLength;
                using (Stream datastream = deleteResponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(datastream))
                    {
                        result = streamReader.ReadToEnd();
                        streamReader.Close();
                        datastream.Close();
                        deleteResponse.Close();
                    }
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.ToString());
                MessageBox.Show("Access denied");
            }
        }

        private void Server_DeleteFile(string fullPath)
        {
            try
            {
                FtpWebRequest deleteRequest = ftpManager.CreatRequestDeleteFile(fullPath);
                FtpWebResponse deleteResponse = (FtpWebResponse)deleteRequest.GetResponse();

                logWindow.WriteLog(deleteRequest);
                logWindow.WriteLog(deleteResponse);

                String result = String.Empty;
                Int64 size = deleteResponse.ContentLength;
                using (Stream datastream = deleteResponse.GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(datastream))
                    {
                        result = streamReader.ReadToEnd();
                        streamReader.Close();
                        datastream.Close();
                        deleteResponse.Close();
                    }
                }
            }
            catch (IOException exception)
            {
                Console.WriteLine(exception.ToString());
                MessageBox.Show("Access denied");
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

// Complete Form1.cs and manage the FTP server aspect 

using FTPLibrary;

namespace FTPClient
{
    public partial class Form1 : Form
    {
        private void btnConnection_Click(object sender, EventArgs e)
        {
            TreeNode serverNode = new TreeNode();
            serverNode = new TreeNode("/");
            serverNode.Tag = "/";
            serverNode.ImageIndex = 0;
            serverNode.SelectedImageIndex = 0;
            serverTreeView.Nodes.Add(serverNode);

            GetTreeViewFromServer("", serverTreeView.Nodes[0]);
        }

        private async void GetTreeViewFromServer(string serverPath, TreeNode parentNode)
        {
            string serverTarget = "ftp://" + this.txtServer.Text + serverPath + "/";

            if (!isSendingListCommand)
            {
                try
                {
                    isSendingListCommand = true;
                    FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverTarget);
                    ftpRequest.KeepAlive = false;
                    ftpRequest.Credentials = new NetworkCredential(this.txtUserName.Text, this.txtPassword.Text);
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                    FtpWebResponse ftpResponse = (FtpWebResponse)await ftpRequest.GetResponseAsync();
                    logWindow.WriteLog(ftpResponse.BannerMessage, Color.Green);
                    logWindow.WriteLog(ftpResponse.WelcomeMessage, Color.Green);
                    logWindow.WriteLog(ftpResponse.StatusDescription, Color.Blue);
                    logWindow.WriteLog(ftpResponse.StatusCode.ToString(), Color.Blue);
                    logWindow.WriteLog(WebRequestMethods.Ftp.ListDirectoryDetails, Color.Black);

                    Stream responseStream = ftpResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream);

                    string rawResult = streamReader.ReadToEnd();
                    string data = rawResult.Remove(rawResult.LastIndexOf("\n"), 1);
                    BuildServerTreeView(data.Split('\n'), parentNode);

                    streamReader.Close();
                    ftpResponse.Close();
                }
                catch (WebException ex)
                {
                    logWindow.WriteLog(ex.Message, Color.Red);
                    isSendingListCommand = false;
                }
                finally
                {
                    isSendingListCommand = false;
                }
            }
        }

        // TODO : Build in async
        private void BuildServerTreeView(string[] directories, TreeNode parentNode)
        {
            FileServer fileServer;
            List<FileServer> fileSystInfos = new List<FileServer>();

            foreach (string rawDirectory in directories)
            {
                fileServer = new FileServer(rawDirectory);
                if (!(fileServer.GetName().Equals(".") || fileServer.GetName().Equals("..")))
                {
                    AddNodeServerTreeView(fileServer, parentNode);
                    fileSystInfos.Add(fileServer);
                }
            }

            PopulateServerListView(fileSystInfos.ToArray());
            parentNode.Expand();
        }

        private void AddNodeServerTreeView(FileServer fileServer, TreeNode parentNode)
        {
            TreeNode serverNode = new TreeNode(fileServer.GetName());
            serverNode.Tag = fileServer;

            if (fileServer.GetDataType().Equals("Directory"))
            {
                serverNode.ImageIndex = 1;
                serverNode.SelectedImageIndex = 1;
            }
            else
            {
                serverNode.ImageIndex = 2;
                serverNode.SelectedImageIndex = 2;
            }

            parentNode.Nodes.Add(serverNode);
        }

        private void treeViewServer_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            if (IsADirectory(nodeClicked))
            {
                serverPath = nodeClicked.FullPath;
                serverListView.Items.Clear();
                if (nodeClicked.Nodes.Count == 0)
                {
                    GetTreeViewFromServer(nodeClicked.FullPath, nodeClicked);
                }
                else
                {
                    List<FileServer> fileSystInfos = new List<FileServer>();
                    TreeNodeCollection nodes = nodeClicked.Nodes;
                    foreach (TreeNode node in nodes)
                    {
                        fileSystInfos.Add((FileServer)node.Tag);
                    }
                    PopulateServerListView(fileSystInfos.ToArray());
                }
            }
        }

        private void PopulateServerListView(FileServer[] files)
        {
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            string extension = "";
            string size = "";
            string date = "";
            string rights = "";
            string owner = "";
            string group = "";

            foreach (FileServer subFile in files)
            {
                item = new ListViewItem(subFile.GetName(), 0);
                item.Name = subFile.GetName();
                item.Tag = subFile;
                size = subFile.GetSize().ToString();
                extension = subFile.GetDataType();
                date = subFile.GetLastModifiedDate();
                rights = subFile.GetRights();
                owner = subFile.GetOwner();
                group = subFile.GetGroup();

                subItems = new ListViewItem.ListViewSubItem[]
                    {
                        new ListViewItem.ListViewSubItem(item, size),
                        new ListViewItem.ListViewSubItem(item, extension), 
                        new ListViewItem.ListViewSubItem(item, date),
                        new ListViewItem.ListViewSubItem(item, rights),
                        new ListViewItem.ListViewSubItem(item, owner),
                        new ListViewItem.ListViewSubItem(item, group)
                    };

                if (extension.Equals("Directory"))
                {
                    item.ImageIndex = 1;
                }
                else
                {
                    item.ImageIndex = 2;
                }

                item.SubItems.AddRange(subItems);
                serverListView.Items.Add(item);
            }

            serverListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
    }
}

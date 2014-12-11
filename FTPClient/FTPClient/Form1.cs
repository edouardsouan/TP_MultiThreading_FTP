using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTPClient
{
    public partial class Form1 : Form
    {
        #region Variables
        bool isSendingListCommand = false;
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            txtPassword.TextBox.PasswordChar = '*';

            listViewLocal.Items.Clear();

            TreeNode serverNode = new TreeNode();
            serverNode = new TreeNode("/");
            serverNode.Tag = "/";
            serverNode.ImageIndex = 0;
            serverNode.SelectedImageIndex = 0;
            treeViewServer.Nodes.Add(serverNode);
        }
        #endregion

        #region Destructor
        ~Form1()
        {
            // LogOut(); Mesure de sécurité, à refaire ???
        }
        #endregion

        #region TreeView with local directories and local files
        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateLocalTreeViewWithLogicalDrives();
        }

        private void PopulateLocalTreeViewWithLogicalDrives()
        {
            TreeNode rootNode;
            List<DirectoryInfo> directories = new List<DirectoryInfo>();
            
            string[] drives = Environment.GetLogicalDrives();
            foreach (string drive in drives)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(drive);
                if (directoryInfo.Exists)
                {
                    rootNode = new TreeNode(directoryInfo.Name);
                    rootNode.Tag = directoryInfo;
                    rootNode.ImageIndex = 0;
                    rootNode.SelectedImageIndex = 0;
                    treeViewLocal.Nodes.Add(rootNode);
                    
                    directories.Add(directoryInfo);
                }
            }

            PopulateLocalListView(directories.ToArray());
        }

        private void treeViewLocal_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode nodeClicked = e.Node;

                if (IsNodeADirectory(nodeClicked))
                {
                    listViewLocal.Items.Clear();
                    if (nodeClicked.Nodes.Count == 0)
                    {
                        PropulateLocalTreeNodeWithDirectories(nodeClicked);
                        PopulateLocalTreeNodeWithFiles(nodeClicked);
                    }
                    else
                    {
                        List<FileSystemInfo> fileSystInfos = new List<FileSystemInfo>();
                        TreeNodeCollection nodes = nodeClicked.Nodes;
                        foreach (TreeNode node in nodes)
                        {
                            fileSystInfos.Add((FileSystemInfo)node.Tag);
                        }
                        PopulateLocalListView(fileSystInfos.ToArray());
                    }
                }
            }
            catch(Exception ex)
            {
                WriteLog("ERROR : "+ex.Message, Color.Red);
            }
        }

        private void PopulateLocalListView(FileSystemInfo[] files)
        {
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;
            string extension = "";
            string size = "";

            foreach (FileSystemInfo subFile in files)
            {
                item = new ListViewItem(subFile.Name, 0);
                extension = subFile.Extension;
                try
                {
                    // TODO : obtenir la size, BUG
                    size = (new FileInfo(subFile.Name)).Length.ToString();
                }
                catch
                {
                    size = "";
                }


                subItems = new ListViewItem.ListViewSubItem[]
                    {new ListViewItem.ListViewSubItem(item, size),
                    new ListViewItem.ListViewSubItem(item, extension), 
                     new ListViewItem.ListViewSubItem(item, 
						subFile.LastAccessTime.ToShortDateString())};
                if (extension.Equals("")){
                    item.ImageIndex = 1;
                }
                else {
                    item.ImageIndex = 2;
                }
                
                item.SubItems.AddRange(subItems);
                listViewLocal.Items.Add(item);
            }

            listViewLocal.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void PropulateLocalTreeNodeWithDirectories(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            List<DirectoryInfo> files = new List<DirectoryInfo>();

            foreach (DirectoryInfo subDir in nodeClickedInfo.GetDirectories())
            {
                TreeNode newDirNode = new TreeNode(subDir.Name, 0, 0);
                newDirNode.Tag = subDir;
                newDirNode.ImageIndex = 1;
                newDirNode.SelectedImageIndex = 1;
                nodeClicked.Nodes.Add(newDirNode);

                files.Add(subDir);
            }

            PopulateLocalListView(files.ToArray());
        }

        private void PopulateLocalTreeNodeWithFiles(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            List<FileInfo> files = new List<FileInfo>();

            foreach (FileInfo file in nodeClickedInfo.GetFiles())
            {
                TreeNode newFileNode = new TreeNode(file.Name, 0, 0);
                newFileNode.Tag = file;
                newFileNode.ImageIndex = 2;
                newFileNode.SelectedImageIndex = 2;
                nodeClicked.Nodes.Add(newFileNode);

                files.Add(file);
            }

            PopulateLocalListView(files.ToArray());
        }

        private bool IsNodeADirectory(TreeNode node)
        {
            bool isADirectory = false;
            if (node.ImageIndex == 0 || node.ImageIndex == 1)
            {
                isADirectory = true;
            }
            return isADirectory;
        }
        #endregion

        #region Log Window
        private void WriteLog(string text, Color color)
        {
            logWindow.SelectionColor = color;
            logWindow.AppendText(DateTime.Now+" - "+text+"\n");
        }
        #endregion

        #region Treeview with server directories and local files
        private void btnConnection_Click(object sender, EventArgs e)
        {
            GetTreeViewFromServer("/", treeViewServer.Nodes[0]);
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
                    WriteLog(ftpResponse.BannerMessage, Color.Green);
                    WriteLog(ftpResponse.WelcomeMessage, Color.Green);
                    WriteLog(ftpResponse.StatusDescription, Color.Blue);
                    WriteLog(ftpResponse.StatusCode.ToString(), Color.Blue);
                    WriteLog(WebRequestMethods.Ftp.ListDirectoryDetails, Color.Black);

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
                    WriteLog(ex.Message, Color.Red);
                    isSendingListCommand = false;
                }
                finally
                {
                    isSendingListCommand = false;
                }
            }
        }

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

        private void treeViewServer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            if (IsNodeADirectory(nodeClicked))
            {

                listViewServer.Items.Clear();
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
                // subFile.LastAccessTime.ToShortDateString()
                if (extension.Equals("Directory"))
                {
                    item.ImageIndex = 1;
                }
                else
                {
                    item.ImageIndex = 2;
                }

                item.SubItems.AddRange(subItems);
                listViewServer.Items.Add(item);
            }

            listViewServer.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        #endregion
    }
}

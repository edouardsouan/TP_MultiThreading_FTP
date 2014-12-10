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
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            txtPassword.TextBox.PasswordChar = '*';

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
                }
            }
        }

        private void treeViewLocal_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode nodeClicked = e.Node;
                if (IsNodeADirectory(nodeClicked))
                {
                    if (nodeClicked.Nodes.Count == 0)
                    {
                        PropulateLocalTreeNodeWithDirectories(nodeClicked);
                        PopulateLocalTreeNodeWithFiles(nodeClicked);
                    }
                }
            }
            catch(Exception ex)
            {
                WriteLog("ERROR : "+ex.Message, Color.Red);
            }
        }

        private void PropulateLocalTreeNodeWithDirectories(TreeNode nodeClicked)
        {
            
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            foreach (DirectoryInfo subDir in nodeClickedInfo.GetDirectories())
            {
                TreeNode newDirNode = new TreeNode(subDir.Name, 0, 0);
                newDirNode.Tag = subDir;
                newDirNode.ImageIndex = 1;
                newDirNode.SelectedImageIndex = 1;
                nodeClicked.Nodes.Add(newDirNode);
            }
        }

        private void PopulateLocalTreeNodeWithFiles(TreeNode nodeClicked)
        {
            DirectoryInfo nodeClickedInfo = (DirectoryInfo)nodeClicked.Tag;
            foreach (FileInfo file in nodeClickedInfo.GetFiles())
            {
                TreeNode newFileNode = new TreeNode(file.Name, 0, 0);
                newFileNode.Tag = file;
                newFileNode.ImageIndex = 2;
                newFileNode.SelectedImageIndex = 2;
                nodeClicked.Nodes.Add(newFileNode);
            }

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

        private void btnConnection_Click(object sender, EventArgs e)
        {
            GetTreeViewFromServer("/", treeViewServer.Nodes[0]);
        }

        private async void GetTreeViewFromServer(string serverPath, TreeNode parentNode)
        {
            string serverTarget = "ftp://" + this.txtServer.Text + serverPath;
            Console.WriteLine(serverTarget);

            try
            {
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(serverTarget);
                ftpRequest.KeepAlive = false;
                ftpRequest.Credentials = new NetworkCredential( this.txtUserName.Text, this.txtPassword.Text);
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
            }
        }

        private void BuildServerTreeView(string[] directories, TreeNode parentNode)
        {
            string directory;
            string fileType;
            int startIndex;
            string fileName;

            foreach (string rawDirectory in directories)
            {
                directory = rawDirectory.Remove(rawDirectory.LastIndexOf("\r"), 1);
                fileType = directory.Substring(0,1);
                startIndex = directory.LastIndexOf(" ");
                fileName = directory.Substring(startIndex);

                AddNodeServerTreeView(fileName, fileType, parentNode);
            }
        }

        private void AddNodeServerTreeView(string name, string fileType, TreeNode parentNode)
        {
            TreeNode serverNode = new TreeNode(name);
            serverNode.Tag = name;

            if (fileType.Equals("d"))
            {
                serverNode.ImageIndex = 1;
                serverNode.SelectedImageIndex = 1;
            }
            else
            {
                serverNode.ImageIndex = 2;
                serverNode.SelectedImageIndex = 2;
            }

            treeViewServer.Nodes.Add(serverNode);
        }

        private void treeViewServer_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode nodeClicked = e.Node;
            if (IsNodeADirectory(nodeClicked))
            {
                if (nodeClicked.Nodes.Count == 0)
                {
                    GetTreeViewFromServer(nodeClicked.FullPath, nodeClicked);
                }
            }
        }
    }
}

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
                    treeViewLocalFiles.Nodes.Add(rootNode);
                }
            }
        }

        private void treeViewLocalFiles_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
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
            Task<string> asyncResult = GetAsyncResponse();
           
        }

        private async Task<string> GetAsyncResponse()
        {
            string serverTarget = "ftp://" + this.txtServer.Text;
            string userName = this.txtUserName.Text;
            string userPassword = this.txtPassword.Text;
            string result = "";

            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverTarget);
                request.KeepAlive = false;
                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(userName, userPassword);

                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                //FtpWebResponse response = (FtpWebResponse)request.GetResponseAsync();
                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();
                WriteLog(response.BannerMessage, Color.Green);
                WriteLog(response.WelcomeMessage, Color.Green);
                WriteLog(response.StatusDescription, Color.Blue);
                WriteLog(response.StatusCode.ToString(), Color.Blue);
                WriteLog(WebRequestMethods.Ftp.ListDirectoryDetails, Color.Black);

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                result = response.StatusDescription;

                reader.Close();
                response.Close();
            }
            catch (WebException ex)
            {
                WriteLog(ex.Message, Color.Red);
            }
            
            return result;
        }       
    }
}
